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
using Metreos.AxlSoap413;
using Metreos.Types.AxlSoap413;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap413.Actions.UpdateTranslationPattern;

namespace Metreos.Native.AxlSoap413
{
    /// <summary> Wraps up the 'updateTranslationPattern' AXL SOAP method for Cisco CallManager 4.1.3 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.PACKAGE_DESCRIPTION)]
    public class UpdateTranslationPattern : INativeAction
    {     
        [ActionParamField(Package.Params.DialPlanName.DISPLAY, Package.Params.DialPlanName.DESCRIPTION, false, Package.Params.DialPlanName.DEFAULT)]
        public string DialPlanName { set { dialPlanName = value;} }
        
        [ActionParamField(Package.Params.DialPlanId.DISPLAY, Package.Params.DialPlanId.DESCRIPTION, false, Package.Params.DialPlanId.DEFAULT)]
        public string DialPlanId { set { dialPlanId = value;} }

        [ActionParamField(Package.Params.DigitDiscardInstructionName.DISPLAY, Package.Params.DigitDiscardInstructionName.DESCRIPTION, false, Package.Params.DigitDiscardInstructionName.DEFAULT)]
        public string DigitDiscardInstructionName { set { digitDiscardInstructionName = value;} }
        
        [ActionParamField(Package.Params.DigitDiscardInstructionId.DISPLAY, Package.Params.DigitDiscardInstructionId.DESCRIPTION, false, Package.Params.DigitDiscardInstructionId.DEFAULT)]
        public string DigitDiscardInstructionId { set { digitDiscardInstructionId = value;} }

        [ActionParamField(Package.Params.NewRouteFilterName.DISPLAY, Package.Params.NewRouteFilterName.DESCRIPTION, false, Package.Params.NewRouteFilterName.DEFAULT)]
        public string NewRouteFilterName { set { newRouteFilterName = value;} }

        [ActionParamField(Package.Params.NewRouteFilterId.DISPLAY, Package.Params.NewRouteFilterId.DESCRIPTION, false, Package.Params.NewRouteFilterId.DEFAULT)]
        public string NewRouteFilterId { set { newRouteFilterId = value;} }
        
        [ActionParamField(Package.Params.CallingSearchSpaceName.DISPLAY, Package.Params.CallingSearchSpaceName.DESCRIPTION, false, Package.Params.CallingSearchSpaceName.DEFAULT)]
        public string CallingSearchSpaceName { set { callingSearchSpaceName = value;} }

        [ActionParamField(Package.Params.CallingSearchSpaceId.DISPLAY, Package.Params.CallingSearchSpaceId.DESCRIPTION, false, Package.Params.CallingSearchSpaceId.DEFAULT)]
        public string CallingSearchSpaceId { set { callingSearchSpaceId = value;} }

        [ActionParamField(Package.Params.BlockEnable.DISPLAY, Package.Params.BlockEnable.DESCRIPTION, false, Package.Params.BlockEnable.DEFAULT)]
        public bool BlockEnable { set { blockEnable = value; blockEnableSpecified = true;} }

        [ActionParamField(Package.Params.CalledPartyTransformationMask.DISPLAY, Package.Params.CalledPartyTransformationMask.DESCRIPTION, false, Package.Params.CalledPartyTransformationMask.DEFAULT)]
        public string CalledPartyTransformationMask { set { calledPartyTransformationMask = value;} }
        
        [ActionParamField(Package.Params.CallingPartyTransformationMask.DISPLAY, Package.Params.CallingPartyTransformationMask.DESCRIPTION, false, Package.Params.CallingPartyTransformationMask.DEFAULT)]
        public string CallingPartyTransformationMask { set { callingPartyTransformationMask = value;} }

        [ActionParamField(Package.Params.UseCallingPartyPhoneMask.DISPLAY, Package.Params.UseCallingPartyPhoneMask.DESCRIPTION, false, Package.Params.UseCallingPartyPhoneMask.DEFAULT)]
        public XStatus UseCallingPartyPhoneMask { set { useCallingPartyPhoneMask = value; useCallingPartyPhoneMaskSpecified = true;} }

