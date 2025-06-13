using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobilityTypes.Types.QueryUserResults;

namespace Metreos.Types.CiscoExtensionMobility
{
    /// <summary>
    /// The native implementation of the results returned by a Cisco Extension Mobility User-DeviceQuery
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class QueryUserResults : IVariable
    {
        public enum UserDeviceUsageStatus
        {
            failure,
            NoDevice,
            NoUser,
            HasDevices
        }

        public enum UserLoggedIntoDeviceStatus
        {
            @true,
            @false,
        }

        /// <summary>
        ///     Given a username and devicename, this will check the results of a QueryUser command 
        ///     and determine if that username has that devicename in his list of controlled devices
        /// </summary>
        /// <returns>A logged in status for the user</returns>
        [TypeMethod(Package.CustomMethods.IsDeviceControlledByUser_String.DESCRIPTION)]        
        public UserLoggedIntoDeviceStatus IsDeviceControlledByUser(string userName, string deviceName)
        {
            if(!results.IsSuccess)
            {
                return UserLoggedIntoDeviceStatus.@false;
            }

            if(results.results == null || results.results.users == null)
            {
                return UserLoggedIntoDeviceStatus.@false;
            }

            DeviceResponse.DeviceResults.User[] users = results.results.users;

            foreach(DeviceResponse.DeviceResults.User user in users)
            {
                if(user.userId == userName)
                {
                    // No user found logged in
                    if(user.noneFlag == String.Empty)
                    {
                        return UserLoggedIntoDeviceStatus.@false;
                    }   
                    else if(user.doesNotExistFlag == String.Empty)
                    {
                        return UserLoggedIntoDeviceStatus.@false;
                    }
                    else
                    {
                        if(user.deviceNames != null)
                        {
                            foreach(string loggedInDevice in user.deviceNames)
                            {
                                if(0 == String.Compare(loggedInDevice, deviceName, true))
                                {
                                    return UserLoggedIntoDeviceStatus.@true;
                                }
                            }

                            break;
                        }
                        else
                        {
                            return UserLoggedIntoDeviceStatus.@false;
                        }

                    }
                }
            }

            return UserLoggedIntoDeviceStatus.@false;
        }

        /// <summary>
        ///     Given a username, will return a status based on the information found for that 
        ///     user.
        /// </summary>
        /// <param name="deviceName"> The SEP000.... device name to search for </param>
        /// <param name="loggedInName"> The name found logged in to the device, if found </param>
        /// <returns> A logged in status for the user </returns>
        [TypeMethod(Package.CustomMethods.GetUserDevices_String.DESCRIPTION)]        
        public UserDeviceUsageStatus GetUserDevices(string userName, out string[] loggedInDevices)
        {
            loggedInDevices = new string[0];
            
            if(!results.IsSuccess)
            {
                return UserDeviceUsageStatus.failure;
            }

            if(results.results == null || results.results.users == null)
            {
                return UserDeviceUsageStatus.NoUser;
            }

            DeviceResponse.DeviceResults.User[] users = results.results.users;

            foreach(DeviceResponse.DeviceResults.User user in users)
            {
                if(user.userId == userName)
                {
                    // No user found logged in
                    if(user.noneFlag == String.Empty)
                    {
                        return UserDeviceUsageStatus.NoDevice;
                    }   
                    else if(user.doesNotExistFlag == String.Empty)
                    {
                        return UserDeviceUsageStatus.NoUser;
                    }
                    else
                    {
                        loggedInDevices = user.deviceNames;
                        return UserDeviceUsageStatus.HasDevices;
                    }
                }
            }

            return UserDeviceUsageStatus.NoUser;
        }

        // Value is a Hashtable of ArrayLists
        // Element name (string) -> Element XML (array of XML serializeable objects)
        private DeviceResponse results;
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="QueryUserResults"/> class. </para>
        /// </summary>
        public QueryUserResults()
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
            if(obj is DeviceResponse)
            {
                results = obj as DeviceResponse;
            }
            
            return true;
        }

        /// <summary>
        /// Removes all directory item and menu item information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            results = new DeviceResponse();
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
            XmlSerializer serializer = new XmlSerializer(typeof(DeviceResponse));
            serializer.Serialize(writer, results);
            writer.Close();

            return sb.ToString();
        }
    }
}
