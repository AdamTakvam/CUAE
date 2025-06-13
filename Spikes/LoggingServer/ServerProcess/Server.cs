using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Messaging.Ipc;

using LoggingCore;

namespace ServerProcess
{
    /// <summary>Basic logging server.</summary>
    class Server
    {
        /// <summary>Hash of socket ID to LogClientSession object.</summary>
        /// <remarks>This handles only the basic semantics of maintaing a hash
        /// of sockets to client sessions.  As data is received client sessions
        /// are passed that data for processing.</remarks>
        private static Hashtable clients = new Hashtable();

        private static SocketServerBase server;

        [STAThread]
        static void Main(string[] args)
        {
            server = new SocketServerThreaded("LoggingServer", 6060, TraceLevel.Info);

            server.onNewConnection    += new NewConnectionDelegate(OnNewConnectionCallback);
            server.onDataReceived     += new DataReceivedDelegate(OnDataReceivedCallback);
            server.onCloseConnection  += new CloseConnectionDelegate(OnCloseConnectionCallback);

            server.Start();
            
            Console.WriteLine("Press enter to quit");
            Console.ReadLine();

            Console.WriteLine("Stopping server");
            server.Stop();
        }

        /// <summary>Fired from SocketServerBase indicating that we have a new connection.</summary>
        /// <param name="socketId">The socket ID of the connection.</param>
        /// <param name="remoteHost">The far end host.</param>
        static void OnNewConnectionCallback(int socketId, string remoteHost)
        {
            Console.WriteLine("Connection established: {0}", socketId);

            LogClientSession client = new LogClientSession();
            clients.Add(socketId, client);
        }

        /// <summary>Fired from SocketServerBase when new data is available on a socket.</summary>
        /// <param name="socketId">The socket ID of the connection with data.</param>
        /// <param name="receiveIpAddress">Where the data was received from.</param>
        /// <param name="data">The data received.</param>
        /// <param name="dataLength">The length of the data received.</param>
        static void OnDataReceivedCallback(int socketId, string receiveIpAddress, byte[] data, int dataLength)
        {
            Console.WriteLine("Data received: {0} {1}/{2}", socketId, data.Length, dataLength);

            if(clients.Contains(socketId))
            {
                LogClientSession client = (LogClientSession)clients[socketId];
                client.AppendMessageData(data, dataLength);
            }
            else
            {
                Console.WriteLine("Warning: Data received from a socket that is not listed in active clients table. Data ignored.");
            }
        }

        /// <summary>Fired from SocketServerBase when a connection has been closed.</summary>
        /// <param name="socketId">The socket ID of the connection that was closed.</param>
        static void OnCloseConnectionCallback(int socketId)
        {
            Console.WriteLine("Connection closed: {0}", socketId);

            clients.Remove(socketId);
        }
    }
}
