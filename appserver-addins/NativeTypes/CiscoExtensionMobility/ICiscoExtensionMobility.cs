using System;
using System.Xml;
using System.Xml.Serialization;

namespace Metreos.Types.CiscoExtensionMobility
{
    #region Requests

    /// <summary> 
    ///     Defines common base of requests for the Extension Mobility API
    /// </summary>
    [Serializable]
    [XmlRoot("request")]
    public abstract class MobilityRequest
    {
        public MobilityRequest() { }

        [XmlElement("appInfo")]
        public ApplicationInfo appInfo;

        [Serializable]
            public class ApplicationInfo
        {
            public ApplicationInfo() { }
        
            /// <summary>
            ///     The UserId of the user set up in CallManager given Mobility Proxy Authorization
            /// </summary>
            [XmlElement("appID")]
            public string id;

            /// <summary>
            ///     The certificate
            /// </summary>
            [XmlElement("appCertificate")]
            public string certificate;
        }
    }

    /// <summary>
    ///     Defines a login request
    /// </summary>
    [Serializable]
    [XmlRoot("request")]
    public class LoginRequest : MobilityRequest
    {
        public LoginRequest() : base() { }

        [XmlElement("login")]
        public LoginData loginData;

        [Serializable]
        public class LoginData
        {
            /// <summary>
            ///     The device to log in
            /// </summary>
            /// <remarks>
            ///     Usually of the form "SEP" + MAC
            /// </remarks>
            [XmlElement("deviceName")]
            public string deviceName;

            /// <summary>
            ///     Login this user in on the device.  UserId as defined by Cisco CallManager LDAP
            /// </summary>
            [XmlElement("userID")]
            public string userId;

            /// <summary>
            ///     The profile to use when login completes.  Can be null
            /// </summary>
            [XmlElement("deviceProfile")]
            public string deviceProfile;

            /// <summary>
            ///     Defines how long
            /// </summary>
            [XmlElement("exclusiveDuration")]
            public Duration exclusiveDuration;

            [Serializable]
            public class Duration
            {
                public Duration() { }

                /// <summary>
                ///     Minutes to expire lease.  Set indefinite to null if using this (which is the default)
                /// </summary>
                [XmlElement("time")]
                public string time;

                /// <summary>
                ///     Set to String.Empty if you want this to take effect.  
                ///     You must set minutes to null if you use this. (which is the default)
                /// </summary>
                [XmlElement("indefinite")]
                public string indefinite;
            }
        }
    }

    /// <summary>
    ///     Defines a logout request
    /// </summary>
    [Serializable]
    [XmlRoot("request")]
    public class LogoutRequest : MobilityRequest
    {
        public LogoutRequest() : base() { }

        [XmlElement("logout")]
        public LogoutData logoutData;

        [Serializable]
        public class LogoutData
        {
            /// <summary>
            ///     The device to log out
            /// </summary>
            /// <remarks>
            ///     Usually of the form "SEP" + MAC
            /// </remarks>
            [XmlElement("deviceName")]
            public string deviceName;
        }
    }

    /// <summary>
    ///     Defines a user request
    /// </summary>
    [Serializable]
    [XmlRoot("query")]
    public class UserRequest : MobilityRequest
    {
        public UserRequest() : base() { }

        [XmlElement("deviceUserQuery")]
        public DevicesQueryData devicesQueryData;

        [Serializable]
        public class DevicesQueryData
        {
            public DevicesQueryData() { }

            /// <summary>
            ///     The key or keys to search users on
            /// </summary>
            [XmlElement("deviceName")]
            public string[] deviceName;
        }
    }

    /// <summary>
    ///     Defines a device request
    /// </summary>
    [Serializable]
    [XmlRoot("query")]
    public class DeviceRequest : MobilityRequest
    {
        public DeviceRequest() : base() { }

        [XmlElement("userDevicesQuery")]
        public UsersQueryData usersQueryData;

        [Serializable]
        public class UsersQueryData
        {
            public UsersQueryData() { }

            /// <summary>
            ///     The key or keys to search devices on
            /// </summary>
            [XmlElement("userID")]
            public string[] userId;
        }
    }

    #endregion Requests

    #region Responses

