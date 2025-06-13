using System;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;
using Metreos.Types.CiscoExtensionMobility;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Actions.Login;

namespace Metreos.Native.CiscoExtensionMobility
{
    public enum CcmVersion 
    {
        Undefined    = 0,
        v333         = 1,
        other        = 2
    }

    /// <summary> 
    ///     Logins in a 'mobile user' to CallManager to a given device using the Cisco 
    ///     Extension Mobility API, which requires a Proxy Authorized User to do the login
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.PACKAGE_DESCRIPTION)]
    public class Login : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }

        [ActionParamField(Package.Params.AppId.DISPLAY, Package.Params.AppId.DESCRIPTION, true, Package.Params.AppId.DEFAULT)]
        public string AppId { set { appId = value; } }
        private string appId;

        [ActionParamField(Package.Params.AppCertificate.DISPLAY, Package.Params.AppCertificate.DESCRIPTION, true, Package.Params.AppCertificate.DEFAULT)]
        public string AppCertificate { set { certificate = value; } }
        private string certificate;

        [ActionParamField(Package.Params.UserId.DISPLAY, Package.Params.UserId.DESCRIPTION, true, Package.Params.UserId.DEFAULT)]
        public string UserId { set { userId = value; } }
        private string userId;

        [ActionParamField(Package.Params.DeviceName.DISPLAY, Package.Params.DeviceName.DESCRIPTION, true, Package.Params.DeviceName.DEFAULT)]
        public string DeviceName { set { deviceName = value; } }
        private string deviceName;

        [ActionParamField(Package.Params.DeviceProfile.DISPLAY, Package.Params.DeviceProfile.DESCRIPTION, false, Package.Params.DeviceProfile.DEFAULT)]
        public string DeviceProfile { set { deviceProfile = value; } }
        private string deviceProfile;

        [ActionParamField(Package.Params.Timeout.DISPLAY, Package.Params.Timeout.DESCRIPTION, false, Package.Params.Timeout.DEFAULT)]
        public int Timeout { set { timeout = value; } }
        private int timeout;

        [ActionParamField(Package.Params.NoTimeout.DISPLAY, Package.Params.NoTimeout.DESCRIPTION, false, Package.Params.NoTimeout.DEFAULT)]
        public bool NoTimeout { set { noTimeout = value; } }
        private bool noTimeout;
        
        [ActionParamField(Package.Params.Url.DISPLAY, Package.Params.Url.DESCRIPTION, false, Package.Params.Url.DEFAULT)]
        public string Url { set { url = value; } }
        private string url;

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { ccmIp = value; } }
        private string ccmIp;

        [ActionParamField(Package.Params.Version.DISPLAY, Package.Params.Version.DESCRIPTION, false, Package.Params.Version.DEFAULT)]
        public CcmVersion Version { set { ccmVersion = value; } }
        private CcmVersion ccmVersion;

        [ResultDataField("The Cisco-specific error code if this action fails  " + 
             "If 0 is returned on failure, then the action was never able to communicate with CallManager")]
        public int ErrorCode { get { return errorCode; } set { errorCode = value; } } 
        private int errorCode;

        [ResultDataField("A Cisco-specific error message returned by the Extension Mobility service.  If it is null, " + 
             " then the action never communicated with CallManager")]
        public string ErrorMessage { get { return errorMessage ; } set { errorMessage = value ; } }
        private string errorMessage;

        public Login() { Clear(); }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            ccmVersion      = CcmVersion.Undefined;
            appId           = null;
            certificate     = null;
            userId          = null;
            deviceName      = null;
            deviceProfile   = null;
            ccmIp           = null;
            url             = null;
            timeout         = 0;
            noTimeout       = false;
            errorCode       = 0;
            errorMessage    = String.Empty;
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            if(noTimeout == true && timeout != 0)
            {
                log.Write(TraceLevel.Error, 
                    "Unable to specify no timeout in combination with a minute timeout");
                return IApp.VALUE_FAILURE;
            }
            else if(timeout < 0)
            {
                log.Write(TraceLevel.Error, "No timeout specified");
                return IApp.VALUE_FAILURE;
            }

            LoginRequest request = new LoginRequest();
            request.appInfo = new MobilityRequest.ApplicationInfo();
            request.appInfo.id = appId;
            request.appInfo.certificate = certificate;
            request.loginData = new LoginRequest.LoginData();
            request.loginData.deviceName = deviceName;
            request.loginData.deviceProfile = deviceProfile;
            request.loginData.userId = userId;
            request.loginData.exclusiveDuration = new LoginRequest.LoginData.Duration();
            if(noTimeout)   request.loginData.exclusiveDuration.indefinite = String.Empty;
            else            request.loginData.exclusiveDuration.time = timeout.ToString();

            //Determine url
            if(ccmIp != null)
            {
                if(ccmVersion == CcmVersion.v333)
                {
                    url = "http://" + ccmIp + "/LoginService/login.asp";
                }
                else
                {
                    url = "http://" + ccmIp + "/emservice/EMServiceServlet";
                }
            }

            object responseUncasted;
            UrlStatus status = Web.CiscoPostXmlTransaction(url, request, typeof(LoginLogoutResponse), out responseUncasted);

            LoginLogoutResponse response = responseUncasted as LoginLogoutResponse;

            if(status != UrlStatus.Success)
            {
                log.Write(TraceLevel.Error, "Unable to communicate with the Extension Mobility service");
                return IApp.VALUE_FAILURE;
            }

            if(response.IsSuccess)  
            {
                return IApp.VALUE_SUCCESS;
            }
            else
            {
                try
                {
                    errorMessage = response.failure.error.errorDescription;
                    errorCode = int.Parse(response.failure.error.code);
                }
                catch { }
                return IApp.VALUE_FAILURE;
            }
        }
    }
}
