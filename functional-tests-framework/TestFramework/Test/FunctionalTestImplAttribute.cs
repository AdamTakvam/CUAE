using System;

namespace Metreos.Samoa.FunctionalTestFramework
{
    /// <summary>
    /// Attribute to be used to decorate the class that implements 
    /// FunctionalTestBase.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class FunctionalTestImplAttribute : System.Attribute
    {
        internal bool isAutomated = true;

        public FunctionalTestImplAttribute()
        {}

        public bool IsAutomated
        {
            get { return this.isAutomated; }
            set { this.isAutomated = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class ExclusiveAttribute : System.Attribute
    {
        internal bool isExclusive = true;

        public ExclusiveAttribute()
        {}

        public bool IsExclusive
        {
            get { return this.isExclusive; }
            set { this.isExclusive = value; }
        }
    }
}
