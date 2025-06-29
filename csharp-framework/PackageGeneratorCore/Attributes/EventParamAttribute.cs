using System;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute to be used to declare an event parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class EventParamAttribute : System.Attribute
    {
        private Type type;
        private string name;
        private string displayName;
        private bool guaranteed;
        private string description;

        public EventParamAttribute(string name)
            : this(name, name, Defaults.EVENT_PARAM_TYPE, Defaults.EVENT_PARAM_GUARANTEED, null) {}

        public EventParamAttribute(string name, Type type)
            : this(name, name, type, Defaults.EVENT_PARAM_GUARANTEED, null) {}

        public EventParamAttribute(string name, bool guaranteed)
            : this(name, name, Defaults.EVENT_PARAM_TYPE, guaranteed, null) {}

        public EventParamAttribute(string name, Type type, bool guaranteed, string description)
            : this(name, name, type, guaranteed, description) {}

        public EventParamAttribute(string name, string displayName, Type type, bool guaranteed, string description)
        {
            this.name = name;
            this.displayName = displayName;
            this.type = type;
            this.guaranteed = guaranteed;
            this.description = description;
        }

        public string Name { get { return name; } }

        public string DisplayName { get { return displayName; } }

        public Type Type { get { return type; } }

        public bool Guaranteed { get { return guaranteed; } }

        public string Description { get { return description; } }
    }
}