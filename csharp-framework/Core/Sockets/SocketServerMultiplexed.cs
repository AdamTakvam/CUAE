using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace Metreos.Core.Sockets
{
    public abstract class SocketServerMultiplexed : SocketServerBase
    {        
        protected ArrayList deadConnections;
        protected object deadConnectionsLock;

        public SocketServerMultiplexed(ushort listenPort) 
            : this("SocketServerMultiplexed", listenPort, TraceLevel.Info) {}


        public SocketServerMultiplexed(string taskName, ushort listenPort, TraceLevel logLevel) 
			: base(taskName, logLevel, listenPort)
        {
            this.deadConnections = new ArrayList();
            this.deadConnectionsLock = new Object();
        }


        protected override void Run()
        {
            this.listenSocket.Listen(10);

            //Socket newClient = null;

            while(this.shutdownRequested.WaitOne(50, false) == false)
            {
                // REFACTOR: Do Socket.Select stuff here

                // Iterate through active sockets to see if data is available.
                // REFACTOR: This should use Socket.Select(), but this is broken
                // in Framework v1.0.
                lock(this.activeConnectionsLock)
                {
                    if(this.activeConnections.Count > 0)
                    {
                        foreach(Socket client in this.activeConnections)
                        {
                            if(client != null)
                            {
                                if(client.Poll(50000, SelectMode.SelectRead) == true)
                                {
                                    this.ReadSocketData(client);
                                }
                            }
                        }
                    }
                }
               
                // Cleanup those sockets that are now closed.
                lock(this.deadConnectionsLock)
                {
                    if(this.deadConnections.Count > 0)
                    {
                        foreach(int i in this.deadConnections)
                        {
                            lock(this.activeConnectionsLock)
                            {
                                this.activeConnections[i] = null;
                            }
                        }

                        this.deadConnections.Clear();
                    }
                }
            }

            // Time to shutdown. Stop the listener.
//            listenSocket.Stop();

            // Cleanup all of the active connections.
            lock(this.activeConnectionsLock)
            {
                Socket client;

                // Iterate through each connection, closing it if it is 
                // valid. Note, this won't actually remote it from activeConnections
                // so we won't hit an iterator issue. This will close the Socket
                // down and set the reference to null.
                //
                // Also, for each connection that we close here, a callback is fired
                // to indicate the conneciton is closed.
                for(int i = 0; i < activeConnections.Count; i++)
                {
                    client = (Socket)activeConnections[i];

                    if(client != null)
                    {
                        this.CloseConnection(i);
                    }
                }

                this.activeConnections.Clear();
            }
            
            this.deadConnections.Clear();
            this.socketToPrimaryIndex.Clear();

            this.shutdownComplete.Set();
        }


        protected void ReadSocketData(Socket client)
        {
            byte[] buffer = new byte[BUFFER_SIZE];

            int bytesReceived = 0;
            
            if(client.Available > 0)
            {
                bytesReceived = client.Receive(buffer, 0, BUFFER_SIZE, SocketFlags.None);
            }

            if(bytesReceived > 0)
            {
                int activeIndex = (int) this.socketToPrimaryIndex[client];

                if(activeIndex != -1)
                {
                    IPEndPoint clientEndPoint = client.LocalEndPoint as IPEndPoint;
                    string receiveInterface;

                    receiveInterface = 
                        clientEndPoint != null ? clientEndPoint.Address.ToString() : "0.0.0.0";

                    DataReceived(activeIndex, receiveInterface, buffer, bytesReceived);
                }
            }
            else
            {
                // No data was received, the connection was closed from the remote side.
                int activeIndex = (int) this.socketToPrimaryIndex[client];

                this.deadConnections.Add(activeIndex);

                ConnectionClosed(activeIndex);
            }

            buffer = null;
        }


        protected override void StopSocket(Socket client)
        {
            int activeIndex = -1;

            // Verify that we have a mapping for this client socket
            // to an index inside our master active connections table.
            if(this.socketToPrimaryIndex.Contains(client))
            {
                activeIndex = (int) this.socketToPrimaryIndex[client];

                lock(this.socketToPrimaryIndexWriteLock)
                {
                    // Mark this entry as invalid.
                    this.socketToPrimaryIndex[client] = -1;
                }
            }

            if(activeIndex != -1)
            {
                lock(this.deadConnectionsLock)
                {
                    this.deadConnections.Add(activeIndex);
                }

                ConnectionClosed(activeIndex);
            }
        }
    }
}
