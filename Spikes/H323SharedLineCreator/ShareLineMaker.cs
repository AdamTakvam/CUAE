using System;
using System.Collections;
using Metreos.AxlSoap413;

namespace H323SharedLineCreator
{
	/// <summary> Performs manipulations of an H.323 Phone device </summary>
	public class H323PhoneManipulator
	{
        private AXLAPIService service;
        private long deviceNameStart;
        private int deviceCount;
        private string phoneIp;

		public H323PhoneManipulator(AXLAPIService service, string phoneIp)
		{
			this.service            = service;
            this.phoneIp            = phoneIp;
        }

        /// <summary>
        ///     Given a range of device SEP's, this process will iterate
        ///     through the range of devices, retrieve the primary line of each device, and share it with
        ///     the H.323 Phone device.
        /// </summary>
        /// <param name="deviceNameStart">[SEP]000011112222</param>
        /// <param name="deviceCount">How many devices to share with</param>
        /// <returns><c>true</c> if succeeded, <c>false</c> if not</returns>
        public bool ShareLineWithDeviceRange(string deviceNameStart, string deviceCount)
        {
            bool success = false;
            ArrayList lineUuids = new ArrayList(); // The UUIDs to share on the H.323 Phone line

            // Rip off SEP
            if(deviceNameStart.StartsWith("SEP"))
            {
                deviceNameStart = deviceNameStart.Substring(3);
            }

            this.deviceNameStart    = long.Parse(deviceNameStart, System.Globalization.NumberStyles.HexNumber);
            this.deviceCount        = int.Parse(deviceCount);

            // Create Device if not found

            string deviceUuid;
            if(!DeviceExists(out deviceUuid))
            {
                if(CreateDevice(out deviceUuid))
                {
                }
                else
                {
                    // Unable to create device.  
                    Console.WriteLine("Unable to create the device {0}", phoneIp);
                }
            }

            // Ok, we have our device to add shared lines too.
            if(deviceUuid != null)
            {
                // Gather up all sharedlines to share on the H.323 Phone Device
                for(long i = 0; i < this.deviceCount; i++)
                {
                    string lineUuid;
                    string deviceName = ConvertMacToSep(i + this.deviceNameStart);
                    
                    // You have to go easy on CCM
                    System.Threading.Thread.Sleep(100);

                    if(GetFirstLineUuidOfDevice(deviceName, out lineUuid))
                    {
                        lineUuids.Add(lineUuid);
                    }
                    else
                    {
                        Console.WriteLine("Could not retrieve first line for {0}", deviceName);
                    }
                }

                // Update the H.323 Phone with the shared lines
                success = AssignLinesToDevice(lineUuids);
            }

            return success;
        }

        protected string ConvertMacToSep(long deviceMac)
        {
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0');
        }

        protected bool AssignLinesToDevice(ArrayList lineUuids)
        {
            bool assigned = false;

            updateH323Phone phone = new updateH323Phone();
            phone.Item = phoneIp;
            phone.ItemElementName = ItemChoiceType40.name;
            phone.lines = new XLine[lineUuids.Count];
         
            for(int i = 0; i < lineUuids.Count; i++)
            {
                XLine shareLine = new XLine();
                shareLine.index = (i + 1).ToString(); // index of the line is 1-based
                shareLine.Item = new XNPDirectoryNumber();
                shareLine.Item.uuid = lineUuids[i] as string;
                shareLine.ringSetting = XRingSetting.UseSystemDefault; // ringSetting must be specified!
                phone.lines[i % breakup] = shareLine;
            }

            try
            {
                service.updateH323Phone(phone);
                assigned = true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to update the H323 Phone Device. {0}", e);
            }
            

            return assigned;
        }

        protected bool GetFirstLineUuidOfDevice(string deviceName, out string lineUuid)
        {
            lineUuid = null;
            bool found = false;
            getPhone request = new getPhone();
            request.Item = deviceName;
            request.ItemElementName = ItemChoiceType5.phoneName;

            try
            {
                getPhoneResponse response = service.getPhone(request);

                // Determine if there is at least one line on this phone
                if( response.@return.device.lines != null &&
                    response.@return.device.lines.Items != null && 
                    response.@return.device.lines.Items.Length > 0)
                {
                    // If there is one line, get its uuid

                    XLine primaryLine = (XLine) response.@return.device.lines.Items[0];
                    lineUuid = primaryLine.Item.uuid; 
                    found = true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return found;
        }

        /// <summary>
        ///     Determines if the phone device already exists.  Also returns the 
        ///     device 'uuid' or Cisco CallManager identifier if found.
        /// </summary>
        protected bool DeviceExists(out string deviceUuid)
        {
            deviceUuid = null;
            bool found = false;
            getH323Phone phoneRequest = new getH323Phone();

            phoneRequest.Item = phoneIp;
            phoneRequest.ItemElementName = ItemChoiceType16.name;

            try
            {
                getH323PhoneResponse response = service.getH323Phone(phoneRequest);
                deviceUuid = response.@return.phone.uuid;
                found = true;
            }
            catch(Exception e)
            {
                //Console.WriteLine(e);

                // We will assume that the phone was not found,
                // Although an exception can occur for many more reasons than this.
            }

            return found;
        }

        /// <summary>
        ///     Create an H.323 Phone with the specified devicename, or IP
        /// </summary>
        /// <returns><c>true</c> if success, otherwise <c>false</c></returns>
        protected bool CreateDevice(out string deviceUuid)
        {
            bool created = false;
            deviceUuid = null;

            addH323Phone request = new addH323Phone();
            request.phone = new XH323Phone();
            request.phone.name = phoneIp;  
            request.phone.Item3 = "Default"; // Must be specified!... leaving blank will not default to 'Default'
            
            try
            {
                addH323PhoneResponse response =  service.addH323Phone(request);

                // ! WORKAROUND
                // Unfortunately, you have to do a second update to fix an apparent bug in AXL SOAP where
                // if you specify a leading digit in the devicename, it tries to encode the digit
                // into a hexcode or something bogus.  an UpdateH323Phone command will correctly overwrite it
                updateH323Phone update = new updateH323Phone();
                update.Item = response.@return;
                update.ItemElementName = ItemChoiceType40.uuid;
                update.newName = phoneIp;

                service.updateH323Phone(update);
                // ! END WORKAROUND

                deviceUuid = response.@return;
                created = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            


            return created;
        }
	}
}
