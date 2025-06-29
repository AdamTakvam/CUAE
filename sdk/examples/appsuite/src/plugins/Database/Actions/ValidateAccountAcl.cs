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
    public class ValidateAccountAcl : INativeAction
    {
        protected enum ReturnValues
        {
            success,
            InvalidNumber,
            NotAuthorized,
            failure
        }

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("User ID", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("Destination Number", true)]
        public string DestinationNumber { set { destinationNumber = value; } }
        private string destinationNumber;

        public ValidateAccountAcl() 
        {
            Clear();
        }

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            if(userId == 0) return false;

            return true;
        }

        public void Clear()
        {
            destinationNumber = null;
            userId = new uint();
        }

        [Action("ValidateAccountAcl", false, "Validate Account ACL", "Determines whether the user has permissions to dial a specific number.")]
        [ReturnValue(typeof(ReturnValues), "Return value for the ACL validation operation.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            if(destinationNumber == null || destinationNumber.Length <= 0) return ReturnValues.InvalidNumber.ToString();
            
            return IApp.VALUE_SUCCESS;
        }

        #endregion
    }
}
