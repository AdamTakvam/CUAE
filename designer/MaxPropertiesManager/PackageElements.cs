using System;
using System.Collections;

using Metreos.Max.Framework.Satellite.Property;

namespace PropertyGrid.Core
{
    #region PackageElement Definition
    /// <summary>
    ///     Defines the lowest abstraction of an element found in a package.
    ///     A name and description are things that are guaranteed to be present or at least
    ///     specificable in a visual environment
    /// </summary>
    public abstract class PackageElement
    {
        public PackageElement(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public string Name
        { 
            get{ return name; } 
            set{ name = value; }
        }
		
        public string Description
        { 
            get{ return description; } 
            set{ description = value; }
        }

        /// <summary> Added for use in the Visual Designer core library </summary>
        public Object Tag
        { 
            get{ return tag; } 
            set{ tag = value; }
        }
 
        protected string name;
        protected string description;
        protected Object tag;
    }
    #endregion 

    #region Action Definition

    public class MaxPmAction: PackageElement
    {
        
        public MaxPmAction(string name, string displayName, string packagename, string description, string[] asyncCallbacks, bool allowCustomParameters, 
            bool final) : base (name, description)
        {
            this.packagename = packagename;
            this.asyncCallbacks = asyncCallbacks;
            this.allowCustomParameters = allowCustomParameters;
            this.final = final;
            if(displayName == null || displayName == String.Empty)
            {
                this.displayName = name;
            }
            else
            {
                this.displayName = displayName;
            }
        }

        public MaxPmAction(string name, string displayName, string packagename, string description, string[] asyncCallbacks, bool allowCustomParameters, 
            bool final, ActionParameter[] parameters, ResultDatum[] resultData, ReturnValue returnValue,  
            ActionType type) : base (name, description)
        {
            this.packagename = packagename;
            this.asyncCallbacks = asyncCallbacks;
            this.allowCustomParameters = allowCustomParameters;
            this.final = final;
            this.parameters = parameters;
            this.resultData = resultData;
            this.returnValue = returnValue;
            this.type = type;
            if(displayName == null || displayName == String.Empty)
            {
                this.displayName = name;
            }
            else
            {
                this.displayName = displayName;
            }
        }

        public ActionParameter[] Parameters{ get { return parameters; } set { parameters = value; }}
        public ResultDatum[] ResultData{ get { return resultData; } set { resultData = value; }}
        public ReturnValue ReturnValue{ get { return returnValue; } set { returnValue = value; }}
        public ActionType Type{ get { return type; } set { type = value; }}
        public bool AllowCustomParameters{ get{ return allowCustomParameters; } set { allowCustomParameters = value; }}
        public bool Final{ get { return final; } set { final = value; }}
        public string[] AsyncCallbacks{ get { return asyncCallbacks; } set { asyncCallbacks = value; }}
        public string PackageName { get { return packagename; } set { packagename = value; } }
        public string DisplayName { get { return displayName; } set { displayName = value; } }

        protected ActionParameter[] parameters;
        protected ResultDatum[] resultData;
        protected ReturnValue returnValue;
        protected ActionType type;
        protected bool allowCustomParameters;
        protected bool final;
        protected string[] asyncCallbacks;
        protected string packagename;
        protected string displayName;

        public ActionParameter GetActionParameter(string parameterName)
        {
            if(parameters == null || parameters.Length == 0) return null;

            foreach(ActionParameter parameter in parameters)
            {
                if(String.Compare(parameter.Name, parameterName, true) == 0) return parameter;
            }

            return null;
        }
    }

    public class ActionParameter
    {
        public ActionParameter(string name, string displayName, string description, string[] enumValues,  string type, bool allowMultiple)
        {
            this.name = name;
            this.displayName = displayName;
            this.description = description;
            this.type = type;
            this.multiple = allowMultiple;
            this.enumValues = enumValues != null ? enumValues.Length != 0 ? enumValues : null : null;
        }

        public ActionParameter(string name, string displayName, string description, string[] enumValues, string type, bool allowMultiple, Use use) : this(name, displayName, description, enumValues, type, allowMultiple)
        {
            this.use = use;
        }

