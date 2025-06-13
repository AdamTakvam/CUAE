using System;
using System.Net;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;

using nsoftware.IPWorks;

namespace Metreos.Stats.Alarms
{
    /// <summary>Sends SNMP traps for alarms (not stats)</summary>
    public class SnmpTrapSender : AlarmSenderBase
    {
        private readonly Snmpagent snmpagent;
        private readonly IConfig.Severity triggerLevel;
        private readonly IPAddress snmpServerIP;

        public IConfig.Severity TriggerLevel { get { return triggerLevel; } }
        public IPAddress SnmpServerIP { get { return snmpServerIP; } }

        public readonly string oidRoot;

        public SnmpTrapSender(LogWriter log)
            : base(log)
        {
            this.triggerLevel = Config.SnmpManager.TriggerLevel;
            this.snmpServerIP = Config.SnmpManager.ServerAddr;

            if(snmpServerIP == null || snmpServerIP == IPAddress.None)
                throw new SenderNotConfiguredException("No SNMP target configured");

            this.oidRoot = Config.Instance.GetOidRoot();

            this.snmpagent = new Snmpagent();

            // Register SNMP agent events (do we need this?)
            this.snmpagent.OnGetNextRequest += new Snmpagent.OnGetNextRequestHandler(this.OnGetNextRequest);
            this.snmpagent.OnSetRequest += new Snmpagent.OnSetRequestHandler(this.OnSetRequest);
            this.snmpagent.OnGetUserPassword += new Snmpagent.OnGetUserPasswordHandler(this.OnGetUserPassword);
            this.snmpagent.OnGetRequest += new Snmpagent.OnGetRequestHandler(this.OnGetRequest);

            this.snmpagent.LocalPort = IStats.SnmpSender.LocalPort;
            this.snmpagent.Active = true;		// make it active and listen to request
        }

        public override void Dispose()
        {
            snmpagent.Dispose();
        }

        /// <summary>Send SNMP TRAP based on error code</summary>
        public override void SendAlarm(AlarmData data, bool cleared)
        {
            // Check if severity is beyond threshold
            if(data.Severity > triggerLevel)
                return;

            string trapText = data.Description;
            if(cleared)
                trapText = "Alarm Cleared: " + trapText;

            string trapOid = oidRoot + data.AlarmCode.ToString();

            snmpagent.Reset();

            snmpagent.SNMPVersion = SnmpagentSNMPVersions.snmpverV2c;
            snmpagent.ObjCount = 1;
            snmpagent.ObjId[1] = "trapText";
            snmpagent.ObjValue[1] = trapText;

            snmpagent.SendTrap(snmpServerIP.ToString(), trapOid);

            log.Write(TraceLevel.Info, "Trap " + trapOid + " has been sent to: " + snmpServerIP);
        }

        #region Event Handler for SNMP Agent

        private void OnGetNextRequest(object sender, SnmpagentGetNextRequestEventArgs e)
        {
            // get next request
            if(!CheckAccess(e.User, e.SecurityLevel, "GET"))
                return;

            //If we get this far, we are answering the request.  By default, ErrorStatus is -1,
            //meaning do NOT answer the request.  So we'll change this to 0, meaning answer the request.
            //if later we need to send an error, we'll set ErrorStatus > 0.
            e.ErrorStatus = 0;
        }

        private void OnGetRequest(object sender, SnmpagentGetRequestEventArgs e)
        {
            // get request
            if(!CheckAccess(e.User, e.SecurityLevel, "GET"))
                return;

            //If we get this far, we are answering the request.  By default, ErrorStatus is -1,
            //meaning do NOT answer the request.  So we'll change this to 0, meaning answer the request.
            //if later we need to send an error, we'll set ErrorStatus > 0.
            e.ErrorStatus = 0;
        }

        private void OnGetUserPassword(object sender, SnmpagentGetUserPasswordEventArgs e)
        {
            // get user password
            // TODO: return user password from user/password mapping 
        }

        private void OnSetRequest(object sender, SnmpagentSetRequestEventArgs e)
        {
            // set request
            if(!CheckAccess(e.User, e.SecurityLevel, "SET"))
                return;

            //If we get this far, we are answering the request.  By default, ErrorStatus is -1,
            //meaning do NOT answer the request.  So we'll change this to 0, meaning answer the request.
            //if later we need to send an error, we'll set ErrorStatus > 0.
            e.ErrorStatus = 0;
        }

        /// <summary>
        /// Check SNMP access right based on user account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="securityLevel"></param>
        /// <param name="requiredAccess"></param>
        /// <returns></returns>
        private bool CheckAccess(string user, int securityLevel, string requiredAccess)
        {
            switch(securityLevel)
            {
                case 0: //anonymous and unauthenticated users
                    if(requiredAccess == "GET")
                        return true;
                    if(requiredAccess == "SET")
                        return false;
                    break;

                case 1: //authenticated users
                    // TODO: Add mapping between user and access rights
                    if(requiredAccess == "GET")
                        return true;
                    if(requiredAccess == "SET")
                        return true;
                    break;
            }
            return false;
        }
        #endregion
    }
}
