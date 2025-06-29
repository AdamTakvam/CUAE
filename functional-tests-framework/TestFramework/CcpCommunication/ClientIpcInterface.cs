using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Net;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Core;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Core.ConfigData;
using Metreos.Messaging.MediaCaps;
using MessageTypes = Metreos.TestCallControl.Communication.ICallControlTest.MessageTypes;
using MessageFields = Metreos.TestCallControl.Communication.ICallControlTest.MessageFields;

using Metreos.Samoa.FunctionalTestFramework;

namespace Metreos.TestCallControl.Communication
{
    public delegate bool AcceptCallReceivedDelegate(long callId);
    public delegate bool AnswerCallReceivedDelegate(long callId);
    public delegate bool HangupCallReceivedDelegate(long callId);
    public delegate bool MakeCallReceivedDelegate(long callId, string to, string from);
    public delegate bool RejectCallReceivedDelegate(long callId);
    public delegate bool SetMediaReceivedDelegate(long callId, string rxIp, uint rxPort, string rxControlIp, uint rxControlPort,
                                                  IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize);

	/// <summary> 
	///     Handles communication to the Call Control Provider
    /// </summary>
	public class ClientIpcInterface
	{
        public event AcceptCallReceivedDelegate AcceptCallReceived;
        public event AnswerCallReceivedDelegate AnswerCallReceived;
        public event HangupCallReceivedDelegate HangupCallReceived;
        public event MakeCallReceivedDelegate MakeCallReceived;
        public event RejectCallReceivedDelegate RejectCallReceived;
        public event SetMediaReceivedDelegate SetMediaReceived;

        protected Settings settings;
        protected IpcFlatmapClient client;
        protected string appServerIp;

        protected volatile static int transId  = 0;

		public ClientIpcInterface(string appServerIp)
		{
            this.appServerIp = appServerIp;
	        this.client = new IpcFlatmapClient();
		}

        public void Open()
        {
            client.onFlatmapMessageReceived += new OnFlatmapMessageReceivedDelegate(OnMessageReceived);
            client.onClose += new OnCloseDelegate(OnClose);
			client.RemoteEp = new IPEndPoint(IPAddress.Parse(appServerIp), ICallControlTest.ListenPort);
            client.Open();
        }

        public void Close()
        {
            client.Close();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public void SendMultipleMessages(FlatmapList[] flatmaps, int[] messageTypes)
        {
            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.NumMessages, flatmaps.Length);
            
            for(int i = 0; i < flatmaps.Length * 2; i = i + 2)
            {
                FlatmapList flatmap = flatmaps[i/2];
                message.Add((uint)(20000 + i), flatmap.ToFlatmap());
                message.Add((uint)(20000 + i + 1), messageTypes[i/2]);
            }

            client.Write(MessageTypes.CompoundRequest, message);
        }

        public void SendCallEstablished(long callId, string to, string from)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(to != null)          message.Add(MessageFields.To, to);
            if(from != null)        message.Add(MessageFields.From, from);

            client.Write(MessageTypes.CallEstablishedRequest, message);
        }

        public void SendCallSetupFailed(long callId, string reason)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(reason != null)      message.Add(MessageFields.Reason, reason);
            
