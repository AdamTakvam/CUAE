using System;
using System.Text;
using System.IO;

namespace BehaviorCore.Actions
{
    class MmsConfig: Action
    {
        private string MmmsConfigFile;
        private string WorkFile;

        public MmsConfig(string workPath, string cuaePath)
        {
            this.MmmsConfigFile = cuaePath + "\\MediaServer\\mmsconfig.properties";
            this.WorkFile = workPath + "\\mmsconfig.properties";
        }

        public override bool CheckPackage()
        {
            if (!File.Exists(this.WorkFile))
            {
                this.SetError("Media engine configuration file not found.");
                return false;
            }
            return true;
        }

        public override bool CheckSystem()
        {
            if (!File.Exists(this.MmmsConfigFile))
            {
                this.SetError("Media engine configuration file not found.");
                return false;
            }
            return true;
        }

        public override bool Backup()
        {
            File.Copy(this.MmmsConfigFile, this.WorkFile);
            return File.Exists(this.WorkFile);
        }

        public override bool Restore()
        {
            File.Delete(this.MmmsConfigFile);
            File.Copy(this.WorkFile, this.MmmsConfigFile);
            return File.Exists(this.MmmsConfigFile);
        }
    }
}