        [ActionParamField(Package.Params.CallingPartyPrefixDigits.DISPLAY, Package.Params.CallingPartyPrefixDigits.DESCRIPTION, false, Package.Params.CallingPartyPrefixDigits.DEFAULT)]
        public string CallingPartyPrefixDigits { set { callingPartyPrefixDigits = value; } }

        [ActionParamField(Package.Params.CallingPartyPresentation.DISPLAY, Package.Params.CallingPartyPresentation.DESCRIPTION, false, Package.Params.CallingPartyPresentation.DEFAULT)]
        public XPresentationBit CallingPartyPresentation { set { callingPartyPresentation = value; } }

        [ActionParamField(Package.Params.MessageWaiting.DISPLAY, Package.Params.MessageWaiting.DESCRIPTION, false, Package.Params.MessageWaiting.DEFAULT)]
        public XLampBlinkRate MessageWaiting { set { messageWaiting = value; messageWaitingSpecified = true;} }

        [ActionParamField(Package.Params.NetworkLocation.DISPLAY, Package.Params.NetworkLocation.DESCRIPTION, false, Package.Params.NetworkLocation.DEFAULT)]
        public XNetworkLocation NetworkLocation { set { networkLocation = value; networkLocationSpecified = true;} }

        [ActionParamField(Package.Params.PatternUrgency.DISPLAY, Package.Params.PatternUrgency.DESCRIPTION, false, Package.Params.PatternUrgency.DEFAULT)]
        public bool PatternUrgency { set { patternUrgency = value; patternUrgencySpecified = true;} }

        [ActionParamField(Package.Params.PrefixDigitsOut.DISPLAY, Package.Params.PrefixDigitsOut.DESCRIPTION, false, Package.Params.PrefixDigitsOut.DEFAULT)]
        public string PrefixDigitsOut { set { prefixDigitsOut = value; } }

        [ActionParamField(Package.Params.CallingLinePresentationBit.DISPLAY, Package.Params.CallingLinePresentationBit.DESCRIPTION, false, Package.Params.CallingLinePresentationBit.DEFAULT)]
        public XPresentationBit CallingLinePresentationBit { set { callingLinePresentationBit = value; callingLinePresentationBitSpecified = true;} }

        [ActionParamField(Package.Params.CallingNamePresentationBit.DISPLAY, Package.Params.CallingNamePresentationBit.DESCRIPTION, false, Package.Params.CallingNamePresentationBit.DEFAULT)]
        public XPresentationBit CallingNamePresentationBit { set { callingNamePresentationBit = value; callingNamePresentationBitSpecified = true;} }

        [ActionParamField(Package.Params.ConnectedLinePresentationBit.DISPLAY, Package.Params.ConnectedLinePresentationBit.DESCRIPTION, false, Package.Params.ConnectedLinePresentationBit.DEFAULT)]
        public XPresentationBit ConnectedLinePresentationBit { set { connectedLinePresentationBit = value; connectedLinePresentationBitSpecified = true;} }

        [ActionParamField(Package.Params.ConnectedNamePresentationBit.DISPLAY, Package.Params.ConnectedNamePresentationBit.DESCRIPTION, false, Package.Params.ConnectedNamePresentationBit.DEFAULT)]
        public XPresentationBit ConnectedNamePresentationBit { set { connectedNamePresentationBit = value; connectedNamePresentationBitSpecified = true;} }

        [ActionParamField(Package.Params.SupportOverlapSending.DISPLAY, Package.Params.SupportOverlapSending.DESCRIPTION, false, Package.Params.SupportOverlapSending.DEFAULT)]
        public bool SupportOverlapSending { set { supportOverlapSending = value; supportOverlapSendingSpecified = true;} }


