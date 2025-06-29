using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Metreos.Interfaces;

namespace Metreos.Core.Sockets
{
    public delegate void NewConnectionDelegate(int socketId, string remoteHost);
    public delegate void CloseConnectionDelegate(int socketId);
    public delegate void ClientCloseConnectionDelegate();
    public delegate void DataReceivedDelegate(int socketId, string receiveIpAddress, byte[] data, int dataLength);

    public abstract class SocketServerBase : TaskBase
	{
        public const int SHUTDOWN_WAIT_PERIOD   = 60000;
        public const int BUFFER_SIZE            = 1024;

        protected ArrayList activeConnections;
        protected object activeConnectionsLock;

        protected Hashtable socketToPrimaryIndex;
        protected object socketToPrimaryIndexWriteLock;

        protected ManualResetEvent shutdownRequested;
        protected ManualResetEvent shutdownComplete;

        protected Socket listenSocket;
        protected ushort listenPort;
        protected bool loopback;

        /// <summary>
        /// Get/Set the port that this socket server
        /// listens for connections on.
        /// </summary>
        /// <remarks>
        /// If you attempt to set the port of the socket
        /// server after the socket server has been
        /// started then no change will take place.
        /// </remarks>
        public ushort ListenPort
        {
            get { return listenPort; }
            set 
            {
                if(this.IsThreadAlive == false)
                {
                    this.listenPort = value;
                }
            }
        }

        new public TraceLevel LogLevel
        {
            get { return log.LogLevel; }
            set { log.LogLevel = value; }
        }

        public IPEndPoint ListenInterface
        {
            get
            {
                return this.listenSocket.LocalEndPoint as IPEndPoint;
            }
        }


        #region Methods to be overriden by implementing child classes.

        /// <summary>
        /// This method will be invoked when individual sockets
        /// need to be stopped allowing for implementors to do
        /// any necessary cleanup.
        /// </summary>
        /// <param name="socketToBeStopped">The socket to be stopped.</param>
        protected abstract void StopSocket(Socket socketToBeStopped);

        protected abstract void NewConnection(int socketId, string remoteHost);
        protected abstract void ConnectionClosed(int socketId);
        protected abstract void DataReceived(int socketId, string receiveIpAddress, byte[] data, int dataLength);

        #endregion

		protected SocketServerBase(string taskName, TraceLevel logLevel, ushort newListenPort, bool loopback) 
			: base(IConfig.ComponentType.Provider, taskName, taskName, logLevel)
        {
            this.activeConnections = new ArrayList();
            this.activeConnections = ArrayList.Synchronized(this.activeConnections);
            this.activeConnectionsLock = new Object();

            this.socketToPrimaryIndex = new Hashtable();
            this.socketToPrimaryIndex = Hashtable.Synchronized(this.socketToPrimaryIndex);
            this.socketToPrimaryIndexWriteLock = new Object();

            this.shutdownRequested = new ManualResetEvent(false);
            this.shutdownComplete = new ManualResetEvent(false);

            this.listenPort = newListenPort;
            this.loopback  = loopback;
        }


        /// <summary>
        /// Starts the socket server.
        /// </summary>
        /// <remarks>
        /// An attempt will be made to bind to the listen port
        /// for the socket server. If the bind fails, the socket
        /// server will not start.
        /// </remarks>
        /// <returns>True if successfull, false if an error occured.</returns>
        public override bool Start()
        {
            if(this.BindListenSocket() == true)
            {
                return base.Start();
            }

            return false;
        }


        /// <summary>
        /// Stops the socket server.
        /// </summary>
        public virtual void Stop()
        {
            this.shutdownRequested.Set();

            // This wait period is a bit kludgy. For a large number of connections
            // it may take a lengthy period of time to close them all.
            if(this.shutdownComplete.WaitOne(SHUTDOWN_WAIT_PERIOD, false) == false)
            {
                log.Write(TraceLevel.Warning, "SocketServerThreaded shutdown timed out.");
            }
        }

        protected bool SendDataToAllSockets(byte[] byteData)
        {
            bool success = true;

            foreach(Socket client in this.activeConnections)
            {
                success &= SendData(client, byteData);
            }

            return success;
        }
        
        protected bool SendData(int socketId, byte[] byteData, bool closeSocket)
        {
            Socket client = (Socket)this.activeConnections[socketId];
            bool success = SendData(client, byteData);

            if(closeSocket)
            {
                CloseConnection(socketId);
            }

            return success;
        }

