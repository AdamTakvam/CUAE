using System;

using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute to be used to declare a provider action parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class ActionParamAttribute : System.Attribute
    {
        private Type type;
        private string name;
        private string displayName;
        private useType use;
        private bool allowMultiple = false;
        private string description;
        private string defaultValue;

        public ActionParamAttribute(string name, Type type, bool required, bool allowMultiple, string description)
            : this(name, name, type, required ? useType.required : useType.optional, allowMultiple, description, null) {}

        public ActionParamAttribute(string name, string displayName, Type type, bool required, bool allowMultiple, string description)
            : this(name, displayName, type, required ? useType.required : useType.optional, allowMultiple, description, null) {}

        public ActionParamAttribute(string name, string displayName, Type type, useType use, bool allowMultiple, string description)
        : this(name, displayName, type, use, allowMultiple, description, null) {}

        public ActionParamAttribute(string name, string displayName, Type type, useType use, bool allowMultiple, string description, string defaultValue)
        {
            this.name = name;
            this.displayName = displayName;
            this.type = type;
            this.use = use;
            this.allowMultiple = allowMultiple;
            this.description = description;
            this.defaultValue = defaultValue;
        }

        public string Name { get { return name; } }

        public string DisplayName { get { return displayName; } }

        public Type Type { get { return type; } }

        public useType Use { get { return use; } }

        public bool AllowMultiple { get { return allowMultiple; } }

        public string Description { get { return description; } }

        public string DefaultValue { get { return defaultValue; } }
    }
}


