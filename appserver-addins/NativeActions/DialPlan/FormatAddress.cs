using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Package = Metreos.Interfaces.PackageDefinitions.DialPlan.Actions.FormatAddress;

namespace Metreos.Native.DialPlan
{
    /// <summary>
    /// Formats the dialed number appropriately for H.323
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.DialPlan.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.DialPlan.Globals.PACKAGE_DESCRIPTION)]
    public class FormatAddress : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public string ResultData { get { return resultData; } }
        private string resultData;

		[ActionParamField(Package.Params.DialedNumber.DISPLAY, Package.Params.DialedNumber.DESCRIPTION, true, Package.Params.DialedNumber.DEFAULT)]
		public string DialedNumber { set { dn = value; } }
		private string dn;

		[ActionParamField(Package.Params.DialingRules.DISPLAY, Package.Params.DialingRules.DESCRIPTION, true, Package.Params.DialingRules.DEFAULT)]
		public Hashtable DialingRules { set { rules = value; } }
		private Hashtable rules;

        public FormatAddress() {}

        public bool ValidateInput()
        {
            if(dn == null || dn == String.Empty)
                return false;

            return true;
        }

        public void Clear()
        {
            dn = null;
            rules = null;
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            resultData = dn;

            if(rules != null)
            {
                string pattern              = null;
                string replacement          = null;
                IDictionaryEnumerator de    = rules.GetEnumerator();
                while(de.MoveNext())
                {
                    pattern = de.Key as string;
                    replacement = de.Value as string;

                    // Verify regex parameters
                    if(pattern == null) { continue; }
                    if(replacement == null) { replacement = ""; }

                    // Remove regex slashes, if present
                    pattern = pattern.Trim('/');
                    replacement = replacement.Trim('/');

                    if(Regex.IsMatch(resultData, pattern))
                    {
                        resultData = Regex.Replace(resultData, pattern, replacement);
                        break;
                    }
                }
            }
            
            return IApp.VALUE_SUCCESS;
        }

    }
}
