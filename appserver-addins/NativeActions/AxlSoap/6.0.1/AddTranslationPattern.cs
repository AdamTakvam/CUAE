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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601.Actions.AddTranslationPattern;

namespace Metreos.Native.AxlSoap601
{
    /// <summary> Wraps up the 'addTranslationPattern' AXL SOAP method for Cisco CallManager 6.0.1 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.PACKAGE_DESCRIPTION)]
    public class AddTranslationPattern : INativeAction
    {     
        [ActionParamField(Package.Params.DialPlanName.DISPLAY, Package.Params.DialPlanName.DESCRIPTION, false, Package.Params.DialPlanName.DEFAULT)]
        public string DialPlanName { set { dialPlanName = value;} }
        
        [ActionParamField(Package.Params.DialPlanId.DISPLAY, Package.Params.DialPlanId.DESCRIPTION, false, Package.Params.DialPlanId.DEFAULT)]
        public string DialPlanId { set { dialPlanId = value;} }

        [ActionParamField(Package.Params.DigitDiscardInstructionName.DISPLAY, Package.Params.DigitDiscardInstructionName.DESCRIPTION, false, Package.Params.DigitDiscardInstructionName.DEFAULT)]
        public string DigitDiscardInstructionName { set { digitDiscardInstructionName = value;} }
        
        [ActionParamField(Package.Params.DigitDiscardInstructionId.DISPLAY, Package.Params.DigitDiscardInstructionId.DESCRIPTION, false, Package.Params.DigitDiscardInstructionId.DEFAULT)]
        public string DigitDiscardInstructionId { set { digitDiscardInstructionId = value;} }
        
        [ActionParamField(Package.Params.DialPlanWizardGenId.DISPLAY, Package.Params.DialPlanWizardGenId.DESCRIPTION, false, Package.Params.DialPlanWizardGenId.DEFAULT)]
        public string DialPlanWizardGenId { set { dialPlanWizardGenId = value;} }
        
        [ActionParamField(Package.Params.CallingSearchSpaceName.DISPLAY, Package.Params.CallingSearchSpaceName.DESCRIPTION, false, Package.Params.CallingSearchSpaceName.DEFAULT)]
        public string CallingSearchSpaceName { set { callingSearchSpaceName = value;} }

        [ActionParamField(Package.Params.CallingSearchSpaceId.DISPLAY, Package.Params.CallingSearchSpaceId.DESCRIPTION, false, Package.Params.CallingSearchSpaceId.DEFAULT)]
        public string CallingSearchSpaceId { set { callingSearchSpaceId = value;} }

        [ActionParamField(Package.Params.BlockEnable.DISPLAY, Package.Params.BlockEnable.DESCRIPTION, false, Package.Params.BlockEnable.DEFAULT)]
        public bool BlockEnable { set { blockEnable = value; } }

        [ActionParamField(Package.Params.CalledPartyTransformationMask.DISPLAY, Package.Params.CalledPartyTransformationMask.DESCRIPTION, false, Package.Params.CalledPartyTransformationMask.DEFAULT)]
        public string CalledPartyTransformationMask { set { calledPartyTransformationMask = value;} }
        
        [ActionParamField(Package.Params.CallingPartyTransformationMask.DISPLAY, Package.Params.CallingPartyTransformationMask.DESCRIPTION, false, Package.Params.CallingPartyTransformationMask.DEFAULT)]
        public string CallingPartyTransformationMask { set { callingPartyTransformationMask = value;} }

        [ActionParamField(Package.Params.UseCallingPartyPhoneMask.DISPLAY, Package.Params.UseCallingPartyPhoneMask.DESCRIPTION, false, Package.Params.UseCallingPartyPhoneMask.DEFAULT)]
        public string UseCallingPartyPhoneMask { set { useCallingPartyPhoneMask = value; } }

        [ActionParamField(Package.Params.CallingPartyPrefixDigits.DISPLAY, Package.Params.CallingPartyPrefixDigits.DESCRIPTION, false, Package.Params.CallingPartyPrefixDigits.DEFAULT)]
        public string CallingPartyPrefixDigits { set { callingPartyPrefixDigits = value; } }

        [ActionParamField(Package.Params.CallingPartyPresentationBit.DISPLAY, Package.Params.CallingPartyPresentationBit.DESCRIPTION, false, Package.Params.CallingPartyPresentationBit.DEFAULT)]
        public string CallingPartyPresentationBit { set { callingPartyPresentationBit = value; /*callingPartyPresentationBitSpecified = true;*/ } }

