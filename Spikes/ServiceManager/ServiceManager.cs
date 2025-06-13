using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;

using Metreos.LoggingFramework;
using Metreos.Utilities;

namespace Metreos.AppServer.ProviderManager
{
    public sealed class ServiceManager
    {
        #region Constants

        private abstract class SCManagerDefs
        {
            // Service Control Manager object specific access types 
            public const int StdRightsRequired  = 0xF0000;
            public const int Connect            = 0x1;
            public const int CreateService      = 0x2;
            public const int EnumService        = 0x4;
            public const int Lock               = 0x8;
            public const int QueryLockStatus    = 0x10;
            public const int ModifyBootConfig   = 0x20; 
            public const int AllAccess = StdRightsRequired | 
                Connect | CreateService | EnumService | 
                Lock | QueryLockStatus | ModifyBootConfig;
        }

        private abstract class ServiceDefs
        {
            // Service Access types 
            public const int QueryConfig        = 0x1;
            public const int ChangeConfig       = 0x2;
            public const int QueryStatus        = 0x4;
            public const int EnumDependents     = 0x8;
            public const int Start              = 0x10;
            public const int Stop               = 0x20;
            public const int PauseContinue      = 0x40;
            public const int Interrogate        = 0x80;
            public const int UserDefinedCtrl    = 0x100;
            public const int AllAccess = SCManagerDefs.StdRightsRequired | 
                QueryConfig | ChangeConfig | QueryStatus | EnumDependents | 
                Start | Stop | PauseContinue | Interrogate | UserDefinedCtrl;

            public const int Win32OwnProcess    = 0x10;
            public const int AutoStart          = 0x2;
            public const int ErrorNormal        = 0x1;
        }
        #endregion

        #region P/Invokes
        /*
    Declare Auto Function OpenSCManager Lib "advapi32.dll" (ByVal sMachName 
As String, ByVal sDbName As String, ByVal iAccess As Integer) As Integer 
    Declare Auto Function CloseServiceHandle Lib "advapi32.dll" (ByVal 
sHandle As Integer) As Integer 
    Declare Auto Function CreateService Lib "advapi32.dll" (ByVal hSCM As 
Integer, ByVal sName As String, ByVal sDisplay As String, ByVal iAccess As 
Integer, ByVal iSvcType As Integer, ByVal iStartType As Integer, ByVal 
iError As Integer, ByVal sPath As String, ByVal sGroup As String, ByVal 
iTag As Integer, ByVal sDepends As String, ByVal sUser As String, ByVal 
sPass As String) As Integer 
    Declare Auto Function OpenService Lib "advapi32.dll" (ByVal hSCManager 
As Integer, ByVal lpServiceName As String, ByVal dwDesiredAccess As 
Integer) As Integer 
    Declare Auto Function DeleteService Lib "advapi32.dll" (ByVal hSvc As 
Integer) As Boolean 
    Declare Auto Function GetLastError Lib "KERNEL32" () As Integer 
         */

        [DllImport("advapi32.dll")]
        private static extern int OpenSCManager(string sMachName, string sDbName, int iAccess);

        [DllImport("advapi32.dll")]
        private static extern int CloseServiceHandle(int sHandle);

        [DllImport("advapi32.dll")]
        private static extern int CreateService(int hSCM, string sName, string sDisplay, 
            int iAccess, int iSvcType, int iStartType, int iError, string sPath, 
            string sGroup, int iTag, string sDepends, string sUser, string sPass);

        [DllImport("advapi32.dll")]
        private static extern int OpenService(int hSCManager, string lpServiceName, int dwDesiredAccess);

        [DllImport("advapi32.dll")]
        private static extern bool DeleteService(int hSvc);

        [DllImport("kernel32.dll")]
        private static extern int GetLastError();

        #endregion
        
        private readonly LogWriter log;

        public ServiceManager(LogWriter log)
        {
            this.log = log;
        }

