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
using Metreos.ApplicationSuite.Storage;
using ChangePinReturnValues = Metreos.ApplicationSuite.Storage.Users.ChangePinReturnValues;
namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Changes the PIN code for a specific user ID </summary>
    [PackageDecl("Metreos.ApplicationSuite.Actions")]
    public class ChangePinCode : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("User ID", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("PIN", true)]
        public string Pin { set { pin = value; } }
        private string pin;

        public ChangePinCode() 
        {
            Clear();
        }

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            pin     = null;
            userId  = 0;
        }

        [ReturnValue(typeof(ChangePinReturnValues), 
            "failure indicates a database error, InvalidPin indicates the pin was not entered into the database because it is invalid")]
        [Action("ChangePinCode", false, "Change PIN Code", "Changes the PIN code for a specific user ID.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ChangePinReturnValues result = usersDbAccess.ChangePin(userId, pin);
                return result.ToString();
            }
        }

        #endregion
    }
}
