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
using Metreos.AxlSoap601;
using Metreos.Types.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601.Actions.AddLineGroup;

namespace Metreos.Native.AxlSoap601
{
    /// <summary> Wraps up the 'addLineGroup' AXL SOAP method for Cisco CallManager 6.0.1 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.PACKAGE_DESCRIPTION)]
    public class AddLineGroup : INativeAction
    {  
		[ActionParamField(Package.Params.Members.DISPLAY, Package.Params.Members.DESCRIPTION, false, Package.Params.Members.DEFAULT)]
		public LineGroupMembers Members { set { members = value; } }

        [ActionParamField(Package.Params.DistributeAlgorithm.DISPLAY, Package.Params.DistributeAlgorithm.DESCRIPTION, false, Package.Params.DistributeAlgorithm.DEFAULT)]
        public string DistributeAlgorithm { set { distributeAlgorithm = value; /*distributeAlgorithmSpecified = true; */} }

		[ActionParamField(Package.Params.HuntAlgorithmBusy.DISPLAY, Package.Params.HuntAlgorithmBusy.DESCRIPTION, false, Package.Params.HuntAlgorithmBusy.DEFAULT)]
		public string HuntAlgorithmBusy { set { huntAlgorithmBusy = value; /*huntAlgorithmBusySpecified = true; */} }

		[ActionParamField(Package.Params.HuntAlgorithmNoAnswer.DISPLAY, Package.Params.HuntAlgorithmNoAnswer.DESCRIPTION, false, Package.Params.HuntAlgorithmNoAnswer.DEFAULT)]
		public string HuntAlgorithmNoAnswer { set { huntAlgorithmNoAnswer = value; /*huntAlgorithmNoAnswerSpecified = true;*/ } }

		[ActionParamField(Package.Params.HuntAlgorithmNotAvailable.DISPLAY, Package.Params.HuntAlgorithmNotAvailable.DESCRIPTION, false, Package.Params.HuntAlgorithmNotAvailable.DEFAULT)]
		public string HuntAlgorithmNotAvailable { set { huntAlgorithmNotAvailable = value; /*huntAlgorithmNotAvailableSpecified = true;*/ } }
		
		[ActionParamField(Package.Params.Name.DISPLAY, Package.Params.Name.DESCRIPTION, false, Package.Params.Name.DEFAULT)]
		public string Name { set { name = value; } }
		
		[ActionParamField(Package.Params.Uuid.DISPLAY, Package.Params.Uuid.DESCRIPTION, false, Package.Params.Uuid.DEFAULT)]
		public string Uuid { set { uuid = value; } }
		
		[ActionParamField(Package.Params.RnaReversionTimeOut.DISPLAY, Package.Params.RnaReversionTimeOut.DESCRIPTION, false, Package.Params.RnaReversionTimeOut.DEFAULT)]
		public string RnaReversionTimeOut { set { rnaReversionTimeOut = value; } }
		
        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.AddLineGroupResponse.DISPLAY, Package.Results.AddLineGroupResponse.DESCRIPTION)]
        public addLineGroupResponse AddLineGroupResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

		private LineGroupMembers members;
		private string distributeAlgorithm;
		//private bool distributeAlgorithmSpecified;
		private string huntAlgorithmBusy;
		//private bool huntAlgorithmBusySpecified;
		private string huntAlgorithmNoAnswer;
		//private bool huntAlgorithmNoAnswerSpecified;
		private string huntAlgorithmNotAvailable;
		//private bool huntAlgorithmNotAvailableSpecified;
		private string name;
		private string uuid;
		private string rnaReversionTimeOut;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        private addLineGroupResponse response;

        public AddLineGroup()
        {
            Clear();	
        }

        public void Clear()
        {
			this.members								= null;
            this.distributeAlgorithm                    = null;//dpotter string.TopDown;
			//this.distributeAlgorithmSpecified			= false;
            this.huntAlgorithmBusy                      = null;//dpotter string.TrynextmemberthentrynextgroupinHuntList;
			//this.huntAlgorithmBusySpecified				= false;
            this.huntAlgorithmNoAnswer                  = null;//dpotter string.TrynextmemberthentrynextgroupinHuntList;
			//this.huntAlgorithmNoAnswerSpecified			= false;
            this.huntAlgorithmNotAvailable              = null;//dpotter string.TrynextmemberthentrynextgroupinHuntList;
			//this.huntAlgorithmNotAvailableSpecified		= false;
			this.name									= null;
			this.rnaReversionTimeOut					= null;
            this.username								= IAxlSoap.DefaultCcmAdmin;
            this.password								= null;
            this.callManagerIP							= null;
            this.response								= new addLineGroupResponse();
            this.message								= String.Empty;
            this.code									= 0;
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
                         
            addLineGroup lineGroup = new addLineGroup();

			lineGroup.lineGroup = new XLineGroup();
			lineGroup.lineGroup.distributionAlgorithm				= distributeAlgorithm;
			//dpotter lineGroup.lineGroup.distributionAlgorithmSpecified		= distributeAlgorithmSpecified;
			lineGroup.lineGroup.huntAlgorithmBusy					= huntAlgorithmBusy;
            //dpotter lineGroup.lineGroup.huntAlgorithmBusySpecified			= huntAlgorithmBusySpecified;
			lineGroup.lineGroup.huntAlgorithmNoAnswer				= huntAlgorithmNoAnswer;
            //dpotter lineGroup.lineGroup.huntAlgorithmNoAnswerSpecified		= huntAlgorithmNoAnswerSpecified;
			lineGroup.lineGroup.huntAlgorithmNotAvailable			= huntAlgorithmNotAvailable;
            //dpotter lineGroup.lineGroup.huntAlgorithmNotAvailableSpecified	= huntAlgorithmNotAvailableSpecified;
			lineGroup.lineGroup.name								= name;
			lineGroup.lineGroup.uuid								= uuid;
			lineGroup.lineGroup.rnaReversionTimeOut					= rnaReversionTimeOut;

			lineGroup.lineGroup.members								= members == null ? null : members.Data;

            try
            {
                response = client.addLineGroup(lineGroup);
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
