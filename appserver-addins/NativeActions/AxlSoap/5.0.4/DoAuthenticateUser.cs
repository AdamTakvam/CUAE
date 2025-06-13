using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;

using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Metreos.AxlSoap;
using Metreos.AxlSoap504;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504.Actions.DoAuthenticateUser;

namespace Metreos.Native.AxlSoap504
{
	/// <summary> Wraps up the 'doAuthenticateUser' AXL SOAP method for Cisco CallManager 5.0.4 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.PACKAGE_DESCRIPTION)]
    public class DoAuthenticateUser: INativeAction
	{
        [ActionParamField(Package.Params.UserId.DISPLAY, Package.Params.UserId.DESCRIPTION, true, Package.Params.UserId.DEFAULT)]
        public string UserId { set { userId = value; } }

        [ActionParamField(Package.Params.UserPassword.DISPLAY, Package.Params.UserPassword.DESCRIPTION, false, Package.Params.UserPassword.DEFAULT)]
        public string UserPassword { set { userPassword = value; } }
      
		[ActionParamField(Package.Params.UserPin.DISPLAY, Package.Params.UserPin.DESCRIPTION, false, Package.Params.UserPin.DEFAULT)]
		public string UserPin { set { userPin = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.Authenticated.DISPLAY, Package.Results.Authenticated.DESCRIPTION)]
        public bool Authenticated { get { return authenticated; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private string userId;
		private string userPin;
		private string userPassword;
		private bool authenticated;
		private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        private doAuthenticateUserResponse response;

		public DoAuthenticateUser()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.userId		    = null;
			this.userPin		= null;
			this.userPassword	= null;
			this.authenticated  = false;
			this.username       = IAxlSoap.DefaultCcmAdmin;
            this.password       = null;
            this.callManagerIP  = null;
            this.response       = new doAuthenticateUserResponse();
            this.message        = String.Empty;
            this.code           = 0;
        }

        public bool ValidateInput()
        {
			return true;
        } 

        public enum Result
        {
            success,
            failure,
            fault,
        }

        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
            
            doAuthenticateUser request = new doAuthenticateUser();
			request.userid = userId;
			request.Item = IAxlSoap.DetermineChosenBetweenStrings(userPassword, userPin);
            request.ItemElementName = (ItemChoiceType25) IAxlSoap.DetermineChosenBetweenStringsType
				(userPassword, userPin, ItemChoiceType25.password, ItemChoiceType25.pin);
            
            try
            {
                response = client.doAuthenticateUser(request);
				if(response.@return != null)
				{
					authenticated = response.@return.userAuthenticated;
				}
				else
				{
					log.Write(TraceLevel.Error, "Received empty response from AXL in doAuthenticateUser");
					return Result.failure.ToString();
				}
            }
            catch(System.Web.Services.Protocols.SoapException e)
            {
                IAxlSoap.ReportSoapError(e, log, ref code, ref message);

                return Result.fault.ToString();
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, Metreos.Utilities.Exceptions.FormatException(e));
                return IApp.VALUE_FAILURE;
            }


            return IApp.VALUE_SUCCESS;
        }           
	}
}
