using System;

using Metreos.MmsTester.AdapterFramework;
using Metreos.Samoa.Core;
using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.ClientFramework;

namespace Metreos.MmsTester.Custom.Clients
{
	/// <summary>
	/// Summary description for ConfigurationClient.
	/// </summary>
	[Client("Configurator")]
	public class ConfigurationClient : ClientBase
	{
		public ConfigurationClient()
		{
			displayName = "Configurator";
		}

        public override bool AssociateAdapter(AdapterBase adapter)
        {
            this.adapter = adapter;
            return true;
        }

        public override bool Send(InternalMessage im)
        {
            return true;
        }

        public override InternalMessage RequestMediaServerInfo(InternalMessage im)
        {
            return new InternalMessage();
        }
	}
}
