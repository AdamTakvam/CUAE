using System;
using Metreos.AxlSoap413;

namespace DeviceManipulator
{
	/// <summary>
	/// Summary description for DeviceManipulator.
	/// </summary>
	public class DeviceManipulator
	{
        private AXLAPIService service;
		public DeviceManipulator(AXLAPIService service)
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
                    }
                }
            
            Console.WriteLine("Found {0} devices, deleted {1}", response.@return.Length, succeeded);

            }

            return success;
        }
	}
}
