using System;

namespace Metreos.CallControl.H323
{
	/// <summary>
	/// Summary description for DummyH323Provider.
	/// </summary>
	public class H323Provider
	{
        public string Name { get { return "Dummy H323 Provider"; } }

        #region Constants
        internal abstract class Consts
        {
            public const string DisplayName         = "H.323 Provider";

            public abstract class ConfigEntries
            {
                public const string ListenPort          = "Port";
                public const string EnableDebug         = "EnableStackDebugging";
                public const string DisableFastStart    = "DisableFastStart";		// Not exposed currently
                public const string DisableH245Tunneling= "DisableH245Tunneling";	// Not exposed currently
                public const string DisableH245InSetup  = "DisableH245InSetup";		// Not exposed currently
                public const string DebugLevel          = "StackDebuggingLogLevel";
                public const string DebugFilename       = "StackDebuggingLogFile";
                public const string H245RangeMin        = "H245RangeMin";
                public const string H245RangeMax		= "H245RangeMax";
                public const string MaxPendingCalls		= "MaxPendingCalls";
                public const string TcpConnectTimeout   = "TcpConnectTimeout";

                public abstract class DisplayNames
                {
                    public const string ListenPort			= "Listen Port";
                    public const string DisableFastStart    = "Disable Fast Start";
                    public const string DisableH245Tunneling= "Disable H.245 Tunneling";
                    public const string DisableH245InSetup  = "Disable H.245 In Setup";
                    public const string H245RangeMin        = "H.245 Range (min)";
                    public const string H245RangeMax		= "H.245 Range (max)";
                    public const string MaxPendingCalls		= "Max Pending Calls";
                    public const string EnableDebug         = "Enable Stack Debugging";
                    public const string DebugLevel          = "Stack Debugging Log Level";
                    public const string DebugFilename       = "Stack Debugging Log File";
                    public const string TcpConnectTimeout   = "TCP Connect Timeout";
                }

                public abstract class Descriptions
                {
                    public const string ListenPort			= "Port on which the stack should listen for incoming H.225 requests";
                    public const string DisableFastStart    = "Disable Fast Start";
                    public const string DisableH245Tunneling= "Disable H.245 Tunneling";
                    public const string DisableH245InSetup  = "Disable H.245 In Setup";
                    public const string H245RangeMin        = "H.245 port range (min)";
                    public const string H245RangeMax		= "H.245 port range (max)";
                    public const string MaxPendingCalls		= "Maximum number of pending calls allowed before stack starts auto-rejecting";
                    public const string EnableDebug         = "Causes stack to write logs to a file directly, instead of via Metreos Log Server";
                    public const string DebugLevel          = "Detail level of stack log messages. 0=Errors-only, 5=Verbose (if stack debugging is enabled)";
                    public const string DebugFilename       = "Name of log file to create (if stack debugging is enabled)";
                    public const string TcpConnectTimeout   = "Number of seconds to attempt to contact a gateway before giving up. A lower number ensures faster failover";
                }

                public abstract class Bounds
                {
                    public const int ListenPortMin		    = 1024;
                    public const int ListenPortMax		    = short.MaxValue;
                    public const int H245RangeMinMin        = 1024;
                    public const int H245RangeMinMax        = short.MaxValue;
                    public const int H245RangeMaxMin	    = 1024;
                    public const int H245RangeMaxMax	    = short.MaxValue;
                    public const int MaxPendingCallsMin	    = 25;
                    public const int MaxPendingCallsMax	    = 1000;
                    public const int DebugLevelMin		    = 0;
                    public const int DebugLevelMax		    = 5;
                    public const int TcpConnectTimeoutMin   = 1;
                    public const int TcpConnectTimeoutMax   = 10;
                }
            }

            public abstract class DefaultValues
            {
                public const bool EnableDebug           = false;
                public const int DebugLevel             = 3;
                public const string DebugFilename       = "H323StackLog.txt";
                public const bool DisableFastStart      = true;   
                public const bool DisableH245Tunneling  = true;
                public const bool DisableH245InSetup    = true;
                public const uint ListenPort            = 1720;
                public const uint H245RangeMin          = 10000;
                public const uint H245RangeMax          = 11000;
                public const uint MaxPendingCalls		= 100;
                public const int TcpConnectTimeout      = 2;
            }

            public const string StackHost           = "127.0.0.1";
            public const int StackIpcPort           = 8500;
            public const int ShutdownTimeoutMs      = 5000;

            public const int MorgueSize             = 100;

            
        }
        #endregion

		public H323Provider()
		{
        }
	}
}
