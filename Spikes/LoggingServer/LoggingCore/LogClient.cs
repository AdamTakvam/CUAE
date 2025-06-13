using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Metreos.Messaging;
using Metreos.Messaging.Ipc;

namespace LoggingCore
{
    /// <summary>Simple client to handle communication with the
    /// logging server.</summary>
	public class LogClient
	{
        /// <summary>Handles TCP communication with the server.</summary>
        private TcpClient tcpClient;

        /// <summary>Network stream for writing to the server.</summary>
        private NetworkStream tcpClientStream;

        private byte[] headerReadBuffer = new byte[8];
        private byte[] messageReadBuffer;

        private AsyncCallback clientHeaderReadCallback;
        private AsyncCallback clientMessageReadCallback;

		public LogClient()
        {
            clientHeaderReadCallback = new AsyncCallback(ClientHeaderReadCallback);
            clientMessageReadCallback = new AsyncCallback(ClientMessageReadCallback);
        }

        /// <summary>Start up a new session with the log server.</summary>
        /// <param name="ipAddress">IP address of the log server.</param>
        /// <param name="port">Port of the log server.</param>
        public void StartLogSession(string ipAddress, int port)
        {
            tcpClient = new TcpClient();
            
            tcpClient.Connect(ipAddress, port);
            tcpClientStream = tcpClient.GetStream();

            tcpClientStream.BeginRead(headerReadBuffer, 0, headerReadBuffer.Length, clientHeaderReadCallback, null);
        }

        private void ClientHeaderReadCallback(IAsyncResult result)
        {
            int pendingMessageLength = BitConverter.ToInt32(headerReadBuffer, 0);
            messageReadBuffer = new byte[pendingMessageLength];

            Console.WriteLine("Pending message: {0}", pendingMessageLength);

            tcpClientStream.BeginRead(messageReadBuffer, 0, pendingMessageLength, clientMessageReadCallback, null);
        }

        private void ClientMessageReadCallback(IAsyncResult result)
        {
            Console.WriteLine("Got message...");
            headerReadBuffer.Initialize();
            //tcpClientStream.BeginRead(headerReadBuffer, 0, headerReadBuffer.Length, clientHeaderReadCallback, null);
        }

        /// <summary>Terminate an existing session with the log server.</summary>
        public void StopLogSession()
        {
            tcpClientStream.Flush();
            tcpClientStream.Close();
            
            tcpClient.Close();
        }

        /// <summary>Write a log message.</summary>
        /// <param name="body">The message body.</param>
        /// <param name="category">The category of message.</param>
        public void Write(string body, string category)
        {
            LogMessage msg = new LogMessage(body, category);
            byte[] flatmapData = msg.ToFlatmapByteArray();
            
            byte[] msgHeader = BitConverter.GetBytes((long)flatmapData.Length);
            Debug.Assert(msgHeader.Length == 8);            

            tcpClientStream.Write(msgHeader, 0, msgHeader.Length);
            tcpClientStream.Write(flatmapData, 0, flatmapData.Length);
        }
	}
}
