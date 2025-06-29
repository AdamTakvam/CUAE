using System;
using System.Text;
using System.IO;
using System.ServiceProcess;


namespace BehaviorCore.Actions
{
    class Licenses : Action
    {
        private const string    CUAE_LICENSE_SERVICE = "CUAE License Server";
        private const int       SERVICE_WAIT_TIMEOUT = 15;

        private string work_directory;
        private string license_directory;

        public Licenses(string workPath, string cuaePath)
        {
            this.work_directory = workPath + "\\Licenses";
            this.license_directory = cuaePath + "\\LicenseServer\\Licenses";
        }

        public override bool CheckPackage()
        {
            if (!Directory.Exists(this.work_directory))
            {
                this.SetError("There are no licenses in this package.");
                return false;
            }
            return true;
        }

        public override bool CheckSystem()
        {
            if (!Directory.Exists(this.license_directory))
            {
                this.SetError(String.Format("{0} does not exist.  Cannot find licenses.", this.license_directory));
                return false;
            }
            return true;
        }

        public override bool Backup()
        {
            Utils.DirectoryCopy(this.license_directory, this.work_directory, true);
            return Directory.Exists(this.work_directory);
        }

        public override bool Restore()
        {
            Directory.Delete(this.license_directory, true);
            Utils.DirectoryCopy(this.work_directory, this.license_directory, true);
            if (Directory.Exists(this.license_directory))
            {
                try
                {
                    ServiceController sc = new ServiceController(CUAE_LICENSE_SERVICE);
                    TimeSpan t = TimeSpan.FromSeconds(SERVICE_WAIT_TIMEOUT);
                    if (sc.CanStop)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, t);
                    }

                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, t);
                    }
                    else
                    {
                        throw new System.ServiceProcess.TimeoutException();
                    }
                }
                catch (System.ServiceProcess.TimeoutException)
                {
                    Console.WriteLine("The CUAE License Server service could not be restarted.");
                    Console.WriteLine("This service needs to be restarted for the restored licenses to take effect.");
                }
                catch (SystemException se)
                {
                    Console.WriteLine("An attempt to restart the CUAE License Server service was aborted due to a system error:");
                    Console.WriteLine(String.Format("     {0}", se.Message));
                    Console.WriteLine("Please restart the CUAE License Server service manually for the restored licenses to take effect.");
                }
                return true;
            }
            else
            {
                this.SetError(String.Format("Restoring of licenses failed: could not create/find the directory {0}", this.license_directory));
                return false;
            }
        }
    }
}
