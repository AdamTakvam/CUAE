using System;

using Metreos.Interfaces;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute to be used to declare provider result data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class ResultDataAttribute : System.Attribute
    {
        private string type;
        private string displayName;
        private string name;
        private string description;

        public ResultDataAttribute()
            : this(Defaults.RESULT_DATA_NAME, Defaults.RESULT_DATA_NAME, Defaults.RESULT_DATA_TYPE, null) {}

        public ResultDataAttribute(string name)
            : this(name, name, Defaults.RESULT_DATA_TYPE, null) {}

        public ResultDataAttribute(Type type, string description)
            : this(Defaults.RESULT_DATA_NAME, Defaults.RESULT_DATA_NAME, type, description) {}

        public ResultDataAttribute(string name, Type type, string description)
            : this(name, name, type, description) {}

        public ResultDataAttribute(string name, string displayName, Type type, string description)
        {
            this.name = name;
            this.displayName = displayName;
            this.type = type.FullName;
            this.description = description;
        }

        public string Name { get { return name; } }

        public string DisplayName { get { return displayName; } }

        public string Type { get { return type; } }

        public string Description { get { return description; } }
    }
}
