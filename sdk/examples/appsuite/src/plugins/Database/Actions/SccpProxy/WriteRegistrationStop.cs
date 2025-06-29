using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Writes a record stop. </summary>
    public class WriteRegistrationStop : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Number of RingIn call state transitions.", true)]
        public  int NumRingIn { set { numRingIn = value; } }
        private int numRingIn;

        [ActionParamField("Number of RingOut call state transitions.", true)]
        public  int NumRingOut { set { numRingOut = value; } }
        private int numRingOut;

        [ActionParamField("Number of Busy call state transitions.", true)]
        public  int NumBusy { set { numBusy = value; } }
        private int numBusy;

        [ActionParamField("Number of Connected call state transitions.", true)]
        public  int NumConnected { set { numConnected = value; } }
        private int numConnected;

        [ActionParamField("Registration record ID.", true)]
        public  int RegistrationId { set { registrationId = value; } }
        private int registrationId;


        public WriteRegistrationStop()
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
            numRingIn           = 0;
            numRingOut          = 0;
            numBusy             = 0;
            numConnected        = 0;
            registrationId      = 0;
        }

        [Action("WriteRegistrationStop", false, "Write Registration Stop", "Completes a registration record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Registrations registration = new Registrations(
                      sessionData.DbConnections[SqlConstants.Tables.Registrations.TableName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = registration.WriteRegistrationStop(
                    registrationId,
                    numRingIn,
                    numRingOut,
                    numBusy,
                    numConnected);

                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
    }	// class WriteRegistrationStart
}
