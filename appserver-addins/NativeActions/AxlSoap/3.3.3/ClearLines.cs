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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333.Actions.ClearLines;

namespace Metreos.Native.AxlSoap333
{
    /// <summary> Clears all lines for a 'updatePhone' AXL SOAP method for Cisco CallManager 3.3.3 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap333.Globals.PACKAGE_DESCRIPTION)]
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
