using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Diagnostics;

using Metreos.LoggingFramework;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Core.Sockets;

namespace Metreos.Providers.Http
{
	/// <summary>
	/// A basic HTTP stack. Not for general purpose use.
	/// </summary>
	public sealed class HttpStack : SocketServerThreaded
	{
        public delegate void IncomingRequestDelegate(HttpMessage message, string guid, string remoteHost, EventMessage.EventType eventType);
        public delegate void IncomingResponseDelegate(HttpMessage message, string guid);

        private sealed class ConnectionInfo
        {
            public string remoteHost;
            public int socketId;
            public string guid;
            public bool valid;
        }

        private new Hashtable activeConnections;
        private new object activeConnectionsLock;

        private Hashtable pendingResponses;
        private object pendingResponsesLock;

        private Hashtable pendingRequests;
        private object pendingRequestsLock;

        private Hashtable pendingMessages;

        public IncomingRequestDelegate onIncomingRequest;
        public IncomingResponseDelegate onIncomingResponse;

        public HttpStack() : this(8000, TraceLevel.Warning) 
        {}

        public HttpStack(int port, TraceLevel logLevel) 
            : base(typeof(HttpStack).Name, (ushort)port, false, logLevel)
        {
            this.activeConnections = new Hashtable();
            this.activeConnectionsLock = new Object();

            this.pendingMessages = new Hashtable();

            this.pendingResponses = new Hashtable();
            this.pendingResponsesLock = new object();

            this.pendingRequests = new Hashtable();
            this.pendingRequestsLock = new object();
        }

        protected override void NewConnection(int socketId, string remoteHost)
        {
            ConnectionInfo conx;

            lock(this.activeConnectionsLock)
            {
                if(this.activeConnections.Contains(socketId) == true)
                {
                    // socketId already in activeConnections.
                    conx = (ConnectionInfo) this.activeConnections[socketId];

                    if(conx.valid == true)
                    {
                        // Connection still valid, this socketId is a duplicate. Throw an error and refuse connection.
                        log.Write(TraceLevel.Error, "A new connection has been created but the socketId is already in activeConnections. Refusing connection.");

                        base.CloseConnection(socketId);
                        return;
                    }
                }
                else
                {
                    // New socketId so definately a new connection.
                    conx = new ConnectionInfo();

                    this.activeConnections.Add(socketId, conx);
                }
            }

            conx.socketId = socketId;
            conx.remoteHost = remoteHost;
            conx.valid = true;
        }


