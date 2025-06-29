using System;
using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.MmsTester.Interfaces;

namespace Metreos.MmsTester.AdapterFramework
{
	/// <summary>
	/// The base class for creating an adapter for the Metreos Media Server.
	/// The adapter will communicate with the Metreos Media Server.
	/// </summary>
	public abstract class AdapterBase : TaskBase, IAdapter
	{
        public string displayName;

		public AdapterBase(string taskName, string logLevel) : base(taskName, logLevel)
		{
            
		}

        public abstract bool Initialize(string mediaServerHandle);

        public override void Cleanup()
        {
            base.Cleanup();
        }

        public override bool Start()
        {
            base.Start();
            return true;
        }

        public virtual bool Send(InternalMessage im, IAdapterTypes.ResponseFromMediaServerDelegate incomingResponseCallback, string messageType)
        {
            return true;
        }

        public virtual void SignalShutdown()
        {

        }

        public virtual bool Restart(string mediaServerHandle)
        {
            return true;
        }
	}
}