    /// <summary>
    ///     Defines a login or logout response
    /// </summary>
    [Serializable]
    [XmlRoot("response")]
    public class LoginLogoutResponse
    {
        public LoginLogoutResponse() { }

        public bool IsSuccess { get { return success == String.Empty; } }

        /// <summary>
        ///     Will be equal to <c>String.Empty</c> if true, <c>null</c> if false
        /// </summary>
        [XmlElement("success")]
        public string success;

        [XmlElement("failure")]
        public Failure failure;

        /// <summary>
        ///     Defines failure response structure
        /// </summary>
        [Serializable]
        public class Failure
        {
            public Failure() { }

            [XmlElement("error")]
            public Error error;
        }

        /// <summary>
        ///     Defines error code structure
        /// </summary>
        [Serializable]
        public class Error
        {
            public Error() { }

            /// <summary>
            ///     Specifies numerical error code
            /// </summary>
            [XmlAttribute("code")]
            public string code;

            /// <summary>
            ///     A friendly error
            /// </summary>
            [XmlText]
            public string errorDescription;
        }
    }

    /// <summary>
    ///     Defines the commonalities between the UserQuery and DeviceQuery
    /// </summary>
    public abstract class QueryResponse
    {
        public QueryResponse() { }

        public bool IsSuccess { get { return failure == null; } }

        [XmlElement("failure")]
        public Failure failure;

        /// <summary>
        ///     Defines failure response structure
        /// </summary>
        [Serializable]
        public class Failure
        {
            public Failure() { }

            [XmlElement("errorMessage")]
            public string errorMessage;

            [XmlElement("error")]
            public string errorMessageAlt;
        }
    }

    /// <summary>
    ///     Defines the device results that belong to user(s) searched on
    /// </summary>
    [Serializable]
    [XmlRoot("response")]
    public class DeviceResponse : QueryResponse
    {
        public DeviceResponse() : base() { } 

        [XmlElement("userDevicesResults")]
        public DeviceResults results;

        [Serializable]
        public class DeviceResults
        {
            public DeviceResults() { }

            /// <summary>
            ///     An array of users, and their devices that belong to them,
            ///     that correspond to the users searched on
            /// </summary>
            [XmlElement("user")]
            public User[] users;

            [Serializable]
            [XmlRoot("user")]
            public class User
            {
                public User() { }

                /// <summary>
                ///     The user ID
                /// </summary>
                [XmlAttributeAttribute("id")]
                public string userId;

//                /// <summary>
//                ///     The device(s) that belong to this user
//                /// </summary>
//                public DeviceName[] deviceNames;

                [XmlElement("deviceName")]
                public string[] deviceNames;
                /// <summary>
                ///     A flag indicating that this user has no devices
                /// </summary>
                [XmlElement("none")]
                public string noneFlag;
                
                /// <summary>
                ///     A flag indicating that this user does not exist
                /// </summary>
                [XmlElement("doesNotExist")]
                public string doesNotExistFlag;

                [Serializable]
                public class DeviceName
                {
                    public DeviceName() { } 

                    [XmlText()]
                    public string Value;
                }
            }
        }
    }

    /// <summary>
    ///     Defines the users that belong to device(s) searched on
    /// </summar>y
    [Serializable]
    [XmlRoot("response")]
    public class UserResponse : QueryResponse
    {
        public UserResponse() : base() {}

        [XmlElement("deviceUserResults")]
        public UserResults results;

        [Serializable]
        public class UserResults
        {
            public UserResults() { }

            /// <summary>
            ///    An array of devices, and the users that own them,
            ///     that correspond to the devices searched on
            /// </summary>
            [XmlElement("device")]
            public Device[] devices;

            [Serializable]
            public class Device
            {
                public Device() { }

                /// <summary>
                ///     The device name
                /// </summary>
                [XmlAttribute("name")]
                public string deviceName;

                /// <summary>
                ///     The user that owns this device
                /// </summary>
                [XmlElement("userID")]
                public string userId;

                /// <summary>
                ///     A flag indicating that no user owns this 
                /// </summary>
                [XmlElement("none")]
                public string noneFlag;

                /// <summary>
                ///     A flag indicating that this device does not exist
                /// </summary>
                [XmlElement("doesNotExist")]
                public string doesNotExistFlag;
            }
        }
    }

    #endregion Responses
}