        [ActionParamField(Package.Params.Description.DISPLAY, Package.Params.Description.DESCRIPTION, false, Package.Params.Description.DEFAULT)]
        public string Description { set { description = value; } }

        [ActionParamField(Package.Params.RoutePartitionName.DISPLAY, Package.Params.RoutePartitionName.DESCRIPTION, false, Package.Params.RoutePartitionName.DEFAULT)]
        public string RoutePartitionName { set { routePartitionName = value; } }

        [ActionParamField(Package.Params.RoutePartitionId.DISPLAY, Package.Params.RoutePartitionId.DESCRIPTION, false, Package.Params.RoutePartitionId.DEFAULT)]
        public string RoutePartitionId { set { routePartitionId = value; } }

        [ActionParamField(Package.Params.NewRoutePartitionName.DISPLAY, Package.Params.NewRoutePartitionName.DESCRIPTION, false, Package.Params.NewRoutePartitionName.DEFAULT)]
        public string NewRoutePartitionName { set { newRoutePartitionName = value; } }

        [ActionParamField(Package.Params.NewRoutePartitionId.DISPLAY, Package.Params.NewRoutePartitionId.DESCRIPTION, false, Package.Params.NewRoutePartitionId.DEFAULT)]
        public string NewRoutePartitionId { set { newRoutePartitionId = value; } }

        [ActionParamField(Package.Params.RouteFilterName.DISPLAY, Package.Params.RouteFilterName.DESCRIPTION, false, Package.Params.RouteFilterName.DEFAULT)]
        public string RouteFilterName { set { routeFilterName = value; } }

        [ActionParamField(Package.Params.RouteFilterId.DISPLAY, Package.Params.RouteFilterId.DESCRIPTION, false, Package.Params.RouteFilterId.DEFAULT)]
        public string RouteFilterId { set { routeFilterId = value; } }

        [ActionParamField(Package.Params.Pattern.DISPLAY, Package.Params.Pattern.DESCRIPTION, false, Package.Params.Pattern.DEFAULT)]
        public string Pattern { set { pattern = value; } }

        [ActionParamField(Package.Params.NewPattern.DISPLAY, Package.Params.NewPattern.DESCRIPTION, false, Package.Params.NewPattern.DEFAULT)]
        public string NewPattern { set { newPattern = value; } }

        [ActionParamField(Package.Params.Uuid.DISPLAY, Package.Params.Uuid.DESCRIPTION, false, Package.Params.Uuid.DEFAULT)]
        public string Uuid { set { uuid = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.UpdateTransPatternResponse.DISPLAY, Package.Results.UpdateTransPatternResponse.DESCRIPTION)]
        public updateTransPatternResponse UpdateTransPatternResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private bool blockEnable;
        private string dialPlanName;
        private string dialPlanId;
        private string digitDiscardInstructionName;
        private string digitDiscardInstructionId;
        private string newRouteFilterName;
        private string newRouteFilterId;
        private string callingSearchSpaceName;
        private string callingSearchSpaceId;
        private string calledPartyTransformationMask;
        private string callingPartyTransformationMask;
        private XStatus useCallingPartyPhoneMask;
        private string callingPartyPrefixDigits;
        private XPresentationBit callingPartyPresentation;
        private XLampBlinkRate messageWaiting;
        private XNetworkLocation networkLocation;
        private bool patternUrgency;
        private string prefixDigitsOut;
        private XPresentationBit callingLinePresentationBit;
        private XPresentationBit callingNamePresentationBit;
        private XPresentationBit connectedLinePresentationBit;
        private XPresentationBit connectedNamePresentationBit;
        private bool supportOverlapSending;

        private bool blockEnableSpecified;
        private bool useCallingPartyPhoneMaskSpecified;
        private bool callingPartyPresentationSpecified;
        private bool messageWaitingSpecified;
        private bool networkLocationSpecified;
        private bool patternUrgencySpecified;
        private bool callingLinePresentationBitSpecified;
        private bool callingNamePresentationBitSpecified;
        private bool connectedLinePresentationBitSpecified;
        private bool connectedNamePresentationBitSpecified;
        private bool supportOverlapSendingSpecified;

