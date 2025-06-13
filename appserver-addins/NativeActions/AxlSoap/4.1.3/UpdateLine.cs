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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap413.Actions.UpdateLine;

namespace Metreos.Native.AxlSoap413
{
    /// <summary> Wraps up the 'updateLine' AXL SOAP method for Cisco CallManager 4.1.3 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.PACKAGE_DESCRIPTION)]
    public class UpdateLine : INativeAction
    {     
        [ActionParamField(Package.Params.AutoAnswer.DISPLAY, Package.Params.AutoAnswer.DESCRIPTION, false, Package.Params.AutoAnswer.DEFAULT)]
        public XAutoAnswer AutoAnswer { set { autoAnswer = value; } }

        [ActionParamField(Package.Params.AutoAnswerSpecified.DISPLAY, Package.Params.AutoAnswerSpecified.DESCRIPTION, false, Package.Params.AutoAnswerSpecified.DEFAULT)]
        public bool AutoAnswerSpecified { set { autoAnswerSpecified = value; } }

        [ActionParamField(Package.Params.CallPickupGroupId.DISPLAY, Package.Params.CallPickupGroupId.DESCRIPTION, false, Package.Params.CallPickupGroupId.DEFAULT)]
        public string CallPickupGroupId { set { callPickupGroupId = value; } }

        [ActionParamField(Package.Params.CallPickupGroupName.DISPLAY, Package.Params.CallPickupGroupName.DESCRIPTION, false, Package.Params.CallPickupGroupName.DEFAULT)]
        public string CallPickupGroupName { set { callPickupGroupName = value; } }

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

        [ActionParamField(Package.Params.AarNeighborhoodName.DISPLAY, Package.Params.AarNeighborhoodName.DESCRIPTION, false, Package.Params.AarNeighborhoodName.DEFAULT)]
        public string AarNeighborhoodName { set { aarNeighborhoodName = value; } }

        [ActionParamField(Package.Params.AarNeighborhoodId.DISPLAY, Package.Params.AarNeighborhoodId.DESCRIPTION, false, Package.Params.AarNeighborhoodId.DEFAULT)]
        public string AarNeighborhoodId { set { aarNeighborhoodId = value; } }

        [ActionParamField(Package.Params.ShareLineAppearanceCSSName.DISPLAY, Package.Params.ShareLineAppearanceCSSName.DESCRIPTION, false, Package.Params.ShareLineAppearanceCSSName.DEFAULT)]
        public string ShareLineAppearanceCSSName { set { shareLineAppearanceCSSName = value; } }

        [ActionParamField(Package.Params.ShareLineAppearanceCSSId.DISPLAY, Package.Params.ShareLineAppearanceCSSId.DESCRIPTION, false, Package.Params.ShareLineAppearanceCSSId.DEFAULT)]
        public string ShareLineAppearanceCSSId { set { shareLineAppearanceCSSId = value; } }

        [ActionParamField(Package.Params.VoiceMailProfileName.DISPLAY, Package.Params.VoiceMailProfileName.DESCRIPTION, false, Package.Params.VoiceMailProfileName.DEFAULT)]
        public string VoiceMailProfileName { set { voiceMailProfileName = value; } }

        [ActionParamField(Package.Params.VoiceMailProfileId.DISPLAY, Package.Params.VoiceMailProfileId.DESCRIPTION, false, Package.Params.VoiceMailProfileId.DEFAULT)]
        public string VoiceMailProfileId { set { voiceMailProfileId = value; } }

        [ActionParamField(Package.Params.RouteFilterName.DISPLAY, Package.Params.RouteFilterName.DESCRIPTION, false, Package.Params.RouteFilterName.DEFAULT)]
        public string RouteFilterName { set { routeFilterName = value; } }

        [ActionParamField(Package.Params.RouteFilterId.DISPLAY, Package.Params.RouteFilterId.DESCRIPTION, false, Package.Params.RouteFilterId.DEFAULT)]
        public string RouteFilterId { set { routeFilterId = value; } }

