using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;

namespace Metreos.Framework.UnitTest.Ipc
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Tester
	{
        public static ushort port = 9001;
        public const int buffertime = 50;

		[STAThread]
		static void Main(string[] args)
		{
            Tester tester = new Tester();

            bool success = true;

            if(args.Length == 2 && args[0] == "client")
            {
                port = Convert.ToUInt16(args[1]);
                success = tester.TestClient();
            }
            else
            {
                success = tester.TestClientServer();
            }
//
//            if(success)
//            {
//                Console.WriteLine("All test(s) completed successfully");
//            }
//            else
//            {
//                Console.WriteLine("Test(s) failed");
//            }

            Console.WriteLine();
            Console.WriteLine("Press any key to end");
            Console.Read();
        }

        private IpcFlatmapClient client;
        private IpcFlatmapServer server;

        public Tester() {}

        public bool TestClientServer()
        {
            int testcount = 0;
            bool success = true;

            client = new IpcFlatmapClient();
            server = new IpcFlatmapServer("IpcServerTest", port, TraceLevel.Verbose);
            client.onReconnect += new OnReconnectDelegate(client_onReconnect);
            client.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.IpcFlatmapClient.OnMessageReceivedDelegate(client_OnMessageReceived);
            client.onConnectionClosed += new Metreos.Core.Sockets.ClientCloseConnectionDelegate(client_onConnectionClosed);
            server.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(server_OnCloseConnection);
            server.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.IpcFlatmapServer.OnMessageReceivedDelegate(server_OnMessageReceived);
            server.OnNewConnection += new Metreos.Core.Sockets.NewConnectionDelegate(server_OnNewConnection);

            server.Start();
            client.Open(IPAddress.Loopback.ToString(), port, 5);
 
            success &= Test1();
            testcount++;

            if(!success) 
            {
                Console.WriteLine(testcount + " failed");
            }

            success &= Test2();
            testcount++;

            if(!success) 
            {
                Console.WriteLine(testcount + " failed");
            }

            server.Cleanup();
            client.Close();
            client.Dispose();

            return success;
        }

        public bool TestClient()
        {
            int testcount = 0;
            bool success = true;

            client = new IpcFlatmapClient();
            client.onReconnect += new OnReconnectDelegate(client_onReconnect);
            client.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.IpcFlatmapClient.OnMessageReceivedDelegate(client_OnMessageReceived);
            client.onConnectionClosed += new Metreos.Core.Sockets.ClientCloseConnectionDelegate(client_onConnectionClosed);

            client.Open(IPAddress.Loopback.ToString(), port, 5);
 
            success &= Test1();
            testcount++;

            if(!success) 
            {
                Console.WriteLine(testcount + " failed");
            }

            success &= Test2();
            testcount++;

            if(!success) 
            {
                Console.WriteLine(testcount + " failed");
            }

            client.Close();
            client.Dispose();

            return success;
        }

        public bool Test1()
        {
            Thread.Sleep(buffertime);
            FlatmapList message = new FlatmapList();
            message.Add(2001, 0);
            return client.Write(2000, message);
        }

        public bool Test2()
        {
            bool written = false;

            for(int i = 0; i < 1000; i++)
            {
                Thread.Sleep(buffertime);

                FlatmapList message = new FlatmapList();
                message.Add(2001, i + 1);

                written = client.Write(2000, message);
                if (!written)
                {
                    break;
                }
                lock (client)
                {
                    if(!received && Monitor.Wait(client, 15000) == false)
                    {
                        Console.WriteLine("No response received from server");
                        break;
                    }
					received = false;
                }
            }

            return written;
        }

		public bool received = false;

        public void client_onReconnect()
        {
            Console.WriteLine("Client: Reconnected");
            Console.WriteLine();
        }

        public void client_onConnectionClosed()
        {
            Console.WriteLine("Client: Connection Closed");
            Console.WriteLine();
        }

        public void client_OnMessageReceived(int messageType, FlatmapList flatmap)
        {
            Console.WriteLine("Client: Message received");
			Console.WriteLine("Client: MessageType: " + messageType);
			Console.WriteLine("Client: Message Count: " + (uint) flatmap.Find(2001, 1).dataValue);
            Console.WriteLine();

            lock (client)
            {
				received = true;
                Monitor.Pulse(client);
            }
        }

        public void server_OnCloseConnection(int socketId)
        {
            Console.WriteLine("Server: Close connection");
            Console.WriteLine();
        }

        public void server_OnMessageReceived(int socketId,
            string receiveInterface, int messageType, FlatmapList message)
        {
			Console.WriteLine("Server: Interface: " + receiveInterface);
            Console.WriteLine("Server: Message received");
            Console.WriteLine("Server: MessageType: " + messageType);
            Console.WriteLine("Server: Message Count: " + (uint) message.Find(2001, 1).dataValue);
            Console.WriteLine();

            bool success = server.Write(socketId, messageType + 1000, message); 
            Debug.Assert(success);
        }

        public void server_OnNewConnection(int socketId, string remoteHost)
        {
            Console.Write("Server: New Connection");
            Console.WriteLine();
        }
    }
}
