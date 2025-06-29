using System;

using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute used to declare that the specified type tcan be assigned to containing native type.
    /// Typically used to decorate the IVariable.Parse(object) method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class TypeInputAttribute : System.Attribute
    {
        private string type;
        private string description; 

        public TypeInputAttribute(string type, string description)
        {
            this.type = type;
            this.description = description;
        }

        public string Type { get { return type; } }

        public string Description { get { return description; } }
    }
}