        [ActionParamField(Package.Params.NetworkHoldMOHAudioSourceId.DISPLAY, Package.Params.NetworkHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.NetworkHoldMOHAudioSourceId.DEFAULT)]
        public string NetworkHoldMOHAudioSourceId { set { networkHoldMOHAudioSourceId = value; } }

        [ActionParamField(Package.Params.Pattern.DISPLAY, Package.Params.Pattern.DESCRIPTION, false, Package.Params.Pattern.DEFAULT)]
        public string Pattern { set { pattern = value; } }

        [ActionParamField(Package.Params.NewPattern.DISPLAY, Package.Params.NewPattern.DESCRIPTION, false, Package.Params.NewPattern.DEFAULT)]
        public string NewPattern { set { newPattern = value; } }

        [ActionParamField(Package.Params.PatternPrecedence.DISPLAY, Package.Params.PatternPrecedence.DESCRIPTION, false, Package.Params.PatternPrecedence.DEFAULT)]
        public XPatternPrecedence PatternPrecedence { set { patternPrecedence = value; } }

        [ActionParamField(Package.Params.PatternPrecedenceSpecified.DISPLAY, Package.Params.PatternPrecedenceSpecified.DESCRIPTION, false, Package.Params.PatternPrecedenceSpecified.DEFAULT)]
        public bool PatternPrecedenceSpecified { set { patternPrecedenceSpecified = value; } }

        [ActionParamField(Package.Params.ReleaseCause.DISPLAY, Package.Params.ReleaseCause.DESCRIPTION, false, Package.Params.ReleaseCause.DEFAULT)]
        public XReleaseCauseValue ReleaseCause { set { releaseCause = value; } }

        [ActionParamField(Package.Params.ReleaseCauseSpecified.DISPLAY, Package.Params.ReleaseCauseSpecified.DESCRIPTION, false, Package.Params.ReleaseCauseSpecified.DEFAULT)]
        public bool ReleaseCauseSpecified { set { releaseCauseSpecified = value; } }

        [ActionParamField(Package.Params.UserHoldMOHAudioSourceId.DISPLAY, Package.Params.UserHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.UserHoldMOHAudioSourceId.DEFAULT)]
        public string UserHoldMOHAudioSourceId { set { userHoldMOHAudioSourceId = value; } }

        [ActionParamField(Package.Params.Uuid.DISPLAY, Package.Params.Uuid.DESCRIPTION, false, Package.Params.Uuid.DEFAULT)]
        public string Uuid { set { uuid = value; } }

        [ActionParamField(Package.Params.CallForwardAll.DISPLAY, Package.Params.CallForwardAll.DESCRIPTION, false, Package.Params.CallForwardAll.DEFAULT)]
        public CallForward CallForwardAll { set { callForwardAll = value; } }

        [ActionParamField(Package.Params.CallForwardBusy.DISPLAY, Package.Params.CallForwardBusy.DESCRIPTION, false, Package.Params.CallForwardBusy.DEFAULT)]
        public CallForward CallForwardBusy { set { callForwardBusy = value; } }
 
        [ActionParamField(Package.Params.CallForwardNoAnswer.DISPLAY, Package.Params.CallForwardNoAnswer.DESCRIPTION, false, Package.Params.CallForwardNoAnswer.DEFAULT)]
        public CallForward CallForwardNoAnswer { set { callForwardNoAnswer = value; } }

        [ActionParamField(Package.Params.CallForwardAlternateParty.DISPLAY, Package.Params.CallForwardAlternateParty.DESCRIPTION, false, Package.Params.CallForwardAlternateParty.DEFAULT)]
        public CallForward CallForwardAlternateParty { set { callForwardAlternateParty = value; } }

