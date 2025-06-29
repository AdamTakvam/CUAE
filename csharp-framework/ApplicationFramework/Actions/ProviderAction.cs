using System;

namespace Metreos.ApplicationFramework.Actions
{
	public class ProviderAction : ActionBase
	{
        public int timeout;

		public ProviderAction(string name)
            : base(name)
		{
            timeout = -1;
		}

        public override ScriptElementBase Clone()
        {
            ProviderAction newAction = base.Clone(new ProviderAction(this.name)) as ProviderAction;
            newAction.timeout = this.timeout;

            return newAction;
        }
	}
}
