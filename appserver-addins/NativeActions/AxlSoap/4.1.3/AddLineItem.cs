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
using Metreos.AxlSoap413;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap413.Actions.AddLineItem;

namespace Metreos.Native.AxlSoap413
{
    /// <summary> Creates a line item for the 'updatePhone' AXL SOAP method for Cisco CallManager 4.1.3 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.PACKAGE_DESCRIPTION)]
    public class AddLineItem : INativeAction
    {    
        [ActionParamField(Package.Params.BusyTrigger.DISPLAY, Package.Params.BusyTrigger.DESCRIPTION, false, Package.Params.BusyTrigger.DEFAULT)]
        public string BusyTrigger { set { busyTrigger = value; } }

        [ActionParamField(Package.Params.ConsecutiveRingSetting.DISPLAY, Package.Params.ConsecutiveRingSetting.DESCRIPTION, false, Package.Params.ConsecutiveRingSetting.DEFAULT)]
        public XRingSetting ConsecutiveRingSetting { set { consecutiveRingSetting = value; } }

        [ActionParamField(Package.Params.Ctiid.DISPLAY, Package.Params.Ctiid.DESCRIPTION, false, Package.Params.Ctiid.DEFAULT)]
        public string Ctiid { set { ctiid = value; } }

        [ActionParamField(Package.Params.DialPlanWizardId.DISPLAY, Package.Params.DialPlanWizardId.DESCRIPTION, false, Package.Params.DialPlanWizardId.DEFAULT)]
        public string DialPlanWizardId { set { dialPlanWizardId = value; } }

        [ActionParamField(Package.Params.Display.DISPLAY, Package.Params.Display.DESCRIPTION, false, Package.Params.Display.DEFAULT)]
        public string Display { set { display = value; } }

        [ActionParamField(Package.Params.E164Mask.DISPLAY, Package.Params.E164Mask.DESCRIPTION, false, Package.Params.E164Mask.DEFAULT)]
        public string E164Mask { set { e164Mask = value; } }

        [ActionParamField(Package.Params.Index.DISPLAY, Package.Params.Index.DESCRIPTION, false, Package.Params.Index.DEFAULT)]
        public string Index { set { index = value; } }

        [ActionParamField(Package.Params.Label.DISPLAY, Package.Params.Label.DESCRIPTION, false, Package.Params.Label.DEFAULT)]
        public string Label { set { label = value; } }

        [ActionParamField(Package.Params.MaxNumCalls.DISPLAY, Package.Params.MaxNumCalls.DESCRIPTION, false, Package.Params.MaxNumCalls.DEFAULT)]
        public string MaxNumCalls { set { maxNumCalls = value; } }

        [ActionParamField(Package.Params.MwlPolicy.DISPLAY, Package.Params.MwlPolicy.DESCRIPTION, false, Package.Params.MwlPolicy.DEFAULT)]
        public XMWLPolicy MwlPolicy { set { mwlPolicy = value; } }

        [ActionParamField(Package.Params.MwlPolicySpecified.DISPLAY, Package.Params.MwlPolicySpecified.DESCRIPTION, false, Package.Params.MwlPolicySpecified.DEFAULT)]
        public bool MwlPolicySpecified { set { mwlPolicySpecified = value; } }

        [ActionParamField(Package.Params.RingSetting.DISPLAY, Package.Params.RingSetting.DESCRIPTION, false, Package.Params.RingSetting.DEFAULT)]
        public XRingSetting RingSetting { set { ringSetting = value; } }

        [ActionParamField(Package.Params.DirectoryNumberId.DISPLAY, Package.Params.DirectoryNumberId.DESCRIPTION, false, Package.Params.DirectoryNumberId.DEFAULT)]
        public string DirectoryNumberId { set { directoryNumberId = value; } }
      
        [ActionParamField(Package.Params.Uuid.DISPLAY, Package.Params.Uuid.DESCRIPTION, false, Package.Params.Uuid.DEFAULT)]
        public string Uuid { set { uuid = value; } }

        [ResultDataField(Package.Results.Line.DISPLAY, Package.Results.Line.DESCRIPTION)]
        public XLine Line { get { return line; } }

        public LogWriter Log { set { log = value; } }

        private string busyTrigger;
        private XRingSetting consecutiveRingSetting;
        private string ctiid;
        private string dialPlanWizardId;
        private string display;
        private string e164Mask;
        private string index;
        private string label;
        private string maxNumCalls;
        private XMWLPolicy mwlPolicy;
        private bool mwlPolicySpecified;
        private XRingSetting ringSetting;
        private string directoryNumberId;
        private string uuid;
        private XLine line;
        private LogWriter log;
        
        public AddLineItem()
        {
            Clear();	
        }

        public void Clear()
        {
            this.busyTrigger                = null;
            this.consecutiveRingSetting     = XRingSetting.UseSystemDefault;
            this.ctiid                      = null;
            this.dialPlanWizardId           = null;
            this.display                    = null;
            this.e164Mask                   = null;
            this.index                      = null;
            this.label                      = null;
            this.maxNumCalls                = null;
            this.mwlPolicy                  = XMWLPolicy.UseSystemPolicy;
            this.mwlPolicySpecified         = false;
            this.directoryNumberId          = null;
            this.uuid                       = null;
            this.line                       = new XLine();
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
            line = new XLine();
            line.busyTrigger = busyTrigger;
            line.consecutiveRingSetting = consecutiveRingSetting;
            line.ctiid = ctiid;
            line.dialPlanWizardId = dialPlanWizardId;
            line.display = display;
            line.e164Mask = e164Mask;
            line.index = index;
            line.label = label;
            line.maxNumCalls = maxNumCalls;
            line.mwlPolicy = mwlPolicy;
            line.mwlPolicySpecified = mwlPolicySpecified;
            line.ringSetting = ringSetting;
            line.uuid = uuid;
            
            if(directoryNumberId != null)
            {
                XNPDirectoryNumber dirn = new XNPDirectoryNumber();
                dirn.uuid = directoryNumberId;
                line.Item = dirn;
            }
            
            return IApp.VALUE_SUCCESS;
        }   
    }
}
