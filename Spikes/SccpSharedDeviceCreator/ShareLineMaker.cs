using System;
using System.Collections;
using Metreos.AxlSoap413;

namespace SharedLineCreator
{
	/// <summary> Performs manipulations of a SCCP Phone device </summary>
	public class SccpPhoneManipulator
	{
        private AXLAPIService service;
        private int startSEPToShare;
        private int startSEPBaseFrom;
        private int deviceCount;
        
		public SccpPhoneManipulator(AXLAPIService service)
		{
			this.service            = service;
        }

        /// <summary>
        ///     Given a range of SCCP devices, this method will attempt to the primary line on
        ///     each device to match a second range of SCCP devices.  The two ranges are forced to 
        ///     be the same size
        /// </summary>
        /// <param name="startSEPToShare">Devices to add shared lines to in order to match baseFrom range</param>
        /// <param name="startSEPBaseFrom">Existing devices to share with</param>
        /// <param name="deviceCount">How many devices to share with</param>
        /// <returns><c>true</c> if succeeded, <c>false</c> if not</returns>
        public bool ShareLineWithDeviceRange(string startSEPToShare, string startSEPBaseFrom, string deviceCount)
        {
            bool success = true;

            // Rip off SEP
            if(startSEPToShare.StartsWith("SEP"))
            {
                startSEPToShare = startSEPToShare.Substring(3);
            }
            if(startSEPToShare.StartsWith("SEP"))
            {
                startSEPToShare = startSEPToShare.Substring(3);
            }

            this.startSEPToShare    = int.Parse(startSEPToShare, System.Globalization.NumberStyles.HexNumber);
            this.startSEPBaseFrom   = int.Parse(startSEPBaseFrom, System.Globalization.NumberStyles.HexNumber);
            this.deviceCount        = int.Parse(deviceCount);

            // Create Device if not found

            for(int i = 0 ; i < this.deviceCount; i++)
            {
                string toShareDevice = ConvertMacToSep(i + this.startSEPToShare);
                    
                string deviceUuid;
                if(!DeviceExists(toShareDevice, out deviceUuid))
                {
                    if(CreateDevice(toShareDevice, out deviceUuid))
                    {
                    }
                    else
                    {
                        // Unable to create device.  
                        Console.WriteLine("Unable to create the device {0}", toShareDevice);
                        success = false;
                    }
                }

                if(deviceUuid != null)
                {
                    string lineUuid;
                    string baseFromDevice = ConvertMacToSep(i + this.startSEPBaseFrom);
                    if(GetFirstLineUuidOfDevice(baseFromDevice, out lineUuid))
                    {
                        AssignLineToDevice(toShareDevice, lineUuid);
                    }
                }
                else
                {
                    success = false;
                    break;
                }
            }

            
            return success;
        }

        protected string ConvertMacToSep(int deviceMac)
        {
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0');
        }

        protected bool AssignLineToDevice(string deviceName, string lineUuid)
        {
            bool assigned = false;

            updatePhone phone = new updatePhone();
            phone.Item = deviceName;
            phone.ItemElementName = ItemChoiceType4.name;
            phone.lines = new UpdatePhoneReqLines();
            phone.lines.Items = new XLine[1];
            
            XLine shareLine = new XLine();
            shareLine.index = "1"; // index of the line is 1-based
            shareLine.Item = new XNPDirectoryNumber();
            shareLine.Item.uuid = lineUuid;
            shareLine.ringSetting = XRingSetting.UseSystemDefault; // ringSetting must be specified!
            phone.lines.Items[0] = shareLine;
            
            try
            {
                service.updatePhone(phone);
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
        protected bool DeviceExists(string deviceName, out string deviceUuid)
        {
            deviceUuid = null;
            bool found = false;
            getPhone phoneRequest = new getPhone();

            phoneRequest.Item = deviceName;
            phoneRequest.ItemElementName = ItemChoiceType5.phoneName;

            try
            {
                getPhoneResponse response = service.getPhone(phoneRequest);
                deviceUuid = response.@return.device.uuid;
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
        protected bool CreateDevice(string deviceName, out string deviceUuid)
        {
            bool created = false;
            deviceUuid = null;

            addH323Phone request = new addH323Phone();
            request.phone = new XH323Phone();
            request.phone.name = deviceName;  
            request.phone.Item3 = "Default"; // Must be specified!... leaving blank will not default to 'Default'
            
            try
            {
                addH323PhoneResponse response =  service.addH323Phone(request);

                // No need, won't have leading number
//                // ! WORKAROUND
//                // Unfortunately, you have to do a second update to fix an apparent bug in AXL SOAP where
//                // if you specify a leading digit in the devicename, it tries to encode the digit
//                // into a hexcode or something bogus.  an UpdateH323Phone command will correctly overwrite it
//                updatePhone update = new updatePhone();
//                update.Item = response.@return;
//                update.ItemElementName = ItemChoiceType40.uuid;
//                update.newName = phoneIp;
//
//                service.updatePhone(update);
//                // ! END WORKAROUND

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