		[ActionParamField(Package.Params.CallingLinePresentationBit.DISPLAY, Package.Params.CallingLinePresentationBit.DESCRIPTION, false, Package.Params.CallingLinePresentationBit.DEFAULT)]
		public string CallingLinePresentationBit { set { callingLinePresentationBit = value; /*callingLinePresentationBitSpecified = true;*/ } }

		[ActionParamField(Package.Params.ConnectedLinePresentationBit.DISPLAY, Package.Params.ConnectedLinePresentationBit.DESCRIPTION, false, Package.Params.ConnectedLinePresentationBit.DEFAULT)]
		public string ConnectedLinePresentationBit { set { connectedLinePresentationBit = value; /*connectedLinePresentationBitSpecified = true;*/ } }
		
		[ActionParamField(Package.Params.ConnectedNamePresentationBit.DISPLAY, Package.Params.ConnectedNamePresentationBit.DESCRIPTION, false, Package.Params.ConnectedNamePresentationBit.DEFAULT)]
		public string ConnectedNamePresentationBit { set { connectedNamePresentationBit = value; /*connectedNamePresentationBitSpecified = true;*/ } }
		
		[ActionParamField(Package.Params.PatternPrecedence.DISPLAY, Package.Params.PatternPrecedence.DESCRIPTION, false, Package.Params.PatternPrecedence.DEFAULT)]
		public string PatternPrecedence { set { patternPrecedence = value; /*patternPrecedenceSpecified = true;*/} }
		
		[ActionParamField(Package.Params.ReleaseCause.DISPLAY, Package.Params.ReleaseCause.DESCRIPTION, false, Package.Params.ReleaseCause.DEFAULT)]
		public string ReleaseCause { set { releaseCause = value; /*releaseCauseSpecified = true;*/ } }

		[ActionParamField(Package.Params.SupportOverlapSending.DISPLAY, Package.Params.SupportOverlapSending.DESCRIPTION, false, Package.Params.SupportOverlapSending.DEFAULT)]
		public bool SupportOverlapSending { set { supportOverlapSending = value; supportOverlapSendingSpecified = true; } }

        [ActionParamField(Package.Params.MessageWaiting.DISPLAY, Package.Params.MessageWaiting.DESCRIPTION, false, Package.Params.MessageWaiting.DEFAULT)]
        public string MessageWaiting { set { messageWaiting = value; } }

        [ActionParamField(Package.Params.NetworkLocation.DISPLAY, Package.Params.NetworkLocation.DESCRIPTION, false, Package.Params.NetworkLocation.DEFAULT)]
        public string NetworkLocation { set { networkLocation = value; } }

        [ActionParamField(Package.Params.PatternUrgency.DISPLAY, Package.Params.PatternUrgency.DESCRIPTION, false, Package.Params.PatternUrgency.DEFAULT)]
        public bool PatternUrgency { set { patternUrgency = value; } }

        [ActionParamField(Package.Params.PrefixDigitsOut.DISPLAY, Package.Params.PrefixDigitsOut.DESCRIPTION, false, Package.Params.PrefixDigitsOut.DEFAULT)]
        public string PrefixDigitsOut { set { prefixDigitsOut = value; } }

        // This is marked as ... read-only in the API.  I don't know why its defined then.  Maybe due to reuse elsewhere.  Testing required with this value,
        // but I suspect its always supposed to be XPatternUsage.Translation, if anything
//        [ActionParamField(Package.Params.Usage.DISPLAY, Package.Params.Usage.DESCRIPTION, false, Package.Params.Usage.DEFAULT)]
//        public XPatternUsage Usage { set { usage = value; } }

