using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary> Writes a new FindMe call detail record. </summary>
	public class WriteFindMeCallRecord : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("The Id of the call record that we're attaching this record to.", true)]
		public uint CallRecordId { set { callRecordId = value; } }
		private uint callRecordId;

		[ActionParamField("Originating number.", true)]
		public  string OriginNumber { set { originNumber = value; } }
		private string originNumber;

		[ActionParamField("Destination number.", true)]
		public  string DestinationNumber { set { destinationNumber = value; } }
		private string destinationNumber;

		[ActionParamField("The type of FindMe call.", true)]
		public  Storage.ActiveRelayCallTypes TypeOfCall { set { typeOfCall = value; } }
		private Storage.ActiveRelayCallTypes typeOfCall;

		[ActionParamField("EndReason for the call.", true)]
		public  Storage.EndReason CallEndReason { set { callEndReason = value; } }
		private Storage.EndReason callEndReason;

		public WriteFindMeCallRecord()
		{
			Clear();
		}

		public bool ValidateInput()
		{
			return true;
		}

		public void Clear()
		{
			callRecordId           = 0;
			originNumber            = null;
			destinationNumber       = null;
			typeOfCall		        = Storage.ActiveRelayCallTypes.FindMe;
			callEndReason           = Storage.EndReason.Invalid;
		}

		[Action("WriteFindMeCallRecord", false, "Write FindMe Call Record", "Writes a new FindMe call detail record.")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
			using(FindMeCallRecords findMeRecDbAccess = new FindMeCallRecords(
					  sessionData.DbConnections[SqlConstants.DbConnectionName],
					  log,
					  sessionData.AppName,
					  sessionData.PartitionName,
					  DbTable.DetermineAllowWrite(sessionData.CustomData)))
			{
                /// Leaving this out for now since the table does not support the script_name column.
                /*
                string scriptName = log.LogName;
                if (scriptName == null || scriptName == string.Empty)
                    scriptName = "UNAVAILABLE";
                else
                {
                    int indexOfDash = scriptName.LastIndexOf('-');
                    if (indexOfDash > 0)
                        scriptName = scriptName.Substring(0, indexOfDash);
                }
                */

				bool success = findMeRecDbAccess.WriteFindMeCallRecord(callRecordId, originNumber, destinationNumber, //scriptName
													(ActiveRelayCallTypes) typeOfCall, (EndReason) callEndReason);

				return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
			}
		}
	}	// class WriteFindMeCallRecord
}
