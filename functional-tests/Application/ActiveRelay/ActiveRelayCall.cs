using System;
using System.Text;
using System.Collections;
using System.Diagnostics;
using Metreos.Utilities;

namespace Metreos.FunctionalTests.App.ActiveRelay
{
    public delegate void CallCompletion(ActiveRelayCall call, CallCompletion callCompleteDelegate, CallCompleteReason reason);
    public delegate void AnswerCall(string id, string callId);
    public delegate void ConfirmCall(string id, string callId);

	public class ActiveRelayCall
	{
        public event CallCompletion CallCompleted;
        public event AnswerCall AnswerCallRequest;
        public event ConfirmCall ConfirmCallRequest;

        public int minimumAnswerCallTime;
        public int maximumAnswerCallTime;
        public int minimumConfTime;
        public int maximumConfTime;
        private const long deadCallTimeout = 120000;
        private long outboundCallPlacedStart;
        private long outboundCallCompleteDuration;

        public long ddtc;
        public ActiveRelayUser user;
        private ArrayList findMeDuration;
        private ArrayList userTimers;
        private volatile bool outboundCallCompleted;
        private volatile bool outboundCallSuccess;
        private volatile bool findMeCallCompleted;
        private TimerHandle deadCallTimer;
        private static Random rand = new Random(DateTime.Now.Millisecond);
        public long id;
        private volatile bool timeout;
        private object callEndingLock;
        private object answerCallTimerLock;
        private Hashtable timers;

        public ActiveRelayCall(long id, ActiveRelayUser user, int minimumAnswerCallTime,
            int maximumAnswerCallTime, int minimumConfTime, int maximumConfTime)
		{
            this.minimumAnswerCallTime = 1;
            this.maximumAnswerCallTime = 1;
            this.minimumConfTime = 1;
            this.maximumConfTime = 1;
            this.ddtc = 0;
            this.id = id;
			this.user = user;
            this.findMeDuration = new ArrayList();
            this.userTimers = new ArrayList();
            this.outboundCallCompleted = false;
            this.findMeCallCompleted = false;
            this.timeout = true;
            this.callEndingLock = new object();
            this.answerCallTimerLock = new object();
            this.timers = new Hashtable();
        }

        public void Start()
        {
            outboundCallPlacedStart = HPTimer.Now();
            timeout = false;
            deadCallTimer = TimerManager.StaticAdd(deadCallTimeout, new WakeupDelegate(CallTimeout));
        }

        public string Report()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("INBOUND AR CALL ");
            builder.Append(outboundCallCompleteDuration);
            builder.Append("ms ");

            return builder.ToString();
        }

        protected long CallTimeout(TimerHandle timer, object state)
        {
            timeout = true;
            lock(callEndingLock)
            {
                if(!IsSequenceCompleted())
                {
                    OnCallCompleted(CallCompleteReason.Timeout);
                }
            }
            
            return 0; // Cancel reoccurrence               
        }

        public bool OutboundCallCompleted(bool success)
        {
            outboundCallCompleteDuration = HPTimer.MillisSince(outboundCallPlacedStart);
            outboundCallCompleted = true;
            outboundCallSuccess = success;

            return AttemptFinalize();
        }

        public void FindMeCallReceived(string to, string routingId, string callId)
        {
            lock(callEndingLock)
            {
                if(!IsSequenceCompleted())
                {
                    bool foundFindMe = false;
                    for(int i = 0; i < user.findMe.Count; i++)
                    {
                        FindMeNumber findMe =  user.findMe[i] as FindMeNumber;
                        if(findMe.number == to)
                        {
                            // Found findme, take stats
                            findMeDuration.Add( new object[] 
                               { to, HPTimer.MillisSince(outboundCallPlacedStart) - findMe.delayTime * 1000 });
        
                            foundFindMe = true;
                            // Call is coming in... its up to us to decide when we want to actually answer it!
                            
                            int answerFireTime = rand.Next(minimumAnswerCallTime, maximumAnswerCallTime) * 1000;
                            timers[to] = new object[] { answerFireTime, 0 };
                            TimerHandle timer = TimerManager.StaticAdd(
                                (long) answerFireTime, new WakeupDelegate(AnswerCall),
                                new object[] { findMe, routingId, callId });
                            userTimers.Add(timer);
                            
                            break;
                        }
                    }

                    if(!foundFindMe)
                    {
                        Debug.Assert(false, "No find me number matching user.");
                    }
                }
            }

        }

