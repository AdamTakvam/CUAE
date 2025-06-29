using System;

namespace Metreos.MmsTester.VisualInterfaceFramework
{
    /// <summary>
    /// Used to mark classes as a Visual Interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class VisualInterfaceAttribute : System.Attribute
    {
        private string displayName;

        public VisualInterfaceAttribute(string displayName)
        {
            this.displayName = displayName;
        }

        public string DisplayName
        {
            get { return displayName; }
        }
    }
}