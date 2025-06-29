using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.Interfaces;

namespace Metreos.Core.IPC.Flatmaps.LogServer
{
    /// <summary>
    ///     Creates a log file and writes to it
    /// </summary>
    public class LogClient : IDisposable
    {
		private const int maxQueueSize	=	1500;

        private IpcFlatmapClient flatmapClient;
        private object flatmapClientLock = new object();
        
		private Thread writeThread;
		private ServerLogQueue msgQ;
		private readonly object syncRoot;

		private string name;
		private ushort port;

		public string Name { get { return name; } set { name = value; } }
		public ushort Port { get { return port; } set { port = value; } }
        
		public LogClient(string name)
            : this(name, IServerLog.Default_Port) { }

		private static int GetNextSeq()
		{
			return Interlocked.Increment( ref nextSeq );
		}

		private static int nextSeq = 1;

        /// <summary>
        /// Log Server Client
        /// </summary>
        /// <param name="name"> The name of the log folder </param>
		/// <param name="port"> Log Server port </param>
		/// <param name="traceLevel"> Trace level </param>
        public LogClient(string name, ushort port)
		{
            if(port < 1024)
                throw new ArgumentException("Invalid log server port: " + port);

			if(name == null)
				name = "LogServerClient" + GetNextSeq();

			this.name = name;
			this.port = port;
			this.syncRoot = new object();
			this.msgQ = new ServerLogQueue(maxQueueSize);

			ConnectLogServer();
		}

        /// <summary>
        ///     Handles connecting to the Log Server.  Will reattempt until successful
        /// </summary>
        public void ConnectLogServer()
        {
            flatmapClient = new IpcFlatmapClient(new IPEndPoint(IPAddress.Loopback, port));
			flatmapClient.onConnect = new OnConnectDelegate(OnConnect);
			flatmapClient.onFlatmapMessageReceived = new OnFlatmapMessageReceivedDelegate(OnMessageReceieved);
			flatmapClient.onClose = new OnCloseDelegate(OnClose);

			flatmapClient.Start();
			StartWrite();
		}

		/// <summary>
		///     The IpcFlatmapClient handles reconnection to the server automatically for us--
		///     this is the event handler for that occurrence.   We send an introduction 
		///     asynchronously, because this thread is shared with the message pump for the socket
		/// </summary>
		private void OnConnect(IpcClient c, bool reconnect)
		{
			// We can't block this event, because it will block the message pump of the IpcClient,
			// so we have to send the introduction asynchronously
			setState(State.OPEN);
		}

		/// <summary>
		///     Receiving a connection closed message will merely cause log requests to build up
		///     until the Log Server comes back online
		/// </summary>
		private void OnClose( IpcClient client, Exception e )
		{
			if (e == null)
			{
				if (state == State.FAILED)
					setState( State.CLOSED );
				else
					setState( State.SHUTDOWN );
			}
			else
			{
				setState( State.CLOSED );
			}
		}

        /// <summary>
        /// Set IPC connection state
        /// </summary>
        /// <param name="newState"></param>
		private void setState( State newState )
		{
			State oldState = state;
			state = newState;
			
			switch (newState)
			{
				case State.CLOSED:
					// don't need to do anything
					break;

				case State.OPEN:
					SendIntroduction();
					break;

				case State.READY:
					break;

				case State.FAILED:
					flatmapClient.Close();
					break;

				case State.SHUTDOWN:
					flatmapClient.Close();
					break;
			}
		}

		private enum State { CLOSED, OPEN, READY, FAILED, SHUTDOWN };

		private State state = State.CLOSED;
 
        /// <summary>
        /// Stop and dispose objects
        /// </summary>
		public void Dispose()
		{
            setState(State.SHUTDOWN);

			StopWrite();

            if(flatmapClient != null)
            {
                flatmapClient.Close();
                flatmapClient.Dispose();
            }
        }
        
		/// <summary>
		/// Callback event handler to write logs
		/// </summary>
		/// <param name="timeStamp">The time that the event that this entry represents occurred.</param>
		/// <param name="errorLevel">Trace level</param>
		/// <param name="message">log message</param>
        public void WriteLog(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            string logTimeStamp = timeStamp.ToString(ILog.LongTimestampFormat);
            WriteMessage logWrite = new WriteMessage(errorLevel, message, logTimeStamp);
            FlatmapList logMessage = logWrite.Create();        
            msgQ.TryEnqueue(logMessage);
		}

