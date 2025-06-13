using System;
using System.Net;

using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps;

namespace Metreos.Providers.SccpProxy.RtpRelay
{
	/// <summary>Special relay connection which exposes the underlying IPC channel</summary>
	public class BackupRelayConnection : RelayConnection
	{
        public IpcFlatmapClient IpcClient { get { return base.ipcClient; } }

        public string ServerAddress { get { return base.relayIpcEP.ToString(); } }

        public BackupRelayConnection(int connectionId, LogWriter log, IPEndPoint ipcAddr, RelayManager relayManager)
            : base(connectionId, log, ipcAddr, relayManager) {}
	}
}
