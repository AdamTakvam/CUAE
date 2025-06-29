using System;
using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading; 

using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Configuration;
using Metreos.Interfaces;
using Metreos.LogSinks;
using Metreos.Stats;

namespace Metreos.WatchdogService
{
    /// <summary>
	/// Monitor Metreos service for QOS.
	/// </summary>
	public sealed class Watchdog : Loggable, IDisposable
	{
        public abstract class Consts
        {
            public const string Name = "Watchdog";
            
            public abstract class Config
            {
                public const string TimeInterval    = "TimeInterval";	// Tag name for config file	
            }

            public abstract class Defaults
            {
                public const int TimeInterval       = 60000;	    	// default time interval is 1 minute.
            }
        }

		private readonly Hashtable services;
        private readonly Hashtable setAlarms;

        private volatile bool shutdown = false;
        private readonly Thread watchdogThread;
        private readonly int timeInterval;

        private readonly StatsClient statsClient;
        private readonly LogServerSink logClient;

		/// <summary>
		/// Watchdog constructor
		/// </summary>
		public Watchdog()
            : base(TraceLevel.Verbose, Consts.Name)
		{
            ushort port = Config.LogService.ListenPort;
            port = port > 0 ? port : IServerLog.Default_Port;
            logClient = new LogServerSink(Consts.Name, port, Config.Watchdog.LogLevel);

            this.statsClient = StatsClient.Instance;
            this.setAlarms = Hashtable.Synchronized(new Hashtable());
			this.services = new Hashtable();

            this.timeInterval = GetTimeInterval();

            this.watchdogThread = new Thread(new ThreadStart(WatchdogWorker));
            this.watchdogThread.Name = "Thread for Watchdog";
            this.watchdogThread.IsBackground = true;
		}

        /// <summary> 
        /// Read configuration data from application config file
        /// </summary>
        private int GetTimeInterval()
        {
            int interval = 0;
            try { interval = Convert.ToInt32(AppConfig.GetEntry(Consts.Config.TimeInterval)); }
            catch { }

            if(interval == 0)
                interval = Consts.Defaults.TimeInterval;

            return interval;
        }

        public void Start()
        {
            watchdogThread.Start();
        }

		public override void Dispose()
		{
            shutdown = true;
            if(!watchdogThread.Join(2000))
                watchdogThread.Abort();

			statsClient.Dispose();

    		services.Clear();

            logClient.Dispose();

            base.Dispose();
        }

        #region Worker Thread

        /// <summary>
        /// Entrypoint of watchdog thread.
        /// </summary>
        private void WatchdogWorker()
        {
            while(!shutdown)
            {
                log.Write(TraceLevel.Verbose, "Enter Watchdog loop");
                // Read service data from db
                ArrayList alServices = Config.Instance.GetServicesInfo();
                if(alServices.Count > 0)
                {
                    try
                    {
                        log.Write(TraceLevel.Verbose, "Number of services: " + alServices.Count);
                        // Refresh the services list
                        RefreshServices(alServices);
                        // Rescan all services
                        ScanServices();
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Error, "Thread exception - " + e.Message);
                    }
                }
                else
                {
                    log.Write(TraceLevel.Error, "DB Error, failed to retrieve service list.");
                }

                Thread.Sleep(timeInterval);
            }
        }

		private void RefreshServices(ArrayList alServices)
		{
			log.Write(TraceLevel.Verbose, "<<< Watchdog starts refreshing and verifying services...");
            
			lock(services.SyncRoot)
			{
				services.Clear();

				foreach(ServiceInfo service in alServices)
				{
					services.Add(service.Name, service);
				}
			}
		}

		/// <summary>
		/// Scanning each listed service to do well check
		/// </summary>
        private void ScanServices()
		{
			lock(services.SyncRoot)
			{
				foreach(ServiceInfo service in services.Values)
				{
					ServiceController sc = FindService(service.Name);
	
					if (sc == null)
						continue;

					service.Status = CheckService(sc);
					if (!VerifyServiceState(service))
					{
						// Wrong state, trigger Alarm then start the service.
                        AddAlarm(service.Name);
                        try
                        {
                            sc.Start();
                            RemoveAlarm(service.Name);
                            log.Write(TraceLevel.Warning, "Alarm cleared, Watchdog starts " + service.Name);
                            StartProcess(service.Name);
                        }
                        catch
                        {
                            log.Write(TraceLevel.Error, "Watchdog failed to start " + service.Name);
                        }
					}
				}
			}

			log.Write(TraceLevel.Verbose, ">>> Watchdog finished refreshing and verifying services...");
		}

