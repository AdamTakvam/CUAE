using System;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute used to declare a package.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class PackageDeclAttribute : System.Attribute
    {
        private string _namespace;
        private string description;

        public PackageDeclAttribute(Type providerType, string description)
            : this(providerType.Namespace, null) {}

        public PackageDeclAttribute(string _namespace)
            : this(_namespace, null) {}

        public PackageDeclAttribute(string _namespace, string description)
        {
            this._namespace = _namespace;
            this.description = description;
        }

        public string Namespace { get { return _namespace; } }

        public string Description { get { return description; } }
    }
}