        private bool SendData(Socket client, byte[] byteData)
        {
            if(client == null) { return false; }

            if(byteData.Length > 0)
            {
                try
                {
                    client.Send(byteData, 0, byteData.Length, SocketFlags.None);
                }
                catch(SocketException)
                {
                    // Do nothing, socket should be cleaned up and closed normally later.
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Create a new connection.
        /// </summary>
        /// <param name="remoteHost"></param>
        /// <param name="remotePort"></param>
        /// <returns></returns>
        public virtual int CreateConnection(string remoteHost, int remotePort)
        {
            Socket newClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(remoteHost);
            System.Net.IPAddress hostAddr = host.AddressList[0];
            System.Net.IPEndPoint hostEp = new System.Net.IPEndPoint(hostAddr, remotePort);

            newClient.Connect(hostEp);

            if(newClient.Connected == false)
                return -1;
            
            return this.AddActiveConnection(newClient);
        }


        /// <summary>
        /// Close a connection to this socket server.
        /// </summary>
        /// <param name="socketId">The ID inside the active connections table of the socket to close.</param>
        public void CloseConnection(int socketId)
        {
            if((socketId >= 0) && (socketId < this.activeConnections.Count))
            {
                Socket client = this.activeConnections[socketId] as Socket;

                if(client != null)
                {
                    this.activeConnections[socketId] = null;
                    this.CloseActiveConnection(client);
                }
            }            
        }


        /// <summary>
        /// Closes an active connection to our socket server.
        /// </summary>
        /// <remarks>
        /// This method will first close the socket by calling
        /// Shutdown() and Close() on it, in that order. Then it
        /// will invoke the StopSocket() method that must be 
        /// implemented by derived classes. This should do any
        /// specific cleanup that is necessary for that derived
        /// class (i.e., stopping read threads, etc).
        /// </remarks>
        /// <param name="client">The socket connection to close.</param>
        protected virtual void CloseActiveConnection(Socket client)
        {
            if(client == null)
            {
                log.Write(TraceLevel.Warning, "Attempt to close a null socket.");
                return;
            }

            this.StopSocket(client);

            if(client.Connected == true)
            {
                // Close the socket out.
                this.ShutdownSocket(client);
            }
        }


        /// <summary>
        /// Shuts a socket down and closes it out.
        /// </summary>
        /// <param name="client">The socket to shutdown and close.</param>
        protected virtual void ShutdownSocket(Socket client)
        {
            if(client != null)
            {
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch(SocketException e)
                {
                    log.Write(TraceLevel.Warning, "Socket exception caught: " + e.ToString());
                }
                catch(ObjectDisposedException)
                {
                    log.Write(TraceLevel.Info, "Attempt to shutdown a socket failed. Reason: Socket is already closed.");
                }
            }
        }


        /// <summary>
        /// Bind the socket server to a listening socket on the
        /// port we want to listen on.
        /// </summary>
        /// <returns>True if bind successful, false otherwise.</returns>
        protected virtual bool BindListenSocket()
        {
            if(this.IsThreadAlive == false)
            {
                try
                {
                    // Bind to our specific listen port and all IP addresses.
                    IPAddress _interface = this.loopback ? IPAddress.Loopback : IPAddress.Any;
                    IPEndPoint ipe = new IPEndPoint(_interface, this.listenPort);

                    this.listenSocket = new Socket(
                        AddressFamily.InterNetwork, 
                        SocketType.Stream, 
                        ProtocolType.Tcp);

                    this.listenSocket.Bind(ipe);
                }
                catch(SocketException)
                {
                    log.Write(TraceLevel.Error, "Unable to bind to listen socket on port " + this.listenPort + ".");

                    return false;
                }

                return true;
            }
            
            return false;            
        }


        /// <summary>
        /// Adds a new active socket connection to our master table.
        /// </summary>
        /// <param name="client">The socket object associated with the connection.</param>
        /// <returns>The index of the new connection in our table.</returns>
        protected virtual int AddActiveConnection(Socket client)
        {
            int activeIndex = 0;

            if(this.socketToPrimaryIndex.Contains(client))
            {
                // This really shouldn't ever happen... yet it does... :(
                return -1;
            }

            // REFACTOR: Look for an empty slot rather than always adding a new one.

            lock(this.activeConnectionsLock)
            {
                activeIndex = this.activeConnections.Add(client);
            }

            lock(this.socketToPrimaryIndexWriteLock)
            {
                this.socketToPrimaryIndex.Add(client, activeIndex);
            }

            try 
            { 
                NewConnection(activeIndex, client.RemoteEndPoint.ToString()); 
            }
            catch 
            {
                this.socketToPrimaryIndex.Remove(client);
                this.activeConnections.RemoveAt(activeIndex);
                activeIndex = 0;
            }

            return activeIndex;
        }
	}
}
