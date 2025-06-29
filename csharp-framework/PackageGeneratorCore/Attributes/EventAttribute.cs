using System;

using Metreos.Utilities;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Interfaces;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute used to declare an event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class EventAttribute : System.Attribute
    {
        private eventTypeType type;
        private string name;
        private string expects;
        private string description;
        private string displayName;

        // Async action callback constructors
        public EventAttribute(string asyncActionName, bool asyncSuccess)
            : this(asyncActionName, null, null, null, asyncSuccess) {}

        public EventAttribute(string asyncActionName, string expects, string displayName, string description, bool asyncSuccess)
            : this(String.Format("{0}_{1}", asyncActionName, asyncSuccess ? IApp.RESULT_COMPLETE : IApp.RESULT_FAILED), 
                eventTypeType.asyncCallback, expects, displayName, description) {}

        // Regular event contructor
        public EventAttribute(string name, bool triggering, string expects, string displayName, string description)
            : this(name, triggering ? eventTypeType.triggering : eventTypeType.nontriggering, 
                expects, displayName, description) {}

        // Raw constructor
        public EventAttribute(string name, eventTypeType type, string expects, string displayName, string description)
        {
            this.name = Namespace.GetName(name);
            this.type = type;
            this.expects = expects;
            this.description = description;

            if((displayName == null) || (displayName == ""))
            {
                this.displayName = name;
            }
            else
            {
                this.displayName = displayName;
            }
        }

        public string Name { get { return name; } }

        public eventTypeType Type { get { return type; } }

        public string Expects { get { return expects; } }

        public string DisplayName { get { return displayName; } }

        public string Description { get { return description; } }
    }
}
