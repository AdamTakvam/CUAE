using System;
using System.IO;
using System.Text;

namespace BehaviorCore.Actions
{
    class RRD : Action
    {

        private string rrd_file;
        private string work_file;

        public RRD(string workpath, string cuaepath)
        {
            this.rrd_file = cuaepath + "\\StatsService\\RRD\\cuae.rrd";
            this.work_file = workpath + "\\cuae.rrd";
        }

        public override bool CheckPackage()
        {
            if (!File.Exists(this.work_file))
            {
                this.SetError("Statistics database file not found.");
                return false;
            }
            return true;
        }

        public override bool CheckSystem()
        {
            if (!File.Exists(this.rrd_file))
            {
                this.SetError("Statistics database file not found.");
                return false;
            }
            return true;
        }

        public override bool Backup()
        {
            File.Copy(this.rrd_file, this.work_file);
            return File.Exists(this.work_file);
        }

        public override bool Restore()
        {
            File.Delete(this.rrd_file);
            File.Copy(this.work_file, this.rrd_file);
            return File.Exists(this.rrd_file);
        }
    }
}
