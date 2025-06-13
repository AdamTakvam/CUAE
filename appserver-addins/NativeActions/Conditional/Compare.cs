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

using Package = Metreos.Interfaces.PackageDefinitions.Conditional.Actions.Compare;

namespace Metreos.Native.Conditional
{
    /// <summary>
    /// Metreos.Native.Conditional.Compare
    /// Input:
    ///     FIELD_VALUE1 = The first value to compare
    ///     FIELD_VALUE2 = The second value to compare
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Conditional.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Conditional.Globals.PACKAGE_DESCRIPTION)]
	public class Compare : INativeAction
    {
		public enum ReturnValues
		{
			equal,
			unequal
		}

        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.Value1.DISPLAY, Package.Params.Value1.DESCRIPTION, true, Package.Params.Value1.DEFAULT)]
		public object Value1 { set { value1 = value; } }
		private object value1;

		[ActionParamField(Package.Params.Value2.DISPLAY, Package.Params.Value2.DESCRIPTION, true, Package.Params.Value2.DEFAULT)]
		public object Value2 { set { value2 = value; } }
		private object value2;

        public Compare() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
        }

        [Action("Compare", false, null, "Compares two values and indicates whether or not they are equal")]
        [ReturnValue(typeof(ReturnValues), "'equal' if the values are the same, otherwise 'unequal'.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            // NULL case
            if(value1 == null)
            {
                return value2 == null ? ReturnValues.equal.ToString() : ReturnValues.unequal.ToString();
            }

            // All other cases
            // Use Equals() to avoid unintended reference comparison
            if(!value1.Equals(value2))
            {
                return ReturnValues.unequal.ToString();
            }

            return ReturnValues.equal.ToString();
        }
	}
}