        [ActionParamField(Package.Params.Description.DISPLAY, Package.Params.Description.DESCRIPTION, false, Package.Params.Description.DEFAULT)]
        public string Description { set { description = value; } }

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

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.AddTransPatternResponse.DISPLAY, Package.Results.AddTransPatternResponse.DESCRIPTION)]
        public addTransPatternResponse AddTransPatternResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private string dialPlanWizardGenId;
        private bool blockEnable;
        private string dialPlanName;
        private string dialPlanId;
        private string digitDiscardInstructionName;
        private string digitDiscardInstructionId;
        private string callingSearchSpaceName;
        private string callingSearchSpaceId;
        private string calledPartyTransformationMask;
        private string callingPartyTransformationMask;
        private string useCallingPartyPhoneMask;
        private string callingPartyPrefixDigits;
        private string callingPartyPresentationBit;
		//private bool callingPartyPresentationBitSpecified;
		private string callingLinePresentationBit;
		//private bool callingLinePresentationBitSpecified;
		private string connectedNamePresentationBit;
		//private bool connectedNamePresentationBitSpecified;
		private string connectedLinePresentationBit;
		//private bool connectedLinePresentationBitSpecified;
		private string patternPrecedence;
		//private bool patternPrecedenceSpecified;
		private string releaseCause;
		//private bool releaseCauseSpecified;
		private bool supportOverlapSending;
		private bool supportOverlapSendingSpecified;
        private string messageWaiting;
        private string networkLocation;
        private bool patternUrgency;
        private string prefixDigitsOut;
        private string usage;

        #region Shared with UpdateLine (Important for future action integration, for UpdateRoutePattern, for instance)
        private string description;
        private string routePartitionName;
        private string routePartitionId;
        private string routeFilterName;
        private string routeFilterId;
        private string pattern;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        #endregion
        
        private addTransPatternResponse response;

        public AddTranslationPattern()
        {
            Clear();	
        }

        public void Clear()
        {
            this.dialPlanWizardGenId                    = String.Empty;
            this.blockEnable                            = false;
            this.dialPlanName                           = null;
            this.dialPlanId                             = null;
            this.digitDiscardInstructionName            = null;
            this.digitDiscardInstructionId              = null;
            this.callingSearchSpaceName                 = null;
            this.callingSearchSpaceId                   = null;
            this.calledPartyTransformationMask          = String.Empty; // necessary for ccm parser, learned through observation
            this.callingPartyTransformationMask         = String.Empty; // necessary for  ccm parser, learned through observation
            this.callingPartyPresentationBit            = null;//dpotter string.Default;
			//this.callingPartyPresentationBitSpecified   = false;
			this.callingLinePresentationBit				= null;//dpotter string.Default;
			//this.callingLinePresentationBitSpecified    = false;
			this.connectedNamePresentationBit			= null;//dpotter string.Default;
			//this.connectedNamePresentationBitSpecified  = false;
			this.connectedLinePresentationBit			= null;//dpotter string.Default;
			//this.connectedLinePresentationBitSpecified  = false;
            this.patternPrecedence                      = null;//dpotter string.Default;
			//this.patternPrecedenceSpecified				= false;
            this.releaseCause                           = null;//dpotter XReleaseCauseValue.NoError;
			//this.releaseCauseSpecified					= false;
			this.supportOverlapSending					= false;
			this.supportOverlapSendingSpecified			= false;
            this.useCallingPartyPhoneMask               = null;//dpotter string.Default;
            this.callingPartyPrefixDigits               = null;
            this.messageWaiting                         = null;//dpotter XLampBlinkRate.LampOff;
            this.networkLocation                        = null;//dpotter XNetworkLocation.OnNet;
            this.patternUrgency                         = false;
            this.prefixDigitsOut                        = String.Empty;
            this.usage                                  = null;//dpotter XPatternUsage.Translation;

            this.description                = null;
            this.pattern                    = null;
            this.routeFilterId              = null;
            this.routeFilterName            = String.Empty;
            this.routePartitionId           = null;
            this.routePartitionName         = String.Empty;
            this.username                   = IAxlSoap.DefaultCcmAdmin;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new addTransPatternResponse();
            this.message                    = String.Empty;
            this.code                       = 0;
        }

        public bool ValidateInput()
        {
            // Not inclusive enough logic
//            if( (uuid == null || uuid == String.Empty) && 
//                (pattern == null || pattern == String.Empty))
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
                         
            addTransPattern trans = new addTransPattern();
            trans.newPattern = new XNPTranslationPattern();
			//trans.newPattern.allowDeviceOverride BUG?  No type defined for this field in WSDL.  

            // line.Item (routePartition), line.Item5 (routeFilter), line.pattern (line number) all act as identifiers 
            // for the line to identify.
            // However, if uuid is specified (which is also an identifier), then routePartition, routeFilter, and pattern
            // can not be specified... not even such as <routePartitionName />.  
            // The element must not be even present in the XML.

           if(routePartitionName != null)
            {
                trans.newPattern.Item = routePartitionName;
            }
            else
            {
                trans.newPattern.Item = CreateXRoutePartition(routePartitionId);
            }
			if(dialPlanName != null)
			{
				trans.newPattern.Item1 = callingSearchSpaceName;
			}
			else
			{
				trans.newPattern.Item1 = CreateXCallingSearchSpace(callingSearchSpaceId);
			}
            if(dialPlanName != null)
            {
                trans.newPattern.Item2 = dialPlanName;
            }
            else
            {
                trans.newPattern.Item2 = CreateXDialPlan(dialPlanId);
            }
            if(digitDiscardInstructionName != null)
            {
                trans.newPattern.Item3 = digitDiscardInstructionName;
            }
            else
            {
                trans.newPattern.Item3 = CreateXDigitDiscardInstruction(digitDiscardInstructionId);
            }
            if(routeFilterName != null)
            {
                trans.newPattern.Item4 = routeFilterName;
            }
            else
            {
                trans.newPattern.Item4 = CreateXRouteFilter(routeFilterId);
            }

			trans.newPattern.dialPlanWizardGenId				   = dialPlanWizardGenId;
			trans.newPattern.usage								   = usage;
			trans.newPattern.description						   = description;
            trans.newPattern.pattern                               = pattern;
            trans.newPattern.blockEnable                           = blockEnable;
            trans.newPattern.calledPartyTransformationMask         = calledPartyTransformationMask;
            trans.newPattern.callingPartyTransformationMask        = callingPartyTransformationMask;
            trans.newPattern.useCallingPartyPhoneMask              = useCallingPartyPhoneMask;
            trans.newPattern.callingPartyPrefixDigits              = callingPartyPrefixDigits;
            trans.newPattern.messageWaiting                        = messageWaiting;
            trans.newPattern.networkLocation                       = networkLocation;
            trans.newPattern.patternUrgency                        = patternUrgency;
            trans.newPattern.prefixDigitsOut                       = prefixDigitsOut;
            trans.newPattern.callingNamePresentationBit            = callingPartyPresentationBit;
            //dpotter trans.newPattern.callingNamePresentationBitSpecified   = callingPartyPresentationBitSpecified;
			trans.newPattern.callingLinePresentationBit			   = callingLinePresentationBit;
            //dpotter trans.newPattern.callingLinePresentationBitSpecified   = callingLinePresentationBitSpecified;
			trans.newPattern.connectedLinePresentationBit		   = connectedLinePresentationBit;
            //dpotter trans.newPattern.connectedLinePresentationBitSpecified = connectedLinePresentationBitSpecified;
			trans.newPattern.connectedNamePresentationBit		   = connectedNamePresentationBit;
            //dpotter trans.newPattern.connectedNamePresentationBitSpecified = connectedNamePresentationBitSpecified;
			trans.newPattern.patternPrecedence					   = patternPrecedence;
            //dpotter trans.newPattern.patternPrecedenceSpecified		       = patternPrecedenceSpecified;
			//trans.newPattern.provideOutsideDialtone			   = BUG?  No type defined for this field in WSDL.  
			trans.newPattern.releaseCause						   = releaseCause;
            //dpotter trans.newPattern.releaseCauseSpecified				   = releaseCauseSpecified;
			trans.newPattern.supportOverlapSending				   = supportOverlapSending;
			trans.newPattern.supportOverlapSendingSpecified        = supportOverlapSendingSpecified;
                
            try
            {
                response = client.addTransPattern(trans);
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

        private XCallingSearchSpace CreateXCallingSearchSpace(string id)
        {
            XCallingSearchSpace css = new XCallingSearchSpace();
            css.uuid = id;
            return css;
        }

        private XDialPlan CreateXDialPlan(string id)
        {
            XDialPlan dp = new XDialPlan();
            dp.uuid = id;
            return dp;
        }

        private XDigitDiscardInstruction CreateXDigitDiscardInstruction(string id)
        {
            XDigitDiscardInstruction ddi = new XDigitDiscardInstruction();
            ddi.uuid = id;
            return ddi;
        }

        private XRouteFilter CreateXRouteFilter(string id)
        {
            XRouteFilter rf = new XRouteFilter();
            rf.uuid = id;
            return rf;
        }

        private XRoutePartition CreateXRoutePartition(string id)
        {
            XRoutePartition rp = new XRoutePartition();
            rp.uuid = id;
            return rp;
        }
    }
}
