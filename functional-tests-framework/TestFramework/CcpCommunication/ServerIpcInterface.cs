using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Core;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Core.Sockets;
using Metreos.Core.ConfigData;
using Metreos.Messaging.MediaCaps;
using MessageTypes = Metreos.TestCallControl.Communication.ICallControlTest.MessageTypes;
using MessageFields = Metreos.TestCallControl.Communication.ICallControlTest.MessageFields;

namespace Metreos.TestCallControl.Communication
{
    public delegate void SendCallEstablishedDelegate(int transactionId, long callId, string to, string from);
    public delegate void SendCallSetupFailedDelegate(int transactionId, long callId, string reason);
    public delegate void SendGotCapabilitiesDelegate(int transactionId, long callId, string txIp, uint txPort, string txControlIp, uint txControlPort, MediaCapsField field);
    public delegate void SendGotDigitsDelegate(int transactionId, long callId, string digits);
    public delegate void SendHangupDelegate(int transactionId, long callId);
    public delegate void SendIncomingCallDelegate(int transactionId, long callId, string to, string from, string originalTo, bool loadTest, bool negCaps);
    public delegate void SendMediaEstablishedDelegate(int transactionId, long callId, string txIp, uint txPort, string txControlIp,
                                                    uint txControlPort, uint rxCodec, uint rxFramesize, uint txCodec,
                                                    uint txFramesize);
    public delegate void SendMediaChangedDelegate(int transactionId, long callId, string txIP, uint txPort);

//    public delegate void SendMediaChangedDelegate(int transactionId, long callId, string txIP, uint txPort, string txControlIP, uint txControlPort, 
//    IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize);
	/// <summary> 
	///     Interface on which a CallControl Provider can communicate in a manner
	///     specific to common methods needed by CallControl.
	///     
	///     Currently ownly supports one client
    /// </summary>
	public class ServerIpcInterface
	{
        public event SendCallEstablishedDelegate    CallEstablishedRequest;
        public event SendCallSetupFailedDelegate    CallSetupFailedRequest;
        public event SendGotCapabilitiesDelegate    GotCapabilitiesRequest;
        public event SendGotDigitsDelegate          GotDigitsRequest;
        public event SendHangupDelegate             HangupRequest;
        public event SendIncomingCallDelegate       IncomingCallRequest;
        public event SendMediaEstablishedDelegate   MediaEstablishedRequest;
        public event SendMediaChangedDelegate       MediaChangedRequest;

        private const int    responseTimeout   = 5000;
        private const string interfaceTaskname = "ServerIpcInterface";
        protected volatile static int transId  = 0;

        protected AutoResetEvent responseReceived;
        protected IpcFlatmapServer server;
        protected int socketId;
        protected bool response;
        protected string newCallId;
        protected LogWriter log;
        public ServerIpcInterface(ushort listenPort)
        {
            log = new LogWriter(TraceLevel.Verbose, interfaceTaskname);
            server = new IpcFlatmapServer(interfaceTaskname + ':' + "IpcServer", listenPort, false, TraceLevel.Verbose);
            responseReceived = new AutoResetEvent(false);
            server.OnCloseConnection += new CloseConnectionDelegate(OnCloseConnection);
            server.OnMessageReceived += new IpcFlatmapServer.OnMessageReceivedDelegate(OnMessageReceived);
            server.OnNewConnection += new NewConnectionDelegate(OnNewConnection);
        }

        public void Start()
        {
            server.Start();
        }
     
        public void Stop()
        {
            server.Stop();
        }

        #region CallControlBase Helpers Implementation

        public bool SendAcceptCall(long callId)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);   // Trans Id
            message.Add(MessageFields.CallId, callId);                 // Data

            server.Write(this.socketId, MessageTypes.AcceptCallPush, message);

            if(!responseReceived.WaitOne(responseTimeout, false))
            {
                log.Write(TraceLevel.Error, "Response timeout from test controller for AcceptCall");
                return false;
            }

