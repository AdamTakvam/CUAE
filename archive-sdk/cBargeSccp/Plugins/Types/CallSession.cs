using System;
using System.Collections;

namespace Metreos.Applications.cBarge
{
	/// <summary>
	/// REWRITE:
	/// Contains information regarding a particular call session. By call session I mean the set of events and parameters taking place
	/// between the Call Master initiating and ending a unique call on a particular line of a particular device. Ie, if the Call Master places
	/// a call on hold on one device, then resumes it on another, the first session would end and the second would begin at the moment when
	/// the call master resumes the call on the first device.
	/// </summary>
	public class CallSession
	{
        #region Enums
        public enum States
        {
            OffHook = 1,
            OnHook = 2,
            RingOut = 3,
            RingIn = 4,
            Connected = 5,
            Busy = 6,
            Congestion = 7,
            Hold = 8,
            CallWaiting = 9,
            CallTransfer = 10,
            CallPark = 11,
            Proceed = 12,
            CallRemoteMultiline = 13,
            InvalidNumber = 14
        }
        #endregion

        #region Properties
        /// <summary>
        /// A unique identifier that identifies the particular call
        /// </summary>
        public int CallReference
        {
            get { return callReference; }
            // needed?
            set { callReference = value; }
        }
        private int callReference;

        public Connection Connection
        {
            get { return connection; }
            set { connection = value; }
        }
        private Connection connection;

        /// <summary>
        /// 'true' if MMS connections have already been established, false otherwise
        /// </summary>
        public bool MediaConnectionsCreated
        {
            get { return mediaConnectionsCreated; }
            set { mediaConnectionsCreated = value; }
        }
        private bool mediaConnectionsCreated;

        /// <summary>
        /// 'true' if a MediaFailure has occured for this call session.
        /// </summary>
        public bool MediaFailureOccured
        {
            get { return mediaFailureOccured; }
            set { mediaFailureOccured = value; }
        }
        private bool mediaFailureOccured;

        /// <summary>
        /// The directory number with which this  session is currently associated. 
        /// </summary>
        public string DirectoryNumber
        {
            get { return directoryNumber; }
            set { directoryNumber = value; }
        }
        private string directoryNumber;

        /// <summary>
        /// Inbound, Outbound, or Forward
        /// </summary>
        public string CallType
        {
            get { return callType; }
            set { callType = value; }
        }
        private string callType;
        
        public string LastRedirectingReason
        {
            get { return lastRedirectingReason; }
            set { lastRedirectingReason = value; }
        }
        private string lastRedirectingReason;

        public string LastRedirectingParty
        {
            get { return lastRedirectingParty; }
            set { lastRedirectingParty = value; }
        }
        private string lastRedirectingParty;

        public string CallingParty
        {
            get { return callingParty; }
            set { callingParty = value; }
        }
        private string callingParty;

        public string CalledParty
        {
            get { return calledParty; }
            set { calledParty = value; }
        }
        private string calledParty;

        public string OriginalCalledParty
        {
            get { return originalCalledParty; }
            set { originalCalledParty = value; }
        }
        private string originalCalledParty;

        /// <summary>
        /// The line instance with which this session is currently associated.
        /// </summary>
        public int LineInstance
        {
            get { return lineInstance; }
            set { lineInstance = value; }
        }
        private int lineInstance;

        /// <summary>
        /// Call instance that identifies the current call across shared lines.
        /// </summary>
        public int CallInstance
        {
            get { return callInstance; }
            set { callInstance = value; }
        }
        private int callInstance;

        /// <summary>
        /// Indicates whether a call record for this session needs to be written
        /// </summary>
        public bool WriteDbRecord
        {
            get { return writeDbRecord; }
            set { writeDbRecord = value; }
        }
        private bool writeDbRecord;
        
        /// <summary>
        /// Indicates whether a barge record needs to be written
        /// </summary>
        public bool WriteBargeRecord
        {
            get { return writeBargeRecord; }
            set { writeBargeRecord = value; }
        }
        private bool writeBargeRecord;
        
        public bool UpdateDbRecord
        {
            set { updateDbRecord = value; }
            get { return updateDbRecord; }
        }
        private bool updateDbRecord;

        public bool UpdateBargeRecord
        {
            set { updateBargeRecord = value; }
            get { return updateBargeRecord; }
        }
        private bool updateBargeRecord;

        public States CurrentState
        {
            get { return currentState; }
            set 
            {
                States tempState = currentState;
                
                try
                {
                    currentState = value;
                }
                catch (Exception e)
                {
                    currentState = tempState;
                    throw e;
                }
            }
            /* String-based implementation
            get 
            {
                return Enum.GetName(typeof(States), currentState);
            }
            set
            {
                States tempState = currentState;
                try
                {
                    currentState = (States) Enum.Parse(typeof(States), value, true);
                }
                catch(Exception e)
                {
                    currentState = tempState;
                    throw e;
                }
            }
            */
        }
        private States currentState;

        #endregion

        public CallSession()
		{
            directoryNumber = "-1";
            callReference = -1;
            mediaConnectionsCreated = mediaFailureOccured = false;
            writeBargeRecord = writeDbRecord = true;
            currentState = States.OnHook;
		}

        public CallSession(int callRef) : this()
        {
            callReference = callRef;
        }
    }
}
