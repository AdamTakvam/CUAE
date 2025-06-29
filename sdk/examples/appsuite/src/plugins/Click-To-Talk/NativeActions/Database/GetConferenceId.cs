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
	/// <summary>Retrieves conferenceID based on hostIP</summary>
	public class GetConferenceId : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The host initiating the application", false)]
        public string HostIP { set { host = IDatabase.FormatHostIP(value); } }
        private string host;

        [ResultDataField("Conference metadata")]
        public uint ConferenceId { get { return conferenceId; } }
        private uint conferenceId;

        public GetConferenceId()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if(host == null) { return false; }
            return true;
        }

        public void Clear()
        {
            host = null;
            conferenceId = 0;
        }

        [Action("GetConferenceId", false, "Get Conference Id", "Gets the conference ID given a hostIP")]      
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.SELECT, IDatabase.CONF_TABLE);
            sql.fieldNames.Add(IDatabase.ID);
            sql.where.Add(IDatabase.HOST, host);

            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    conferenceId = Convert.ToUInt32(command.ExecuteScalar());
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database query ({0}). Error was: {1}", sql.ToString(), e.Message);
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            IDatabase.Close(db, notPreviouslyOpen, log);

            // If a 0 is pulled out, then there is no conference with this host value
            return conferenceId == 0 ? IApp.VALUE_FAILURE : IApp.VALUE_SUCCESS;
        }
	}
}
