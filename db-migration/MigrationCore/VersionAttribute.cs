using System;
using System.Collections.Generic;
using System.Text;

namespace MigrationCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class VersionAttribute : Attribute
    {

        private string      version;

        public VersionAttribute(string version)
        {
            this.version = version;
        }

        public string Version
        {
            get
            {
                return this.version;
            }
        }

    }
}
