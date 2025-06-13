using System;

namespace DebugConnection
{
    /// <summary>
    /// This object represents a command sent to or from the Application Server during debugging.
    /// </summary>
    /// <remarks>
    /// Debugging will begin on the next script instance to begin after a breakpoint
    ///    has been set. You cannot debug a running script instance.
    ///    
    /// 'SetBreakpoint' must be sent to start debugging
    /// 'SetBreakpoint' with no action ID will assume the first action
    /// The action ID specified in 'HitBreakpoint' has not yet executed
    /// The action ID specified in the response to 'ExecuteAction' is the next action
    ///    to be executed
    /// </remarks>
    [Serializable]
	public class DebugCommand
	{
        public enum CommandType
        {
            Undefined,
            SetBreakpoint,
            HitBreakpoint,
            ExecuteAction,
            StopDebugging
        }

        public CommandType type;
        public string appName;
        public string scriptName;
        public string actionId;
        public string transactionId;

        public DebugCommand() {}
	}
    
    [Serializable]
    public class DebugResponse
    {
        public bool success;
        public string failureReason;
        public string transactionId;

        public string actionId;
        public string actionResult;

        public DebugResponse() {}
    }
}
