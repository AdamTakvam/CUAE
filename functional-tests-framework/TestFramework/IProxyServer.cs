using System;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for IProxyServer.
	/// </summary>
	public interface IProxyServer
	{
        void SendSignalToTestBase(InternalMessage im);		

        void SendShutdown();

        int RegisterTest(ProxyClient client);

        void UnregisterTest(int id);

        bool RegisterSignals(int id, string[] signals);

        void SendTrigger(InternalMessage im);

        void SendEvent(InternalMessage im);

        void SendResponse(InternalMessage im);
	}
}
