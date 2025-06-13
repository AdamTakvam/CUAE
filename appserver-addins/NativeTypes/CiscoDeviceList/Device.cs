using System;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using InnerDevice = Metreos.Interfaces.ICiscoDeviceList.Device;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceListTypes.Types.Device;

namespace Metreos.Types.CiscoDeviceList
{
    /// <summary>
    ///     InnerDevice encapsulates a device list X record.   This class encapsulates
    ///     InnerDevice to work in an application by implementing IVariable.
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Device : IVariable
    {
        /// <summary> Inner data store </summary>
        private InnerDevice device; 

        public string Name { get { return device.Name; } }
        public string IP { get { return device.IP; } }
        public string Type { get { return device.Type; } }
        public string Status { get { return device.Status; } }
        public string Pool { get { return device.Pool; } }
        public string SearchSpace { get { return device.SearchSpace; } }

        /// <summary> Resets a device </summary>
        public Device()
        {
            Reset();
        }
        
        /// <summary> Not implemented.  A string can not parse into a complex device structure </summary>
        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string newValue)
        {
            return false;
        }

        /// <summary>
        ///     System.Data.DataTable - A datatable resulting from a DeviceListX query
        /// </summary>
        [TypeInput("System.Data.DataTable", "A datatable resulting from a DeviceListX query")]
        public bool Parse(System.Data.DataTable table)
        {
            if(table != null && table.Rows.Count > 0)
            {
                device = new InnerDevice(table.Rows[0]);
            }
            return true;
        }

        
        /// <summary>
        ///     System.Data.DataRow - A row from a datatable resulting from a DeviceListX query
        /// </summary>
        [TypeInput("System.Data.DataRow", "A row from a datatable resulting from a DeviceListX query")]
        public bool Parse(System.Data.DataRow row)
        {
            if(row != null)
            {
                device = new InnerDevice(row);
            }
            return true;
        }

        /// <summary>
        /// Removes all input item and softkey information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            device = new InnerDevice();
        }

        /// <summary> Displays all information about this device </summary>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            return device.ToString();
        }
    }
}
