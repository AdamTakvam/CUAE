using System;
using System.Collections;
using System.Diagnostics;

using Metreos.ApplicationFramework.Collections;

namespace Metreos.ApplicationFramework.Actions
{
    public abstract class ActionBase : ScriptElementBase
    {
        public string name;

        // ResultData field name -> ResultDataBase
        public ResultDataCollection resultData;
        
        // List of ActionParam's (object)
        public ActionParamCollection actionParams;
        
        // Return value -> next action ID
        public MStringDictionary nextActions;

        protected ActionBase(string name)
            : base()
        {
            Debug.Assert(name != null, "Action name cannot be null");

            this.name = name;

            actionParams = new ActionParamCollection();
            nextActions = new MStringDictionary();
            resultData = new ResultDataCollection();
        }

        protected ActionBase Clone(ActionBase newAction)
        {
            newAction.resultData = this.resultData.Clone();
            newAction.actionParams = this.actionParams.Clone();
            newAction.nextActions = this.nextActions.Clone();

            return newAction;
        }

		protected virtual void Reset()
		{
            this.actionParams.Reset();
		}

        protected virtual void Clear()
        {
            actionParams.Clear();
            nextActions.Clear();
            resultData.Clear();
        }
    }
}
