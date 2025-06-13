using System;
using System.Net;
using System.Threading;
using System.Diagnostics;

using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps;

namespace Metreos.Providers.SccpProxy.RtpRelay
{
    /// <summary>This class is a proxy to an RTP relay server</summary>
	public class Relay
	{
        private const int ResponseTimeout = 3000;  // 3 seconds

        private int connectionId;
        public int ConnectionId { get { return connectionId; } }

        private int relayObjectId;
        private LogWriter log;

        private string serverRelayId;
        public string ServerRelayId
        {
            get { return serverRelayId; }
            set { serverRelayId = value; }
        }

        private AutoResetEvent gotResponse;
        public bool GotResponse { set { gotResponse.Set(); } }

        private int lastResultCode = 0;
        public int LastResultCode { get { return lastResultCode; } }

        private IPEndPoint serverAddr1;
        public IPEndPoint ServerAddr1 
        { 
            get { return serverAddr1; } 
            set { serverAddr1 = value; }
        }

        private IPEndPoint serverAddr2;
        public IPEndPoint ServerAddr2 
        { 
            get { return serverAddr2; } 
            set { serverAddr2 = value; }
        }

        private IPEndPoint remoteAddr1;
        public IPEndPoint RemoteAddr1 { get { return remoteAddr1; } }

        private IPEndPoint remoteAddr2;
        public IPEndPoint RemoteAddr2 { get { return remoteAddr2; } }

		public Relay(int connectionId, int relayObjectId, LogWriter log, RelayManager relayManager)
		{
            this.connectionId = connectionId;
            this.relayObjectId = relayObjectId;
            this.log = log;
			this.relayManager = relayManager;

            this.gotResponse = new AutoResetEvent(false);
		}

		private RelayManager relayManager;

