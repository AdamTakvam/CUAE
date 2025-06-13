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
using Metreos.AxlSoap413;
using Metreos.Types.AxlSoap333;
using Metreos.Types.AxlSoap413;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333.Actions.CopyLines;

namespace Metreos.Native.AxlSoap333
{
    /// <summary> Copies lines from a getDeviceProfileResponse and transfers it to a line variable</summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.PACKAGE_DESCRIPTION)]
    public class CopyLines : INativeAction
    {    
        [ActionParamField(Package.Params.DeviceResponse.DISPLAY, Package.Params.DeviceResponse.DESCRIPTION, true, Package.Params.DeviceResponse.DEFAULT)]
        public object DeviceResponse { set { device = value; } }
        private object device;

        [ResultDataField("A line structure copied from the phone or device profile response")]
        public Metreos.AxlSoap333.XLine[] Lines { get { return lines; } }

        public LogWriter Log { set { log = value; } }

        private Metreos.AxlSoap333.XLine[] lines;
        private LogWriter log;
        
        public CopyLines()
        {
            Clear();	
        }

        public void Clear()
        {
            this.device = null;
            this.lines = null;
        }

        public bool ValidateInput()
        {
            return true;
        } 

        public enum Result
        {
            success,
            nolines,
            failure
        }

        [ReturnValue(typeof(Result), "Success, no lines, or failure")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            object[] oldLines333 = null;
            object[] oldLines413 = null;

            Metreos.AxlSoap333.XLine[] newLines = null;

            if(device is Metreos.Types.AxlSoap333.GetDeviceProfileResponse)
            {
                Metreos.Types.AxlSoap333.GetDeviceProfileResponse deviceResponse = device as Metreos.Types.AxlSoap333.GetDeviceProfileResponse;

                if(	deviceResponse != null ||
                    deviceResponse.Response != null ||
                    deviceResponse.Response.@return == null ||
                    deviceResponse.Response.@return.profile != null ||
                    deviceResponse.Response.@return.profile.lines != null ||
                    deviceResponse.Response.@return.profile.lines.Items != null ||
                    deviceResponse.Response.@return.profile.lines.Items.Length != 0) 
                {
                    oldLines333 = deviceResponse.Response.@return.profile.lines.Items as object[];
                }
            }
            else if(device is Metreos.Types.AxlSoap333.GetPhoneResponse)
            {
                Metreos.Types.AxlSoap333.GetPhoneResponse deviceResponse = device as Metreos.Types.AxlSoap333.GetPhoneResponse;
                if(	deviceResponse != null ||
                    deviceResponse.Response != null ||
                    deviceResponse.Response.@return != null ||
                    deviceResponse.Response.@return.device != null ||
                    deviceResponse.Response.@return.device.lines != null ||
                    deviceResponse.Response.@return.device.lines.Items != null ||
                    deviceResponse.Response.@return.device.lines.Items.Length != 0) 
                {
                    oldLines333 = deviceResponse.Response.@return.device.lines.Items as object[];
                }
            }
            else if(device is Metreos.Types.AxlSoap413.GetDeviceProfileResponse)
            {
                Metreos.Types.AxlSoap413.GetDeviceProfileResponse deviceResponse = device as Metreos.Types.AxlSoap413.GetDeviceProfileResponse;
                if(	deviceResponse != null ||
                    deviceResponse.Response != null ||
                    deviceResponse.Response.@return != null ||
                    deviceResponse.Response.@return.profile != null ||
                    deviceResponse.Response.@return.profile.lines != null ||
                    deviceResponse.Response.@return.profile.lines.Items != null ||
                    deviceResponse.Response.@return.profile.lines.Items.Length != 0) 
                {
                    oldLines413 = deviceResponse.Response.@return.profile.lines.Items as object[];
                }
            }
            else if(device is Metreos.Types.AxlSoap413.GetPhoneResponse)
            {
                Metreos.Types.AxlSoap413.GetPhoneResponse deviceResponse = device as Metreos.Types.AxlSoap413.GetPhoneResponse;
                if(	deviceResponse != null ||
                    deviceResponse.Response != null ||
                    deviceResponse.Response.@return != null ||
                    deviceResponse.Response.@return.device != null ||
                    deviceResponse.Response.@return.device.lines != null ||
                    deviceResponse.Response.@return.device.lines.Items != null ||
                    deviceResponse.Response.@return.device.lines.Items.Length != 0) 
                {
                    oldLines413 = deviceResponse.Response.@return.device.lines.Items as object[];
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "Unable to determine the type of the device response.  Type={0}", device == null ? "NULL" : device);
                return Result.failure.ToString();
            }

            if(oldLines333 == null && oldLines413 == null)
            {
                return Result.nolines.ToString();
            }
            else if(oldLines413 != null)
            {
                // Initialize the line data structure
                newLines = new Metreos.AxlSoap333.XLine[oldLines413.Length];
                
                for(int i = 0; i < oldLines413.Length; i++)
                {
                    Metreos.AxlSoap413.XLine oldLine = oldLines413[i] as Metreos.AxlSoap413.XLine;
                    Metreos.AxlSoap333.XLine newLine = new Metreos.AxlSoap333.XLine();

                    newLine.busyTrigger = oldLine.busyTrigger;
                    // newLine.callInfoDisplay = 4.1.3 only concept
                    newLine.consecutiveRingSetting = (Metreos.AxlSoap333.XRingSetting) Enum.Parse(typeof(Metreos.AxlSoap333.XRingSetting), oldLine.consecutiveRingSetting.ToString(), true);
                    newLine.ctiid = oldLine.ctiid;
                    newLine.dialPlanWizardId = oldLine.dialPlanWizardId;
                    newLine.display = oldLine.display;
                    newLine.e164Mask = oldLine.e164Mask;
                    newLine.index = oldLine.index;
                    newLine.Item = ExtractDirectoryNumber413(oldLine.Item);
                    newLine.label = oldLine.label;
                    newLine.maxNumCalls = oldLine.maxNumCalls;
                    newLine.mwlPolicy = (Metreos.AxlSoap333.XMWLPolicy) Enum.Parse(typeof(Metreos.AxlSoap333.XMWLPolicy), oldLine.mwlPolicy.ToString(), true);
                    newLine.mwlPolicySpecified = oldLine.mwlPolicySpecified;
                    newLine.ringSetting = (Metreos.AxlSoap333.XRingSetting) Enum.Parse(typeof(Metreos.AxlSoap333.XRingSetting), oldLine.ringSetting.ToString(), true);
                    newLine.uuid = oldLine.uuid;
                    newLines[i] = newLine;
                }

                lines = newLines;
            }
            else // if(oldLines333 != null)
            {
                newLines = new Metreos.AxlSoap333.XLine[oldLines413.Length];
                Array.Copy(oldLines413,
                    newLines, newLines.Length);

                lines = newLines;
            }

            return Result.success.ToString();
        }   

