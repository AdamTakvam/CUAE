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
    public class ValidateClientMatterCode : INativeAction
    {
        protected enum ReturnValues
        {
            success,
            InvalidCode,
            failure
        }

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Client Code", true)]
        public string ClientCode { set { clientCode = value; } }
        private string clientCode;

        [ActionParamField("Matter Code", false)]
        public string MatterCode { set { matterCode = value; } }
        private string matterCode;

        public ValidateClientMatterCode() 
        {
            Clear();
        }

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            if((matterCode == null) || (matterCode.Length <= 0)) return false;

            return true;
        }

        public void Clear()
        {
            matterCode = null;
            clientCode = null;
        }

        [Action("ValidateClientMatterCode", false, "Validate Client Matter Code", "Validates a client/matter code.")]
        [ReturnValue(typeof(ReturnValues), "Return values for validate project code operation.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            return IApp.VALUE_SUCCESS;
        }

        #endregion
    }
}
