using System;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute to be used to declare a native type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=false)]
    public class TypeDeclAttribute : System.Attribute
    {
        private string displayName;
        private string description;
        private bool serializable;

        public TypeDeclAttribute(string description)
            : this(null, description, true) {}

        public TypeDeclAttribute(string displayName, string description, bool serializable)
        {
            this.displayName = displayName;
            this.description = description;
            this.serializable = serializable;
        }

        public string DisplayName { get { return displayName; } }

        public string Description { get { return description; } }

        public bool Serializable { get { return serializable; } }
    }
}

