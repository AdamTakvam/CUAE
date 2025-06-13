using System;
using System.IO;
using System.Diagnostics;
using System.Collections;

using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Core.IPC.Flatmaps.LogServer;
using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.Interfaces;

namespace Metreos.LogServer
{
	/// <summary> 
	///     The Log Server
	///     
	///     Usage:
	///     ------  
	///     
	///     Connect using the Flatmap Client.  
	///     Send an Introduction Message, which can have success/failure response.
	///     After a successful introduction, you can send Write Messages, Refresh Messages, and 
	///     a Dispose Message.  Send a Dispose Message to indicate that you are leaving.
	///     If you don't send a Dispose and close your socket, 
	///     once the server becomes aware of a disconnect, it will remove the clients session.
	///     
	///     Messages:
	///     
	///     1.  Introduction Message
	///     ------------------------
	///     
	///     Message ID = 1004
	///     Message Parameter 'Name' = 1000
	///         'Name' will result in a folder being created by the LogServer of the same
	///                 value as 'Name', which will contain all logs from this client.
	///     
	///     2.  Introduction Response
	///     -------------------------
	///     
	///     Message ID = 1005
	///     Message Parameter 'Success' = 1000
	///         'Success' is a boolean value.  0 is failure, 1 is success
	///         The LogServer will not close the connection if a failure occurs.
	///         
	///     3.  Write Message
	///     ----------------
	///     
	///     Message ID = 1001
	///     Message Parameter 'LogLevel' = 1000
	///         'LogLevel' is a string value enumeration, of 'Error', 'Warning', 'Info', or 'Verbose'.
	///         This value will be included in the log message
	///     Message Parameter 'Message' = 1001
	///         'Message' is a string representing the log message
	///     Message Parameter 'TimeStamp' = 1002
	///         'TimeStamp' is a string representing the event time
	///     
	///     4.  Write Response
	///     ------------------
	///
	///     Message ID = 1006
	///         No parameters
	///         Any client implementation should wait on the WriteResponse before writing again.  To do otherwise
	///         can overrun the LogServer, causing it to drop messages.  
	///         If a response is not received after X amount of time,
	///         then it is up to the client to retry or not.        
	///          
	///     5.  Refresh Message
	///     -------------------
	///     
	///         No purpose at the moment
	///     
	///     6.  Dispose Message
	///     -------------------
	///     
	///     Message ID = 1002
	///         No parameters.
	///         The Server will close the connection after processing this message
	///         
	///     7.  Flush Message
	///     -------------------
	///     
	///     Message ID = 1007
	///         No parameters.
	///         The Server will close the current log file and create a new one
	///         
    /// </summary>
    /// 

	#region Log Server Implementation
	public class LogServer : IDisposable
	{
        public static string DefaultPath { get { return System.Environment.CurrentDirectory; } }
        public string LocalPath { get { return path; } set { path = value; } }
        public ushort Port { get { return port; } set { port = value; } }
        public uint NumberLines { get { return numberLines; } set { numberLines = value; } } 
		public uint NumberFiles { get { return numberFiles; } set { numberFiles = value; }}

        /// <summary>
        ///     Private accessor to simplify the situation of a non-specified LocalPath
        /// </summary>
        private string Path { get { if(LocalPath == null)
                {
                    return DefaultPath;
                }
                else
                {
                    return LocalPath;
                }
            }
        }

        private string path;                        // log path
        private ushort port;                        // IPC port
        private uint numberLines;                   // number of log lines
		private uint numberFiles;                   // number of log files
        private LogClient logger;                   // internal logger
        
        private Hashtable clients;                  // log client map
        private IpcFlatmapServer flatmapServer;     // flatmap IPC server

        private Metreos.Core.Sockets.NewConnectionDelegate newConnDelegate;
        private IpcFlatmapServer.OnMessageReceivedDelegate messageReceiveDelegate;
        private Metreos.Core.Sockets.CloseConnectionDelegate closeConnDelegate;
		
        public LogServer()
		{
            port                             = IServerLog.Default_Port;
            path                             = null;
            clients                          = Hashtable.Synchronized(new Hashtable());
            numberLines                      = IServerLog.Default_NumberLines;
			numberFiles						 = IServerLog.Default_NumFiles;
            newConnDelegate                  = new Metreos.Core.Sockets.NewConnectionDelegate(OnNewConnection);
            messageReceiveDelegate           = new IpcFlatmapServer.OnMessageReceivedDelegate(OnMessageReceieved);
            closeConnDelegate                = new Metreos.Core.Sockets.CloseConnectionDelegate(OnCloseConnection);
        }
    
