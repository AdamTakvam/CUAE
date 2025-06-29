using System;

namespace Metreos.Samoa.FunctionalTestFramework
{
    /// <summary>
    /// Attribute to be used to decorate the class that implements 
    /// FunctionalTestBase.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class FunctionalGroupAttribute : System.Attribute
    {
        internal string group = "Generic";

        public FunctionalGroupAttribute()
        {}

        public string Group
        {
            get { return this.group; }
            set { this.group = value; }
        }
    }
}