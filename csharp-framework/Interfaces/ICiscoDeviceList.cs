using System;
using System.IO;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace Metreos.Interfaces
{
	public abstract class ICiscoDeviceList
	{
        // Responses
        public const string RESP_NO_CONFIG      = "NotConfigured";
        public const string RESP_NOT_FOUND      = "NotFound";
        public const string RESP_NO_CRIT        = "NoCriteria";
        public const string RESP_DB_ERROR       = "DatabaseError";

        // Fields
        public const string TABLE_DEVICE_INFO   = "device_info";
        public const string FIELD_TYPE          = "Type";
        public const string FIELD_NAME          = "Name";
        public const string FIELD_DESCR         = "Description";
        public const string FIELD_SPACE         = "SearchSpace";
        public const string FIELD_POOL          = "Pool";
        public const string FIELD_IP            = "IP";
        public const string FIELD_STATUS        = "Status";
        public const string FIELD_CCMIP         = "CCMIP";

        // Definitions
        public const string DB_NAME             = "CiscoDeviceListX";
        public const string USERNAME            = "Administrator";

        // Field values
        public const string STATUS_NOT_FOUND                = "0";
        public const string STATUS_REGISTERED               = "1";
        public const string STATUS_FOUND_AND_UNREGISTERED   = "2";
        public const string STATUS_NONE                     = ""; // empty string


        #region Device Types
        [Flags()]
        public enum DeviceTypes
        {
            // enum used by Max to display choices, values do not correspond to DLX values
            None        = 0,
            Cisco7960   = 1,
            Cisco7940   = 2,
            Cisco7970   = 4,
            Cisco7965   = 8,
            Cisco7920   = 16,
            Cisco7905   = 32,
            Cisco7912   = 64,
            IPCommunicator = 128,
            Cisco7941   = 256,
            Cisco7941G  = 512,
            Cisco7961G  = 1024,
            Cisco7971   = 2048,
        }

        public static string ConvertToDeviceListXDeviceType(DeviceTypes deviceType)
        {
            switch(deviceType)
            {
                case DeviceTypes.Cisco7905:
                    return "20003";

                case DeviceTypes.Cisco7912:
                    return "30022";

                case DeviceTypes.Cisco7920:
                    return "30002";

                case DeviceTypes.Cisco7940:
                    return "36";

                case DeviceTypes.Cisco7960:
                    return "35";

                case DeviceTypes.Cisco7965:
                    return "30044";

                case DeviceTypes.Cisco7970:
                    return "30018";

                case DeviceTypes.IPCommunicator:
                    return "30041";

                case DeviceTypes.Cisco7941:
                    return "115";

                case DeviceTypes.Cisco7941G:
                    return "208";

                case DeviceTypes.Cisco7961G:
                    return "207";
                
                case DeviceTypes.Cisco7971:
                    return "119";

                default:
                    return null;
            }
        }
        #endregion

        #region Status Codes

        // enum used by Max to display choices, values do not correspond to DLX values
        // see STATUS_* constants above for that.
        [Flags()]
        public enum StatusCodes 
        {
            None                    = 0,
            NotFound                = 1,
            Registered              = 2,
            FoundAndUnregistered    = 4
        }

        public static string ConvertToDeviceListXStatusCode(StatusCodes code)
        {
            switch(code)
            {
                case StatusCodes.NotFound:
                    return STATUS_NOT_FOUND;

                case StatusCodes.Registered:
                    return STATUS_REGISTERED;

                case StatusCodes.FoundAndUnregistered:
                    return STATUS_FOUND_AND_UNREGISTERED;

                default:
                    return null;
            }
        }
        #endregion

        #region Strongly Typed CDLX Information
   
        /// <summary>
        ///     Encapsulates a database record defining a DLX device.
        /// </summary>
        [Serializable] public class Device
        {
            public string IP { get { return ip; } }
            public string Name { get { return name; } }
            public string Type { get { return type; } }
            public string Status { get { return status; } }
            public string Pool { get { return pool; } }
            public string SearchSpace { get { return searchSpace; } }
            public string CallManagerIP { get { return callManagerIP; } }

            private string ip;
            private string name;
            private string type;
            private string status;
            private string pool;
            private string searchSpace;
            private string callManagerIP;

            public Device() { }

            public Device(DataRow row)
            {
                if(row != null)
                {
                    ip              = row[FIELD_IP] as string;
                    name            = row[FIELD_NAME] as string;
                    type            = row[FIELD_TYPE] as string;
                    status          = row[FIELD_STATUS] as string;
                    pool            = row[FIELD_POOL] as string;
                    searchSpace     = row[FIELD_STATUS] as string;
                    callManagerIP   = row[FIELD_CCMIP] as string;
                }
            }

            public Device(string ip, string name, string type, string status, string pool, string searchSpace, string callManagerIP)
            {
                this.ip             = ip;
                this.name           = name;
                this.type           = type;
                this.status         = status;
                this.pool           = pool;
                this.searchSpace    = searchSpace;
                this.callManagerIP  = callManagerIP;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode ();
            }

            public override bool Equals(object obj)
            {
                bool isEqual = false;
                Device device = obj as Device;
                
                if(device != null)
                {
                    string thisID   = Device.FormatID(this.Name, this.CallManagerIP);
                    string deviceID = Device.FormatID(device.Name, device.CallManagerIP);
                    isEqual = String.Compare(thisID, deviceID, true) == 0;   
                }
                return isEqual;
            }

            /// <returns>SEP000011112222@10.1.10.1 => IP=10.1.10.2 | Status=2 | Type=35 | CSS=Unrestricted | Pool=Default</returns>
            public override string ToString()
            {
                return String.Format("{0} => IP={1} | Status={2} | Type={3} | CSS={4} | POOL={5}", 
                    FormatID(this.Name, this.CallManagerIP),
                    this.IP,
                    this.Status,
                    this.Type,
                    this.SearchSpace,
                    this.Pool);
            }

            public static string FormatID(string deviceName, string ccmIP)
            {
                return deviceName + '@' + ccmIP;
            }
        }

        public class DeviceList : CollectionBase, IList, ICollection
        {
            public DeviceList() : base () { }
            
            public DeviceList(DataTable data) : base() 
            {
                if(data != null)
                {
                    foreach(DataRow row in data.Rows)
                    {
                        this.Add(new Device(row));
                    }
                }
            }

            public void Add(Device device)
            {
                InnerList.Add(device);
            }

            #region ICollection Members

            public bool IsSynchronized { get { return false; } }
            public object SyncRoot { get { return InnerList.SyncRoot; } }

            public void CopyTo(Array array, int index)
            {
                InnerList.CopyTo(array, index);
            }

            #endregion

            #region IList Members

            public bool IsReadOnly { get { return false; } }

            public object this[int index] 
            { 
                get { return InnerList[index]; }
                set { InnerList[index] = value; }
            }

            public void Insert(int index, object value)
            {
                InnerList.Insert(index, value);
            }

            public void Remove(object value)
            {
                InnerList.Remove(value);
            }

            public bool Contains(object value)
            {
                return InnerList.Contains(value);
            }


            public int IndexOf(object value)
            {
                return InnerList.IndexOf(value);
            }

            public int Add(object value)
            {
                // TODO:  Add DeviceList.Add implementation
                return 0;
            }

            public bool IsFixedSize
            {
                get
                {
                    // TODO:  Add DeviceList.IsFixedSize getter implementation
                    return false;
                }
            }

            #endregion
        }

        #endregion
	}
}
