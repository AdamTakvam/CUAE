using System;
using System.Reflection;
using System.Diagnostics;

using Metreos.ApplicationFramework.Assembler;

namespace Metreos.ApplicationFramework.Actions
{
	/// <summary>
	/// Summary description for NativeAction.
	/// </summary>
	public class NativeAction : ActionBase
	{
        // Native action implementation instance
        public INativeAction actionInstance;

		public NativeAction(string name, INativeAction actionInstance)
            : base(name)
		{
            if(actionInstance == null)
            {
                throw new CodeNotFoundException("Could not instantiate native action: " + name);
            }

            this.actionInstance = actionInstance;
		}

        public override ScriptElementBase Clone()
        {
            INativeAction newActionInstance = LibraryManager.GetObjFromAssembly(this.name) as INativeAction;
            
            return base.Clone(new NativeAction(this.name, newActionInstance)) as NativeAction;
        }
	}
}
