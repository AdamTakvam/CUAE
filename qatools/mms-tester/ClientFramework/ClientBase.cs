using System;

using Metreos.MmsTester.AdapterFramework;

using Metreos.MmsTester.Interfaces;
using Metreos.Samoa.Core;

namespace Metreos.MmsTester.ClientFramework
{
	/// <summary>
	/// Provides a framework with which to create clients
	/// </summary>
	public abstract class ClientBase
	{
        public string displayName;
        public AdapterBase adapter; 

		public ClientBase()
		{
			
		}

        public abstract bool AssociateAdapter(AdapterBase adapter);

        public abstract bool Send(InternalMessage im);

        public abstract InternalMessage RequestMediaServerInfo(InternalMessage im);
	}
}