        [ActionParamField(Package.Params.CallForwardOnFailure.DISPLAY, Package.Params.CallForwardOnFailure.DESCRIPTION, false, Package.Params.CallForwardOnFailure.DEFAULT)]
        public CallForward CallForwardOnFailure { set { callForwardOnFailure = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.UpdateLineResponse.DISPLAY, Package.Results.UpdateLineResponse.DESCRIPTION)]
        public updateLineResponse UpdateLineResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private XAutoAnswer autoAnswer;
        private bool autoAnswerSpecified;
        private string callPickupGroupId;
        private string callPickupGroupName;
        private string description;
        private string routePartitionName;
        private string routePartitionId;
        private string newRoutePartitionName;
        private string newRoutePartitionId;
        private string aarNeighborhoodName;
        private string aarNeighborhoodId;
        private string shareLineAppearanceCSSName;
        private string shareLineAppearanceCSSId;
        private string voiceMailProfileName;
        private string voiceMailProfileId;
        private string routeFilterName;
        private string routeFilterId;
        private string networkHoldMOHAudioSourceId;
        private string newPattern;
        private string pattern;
        private XPatternPrecedence patternPrecedence;
        private bool patternPrecedenceSpecified;
        private XReleaseCauseValue releaseCause;
        private bool releaseCauseSpecified;
        private string userHoldMOHAudioSourceId;
        private string uuid;
        private CallForward callForwardAll;
        private CallForward callForwardAlternateParty;
        private CallForward callForwardBusy;
        private CallForward callForwardNoAnswer;
        private CallForward callForwardOnFailure;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        private updateLineResponse response;

        public UpdateLine()
        {
            Clear();	
        }

        public void Clear()
        {
            this.aarNeighborhoodId          = null;
            this.aarNeighborhoodName        = null;
            this.autoAnswer                 = XAutoAnswer.AutoAnswerOff;
            this.autoAnswerSpecified        = false;
            this.callPickupGroupId          = null;
            this.callPickupGroupName        = null;
            this.description                = null;
            this.networkHoldMOHAudioSourceId= null;
            this.newPattern                 = null;
            this.newRoutePartitionId        = null;
            this.newRoutePartitionName      = null;
            this.pattern                    = null;
            this.patternPrecedence          = XPatternPrecedence.Default;
            this.patternPrecedenceSpecified = false;
            this.releaseCause               = XReleaseCauseValue.NoError;
            this.releaseCauseSpecified      = false;
            this.routeFilterId              = null;
            this.routeFilterName            = String.Empty;
            this.routePartitionId           = null;
            this.routePartitionName         = String.Empty;
            this.shareLineAppearanceCSSId   = null;
            this.shareLineAppearanceCSSName = null;
            this.userHoldMOHAudioSourceId   = null;
            this.uuid                       = null;
            this.callForwardAll             = null;
            this.callForwardAlternateParty  = null;
            this.callForwardBusy            = null;
            this.callForwardNoAnswer        = null;
            this.callForwardOnFailure       = null;
            this.voiceMailProfileId         = null;
            this.voiceMailProfileName       = null;
            this.username                   = IAxlSoap.DefaultCcmAdmin;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new updateLineResponse();
            this.message                    = String.Empty;
            this.code                       = 0;
        }