		private ServiceInfo.State CheckService(ServiceController service)
		{
			ServiceInfo.State st = ServiceInfo.State.Unknown;

			if (service == null)
				return st;
              
            try 
            {
                switch(service.Status)
                {
                    case ServiceControllerStatus.ContinuePending:
                        st = ServiceInfo.State.Continuing;
                        log.Write(TraceLevel.Info, service.ServiceName + " is continue pending");
                        break;

                    case ServiceControllerStatus.Paused:
                        st = ServiceInfo.State.Paused;
                        log.Write(TraceLevel.Info, service.ServiceName + " is paused");
                        break;

                    case ServiceControllerStatus.PausePending:
                        st = ServiceInfo.State.Pausing;
                        log.Write(TraceLevel.Info, service.ServiceName + " is pausing");
                        break;

                    case ServiceControllerStatus.Running:
                        st = ServiceInfo.State.Started;
                        log.Write(TraceLevel.Verbose, service.ServiceName + " is running");
                        break;

                    case ServiceControllerStatus.StartPending:
                        st = ServiceInfo.State.Starting;
                        log.Write(TraceLevel.Info, service.ServiceName + " is starting");
                        break;

                    case ServiceControllerStatus.Stopped:
                        st = ServiceInfo.State.Stopped;
                        log.Write(TraceLevel.Info, service.ServiceName + " is stopped");
                        break;

                    case ServiceControllerStatus.StopPending:
                        st = ServiceInfo.State.Stopping;
                        log.Write(TraceLevel.Info, service.ServiceName + " is stopping");
                        break;

                    default:
                        log.Write(TraceLevel.Warning, service.ServiceName + " is in unknown state");
                        break;
                }
            }
            catch
            {
                log.Write(TraceLevel.Error, "Unable to access service status!");
            }

			return st;
		}

		private ServiceController FindService(string name)
		{ 
			log.Write(TraceLevel.Verbose, "FindService("+name +")");

			ServiceController service = null;
			try
			{ 				
				ServiceController[] services;
				services = ServiceController.GetServices();
				for (int i = 0; i < services.Length; i++)
				{
					if(services[i].ServiceName == name)
					{
						service = services[i];
						log.Write(TraceLevel.Verbose, "Service("+name +") found!");
						break;
					}
				}                               				        
			}  
			catch
			{

			}

			return service;
		}

		/// <summary>
		/// Verify service's state, if not running then start it.
		/// </summary>
		/// <param name="name">Service object</param>
		/// <returns></returns>
		private bool VerifyServiceState(ServiceInfo service)
		{
			bool goodState = true;

			if (service.Enabled && !service.UserStopped)
			{
				switch(service.Status)
				{
					case ServiceInfo.State.Continuing:
					case ServiceInfo.State.Paused:
					case ServiceInfo.State.Pausing:
						// assume ok, in the process of continuing, pausing.						
						break;

					case ServiceInfo.State.Started:
					case ServiceInfo.State.Starting:
						// assume ok, in the process of starting or already started.
						break;

					case ServiceInfo.State.Stopping:
						// let's wait til next scan, cannot start it anyway.
						break;

					case ServiceInfo.State.Stopped:
						// should be running not not, start it.
						goodState = false;						
						break;

					case ServiceInfo.State.Unknown:
					default:
						// in an unknown state, do not attempt to start it.
						break;
				}
			}

			return goodState;
		}

        private void StartProcess(string name)
        {
            log.Write(TraceLevel.Warning, "Watchdog detected stopped service and restarted " + name);
        }
        #endregion

        #region Alarms

        private bool IsAlarmTriggered(string name)
        {
            return this.setAlarms.ContainsKey(name);
        }

        private void AddAlarm(string name)
        {
            if (IsAlarmTriggered(name))
                return;

            string guid = statsClient.TriggerAlarm(IConfig.Severity.Red, 
                                                IStats.AlarmCodes.General.ServiceUnavailable,
                                                IStats.AlarmCodes.General.Descriptions.ServiceUnavailable, 
                                                name);                       
            this.setAlarms.Add(name, guid);
        }

        private void RemoveAlarm(string name)
        {
            if (!IsAlarmTriggered(name))
                return;
            
            string guid = this.setAlarms[name].ToString();
            statsClient.ClearAlarm(guid);

            this.setAlarms.Remove(name);
        }
        #endregion
    }
}
