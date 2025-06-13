using System;
using System.Diagnostics;
using System.Collections;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.LoggingFramework;
using Metreos.Messaging;

namespace Metreos.Providers.Http
{
    /// <summary>
    /// Listening for HTTP requests sent by Metreos Apache module
    /// </summary>
    public class HttpListener : Loggable
    {
        public delegate void IncomingRequestDelegate(HttpMessage message, string guid, string remoteHost, EventMessage.EventType eventType);
        public delegate void IncomingResponseDelegate(HttpMessage message, string guid);

        public IncomingRequestDelegate onIncomingRequest;
        public IncomingResponseDelegate onIncomingResponse;

        private IpcFlatmapServer flatmapServer;
        private int ipcPort;
        private int socketId;			// only has one Ipc client, Apache module.
		private Hashtable guidUuid;

        private const int MESSAGE_APACHE_MODULE	= 3001;

        public HttpListener(int ipcPort) : base(TraceLevel.Warning, "HttpListener")
        {
            this.ipcPort = ipcPort;
            this.socketId = -1;
        }

        public void Start()
        {
			guidUuid = new Hashtable();
			flatmapServer = new IpcFlatmapServer("HttpListener", (ushort)ipcPort, true, base.log.LogLevel);
            flatmapServer.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(this.OnCloseConnection);
            flatmapServer.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.IpcFlatmapServer.OnMessageReceivedDelegate(this.OnMessageReceieved);
            flatmapServer.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(this.OnCloseConnection);
            flatmapServer.Start();

            log.Write(TraceLevel.Info, "HttpListener started, waiting for Apache module to connect.  IPC port is " + ipcPort.ToString());
        }

        public void Stop()
        {
			guidUuid.Clear();
            flatmapServer.Stop();
            log.Write(TraceLevel.Info, "HttpListener stopped, Apache module requests won't be accepted.");
        }

        
        /// <summary> Use this method in the case the assumed GUID found in the 
        /// guid-uuid mapping was invalid because of old, invalid cookies.  In that case,
        /// we should break out the existing queue for the assumed GUID into new guids </summary>
        public string CorrectQueueMapping(string assumedGuid)
        {
            string newGuid = System.Guid.NewGuid().ToString();

            lock(guidUuid.SyncRoot)
            {
                if(guidUuid.Contains(assumedGuid))
                {
                    Queue q = guidUuid[assumedGuid] as Queue;
                    lock(q.SyncRoot)
                    {
                        if(q.Count == 1)
                        {
                            guidUuid.Remove(assumedGuid);
                            guidUuid[newGuid] = q;
                        }
                        else
                        {
                            Queue newQueue = new Queue(new object[] { q.Dequeue() });
                            guidUuid[newGuid] = newQueue;
                        }
                        
                    }
                }
            }
            return newGuid;
        }
		
        private void OnNewConnection(int socketId, string remoteHost)
        { 
            log.Write(TraceLevel.Info, "Apache module connected.");
        }

        private void OnCloseConnection(int socketId)
        {
            log.Write(TraceLevel.Info, "Apache module disconnected.");
        }

        private void OnMessageReceieved(int socketId, string receiveInterface, int messageType, FlatmapList message)
        {
			log.Write(TraceLevel.Verbose, "Received message from Apache module");

            if (messageType != MESSAGE_APACHE_MODULE)
            {
                log.Write(TraceLevel.Warning, "Received unknown message type from Apache module.");
                return;
            }

            this.socketId = socketId;

            for (int i=0; i<message.Count; i++)
            {
                // LRM 6/11: Change key/value logging to verbose.
                log.Write(TraceLevel.Verbose, "key=" + message.GetAt(i).key.ToString() + " " +
                    "value=" + message.GetAt(i).dataValue.ToString());
            }

            HttpMessage msg = new HttpMessage();
            if(msg.LoadFromFlatmap(message) == false)
            {
                log.Write(TraceLevel.Warning, "Failed to load flatmap message into HttpMesage.");
                return;
            }
            else
            {
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

				string apacheUuid = msg.uniqueId;
				msg.uniqueId = guid;

				lock(guidUuid.SyncRoot)
				{
					if (guidUuid.ContainsKey(msg.uniqueId))
					{
						Queue q = guidUuid[msg.uniqueId] as Queue;

						lock(q.SyncRoot )
						{
							q.Enqueue(apacheUuid);
						}
					}
					else
					{
						Queue q = new Queue();
						lock(q.SyncRoot )
						{
							q.Enqueue(apacheUuid);
						}
						guidUuid.Add(msg.uniqueId, q);
					}
				}

                if(this.onIncomingRequest != null)
                {
                    log.Write(TraceLevel.Verbose, "Passing Apache module request to Http Provider.");
                    this.onIncomingRequest(msg, msg.uniqueId, msg.remoteHost, eventType);
                }
            }
        }

        public void SendResponse(HttpMessage msg, string remoteHost)
        {
            log.Write(TraceLevel.Verbose, "Enter SendResponse, remote host is " + remoteHost + " and GUID=" + msg.uniqueId);

            if(msg.type != HttpMessage.Type.Response)
            {
                log.Write(TraceLevel.Warning, "Invalid message type for response, type is " + msg.type);
                return;
            }

            if (socketId == -1)
            {
                log.Write(TraceLevel.Warning, "No valid socket id to send to");
                return;
            }

			bool bFound = false;
			string apacheUuid = null;
			lock(guidUuid.SyncRoot)
			{
				if (guidUuid.ContainsKey(msg.uniqueId))
				{
					object o = guidUuid[msg.uniqueId];
					Queue q = (Queue)o;	
					lock(q.SyncRoot )
					{
						if (q.Count > 0)
							apacheUuid = q.Dequeue() as string;
						if (q.Count == 0)
							guidUuid.Remove(msg.uniqueId);
					}
					
					if (apacheUuid != null)
					{
						msg.uniqueId = apacheUuid;
						bFound = true;
					}
				}
			}

			if (!bFound)
				return;

            FlatmapList message = new FlatmapList();

            if (msg.PopulateFlatmap(ref message))
            {
                for (int i=0; i<message.Count; i++)
                {
                    // LRM 6/11: Change key/value logging to verbose.
                    log.Write(TraceLevel.Verbose, "key=" + message.GetAt(i).key.ToString() + " " +
                        "value=" + message.GetAt(i).dataValue.ToString());
                }
                flatmapServer.Write(socketId, MESSAGE_APACHE_MODULE, message);
                log.Write(TraceLevel.Verbose, "Response has been sent to Apache module, entry count is " + message.Count.ToString());
            }
            else
            {
                log.Write(TraceLevel.Warning, "Failed to populate flatmap from HttpMessage.");
            }
        }
    }
}
