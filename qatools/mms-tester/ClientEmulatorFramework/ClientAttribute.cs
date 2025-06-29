using System;

namespace Metreos.MmsTester.ClientFramework
{
    /// <summary>
    /// Used to mark classes as clients
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class ClientAttribute : System.Attribute
    {
        private string displayName;

        public ClientAttribute(string displayName)
        {
            this.displayName = displayName;
        }

        public string DisplayName
        {
            get { return displayName; }
        }
    }
}