        /// <summary>
        ///     Invoked whenever a FindMe Call is answered, although not necessarily confirmed
        /// </summary>
        public bool FindMeCallCompleted(bool success, string to)
        {
            bool finalized = false;

            lock(callEndingLock)
            {
                if(!IsSequenceCompleted())
                {
                    if(success)
                    {
                        bool foundFindMe = false;
                        for(int i = 0; i < user.findMe.Count; i++)
                        {
                            FindMeNumber findMe =  user.findMe[i] as FindMeNumber;
                            if(findMe.number == to)
                            {
                                // Add up test-created delays MSC
                                int testDelays = 0;
                                if(timers.Contains(to))
                                {
                                    object[] timerData = timers[to] as object[];
                                    testDelays += (int) timerData[0];
//                                    testDelays += (int) timerData[1]; // Doh, I forget, I can't add this here
                                                                        // because this method is called when the call
                                                                        // is answered; not necessarily confirmed
                                }

                                // Found findme, take stats
                                ddtc = HPTimer.MillisSince(outboundCallPlacedStart) - (findMe.delayTime * 1000) - testDelays;
        
                                foundFindMe = true;
                                findMeCallCompleted = true;
                                finalized = AttemptFinalize();
                            }

                        }

                        if(!foundFindMe)
                        {
                            Debug.Assert(false, "No find me number matching user.");
                        }
                    }
                }
                else
                {
                    finalized = true;
                }
            }

            return finalized;
        }

        /// <summary> 
        ///     Attempt Finalize is called after you've set a significant flag 
        ///     relating to the state of the call, like timeout, 
        ///     outboundCallCompleted, findMeCallCompleted 
        /// </summary>
        private bool AttemptFinalize()
        {
            bool finalized = false;
            lock(callEndingLock)
            {
                if(IsSequenceCompleted())
                {
                    if(deadCallTimer != null)
                    {
                        deadCallTimer.Cancel();
                    }

                    OnCallCompleted(CallCompleteReason.Normal);

                    finalized = true;
                }
            }

            return finalized;
        }   

        private bool IsSequenceCompleted()
        {
            return timeout || (outboundCallCompleted && findMeCallCompleted);
        }
        
        protected void OnCallCompleted(CallCompleteReason reason)
        {
            if(CallCompleted != null)
            {
                Delegate[] delegateList = CallCompleted.GetInvocationList();
                for(int i = 0; i < delegateList.Length; i++)
                {
                    CallCompletion callComplete = delegateList[i] as CallCompletion;
                    callComplete(this, callComplete, reason);
                    delegateList = CallCompleted.GetInvocationList();
                }
            }
        }

        protected long AnswerCall(TimerHandle timer, object state)
        {
            lock(callEndingLock)
            {
                if(!IsSequenceCompleted())
                {
                    userTimers.Remove(timer);

                    object[] stateStuff = state as Object[];

                    FindMeNumber findMe = stateStuff[0] as FindMeNumber;
                    string routingId = stateStuff[1] as string;
                    string callId = stateStuff[2] as String;

                    OnAnswerCallRequest(routingId, callId, !findMe.confRequired);
            
                    if(findMe.confRequired)
                    {
                        int confFireTime = rand.Next(minimumConfTime, maximumConfTime) * 1000;

                        object[] timerData = timers[findMe.number] as object[];
                        timerData[1] = confFireTime;

                        TimerHandle newTimer = TimerManager.StaticAdd(
                            (long) confFireTime, new WakeupDelegate(ConfirmCall), state);
                        userTimers.Add(newTimer);
                    }
                }
            }

            return 0;
        }

        protected long ConfirmCall(TimerHandle timer, object state)
        {
            lock(callEndingLock)
            {
                if(!IsSequenceCompleted())
                {
                    // Purposely only send 1 confirmation.  This doesn't have to be this way, it just is.
                    // It really should not do this, to test race conditions with multilpe Find Me's
                    // pressing confirmation
                    userTimers.Remove(timer);
         
                    object[] stateStuff = state as Object[];
                    FindMeNumber findMe = stateStuff[0] as FindMeNumber;
                    string routingId = stateStuff[1] as string;
                    string callId = stateStuff[2] as String;

                    OnConfirmCallRequest(routingId, callId);
                }
            }
            return 0;
        }

        /// <summary> Always called from within lock(callEndingLock) </summary>
        protected void OnAnswerCallRequest(string id, string callId, bool isFinal)
        {
            if(AnswerCallRequest != null)
            { 
                AnswerCallRequest(id, callId);

                if(isFinal)
                {
                    // The call should now be completed, so expire all Find Me confirmation timers
                    for(int i = 0; i < userTimers.Count; i++)
                    {
                        TimerHandle timer = userTimers[i] as TimerHandle;
                        timer.Cancel();
                    }
                }
            }
        }

        /// <summary> Always called from within lock(callEndingLock) </summary>
        protected void OnConfirmCallRequest(string id, string callId)
        {
            if(ConfirmCallRequest != null)
            {
                ConfirmCallRequest(id, callId);

                // The call should now be completed, so expire all Find Me confirmation timers
                for(int i = 0; i < userTimers.Count; i++)
                {
                    TimerHandle timer = userTimers[i] as TimerHandle;
                    timer.Cancel();
                }
            }
        }
	}

    public enum ARCallState
    {
        None,
        CallInitiating,
        Bridged,
        HangingUp,
        Done
    }
}
