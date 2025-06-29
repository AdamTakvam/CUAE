using System;

namespace Metreos.ApplicationFramework.ActionParameters
{
    /// <summary>
    /// Value type used to specify a parameter of an action
    /// </summary>
    public abstract class ActionParamBase
    {
        public string name;
        public object Value;
        
        public ActionParamBase(string name)
        {
            System.Diagnostics.Debug.Assert(name != null, "Cannot create action param with no name.");

            this.name = name;
            Value = null;
        }

        public abstract ActionParamBase Clone();

        public virtual void Reset()
        {
            Value = null;
        }
    }
}
