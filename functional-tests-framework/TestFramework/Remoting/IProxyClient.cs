using System;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for IProxyClient.
	/// </summary>
	public interface IProxyClient
	{
        void SignalReceived(ActionMessage im);

        int ServerId { get; set; }
	}
}