        /// <summary>Starts a connected relay</summary>
        /// <remarks>This method is called by the connection when the realy is created</remarks>
        /// <param name="remoteAddr1">Address for a remote side of the relay</param>
        /// <returns>success</returns>
        public bool Start(IPEndPoint remoteAddr, int addrIndice)
        {
            this.remoteAddr1 = remoteAddr1;
            
            // Post modify message to relay manager queue
            RelayMsg msg = new RelayMsg(this.relayObjectId, this.connectionId, MsgApi.MsgTypes.StartReq);
			msg.Fields.Add(MsgApi.Fields.aRelayIpSel, MsgApi.Interfaces.Internal);
			msg.Fields.Add(MsgApi.Fields.bRelayIpSel, MsgApi.Interfaces.External);
			
			if(addrIndice == 1)
			{
				msg.Fields.Add(MsgApi.Fields.aIp, remoteAddr.Address.ToString());
				msg.Fields.Add(MsgApi.Fields.aPort, remoteAddr.Port);
			}
			else
			{
				msg.Fields.Add(MsgApi.Fields.bIp, remoteAddr.Address.ToString());
				msg.Fields.Add(MsgApi.Fields.bPort, remoteAddr.Port);
			}

            relayManager.MsgQueueEnqueue(msg);

            // Wait for response
            if(gotResponse.WaitOne(ResponseTimeout, false) == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>Modifies a connected relay</summary>
        /// <param name="remoteAddr">New address</param>
        /// <param name="addrIndice">Indicates whether this address refers to connection 1 or 2</param>
        /// <returns>success</returns>
        public bool Modify(IPEndPoint remoteAddr, uint addrIndice)
        {
            RelayMsg msg = new RelayMsg(this.relayObjectId, this.connectionId, MsgApi.MsgTypes.ChangeReq);
            msg.Fields.Add(MsgApi.Fields.relayId, this.serverRelayId);

            if(addrIndice == 1)
            {
                this.remoteAddr1 = remoteAddr;
                msg.Fields.Add(MsgApi.Fields.aIp, remoteAddr1.Address.ToString());
                msg.Fields.Add(MsgApi.Fields.aPort, remoteAddr1.Port);
            }
            else
            {
                this.remoteAddr2 = remoteAddr;
                msg.Fields.Add(MsgApi.Fields.bIp, remoteAddr2.Address.ToString());
                msg.Fields.Add(MsgApi.Fields.bPort, remoteAddr2.Port);
            }

            // Post modify message to relay manager queue
            relayManager.MsgQueueEnqueue(msg);

            // Wait for response
            if(gotResponse.WaitOne(ResponseTimeout, false) == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>Tears down relay channel.</summary>
        public void Stop()
        {
            log.Write(TraceLevel.Info, "Stopping relay: {0}", this.relayObjectId);

            RelayMsg msg = new RelayMsg(this.relayObjectId, this.connectionId, MsgApi.MsgTypes.StopReq);
            msg.Fields.Add(MsgApi.Fields.relayId, this.serverRelayId);
            relayManager.MsgQueueEnqueue(msg);

            // Don't need to wait on the response
        }

        /// <summary>Processes response from RTP relay server</summary>
        /// <param name="flatmap">The IPC response message</param>
        public void ProcessResponse(FlatmapList flatmap)
        {
            if(flatmap.Contains((uint)MsgApi.Fields.relayId))
                this.serverRelayId = Convert.ToString(flatmap.Find((uint)MsgApi.Fields.relayId, 1).dataValue);

            this.lastResultCode = Convert.ToInt32(flatmap.Find((uint)MsgApi.Fields.resultCode, 1).dataValue);
            
			if(lastResultCode != 0)
			{
				log.Write(TraceLevel.Warning, "Terminating relay '{0}' due to error: {1}",
					this.ToString(), ((MsgApi.ResultCodes)lastResultCode).ToString());
				Stop();
			}
			else
			{
				if(flatmap.Contains((uint)MsgApi.Fields.aRelayIp) && flatmap.Contains((uint)MsgApi.Fields.aRelayPort))
				{
					string aRelayIP = null;
					int aRelayPort = 0;
					try 
					{
						aRelayIP = Convert.ToString(flatmap.Find((uint)MsgApi.Fields.aRelayIp, 1).dataValue);
						aRelayPort = Convert.ToInt32(flatmap.Find((uint)MsgApi.Fields.aRelayPort, 1).dataValue);
						this.serverAddr1 = new IPEndPoint(IPAddress.Parse(aRelayIP), aRelayPort); 
					}
					catch
					{
						log.Write(TraceLevel.Warning, "Received invalid relay address: {0}:{1}", aRelayIP, aRelayPort);
					}
				}

				if(flatmap.Contains((uint)MsgApi.Fields.bRelayIp) && flatmap.Contains((uint)MsgApi.Fields.bRelayPort))
				{
					string bRelayIP = null;
					int bRelayPort = 0;
					try 
					{
						bRelayIP = Convert.ToString(flatmap.Find((uint)MsgApi.Fields.bRelayIp, 1).dataValue);
						bRelayPort = Convert.ToInt32(flatmap.Find((uint)MsgApi.Fields.bRelayPort, 1).dataValue);
						this.serverAddr2 = new IPEndPoint(IPAddress.Parse(bRelayIP), bRelayPort); 
					}
					catch
					{
						log.Write(TraceLevel.Warning, "Received invalid relay address: {0}:{1}", bRelayIP, bRelayPort);
					}
				}
			}

            this.gotResponse.Set();
        }

		/// <summary>Return string representation of this object.</summary>
		public override string ToString()
		{
            string addr1 = this.remoteAddr1 != null ? remoteAddr1.ToString() : "[Not Set]";
            string addr2 = this.serverAddr1 != null ? serverAddr1.ToString() : "[Not Set]";
            string addr3 = this.serverAddr2 != null ? serverAddr2.ToString() : "[Not Set]";
            string addr4 = this.remoteAddr2 != null ? remoteAddr2.ToString() : "[Not Set]";
			
            return "id "+relayObjectId+": "+ addr1 + " <-> " + addr2 + " <-> " + addr3 + " <-> " + addr4;
		}
	}
}
