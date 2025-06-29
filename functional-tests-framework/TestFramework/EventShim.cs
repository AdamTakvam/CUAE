using System;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	public class EventShim : MarshalByRefObject
	{
		private InternalMessageDelegate target;

        private EventShim( InternalMessageDelegate target )
        {
            this.target += target;
        }

        public void InternalMessageShim(InternalMessage im)
        {
            target(im);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }


        public static InternalMessageDelegate Create( InternalMessageDelegate target )
        {
            EventShim shim = new EventShim(target);
            return new InternalMessageDelegate(shim.InternalMessageShim);
        }	
	}
}
