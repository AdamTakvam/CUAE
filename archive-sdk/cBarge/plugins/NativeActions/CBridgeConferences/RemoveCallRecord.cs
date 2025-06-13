using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using ReturnValues = Metreos.Native.CBarge.Const.ConferenceReturnValues;

namespace Metreos.Native.CBarge
{
	/// <summary>
	/// Removes the record of the conference with specified lineId and routingGuid 
	/// </summary>
	[PackageDecl("Metreos.Native.CBarge")]
	public class RemoveCallRecord : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("LineId", true)]
		public string LineId { set { lineId = value; } }
		private string lineId;

        [ActionParamField("RoutingGuid", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

		public RemoveCallRecord()
		{
			Clear();
		}
 
		public void Clear()
		{
		}

		public void Reset()
		{
		}

		public bool ValidateInput()
		{
			return true;
		}
 
		[Action("RemoveCallRecord", false, "Removes Call record", "Removes the record of the call with specified lineId and routingGuid")]
		[ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{            
			SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.DELETE, CBargeCallRecords.TableName);
			builder.where[CBargeCallRecords.LineId] = lineId;
            builder.where[CBargeCallRecords.RoutingGuid] = routingGuid;


			IDbConnection connection = null;
			try
			{
				connection = DbInteraction.GetConnection(sessionData, Const.CbDbConnectionName, Const.CbDbConnectionString);
			}
			catch (Exception e)
			{
				object[] msgArray = new object[2] { Const.CbDbConnectionString, e.Message } ;
				log.Write(TraceLevel.Warning, "Could not open database at {0}.\n" + "Error Message: {1}", msgArray);
				return ReturnValues.failure.ToString();
			}
			try
			{
				int affectedRows = DbInteraction.ExecuteNonQuery(builder.ToString(), connection);
                
				return (affectedRows == 0) ? ReturnValues.NotFound.ToString() : ReturnValues.success.ToString();
			}
			catch (Exception e)
			{
				object[] msgArray = new object[2] { lineId, e.Message } ;
				log.Write(TraceLevel.Error, "Error encountered in the RemoveCallRecord method, using lineId: {0}\n"+
					"Error message: {1}", msgArray);
				return ReturnValues.failure.ToString();
			}
		}
	}
}
