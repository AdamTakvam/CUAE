using System;

namespace Metreos.MmsTester.AdapterFramework
{
    /// <summary>
    /// Used to mark classes as an adapter
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class AdapterAttribute : System.Attribute
    {
        private string displayName;

        public AdapterAttribute(string displayName)
        {
            this.displayName = displayName;
        }

        public string DisplayName
        {
            get { return displayName; }
        }
    }
}