            return response;
        }

        public bool SendAnswerCall(long callId)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);    // Trans Id
            message.Add(MessageFields.CallId, callId);                 // Data

            server.Write(this.socketId, MessageTypes.AnswerCallPush, message);

            if(!responseReceived.WaitOne(responseTimeout, false))
            {
                log.Write(TraceLevel.Error, "Response timeout from test controller for AnswerCall");
                return false;
            }

            return response;
        }
        
        public bool SendHangup(long callId)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);          // Trans Id
            message.Add(MessageFields.CallId, callId);                 // Data

            server.Write(this.socketId, MessageTypes.HangupCallPush, message);

            if(!responseReceived.WaitOne(responseTimeout, false))
            {
                log.Write(TraceLevel.Error, "Response timeout from test controller for Hangup");
                return false;
            }

            return response;
        }

        public bool SendMakeCall(long callId, string to, string from, string displayName, MediaCapsField mediaCaps)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);          // Trans Id
            if(to != null)              message.Add(MessageFields.To, to);                     // Data
            if(from != null)            message.Add(MessageFields.From, from);                   // Data
            if(displayName != null)      message.Add(MessageFields.DisplayName, displayName);
            //message.Add(MessageFields.MediaCaps, mediaCaps); // TODO media cap converter

            server.Write(this.socketId, MessageTypes.MakeCallPush, message);
            
            if(!responseReceived.WaitOne(responseTimeout, false))
            {
                log.Write(TraceLevel.Error, "Response timeout from test controller for MakeCall");
                return false;
            }
 
            return response;
        }

        public bool SendRejectCall(long callId)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);          // Trans Id
            message.Add(MessageFields.CallId, callId);                 // Data

            server.Write(this.socketId, MessageTypes.RejectCallPush, message);

            if(!responseReceived.WaitOne(responseTimeout, false))
            {
                log.Write(TraceLevel.Error, "Response timeout from test controller for RejectCall");
                return false;
            }

            return response;
        }

        public bool SendSetMedia(long callId, string rxIp, uint rxPort, string rxControlIp, uint rxControlPort, 
            IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId,  transactionId);
            message.Add(MessageFields.CallId,  callId);                 
            if(rxIp != null)        message.Add(MessageFields.RxIp,  rxIp);
             message.Add(MessageFields.RxPort,  rxPort);
            if(rxControlIp != null) message.Add(MessageFields.RxControlIp,  rxControlIp);
            message.Add(MessageFields.RxControlPort,  rxControlPort);
            message.Add(MessageFields.RxCodec,  rxCodec);
            message.Add(MessageFields.RxFramesize,  rxFramesize);
            message.Add(MessageFields.TxCodec,  txCodec);
            message.Add(MessageFields.TxFramesize, txFramesize);

            server.Write(this.socketId, MessageTypes.SetMediaPush, message);

            if(!responseReceived.WaitOne(responseTimeout, false))
            {
                log.Write(TraceLevel.Error, "Response timeout from test controller for SetMedia");
                return false;
            }

            return response;
        }


        #endregion

        #region IpcServer Event Hookups

        private void OnMessageReceived(int socketId, string sourceAddr, int messageType, FlatmapList message)
        {
            if(messageType == MessageTypes.CompoundRequest)
            {
                int transactionId       = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                int numMessages     = Convert.ToInt32(message.Find(MessageFields.NumMessages, 1).dataValue);
                for(int i = 0; i < numMessages * 2; i = i + 2)
                {
                    Flatmap.MapEntry flatmap = message.Find((uint)(20000 + i), 1);
                    Flatmap.MapEntry innerMessageType = message.Find((uint)(20000 + i + 1), 1);
                    FlatmapList embeddedFlatmapList = new FlatmapList((byte[])flatmap.dataValue);
                    // I couldn't use HeaderExtension bytes to determine embedded message type
                    ProcessMessage(int.Parse(innerMessageType.dataValue.ToString()), embeddedFlatmapList);
                }
            }
            else
            {
                ProcessMessage(messageType, message);
            }
        }

        private void ProcessMessage(int messageType, FlatmapList message)
        {
            int transactionId   = 0;
            long callId         = 0;
            string txIp         = null;
            string txControlIp  = null;
            string to           = null;
            string from         = null;
            uint txPort         = 0;
            uint txControlPort  = 0;
            int responseAsInt   = 0;

            switch(messageType)
            {
                #region Requests

                case MessageTypes.CallEstablishedRequest:

                    transactionId       = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId              = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    to                  = message.Contains(MessageFields.To) ? Convert.ToString(message.Find(MessageFields.To, 1).dataValue) : null;      
                    from                = message.Contains(MessageFields.From) ? Convert.ToString(message.Find(MessageFields.From, 1).dataValue) : null;

                    if(CallEstablishedRequest != null)
                    {
                        CallEstablishedRequest(transactionId, callId, to, from);
                    }

                    break;

                case MessageTypes.CallSetupFailedRequest:

                    transactionId       = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId              = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    string reason       = message.Contains(MessageFields.Reason) ? Convert.ToString(message.Find(MessageFields.Reason, 1).dataValue) : null;

                    if(CallSetupFailedRequest != null)
                    {
                        CallSetupFailedRequest(transactionId, callId, reason);
                    }

                    break;

                case MessageTypes.GotCapabilitiesRequest:

                    transactionId       = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId              = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    txIp                = message.Contains(MessageFields.TxIp) ? Convert.ToString(message.Find(MessageFields.TxIp, 1).dataValue) : null;
                    txPort              = Convert.ToUInt32(message.Find(MessageFields.TxPort, 1).dataValue);
                    txControlIp         = message.Contains(MessageFields.TxControlIp) ? Convert.ToString(message.Find(MessageFields.TxControlIp, 1).dataValue) : null;
                    txControlPort       = Convert.ToUInt32(message.Find(MessageFields.TxControlPort, 1).dataValue);
                    
                    MediaCapsField caps = null;

                    byte[] codecsBytes = message.Find(MessageFields.MediaCaps, 1).dataValue as byte[];
                    if(codecsBytes != null)
                    {
                        caps = new MediaCapsField();
                        FlatmapList codecs  = new FlatmapList(codecsBytes);
                    
                        for(int i = 0; i < codecs.Count; i++)
                        {
                            byte[] codecBytes = codecs.GetAt(i).dataValue as byte[];
                            if(codecBytes == null) { continue; }

                            FlatmapList codec = new FlatmapList(codecBytes);

                            IMediaControl.Codecs codecName = IMediaControl.Codecs.Unspecified;
                            ArrayList framesizes = new ArrayList();
                            for(int j = 0; j < codec.Count; j++)
                            {
                                Flatmap.MapEntry entry = codec.GetAt(j);
                                if(entry.key == MessageFields.CodecName)
                                {
                                    try 
                                    { 
                                        codecName = (IMediaControl.Codecs) Convert.ToUInt32(entry.dataValue);
                                    }
                                    catch 
                                    {
                                        log.Write(TraceLevel.Error, "Invalid codec value: " + entry.dataValue);
                                        continue;
                                    }
                                }
                                else
                                {
                                    framesizes.Add(Convert.ToUInt32(entry.dataValue));
                                }
                            }

                            if(codecName != IMediaControl.Codecs.Unspecified)
                            {
                                caps.Add(codecName, (uint[]) framesizes.ToArray(typeof(uint)));
                            }
                        }
                    }

                    if(GotCapabilitiesRequest != null)
                    {
                        GotCapabilitiesRequest(transactionId, callId, txIp, txPort, txControlIp, txControlPort, caps);
                    }

                    break;

                case MessageTypes.GotDigitsRequest:

                    transactionId           = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId                  = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    string digits           = message.Contains(MessageFields.Digits) ? Convert.ToString(message.Find(MessageFields.Digits, 1).dataValue) : null;

                    if(GotDigitsRequest != null)
                    {
                        GotDigitsRequest(transactionId, callId, digits);
                    }

                    break;

                case MessageTypes.HangupRequest:
    
                    transactionId           = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId                  = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    
                    if(HangupRequest != null)
                    {
                        HangupRequest(transactionId, callId);
                    }
    
                    break;

                case MessageTypes.IncomingCallRequest:

                    transactionId           = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId                  = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    to                      = message.Contains(MessageFields.To) ? Convert.ToString(message.Find(MessageFields.To, 1).dataValue) : null;
                    from                    = message.Contains(MessageFields.From) ? Convert.ToString(message.Find(MessageFields.From, 1).dataValue) : null;
                    string originalTo       = message.Contains(MessageFields.OriginalTo) ? Convert.ToString(message.Find(MessageFields.OriginalTo, 1).dataValue) : null;
                    bool loadTest           = Convert.ToBoolean(message.Find(MessageFields.LoadTest, 1).dataValue);
                    bool negCaps            = Convert.ToBoolean(message.Find(MessageFields.NegotiateCaps, 1).dataValue);

                    if(IncomingCallRequest != null)
                    {
                        IncomingCallRequest(transactionId, callId, to, from, originalTo, loadTest, negCaps);
                    }

                    break;

                case MessageTypes.MediaEstablishedRequest:

                    transactionId           = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId                  = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    txIp                    = message.Contains(MessageFields.TxIp) ? Convert.ToString(message.Find(MessageFields.TxIp, 1).dataValue) : null;
                    txPort                  = Convert.ToUInt32(message.Find(MessageFields.TxPort, 1).dataValue);
                    txControlIp             = message.Contains(MessageFields.TxControlIp) ? Convert.ToString(message.Find(MessageFields.TxControlIp, 1).dataValue) : null;
                    txControlPort           = Convert.ToUInt32(message.Find(MessageFields.TxControlPort, 1).dataValue);
                    uint rxCodec            = Convert.ToUInt32(message.Find(MessageFields.RxCodec, 1).dataValue);
                    uint rxFramesize        = Convert.ToUInt32(message.Find(MessageFields.RxFramesize, 1).dataValue);
                    uint txCodec            = Convert.ToUInt32(message.Find(MessageFields.TxCodec, 1).dataValue);
                    uint txFramesize        = Convert.ToUInt32(message.Find(MessageFields.TxFramesize, 1).dataValue);

                    if(MediaEstablishedRequest != null)
                    {
                        MediaEstablishedRequest(transactionId, callId, txIp, txPort, txControlIp, 
                            txControlPort, rxCodec, rxFramesize, txCodec, txFramesize);
                    }

                    break;

                case MessageTypes.MediaChangedRequest:

                    transactionId           = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId                  = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    txIp                    = message.Contains(MessageFields.TxIp) ? Convert.ToString(message.Find(MessageFields.TxIp, 1).dataValue) : null;
                    txPort                  = Convert.ToUInt32(message.Find(MessageFields.TxPort, 1).dataValue);
                    
                    if(MediaChangedRequest != null)
                    {
                        MediaChangedRequest(transactionId, callId, txIp, txPort);
                    }

                    break;
                    #endregion

                    #region Responses

                case MessageTypes.AcceptCallPush:
                case MessageTypes.AnswerCallPush:
                case MessageTypes.HangupCallPush:
                case MessageTypes.MakeCallPush:

                    responseAsInt = Convert.ToInt32(message.Find(MessageFields.Response, 1).dataValue);
                    response = responseAsInt == 0 ? false : true;
                    responseReceived.Set();

                    newCallId = Convert.ToString(message.Find(MessageFields.CallId, 1).dataValue);

                    break;

                case MessageTypes.RejectCallPush:
                case MessageTypes.SetMediaPush:
                
                    responseAsInt = Convert.ToInt32(message.Find(MessageFields.Response, 1).dataValue);
                    response = responseAsInt == 0 ? false : true;
                    responseReceived.Set();

                    break;
                    #endregion

                    #region Default

                default:
                    // This must be a response from a previous transaction
                    break;

                    #endregion
            }
        }

        private void OnNewConnection(int socketId, string remoteHost)
        {
            log.Write(TraceLevel.Info, "Test controller connected from {0}", remoteHost);
            this.socketId = socketId;
        }

        private void OnCloseConnection(int socketId)
        {
            log.Write(TraceLevel.Info, "Test controller disconnected");      
        }

        #endregion

        protected static int GetNewTransactionId()
        {
            return ++transId;
        }
    }
}
