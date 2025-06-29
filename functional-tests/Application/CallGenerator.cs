using System;
using System.Collections;
using System.Threading;
using Metreos.Utilities;
using Metreos.FunctionalTests.App.ActiveRelay;

namespace Metreos.FunctionalTests.App
{
	/// <summary> Generates call patterns </summary>
	public class CallGenerator
	{
        public delegate void CallRequest(long id, string from, string to, ActiveRelayUser user); 
        public delegate void EndCallRequest(long id);

        public event CallRequest CallRequested;
        public event EndCallRequest EndCallRequested;

        public Thread thread;
        public volatile bool started;
        
        private string[] callers;
        private string[] receivers;
        private ArrayList users;
        private int bhca;
        private int talkTimeMs;
        private int testTimeS;
        private long testStartTime;
        private long id;
        private Hashtable outstandingCalls;
        private int resolution;
        private int timeBetweenCallsMs;
        private long lastCallInterval;
        private AutoResetEvent are;

        public CallGenerator()
		{
            this.are = new AutoResetEvent(false);
            this.started = false;            		
            this.resolution = 30;
            this.outstandingCalls = new Hashtable();    
        }

        public bool Stop(int timeout)
        {
            started = false;
            bool stopped = true;
            if(thread != null)
            {
                stopped = thread.Join(timeout);
            }
            return stopped;
        }

        public void BHCAGen(string[] callers, string[] receivers, int bhca, int talkTimeMs, int testTimeS)
        {
            if(started) return;
            started = true;

            this.timeBetweenCallsMs = (3600  * 1000) / bhca;
            this.id = 0;
            this.outstandingCalls.Clear();
            this.callers = callers;
            this.receivers = receivers;
            this.bhca = bhca;
            this.talkTimeMs = talkTimeMs;
            this.testTimeS = testTimeS;

            lastCallInterval = testStartTime = HPTimer.Now();

            thread = new Thread(new ThreadStart(Run));
            thread.IsBackground = true;
            thread.Start();
        }

        public void BHCAGen(ArrayList arUsers, int bhca, int talkTimeMs, int testTimeS, string mapPrepend)
        {
            if(started) return;
            started = true;

            this.users = arUsers;
            this.timeBetweenCallsMs = (3600  * 1000) / bhca;
            this.id = 0;
            this.outstandingCalls.Clear();
            this.bhca = bhca;
            this.talkTimeMs = talkTimeMs;
            this.testTimeS = testTimeS;

            ArrayList receiversList = new ArrayList();
            ArrayList callersList = new ArrayList();

            // Populate callers/receivers
            foreach(ActiveRelayUser user in arUsers)
            {
                receiversList.Add(user.extension);
                callersList.Add(mapPrepend + user.extension); // MapPrepend is a prepend that allows you to map
                                                              // back to the called account using 'From'
            }

            receivers = new string[receiversList.Count];
            receiversList.CopyTo(receivers);
            callers = new string[callersList.Count];
            callersList.CopyTo(callers);

            lastCallInterval = testStartTime = HPTimer.Now();

            thread = new Thread(new ThreadStart(Run));
            thread.IsBackground = true;
            thread.Start();
        }

        public void CallStarted(long id)
        {
            lock(outstandingCalls.SyncRoot)
            {
                outstandingCalls[id] = HPTimer.Now();
            }
        }

        public void WaitForTestEnd()
        {
            are.WaitOne();
        }

        private void Run()
        {
            int index = 0;

            while(true)
            {
                CheckForEndTest();

                if(started && TimeForNewCall())
                {
                    id++;
                    index++;

                    string caller = callers[index % callers.Length];
                    string receiver = receivers[index % receivers.Length];    
                    ActiveRelayUser user = users[index % users.Count] as ActiveRelayUser;

                    if(CallRequested != null)
                    {
                        CallRequested(id, caller, receiver, user);
                    }
                }

                if(CleanUpCalls() && !started)
                {
                    // We've cleaned up all calls and the test is stopped
                    break;
                }

                Thread.Sleep(resolution);
            }

            are.Set();
        }

        private void CheckForEndTest()
        {
            if(started)
            {
                // Check testOver
                long testLength = HPTimer.MillisSince(testStartTime);
                started = testTimeS * 1000 > testLength; // to mills
            }
        }        

        private bool TimeForNewCall()
        {
            long timeSinceLastCall = HPTimer.MillisSince(lastCallInterval);
            if(timeSinceLastCall > timeBetweenCallsMs)
            {
                // Time for a new call! Oh joy.
                lastCallInterval= HPTimer.Now();
                return true;
            }

            return false;
        }

        private bool CleanUpCalls()
        {
            bool noCalls = true;
            lock(outstandingCalls.SyncRoot)
            {
                if(outstandingCalls.Count > 0)
                {
                    noCalls = false;
                    IDictionaryEnumerator dictEnum = outstandingCalls.GetEnumerator();

                    ArrayList toRemove = new ArrayList();

                    while(dictEnum.MoveNext())
                    {
                        long callId = (long)dictEnum.Key;
                        long callStartTime = (long)dictEnum.Value;
 
                        long timeElapsed = HPTimer.MillisSince(callStartTime);
                        if(timeElapsed > talkTimeMs)
                        {
                            toRemove.Add(callId);
                        }
                    }

                    foreach(object callId in toRemove)
                    {
                        if(EndCallRequested != null)
                        {
                            EndCallRequested((long)callId);
                        }
                        outstandingCalls.Remove(callId);
                    }
                }
            
                return noCalls;
            }
        }
	}
}
