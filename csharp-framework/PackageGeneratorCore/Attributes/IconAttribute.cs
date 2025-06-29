using System;

using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute to be used to declare an icon.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class IconAttribute : System.Attribute
    {
        private string name;
        private iconTypeType type;

        public IconAttribute(string name, iconTypeType type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name { get { return name; } }

        public iconTypeType Type { get { return type; } }
    }
}