        /// <summary>
        ///     Starts the Log Server.  Before starting the Log Server, be sure to have all properties
        ///     set to the values to use during operation.
        /// </summary>
        public void Start()
        {
            // Add internal logger
            logger = new LogClient(Path, "LogServer", numberLines, numberFiles);
            logger.onFileError += new OnFileErrorDelegate(client_onFileError);
            logger.CreateLogFile(null);

            flatmapServer                    = new IpcFlatmapServer("Log Server", port, true, TraceLevel.Info);
            flatmapServer.OnNewConnection   += newConnDelegate;
            flatmapServer.OnMessageReceived += messageReceiveDelegate;
            flatmapServer.OnCloseConnection += closeConnDelegate;
            flatmapServer.Start();
        }

        /// <summary>
        ///     Stops accepting connections.  Closes all open logs and client sessions.
        /// </summary>
        public void Stop()
        {
            flatmapServer.Stop();
            flatmapServer.OnNewConnection   -= newConnDelegate;
            flatmapServer.OnMessageReceived -= messageReceiveDelegate;
            flatmapServer.OnCloseConnection -= closeConnDelegate;

            lock(clients.SyncRoot)
            {
                foreach(LogClient client in clients.Values)
                {
                    client.WriteLog(TraceLevel.Verbose, "Log Server:  Ending log file due to Log Server Stop.\n" + 
                        "Any further logging by this client will be in the next log", null);

                    client.Dispose();
                }
            }

            logger.Dispose();
        }

        /// <summary>
        ///     Event provided by IpcFlatmapServer.  Not used.  
        /// </summary>
        private void OnNewConnection(int socketId, string remoteHost)
        { }

