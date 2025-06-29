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
using ReturnValues = Metreos.Applications.cBarge.Const.ConferenceReturnValues;

namespace Metreos.Applications.cBarge
{
	/// <summary>
	/// Removes the record of the conference with specified directoryNumber and routingGuid 
	/// </summary>
	[PackageDecl("Metreos.Applications.cBarge")]
	public class RemoveCallRecord : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("DirectoryNumber", false)]
		public string DirectoryNumber 
        { 
            set 
            { 
                directoryNumber = value;
                directoryNumberSet = true;
            } 
        }
		private string directoryNumber;
        private bool directoryNumberSet;

        [ActionParamField("Only match records with this callReference", false)]
        public int CallReference
        {
            set
            {
                callReference = value;
                callReferenceSet = true;
            }
        }
        private int callReference;
        private bool callReferenceSet = false;

        [ActionParamField("Only match records with this callInstance", false)]
        public int CallInstance
        {
            set
            {
                callInstance = value;
                callInstanceSet = true;
            }
        }
        private int callInstance;
        private bool callInstanceSet = false;

        [ActionParamField("Only match records with this device name", false)]
        public string Sid 
        { 
            set 
            { 
                sid = value;
                sidSet = true;
            } 
        }
        private string sid;
        private bool sidSet = false;
        
        [ActionParamField("Only match records with this lineInstance", false)]
        public int LineInstance
        {
            set
            {
                lineInstance = value;
                lineInstanceSet = true;
            }
        }
        private int lineInstance;
        private bool lineInstanceSet = false;

        [ActionParamField("RoutingGuid", false)]
        public string RoutingGuid 
        { 
            set 
            { 
                routingGuid = value; 
                routingGuidSet = true;
            } 
        }
        private string routingGuid;
        private bool routingGuidSet;

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
 
		[Action("RemoveCallRecord", false, "Removes Call record", "Removes the record of the call with specified directoryNumber and routingGuid")]
		[ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{            
			SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.DELETE, CBargeCallRecords.TableName);
            if (routingGuidSet)
                builder.where[CBargeCallRecords.RoutingGuid] = routingGuid;
            if (callReferenceSet)
                builder.where[CBargeCallRecords.CallReference] = callReference;
            if (sidSet)
                builder.where[CBargeCallRecords.Sid] = sid;
            if (callInstanceSet)
                builder.where[CBargeCallRecords.CallInstance] = callInstance;
            if (lineInstanceSet)
                builder.where[CBargeCallRecords.LineInstance] = lineInstance;
			if (directoryNumberSet)
                builder.where[CBargeCallRecords.DirectoryNumber] = directoryNumber;


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
				object[] msgArray = new object[2] { directoryNumber, e.Message } ;
				log.Write(TraceLevel.Error, "Error encountered in the RemoveCallRecord method, using directoryNumber: {0}\n"+
					"Error message: {1}", msgArray);
				return ReturnValues.failure.ToString();
			}
		}
	}
}
