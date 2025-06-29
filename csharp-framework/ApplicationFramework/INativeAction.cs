using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.ApplicationFramework
{
	/// <summary>
    /// Actions that can utilize the .net framework
    /// without the rest of the AppServer stack
	/// </summary>
	public interface INativeAction
	{
        LogWriter Log { set; }

        bool ValidateInput();

        string Execute(SessionData sessionData, IConfigUtility configUtility);

        void Clear();
    }
    
    /// <summary>
    /// This attribute specifies that a native action property represents an native action ActionParameter field.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited=false, AllowMultiple=false)]
	public class ActionParamFieldAttribute : Attribute
	{
        public string displayName;
		public string description;
		public bool mandatory;
        public string defaultValue;

		public ActionParamFieldAttribute()
			: this(null, null, true, null) {}

		public ActionParamFieldAttribute(string description)
			: this(null, description, true, null) {}

		public ActionParamFieldAttribute(bool mandatory)
			: this(null, null, mandatory, null) {}

        public ActionParamFieldAttribute(string description, bool mandatory)
            : this(null, description, mandatory, null) {}

        public ActionParamFieldAttribute(string displayName, string description, bool mandatory)
            : this(displayName, description, mandatory, null) {}

		public ActionParamFieldAttribute(string displayName, string description, bool mandatory, string defaultValue)
		{
            this.displayName = displayName;
			this.description = description;
			this.mandatory = mandatory;
            this.defaultValue = defaultValue;
		}
	}

    /// <summary>
    /// This attribute specifies that a native action property represents a native action ResultData field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited=false, AllowMultiple=false)]
    public class ResultDataFieldAttribute : Attribute
    {
        public string displayName;
        public string description;

        public ResultDataFieldAttribute() 
            : this(null, null) {}

        public ResultDataFieldAttribute(string description)
            : this(null, description) {}

        public ResultDataFieldAttribute(string displayName, string description)
        {
            this.displayName = displayName;
            this.description = description;
        }
    }
}
