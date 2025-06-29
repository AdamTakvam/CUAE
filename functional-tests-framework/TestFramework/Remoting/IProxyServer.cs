using System;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>  ProxyServer for FTP </summary>
	public interface IProxyServer
	{
        void SendSignalToTestBase(ActionMessage im);		

        void SendShutdown();

        int RegisterTest(ProxyClient client);

        string ServerName { get; set ; } 

        void UnregisterTest(int id);

        bool RegisterSignals(int id, string[] signals);

        void SendTrigger(CommandMessage im);

        void SendEvent(CommandMessage im);

        void SendResponse(CommandMessage im);

        void UpdateConfigValue(CommandMessage im);

        void UpdateScriptParameter(CommandMessage im);

        void UpdateCallRouteGroup(CommandMessage im);

        void UpdateMediaRouteGroup(CommandMessage im);

        bool CreateComponentGroup(string testname);

        void CreatePartition(CommandMessage im);

        void CreatePartitionConfig(CommandMessage im);
	}
}
