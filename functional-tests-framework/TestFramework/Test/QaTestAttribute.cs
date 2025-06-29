using System;

namespace Metreos.Samoa.FunctionalTestFramework
{
    /// <summary> Defines which test issue a FunctionalTestBase class tests for, if any </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class QaTestAttribute : System.Attribute
    {
        internal string id = "0";

        public QaTestAttribute()
        {}

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}