using System;
using System.Net;
using System.Diagnostics;
using Metreos.Interfaces;

namespace Metreos.Interfaces
{
	/// <summary>Alarm service definitions</summary>
	public abstract class IStats
	{
        public const string TimestampFormat     = "yyyy-MM-dd HH-mm-ss";
        public const int AlarmClearedIdOffset   = 1000;

        public abstract class MgmtListener
        {
            public abstract class Defaults
            {
                public static readonly IPAddress Interface  = IPAddress.Loopback;
                public const ushort Port                    = 9200;
            }

            public abstract class Commands
            {
                public const string GenerateGraph   = "GenerateGraph";
                public const string GetStatistic    = "GetStatistic";

                public abstract class Parameters
                {
                    public const string Oid         = "OID";
                    public const string StartTime   = "StartTime";
                    public const string EndTime     = "EndTime";
                    public const string Interval    = "Interval";  // Alternative to StartTime and EndTime
                    public const string HLine       = "HLine";     // Used to mark license limits
                    public const string ImagePath   = "ImagePath";
                }

                public enum Interval
                {
                    Hour,
                    SixHour,
                    TwelveHour,
                    Day,
                    Week,
                    Month,
                    Year
                }
            }
        }

        public abstract class StatsListener
        {
            public abstract class Defaults
            {
                public static readonly IPAddress Interface  = IPAddress.Loopback;
                public const ushort Port                    = 9201;
            }

            public abstract class Messages
            {
                public const int NewEvent			    = 1000;		
                public const int ClearEvent			    = 1001;		
                public const int EventCleared		    = 1002;		
                public const int EventAccepted		    = 1003;	
	
                public const int SetStatistic           = 1100;
                public const int StatAck                = 1101;
                public const int StatNack               = 1102;

                public abstract class Fields
                {
                    public const int AlarmCode		    = 2001;		
                    public const int Severity			= 2002;		
                    public const int Description		= 2003;		
                    public const int Timestamp		    = 2004;		
                    public const int Guid				= 2005;		
                    public const int ClearedTimestamp   = 2006;

                    public const int Oid                = 2100;
                    public const int Value              = 2101;
                    public const int ErrorText          = 2102;
                }
            }
        }

        public abstract class SnmpListener
        {
            public abstract class Defaults
            {
                public static readonly IPAddress Interface  = IPAddress.Loopback;
                public const ushort Port			        = 9202;
            }

            public abstract class Messages
            {
                // MessageId = Low-order OID number

                public abstract class Fields
                {
                    public const int Value = 1;
                }
            }
        }

        public abstract class SmtpSender
        {
            public const int Port = 25;
        }

        public abstract class SnmpSender
        {
            public const int LocalPort = 9161;
        }

        public abstract class AlarmCodes
        {
            public abstract class General
            {
                public const uint ServiceUnavailable        = 100;
                public const uint MediaServerUnavailable    = 101;
                public const uint OutOfMemory			    = 102;

                public abstract class Descriptions
                {
                    public const string ServiceUnavailable      = "CUAE service '{0}' is unavailable.";
                    public const string MediaServerUnavailable  = "CUAE media server '{0}' is unavailable.";
                    public const string OutOfMemory             = "CUAE service '{0}' is out of memory.";
                }
            }

            public abstract class AppServer
            { 
                public const uint UnexpectedShutdown    = 300;
                public const uint StartFailure          = 301;
                public const uint AppLoadFailure        = 302;
                public const uint ProviderLoadFailure   = 303;
                public const uint AppReloaded           = 304;
                public const uint ProviderReloaded      = 305;
                public const uint MediaDeployFailure    = 306;

                public const uint AppSessionHW          = 310;
                public const uint AppSessionLW          = 311;

                public const uint MgmtLoginFailure      = 320;
                public const uint MgmtLoginSuccess      = 321;

                public abstract class Descriptions
                {
                    public const string UnexpectedShutdown  = "CUAE Server '{0}' shutdown unexpectedly";
                    public const string StartFailure        = "CUAE Server '{0}' failed to start";
                    public const string AppLoadFailure      = "Application '{0}' failed to load";
                    public const string ProviderLoadFailure = "Provider '{0}' failed to load";
                    public const string AppReloaded         = "Application '{0}' reloaded due to failure";
                    public const string ProviderReloaded    = "Provider '{0}' reloaded due to failure";
                    public const string MediaDeployFailure  = "Media deploy failure (app={0}, server={1})";

                    public const string AppSessionHW        = "High water mark for application sessions exceeded: {0}";
                    public const string AppSessionLW        = "Low water mark for application sessions exceeded: {0}";

                    public const string MgmtLoginFailure    = "Management login failure (user={0}, remoteAddr={1})";
                    public const string MgmtLoginSuccess    = "Management login success (user={0}, remoteAddr={1})";
                }
            }

            public abstract class Licensing
            { 
                public const uint AppSessionsExceeded       = 400;
                public const uint AppSessionsExceededFinal  = 401;
                public const uint AppSessionDenied          = 402;

                public abstract class Descriptions
                {
                    public const string AppSessionsExceeded = "Number of licensed application instances exceeded: {0}";
                    public const string AppSessionsExceededFinal = "Number of licensed application instances exceeded; licenses are now strictly enforced.";
                    public const string AppSessionDenied    = "An attempt has been made to exceed the maximum number of licensed application instances: {0}";
                }
            }
        }

        public abstract class Statistics
        {
            public const int CpuLoad            = 2000;
            public const int AppServerMemory    = 2001;

            public const int AppSessions        = 2010;
            public const int ActiveCalls        = 2011;

            public abstract class Router
            {
                public const int MessageRate    = 2020;
                public const int EventRate      = 2021;
                public const int ActionRate     = 2022;
            }
        }
	}
}