        public string Name{ get{ return name; } set{ name = value; }}
        public string Description{ get{ return description; } set{ description = value; }}
        public string Type{ get{ return type; } set{ type = value; }}
        public bool AllowMultiple { get { return multiple; } set { multiple = value; } }
        public Use Use{ get{ return use; } set{ use = value; }}
        public string DisplayName { get { return displayName; } set { displayName = value; } }
        public string[] EnumValues { get { return enumValues; } set { enumValues = value; } }

        protected string name;
        protected string description;
        protected string type;
        protected bool   multiple;
        protected Use    use;
        protected string displayName;
        protected string[] enumValues;
    }

    public class ReturnValue
    { 
        public string Description{ get { return description; } }
        public string[] Values { get { return values; } }

        protected string description;
        protected string[] values;

        public ReturnValue()
        {
            this.description = Defaults.RETURN_VALUE_DESC;
            this.values =  new string[] {"default", "success", "failure", "timeout"};
        }

        public ReturnValue(string description, string[] values)
        {
            this.description = description != null ? description :  Defaults.RETURN_VALUE_DESC;
            if(values != null)
            {
                ArrayList defaultAdded = new ArrayList();
                defaultAdded.AddRange(values);
                defaultAdded.Add(Defaults.USER_FRIENDLY_DEFAULT);
                this.values = new string[defaultAdded.Count];
                defaultAdded.CopyTo(this.values);
            }
            else
            {
                this.values = values != null ? values : new string[] {"default", "success", "failure", "timeout"} ;
            }
        }
    }

    public class ResultDatum
    {
        public ResultDatum(string name, string displayName, string description, string type)
        {
            this.name = name;
            this.displayName = displayName;
            this.description = description;
            this.type = type;
        }

        public string Name{ get{ return name; } set{ name = value; }}
        public string DisplayName { get { return displayName; } set { displayName = value; } }
        public string Description{ get{ return description; } set{ description = value; }}
        public string Type{ get{ return type; } set{ type = value; }}

        protected string name;
        protected string displayName;
        protected string description;
        protected string type;
    }

    public enum ActionType
    {
        provider,
        native,
        appControl,
        Code
    }

    public enum ReturnValueType
    {
        success,
        boolean,
        custom,
        yes_no
    }

    public enum Use
    {
        required,
        optional
    }

    #endregion Action Definition

    #region Event Definition

    public class MaxPmEvent : PackageElement
    {
        public MaxPmEvent(string name, string description, string expects) : base(name, description)
        {
            this.expects = expects;
        }

        public MaxPmEvent(string name, string description, string expects, EventType type, 
            EventParameter[] parameters) : base(name, description)
        {
            this.expects = expects;
            this.type = type;
            this.parameters = parameters;
        }

        public EventParameter[] Parameters{ get{ return parameters; } set{ parameters = value; }}
        public EventType Type{ get{ return type; } set{ type = value; }}
        public string Expects{ get{ return expects; } set{ expects = value; }}
        
        protected EventParameter[] parameters;
        protected EventType type;
        protected string expects;
  
    }

    public class EventParameter
    {
        public EventParameter(string name, string displayName, string description, string[] enumValues, string type, bool guaranteed)
        {
            this.name = name;
            this.displayName = displayName;
            this.description = description;
            this.type = type;
            this.guaranteed = guaranteed;
            this.enumValues = enumValues != null ? enumValues.Length != 0 ? enumValues : null : null;
        }

        public string Name{ get{ return name; } set{ name = value; }}
        public string DisplayName { get { return displayName; } set { displayName = value; } }
        public string Description{ get{ return description; } set{ description = value; }}
        public string Type{ get{ return type; } set{ type = value; }}
        public bool Guaranteed{ get{ return guaranteed; } set{ guaranteed = value; }}
        public string[] EnumValues { get { return enumValues; } set { enumValues = value; } }

        protected string name;
        protected string displayName;
        protected string description;
        protected string type;
        protected bool guaranteed;
        protected string[] enumValues;
    }

    public enum EventType
    {
        triggering,
        nonTriggering,
        hybrid,
        asyncCallback,
        Unknown
    }
  
    #endregion Event Definition
}
