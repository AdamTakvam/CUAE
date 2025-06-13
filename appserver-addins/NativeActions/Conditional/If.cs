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

using Package = Metreos.Interfaces.PackageDefinitions.Conditional.Actions.If;

namespace Metreos.Native.Conditional
{
    /// <summary>
    /// Metreos.Native.Conditional.If
    /// Input:
    ///     FIELD_VALUE1 = Boolean resolvable string, boolean variable, or boolean resolved c# snippet
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Conditional.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Conditional.Globals.PACKAGE_DESCRIPTION)]
    public class If : INativeAction
    {
		public enum ReturnValues
		{
			@true,
			@false
		}

        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.Value1.DISPLAY, Package.Params.Value1.DESCRIPTION, true, Package.Params.Value1.DEFAULT)]
		public bool Value1 { set { value1 = value; } }
		private bool value1;

        public If() {}

        public bool ValidateInput()
        {
           return true;
        }

        public void Clear()
        {
        }

        [Action("If", false, null, "Returns the string representation of a boolean expression.")]
        [ReturnValue(typeof(ReturnValues), "'true' if true, 'false' if false")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            if(value1)
            {
                return ReturnValues.@true.ToString();
            }
            else
            {
                return ReturnValues.@false.ToString();
            }
        }
	}
}
