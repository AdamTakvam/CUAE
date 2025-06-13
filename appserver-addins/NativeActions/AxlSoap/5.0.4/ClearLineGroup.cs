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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504.Actions.ClearLineGroup;

namespace Metreos.Native.AxlSoap504
{
    /// <summary> Clears all line group items for a 'updateLineGroup' AXL SOAP method for Cisco CallManager 5.0.4 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.PACKAGE_DESCRIPTION)]
    public class ClearLineGroup : INativeAction
    {    
        [ResultDataField("The information for this line")]
        public XLineGroupMember[] LineGroupMembers { get { return clear; } }

        public LogWriter Log { set { log = value; } }

        private XLineGroupMember[] clear;
        private LogWriter log;
        
        public ClearLineGroup()
        {
            Clear();	
        }

        public void Clear()
        {
            this.clear = new XLineGroupMember[0];

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
            return IApp.VALUE_SUCCESS;
        }   
    }
}
