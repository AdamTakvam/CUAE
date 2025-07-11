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
using Metreos.AxlSoap504;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504.Actions.GetTranslationPattern;

namespace Metreos.Native.AxlSoap504
{
	/// <summary> Wraps up the 'getTranslationPattern' AXL SOAP method for Cisco CallManager 5.0.4 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.PACKAGE_DESCRIPTION)]
    public class GetTranslationPattern : INativeAction
	{
        [ActionParamField(Package.Params.RoutePartitionName.DISPLAY, Package.Params.RoutePartitionName.DESCRIPTION, false, Package.Params.RoutePartitionName.DEFAULT)]
        public string RoutePartitionName { set { routePartitionName = value; } }

        [ActionParamField(Package.Params.RoutePartitionId.DISPLAY, Package.Params.RoutePartitionId.DESCRIPTION, false, Package.Params.RoutePartitionId.DEFAULT)]
        public string RoutePartitionId { set { routePartitionId = value; } }
            
        [ActionParamField(Package.Params.RouteFilterName.DISPLAY, Package.Params.RouteFilterName.DESCRIPTION, false, Package.Params.RouteFilterName.DEFAULT)]
        public string RouteFilterName { set { routeFilterName = value; } }

        [ActionParamField(Package.Params.RouteFilterId.DISPLAY, Package.Params.RouteFilterId.DESCRIPTION, false, Package.Params.RouteFilterId.DEFAULT)]
        public string RouteFilterId { set { routeFilterId = value; } }
            
        [ActionParamField(Package.Params.Pattern.DISPLAY, Package.Params.Pattern.DESCRIPTION, false, Package.Params.Pattern.DEFAULT)]
        public string Pattern { set { pattern = value; } }

        [ActionParamField(Package.Params.Uuid.DISPLAY, Package.Params.Uuid.DESCRIPTION, false, Package.Params.Uuid.DEFAULT)]
        public string Uuid { set { uuid = value; } }
        
        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.GetTranslationPatternResponse.DISPLAY, Package.Results.GetTranslationPatternResponse.DESCRIPTION)]
        public getTransPatternResponse GetTranslationPatternResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private string routePartitionId;
        private string routePartitionName;
        private string routeFilterId;
        private string routeFilterName;
        private string pattern;
        private string uuid;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        private getTransPatternResponse response;

		public GetTranslationPattern()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.routePartitionName         = String.Empty;
            this.routePartitionId           = null;
            this.routeFilterName            = String.Empty;
            this.routeFilterId              = null;
            this.pattern                    = null;
            this.uuid                       = null;
            this.username                   = IAxlSoap.DefaultCcmAdmin;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new getTransPatternResponse();
            this.message                    = String.Empty;
            this.code                       = 0;
        }

        public bool ValidateInput()
        {
            // Not inclusive enough logic
            //            if( (uuid == null || uuid == String.Empty) && 
            //                (pattern == null || pattern == String.Empty) &&
            //                (route)
            //            {
            //                log.Write(TraceLevel.Error, 
            //                    "Both 'Uuid' and 'Pattern' can not both be undefined.  " + 
            //                    "At least one must be defined.  Note: if you use pattern, " + 
            //                    "RoutePartition must also be specified unless the line " + 
            //                    "is in the null partition.");
            //
            //                return false;
            //            }

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
            
            getTransPattern request = new getTransPattern();
			request.Item = IAxlSoap.DetermineChosenBetweenStrings(routePartitionName, routePartitionId);
			request.ItemElementName = (ItemChoiceType57) IAxlSoap.DetermineChosenBetweenStringsType(
				routePartitionName, routePartitionId, ItemChoiceType57.routePartitionName, ItemChoiceType57.routePartitionId);
			request.Item1 = IAxlSoap.DetermineChosenBetweenStrings(routeFilterName, routeFilterId);
			request.Item1ElementName = (Item1ChoiceType15) IAxlSoap.DetermineChosenBetweenStringsType(
				routeFilterName, routeFilterId, Item1ChoiceType15.routeFilterName, Item1ChoiceType15.routeFilterId);
			request.pattern = pattern;
			request.uuid = uuid;
             
            try
            {
                response = client.getTransPattern(request);
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