        /// <summary>
        ///     Event provided by IpcFlatmapServer.   
        ///     Will either
        ///     1.  Create a new session for a client if an Introduction is provided,
        ///     2.  Write to a client's log file
        ///     3.  Process a Refresh
        ///     4.  Process a Dispose request, which will shutdown the server completely
        /// </summary>
        /// <param name="socketId"> The ID of the socket </param>
        /// <param name="receiveInterface"> The interface listened to which received this message </param>
        /// <param name="messageType"> The unique ID of the message </param>
        /// <param name="message"> The body of the message </param>
        private void OnMessageReceieved(int socketId, string receiveInterface, int messageType, FlatmapList message)
        {
            lock(clients.SyncRoot)
            {
                // A new client is communicating with us. This message should be a introduction message
                if(!clients.Contains(socketId))
                {
                    // We have a new client with an introduction.  
                    // Add a new client to the clients list, and create the directory and a new log
                    // And respond to client
                    switch(messageType)
                    {
                        case IServerLog.Message_IntroductionRequest:

                            bool success = CreateClient(socketId, message);

                            if(success == false)
                            {
                                // Unable to create the client in our internal data structures. Must bail out
                                SendIntroductionFailure(socketId);
                                return;
                            }

                            SendIntroductionSuccess(socketId);

                            DumpClientList();

                            break;
                    }
                }
                else
                {
                    LogClient client = clients[socketId] as LogClient;

                    switch(messageType)
                    {
                        case IServerLog.Message_WriteRequest:
                            WriteClient(socketId, client, message);
                            break;

                        case IServerLog.Message_DisposeRequest:
                            DisposeClient(socketId, client);
                            break;

                        case IServerLog.Message_IntroductionRequest:
                            // Since we already have this socketId in our client list
                            // We can assume success
                            
                            // If the client is sending an introduction, then it in effect 
                            // expecting a new session, which means we should start a new log
                            //client.NewLog();
                            SendIntroductionSuccess(socketId);
                            break;

                        case IServerLog.Message_RefreshRequest:
                            RefreshClient(client, message);
                            break;

						case IServerLog.Message_FlushRequest:
							client.NewLog(null);
							break;

                        default:
                            // Log unknown message
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     An event provided by the IpcFlatmapServer. 
        ///     A closing of a socket by the client is treated as a session end
        /// </summary>
        /// <param name="socketId"></param>
        private void OnCloseConnection(int socketId)
        {
            // Check if the client was not able to gracefully disconnect
            lock(clients.SyncRoot)
            {
                if(clients.Contains(socketId))
                {
                    LogClient client = clients[socketId] as LogClient;
                    DisposeClient(socketId, client);
                }
            }
            DumpClientList();
        }

        /// <summary>
        ///     Writes a message to a client's log file, and ACKs the write request,
        ///     to avoid a client overrunning the server socket's buffer
        /// </summary>
        /// <param name="socketId"> The ID of the socket </param>
        /// <param name="client"> The Client session datastructure </param>
        /// <param name="flatmap"> The body of the write message </param>
        private void WriteClient(int socketId, LogClient client, FlatmapList flatmap)
        {
            WriteMessage writeMessage = new WriteMessage(flatmap);
            client.WriteLog(writeMessage.LogLevel, writeMessage.Message, writeMessage.TimeStamp);

            // No need to confirm delivery.
            //WriteResponse ack = new WriteResponse();
            //flatmapServer.Write(socketId, IServerLog.Message_WriteResponse, ack.Create());
        }

        /// <summary>
        ///     Called to dispose the Log Server completely. Closes all client's log files
        /// </summary>
        public void Dispose()
        {
            if(flatmapServer != null)
            {
                flatmapServer.Dispose();
            }

            lock(clients.SyncRoot)
            {
                foreach(LogClient client in clients.Values)
                {
                    client.WriteLog(TraceLevel.Verbose, "Log Server:  Ending log file due to Log Server shutdown.\n" + 
                        "Any further logging by this client will be in the next log", null);

                    client.Dispose();
                }
            }
        }

        /// <summary>
        ///     Refresh the configuration of the Log Server.
        ///     TODO: Only a stub right now
        /// </summary>
        /// <param name="client"> The client session datastructure </param>
        /// <param name="flatmap"> The body of the refresh message </param>
        private void RefreshClient(LogClient client, FlatmapList flatmap)
        {
            RefreshMessage refreshMessage = new RefreshMessage(flatmap);
            client.Name = refreshMessage.Name;
        }

        /// <summary>
        ///     Ensures that the resources contained by the client are destroyed, 
        ///     and removes the client from the list of clients
        /// </summary>
        /// <param name="socketId"> The ID of the socket </param>
        /// <param name="client"> The client data structure </param>
        private void DisposeClient(int socketId, LogClient client)
        {
            string msg = string.Format("Log client {0} removed.", client.Name);
            logger.WriteLog(TraceLevel.Info, msg, null);

			client.Dispose();
            lock(clients.SyncRoot)
            {
                clients.Remove(socketId);
            }
			//flatmapServer.CloseConnection(socketId);
        }

        /// <summary>
        ///     Sends an Introduction Success message to the specified socket
        /// </summary>
        /// <param name="socketId"> The ID of the socket </param>
        private void SendIntroductionSuccess(int socketId)
        {
            IntroductionResponse response = new IntroductionResponse(true);
            flatmapServer.Write(socketId, IServerLog.Message_IntroductionResponse, response.Create());
        }

        /// <summary>
        ///     Sends an Introduction Failure message to the specified socket
        /// </summary>
        /// <param name="socketId"> The ID of the socket </param>
        private void SendIntroductionFailure(int socketId)
        {
            IntroductionResponse response = new IntroductionResponse(false);
            flatmapServer.Write(socketId, IServerLog.Message_IntroductionResponse, response.Create());
        }

        /// <summary>
        ///     Initializes a client session data struture and the logging resources necessary
        ///     to handle Log Messages from this client
        /// </summary>
        /// <param name="socketId"> The ID of the socket </param>
        /// <param name="message"> The body of the Introduction Message </param>
        /// <returns> <c>true</c> if the client log resources could be created, otherwise <c>false</c></returns>
        private bool CreateClient(int socketId, FlatmapList message)
        {
            bool success = true;
            try
            {
                IntroductionMessage intro = new IntroductionMessage(message);
                LogClient client = new LogClient(Path, intro.Name, numberLines, numberFiles);
                client.onFileError += new OnFileErrorDelegate(client_onFileError);
                clients[socketId] = client;
                client.CreateLogFile(null);

                string msg = string.Format("New log client {0} added.", intro.Name);
                logger.WriteLog(TraceLevel.Info, msg, null);
            }
            catch{ success = false; }

            return success;
        } 
 
        /// <summary>
        /// Dump current list into log file
        /// </summary>
        private void DumpClientList()
        {
            logger.WriteLog(TraceLevel.Info, "****** Current client list:", null);
            int count = 0;
            lock(clients.SyncRoot)
            {
                foreach(LogClient client in clients.Values)
                {
                    count++;
                    string msg = string.Format("\t\t{0}-{1}", count, client.Name);
                    logger.WriteLog(TraceLevel.Info, msg, null);
                }                    
            }
        }

        /// <summary>
        /// Callback to log error into internal logging
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        private void client_onFileError(string name, string msg)
        {
            string emsg = string.Format("{0} - {1}", name, msg);
            logger.WriteLog(TraceLevel.Error, emsg, null);
        }
    }
	#endregion	
}
