using System;
using System.Text;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using InnerDevice = Metreos.Interfaces.ICiscoDeviceList.Device;
using InnerDevices = Metreos.Interfaces.ICiscoDeviceList.DeviceList;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceListTypes.Types.Devices;

namespace Metreos.Types.CiscoDeviceList
{
    /// <summary>
    ///     Encapusates the InnerDevices collection to work in an application.
    /// </summary>
    /// <remarks>
    ///     CollectionVariableBase does all the collection-specific wrapping of a 
    ///     collection.   Since that's virtually all we wanted to do, there is little work 
    ///     to do here.
    /// </remarks>
    ///     
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Devices : CollectionVariableBase
    {
        public Devices() : base( typeof(InnerDevices) )
        {
        }

        public override string ToString()
        {
            int i = 0;
            StringBuilder allDevices = new StringBuilder();
            foreach(InnerDevice device in collection)
            {
                allDevices.AppendFormat("{0} {1}\n", i, device.ToString());
            }

            return allDevices.ToString();
        }
    }
}
