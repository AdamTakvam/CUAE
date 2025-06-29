using System;
using System.Collections;

namespace Metreos.AppServer.TelephonyManager
{
    #region Enumerations

    internal enum Actions
    {
        ForwardEventToApp,      // Forwarding
        ForwardResponseToApp,
        ForwardActionToProvider,
        SendActionFailureToApp,
        SendMakeCallCompleteToApp,
        SendMakeCallFailedToApp,
        SendBridgeSuccessToApp,
        SendUnbridgeSuccessToApp,
        SendHangupToApp,        // Event creation
        SendStartTxToApp,
        SendStartRxToApp,
        SendStopTxToApp,
        HangupCall,             // Action creation
        RejectCall,
        AcceptCall,
        HoldCall,
        ResumeCall,
        GetMediaCaps,           // Media commands
        ReserveConnection,
        CreateConnection,
        ModifyConnection,
        DeleteConnection,
        CreateConference,
        JoinConference,
        StopMediaOperation,
        SetMedia,
        ClearMediaInfo,
        SyncPeerMedia,          // Peer-To-Peer commands
        ClearPeerMediaInfo,
        ForwardResponseToPeer,
        SendActionFailureToPeer,
        SendActionSuccessToPeer,
        AcceptPeerCall,
        AnswerPeerCall,
        HoldPeerCall,
        ResumePeerCall,
        JoinPeerConference,
        CreatePeerConference,
        DeletePeerConnection,
        SetPeerMedia,
        UseMohMedia,
        HangupPeerCall,
        SelectTxCodec,          // Script engine instructions
        AssumePreferredTxCodec,
        Wait,
        EndScript,
        EndCall
    }

    internal enum DataFields
    {
        callId,
        peerCallId,
        mmsId,
        connectionId,
        conference,
        conferenceId,
        localMediaCaps,
        remoteMediaCaps,
        txAddr,
        txCodec,
        txFramesize,
        rxAddr,
        rxCodec,
        rxFramesize,
        waitForMedia,
		negCaps,
        earlyMedia
    }

    internal enum Comparators
    {
        Equal,
        NotEqual
    }

    internal enum WaitMedia
    {
        None = 0,
        Tx,
        Rx,
        TxRx
    }

    #endregion

	internal sealed class StateDescription
	{
        public Actions action;

        public string defaultNextState;
        public Hashtable eventNextStates;       // Event name -> stateId
        public Hashtable actionNextStates;      // Action name -> stateId
        public Hashtable responseNextStates;    // Response -> stateId
        public Hashtable dataNextStates;        // DataCriteria -> stateId

        private TimeSpan timeout;
        public TimeSpan Timeout { get { return timeout; } }

        private string timeoutNextState;
        public string TimeoutNextState { get { return timeoutNextState; }  }

		public StateDescription() {}

        public bool SetTimeout(uint seconds, string nextState)
        {
            if((seconds == 0) || (nextState == null)) { return false; }

            timeout = new TimeSpan(0, 0, (int)seconds);
            timeoutNextState = nextState;
            return true;
        } 
	}

    internal sealed class DataCriteria
    {
        public DataFields fieldName;
        public Comparators comparator;
        
        private object _value;
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public DataCriteria() {}
    }
}
