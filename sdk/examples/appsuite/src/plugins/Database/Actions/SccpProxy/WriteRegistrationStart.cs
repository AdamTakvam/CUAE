using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Writes a new registration record. </summary>
	public class WriteRegistrationStart : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Device Identifier.", true)]
        public  string Sid { set { sid = value; } }
        private string sid;

        [ActionParamField("CallManager IP.", true)]
        public  string CallManagerIp { set { callManagerIp = value; } }
        private string callManagerIp;

        [ResultDataField("The ID of the new call record.")]
        public  int RegistrationId { get { return registrationId; } }
        private int registrationId;

        public WriteRegistrationStart()
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
            registrationId = 0;
            callManagerIp = null;
            sid = null;
        }

        [Action("WriteRegistrationStart", false, "Write Registration Start", "Writes a new registration record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Registrations registration = new Registrations(
                      sessionData.DbConnections[SqlConstants.Tables.Registrations.TableName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = registration.WriteRegistrationStart(
                    sid,
                    callManagerIp,
                    out registrationId);

                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
	}	// class WriteRegistrationStart
}