        protected Metreos.AxlSoap333.XNPDirectoryNumber ExtractDirectoryNumber413(Metreos.AxlSoap413.XNPDirectoryNumber oldNumber)
        {
            Metreos.AxlSoap333.XNPDirectoryNumber newNumber = null;
            if(oldNumber != null)
            {
                newNumber = new Metreos.AxlSoap333.XNPDirectoryNumber();
                //newNumber.alertingName  = 4.1.3 concept
                newNumber.autoAnswer = (Metreos.AxlSoap333.XAutoAnswer) Enum.Parse(typeof(Metreos.AxlSoap333.XAutoAnswer), oldNumber.autoAnswer.ToString(), true);
                newNumber.autoAnswerSpecified = oldNumber.autoAnswerSpecified;
                newNumber.callForwardAll = ExtractCallForward413(oldNumber.callForwardAll);
                newNumber.callForwardAlternateParty = ExtractCallForward413(oldNumber.callForwardAlternateParty);
                newNumber.callForwardBusy = ExtractCallForward413(oldNumber.callForwardBusy);
                //newNumber.callForwardBusyInt = 4.1.3 concept
                newNumber.callForwardNoAnswer = ExtractCallForward413(oldNumber.callForwardNoAnswer);
                //newNumber.callForwardNoAnswerInt = 4.1.3 concept
                //newNumber.callForwardNoCoverage = 4.1.3 concept
                //newNumber.callForwardNoCoverageInt = 4.1.3 concept
                newNumber.callForwardOnFailure = ExtractCallForward413(oldNumber.callForwardOnFailure);
                newNumber.cfaCSSClause = oldNumber.cfaCSSClause;
                newNumber.description = oldNumber.description;

                if(oldNumber.Item is Metreos.AxlSoap413.XRoutePartition)
                {
                    Metreos.AxlSoap413.XRoutePartition oldRp = oldNumber.Item as Metreos.AxlSoap413.XRoutePartition;
                    Metreos.AxlSoap333.XRoutePartition newRp = new Metreos.AxlSoap333.XRoutePartition();
                    newRp.uuid = oldRp.uuid;
                    newNumber.Item = newRp;
                }
                else // .Item == string
                {
                    newNumber.Item = oldNumber.Item; 
                }

                if(oldNumber.Item1 is Metreos.AxlSoap413.XAARNeighborhood)
                {
                    Metreos.AxlSoap413.XAARNeighborhood oldAar = oldNumber.Item1 as Metreos.AxlSoap413.XAARNeighborhood;
                    Metreos.AxlSoap333.XAARNeighborhood newAar = new Metreos.AxlSoap333.XAARNeighborhood();
                    newAar.uuid = oldAar.uuid;
                    newNumber.Item1 = newAar;
                }
                else // .Item1 == string
                {
                    newNumber.Item1 = oldNumber.Item1; 
                }

//               Item2 == 4.1.3 Concept (Call Pickup Group)

                if(oldNumber.Item3 is Metreos.AxlSoap413.XNPDirectoryNumberShareLineAppearanceCSS)
                {
                    Metreos.AxlSoap413.XNPDirectoryNumberShareLineAppearanceCSS oldSharedCss = oldNumber.Item3 as Metreos.AxlSoap413.XNPDirectoryNumberShareLineAppearanceCSS;
                    Metreos.AxlSoap333.XNPDirectoryNumberShareLineAppearanceCSS newSharedCss = new Metreos.AxlSoap333.XNPDirectoryNumberShareLineAppearanceCSS();
                    newSharedCss.uuid = oldSharedCss.uuid;
                    newNumber.Item2 = newSharedCss;
                }
                else // .Item2 == string
                {
                    newNumber.Item2 = oldNumber.Item3; 
                }

                if(oldNumber.Item4 is Metreos.AxlSoap413.XVoiceMailProfile)
                {
                    Metreos.AxlSoap413.XVoiceMailProfile oldVmProfile = oldNumber.Item4 as Metreos.AxlSoap413.XVoiceMailProfile;
                    Metreos.AxlSoap333.XVoiceMailProfile newVmProfile = new Metreos.AxlSoap333.XVoiceMailProfile();
                    newVmProfile.uuid = oldVmProfile.uuid;
                    newNumber.Item3 = newVmProfile;
                }
                else // .Item3 == string
                {
                    newNumber.Item3 = oldNumber.Item4; 
                }

                newNumber.networkHoldMOHAudioSourceId = oldNumber.networkHoldMOHAudioSourceId;
                newNumber.pattern = oldNumber.pattern;
                if(oldNumber.patternPrecedence != Metreos.AxlSoap413.XPatternPrecedence.ExecutiveOverride)
                {
                    newNumber.patternPrecedence = (Metreos.AxlSoap333.XPatternPrecedence) Enum.Parse(typeof(Metreos.AxlSoap333.XPatternPrecedence), oldNumber.patternPrecedence.ToString(), true); // ExecutiveOverride unique in 4.1.3
                }
                newNumber.patternPrecedenceSpecified = oldNumber.patternPrecedenceSpecified;
                newNumber.releaseCause = (Metreos.AxlSoap333.XReleaseCauseValue) Enum.Parse(typeof(Metreos.AxlSoap333.XReleaseCauseValue), oldNumber.releaseCause.ToString(), true);
                newNumber.releaseCauseSpecified = oldNumber.releaseCauseSpecified;
                if(oldNumber.usage != Metreos.AxlSoap413.XPatternUsage.Hunt)
                {
                    newNumber.usage = (Metreos.AxlSoap333.XPatternUsage) Enum.Parse(typeof(Metreos.AxlSoap333.XPatternUsage), oldNumber.usage.ToString(), true);  //Hunt unique in 4.1.3
                }
                newNumber.userHoldMOHAudioSourceId = oldNumber.userHoldMOHAudioSourceId;
                newNumber.uuid = oldNumber.uuid;
            }
            return newNumber;
        }

        protected Metreos.AxlSoap333.XCallForwardInfo ExtractCallForward413(Metreos.AxlSoap413.XCallForwardInfo callForwardInfo)
        {
            Metreos.AxlSoap333.XCallForwardInfo newCfInfo = null;
            if(callForwardInfo != null)
            {
                newCfInfo.destination = callForwardInfo.destination;
                newCfInfo.duration = callForwardInfo.duration;
                newCfInfo.forwardToVoiceMail = callForwardInfo.forwardToVoiceMail;

                if(callForwardInfo.Item is Metreos.AxlSoap413.XCallingSearchSpace)
                {
                    Metreos.AxlSoap413.XCallingSearchSpace oldCss = callForwardInfo.Item as Metreos.AxlSoap413.XCallingSearchSpace;
                    Metreos.AxlSoap333.XCallingSearchSpace newCss = new Metreos.AxlSoap333.XCallingSearchSpace();
                    newCss.uuid = oldCss.uuid;
                    newCfInfo.Item = newCss;
                }
                else // .Item == string
                {
                    newCfInfo.Item = callForwardInfo.Item; 
                }
            }
            return newCfInfo;
        }
    }
}
