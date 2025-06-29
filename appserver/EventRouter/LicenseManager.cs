using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Utilities.Collections;

using Metreos.Configuration;
using Metreos.LicensingFramework;
using Metreos.Stats;

namespace Metreos.AppServer.EventRouter
{
    /// <summary>
    /// Handles license check-in/check-out operations for the router
    /// </summary>
    internal sealed class LicenseManager : Loggable
    {
        // License file constants
        private const string SCRIPT_INSTANCE_FEATURE = "ScriptInstances";
        private const string LICENSE_MODE_FEATURE    = "LicenseModeCUAE";
        private const string LICENSE_MGR_HOSTNAME    = "localhost";
        private const string LICENSE_LIST            = "@" + LICENSE_MGR_HOSTNAME;
        private const string LICENSE_VERSION         = "2.4";

        /// <summary>
        /// The license mode obtained from the license server
        /// </summary>
        public Constants.LicenseModes LicenseMode { get { return licenseMode; } }
        private Constants.LicenseModes licenseMode = Constants.LicenseModes.SDK;

        /// <summary>
        /// The number of script instances retrieved from the license server
        /// </summary>
        public ulong LicensedInstances { get { return licensedInstances; } }
        private ulong licensedInstances = 0;

        private Thread refreshThread;

        // handles license overage checks and tasks
        private OverageMonitor overageMon;

        // used to report licensing statistics and alarms
        private StatsClient statsClient;

        // tracks the number of currently running script instances
        private ulong numberOfRunningScripts;

        public LicenseManager() : base(Config.LicenseManager.LogLevel, "LM")
        { 
            this.numberOfRunningScripts = 0;
            this.overageMon = new OverageMonitor();
            this.statsClient = StatsClient.Instance;
        }

        /// <summary>
        /// Startup invokes a blocking config refresh
        /// </summary>
        public void Startup()
        {
            // Manually set component status because this class does not extend PrimaryTaskBase
            Config.Instance.UpdateStatus(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LICENSE_MANAGER, IConfig.Status.Enabled_Running);

            // Set num sessions to zero
            statsClient.SetStatistic(IStats.Statistics.AppSessions, 0);

            RefreshConfiguration(true);
        }

        /// <summary>
        /// Checks all checked out license instances back into the license server
        /// </summary>
        public void Shutdown()
        {
            // Manually set component status because this class does not extend PrimaryTaskBase
            Config.Instance.UpdateStatus(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LICENSE_MANAGER, IConfig.Status.Enabled_Stopped);

            // Set num sessions to zero
            statsClient.SetStatistic(IStats.Statistics.AppSessions, 0);
        }

        /// <summary>
        /// Refreshes the licensing informatin used by the router by reading it from the license server.
        /// Checks in any checked out resources to the license server, prior to checking them back out.
        /// </summary>
        /// <param name="blockingRefresh">If true, the refresh operation will block. Otherwise, it will execute in its own thread.</param>
        public void RefreshConfiguration(bool blockingRefresh)
        {
            this.log.LogLevel = Config.LicenseManager.LogLevel;

            if(blockingRefresh)
            {
                log.Write(TraceLevel.Info, "Beginning blocking License Manager refresh...");
                RetrieveLicenseInfo();
            }
            else 
            {
                log.Write(TraceLevel.Info, "Beginning async License Manager refresh...");
                refreshThread = new Thread(new ThreadStart(RetrieveLicenseInfo));
                refreshThread.Start();
            }
        }

        /// <summary>
        /// Retrieves licensing information from the license server
        /// </summary>
        /// <returns>true if the operation succeeded, false otherwise</returns>
        private void RetrieveLicenseInfo()
        {
            bool success = false;
            LicenseInformationCUAE licenseInfo = new LicenseInformationCUAE();

            try
            {
                int result = LicenseUtilities.CheckOut(ref licenseInfo);
                Constants.LicenseModes newLicenseMode = (Constants.LicenseModes)Enum.Parse(typeof(Constants.LicenseModes), licenseInfo.licenseMode, true);
                ulong newScriptInstances = GetScriptInstanceLimit(newLicenseMode, licenseInfo);
                licenseMode = newLicenseMode;
                licensedInstances = newScriptInstances;
                log.Write(TraceLevel.Info, "License Mode set to '" + licenseMode + "'");
                log.Write(TraceLevel.Info, "License Mode threshold limits script instances to: '" + licensedInstances + "'");
                success = true;
            }
            catch (Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                success = false;
            }

            if(success)
            {
                log.Write(TraceLevel.Info, "License Manager refresh complete.");
            }
            else
                log.Write(TraceLevel.Warning, "License Manager refresh failed.");

        }  // RetrieveLicenseInfo method

        /// <summary>
        /// Checks whether or not a script is allowed to run based on the licensed number of script instances
        /// </summary>
        /// <returns>'true' is script is allowed to run, 'false' otherwise</returns>
        public bool IsScriptAllowedToRun()
        {
            bool allowScript;

            string licMsg;
            TraceLevel level;

            // Check whether this script will be allowed to run
            if(numberOfRunningScripts < licensedInstances)
            {
                licMsg = String.Empty;
                level = TraceLevel.Verbose;
                allowScript = true;
            }
            else if (overageMon.PerformOverageCheck(numberOfRunningScripts))
            {
                level = TraceLevel.Warning;
                licMsg = "Running script instances has exceeded license. You are within your license overage period.";
                allowScript = true;
            }
            else
            {
                level = TraceLevel.Error;
                licMsg = "Running script instances has exceeded license. License overage period exceeded. Licensing limits are being strictly enforced.";
                allowScript = false;
            }

            log.Write(level, String.Format("License check {0}: licensed: {1}, running: {2}. {3}",
                                            allowScript ? "PASS" : "FAIL",
                                            licensedInstances,
                                            numberOfRunningScripts,
                                            licMsg));

            return allowScript;
        }

        private ulong GetScriptInstanceLimit(Constants.LicenseModes newLicenseMode, LicenseInformationCUAE licenseInfo)
        {
            ulong newScriptInstances = (ulong)licenseInfo.scriptInstances;

            if (newLicenseMode != Constants.LicenseModes.Premium && newLicenseMode != Constants.LicenseModes.StdPrem &&
                    newLicenseMode != Constants.LicenseModes.EnhPrem)
            {
                if (licenseInfo.scriptInstances > licenseInfo.licenseModeThreshold)
                {
                    newScriptInstances = (ulong)licenseInfo.licenseModeThreshold;
                }
            }

            return newScriptInstances;
        }

        /// <summary>
        /// Increments the number of running script instances
        /// </summary>
        public void IncrementNumberOfRunningInstances()
        {
            numberOfRunningScripts++;
            statsClient.SetStatistic(IStats.Statistics.AppSessions, numberOfRunningScripts);
        }

        /// <summary>
        /// Decrements the number of running script instances
        /// </summary>
        public void DecrementNumberOfRunningInstances()
        {
            if(numberOfRunningScripts != 0)
                numberOfRunningScripts--;
            statsClient.SetStatistic(IStats.Statistics.AppSessions, numberOfRunningScripts);
        }
    } // class
} //namespace 
