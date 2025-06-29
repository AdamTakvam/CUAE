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
	/// Check if conference is empty and retrieves participant count for a conference
	/// </summary>
    [PackageDecl("Metreos.Native.ScheduledConference")]
    public class IsConferenceEmpty : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Conference pin", true)]
        public uint ConferencePin { set { conferencePin = value; } }
        private uint conferencePin;

        [ResultDataField("The number of participants present in conference")]
        public uint ParticipantCount { get { return participantCount; } }
        private uint participantCount;
 
        public IsConferenceEmpty()
        {
            Clear();
        }
 
        public void Clear()
        {
            conferencePin = participantCount = 0;
        }

        public void Reset()
        {
        }

        public bool ValidateInput()
        {
            return true;
        }
 
        [Action("IsConferenceEmpty", false, "Check if conference is empty and retrieves participant count for a conference", "Check if conference is empty and retrieves participant count for a conference")]
        [ReturnValue(typeof(SC.ReturnValues), "Return values of ScheduledConference native actions")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, SC.TableName);
            builder.fieldNames.Add(SC.ParticipantCount);
            builder.where[SC.ConferencePin] = conferencePin;
            
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
                DataTable table = ScheduledConference.ExecuteQuery(builder.ToString(), connection);
                
                if (table == null || table.Rows.Count == 0)
                {
                    return SC.ReturnValues.@true.ToString();
                }

                DataRow row = table.Rows[0];
                participantCount = Convert.ToUInt32(row[SC.ParticipantCount]);
                
                return (participantCount == 0) ? SC.ReturnValues.@true.ToString() : SC.ReturnValues.@false.ToString();
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { conferencePin, e.Message } ;
                log.Write(TraceLevel.Warning, "Error encountered in the GetParticipantCount method, using pin: {0}\n"+
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
