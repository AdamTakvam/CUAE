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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap413.Actions.AddSpeeddialItem;

namespace Metreos.Native.AxlSoap413
{
    /// <summary> Creates a speeddial item for the 'updatePhone' AXL SOAP method for Cisco CallManager 4.1.3 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.PACKAGE_DESCRIPTION)]
    public class AddSpeeddialItem : INativeAction
    {    
        [ActionParamField(Package.Params.DirectoryNumber.DISPLAY, Package.Params.DirectoryNumber.DESCRIPTION, false, Package.Params.DirectoryNumber.DEFAULT)]
        public string DirectoryNumber { set { directoryNumber = value; } }

        [ActionParamField(Package.Params.Label.DISPLAY, Package.Params.Label.DESCRIPTION, false, Package.Params.Label.DEFAULT)]
        public string Label { set { label = value; } }

        [ActionParamField(Package.Params.Index.DISPLAY, Package.Params.Index.DESCRIPTION, false, Package.Params.Index.DEFAULT)]
        public string Index { set { index = value; } }

        [ResultDataField(Package.Results.Speeddial.DISPLAY, Package.Results.Speeddial.DESCRIPTION)]
        public XSpeeddial Speeddial { get { return speeddial; } }

        public LogWriter Log { set { log = value; } }

        private string directoryNumber;
        private string label;
        private string index;
        private XSpeeddial speeddial;
        private LogWriter log;
        
        public AddSpeeddialItem()
        {
            Clear();	
        }

        public void Clear()
        {
            directoryNumber = null;
            index = null;
            label = null;
            speeddial = new XSpeeddial();
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
            speeddial = new XSpeeddial();
            speeddial.dirn = directoryNumber;
            speeddial.index = index;
            speeddial.label = label;

            return IApp.VALUE_SUCCESS;
        }   
    }
}
