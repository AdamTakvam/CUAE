using System;
using System.Text;
using System.IO;

namespace BehaviorCore.Actions
{
    class Applications : Action
    {
        private string app_directory;
        private string work_directory;

        public Applications(string workpath, string cuaepath)
        {
            this.app_directory = cuaepath + "\\AppServer\\Applications";
            this.work_directory = workpath + "\\Applications";
        }

        public override bool CheckSystem()
        {
            if (!Directory.Exists(this.app_directory))
            {
                this.SetError(String.Format("{0} does not exist.  Cannot find applications.", this.app_directory));
                return false;
            }
            return true;            
        }

        public override bool CheckPackage()
        {
            if (!Directory.Exists(this.work_directory))
            {
                this.SetError("No applications exist in this package.");
                return false;
            }
            return true;
        }

        public override bool Backup()
        {
            Utils.DirectoryCopy(this.app_directory, this.work_directory, true);
            return Directory.Exists(this.work_directory);
        }

        public override bool Restore()
        {
            Directory.Delete(this.app_directory);
            Utils.DirectoryCopy(this.work_directory, this.app_directory, true);
            return Directory.Exists(this.app_directory);
        }
    }
}
