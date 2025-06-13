/*  In testing GetUser with 3.3.3, found this error and gave up.
 * 
 * 20:49:11.997 W script1-1 Soap Fault.

<axl:error sequence="0" xmlns:axl="http://www.cisco.com/AXL/1.0">
<code>5005</code>
<message>
<![CDATA[Unexpected element. Found <userid>, expecting <ldapRN>..]]></message>
<request>getUser</request>
</axl:error>

* 
*/

//using System;
//using System.Net;
//using System.Web;
//using System.Web.Services.Protocols;
//
//using System.Data;
//using System.Collections;
//using System.Diagnostics;
//
//using Metreos.Core;
//using Metreos.Interfaces;
//using Metreos.LoggingFramework;
//using Metreos.ApplicationFramework;
//using Metreos.PackageGeneratorCore.Attributes;
//using Metreos.ApplicationFramework.Collections;
//
//using Metreos.AxlSoap;
//using Metreos.AxlSoap333;
//
//namespace Metreos.Native.AxlSoap333
//{
//	/// <summary> Wraps up the 'getUser' AXL SOAP method for Cisco CallManager 3.3.3 </summary>
//    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.PACKAGE_DESCRIPTION)]
//    public class GetUser : INativeAction
//	{
//        [ActionParamField(Package.Params.UserId.DISPLAY, Package.Params.UserId.DESCRIPTION, false, Package.Params.UserId.DEFAULT)]
//        public string UserId { set { userId = value; } }
//
//        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
//        public string CallManagerIP { set { callManagerIP = value; } }
//
//        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
//        public string AdminUsername { set { username = value; } }
//
//        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
//        public string AdminPassword { set { password = value; } }
//
//        [ResultDataField(Package.Results.GetUserResponse.DISPLAY, Package.Results.GetUserResponse.DESCRIPTION)]
//        public getUserResponse GetUserResponse { get { return response; } }
//
//        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
//        public string FaultMessage { get { return message; } }
//
//        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
//        public int FaultCode { get { return code; } }
//
//        public LogWriter Log { set { log = value; } }
//
//        private string userId;
//        private string username;
//        private string password;
//        private string message;
//        private string callManagerIP;
//        private int code;
//        private LogWriter log;
//        private getUserResponse response;
//
//		public GetUser()
//		{
//		    Clear();	
//		}
//
//        public void Clear()
//        {
//            this.userId        = null;
//            this.username       = IAxlSoap.DefaultCcmAdmin;
//            this.password       = null;
//            this.callManagerIP  = null;
//            this.response       = new getUserResponse();
//            this.message        = String.Empty;
//            this.code           = 0;
//        }
//
//        public bool ValidateInput()
//        {
//            return true;
//        } 
//
//        public enum Result
//        {
//            success,
//            failure,
//            fault,
//        }
//
//        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
//        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
//        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
//        {
//            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
//            
//            getUser request = new getUser();
//			request.userid = userId;
//            
//            try
//            {
//                response = client.getUser(request);
//            }
//            catch(System.Web.Services.Protocols.SoapException e)
//            {
//                IAxlSoap.ReportSoapError(e, log, ref code, ref message);
//
//                return Result.fault.ToString();
//            }
//            catch(Exception e)
//            {
//                log.Write(TraceLevel.Error, Metreos.Utilities.Exceptions.FormatException(e));
//                return IApp.VALUE_FAILURE;
//            }
//
//            return IApp.VALUE_SUCCESS;
//        }               
//	}
//}
