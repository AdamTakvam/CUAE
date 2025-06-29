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
    /// Summary description for SetActiveRelayInfo.
    /// </summary>
    public class SetActiveRelayInfo : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("User Id", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("AppRoutingGuid", true)]
        public string AppRoutingGuid { set { appRoutingGuid = value; } }
        private string appRoutingGuid;

        [ActionParamField("The number of the calling party", true)]
        public string FromNumber { set { fromNumber = value; } }
        private string fromNumber;

        [ActionParamField("The destination party number", true)]
        public string ToNumber { set { toNumber = value; } }
        private string toNumber;
        
        [ActionParamField("'true' indicates that the call was already swapped", true)]
        public bool WasSwapped { set { wasSwapped = value; } }
        private bool wasSwapped;

        public SetActiveRelayInfo()
        {
            Clear();
        }
 
        public void Clear()
        {
            appRoutingGuid = null;
            userId = 0;
        }
 
        public bool ValidateInput()
        {
            return !( (userId < ActiveRelay.StandardPrimaryKeySeed) || (appRoutingGuid == null) );
        }
 
        [Action("SetActiveRelayInfo", false, "Sets AR Info", "Sets ActiveRelay info based on user id")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, ActiveRelay.TableName);
            builder.AddFieldValue(ActiveRelay.UserId, userId);
            builder.AddFieldValue(ActiveRelay.RoutingGuid, appRoutingGuid);
            builder.AddFieldValue(ActiveRelay.TimeStamp, System.DateTime.Now);
            builder.AddFieldValue(ActiveRelay.FromNumber, fromNumber);
            builder.AddFieldValue(ActiveRelay.ToNumber, toNumber);
            builder.AddFieldValue(ActiveRelay.WasSwapped, wasSwapped);
            
            bool success = false;
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
                string insert = builder.ToString();

                int affectedRows = ActiveRelay.ExecuteNonQuery(insert, connection);
                if (affectedRows == 0)
                {
                    log.Write(TraceLevel.Warning, "INSERT attempt into table \"{0}\" FAILED when using SQL command: " +
                        "{1}\n", ActiveRelay.TableName, insert);
                    success = false;
                }
                else
                    success = true;
            }
            catch (Exception e)
            {
                log.Write(TraceLevel.Warning, "Error encountered in the SetActiveRelayInfo method, using UserId: " +
                    "\"{0}\" and AppRoutingGuid: \"{1}\"\n" +
                    "Error message: {2}", userId, appRoutingGuid, e.Message);
                success = false;
            }

            if (connection != null)
                connection.Close();
            return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
        }
    }
}
