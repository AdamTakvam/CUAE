using System;
using Metreos.AxlSoap413;

namespace DeviceDeleter
{
	public class DeviceDeleter
	{
        private AXLAPIService service;
		public DeviceDeleter(AXLAPIService service)
		{
			this.service = service;
		}

        public bool RemoveAllDevices(int maxAxlWrite, string searchString)
        {
            bool querySuccess = false;
            bool success = true;
            listDeviceByNameAndClassResponse response = null;

            listDeviceByNameAndClass list = new listDeviceByNameAndClass();
            list.@class = XClass.Phone;
            list.searchString = searchString;

            int succeeded = 0;
            try
            {
                response = service.listDeviceByNameAndClass( list );
                querySuccess = true;
            }
            catch
            {
                Console.WriteLine("Unable to query all devices");
            }
            if(querySuccess)
            {
                if(response.@return != null)
                {
                    
                    foreach(ListDeviceResDevice device in response.@return)
                    {
                        string uuid = device.uuid;

                        // First get the phone, so we can iterate through lines and delete
                        getPhone getPhoneReq = new getPhone();
                        getPhoneReq.Item  = uuid;
                        getPhoneReq.ItemElementName = ItemChoiceType5.phoneId;

                        try
                        {
                            getPhoneResponse getPhoneRes = service.getPhone(getPhoneReq);
                         
                            System.Threading.Thread.Sleep(75);

                            // Delete device

                            removePhone request = new removePhone();
                            request.Item = uuid;
                            request.ItemElementName = ItemChoiceType3.phoneId;
                            try
                            {
                                service.removePhone(request);
                                succeeded++;
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Unable to delete a device {0}. {1}", uuid, e);
                                success = false;
                            }

                            System.Threading.Thread.Sleep(1000 * 60 / maxAxlWrite + 500 );

                            if(getPhoneRes.@return.device.lines != null &&
                                getPhoneRes.@return.device.lines.Items != null)
                            {
                                foreach(XLine line in getPhoneRes.@return.device.lines.Items)
                                {
                                    // Delete each line 
                                    
                                    removeLine removeLineReq = new removeLine();
                                    removeLineReq.uuid = line.Item.uuid;
                            
                                    service.removeLine(removeLineReq);
                                    System.Threading.Thread.Sleep(1000 * 60 / maxAxlWrite + 500 );
                                 }
                                                                                 
                            }

                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("UNable to retrieve device {0}.  {1}", uuid, e);
                            success = false;
                        }
                    }
                }
            
            Console.WriteLine("Found {0} devices, deleted {1}", response.@return.Length, succeeded);

            }

            return success;
        }
	}
}
