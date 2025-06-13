using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobilityTypes.Types.QueryDeviceResults;

namespace Metreos.Types.CiscoExtensionMobility
{
    /// <summary>
    /// The native implementation of the results returned by a Cisco Extension Mobility DeviceUsersQuery
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class QueryDeviceResults : IVariable
    {
        public enum DeviceLoggedInStatus
        {
            failure,
            NoDevice,
            NoUser,
            LoggedIn
        }

        /// <summary>
        ///     Given a device name and a username, will return a status based on the information found for that 
        ///     device.
        /// </summary>
        /// <param name="deviceName"> The SEP000.... device name to search for </param>
        /// <param name="loggedInName"> The name found logged in to the device, if found </param>
        /// <returns> A logged in status for the device </returns>
        [TypeMethod(Package.CustomMethods.GetDeviceStatus_String.DESCRIPTION)]        
        public DeviceLoggedInStatus GetDeviceStatus(string deviceName, out string loggedInName)
        {
            loggedInName = String.Empty;
            
            if(!results.IsSuccess)
            {
                return DeviceLoggedInStatus.failure;
            }

            if(results.results == null || results.results.devices == null || results.results.devices.Length == 0)
            {
                return DeviceLoggedInStatus.NoDevice;
            }

            UserResponse.UserResults.Device[] devices = results.results.devices;

            foreach(UserResponse.UserResults.Device device in devices)
            {
                if(device.deviceName == deviceName)
                {
                    // No user found logged in
                    if(device.noneFlag == String.Empty)
                    {
                        return DeviceLoggedInStatus.NoUser;
                    }
                    else if(device.doesNotExistFlag == String.Empty)
                    {
                        return DeviceLoggedInStatus.NoDevice;
                    }
                    else
                    {
                        loggedInName = device.userId;
                        return DeviceLoggedInStatus.LoggedIn;
                    }
                }
            }

            return DeviceLoggedInStatus.NoDevice;
        }

        // Value is a Hashtable of ArrayLists
        // Element name (string) -> Element XML (array of XML serializeable objects)
        private UserResponse results;
        /// <summary>
        /// Initializes the Directory information and menu items to an empty set
        /// </summary>
        public QueryDeviceResults()
        {
            Reset();
        }
        
        /// <summary>
        /// Not implemented.  A simple string wouldn't convert to Cisco IP Phone XML well
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string newValue)
        { 
            return false;
        }

        [TypeInput("Object", Package.CustomMethods.Parse_Object.DESCRIPTION)]        
        public bool Parse(object obj)
        {
            if(obj is UserResponse)
            {
                results = obj as UserResponse;
            }
            
            return true;
        }

        /// <summary>
        /// Removes all directory item and menu item information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            results = new UserResponse();
        }

        /// <summary>
        /// Converts the native Directory object to Cisco IP Phone compatible XML
        /// </summary>
        /// <returns>XML representation of the Directory object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(UserResponse));
            serializer.Serialize(writer, results);
            writer.Close();

            return sb.ToString();
        }
    }
}
