using System;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for IProxyClient.
	/// </summary>
	public interface IProxyClient
	{
        void SignalReceived(InternalMessage im);
	}
}
