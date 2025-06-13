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
using Metreos.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601.Actions.ClearLines;

namespace Metreos.Native.AxlSoap601
{
    /// <summary> Clears all lines for a 'updatePhone' AXL SOAP method for Cisco CallManager 6.0.1 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.PACKAGE_DESCRIPTION)]
    public class ClearLines : INativeAction
    {    
        [ResultDataField(Package.Results.Lines.DISPLAY, Package.Results.Lines.DESCRIPTION)]
        public UpdatePhoneReqLines Lines { get { return clear; } }

        public LogWriter Log { set { log = value; } }

        private UpdatePhoneReqLines clear;
        private LogWriter log;
        
        public ClearLines()
        {
            Clear();	
        }

        public void Clear()
        {
            this.clear = new UpdatePhoneReqLines();

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
