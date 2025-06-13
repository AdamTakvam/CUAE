using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;

using Metreos.Samoa.Core;

namespace Metreos.Providers.Http.Tests
{
    public sealed class MyTestThreadedProtocolStack
    {
        public StringCollection dataReceived;
        public object dataReceivedLock;
        
        public int connections;
        public object connectionsLock;

        public SocketServerThreaded server;

        public int lastConnectionHandle;

        public MyTestThreadedProtocolStack(ushort port)
        {
            server = new SocketServerThreaded("MyTestProtocolStack", port, TraceLevel.Info);
            
            dataReceived = new StringCollection();
            dataReceivedLock = new Object();

            connections = 0;
            connectionsLock = new Object();

            server.onDataReceived = new SocketServerBase.DataReceivedDelegate(this.OnNewData);
            server.onNewConnection = new SocketServerBase.NewConnectionDelegate(this.OnNewConnection);
            server.onCloseConnection = new SocketServerBase.CloseConnectionDelegate(this.OnCloseConnection);

            server.Start();

            System.Threading.Thread.Sleep(1000);
        }

        public void SendTo(int handle, string data)
        {
            server.SendData(handle, data);
        }

        public void OnNewData(int handle, string data)
        {
            lock(dataReceivedLock)
            {
                dataReceived.Add(data);
            }
        }

        public void OnNewConnection(int handle, string remoteHost)
        {
            lock(connectionsLock)
            {
                connections++;
            }

            this.lastConnectionHandle = handle;
        }

        public void OnCloseConnection(int handle)
        {
            lock(connectionsLock)
            {
                connections--;
            }
        }

        public void Stop()
        {
            server.Stop();
        }
    }


    public class SocketServerThreadedTest
    {
        public const ushort PORT = 23127;
        public MyTestThreadedProtocolStack stack;

        public SocketServerThreadedTest()
        {
        }

        [csUnit.FixtureSetUp]
        public void FixtureSetUp()
        {
            stack = new MyTestThreadedProtocolStack(PORT);
            System.Threading.Thread.Sleep(1000);
        }

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            stack.Stop();
            stack = null;
        }

        [csUnit.Test]
        public void testSingleConnection()
        {
            TcpClient client = new TcpClient(IpUtility.GetIPAddresses()[0], PORT);

            System.Threading.Thread.Sleep(100);

            csUnit.Assert.Equals(1, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);

            client.Close();

            System.Threading.Thread.Sleep(200);

            csUnit.Assert.Equals(0, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);
        }

        [csUnit.Test]
        public void testMultipleConnections()
        {
            int numClientsToUse = 20;
            TcpClient[] clients = new TcpClient[numClientsToUse];

            for(int i = 0; i < numClientsToUse; i++)
            {
                clients[i] = new TcpClient(IpUtility.GetIPAddresses()[0], PORT);
                System.Threading.Thread.Sleep(10);
            }

            System.Threading.Thread.Sleep(100);
            
            csUnit.Assert.Equals(numClientsToUse, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);

            for(int i = 0; i < numClientsToUse; i++)
            {
                clients[i].Close();
                System.Threading.Thread.Sleep(10);
            }

            System.Threading.Thread.Sleep(1000);

            csUnit.Assert.Equals(0, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);
        }

        [csUnit.Test]
        public void testSingleConnectionSend()
        {
            stack.dataReceived.Clear();

            TcpClient client = new TcpClient(IpUtility.GetIPAddresses()[0], PORT);

            System.Threading.Thread.Sleep(100);

            csUnit.Assert.Equals(1, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);

            NetworkStream stream = client.GetStream();

            string strData = "Hello, out there";
            byte[] data = System.Text.Encoding.ASCII.GetBytes(strData);

            stream.Write(data, 0, data.Length);
            stream.Close();

            System.Threading.Thread.Sleep(100);

            csUnit.Assert.Equals(1, stack.dataReceived.Count);
            csUnit.Assert.Equals("Hello, out there", stack.dataReceived[0]);

            client.Close();

            System.Threading.Thread.Sleep(100);

            csUnit.Assert.Equals(0, stack.connections);
        }

        [csUnit.Test]
        public void testSendToConnection()
        {
            stack.dataReceived.Clear();

            TcpClient client = new TcpClient(IpUtility.GetIPAddresses()[0], PORT);

            System.Threading.Thread.Sleep(100);

            csUnit.Assert.Equals(1, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);

            string data = "Wheeeeeeeeeeee!!!!";

            stack.SendTo(stack.lastConnectionHandle, data);

            System.Threading.Thread.Sleep(100);

            NetworkStream stream = client.GetStream();

            byte[] byteBuf = new Byte[4096];
            int bytesRead = stream.Read(byteBuf, 0, 4096);

            data = null;
            data = System.Text.Encoding.ASCII.GetString(byteBuf, 0, bytesRead);

            csUnit.Assert.Equals("Wheeeeeeeeeeee!!!!", data);

            stream.Close();
            client.Close();

            System.Threading.Thread.Sleep(100);

            csUnit.Assert.Equals(0, stack.connections);
        }

        [csUnit.Test]
        public void testSendToMultipleConnections()
        {
            stack.dataReceived.Clear();

            int numClientsToUse = 5;
            int[] handles = new int[numClientsToUse];

            string dataToSend = "This is some test data";

            TcpClient[] clients = new TcpClient[numClientsToUse];

            for(int i = 0; i < numClientsToUse; i++)
            {
                clients[i] = new TcpClient(IpUtility.GetIPAddresses()[0], PORT);
                System.Threading.Thread.Sleep(100);
                handles[i] = stack.lastConnectionHandle;
            }
            
            csUnit.Assert.Equals(numClientsToUse, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);

            for(int i = 0; i < numClientsToUse; i++)
            {
                string dataReceived;

                stack.SendTo(handles[i], dataToSend);

                System.Threading.Thread.Sleep(100);

                NetworkStream stream = clients[i].GetStream();

                byte[] byteBuf = new Byte[4096];
                int bytesRead = 0;

                if(stream.CanRead)
                {
                     bytesRead = stream.Read(byteBuf, 0, 4096);
                }

                stream.Close();

                csUnit.Assert.NotEquals(0, bytesRead);

                dataReceived = System.Text.Encoding.ASCII.GetString(byteBuf, 0, bytesRead);

                csUnit.Assert.Equals(dataToSend, dataReceived);
            }


            for(int i = 0; i < numClientsToUse; i++)
            {
                clients[i].Close();
                System.Threading.Thread.Sleep(10);
            }

            System.Threading.Thread.Sleep(1000);

            csUnit.Assert.Equals(0, stack.connections);
            csUnit.Assert.Equals(0, stack.dataReceived.Count);
        }
    }
}
