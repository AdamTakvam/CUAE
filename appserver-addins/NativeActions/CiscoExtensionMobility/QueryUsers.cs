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

using Package = Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Actions.QueryUsers;

namespace Metreos.Native.CiscoExtensionMobility
{
    /// <summary> 
    ///     Query a set of users, returning which devices are associated with those users, using the Cisco 
    ///     Extension Mobility API, which requires a Proxy Authorized User to do the query
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.PACKAGE_DESCRIPTION)]
    public class QueryUsers : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.AppId.DISPLAY, Package.Params.AppId.DESCRIPTION, true, Package.Params.AppId.DEFAULT)]
        public string AppId { set { appId = value; } }
        private string appId;

        [ActionParamField(Package.Params.AppCertificate.DISPLAY, Package.Params.AppCertificate.DESCRIPTION, true, Package.Params.AppCertificate.DEFAULT)]
        public string AppCertificate { set { certificate = value; } }
        private string certificate;

        [ActionParamField("The ID(s) of the user(s)", true)]
        public string[] Users { set { users = value; } }
        private string[] users;

        [ActionParamField(Package.Params.Url.DISPLAY, Package.Params.Url.DESCRIPTION, false, Package.Params.Url.DEFAULT)]
        public string Url { set { url = value; } }
        private string url;

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, false, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { ccmIp = value; } }
        private string ccmIp;

        [ActionParamField(Package.Params.Version.DISPLAY, Package.Params.Version.DESCRIPTION, false, Package.Params.Version.DEFAULT)]
        public CcmVersion Version { set { ccmVersion = value; } }
        private CcmVersion ccmVersion;

        [ResultDataField(Package.Results.QueryUsersResult.DISPLAY, Package.Results.QueryUsersResult.DESCRIPTION)]
        public DeviceResponse QueryUsersResult { get { return deviceResponse; } set { deviceResponse = value ; } }
        private DeviceResponse deviceResponse;

        [ResultDataField("The descriptive message returned by CallManager.  Can be null even if CallManager " + 
             "was contacted")]
        public string ErrorMessage { get { return errorMessage ; } set { errorMessage = value ; } }
        private string errorMessage;

        public QueryUsers() { }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            ccmVersion      = CcmVersion.Undefined;
            appId           = null;
            certificate     = null;         
            users           = null;
            ccmIp           = null;
            url             = null;
            errorMessage    = String.Empty; 
            deviceResponse  = new DeviceResponse();
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            DeviceRequest request = new DeviceRequest();
            request.appInfo = new MobilityRequest.ApplicationInfo();
            request.appInfo.id = appId;
            request.appInfo.certificate = certificate;
            request.usersQueryData = new DeviceRequest.UsersQueryData();
            request.usersQueryData.userId = users;

            //Determine url
            if(ccmIp != null)
            {
                if(ccmVersion == CcmVersion.v333)
                {
                    url = "http://" + ccmIp + "/LoginService/query.asp";
                }
                else
                {
                    url = "http://" + ccmIp + "/emservice/EMServiceServlet";
                }
            }

            object responseUncasted;
            UrlStatus status = Web.CiscoPostXmlTransaction(url, request, typeof(DeviceResponse), out responseUncasted);

            DeviceResponse response = responseUncasted as DeviceResponse;

            if(status != UrlStatus.Success)
            {
                log.Write(TraceLevel.Error, "Unable to communicate with the Extension Mobility service");
                return IApp.VALUE_FAILURE;
            }

            if(response.IsSuccess)  
            {
                deviceResponse = response;
                return IApp.VALUE_SUCCESS;
            }
            else
            {
                try
                {
                    if(response.failure.errorMessageAlt != null)
                    {
                        errorMessage = response.failure.errorMessageAlt;
                    }
                    else
                    {
                        errorMessage = response.failure.errorMessage;     
                    }
                }
                catch { errorMessage = "No error received by CallManager"; }
                return IApp.VALUE_FAILURE;
            }
        }
    }
}
