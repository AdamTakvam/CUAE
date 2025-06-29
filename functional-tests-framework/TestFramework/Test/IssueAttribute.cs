using System;

namespace Metreos.Samoa.FunctionalTestFramework
{
    /// <summary> Defines which official issue a FunctionalTestBase class tests for, if any </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class IssueAttribute : System.Attribute
    {
        internal string id = "0";

        public IssueAttribute()
        {}

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}