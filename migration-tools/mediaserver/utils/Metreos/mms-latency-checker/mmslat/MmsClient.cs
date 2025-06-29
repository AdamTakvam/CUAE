//
// MmsClient.cs
//
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
using Metreos.Core.IPC.Flatmaps;  



namespace mmslat
{
    public delegate void MmsResponseDelegate(int clientID, string msg);

	/// <summary>Represents a client of the media server</summary>
	public class MmsClient
	{
        private IpcFlatmapClient ipcClient;

        public MmsResponseDelegate raiseResponseReceived;

        public MmsClient(int clientid) 
        {
            this.ClientID = clientid;
            ipcClient = new IpcFlatmapClient(); 
            ipcClient.onFlatmapMessageReceived += new OnFlatmapMessageReceivedDelegate(OnMessageReceived);
        }


        /// <summary>Start socket</summary>
        public bool Start(string ipAddress, int remotePort)
        {
            this.mmsIP   = ipAddress;
            this.mmsPort = remotePort;
            IPAddress ipaddr = null;
            
            try { ipaddr = IPAddress.Parse(ipAddress); }
            catch
            {
                try 
                { 
                    IPHostEntry hostEntry = Dns.Resolve(ipAddress);
                    if (hostEntry != null && hostEntry.AddressList != null 
                     && hostEntry.AddressList.Length > 0)                     
                        ipaddr = hostEntry.AddressList[0];                     
                }
                catch { }
            }

            if (ipaddr == null) return false;

            ipcClient.RemoteEp = new IPEndPoint(ipaddr, remotePort);
            bool result = ipcClient.Open();

            if (!result) Console.WriteLine("Cannot connect to " + ipAddress + "/" + remotePort);
             
            return result;
        }


        /// <summary>Stop socket</summary>
        public void Stop()
        {
            ipcClient.Close();
        }


        /// <summary>Post a message ot media server</summary>
        public void PostMsg(string xml)
        {
            FlatmapList map = new FlatmapList();

            map.Add(Const.IpcMessageBodyID, xml);

            if (LatMain.msglvl == Const.MSGLVL_DEBUG)
            {
                Console.WriteLine("\noutbound xml:");
                Console.WriteLine(xml);
            }

            ipcClient.Write(Const.FlatmapMsgTypeId, map, 0);             
        }


        /// <summary>Media server message received event handler</summary>
        private void OnMessageReceived(IpcFlatmapClient ipcClient, int messageType, FlatmapList list)
        {
            if (messageType != Const.FlatmapMsgTypeId) return;

            string xml = list.Find(Const.IpcMessageBodyID, 1).dataValue as string;

            if (raiseResponseReceived != null) raiseResponseReceived(this.ClientID, xml);             
        }
       
        private string mmsIP = null;
        private int    mmsPort = 0;
        public  string MmsIP       { get { return mmsIP;   } }
        public  int    MmsPort     { get { return mmsPort; } }
       
        public int ClientID = 0;
	}
}
