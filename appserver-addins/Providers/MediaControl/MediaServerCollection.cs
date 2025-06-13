using System;
using System.Net;
using System.Collections;

namespace Metreos.MediaControl
{
	/// <summary>Optimized collection of media server information</summary>
	public class MediaServerCollection : IEnumerable
	{
        /// <summary>
        /// Server ID (uint) -> Server info (MediaServerInfo)
        /// </summary>
        protected Hashtable mediaServers;

        /// <summary>
        /// Server address (IPAddress) -> Server info (MediaServerInfo)
        /// </summary>
        protected Hashtable mediaServerAddrs;

        public object SyncRoot { get { return mediaServers.SyncRoot; } }

        public int Count { get { return  mediaServers.Count; } }

        public MediaServerInfo this[uint serverId]
        {
            get { return mediaServers[serverId] as MediaServerInfo; }
        }

		public MediaServerCollection()
		{
            mediaServers = Hashtable.Synchronized(new Hashtable());
            this.mediaServerAddrs = Hashtable.Synchronized(new Hashtable());
		}

        public bool Add(MediaServerInfo msInfo)
        {
            if(msInfo.ServerId == 0 || msInfo.Address == null)
                return false;

            lock(mediaServers.SyncRoot)
            {
                if (mediaServers.Contains(msInfo.ServerId) ||
                    mediaServerAddrs.Contains(msInfo.Address))
                    return false;

                mediaServers[msInfo.ServerId] = msInfo;
                this.mediaServerAddrs[msInfo.Address] = msInfo;
            }
            return true;
        }

        public MediaServerInfo GetByAddr(IPAddress addr)
        {
            if(addr == null)
                return null;

            return this.mediaServerAddrs[addr] as MediaServerInfo;
        }

        public void Remove(uint serverId)
        {
            lock(mediaServers.SyncRoot)
            {
                MediaServerInfo msInfo = this[serverId];
                if(msInfo != null)
                {
                    mediaServers.Remove(serverId);
                    mediaServerAddrs.Remove(msInfo.Address);
                }
            }
        }

        public void Clear()
        {
            lock(mediaServers.SyncRoot)
            {
                this.mediaServerAddrs.Clear();
                mediaServers.Clear();
            }
        }

        public override string ToString()
        {
            const string heading1 = " ID        IP        Type    Status    IP  LBR    Name\r\n";
            const string heading2 = "---- --------------- ---- ------------ --- --- ----------\r\n";

            System.Text.StringBuilder sb = new System.Text.StringBuilder(heading1);
            sb.Append(heading2);

            lock(mediaServers.SyncRoot)
            {
                foreach(DictionaryEntry de in mediaServers)
                {
                    string serverId = Convert.ToString(de.Key);
                    MediaServerInfo msInfo = de.Value as MediaServerInfo;
                    
                    if (serverId == null || serverId == String.Empty ||
                        msInfo == null)
                        continue;

                    sb.Append(serverId.PadRight(5));
                    sb.Append(msInfo.Address.ToString().PadRight(16));
					sb.Append(msInfo is MediaServerInfoIPC ? "IPC  " : "MSMQ ");
                    sb.Append(msInfo.ConnectedToMediaServer ? "Connected    " : "Disconnected ");
					sb.Append(msInfo.Resources.ipResAvail.ToString().PadRight(4));
                    sb.Append(msInfo.Resources.lbrResAvail.ToString().PadRight(4));
                    sb.Append(msInfo.MediaServerName);
                    sb.Append("\r\n");
                }
            }

            return sb.ToString();
        }

        public string GetDiagnosticMessage()
        {
            const string heading1 = "sId    tId=       ttId    tSid  tType   msMsg  Created    Due   \r\n";
            const string heading2 = "--- ---------- ---------- ---- ------- ------- -------- --------\r\n";

            System.Text.StringBuilder sb = new System.Text.StringBuilder(heading1);
            sb.Append(heading2);

            lock(mediaServers.SyncRoot)
            {
                foreach(DictionaryEntry de in mediaServers)
                {
                    string serverId = Convert.ToString(de.Key);
                    MediaServerInfo msInfo = de.Value as MediaServerInfo;
                    
                    if (serverId == null || serverId == String.Empty ||
                        msInfo == null)
                        continue;

                    sb.Append(msInfo.GetDiagnosticMessage());
                }
            }

            return sb.ToString();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return mediaServers.GetEnumerator();
        }

        #endregion
    }
}
