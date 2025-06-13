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

namespace Metreos.Native.AxlSoap413
{
    /// <summary> Copies lines from a getDeviceProfileResponse and transfers it to a line variable</summary>
    [PackageDecl("Metreos.Native.AxlSoap413")]
    public class CopyLines : INativeAction
    {    
        [ActionParamField(Package.Params.DeviceResponse.DISPLAY, Package.Params.DeviceResponse.DESCRIPTION, true, Package.Params.DeviceResponse.DEFAULT)]
        public object DeviceResponse { set { device = value; } }
        private object device;

        [ResultDataField("A line structure copied from the phone or device profile response")]
        public Metreos.AxlSoap413.XLine[] Lines { get { return lines; } }

        public LogWriter Log { set { log = value; } }

        private Metreos.AxlSoap413.XLine[] lines;
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

            Metreos.AxlSoap413.XLine[] newLines = null;


            if(device is Metreos.Types.AxlSoap333.GetDeviceProfileResponse)
            {
                Metreos.Types.AxlSoap333.GetDeviceProfileResponse deviceResponse = device as Metreos.Types.AxlSoap333.GetDeviceProfileResponse;

                if(	deviceResponse != null ||
                    deviceResponse.Response != null ||
                    deviceResponse.Response.@return != null ||
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
            else if(oldLines333 != null)
            {
                // Initialize the line data structure
                newLines = new Metreos.AxlSoap413.XLine[oldLines333.Length];
                
                for(int i = 0; i < oldLines333.Length; i++)
                {
                    Metreos.AxlSoap333.XLine oldLine = oldLines333[i] as Metreos.AxlSoap333.XLine;
                    Metreos.AxlSoap413.XLine newLine = new Metreos.AxlSoap413.XLine();

                    newLine.busyTrigger = oldLine.busyTrigger;
                    // newLine.callInfoDisplay = 4.1.3 only concept
                    newLine.consecutiveRingSetting = (Metreos.AxlSoap413.XRingSetting) Enum.Parse(typeof(Metreos.AxlSoap413.XRingSetting), oldLine.consecutiveRingSetting.ToString(), true);
                    newLine.ctiid = oldLine.ctiid;
                    newLine.dialPlanWizardId = oldLine.dialPlanWizardId;
                    newLine.display = oldLine.display;
                    newLine.e164Mask = oldLine.e164Mask;
                    newLine.index = oldLine.index;
                    newLine.Item = ExtractDirectoryNumber333(oldLine.Item);
                    newLine.label = oldLine.label;
                    newLine.maxNumCalls = oldLine.maxNumCalls;
                    newLine.mwlPolicy = (Metreos.AxlSoap413.XMWLPolicy) Enum.Parse(typeof(Metreos.AxlSoap413.XMWLPolicy), oldLine.mwlPolicy.ToString(), true);
                    newLine.mwlPolicySpecified = oldLine.mwlPolicySpecified;
                    newLine.ringSetting = (Metreos.AxlSoap413.XRingSetting) Enum.Parse(typeof(Metreos.AxlSoap413.XRingSetting), oldLine.ringSetting.ToString(), true);
                    newLine.uuid = oldLine.uuid;
                    newLines[i] = newLine;
                }

                lines = newLines;
            }
            else // if(oldLines413 != null)
            {
                newLines = new Metreos.AxlSoap413.XLine[oldLines413.Length];
                Array.Copy(oldLines413,
                    newLines, newLines.Length);

                lines = newLines;
            }

            return Result.success.ToString();
        }   

        protected Metreos.AxlSoap413.XNPDirectoryNumber ExtractDirectoryNumber333(Metreos.AxlSoap333.XNPDirectoryNumber oldNumber)
        {
            Metreos.AxlSoap413.XNPDirectoryNumber newNumber = null;
            if(oldNumber != null)
            {
                newNumber = new Metreos.AxlSoap413.XNPDirectoryNumber();
                //newNumber.alertingName  = 4.1.3 concept
                newNumber.autoAnswer = (Metreos.AxlSoap413.XAutoAnswer) Enum.Parse(typeof(Metreos.AxlSoap413.XAutoAnswer), oldNumber.autoAnswer.ToString(), true);
                newNumber.autoAnswerSpecified = oldNumber.autoAnswerSpecified;
                newNumber.callForwardAll = ExtractCallForward333(oldNumber.callForwardAll);
                newNumber.callForwardAlternateParty = ExtractCallForward333(oldNumber.callForwardAlternateParty);
                newNumber.callForwardBusy = ExtractCallForward333(oldNumber.callForwardBusy);
                //newNumber.callForwardBusyInt = 4.1.3 concept
                newNumber.callForwardNoAnswer = ExtractCallForward333(oldNumber.callForwardNoAnswer);
                //newNumber.callForwardNoAnswerInt = 4.1.3 concept
                //newNumber.callForwardNoCoverage = 4.1.3 concept
                //newNumber.callForwardNoCoverageInt = 4.1.3 concept
                newNumber.callForwardOnFailure = ExtractCallForward333(oldNumber.callForwardOnFailure);
                newNumber.cfaCSSClause = oldNumber.cfaCSSClause;
                newNumber.description = oldNumber.description;

                if(oldNumber.Item is Metreos.AxlSoap333.XRoutePartition)
                {
                    Metreos.AxlSoap333.XRoutePartition oldRp = oldNumber.Item as Metreos.AxlSoap333.XRoutePartition;
                    Metreos.AxlSoap413.XRoutePartition newRp = new Metreos.AxlSoap413.XRoutePartition();
                    newRp.uuid = oldRp.uuid;
                    newNumber.Item = newRp;
                }
                else // .Item == string
                {
                    newNumber.Item = oldNumber.Item; 
                }

                if(oldNumber.Item1 is Metreos.AxlSoap333.XAARNeighborhood)
                {
                    Metreos.AxlSoap333.XAARNeighborhood oldAar = oldNumber.Item1 as Metreos.AxlSoap333.XAARNeighborhood;
                    Metreos.AxlSoap413.XAARNeighborhood newAar = new Metreos.AxlSoap413.XAARNeighborhood();
                    newAar.uuid = oldAar.uuid;
                    newNumber.Item1 = newAar;
                }
                else // .Item1 == string
                {
                    newNumber.Item1 = oldNumber.Item1; 
                }

//               Item2 == 4.1.3 Concept (Call Pickup Group)

                if(oldNumber.Item2 is Metreos.AxlSoap333.XNPDirectoryNumberShareLineAppearanceCSS)
                {
                    Metreos.AxlSoap333.XNPDirectoryNumberShareLineAppearanceCSS oldSharedCss = oldNumber.Item2 as Metreos.AxlSoap333.XNPDirectoryNumberShareLineAppearanceCSS;
                    Metreos.AxlSoap413.XNPDirectoryNumberShareLineAppearanceCSS newSharedCss = new Metreos.AxlSoap413.XNPDirectoryNumberShareLineAppearanceCSS();
                    newSharedCss.uuid = oldSharedCss.uuid;
                    newNumber.Item3 = newSharedCss;
                }
                else // .Item3 == string
                {
                    newNumber.Item3 = oldNumber.Item2; 
                }

                if(oldNumber.Item3 is Metreos.AxlSoap333.XVoiceMailProfile)
                {
                    Metreos.AxlSoap333.XVoiceMailProfile oldVmProfile = oldNumber.Item3 as Metreos.AxlSoap333.XVoiceMailProfile;
                    Metreos.AxlSoap413.XVoiceMailProfile newVmProfile = new Metreos.AxlSoap413.XVoiceMailProfile();
                    newVmProfile.uuid = oldVmProfile.uuid;
                    newNumber.Item4 = newVmProfile;
                }
                else // .Item4 == string
                {
                    newNumber.Item4 = oldNumber.Item3; 
                }

                newNumber.networkHoldMOHAudioSourceId = oldNumber.networkHoldMOHAudioSourceId;
                newNumber.pattern = oldNumber.pattern;
                newNumber.patternPrecedence = (Metreos.AxlSoap413.XPatternPrecedence) Enum.Parse(typeof(Metreos.AxlSoap413.XPatternPrecedence), oldNumber.patternPrecedence.ToString(), true); // ExecutiveOverride unique in 4.1.3
                newNumber.patternPrecedenceSpecified = oldNumber.patternPrecedenceSpecified;
                newNumber.releaseCause = (Metreos.AxlSoap413.XReleaseCauseValue) Enum.Parse(typeof(Metreos.AxlSoap413.XReleaseCauseValue), oldNumber.releaseCause.ToString(), true);
                newNumber.releaseCauseSpecified = oldNumber.releaseCauseSpecified;
                newNumber.usage = (Metreos.AxlSoap413.XPatternUsage) Enum.Parse(typeof(Metreos.AxlSoap413.XPatternUsage), oldNumber.usage.ToString(), true);  //Hunt unique in 4.1.3
                newNumber.userHoldMOHAudioSourceId = oldNumber.userHoldMOHAudioSourceId;
                newNumber.uuid = oldNumber.uuid;
            }
            return newNumber;
        }

        protected Metreos.AxlSoap413.XCallForwardInfo ExtractCallForward333(Metreos.AxlSoap333.XCallForwardInfo callForwardInfo)
        {
            Metreos.AxlSoap413.XCallForwardInfo newCfInfo = null;
            if(callForwardInfo != null)
            {
                newCfInfo.destination = callForwardInfo.destination;
                newCfInfo.duration = callForwardInfo.duration;
                newCfInfo.forwardToVoiceMail = callForwardInfo.forwardToVoiceMail;

                if(callForwardInfo.Item is Metreos.AxlSoap333.XCallingSearchSpace)
                {
                    Metreos.AxlSoap333.XCallingSearchSpace oldCss = callForwardInfo.Item as Metreos.AxlSoap333.XCallingSearchSpace;
                    Metreos.AxlSoap413.XCallingSearchSpace newCss = new Metreos.AxlSoap413.XCallingSearchSpace();
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