        public bool ValidateInput()
        {
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
                         
            updateLine line = new updateLine();

            // line.Item (routePartition), line.Item5 (routeFilter), line.pattern (line number) all act as identifiers 
            // for the line to identify.
            // However, if uuid is specified (which is also an identifier), then routePartition, routeFilter, and pattern
            // can not be specified... not even such as <routePartitionName />.  
            // The element must not be even present in the XML.
            line.autoAnswer = autoAnswer;
            line.autoAnswerSpecified = false;
            line.description = description;
            IAxlSoap.DetermineChosenBetweenStrings(routePartitionName, routePartitionId, ref line.routePartitionName, ref line.routePartitionId);
            IAxlSoap.DetermineChosenBetweenStrings(routeFilterName, routeFilterId, ref line.routeFilterName, ref line.routeFilterId);
            IAxlSoap.UuidOrPattern(pattern, uuid, ref line.routePartitionName, ref line.routeFilterName, ref line.uuid);

            line.Item1 = IAxlSoap.DetermineChosenBetweenStrings(newRoutePartitionName, newRoutePartitionId);
            line.Item1ElementName = (Item1ChoiceType3) IAxlSoap.DetermineChosenBetweenStringsType(
                newRoutePartitionName, newRoutePartitionId, Item1ChoiceType3.newRoutePartitionName, Item1ChoiceType3.newRoutePartitionId);
            line.Item2 = IAxlSoap.DetermineChosenBetweenStrings(aarNeighborhoodName, aarNeighborhoodId);
            line.Item2ElementName = (Item2ChoiceType1) IAxlSoap.DetermineChosenBetweenStringsType(
                aarNeighborhoodName, aarNeighborhoodId, Item2ChoiceType1.aarNeighborhoodName, Item2ChoiceType1.aarNeighborhoodId);
            line.Item3 = IAxlSoap.DetermineChosenBetweenStrings(callPickupGroupName, callPickupGroupId);
            line.Item3ElementName = (Item3ChoiceType1) IAxlSoap.DetermineChosenBetweenStringsType(
                callPickupGroupName, callPickupGroupId, Item3ChoiceType1.callPickupGroupName, Item3ChoiceType1.callPickupGroupId);
            line.Item4 = IAxlSoap.DetermineChosenBetweenStrings(shareLineAppearanceCSSName, shareLineAppearanceCSSId);
            line.Item4ElementName = (Item4ChoiceType1) IAxlSoap.DetermineChosenBetweenStringsType(
                shareLineAppearanceCSSName, shareLineAppearanceCSSId, Item4ChoiceType1.shareLineAppearanceCSSName, Item4ChoiceType1.shareLineAppearanceCSSId);
            line.Item5 = IAxlSoap.DetermineChosenBetweenStrings(voiceMailProfileName, voiceMailProfileId);
            line.Item5ElementName = (Item5ChoiceType1) IAxlSoap.DetermineChosenBetweenStringsType(
                voiceMailProfileName, voiceMailProfileId, Item5ChoiceType1.voiceMailProfileName, Item5ChoiceType1.voiceMailProfileId);
            line.networkHoldMOHAudioSourceId = networkHoldMOHAudioSourceId;
            line.newPattern = newPattern;
            line.pattern = uuid != null ? null : pattern;
            line.patternPrecedence = patternPrecedence;
            line.patternPrecedenceSpecified = patternPrecedenceSpecified;
            line.releaseCause = releaseCause;
            line.releaseCauseSpecified = releaseCauseSpecified;
            line.userHoldMOHAudioSourceId = userHoldMOHAudioSourceId;
            line.uuid = uuid;
            line.callForwardAll = callForwardAll != null ? callForwardAll.Data : null;
            line.callForwardAlternateParty = callForwardAlternateParty != null ? callForwardAlternateParty.Data : null;
            line.callForwardBusy = callForwardBusy != null ? callForwardBusy.Data : null;
            line.callForwardNoAnswer = callForwardNoAnswer != null ? callForwardNoAnswer.Data : null;
            line.callForwardOnFailure = callForwardOnFailure != null ? callForwardOnFailure.Data : null;

            CorrectCallForward(line.callForwardAll);
            CorrectCallForward(line.callForwardBusy);
            CorrectCallForward(line.callForwardNoAnswer);

            try
            {
                response = client.updateLine(line);
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

        protected void CorrectCallForward(XCallForwardInfo callForward)
        {
            if(callForward == null)
                return;

            // We must force destination tag to be present.  Otherwise, the 3.3.3 CCM Soap parser fails completely
            if(callForward.destination == null)
                callForward.destination = String.Empty;
        }
    }
}
