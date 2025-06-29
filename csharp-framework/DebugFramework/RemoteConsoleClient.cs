using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;

using Metreos.Utilities;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Xml;

namespace Metreos.DebugFramework
{
    public delegate void VoidDelegate();
	public delegate void ConsoleMessageDelegate(string message);

	/// <summary>
	/// Core library for making a remote console client
	/// </summary>
	public class RemoteConsoleClient
	{
        #region Consts

        public abstract class Consts
        {
            public const char AuthDelimiter     = '|';
            public const string AnonAuthString  = "anonymous";

            public const string AuthDenied      = "$$AccessDenied";
            public const string AuthSuccess     = "$$AccessGranted";
            public const string AuthTimeout     = "Authentication timeout";
        }
        #endregion

        public IPEndPoint RemoteEP { get { return client.RemoteEp; } }

		public event ConsoleMessageDelegate messageWriter;
        public event VoidDelegate onClose;
        public event VoidDelegate onAuthSuccess;
        public event VoidDelegate onAuthDenied;

        private readonly IpcXmlClient client;
        
        private Thread reconThread;
        private string username;
        private string password;

        public bool Reconnecting { get { return this.reconThread != null && this.reconThread.IsAlive; } }

		public RemoteConsoleClient() 
        {
            this.client = new IpcXmlClient(); 
            this.client.onClose += new OnCloseDelegate(OnClose);
            this.client.onXmlMessageReceived += new OnXmlMessageReceivedDelegate(OnMessageReceived);
        }

		public bool Start(string ipAddress, int remotePort, string username, string password)
		{
            this.username = username;
            this.password = password;

            IPAddress addr = IpUtility.ResolveHostname(ipAddress);
            if(addr == null)
                return false;

            client.RemoteEp = new IPEndPoint(addr, remotePort);

            if(!client.Open())
                return false;

            // Send authentication packet
            client.Write(CreateAuthString(username, password));
            return true;
		}

        public bool BeginReconnect()
        {
            if(!Reconnecting)
            {
                reconThread = new Thread(new ThreadStart(ReconnectThread));
                reconThread.IsBackground = true;
                reconThread.Start();
                return true;
            }
            return false;
        }

        private void ReconnectThread()
        {
            try { client.Close(); }
            catch {}

            while(!client.Open()) {}

            // Authenticate
            client.Write(CreateAuthString(username, password));
        }

        public void Send(string message)
        {
            client.Write(message);
        }

		public void Close()
		{
            if(Reconnecting)
                reconThread.Abort();

			client.Close();
		}

        #region Private Helpers

        private string CreateAuthString(string username, string password)
        {
            if(username == null || username == String.Empty)
                username = Consts.AnonAuthString;

            if(password == null || password == String.Empty)
                password = Security.EncryptPassword(Consts.AnonAuthString);
            
            return username + Consts.AuthDelimiter + password;
        }

		private void Write(string message)
		{
			if(messageWriter != null)
				messageWriter(message);
		}

        private void OnMessageReceived(IpcXmlClient ipcClient, string message)
        {
            if(message == Consts.AuthDenied)
            {
                if(onAuthDenied != null)
                    onAuthDenied();
            }
            else if(message == Consts.AuthSuccess)
            {
                if(onAuthSuccess != null)
                    onAuthSuccess();
            }
            else
            {
                Write(message);
            }
        }

        private void OnClose(IpcClient ipcClient, Exception e)
        {
            if(onClose != null)
                onClose();
        }
        #endregion
    }
}
