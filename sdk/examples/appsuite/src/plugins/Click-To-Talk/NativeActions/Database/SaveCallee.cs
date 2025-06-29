using System;
using System.Data;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Utils=Metreos.Utilities;

namespace Metreos.Native.ClickToTalk
{
    /// <summary>
    /// Saves callee information to application-specific database
    /// </summary>
    [PackageDecl("Metreos.Native.ClickToTalk", "Saves information to application-specific database")]
	public class SaveCallee : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Conference ID", true)]
        public uint ConferencesID { set { confId = value; } }
        private uint confId;

        [ActionParamField("Callee name", true)]
        public string Name { set { name = value; } }
        private string name;

        [ActionParamField("Callee telephony address (DN)", true)]
        public string Address { set { address = value; } }
        private string address;

        public SaveCallee()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if((address == null) || (confId == 0)) { return false; }
            return true;
        }

        public void Clear()
        {
            confId = 0;
            address = null;
            name = null;
        }

        [Action("SaveCallee", false, "Save Callee", "Adds callee info to database")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME]; 
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.INSERT, IDatabase.CALLEE_TABLE);
            sql.AddFieldValue(IDatabase.CONF_ID, confId);
            sql.AddFieldValue(IDatabase.ADDRESS, address);
            sql.AddFieldValue(IDatabase.NAME, name);

            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database command. Error was: " + e.Message);
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }
	}
}
