using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Serialization;

using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Core;
using Metreos.Core.IPC.Xml;
using Metreos.DebugFramework;

namespace Metreos.LogSinks
{
	public class TcpLoggerSink : LoggerSinkBase
	{
        #region AuthInfo class

        private sealed class AuthInfo : IDisposable
        {
            private static TimeSpan AuthTimeout = new TimeSpan(0, 0, 5);

            public int SocketId { get { return socketId; } }
            private readonly int socketId;

            public string Remotehost { get { return remoteHost; } }
            private readonly string remoteHost;
            
            public Timer AuthTimer { get { return authTimer; } }
            private readonly Timer authTimer;

            public bool Authenticated { get { return authenticated; } }
            private bool authenticated;

            public bool AuthDenied { get { return authDenied; } }
            private bool authDenied;

            public AuthInfo(int socketId, string remoteHost, TimerCallback handleAuthTimeout)
            {
                this.socketId = socketId;
                this.remoteHost = remoteHost;
                this.authTimer = new Timer(handleAuthTimeout, socketId, 
                    Convert.ToInt32(AuthTimeout.TotalMilliseconds), Timeout.Infinite);
                this.authenticated = false;
                this.authDenied = false;
            }

            public void Authenticate()
            {
                this.authTimer.Dispose();
                this.authenticated = true;
                this.authDenied = false;
            }

            public void SetDenied()
            {
                this.authenticated = false;
                this.authDenied = true;
            }

            public void Dispose()
            {
                if(!authenticated)
                    this.authTimer.Dispose();
            }
        }
        #endregion

        private readonly IConfigUtility config;
        private readonly IpcXmlServer ipcServer;
        private readonly Hashtable socketTable;

        public TcpLoggerSink(ushort listenPort, IConfigUtility config, TraceLevel initLogLevel)
            : base(initLogLevel)
		{
            if(listenPort < 1024)
                throw new ArgumentException("Invalid listen port for TCP logger: " + listenPort, "listenPort");

            if(config == null)
                throw new ArgumentException("Cannot create TCP logger with null config", "config");

            this.config = config;
            this.socketTable = Hashtable.Synchronized(new Hashtable());

            this.ipcServer = new IpcXmlServer(typeof(TcpLoggerSink).Name, listenPort, false, TraceLevel.Error);
            this.ipcServer.OnNewConnection += new Metreos.Core.Sockets.NewConnectionDelegate(OnNewConnection);
            this.ipcServer.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(OnCloseConnection);
            this.ipcServer.OnMessageReceived += new IpcXmlServer.OnMessageReceivedDelegate(OnMessageReceived);
            this.ipcServer.Start();
		}

        public override void Dispose()
        {
            ipcServer.Stop();
        }

		public override void LoggerWriteCallback(DateTime timeStamp, TraceLevel errorLevel, string message)
		{
            if(message == null) { return; }

            string formattedMsg = String.Format("{0} {1} {2}",
				timeStamp.ToString(ILog.ShortTimestampFormat), errorLevel.ToString()[0], message);
            
            lock(socketTable)
            {
                foreach(AuthInfo aInfo in socketTable.Values)
                {
                    if(aInfo.Authenticated)
                    {
                        try
                        {
                            ipcServer.Write(aInfo.SocketId, formattedMsg);
                        }
                        catch(Exception e)
                        {
                            Trace.Write("Remote Console: Failed to send log message to client. Error: " + e.Message, 
                                TraceLevel.Info.ToString());
                        }
                    }
                }
            }
        }

        #region IPC Server callbacks

        private void OnNewConnection(int socketId, string remoteHost)
        {
            socketTable[socketId] = new AuthInfo(socketId, remoteHost, new TimerCallback(HandleAuthTimeout));
        }

        private void OnCloseConnection(int socketId)
        {
            lock(socketTable)
            {
                AuthInfo aInfo = socketTable[socketId] as AuthInfo;
                if(aInfo != null)
                    aInfo.Dispose();
                
                socketTable.Remove(socketId);
            }
        }

        private void OnMessageReceived(int socketId, string localInterface, string message)
        {
            // Get the metadata
            AuthInfo aInfo = socketTable[socketId] as AuthInfo;
            if(aInfo == null)
                return;

            // The only packet we should ever get from the client is an authentication packet of the form:
            //  <username>|<password>
            char[] delimiter = new char[] { RemoteConsoleClient.Consts.AuthDelimiter };
            string[] bits = message.Split(delimiter, 2);
            if(bits == null || bits.Length != 2)
            {
                Trace.Write("Remote Console: Received invalid authentication packet from: " + aInfo.Remotehost, 
                    TraceLevel.Warning.ToString());
                return;
            }

            IConfig.AccessLevel access = config.ValidateUser(bits[0], bits[1], null);

            if(access == IConfig.AccessLevel.Unspecified || access == IConfig.AccessLevel.Restricted)
            {
                aInfo.SetDenied();
                ipcServer.Write(socketId, RemoteConsoleClient.Consts.AuthDenied);

                Trace.Write("Remote Console: Unauthorized connection attempt from: " + aInfo.Remotehost, 
                    TraceLevel.Warning.ToString());
                return;
            }

            // Mark socket authenticated
            aInfo.Authenticate();

            // Send success response
            ipcServer.Write(socketId, RemoteConsoleClient.Consts.AuthSuccess);

            // Log it
            Trace.Write("Remote Console: Connection accepted from: " + aInfo.Remotehost, 
                TraceLevel.Info.ToString());
        }
        #endregion

        #region Private helpers

        private void HandleAuthTimeout(object state)
        {
            int socketId = Convert.ToInt32(state);
            AuthInfo aInfo = socketTable[socketId] as AuthInfo;
            if(aInfo != null && !aInfo.AuthDenied)
            {
                ipcServer.Write(socketId, RemoteConsoleClient.Consts.AuthTimeout);
                Trace.Write("Remote Console: Authentication timed out from: " + aInfo.Remotehost, 
                    TraceLevel.Warning.ToString());
            }

            ipcServer.CloseConnection(socketId);
        }

        private bool IsAuthenticated(int socketId)
        {
            AuthInfo aInfo = socketTable[socketId] as AuthInfo;
            if(aInfo != null)
                return aInfo.Authenticated;
            return false;
        }
        #endregion
    }
}
