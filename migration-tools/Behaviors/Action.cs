using System;
using System.Text;

namespace BehaviorCore
{
    /// <summary>
    /// An abstraction for Action objects which define each task
    /// </summary>
    abstract public class Action
    {
        protected string workpath;
        protected string cuaepath;

        private string error;
        private bool run;

        public string Error
        {
            get
            {
                return this.error;
            }
        }

        public bool CanRun
        {
            get
            {
                return this.run;
            }
        }

        protected Action(string workpath, string cuaepath):
            this()
        {
            this.workpath = workpath;
            this.cuaepath = cuaepath;
        }

        protected Action()
        {
            this.error = null;
            this.run = true;
        }

        protected void SetError(string message)
        {
            this.error = message;
            this.run = false;
        }

        abstract public bool CheckSystem();

        abstract public bool CheckPackage();

        abstract public bool Backup();

        abstract public bool Restore();

    }
}
