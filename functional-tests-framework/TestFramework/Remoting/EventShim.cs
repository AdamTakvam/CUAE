using System;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	public class EventShim : MarshalByRefObject
	{
		private CommandMessageDelegate commandTarget;
        private ActionMessageDelegate actionTarget;

        private EventShim( CommandMessageDelegate target )
        {
            this.commandTarget += target;
        }

        private EventShim( ActionMessageDelegate target )
        {
            this.actionTarget += target;
        }

        public void CommandMessageShim(CommandMessage im)
        {
            this.commandTarget(im);
        }

        public void ActionMessageShim(ActionMessage im)
        {
            this.actionTarget(im);
        }


        public override object InitializeLifetimeService()
        {
            return null;
        }


        public static CommandMessageDelegate CreateCommand( CommandMessageDelegate target )
        {
            EventShim shim = new EventShim(target);
            return new CommandMessageDelegate(shim.CommandMessageShim);
        }	

        public static ActionMessageDelegate CreateAction( ActionMessageDelegate target )
        {
            EventShim shim = new EventShim(target);
            return new ActionMessageDelegate(shim.ActionMessageShim);
        }
	}
}
