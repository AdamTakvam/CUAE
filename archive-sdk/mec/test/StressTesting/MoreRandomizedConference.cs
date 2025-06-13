using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;

using Metreos.Native.ConferenceData;
using Metreos.Samoa.Core;
using Metreos.Mec.WebMessage;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// The implementation of running Randomized Conference
	/// </summary>
    public class MoreRandomizedConference : TestBase
    {

        #region VariableDeclarations

		public const int minimumInterval = 100;

        public AutoResetEvent blocker = new AutoResetEvent(true);
        public AutoResetEvent blockForDurationOfTest = new AutoResetEvent(false);
        public delegate bool StartTestDelegate();
        StartTestDelegate startTestDelegate;

        private System.Timers.Timer howLongIsTest;
		private System.Timers.Timer joinTimer;
		private System.Timers.Timer kickTimer;
		private System.Timers.Timer createConferenceTimer;
		private System.Timers.Timer destroyConferenceTimer;
        private System.Timers.Timer joinMultipleTimer;
        private System.Timers.Timer kickMultipleTimer;

        private DateTime joinKickTimerRandomTime;
        private DateTime checkStatusRandomTime;
		private DateTime timeOfEndOfTest;
                
        // values used to ensure that "random" is enforced
        public int forcedSkewAverageConferences;
        public int forcedSkewConnections;
        public int desiredConnections;
        public int desiredConferences;
        public Random rand;

        public int numRunningAverageConferencesSamples;
        public int numRunningAverageConnectionsSamples;
        private object conferenceReferenceSyncLock;
        private object joinKickTimerRandomFiredLock;
        private object createInsteadOfJoinLock;
        private object joinOrKickLock;
        private object forcedSkewConnectionsLock;
        private object createConferenceLock;
        private object joinConferenceLock;
        private object kickParticipantLock;
        private object createOrDestroyLock;
        private object blockLock;
        private bool joinOrKick;
        public bool JoinOrKick
        {
            get
            {
                    return joinOrKick;
            }
            set
            {
                    joinOrKick = value;
            }
        }

        private bool createInsteadOfJoin;
        
        public bool CreateInsteadOfJoin
        {
            get
            {
                lock(createInsteadOfJoinLock)
                {
                    return createInsteadOfJoin;
                }
            }
            set
            {
                lock(createInsteadOfJoinLock)
                {
                    createInsteadOfJoin = value;
                }
            }
        }

        private int ForcedSkewConnections
        {
            get
            {
                lock(forcedSkewConnectionsLock)
                {
                    return forcedSkewConnections;
                }
            }
            set
            {
                lock(forcedSkewConnectionsLock)
                {
                    forcedSkewConnections = value;
                }
            }
        }

        private TimeSpan durationOfTest;
        private TimeSpan baseTimeForJoinOrKick;
        public bool testIsDone;
        public bool createOrDestroy;
        public bool CreateOrDestroy
        {
            get 
            {
                lock(createOrDestroyLock)
                {
                    return createOrDestroy;
                }
            }
            set
            {
                lock(createOrDestroyLock)
                {
                    createOrDestroy = value;
                }
            }
        }
        public ProgressBar progressBar;
        public ProgressBar overallProgressBar;
        public TimeSpan periodBetweenCalls;
        public int numberConnections;
        public int numberConferences;
        public int connectionIntensity;
        public int conferenceIntensity;
        public int maximumConnections;
        public int maximumConferences;
        public int averageSpike;
        public int nextOpenConference;
        private int averagePeriodBetweenCalls;
        public TimeSpan periodBetweenHangups;
        public RichTextBox debugOutput;
        public RichTextBox errorOutput;
        public AutoResetEvent testIsDoneEvent;
        private string locationIdReturned;
        private string conferenceIdReturned;
        public ArrayList conferenceReference;
        public ArrayList locationIdList;
        public ArrayList sessionsList;
        public ArrayList locationIdListList;
        
        // Get synchronized versions of these ArrayLists
        public ArrayList conferenceReferenceSync;
        public ArrayList locationIdListSync;
        public ArrayList sessionsListSync;
        public ArrayList locationIdListListSync;

        public bool endOfTest;
        #endregion VariableDeclarations

        // Full Random with no database checking constructor
        public MoreRandomizedConference(MecStressTest mecStressTest) : base(mecStressTest)
        {
            conferenceReferenceSyncLock = new object();
            joinOrKickLock = new object();
            createInsteadOfJoinLock = new object();
            forcedSkewConnectionsLock = new object();
            joinKickTimerRandomFiredLock = new object();
            createConferenceLock = new object();
            joinConferenceLock = new object();
            kickParticipantLock = new object();
            createOrDestroyLock = new object();
            blockLock = new object();
            
            this.periodBetweenCalls = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.randomizedConference_Particular_PeriodBetweenCalls.Text), 0);
            this.periodBetweenHangups = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.randomizedConference_Particular_PeriodBetweenCalls.Text),0);
            this.durationOfTest = new TimeSpan(0,0, System.Int32.Parse(mecStressTest.randomizedConference_Particular_DurationTest.Text), 0, 0);
            this.maximumConferences = System.Int32.Parse(mecStressTest.randomizedConference_Particular_MaximumConferences.Text);
            this.maximumConnections = System.Int32.Parse(mecStressTest.randomizedConference_Particular_MaximumConnections.Text);
            this.connectionIntensity = mecStressTest.randomizedConference_Particular_ConnectionIntensity.Value;
            this.progressBar = mecStressTest.individualTestBar;
            this.overallProgressBar = mecStressTest.overallTestBar;
            this.averagePeriodBetweenCalls = System.Int32.Parse(mecStressTest.randomizedConference_Particular_AveragePeriod.Text);
            this.averageSpike = System.Int32.Parse(mecStressTest.randomizedConference_Particular_AverageSpike.Text);
            this.testIsDoneEvent = mecStressTest.testIsDone;

            this.endOfTest = false;

            this.desiredConnections = (int) ((float)maximumConnections * (float)((float) connectionIntensity / (float) 21));
            
            // Ensure that we have at least 1 connections
            if(desiredConnections < 1)
            {
                desiredConnections = 1;
            }
            this.desiredConferences = (int) ((float)maximumConferences * (float)((float) connectionIntensity / (float) 21));
            
            // Ensure that we have at least 1 conference desired
            if(desiredConferences < 1)
            {
                desiredConferences = 1;
            }
            this.forcedSkewConnections = desiredConnections;

            
            baseTimeForJoinOrKick = new TimeSpan(0,0,2,0,0);
            JoinOrKick = false;
            CreateInsteadOfJoin = false;
            numberConnections = 1;
            numberConferences = 1;

            locationIdList = new ArrayList();
            locationIdListList = new ArrayList(maximumConferences);
            sessionsList = new ArrayList(maximumConferences);
			conferenceReference = new ArrayList();

            rand = new Random((int) DateTime.Now.Ticks);

            locationIdListSync = ArrayList.Synchronized(locationIdList);
            sessionsListSync = ArrayList.Synchronized(sessionsList);
            locationIdListListSync = ArrayList.Synchronized(locationIdListList);
			conferenceReferenceSync = ArrayList.Synchronized(conferenceReference);

            
            joinKickTimerRandomTime = DateTime.MaxValue;
            checkStatusRandomTime = DateTime.MaxValue;
			timeOfEndOfTest = DateTime.MaxValue;


			// Timer initializatioon.
			// Timers relating to conference participant kicking and joining
			// Join timer
			joinTimer = new System.Timers.Timer();
			joinTimer.Elapsed += new System.Timers.ElapsedEventHandler( this.JoinTimerFired );
			// Kick timer
			kickTimer = new System.Timers.Timer();
			kickTimer.Elapsed += new System.Timers.ElapsedEventHandler( this.KickTimerFired );
			// Create conference timer
			createConferenceTimer = new System.Timers.Timer();
			createConferenceTimer.Elapsed += new System.Timers.ElapsedEventHandler( this.CreateConferenceTimerFired );
			// Destroy conference timer
			destroyConferenceTimer = new System.Timers.Timer();
			destroyConferenceTimer.Elapsed += new System.Timers.ElapsedEventHandler( this.DestroyConferenceTimerFired );
			// Spike timers
			// JoinMultiple timer
			joinMultipleTimer = new System.Timers.Timer();
			joinMultipleTimer.Elapsed += new System.Timers.ElapsedEventHandler( this.JoinMultipleTimerFired );
			// KickMultiple timer
			kickMultipleTimer = new System.Timers.Timer();
			kickMultipleTimer.Elapsed += new System.Timers.ElapsedEventHandler( this.KickMultipleTimerFired );
            // Gui timers
			// Timer to let the user see the test duration.
            this.howLongIsTest = new System.Timers.Timer(1000);
            howLongIsTest.Elapsed += new System.Timers.ElapsedEventHandler( this.UpdateFormClock );

            startTestDelegate += new StartTestDelegate( StartTestRandom );
        }


        public override bool Start()
        {
            return startTestDelegate();
        }

        public bool StartTestRandom()
        {
            if( !TestInitialization() )
            {
                blocker.Set();
            }

            mecStressTest.DebugOutputText = "\nEntering blocker";     
            mecStressTest.debugOutput.ScrollToCaret();

            Blocker();

            // Open ups the spigot at the block,
            // and allows all the queued up timer functions to die gracefully
            endOfTest = true;
            blocker.Set();
            
            TestCleanup();
            return true;
        }

		
		public void JoinTimerFired(object state, System.Timers.ElapsedEventArgs e)
		{
			if(IsTestStillExecuting(e))
			{
				UpdateTimerFireInterval(averagePeriodBetweenCalls, joinTimer);

				mecStressTest.DebugOutputText = "\n\nAttempting a join...";
				// Make sure that there is a conference to join to
                if(NumberOfLiveConferences > 0)
                {
                    // Make sure we aren't going to surpass our maximum # of connections
                    if(NumberOfLiveConnections < maximumConnections)
                    {
                        // Checkout a conference, so no other operations can mess with the list of locations
                        if(conferenceReferenceSync.Count > 0)
                        {
                            int conferenceToUse = PullOutConference(conferenceReferenceSync);

                            // Randomly choose a conference to stick someone in
                            if( ConferenceCommand.CreateJoin( sessionsListSync, locationIdListListSync, mecStressTest, conferenceToUse, ref locationIdReturned, ref conferenceIdReturned) )
                            {
                                NumberOfLiveConnections++;
                                mecStressTest.DebugOutputText = " CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                                mecStressTest.debugOutput.ScrollToCaret();
                            }
                            else
                            {
                                mecStressTest.ErrorOutputText = "\nCreate Join failed";
                                mecStressTest.debugOutput.ScrollToCaret();
                                // REFACTOR:  must exit at this point.  
                            }
                        }
                        else
                        {
                            mecStressTest.DebugOutputText = " wanted to execute a join conference, but all conferences are presently being operated on";
                            mecStressTest.debugOutput.ScrollToCaret();
                        }
                    }
                    else
                    {
                        mecStressTest.DebugOutputText = " wanted to execute a join conference, but maximum # of connections has been reached";
                        mecStressTest.debugOutput.ScrollToCaret();
                    }
                }
                else
                {
                    mecStressTest.DebugOutputText = " no available conferences to join to";
                }
			}
			
		}

		public void KickTimerFired(object state, System.Timers.ElapsedEventArgs e)
		{
			if(IsTestStillExecuting(e))
			{
				UpdateTimerFireInterval(averagePeriodBetweenCalls, kickTimer);


			}
		}
		public void CreateConferenceTimerFired(object state, System.Timers.ElapsedEventArgs e)
		{
			if(IsTestStillExecuting(e))
			{
				UpdateTimerFireInterval(averageSpike, createConferenceTimer);


			}
		}
		public void DestroyConferenceTimerFired(object state, System.Timers.ElapsedEventArgs e)
		{
			if(IsTestStillExecuting(e))
			{
				UpdateTimerFireInterval(averageSpike, destroyConferenceTimer);

			}
		}

		public void JoinMultipleTimerFired(object state, System.Timers.ElapsedEventArgs e)
		{
			if(IsTestStillExecuting(e))
			{
				UpdateTimerFireInterval(averageSpike, joinMultipleTimer);

			}
		}

		public void KickMultipleTimerFired(object state, System.Timers.ElapsedEventArgs e)
		{
			if(IsTestStillExecuting(e))
			{
				UpdateTimerFireInterval(averageSpike, kickMultipleTimer);

			}
		}
 
        
        public override bool EndTest()
        {
            blockForDurationOfTest.Set();
            endOfTest = true;
            return true;
        }

        public void Blocker()
        {   
            // All timed events will first enter this function, and hang right here, until another exits this block
            lock(blockLock)
            {
                // Open up spiggot at end of the test
                if( !endOfTest )
                {
                    blocker.WaitOne();
                }
            }
        }
       


        public bool TestInitialization()
        {
            int randomNumberOfConferences = rand.Next(1, desiredConferences + 1);

            // remember, each created conference takes up one connection
            if(randomNumberOfConferences > maximumConnections)
            {
                randomNumberOfConferences = maximumConnections;
            }
            if(randomNumberOfConferences > maximumConferences)
            {
                randomNumberOfConferences = maximumConferences;
            }
           
            // Create the random number of conferences
            for(int conferenceIteration = 0; conferenceIteration < randomNumberOfConferences; conferenceIteration++)
            {
                

                if( !endOfTest && ConferenceCommand.CreateConference(sessionsListSync, locationIdListListSync, mecStressTest, ref locationIdReturned, ref conferenceIdReturned ) == true)
                {
                    NumberOfLiveConnections++;
                    NumberOfLiveConferences++;  
                    mecStressTest.DebugOutputText = "\nCreating conference " + (conferenceIteration+1) + ". CID: " + conferenceIdReturned + " LID: " + locationIdReturned ;
                    mecStressTest.debugOutput.ScrollToCaret();
                    System.Threading.Thread.Sleep(this.periodBetweenCalls);
                }
                else
                {
                    blocker.Set();
                    mecStressTest.DebugOutputText = "\nCreate conference failed on initiation";
                    return false;
                }
                
            }   

            // Very possibly there will be more connections to fill

            while( !endOfTest && NumberOfLiveConnections < desiredConnections)
            {
                // Randomly choose a conference to stick someone in
                if( ConferenceCommand.CreateJoin( sessionsListSync, locationIdListListSync, mecStressTest, rand.Next( 0, sessionsListSync.Count ), ref locationIdReturned, ref conferenceIdReturned ) )
                {
                    mecStressTest.DebugOutputText = "\nAdded initial participant. CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                    mecStressTest.debugOutput.ScrollToCaret();
                    NumberOfLiveConnections++;

                    System.Threading.Thread.Sleep(this.periodBetweenCalls);
                }
                else
                {
                    mecStressTest.ErrorOutputText = "\nUnable to join a participant.  Exiting test";
                    mecStressTest.debugOutput.ScrollToCaret();
                    blocker.Set();
                    return false;
                }            
            }

            mecStressTest.DebugOutputText = "\nDone adding initial participants.";
            mecStressTest.debugOutput.ScrollToCaret();

            // Initiate all pertinent timers
//            this.checkStatusRandom.Start();
//            this.joinKickTimerRandom.Start();
//            this.createNewConference.Start();
            this.howLongIsTest.Start();

            blockForDurationOfTest.WaitOne(durationOfTest, false);

            return true;
        }

        public bool TestCleanup()
        {
            joinKickTimerRandomTime = DateTime.Now;
            checkStatusRandomTime = DateTime.Now;
//            joinKickTimerRandom.Stop();
//            checkStatusRandom.Stop();
//            createNewConference.Stop();

            // Clean up
            for(int i = 0; i < this.locationIdListListSync.Count; i++)
            {
                ArrayList tempArrayList = (ArrayList) locationIdListListSync[i];

                for(int j = 0; j < tempArrayList.Count; j++)
                {
                    if(ConferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, (string) tempArrayList[j], i) )
                    {
                        NumberOfLiveConnections--;
                        mecStressTest.DebugOutputText = "\nEnd of Test kicking";
                        mecStressTest.debugOutput.ScrollToCaret();
                    }
                    else
                    {
                        mecStressTest.DebugOutputText = "\nKicking participants at the end of the test failed";
                        mecStressTest.debugOutput.ScrollToCaret();
                        blocker.Set();
                        return false;
                    }
                }
    
                NumberOfLiveConferences--;
                System.Threading.Thread.Sleep(this.periodBetweenCalls);
            }
            mecStressTest.DebugOutputText = "\nDone kicking all remaining conference participants.";
            mecStressTest.DebugOutputText = "\n\n\nCreated a total of " + NumberOfTotalConferences + " conferences.";
            mecStressTest.DebugOutputText = "\nEstablished a total of " + NumberOfTotalConnections + " connections to a conference.";
            mecStressTest.DebugOutputText = "\nEstablished a total of " + NumberOfTotalKicks + " disconnections from a conference.";
            mecStressTest.debugOutput.ScrollToCaret();

            // Open up the spiggot again just to be sure
            blocker.Set();

            // Clean up timers