        protected override void DataReceived(int socketId, string receiveIpAddress, byte[] byteData, int dataLength)
        {
            ConnectionInfo conxInfo = null;

            lock(this.activeConnectionsLock)
            {
                if(this.activeConnections.Contains(socketId) == false)
                {
                    // socketId not in activeConnections so there is no way we should be receiving data for this socket.
                    // Ignore data and toss an error.
                    log.Write(TraceLevel.Error, "Received data for a connection that is not in activeConnections table. Ignoring");
                    return;
                }

                conxInfo = (ConnectionInfo)this.activeConnections[socketId];
            }

            if(conxInfo == null)
            {
                log.Write(TraceLevel.Error, "No connection info for inbound data. Ignoring");
                return;
            }

            HttpMessage msg;
            bool isNewMessage;

            if((isNewMessage = this.pendingMessages.Contains(socketId)) == true)
            {
                // Append this new data to a pending HttpMessage.
                msg = (HttpMessage)this.pendingMessages[socketId];
            }
            else
            {
                // No previous partial message, so this is a new message entirely.
                msg = new HttpMessage(receiveIpAddress, base.ListenPort);
            }

            // Build a buffer to send the data along.
            string data = System.Text.Encoding.ASCII.GetString(byteData, 0, dataLength);

            if(msg.Parse(data) == false)
            {
                if(msg.ParseError == true)
                {
                    log.Write(TraceLevel.Warning, "HttpMessage returned a parse error.");

                    // REFACTOR: Send a bad message response. Probably need to send a 406 Not Acceptable.
                    // REFACTOR: Close the connection after sending response.

                    return;
                }
                else if(isNewMessage == false)
                {
                    // Parsing was incomplete, and this is a new message, so lets save it for 
                    // later so we can append data.
                    
                    this.pendingMessages.Add(socketId, msg);
                }
            }
            else
            {
                // Parsing was successfull, lets pass this message on to the provider

                if(msg.type == HttpMessage.Type.Request)
                {
                    // It is a request, either grab a guid from the sessionId or generate a new guid.
                    // This is the routingGuid.

                    string guid;
                    EventMessage.EventType eventType;

                    if(msg.headers.Contains(IHttp.Headers.SESSION_ID) == true)
                    {
                        guid = msg.headers[IHttp.Headers.SESSION_ID] as string;
                        eventType = EventMessage.EventType.NonTriggering;
                    }
                    else if(msg.cookies.Contains(IHttp.COOKIE_SESSION_ID) == true)
                    {
                        guid = msg.cookies[IHttp.COOKIE_SESSION_ID] as string;
                        eventType = EventMessage.EventType.NonTriggering;
                    }
                    else if(msg.queryParams.ToLower().IndexOf(IHttp.QUERY_SESSION_ID) != -1)
                    {
                        int start = msg.queryParams.ToLower().IndexOf(IHttp.QUERY_SESSION_ID);

                        int end = msg.queryParams.IndexOf("&", start);   
                        if(end < 0) end = msg.queryParams.Length;

                        string tempId = msg.queryParams.Substring(start, end - start);
                        tempId = tempId.Trim(new char[]{' ', '&'});

                        string[] idBits = tempId.Split(new char[] {'='}, 2);
                        if((idBits[1] != String.Empty) && (idBits[1] != null))
                        {
                            guid = idBits[1];
                            eventType = EventMessage.EventType.NonTriggering;
                        }
                        else
                        {
                            guid = System.Guid.NewGuid().ToString();
                            eventType = EventMessage.EventType.Triggering;
                        }
                    }
                    else
                    {
                        guid = System.Guid.NewGuid().ToString();
                        eventType = EventMessage.EventType.Triggering;
                    }

                    conxInfo.guid = guid;

                    // Add a new pending response entry.
                    lock(this.pendingResponsesLock)
                    {
                        if(!this.pendingResponses.Contains(conxInfo.remoteHost))
                        {
                            this.pendingResponses.Add(conxInfo.remoteHost, conxInfo);
                        }
                        else
                        {
                            HttpMessage busyResponse = new HttpMessage(base.ListenInterface.Address.ToString(), 
                                base.ListenInterface.Port);

                            busyResponse.type = HttpMessage.Type.Response;
                            busyResponse.responseCode = "409";
                            busyResponse.responsePhrase = "Connection already open to this address and port";

                            this.SendResponse(busyResponse, conxInfo.remoteHost);
                        }
                    }
                    
                    if(this.onIncomingRequest != null)
                    {
                        this.onIncomingRequest(msg, conxInfo.guid, conxInfo.remoteHost, eventType);
                    }
                }
                else if(msg.type == HttpMessage.Type.Response)
                {
                    // This is a response, so the guid should be contained inside the connection info object.
                    // This is the actionGuid that this response is correlated with.

                    if((conxInfo.guid == null) || (conxInfo.guid == ""))
                    {
                        log.Write(TraceLevel.Error, "No actionGuid associated with this response. Ignoring message and closing connection.");

                        base.CloseConnection(socketId);
                        
                        return;
                    }
                    
                    // REFACTOR: Cleanup pendingRequest here.

                    if(this.onIncomingResponse != null)
                    {
                        this.onIncomingResponse(msg, conxInfo.guid);
                    }

                    // We close the connection because we only support 1 request/response per connection.
                    // The server will call back into the stack which will cause us to clean up activeConnections.
                    base.CloseConnection(socketId);
                }
                else
                {
                    log.Write(TraceLevel.Error, "Unknown HTTP message type Ignoring message.");
                    return;
                }
            }
        }


        protected override void ConnectionClosed(int socketId)
        {
        }


        public bool SendRequest(HttpMessage msg, string actionGuid)
        {
            DebugLog.MethodEnter();

            // Sanity check
            if(msg.type != HttpMessage.Type.Request)
            {
                log.Write(TraceLevel.Error, "Cannot send request: Malformed data.");
				DebugLog.MethodExit();
				return false;
            }

            ConnectionInfo conxInfo;

            int socketId = base.CreateConnection(msg.requestUri.Host, msg.requestUri.Port);
            
            if(socketId != -1)
            {
                conxInfo = (ConnectionInfo) this.activeConnections[socketId];

                conxInfo.socketId = socketId;
                conxInfo.remoteHost = msg.requestUri.Host + ":" + msg.requestUri.Port;
                conxInfo.guid = actionGuid;
                conxInfo.valid = true;

                byte[] buffer = System.Text.Encoding.Default.GetBytes(msg.ToString());

				DebugLog.MethodExit(); 
				return base.SendData(socketId, buffer, false);
            }

			DebugLog.MethodExit();
			return false;
        }


        public void SendResponse(HttpMessage msg, string remoteHost)
        {
            DebugLog.MethodEnter();

            // Sanity check
            if(msg.type != HttpMessage.Type.Response)
            {
                log.Write(TraceLevel.Error, "Cannot send response: Malformed data.");
                return;
            }

            ConnectionInfo conxInfo = null;

            lock(this.pendingResponsesLock)
            {
                conxInfo = (ConnectionInfo) this.pendingResponses[remoteHost];
            }

            if(conxInfo == null)
            {
                log.Write(TraceLevel.Error, "No pendingResponse for remoteHost " + remoteHost + ". Ignoring");
                
                // REFACTOR: Send up a failure?
                
                return;
            }

			lock(this.pendingResponsesLock)
			{
				pendingResponses.Remove(remoteHost);
			}

            byte[] buffer = System.Text.Encoding.Default.GetBytes(msg.ToString());

            base.SendData(conxInfo.socketId, buffer, true);

            // REFACTOR:  Seth's fix to make an error response of 404 work.  
            base.CloseConnection(conxInfo.socketId);

            DebugLog.MethodExit();
        }
	}
}