		/// <summary>
		/// Callback event handler to write logs
		/// </summary>
		/// <param name="timeStamp">The time that the event that this entry represents occurred.</param>
		/// <param name="traceLevel">Trace level</param>
		/// <param name="message">log message</param>
		public void WriteLog(DateTime timeStamp, int traceLevel, string message)
		{
			WriteLog(timeStamp, (TraceLevel)traceLevel, message);
		}

		/// <summary>
		/// Callback event handler to write logs
		/// </summary>
		/// <param name="traceLevel">Trace level</param>
		/// <param name="message">log message</param>
		public void WriteLog(TraceLevel traceLevel, string message)
		{
			WriteLog(DateTime.Now, traceLevel, message);
		}

		/// <summary>
		/// Callback event handler to write logs
		/// </summary>
		/// <param name="traceLevel">Trace level</param>
		/// <param name="message">log message</param>
		public void WriteLog(int traceLevel, string message)
		{
			WriteLog(DateTime.Now, traceLevel, message);
		}

        /// <summary>
        ///     Here we either expect a response to our introduction message,
        ///     or to an ACK to a log message
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="flatmap"></param>
        private void OnMessageReceieved(IpcFlatmapClient ipcClient, int messageType, FlatmapList flatmap)
        {
            lock(flatmapClientLock)
            {
                switch(messageType)
                {
                    case IServerLog.Message_IntroductionResponse:
                        IntroductionResponse response = new IntroductionResponse(flatmap);
                        setState( response.Success ? State.READY : State.FAILED );
                        break;
                }
            }
        }

        /// <summary>
        ///     Attempts to send a log if possible, returning false if the write failed or if 
        ///     we are not ready to send to the server, because the server is down
        /// </summary>
        /// <param name="logMessage"> A properly formed Log Message flatmap </param>
        /// <returns> <c>true</c> if successful, otherwise <c>false</c> </returns>
        private bool SendLog(FlatmapList logMessage)
        {
            if (state == State.READY)
            {
                return flatmapClient.Write(IServerLog.Message_WriteRequest, logMessage);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Sends an introductory message to the Log Server, and waits on the response.
        /// </summary>
        private void SendIntroduction()
        {
            // Construct Introduction Request            
            IntroductionMessage introduction = new IntroductionMessage(name);

            bool success = flatmapClient.Write(IServerLog.Message_IntroductionRequest, introduction.Create()); 
        
			if(success == false)
			{
				Console.WriteLine("Unable to communicate with the Log Server");
			}
        }

		/// <summary>
		/// Start write thread
		/// </summary>
		public void StartWrite()
		{
			writeThread = new Thread(new ThreadStart(WriteWorker));
            writeThread.Name = "Log server client writer thread";
            writeThread.IsBackground = true;
			writeThread.Start();
		}

		/// <summary>
		/// Stop write thread
		/// </summary>
		public void StopWrite()
		{
			if (writeThread != null)
			{
				writeThread.Abort();
				writeThread = null;
			}
		}
		
		/// <summary>
		/// Entrypoint of write thread.
		/// </summary>
		private void WriteWorker()
		{
			while(true)
			{
				if (state != State.READY)
					WaitServerReady();

				int tossed = 0;

				object obj = msgQ.TryPeekWait(10, out tossed);
				if (obj == null)
					continue;

				if (tossed > 0)
				{
					if (!SendLog(new WriteMessage(
						TraceLevel.Warning,
						"-- tossed "+tossed+" messages --",
						DateTime.Now.ToString(ILog.LongTimestampFormat)).Create()))
					{
						// server may not be ready...
						WaitServerReady();
						continue;
					}
				}

				FlatmapList logMessage = obj as FlatmapList;        
                if (!SendLog(logMessage))
                {
                    // server may not be ready...
                    WaitServerReady();
                    continue;
                }
                else
                {
                    // dequeue it (it may not be in the queue anymore)
                    msgQ.DequeueIf(obj, tossed);
                }
			}
		}

        /// <summary>
        /// Wait for server to get into READY state
        /// </summary>
		private void WaitServerReady()
		{
			while (state != State.READY)
				Thread.Sleep(50);
		}
	}
}
