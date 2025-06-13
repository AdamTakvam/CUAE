using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Package = Metreos.Interfaces.PackageDefinitions.ApplicationControl.Actions.Sleep;

namespace Metreos.Native.Standard
{
    /// <summary>
    ///     Puts the script to sleep for specified number of milliseconds
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.ApplicationControl.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.ApplicationControl.Globals.PACKAGE_DESCRIPTION)]
    public class Sleep : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.SleepTime.DISPLAY, Package.Params.SleepTime.DESCRIPTION, true, Package.Params.SleepTime.DEFAULT)]
        public int SleepTime { set { sleepTime = value; } }
        private int sleepTime;
                  
        public Sleep() 
        {
            Clear();
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            try
            {
                System.Threading.Thread.Sleep(sleepTime);
            }
            catch
            {
                return IApp.VALUE_FAILURE;
            }
            
            return IApp.VALUE_SUCCESS;
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            sleepTime = 0;
        }
    }
}