        public bool InstallService(FileInfo serviceFile, string serviceName, string displayName, out string failReason) 
        {
            failReason = null;

            if(ServiceInstalled(serviceName))
            {
                log.Write(TraceLevel.Info, "Service '{0}' already installed", serviceName);

                try { StartService(serviceName); }
                catch(Exception e)
                {
                    failReason = Exceptions.FormatException(e);
                    return false;
                }
                return true;
            }

            int hSCM = OpenSCManager(null, null, SCManagerDefs.AllAccess);
            if(hSCM == 0)
            {
                failReason = "Unable to open Service Control Manager: " + GetLastError();
                return false;
            }

            int hSvc = CreateService(hSCM, serviceName, displayName, ServiceDefs.AllAccess, 
                ServiceDefs.Win32OwnProcess, ServiceDefs.AutoStart, ServiceDefs.ErrorNormal, 
                serviceFile.FullName, null, 0, null, null, null);

            if(hSvc == 0)
            {
                CloseServiceHandle(hSvc);
                failReason = "Could not create service handle: " + GetLastError(); 
                return false;
            } 

            CloseServiceHandle(hSvc);
            CloseServiceHandle(hSCM);

            log.Write(TraceLevel.Info, "Service '{0}' installed successfully", serviceName);

            try { StartService(serviceName); }
            catch(Exception e)
            {
                failReason = Exceptions.FormatException(e);
                return false;
            }
            return true;
        }


        public bool UninstallService(string serviceName, out string failReason) 
        {
            failReason = null;

            if(!ServiceInstalled(serviceName))
            {
                log.Write(TraceLevel.Info, "Service '{0}' not installed", serviceName);
                return true;
            }

            try { StopService(serviceName); }
            catch(Exception e)
            {
                failReason = e.Message;
                return false;
            }

            log.Write(TraceLevel.Info, "Service '{0}' stopped successfully", serviceName);

            int hSCM = OpenSCManager(null, null, SCManagerDefs.AllAccess);
            if(hSCM == 0)
            {
                failReason = String.Format("Unable to open Service Control Manager '{0}': {1}", serviceName, GetLastError()); 
                return false; 
            }

            int hSvc = OpenService(hSCM, serviceName, ServiceDefs.AllAccess);
            if(hSvc == 0)
            {
                CloseServiceHandle(hSCM);
                failReason = String.Format("Unable to open service '{0}': {1}", serviceName, GetLastError()); 
                return false;
            }

            if(!DeleteService(hSvc))
            {
                failReason = String.Format("Unable to delete service '{0}': {1}", serviceName, GetLastError());
                CloseServiceHandle(hSvc);
                CloseServiceHandle(hSCM);
                return false;
            }

            log.Write(TraceLevel.Info, "Service '{0}' uninstalled successfully", serviceName);

            CloseServiceHandle(hSvc);
            CloseServiceHandle(hSCM);
            return true;
        }

        private bool ServiceInstalled(string serviceName)
        {
            bool found = false;
            foreach(ServiceController sc in ServiceController.GetServices())
            {
                Console.Write(sc.ServiceName);
                if(sc.ServiceName == serviceName)
                {
                    Console.WriteLine("   <---");
                    found = true;
                }
                else
                    Console.WriteLine();
            }
            return found;
        }

        private void StartService(string serviceName)
        {
            ServiceController svc = new ServiceController(serviceName);
            if(svc.Status == ServiceControllerStatus.Running)
                return;

            svc.Start();
            svc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 2, 0));
            if(svc.Status != ServiceControllerStatus.Running)
                throw new System.ServiceProcess.TimeoutException(String.Format("Service '{0}' startup timed out.", serviceName));
        }

        private void StopService(string serviceName)
        {
            ServiceController svc = new ServiceController(serviceName);
            if(svc.Status == ServiceControllerStatus.Stopped) 
                return;

            svc.Stop();
            svc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 2, 0));
            if(svc.Status != ServiceControllerStatus.Stopped)
                throw new System.ServiceProcess.TimeoutException(String.Format("Service '{0}' startup timed out.", serviceName));
        }
    }
}
