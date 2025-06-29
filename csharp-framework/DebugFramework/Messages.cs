using System;
using System.Collections;

using Metreos.ApplicationFramework;

namespace Metreos.DebugFramework
{
	/// <summary>
	/// This object represents a message sent to or from the Application Server during debugging.
	/// </summary>
	/// <remarks>
	/// Debugging will begin on the next script instance to begin after a breakpoint
	///    has been set. You cannot debug a running script instance.
	///    
	/// The action ID specified in 'HitBreakpoint' has not yet executed
	/// The action ID specified in the response to 'StepOver' or 'StepInto' is the next action
	///    to be executed
	/// 
	/// The Step commands keep setting breakpoints on each as action as they are executed
	/// 'StepInto' will follow the execution path into functions
	/// 'Break' responds with info about the current action then it sets a breakpoint on the next action
	/// 'StopDebugging' is sent to the client when the script exists
	/// 'UpdateValue' allows the client to change the value of a single variable at runtime
	/// </remarks>
	[Serializable]
	public abstract class DebugMessage
	{
		public string transactionId;
        public string failReason;
		public Hashtable funcVars;
		public Hashtable scriptVars;
		public SessionData sessionData;
        public Stack callStack;         // action IDs
	}

	[Serializable]
	public sealed class DebugCommand : DebugMessage
	{
		public enum CommandType
		{
			Undefined,
            StartDebugging,  // client  -> server
            StopDebugging,   // client <-> server
			SetBreakpoint,   // client  -> server
            GetBreakpoints,  // client  -> server
            ClearBreakpoint, // client  -> server
			HitBreakpoint,   // client <-  server
			Run,             // client  -> server
            StepOver,        // clinet  -> server
            StepInto,        // clinet  -> server
			Break,           // clinet  -> server
            UpdateValue,     // client  -> server
            Ping             // client  -> server
		}

		public CommandType type;
		public string appName;
		public string scriptName;
		public string actionId;

		public DebugCommand() {}
	}
    
	[Serializable]
	public sealed class DebugResponse : DebugMessage
	{
		public bool success;
		public string nextActionId;
		public string actionResult;

		public DebugResponse() {}
	}
}
