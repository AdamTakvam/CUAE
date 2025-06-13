using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;

namespace Metreos.Providers.Http.Tests
{
    internal class MockSocketServer : SocketServerBase
    {
        public MockSocketServer(ushort listenPort) 
			: base("MockSocketServer", TraceLevel.Info, listenPort)
        {}

        protected override void Run()
        {
            this.shutdownComplete.Set();
        }

        protected override void StopSocket(Socket client)
        {}
    }


    public class SocketServerBaseTest
    {
        public const ushort PORT = 34245;

        private MockSocketServer socketServerA;
        private MockSocketServer socketServerB;

        public SocketServerBaseTest()
        {}

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            socketServerA.Cleanup();
            socketServerB.Cleanup();
        }

        [csUnit.Test]
        public void testTwoSocketServersOnTheSamePort()
        {
            socketServerA = new MockSocketServer(PORT);
            socketServerB = new MockSocketServer(PORT);

            csUnit.Assert.True(socketServerA.Start());
            csUnit.Assert.False(socketServerB.Start());

            socketServerB.ListenPort = PORT + 1;

            csUnit.Assert.Equals(PORT + 1, socketServerB.ListenPort);
            csUnit.Assert.True(socketServerB.Start());

            // Should have no affect because the socket server is already started.
            socketServerB.ListenPort = 0;

            csUnit.Assert.Equals(PORT + 1, socketServerB.ListenPort);

            socketServerA.Stop();
            socketServerB.Stop();
        }
    }
}
