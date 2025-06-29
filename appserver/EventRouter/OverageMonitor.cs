using System;
using System.Collections;
using System.Threading;
using System.Data;

using Metreos.Configuration;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.Stats;

namespace Metreos.AppServer.EventRouter
{
    internal sealed class OverageMonitor
    {
        private const int NUMBER_OF_ALLOWED_OVERAGE_DAYS = 10;
        /// <summary>
        /// Timer used to reset any blocking flags on midnight, if user is allowed more overage days
        /// </summary>
        private Timer flagResetTimer;

        /// <summary>
        /// Table used to store all the dates on which an overage occured
        /// </summary>
        private ArrayList overageDatesList;

        private StatsClient statsClient;

        private bool alarmSent;
        private bool overageOccured;
        private bool overageModeAllowScript;

        public OverageMonitor()
        {
            overageDatesList = new ArrayList();
            statsClient = StatsClient.Instance;
            Initialize();
        }

        /// <summary>
        /// This method gets called by license manager when an overage occurs - this method checks if the script is allowed to run despite
        /// the fact that the overage occured.
        /// </summary>
        /// <returns>true if script is allowed to run, false otherwise</returns>
        public bool PerformOverageCheck(ulong numberOfRunningScripts)
        {
            if(overageOccured)
                return overageModeAllowScript;

            overageOccured = true;

            uint alarmToSend = IStats.AlarmCodes.Licensing.AppSessionsExceeded;
            if (overageDatesList.Count < NUMBER_OF_ALLOWED_OVERAGE_DAYS)
            {
                overageModeAllowScript = true;
                SetMostRecentOverageDate();
                SetResetTimer();
            }
            else
            {
                alarmToSend = IStats.AlarmCodes.Licensing.AppSessionsExceededFinal;
                overageModeAllowScript = false;
            }

            if (!alarmSent)
            {
                RaiseOverageAlarm(alarmToSend, numberOfRunningScripts);
            }

            return overageModeAllowScript;
        }

        private void Initialize()
        {
            // get the overage dates from DB and determine the most recent one
            RefreshOverageTable();

            // check to see that most recent date was successfully determined
            if(overageDatesList.Count > 0)
            {
                DateTime mostRecentDate = Convert.ToDateTime(overageDatesList[overageDatesList.Count - 1]);

                // if most recent date is today...
                if(mostRecentDate.Date.CompareTo(DateTime.Now.Date) == 0)
                { 
                    // then overage already occured today
                    overageOccured = true;
                    alarmSent = true;

                    // if there are still overage days left, Set reset timer
                    if (overageDatesList.Count < NUMBER_OF_ALLOWED_OVERAGE_DAYS)
                    {
                        SetResetTimer();
                    }
                }
            }
        }
        
        /// <summary>
        /// Sets the reset time to the upcoming midnight.
        /// </summary>
        private void SetResetTimer()
        { 
            flagResetTimer = new Timer(new TimerCallback(HandleTimerResetEvent),null, System.DateTime.Now.Date.AddDays(1) - System.DateTime.Now, System.TimeSpan.Zero);
        }

        /// <summary>
        /// Reset timer async callback handler.
        /// </summary>
        private void HandleTimerResetEvent(object args)
        {
            alarmSent = false;
            overageOccured = false;
            overageModeAllowScript = false;
            RefreshOverageTable();
        }

        /// <summary>
        /// Retrieves the overage table from the database
        /// </summary>
        private void RefreshOverageTable()
        {
            ArrayList overageDates = null;
            Config configUtil = Config.Instance;
            ConfigEntry configEntry = configUtil.GetEntry(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LICENSE_MANAGER, IConfig.Entries.Names.LIC_OVERAGE_TABLE);

            if(configEntry != null)
            {
                if(configEntry.Value != null)
                {
                    try
                    {
                        overageDates = configEntry.Value as ArrayList;
                        if (overageDates != null)
                            overageDatesList = overageDates;
                    }
                    catch
                    {
                        overageDatesList.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Writes the most current list of overage dates to the database
        /// </summary>
        private void UpdateOverageTable()
        {
            Config configUtil = Config.Instance;
            ConfigEntry configEntry = configUtil.GetEntry(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LICENSE_MANAGER, IConfig.Entries.Names.LIC_OVERAGE_TABLE);

            // If the entry was not found, we add it
            if(configEntry == null)
                configEntry = new ConfigEntry(IConfig.Entries.Names.LIC_OVERAGE_TABLE, overageDatesList, "License Overage Table", IConfig.StandardFormat.Array, true);
            else
                configEntry.Value = overageDatesList;
 
            configUtil.AddEntry(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LICENSE_MANAGER, configEntry);
        }

        /// <summary>
        /// Retrieves the most recent overage date from the overage date table.
        /// </summary>
        /// <returns></returns>
        private void SetMostRecentOverageDate()
        {
            overageDatesList.Add(DateTime.Now.Date.ToString());

            //sort increasing
            overageDatesList.Sort();

            UpdateOverageTable();
        }

        private void RaiseOverageAlarm(uint alarmToSend, ulong numberOfRunningScripts)
        {
            switch (alarmToSend)
            { 
                case IStats.AlarmCodes.Licensing.AppSessionsExceededFinal :
                    statsClient.TriggerAlarm(IConfig.Severity.Red, alarmToSend, IStats.AlarmCodes.Licensing.Descriptions.AppSessionsExceededFinal, numberOfRunningScripts);
                    break;
                case IStats.AlarmCodes.Licensing.AppSessionsExceeded:
                default :
                    statsClient.TriggerAlarm(IConfig.Severity.Yellow, alarmToSend, IStats.AlarmCodes.Licensing.Descriptions.AppSessionsExceeded, numberOfRunningScripts);
                    break;
            }

            alarmSent = true;
        }
    }
}