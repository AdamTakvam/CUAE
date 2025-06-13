using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.ApplicationFramework.Collections;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;

using Package = Metreos.Interfaces.PackageDefinitions.Conditional.Actions.Switch;

namespace Metreos.Native.Conditional
{
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Conditional.Globals.NAMESPACE, 
                 Metreos.Interfaces.PackageDefinitions.Conditional.Globals.PACKAGE_DESCRIPTION)]
	public class Switch : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.SwitchOn.DISPLAY, Package.Params.SwitchOn.DESCRIPTION, true, Package.Params.SwitchOn.DEFAULT)]
		public object SwitchOn { set { switchOn = value; } }
		private object switchOn;

        public Switch() {}

        public bool ValidateInput()
        {
            if(switchOn == null) { return false; }
            return true;
        }

        public void Clear()
        {
        }

        [Action("Switch", false, "Switch", "Evaluates the given expression and returns the result, which is intended to be used as a branching criterion")]
        [ReturnValue(true, "The value specified in the " + Package.Params.SwitchOn.DISPLAY + " parameter")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            return Convert.ToString(switchOn);
        }
	}
}
