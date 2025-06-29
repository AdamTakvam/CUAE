using System;
using System.ServiceProcess;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for ServiceManager.
	/// </summary>
	public class ServiceManager
	{
        static ServiceManager instance = null;
        static readonly object padlock = new object();
        const string RECORD_AGENT_SERVICE_NAME = "MetreosRecordAgentService";

        private ServiceController sc = null;

        public ServiceManager()
        {
            sc = new ServiceController();
            sc.ServiceName = RECORD_AGENT_SERVICE_NAME;
        }

        public static ServiceManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceManager();
                    }
                    return instance;
                }
            }
        }

        public void StartService()
        {
            try
            {
                sc = FindService();
                if (sc != null)
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                        sc.Start();
                }                
            }
            catch
            {
            }
        }

        public void StopService()
        {
            try
            {
                sc = FindService();
                if (sc != null)
                {
                    if (sc.Status == ServiceControllerStatus.Running)
                        sc.Stop();
                }
            }
            catch
            {
            }
        }

        public ServiceController FindService()
        { 
            ServiceController service = null;
            try
            { 				
                ServiceController[] services;
                services = ServiceController.GetServices();
                for (int i = 0; i < services.Length; i++)
                {
                    if(services[i].ServiceName == RECORD_AGENT_SERVICE_NAME)
                    {
                        service = services[i];
                        break;
                    }
                }                               				        
            }  
            catch
            {
            }

            return service;
        }
	}
}

