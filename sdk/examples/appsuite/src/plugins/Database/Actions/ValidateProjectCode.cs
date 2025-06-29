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

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Changes the PIN code for a specific user ID.</summary>
    [PackageDecl("Metreos.ApplicationSuite.Actions")]
    public class ValidateProjectCode : INativeAction
    {
        protected enum ReturnValues
        {
            success,
            InvalidCode,
            failure
        }

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Project Code", true)]
        public string ProjectCode { set { projectCode = value; } }
        private string projectCode;

        public ValidateProjectCode() 
        {
            Clear();
        }

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            if((projectCode == null) || (projectCode.Length <= 0)) return false;

            return true;
        }

        public void Clear()
        {
            projectCode = null;
        }

        [Action("ValidateProjectCode", false, "Validate Project Code", "Validates a project code.")]
        [ReturnValue(typeof(ReturnValues), "Return values for validate project code operation.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            return IApp.VALUE_SUCCESS;
        }

        #endregion
    }
}
