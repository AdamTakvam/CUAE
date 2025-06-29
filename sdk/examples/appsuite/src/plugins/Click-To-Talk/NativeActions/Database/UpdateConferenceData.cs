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
	/// <summary>Update conference information</summary>
	public class UpdateConferenceData : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The unique ID of this conference", true)]
        public uint ID { set { conferenceId = value; } }
        private uint conferenceId;

        [ActionParamField("IP address of conference host", false)]
        public string HostIP { set { host = IDatabase.FormatHostIP(value); } }
        private string host;

        [ActionParamField("Friendly name for host", false)]
        public string HostDescription { set { hostDesc = value; } }
        private string hostDesc;

        [ActionParamField("Indicates whether or not to record the conference", false)]
        public bool Record { set { record = value; } }
        private bool record;

        [ActionParamField("Email address to mail recorded conference to", false)]
        public string Email { set { email = value; } }
        private string email;

        [ActionParamField("Cisco IP Phone username for host's phone", false)]
        public string HostUsername { set { username = value; } }
        private string username;

        [ActionParamField("Cisco IP Phone password for host's phone", false)]
        public string HostPassword { set { password = value; } }
        private string password;

        [ActionParamField("Recording connection ID", false)]
        public uint RecordConnectionId { set { recordId = value; } }
        private uint recordId;

        [ActionParamField("Specifies whether the recording has terminated", false)]
        public bool RecordEnded { set { recordEnd = value; } }
        private bool recordEnd;

        [ResultDataField("Conference metadata")]
        public DataRow ResultData { get { return resultData; } }
        private DataRow resultData;

        public UpdateConferenceData() 
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if(conferenceId == 0) { return false; }
            return true;
        }

        public void Clear()
        {
            conferenceId = 0;
            host = null;
            hostDesc = null;
            record = false;
            email = null;
            username = null;
            password = null;
            recordId = 0;
            recordEnd = false;
        }

        [Action("UpdateConferenceData", false, "Update Conference Data", "Updates conference metadata")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            this.log = log;
            
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            string sql = BuildUpdateCommand();
            if(sql == null)
            {
                log.Write(TraceLevel.Warning, "No data passed to UpdateConferenceData action. No database action taken.");
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }
        
            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database command. Error was: " + e.Message);
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            if(GetConferenceData.GetConferenceObject(conferenceId, db, log, out resultData) == false)
            {
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }

        private string BuildUpdateCommand()
        {
            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.UPDATE, IDatabase.CONF_TABLE);
            sql.where.Add(IDatabase.ID, conferenceId);

            sql.AddFieldValue(IDatabase.RECORD_END, recordEnd.ToString());
            sql.AddFieldValue(IDatabase.RECORD, record.ToString());

            if(host != null)        { sql.AddFieldValue(IDatabase.HOST, host); }
            if(hostDesc != null)    { sql.AddFieldValue(IDatabase.HOST_DESC, hostDesc); }
            if(username != null)    { sql.AddFieldValue(IDatabase.HOST_USER, username); }
            if(password != null)    { sql.AddFieldValue(IDatabase.HOST_PASS, password); }
            if(recordId != 0)       { sql.AddFieldValue(IDatabase.RECORD_ID, recordId.ToString()); }
            if(email != null)       { sql.AddFieldValue(IDatabase.EMAIL, email); }

            if(sql.fieldNames.Count == 0) { return null; }
            return sql.ToString();
        }
	}
}
