using System;

namespace Metreos.Native.CallMonitor
{
	/// <summary> Contains mapping information for the actual Call Record database </summary>
	public class CallRecordDatabase
	{
        public const string DatabaseName                = "monitor_call";
		public const string TableName                   = "monitored_calls";
        public const string Id                          = "mc_monitored_call_id";
        public const string GovernmentAgentNumber       = "mc_government_agent_number";
        public const string DidNumber                   = "mc_did_number";
        public const string InsuranceAgentNumber        = "mc_insurance_agent_number";
        public const string CustomerNumber              = "mc_customer_number";
        public const string MonitoredSid                = "mc_monitored_sid";
        public const string StartMonitorTime            = "mc_start_monitor_timestamp";
	}
}