//            joinKickTimerRandom.Dispose();
//            checkStatusRandom.Dispose();
//            createNewConference.Dispose();
            howLongIsTest.Dispose();
            return true;

        }


        public void UpdateFormClock(object state, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan temp = DateTime.Now.Subtract(mecStressTest.testStartedAtThisTime);
            mecStressTest.testOutput_Dashboard_TotalTestTime.Text = "" + temp.Days + " : " + temp.Hours + " : " + temp.Minutes + " : " + temp.Seconds;
        }

		public bool IsTestStillExecuting(System.Timers.ElapsedEventArgs e)
		{	
			// timeEndOfTest is greater than the timer signal time when the test is still going on
			if(timeOfEndOfTest.Subtract(e.SignalTime) > TimeSpan.Zero)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public void UpdateTimerFireInterval(int averageTimerInterval, System.Timers.Timer timerToUpdate)
		{
			// Choose a random interval of time to fire another kick/join even
			// Well... random insofar as its between the minimum time between calls, and the desired
			//  frequency of calls
			int randomNumberOfSeconds = rand.Next(0, averageTimerInterval * 2 + 1);

			// Having an interval of 0 is bothersome.  Minimum is 100 milliseconds
			if(randomNumberOfSeconds == 0)
			{
				timerToUpdate.Interval = minimumInterval;
			}     
			else
			{
				TimeSpan tempTimeSpan = new TimeSpan(0,0,0,randomNumberOfSeconds,0);
				timerToUpdate.Interval = tempTimeSpan.TotalMilliseconds;    
			}
		}
            
        public int PullOutConference(ArrayList conferenceReferenceSync)
        {
            // Changing size and members of an array is not thread safe...
            lock(conferenceReferenceSyncLock)
            {
                int position = rand.Next( 0, conferenceReferenceSync.Count );
                // pull out the conference number, and remove it from the conferenceReference list
                int conferenceToUse = (int) conferenceReferenceSync[position];
                conferenceReferenceSync.Remove(position);
                return conferenceToUse;
            }
        }

        public void AddBackConference(ArrayList conferenceReferenceSync, int conferenceToRestore)
        {
            // Changing size and memebers of an array is not thread safe...
            lock(conferenceReferenceSyncLock)
            {
                int position = rand.Next( 0, conferenceReference.Count );
                // add back the conference number
                conferenceReferenceSync.Add(conferenceToRestore);
            }
        }
    }
}