            client.Write(MessageTypes.CallEstablishedRequest, message);
        }

        public void SendGotCapabilities(long callId)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);

            client.Write(MessageTypes.GotCapabilitiesRequest, message);
        }

        public void SendGotCapabilities(long callId, string txIp, uint txPort, 
            string txControlIp, uint txControlPort, MediaCapsField caps)
        {
            client.Write(MessageTypes.GotCapabilitiesRequest, CreateGotCapabilitiesMessage(callId, txIp,
                txPort, txControlIp, txControlPort, caps));
        }

        public void SendGotCapabilities(FlatmapList message)
        {
            client.Write(MessageTypes.GotCapabilitiesRequest, message);
        }

        public void SendGotDigits(long callId, string digits)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(digits != null)      message.Add(MessageFields.Digits, digits);

            client.Write(MessageTypes.GotDigitsRequest, message);
        }

        public void SendHangup(long callId)
        { 
            client.Write(MessageTypes.HangupRequest, CreateHangupMessage(callId));
        }

        public void SendHangup(FlatmapList message)
        {
            client.Write(MessageTypes.HangupRequest, message);
        }

        public void SendIncomingCall(long callId, string to, string from, string originalTo)
        {
            SendIncomingCall(callId, to, from, originalTo, false);
        }

        public void SendIncomingCall(long callId, string to, string from, string originalTo, bool loadTest)
        {
            client.Write(MessageTypes.IncomingCallRequest, CreateIncomingCallMessage(callId, to, from, originalTo, loadTest));
        }

        public void SendIncomingCall(FlatmapList message)
        {
            client.Write(MessageTypes.IncomingCallRequest, message);
        }

        public void SendMediaEstablished(long callId, string txIp, uint txPort, string txControlIp, uint txControlPort, 
            IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            client.Write(MessageTypes.MediaEstablishedRequest, CreateMediaEstablishedMessage(callId, txIp, txPort, 
                txControlIp, txControlPort, rxCodec, rxFramesize, txCodec, txFramesize));
        }

        public void SendMediaEstablished(FlatmapList message)
        {
            client.Write(MessageTypes.MediaEstablishedRequest, message);
        }

        public void SendMediaChanged(long callId, string txIp, uint txPort)
        {
            client.Write(MessageTypes.MediaChangedRequest, CreateMediaChangedMessage(callId, txIp, txPort));
        }

        public void SendMediaChanged(FlatmapList message)
        {
            client.Write(MessageTypes.MediaChangedRequest, message);
        }

        public void SendMediaChanged(long callId, string txIp, uint txPort, string txControlIp, uint txControlPort, 
            IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(txIp != null)        message.Add(MessageFields.TxIp, txIp);
            message.Add(MessageFields.TxPort, txPort);
            if(txControlIp != null) message.Add(MessageFields.TxControlIp, txControlIp);
            message.Add(MessageFields.TxControlPort, txControlPort);
            message.Add(MessageFields.RxCodec, (uint) rxCodec);
            message.Add(MessageFields.RxFramesize, rxFramesize);
            message.Add(MessageFields.TxCodec, (uint) txCodec);
            message.Add(MessageFields.TxFramesize, txFramesize);

            client.Write(MessageTypes.MediaChangedRequest, message);
        }

        #region Create Messages

        public FlatmapList CreateIncomingCallMessage(long callId, string to, string from, string originalTo)
        {
            return CreateIncomingCallMessage(callId, to, from, originalTo, false);
        }

        public FlatmapList CreateIncomingCallMessage(long callId, string to, string from, string originalTo, bool loadTest)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(to != null)          message.Add(MessageFields.To, to);
            if(from != null)        message.Add(MessageFields.From, from);
            if(originalTo != null)  message.Add(MessageFields.OriginalTo, originalTo);
            message.Add(MessageFields.LoadTest, loadTest ? 1 : 0);

            return message;
        }

        public FlatmapList CreateMediaEstablishedMessage(long callId, string txIp, uint txPort, string txControlIp, uint txControlPort, 
            IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(txIp != null)       message.Add(MessageFields.TxIp, txIp);
            message.Add(MessageFields.TxPort, txPort);
            if(txControlIp != null)message.Add(MessageFields.TxControlIp, txControlIp);
            message.Add(MessageFields.TxControlPort, txControlPort);
            message.Add(MessageFields.RxCodec, (uint) rxCodec);
            message.Add(MessageFields.RxFramesize, rxFramesize);
            message.Add(MessageFields.TxCodec, (uint) txCodec);
            message.Add(MessageFields.TxFramesize, txFramesize);

            return message;
        }

        public FlatmapList CreateMediaChangedMessage(long callId, string txIp, uint txPort)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(txIp != null)        message.Add(MessageFields.TxIp, txIp);
            message.Add(MessageFields.TxPort, txPort);

            return message;
        }

        public FlatmapList CreateGotCapabilitiesMessage(long callId, string txIp, uint txPort, 
            string txControlIp, uint txControlPort, MediaCapsField caps)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);
            if(txIp != null)        message.Add(MessageFields.TxIp, txIp);
            message.Add(MessageFields.TxPort, txPort);
            if(txControlIp != null) message.Add(MessageFields.TxControlIp, txControlIp);
            message.Add(MessageFields.TxControlPort, txControlPort);

            if(caps != null)
            {
                IDictionaryEnumerator capDictEnum = (IDictionaryEnumerator) caps.GetEnumerator();
            
                ArrayList allCaps = new ArrayList();
                while(capDictEnum.MoveNext())
                {
                    IMediaControl.Codecs codecName = (IMediaControl.Codecs) capDictEnum.Key;
                    uint[] framesizes = caps[codecName];
    
                    FlatmapList codec = new FlatmapList();
                    codec.Add(MessageFields.CodecName, codecName);
                    if(framesizes != null)
                    {
                        foreach(uint fSize in framesizes)
                        {
                            codec.Add(MessageFields.Unspecified, fSize);
                        }
                    }

                    allCaps.Add(codec.ToFlatmap());
                }

                FlatmapList codecs = new FlatmapList();
                for(int i = 0; i < allCaps.Count; i++)
                {
                    codecs.Add(MessageFields.Unspecified, allCaps[i]);
                }
            
                message.Add(MessageFields.MediaCaps, codecs.ToFlatmap());
            }

            return message;
        }

        public FlatmapList CreateHangupMessage(long callId)
        {
            int transactionId = GetNewTransactionId();

            FlatmapList message = new FlatmapList();
            message.Add(MessageFields.TransactionId, transactionId);
            message.Add(MessageFields.CallId, callId);

            return message;
        }

        #endregion

        public void OnMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList message)
        {
            int transactionId = 0;
            long callId       = 0;
            bool response     = false;
            FlatmapList responseMessage = null; 

            switch(messageType)
            {
                case MessageTypes.AcceptCallPush:

                    transactionId   = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId          = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);

                    response = AcceptCallReceived(callId);

                    responseMessage = new FlatmapList();
                    responseMessage.Add(MessageFields.TransactionId, transactionId);
                    responseMessage.Add(MessageFields.Response, response ? 1 : 0);

                    client.Write(MessageTypes.AcceptCallPush, responseMessage);
                    
                    break;

                case MessageTypes.AnswerCallPush:

                    transactionId   = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId          = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);

                    response = AnswerCallReceived(callId);

                    responseMessage = new FlatmapList();
                    responseMessage.Add(MessageFields.TransactionId, transactionId);
                    responseMessage.Add(MessageFields.Response, response ? 1 : 0);

                    client.Write(MessageTypes.AnswerCallPush, responseMessage);

                    break;

                case MessageTypes.HangupCallPush:

                    transactionId   = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId          = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);

                    response = HangupCallReceived(callId);

                    responseMessage = new FlatmapList();
                    responseMessage.Add(MessageFields.TransactionId, transactionId);
                    responseMessage.Add(MessageFields.Response, response ? 1 : 0);

                    client.Write(MessageTypes.HangupCallPush, responseMessage);

                    break;

                case MessageTypes.MakeCallPush:

                    transactionId   = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId          = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);
                    string to       = message.Contains(MessageFields.To)   ? Convert.ToString(message.Find(MessageFields.To, 1).dataValue) : null;
                    string from     = message.Contains(MessageFields.From) ? Convert.ToString(message.Find(MessageFields.From, 1).dataValue) : null;

                    response = MakeCallReceived(callId, to, from);

                    responseMessage = new FlatmapList();
                    responseMessage.Add(MessageFields.TransactionId, transactionId);
                    responseMessage.Add(MessageFields.CallId, callId);
                    responseMessage.Add(MessageFields.Response, response ? 1 : 0);

                    client.Write(MessageTypes.AcceptCallPush, responseMessage);
                  
                    break;

                case MessageTypes.RejectCallPush:
                    
                    transactionId   = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId          = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);

                    response = RejectCallReceived(callId);

                    responseMessage = new FlatmapList();
                    responseMessage.Add(MessageFields.TransactionId, transactionId);
                    responseMessage.Add(MessageFields.Response, response ? 1 : 0);

                    client.Write(MessageTypes.RejectCallPush, responseMessage);

                    break;

                case MessageTypes.SetMediaPush:

                    string rxIp=null, rxControlIp=null;
                    uint rxPort=0, rxControlPort=0, rxFramesize=0, txFramesize=0;

                    transactionId       = Convert.ToInt32(message.Find(MessageFields.TransactionId, 1).dataValue);
                    callId              = Convert.ToInt64(message.Find(MessageFields.CallId, 1).dataValue);

                    if(message.Contains(MessageFields.RxIp, 1))
                        rxIp            = Convert.ToString(message.Find(MessageFields.RxIp, 1).dataValue);
                    if(message.Contains(MessageFields.RxPort, 1))
                        rxPort          = Convert.ToUInt32(message.Find(MessageFields.RxPort, 1).dataValue);
                    if(message.Contains(MessageFields.RxControlIp, 1))
                        rxControlIp     = Convert.ToString(message.Find(MessageFields.RxControlIp, 1).dataValue);
                    if(message.Contains(MessageFields.RxControlPort, 1))
                        rxControlPort   = Convert.ToUInt32(message.Find(MessageFields.RxControlPort, 1).dataValue);
                    if(message.Contains(MessageFields.RxFramesize, 1))
                        rxFramesize     = Convert.ToUInt32(message.Find(MessageFields.RxFramesize, 1).dataValue);
                    if(message.Contains(MessageFields.TxFramesize, 1))
                        txFramesize     = Convert.ToUInt32(message.Find(MessageFields.TxFramesize, 1).dataValue);
                    
                    IMediaControl.Codecs rxCodec = (IMediaControl.Codecs) Convert.ToUInt32(message.Find(MessageFields.RxCodec, 1).dataValue);
                    IMediaControl.Codecs txCodec = (IMediaControl.Codecs) Convert.ToUInt32(message.Find(MessageFields.TxCodec, 1).dataValue);
                    
                    response = SetMediaReceived(callId, rxIp, rxPort, rxControlIp, 
                        rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize);

                    responseMessage = new FlatmapList();
                    responseMessage.Add(MessageFields.TransactionId, transactionId);
                    responseMessage.Add(MessageFields.Response, response ? 1 : 0);

                    client.Write(MessageTypes.SetMediaPush, responseMessage);
                    
                    break;
            }
        }

        protected static int GetNewTransactionId()
        {
            return ++transId;
        }

        private void OnClose(IpcClient ipcClient, Exception e)
        {
            // nothing to do.
        }
    }
}
