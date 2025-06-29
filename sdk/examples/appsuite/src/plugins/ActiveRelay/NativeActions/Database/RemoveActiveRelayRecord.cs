using System;
using System.Data;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;

namespace Metreos.Native.ActiveRelay
{
	/// <summary>
	/// Summary description for RemoveActiveRelayRecord.
	/// </summary>
    public class RemoveActiveRelayRecord : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("User Id", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("Match this routing guid when removing", false)]
        public string AppRoutingGuid { set { appRoutingGuid = value; } }
        private string appRoutingGuid;
        
        public RemoveActiveRelayRecord()
        {
            Clear();
        }
 
        public void Clear()
        {
            userId = 0;
            appRoutingGuid = null;
        }
 
        public bool ValidateInput()
        {
            return (userId < ActiveRelay.StandardPrimaryKeySeed) ? false : true;
        }
 
        [Action("RemoveActiveRelayRecord", false, "Removes AR Record", "Removes the ActiveRelay call record for a user from the ActiveRelay database")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            bool success = false;

            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.DELETE, ActiveRelay.TableName);
            builder.where[ActiveRelay.UserId] = userId;
            if (appRoutingGuid != null)
                builder.where[ActiveRelay.RoutingGuid] = appRoutingGuid;

            
            IDbConnection connection = null;
            connection = ActiveRelay.GetConnection(sessionData, ActiveRelay.ArDbConnectionName, ActiveRelay.ArDbConnectionString);

            try
            {
                if (connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed)
                    connection.Open();
            }
            catch (Exception e)
            {
                log.Write(TraceLevel.Warning, "Could not open database at {0}.\n" + "Error Message: {1}", 
                    ActiveRelay.ArDbConnectionString, e.Message);
                if (connection != null)
                    connection.Close();
                return IApp.VALUE_FAILURE;
            }
            try
            {
                string delete = builder.ToString();

                int rowsAffected = ActiveRelay.ExecuteNonQuery(delete, connection);
                if (rowsAffected == 0)
                {
                    log.Write(TraceLevel.Warning, "DELETE attempt from table \"{0}\" FAILED when using SQL command: {1}\n", 
                        ActiveRelay.TableName, delete);
                    success = false;
                }
                else
                    success = true;
            }
            catch (Exception e)
            {
                log.Write(TraceLevel.Warning, "Error encountered in the RemoveActiveRelayRecord method, using UserId: \"{0}\"\n" +
                    "Error message: {1}", userId, e.Message);
                success = false;
            }

            if (connection != null)
                connection.Close();
            return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
        }
	}
}
