using System;
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
using Metreos.AxlSoap333;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333.Actions.ListUserByName;

namespace Metreos.Native.AxlSoap333
{
	/// <summary> Wraps up the 'listUserByName' AXL SOAP method for Cisco CallManager 3.3.3 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.PACKAGE_DESCRIPTION)]
    public class ListUserByName : INativeAction
	{     
        [ActionParamField(Package.Params.FirstName.DISPLAY, Package.Params.FirstName.DESCRIPTION, false, Package.Params.FirstName.DEFAULT)]
        public string FirstName { set { firstName = value; } }

        [ActionParamField(Package.Params.LastName.DISPLAY, Package.Params.LastName.DESCRIPTION, false, Package.Params.LastName.DEFAULT)]
        public string LastName { set { lastName = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.ListUserByNameResponse.DISPLAY, Package.Results.ListUserByNameResponse.DESCRIPTION)]
        public listUserByNameResponse ListUserByNameResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private string firstName;
        private string lastName;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        private listUserByNameResponse response;

		public ListUserByName()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.firstName                  = null;
            this.lastName                   = null;
            this.username                   = null;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new listUserByNameResponse();
            this.message                    = String.Empty;
            this.code                       = 0;
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
            
            listUserByName request = new listUserByName();
            request.firstname = firstName;
            request.lastname = lastName;
             
            try
            {
                response = client.listUserByName(request);
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