        #region Shared with UpdateLine (Important for future action integration, for UpdateRoutePattern, for instance)
        private string description;
        private string routePartitionName;
        private string routePartitionId;
        private string newRoutePartitionName;
        private string newRoutePartitionId;
        private string routeFilterName;
        private string routeFilterId;
        private string newPattern;
        private string pattern;
        private string uuid;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        #endregion
        
        private updateTransPatternResponse response;

        public UpdateTranslationPattern()
        {
            Clear();	
        }

        public void Clear()
        {
            this.blockEnable                            = false;
            this.dialPlanName                           = null;
            this.dialPlanId                             = null;
            this.digitDiscardInstructionName            = null;
            this.digitDiscardInstructionId              = null;
            this.newRouteFilterName                     = null;
            this.newRouteFilterId                       = null;
            this.callingSearchSpaceName                 = null;
            this.callingSearchSpaceId                   = null;
            this.calledPartyTransformationMask          = null;
            this.callingPartyTransformationMask         = null;
            this.useCallingPartyPhoneMask               = XStatus.Default;
            this.callingPartyPrefixDigits               = null;
            this.callingPartyPresentation               = XPresentationBit.Default;
            this.messageWaiting                         = XLampBlinkRate.LampOff;
            this.networkLocation                        = XNetworkLocation.OnNet;
            this.patternUrgency                         = false;
            this.prefixDigitsOut                        = null;
            this.callingLinePresentationBit             = XPresentationBit.Default;
            this.callingNamePresentationBit             = XPresentationBit.Default;
            this.connectedLinePresentationBit           = XPresentationBit.Default;
            this.connectedNamePresentationBit           = XPresentationBit.Default;
            this.supportOverlapSending                  = false;

            this.blockEnableSpecified                   = false;
            this.useCallingPartyPhoneMaskSpecified      = false;
            this.callingPartyPresentationSpecified      = false;
            this.messageWaitingSpecified                = false;
            this.networkLocationSpecified               = false;
            this.patternUrgencySpecified                = false;
            this.callingLinePresentationBitSpecified    = false;
            this.callingNamePresentationBitSpecified    = false;
            this.connectedLinePresentationBitSpecified  = false;
            this.connectedNamePresentationBitSpecified  = false;
            this.supportOverlapSendingSpecified         = false;

            this.description                = null;
            this.newPattern                 = null;
            this.newRoutePartitionId        = null;
            this.newRoutePartitionName      = null;
            this.pattern                    = null;
            this.routeFilterId              = null;
            this.routeFilterName            = String.Empty;
            this.routePartitionId           = null;
            this.routePartitionName         = String.Empty;
            this.uuid                       = null;
            this.username                   = IAxlSoap.DefaultCcmAdmin;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new updateTransPatternResponse();
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
                         
            updateTransPattern trans = new updateTransPattern();

            // line.Item (routePartition), line.Item5 (routeFilter), line.pattern (line number) all act as identifiers 
            // for the line to identify.
            // However, if uuid is specified (which is also an identifier), then routePartition, routeFilter, and pattern
            // can not be specified... not even such as <routePartitionName />.  
            // The element must not be even present in the XML.

            trans.description = description;
			trans.Item2 = IAxlSoap.DetermineChosenBetweenStrings(dialPlanName, dialPlanId);
			trans.Item2ElementName = (Item2ChoiceType10) IAxlSoap.DetermineChosenBetweenStringsType(
				dialPlanName, dialPlanId, Item2ChoiceType10.dialPlanName, Item2ChoiceType10.dialPlanId);
       
//            trans.Item1 = IAxlSoap.DetermineChosenBetweenStrings(newRoutePartitionName, newRoutePartitionId);
//            trans.Item1ElementName = (Item1ChoiceType3) IAxlSoap.DetermineChosenBetweenStringsType(
//                newRoutePartitionName, newRoutePartitionId, Item1ChoiceType3.newRoutePartitionName, Item1ChoiceType3.newRoutePartitionId);
            trans.Item2 = IAxlSoap.DetermineChosenBetweenStrings(dialPlanName, dialPlanId);
            trans.Item2ElementName = (Item2ChoiceType10) IAxlSoap.DetermineChosenBetweenStringsType(
                dialPlanName, dialPlanId, Item2ChoiceType10.dialPlanName, Item2ChoiceType10.dialPlanId);
            trans.Item3 = IAxlSoap.DetermineChosenBetweenStrings(digitDiscardInstructionName, digitDiscardInstructionId);
            trans.Item3ElementName = (Item3ChoiceType9) IAxlSoap.DetermineChosenBetweenStringsType(
                digitDiscardInstructionName, digitDiscardInstructionId, Item3ChoiceType9.digitDiscardInstructionName, Item3ChoiceType9.digitDiscardInstructionId);
//            trans.Item4 = IAxlSoap.DetermineChosenBetweenStrings(newRouteFilterName, newRouteFilterId);
//            trans.Item4ElementName = (Item4ChoiceType2) IAxlSoap.DetermineChosenBetweenStringsType(
//                routeFilterName, routeFilterId, Item4ChoiceType2.routeFilterName, Item4ChoiceType2.routeFilterId);
            trans.Item5 = IAxlSoap.DetermineChosenBetweenStrings(callingSearchSpaceName, callingSearchSpaceId);
            trans.Item5ElementName = (Item5ChoiceType6) IAxlSoap.DetermineChosenBetweenStringsType(
                callingSearchSpaceName, callingSearchSpaceId, Item5ChoiceType6.callingSearchSpaceName, Item5ChoiceType6.callingSearchSpaceId);
            trans.newPattern = newPattern;
            trans.pattern = uuid != null ? null : pattern;
            trans.uuid = uuid;

            trans.blockEnable                           = blockEnable;
            trans.blockEnableSpecified                  = blockEnableSpecified;
            trans.calledPartyTransformationMask         = calledPartyTransformationMask;
            trans.callingPartyTransformationMask        = callingPartyTransformationMask;
            trans.useCallingPartyPhoneMask              = useCallingPartyPhoneMask;
            trans.useCallingPartyPhoneMaskSpecified     = useCallingPartyPhoneMaskSpecified;
            trans.callingPartyPrefixDigits              = callingPartyPrefixDigits;
            trans.callingPartyPresentation              = callingPartyPresentation;
            trans.callingPartyPresentationSpecified     = callingPartyPresentationSpecified;
            trans.messageWaiting                        = messageWaiting;
            trans.messageWaitingSpecified               = messageWaitingSpecified;
            trans.networkLocation                       = networkLocation;
            trans.networkLocationSpecified              = networkLocationSpecified;
            trans.patternUrgency                        = patternUrgency;
            trans.patternUrgencySpecified               = patternUrgencySpecified;
            trans.prefixDigitsOut                       = prefixDigitsOut;
            trans.callingLinePresentationBit            = callingLinePresentationBit;
            trans.callingLinePresentationBitSpecified   = callingLinePresentationBitSpecified;
            trans.callingNamePresentationBit            = callingNamePresentationBit;
            trans.callingNamePresentationBitSpecified   = callingNamePresentationBitSpecified;
            trans.connectedLinePresentationBit          = connectedLinePresentationBit;
            trans.connectedLinePresentationBitSpecified = connectedLinePresentationBitSpecified;
            trans.connectedNamePresentationBit          = connectedNamePresentationBit;
            trans.connectedNamePresentationBitSpecified = connectedNamePresentationBitSpecified;
            trans.supportOverlapSending                 = supportOverlapSending;
            trans.supportOverlapSendingSpecified        = supportOverlapSendingSpecified;
                
            try
            {
                response = client.updateTransPattern(trans);
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
