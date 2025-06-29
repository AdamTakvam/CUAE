using System;

using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute used to declare a custom method on a Native type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited=false, AllowMultiple=true)]
    public class TypeMethodAttribute : System.Attribute
    {
        private string description;

        public TypeMethodAttribute(string description)
        {
            this.description = description;
        }

        public string Description { get { return description; } }
    }
}


