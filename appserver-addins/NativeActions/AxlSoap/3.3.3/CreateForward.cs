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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333.Actions.CreateForward;

namespace Metreos.Native.AxlSoap333
{
    /// <summary> 
    ///     Creates the forwarding information for a forward attribute for a line, 
    ///     to be later used with an 'updateLine' AXL SOAP method for Cisco CallManager 3.3.3 
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.PACKAGE_DESCRIPTION)]
    public class CreateForward : INativeAction
    {    
        [ActionParamField(Package.Params.Destination.DISPLAY, Package.Params.Destination.DESCRIPTION, false, Package.Params.Destination.DEFAULT)]
        public string Destination { set { destination = value; } }

        [ActionParamField(Package.Params.Duration.DISPLAY, Package.Params.Duration.DESCRIPTION, false, Package.Params.Duration.DEFAULT)]
        public string Duration { set { duration = value; } }

        [ActionParamField(Package.Params.ForwardToVoiceMail.DISPLAY, Package.Params.ForwardToVoiceMail.DESCRIPTION, false, Package.Params.ForwardToVoiceMail.DEFAULT)]
        public bool ForwardToVoiceMail { set { forwardToVoiceMail = value; forwardToVoiceMailSpecified = true; } }

        [ActionParamField(Package.Params.CallingSearchSpaceId.DISPLAY, Package.Params.CallingSearchSpaceId.DESCRIPTION, false, Package.Params.CallingSearchSpaceId.DEFAULT)]
        public string CallingSearchSpaceId { set { callingSearchSpaceId = value; } }

        [ActionParamField(Package.Params.CallingSearchSpaceName.DISPLAY, Package.Params.CallingSearchSpaceName.DESCRIPTION, false, Package.Params.CallingSearchSpaceName.DEFAULT)]
        public string CallingSearchSpaceName { set { callingSearchSpaceName = value; } }

        [ResultDataField(Package.Results.Forward.DISPLAY, Package.Results.Forward.DESCRIPTION)]
        public XCallForwardInfo Forward { get { return forward; } }

        public LogWriter Log { set { log = value; } }

        private string destination;
        private string duration;
        private bool forwardToVoiceMail;
        private bool forwardToVoiceMailSpecified;
        private string callingSearchSpaceId;
        private string callingSearchSpaceName;

        private XCallForwardInfo forward;
        private LogWriter log;
        
        public CreateForward()
        {
            Clear();	
        }

        public void Clear()
        {
            this.destination = null;
            this.duration = null;
            this.forwardToVoiceMail = false;
            this.forwardToVoiceMailSpecified = false;
            this.callingSearchSpaceId = null;
            this.callingSearchSpaceName = null;
            this.forward = new XCallForwardInfo();
        }

        public bool ValidateInput()
        {
            return true;
        } 

        public enum Result
        {
            success
        }

        [ReturnValue(typeof(Result), "Only success")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {  
            forward = new XCallForwardInfo();
            forward.destination = destination;
            forward.duration = duration;
            forward.forwardToVoiceMail = forwardToVoiceMail;
            forward.forwardToVoiceMailSpecified = forwardToVoiceMailSpecified;
            
            if(callingSearchSpaceId != null)
            {
                XCallingSearchSpace css = new XCallingSearchSpace();
                css.uuid = callingSearchSpaceId;
                forward.Item = css;
                forward.ItemElementName = ItemChoiceType70.callingSearchSpace;
            }
            else
            {
                forward.Item = callingSearchSpaceName;
                forward.ItemElementName = ItemChoiceType70.callingSearchSpaceName;
            }

            return IApp.VALUE_SUCCESS;
        }   
    }
}
