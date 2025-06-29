using System;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute to be used to declare an action handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class ActionAttribute : System.Attribute
    {
        private actionTypeType type;
        private string name;
        private string[] asyncCallbacks;
        private bool allowCustomParams;
        private string displayName;
        private string description;

        /// <summary>
        /// Native action constructor
        /// </summary>
        public ActionAttribute(string nativeActionName, bool allowCustomParams, string displayName, string description)
            : this(nativeActionName, actionTypeType.native, allowCustomParams, displayName, description, false, null) {}

        /// <summary>
        /// Provider action constructor
        /// </summary>
        public ActionAttribute(string providerActionName, bool allowCustomParams, string displayName, string description, bool async)
            : this(providerActionName, actionTypeType.provider, allowCustomParams, displayName, description, async, null) {}

        /// <summary>
        /// Provider action constructor with unsolicited callbacks
        /// </summary>
        public ActionAttribute(string providerActionName, bool allowCustomParams, string displayName, string description, bool async, string[] unsolicitedCallbacks)
        : this(providerActionName, actionTypeType.provider, allowCustomParams, displayName, description, async, unsolicitedCallbacks) {}

        /// <summary>
        /// Raw constructor
        /// </summary>
        public ActionAttribute(string name, actionTypeType type, bool allowCustomParams, string displayName, string description, bool async, string[] unsolicitedCallbacks)
        {
            this.name = Namespace.GetName(name);
            this.type = type;
            this.allowCustomParams = allowCustomParams;
            this.description = description;

            if((displayName == null) || (displayName == ""))
            {
                this.displayName = name;
            }
            else
            {
                this.displayName = displayName;
            }

            // Async Callbacks with possible unsolicited events
			if(async && (type == actionTypeType.provider))
			{
				int asyncCallbackCount = 2 + (unsolicitedCallbacks != null ? unsolicitedCallbacks.Length : 0 );
				this.asyncCallbacks = new string[asyncCallbackCount];
				this.asyncCallbacks[0] = String.Format("{0}_{1}", name, IApp.RESULT_COMPLETE);
				this.asyncCallbacks[1] = String.Format("{0}_{1}", name, IApp.RESULT_FAILED);
				if(unsolicitedCallbacks != null)
				{
					unsolicitedCallbacks.CopyTo(asyncCallbacks, 2);
				}
			} // Unsolicited events only
			else if(unsolicitedCallbacks != null)
			{
				this.asyncCallbacks = new string[unsolicitedCallbacks.Length];
				unsolicitedCallbacks.CopyTo(asyncCallbacks, 0);
			}
			else
			{
				this.asyncCallbacks = null;
			}
        }

        public string Name { get { return name; } }

        public actionTypeType Type { get { return type; } }

        public string[] AsyncCallbacks { get { return asyncCallbacks; } }

        public bool AllowCustomParams { get { return allowCustomParams; } }

        public string DisplayName { get { return displayName; } }

        public string Description { get { return description; } }
    }
}

