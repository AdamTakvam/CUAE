using System;
using System.Data;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using SC=Metreos.Native.ScheduledConference.ScheduledConference;
using Metreos.Utilities;

namespace Metreos.Native.ScheduledConference
{
    /// <summary>
    /// Creates a conference record
    /// </summary>
    [PackageDecl("Metreos.Native.ScheduledConference")]
    public class CreateConfRecord : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Conference pin", true)]
        public uint ConferencePin { set { conferencePin = value; } }
        private uint conferencePin;

        public CreateConfRecord()
        {
            Clear();
        }
 
        public void Clear()
        {
            conferencePin = 0;
        }

        public void Reset()
        {
        }

        public bool ValidateInput()
        {
            return true;
        }
 
        [Action("CreateConfRecord", false, "Creates a conference record", "Creates a conference record")]
        [ReturnValue(typeof(SC.ReturnValues), "Return values of ScheduledConference native actions")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, SC.TableName);
            builder.AddFieldValue(SC.ConferencePin, Convert.ToString(conferencePin));

            bool alreadyOpen = false;
            IDbConnection connection = null;
            try
            {
                connection = SC.GetConnection(sessionData, ScheduledConference.ScDbConnectionName, ScheduledConference.ScDbConnectionString);

                if(connection.State == ConnectionState.Open)
                {
                    alreadyOpen = true;
                }
                else
                {
                    connection.Open();
                }
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { SC.ScDbConnectionString, e.Message } ;
                log.Write(TraceLevel.Warning, "Could not open database at {0}.\n" + "Error Message: {1}", msgArray);
                Reset();
                return SC.ReturnValues.failure.ToString();
            }
            try
            {
                int numAffectedRows = ScheduledConference.ExecuteNonQuery(builder.ToString(), connection);
                
                if (numAffectedRows == 0)
                {
                    object[] msgArray = new object[2] { "Could not create conference record for pin: ", conferencePin } ;
                    log.Write(TraceLevel.Error, "{0}{1}", msgArray);
                    return SC.ReturnValues.failure.ToString();
                }

                return SC.ReturnValues.success.ToString();
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { conferencePin, e.Message } ;
                log.Write(TraceLevel.Warning, "Error encountered in the CreateConfRecord method, using pin: {0}\n"+
                    "Error message: {1}", msgArray);
                return SC.ReturnValues.failure.ToString();
            }
            finally
            {
                try
                {
                    if(!alreadyOpen)
                    {
                        connection.Close();
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Could not close database connection. " + Exceptions.FormatException(e));
                }
            }
        }
    }
}
