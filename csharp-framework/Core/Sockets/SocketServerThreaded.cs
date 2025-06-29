using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace Metreos.Core.Sockets
{
    public abstract class SocketServerThreaded : SocketServerBase
    {
        /// <summary>
        /// Time to wait for the read socket to shutdown.
        /// </summary>
        public const int READ_THREAD_SHUTDOWN_TIMEOUT = 250;

        /// <summary>
        /// A simple threaded object to read data from a given
        /// socket until the socket is closed.
        /// </summary>
        protected class ReadSocketThread
        {
            public CloseConnectionDelegate onCloseConnection;
            public DataReceivedDelegate onDataReceived;

            protected Thread thread;
            protected Socket client;
            protected SocketServerThreaded server;
            protected int activeIndex;

            public ReadSocketThread(SocketServerThreaded server, int activeIndex, Socket client)
            {
                this.server = server;
                this.client = client;
                this.activeIndex = activeIndex;
            }

            /// <summary>
            /// Kick off this read thread for this socket.
            /// </summary>
            public void Start()
            {
                thread = new Thread(new ThreadStart(this.ReadSocketData));
                thread.IsBackground = true;
                thread.Name = "Network Socket";
                thread.Start();
            }


            /// <summary>
            /// Stop the read thread if it is currently executing.
            /// </summary>
            public void Stop()
            {
                if(client.Connected == true)
                {
                    try
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                    catch(ObjectDisposedException)
                    {
                        // Already closed.
                    }
                }

                // Verify the thread is done.
                if(thread.Join(READ_THREAD_SHUTDOWN_TIMEOUT) == false)
                {
                    thread.Abort();
                }
            }


            /// <summary>
            /// Will loop, reading data from the socket.
            /// </summary>
            /// <remarks>
            /// This method will exit when the socket is closed.
            /// When the socket is closed it will cause the
            /// Receive() to exit with 0 bytes read.
            /// </remarks>
            protected void ReadSocketData()
            {
                byte[] buffer;
                bool done = false;

                IPEndPoint clientEndPoint = null;
                try { clientEndPoint = client.LocalEndPoint as IPEndPoint; }
                catch { return; }

                string receiveInterface = 
                    clientEndPoint != null ? clientEndPoint.Address.ToString() : "127.0.0.1";

                while(done == false)
                {
                    buffer = new byte[BUFFER_SIZE];

                    int bytesReceived = 0;
            
                    Debug.Assert(client != null, "Client is null");

                    try
                    {
                        // Pull data from the socket.
                        // If bytesReceived == 0 after this returns, the socket
                        // is closed and we should exit this method.
                        bytesReceived = client.Receive(buffer);
                    }
                    catch(SocketException)
                    {}
                    catch(ObjectDisposedException)
                    {
                        // Socket closed. Caused by another thread invoking
                        // Shutdown() and Close() on this socket.
                        bytesReceived = 0;
                    }
                    catch(NullReferenceException)
                    {
                        bytesReceived = 0;
                    }

                    if(bytesReceived > 0)
                    {
                        if(onDataReceived != null)
                        {
                            onDataReceived(this.activeIndex, receiveInterface, buffer, bytesReceived);
                        }
                    }
                    else
                    {
                        // The connect has been closed. Time to exit.
                        if(onCloseConnection != null)
                        {
                            // First, invoke the close connection delegate if possible.
                            onCloseConnection(this.activeIndex);
                        }

                        try { client.Close(); }
                        catch {}

                        server.activeConnections[activeIndex] = null;

                        // Mark ourselves as complete.
                        done = true;
                    }
                }
            }
        }

        protected Thread acceptThread;
        protected Hashtable socketToReadThread;

        public SocketServerThreaded(ushort listenPort, bool loopback) 
            : this("SocketServerThreaded", listenPort, loopback, TraceLevel.Info) {}

        public SocketServerThreaded(string taskName, ushort listenPort, bool loopback, TraceLevel logLevel) 
			: base(taskName, logLevel, listenPort, loopback)
        {
            this.socketToReadThread = new Hashtable();
        }

        protected override void Run()
        {
            this.acceptThread = new Thread(new ThreadStart(this.AcceptNewConnections));
            this.acceptThread.IsBackground = true;
            this.acceptThread.Name = "Socket Accept Thread";
            this.acceptThread.Start();

            while(this.shutdownRequested.WaitOne(30000, false) == false)
            {}

            // A shutdown has been requested, clean up resources because
            // this socket server is done processing requests.
            if(listenSocket.Connected)
                listenSocket.Shutdown(SocketShutdown.Both);

            listenSocket.Close();
            
            if(listenSocket.Connected) 
                log.Write(TraceLevel.Error, "Winsock error: " + Convert.ToString(System.Runtime.InteropServices.Marshal.GetLastWin32Error()) );

            System.Threading.Thread.Sleep(READ_THREAD_SHUTDOWN_TIMEOUT);

            if(this.acceptThread.IsAlive == true)
            {
                acceptThread.Abort();
            }

            // Enumerate each of our read threads and shut them down.
			foreach(ReadSocketThread readThread in new ArrayList(this.socketToReadThread.Values))
			{
				if(readThread != null)
					readThread.Stop();
			}

			this.socketToReadThread.Clear();

            this.shutdownComplete.Set();
        }


        protected void AcceptNewConnections()
        {
            this.listenSocket.Listen(10);

            Socket newClient = null;

            bool done = false;

            while(done == false)
            {
                try
                {
                    newClient = this.listenSocket.Accept();
					newClient.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1);
                }
                catch
                {
                    done = true;
                }

                if(newClient != null)
                {
                    this.AddActiveConnection(newClient);
                }
            }
        }


        protected void CreateNewReadThread(int activeIndex, Socket client)
        {
            Debug.Assert(client != null, "client is null");

            ReadSocketThread readThread = new ReadSocketThread(this, activeIndex, client);
            readThread.onCloseConnection = new CloseConnectionDelegate(ConnectionClosed);
            readThread.onDataReceived = new DataReceivedDelegate(DataReceived);
            
            this.socketToReadThread.Add(client, readThread);
            
            readThread.Start();            
        }   

        
        protected override void StopSocket(Socket client)
        {
            Debug.Assert(client != null, "client is null");

            ReadSocketThread readThread = this.socketToReadThread[client] as ReadSocketThread;
            this.socketToReadThread.Remove(client);

			// JDL, 04/13/05, Add check to thread object.
			if (readThread != null)
				readThread.Stop();
        }


        protected override int AddActiveConnection(Socket client)
        {
            Debug.Assert(client != null, "client is null");

            int activeIndex = base.AddActiveConnection(client);
            
            if(activeIndex != -1)
                this.CreateNewReadThread(activeIndex, client);

            return activeIndex;
        }
    }
}