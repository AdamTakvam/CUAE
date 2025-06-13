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
    public class RandomizedConference : TestBase
    {

        #region VariableDeclarations

        public StressTesting.RangeQueue rangeQueue;
        public StressTesting.Settings settings;
        public AutoResetEvent blocker = new AutoResetEvent(true);
        public AutoResetEvent blockForDurationOfTest = new AutoResetEvent(false);
        public delegate bool StartTestDelegate();
        StartTestDelegate startTestDelegate;
        private System.Timers.Timer joinKickTimerRandom;
        private System.Timers.Timer checkStatusRandom;
        private System.Timers.Timer createNewConference;
        private System.Timers.Timer howLongIsTest;
        private ConferenceCommand conferenceCommand; 
        private DateTime joinKickTimerRandomTime;
        private DateTime checkStatusRandomTime;
                
        // values used to ensure that "random" is enforced
        public int forcedSkewAverageConferences;
        public int forcedSkewConnections;
        public int desiredConnections;
        public int desiredConferences;
        public Random rand;

        public int numRunningAverageConferencesSamples;
        public int numRunningAverageConnectionsSamples;
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
        private int averagePeriodBetweenCalls;
        public TimeSpan periodBetweenHangups;
        public RichTextBox debugOutput;
        public RichTextBox errorOutput;
        public AutoResetEvent testIsDoneEvent;
        private string locationIdReturned;
        private string conferenceIdReturned;
        private string lastLocationMade;
        public ArrayList locationIdList;
        public ArrayList sessionsList;
        public ArrayList locationIdListList;
        public Hashtable lookupTableForPhoneNumbers;
        
        
        // Get synchronized versions of these ArrayLists
        public ArrayList locationIdListSync;
        public ArrayList sessionsListSync;
        public ArrayList locationIdListListSync;

        public bool endOfTest;
        #endregion VariableDeclarations

        // Full Random with no database checking constructor
        public RandomizedConference(MecStressTest mecStressTest, StressTesting.Settings settings) : base(mecStressTest)
        {
            this.settings = settings;
            rangeQueue = new  StressTesting.RangeQueue(settings.numberOfRanges, settings.lowerBounds, settings.upperBounds);
            conferenceCommand = new ConferenceCommand(settings.appServerIp, settings.callGenIp, settings.chooseSim, settings.callManagerIp, settings.errorChecking, settings.initialPause);
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
            lastLocationMade = "";
            locationIdList = new ArrayList();
            locationIdListList = new ArrayList();
            sessionsList = new ArrayList();
            lookupTableForPhoneNumbers = new Hashtable();

            rand = new Random((int) DateTime.Now.Ticks);

            locationIdListSync = ArrayList.Synchronized(locationIdList);
            sessionsListSync = ArrayList.Synchronized(sessionsList);
            locationIdListListSync = ArrayList.Synchronized(locationIdListList);

            
            joinKickTimerRandomTime = DateTime.MaxValue;
            checkStatusRandomTime = DateTime.MaxValue;

            joinKickTimerRandom = new System.Timers.Timer(6000);
            joinKickTimerRandom.Elapsed += new System.Timers.ElapsedEventHandler( this.JoinKickTimerRandomFired );

            // When this timer gets fired, it tells the  next random create join to instead do a create conference
            createNewConference = new System.Timers.Timer(60000);
            createNewConference.Elapsed += new System.Timers.ElapsedEventHandler( this.CreateNewConferenceFired );
           

            // Force a spike
            checkStatusRandom = new System.Timers.Timer(30000);
            checkStatusRandom.Elapsed += new System.Timers.ElapsedEventHandler( this.CheckStatusRandomFired );

            // Create handy dandy timer to let the user see the test duration.
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
            Reset();

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

        public bool StartTestSkewedRandom(ProgressBar progressBar){return true;}

  
 
        public void JoinKickTimerSkewedFired(object state, System.Timers.ElapsedEventArgs e){}

        public void JoinKickTimerRandomFired(object state, System.Timers.ElapsedEventArgs e)
        {
            if(joinKickTimerRandom != null)
            {
                joinKickTimerRandom.Stop();
            }

            // Hang if any other timer is going off
            Blocker();

            if( !endOfTest )
            {

                // Sleep for the minimum amount of time between calls/hangups
                      
                System.Threading.Thread.Sleep(this.periodBetweenCalls);
                // make sure that this event  be fired
                if( joinKickTimerRandomTime.Subtract(e.SignalTime) > TimeSpan.Zero)
                {                

                    // Choose a random interval of time to fire another kick/join even
                    // Well... random insofar as its between the minimum time between calls, and the desired
                    //  frequency of calls
                    int randomNumberOfSeconds = rand.Next(0, this.averagePeriodBetweenCalls * 2 + 1);

                    // Having an interval of 0 is bothersome.  Minimum is 100 milliseconds
                    if(randomNumberOfSeconds == 0)
                    {
                        joinKickTimerRandom.Interval = 100;
                    }     
                    else
                    {
                        TimeSpan tempTimeSpan = new TimeSpan(0,0,0,randomNumberOfSeconds,0);
                        joinKickTimerRandom.Interval = tempTimeSpan.TotalMilliseconds;    
                    }
                    // Randomly choose whether a location will be added or removed
                    int trueOrFalse = rand.Next(0,2);

                    switch(trueOrFalse)
                    {
                        case 0:               
                            JoinOrKick = false;
                            break;
                        case 1:
                            JoinOrKick = true;
                            break;
                        case 2:
                            mecStressTest.ErrorOutputText = "\nCrazy Stuff here rand result = "  + trueOrFalse;
                            mecStressTest.errorOutput.ScrollToCaret();
                            break;
                    }
                    // True is join/create
                    if(JoinOrKick)
                    {                       
                        // True is Create
                        if(CreateInsteadOfJoin)
                        {
                            mecStressTest.DebugOutputText = "\n\nAttempting to create a conference...";
                            mecStressTest.debugOutput.ScrollToCaret();
                            CreateInsteadOfJoin = false;

                            // Too many conferences already?
                            if(NumberOfLiveConferences < maximumConferences)
                            {
                                // Too many connections already?
                                if(NumberOfLiveConnections < maximumConnections)
                                {     
                                    int phoneNumber;
                                    if(rangeQueue.GrabANumber( out phoneNumber))
                                    {
                                        if(conferenceCommand.CreateConference(sessionsListSync, locationIdListListSync, mecStressTest, phoneNumber, ref locationIdReturned, ref conferenceIdReturned) )
                                        {
                                            
                                            lookupTableForPhoneNumbers[locationIdReturned] = phoneNumber;
                                            NumberOfLiveConnections++;
                                            NumberOfLiveConferences++;
                                            mecStressTest.DebugOutputText = " CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                                            mecStressTest.debugOutput.ScrollToCaret();
                                            lastLocationMade = locationIdReturned;
                                        }
                                        else
                                        {
                                            rangeQueue.ReturnANumber(phoneNumber);
                                            mecStressTest.ErrorOutputText = "\n create Conference failed";
                                            mecStressTest.errorOutput.ScrollToCaret();
                                            // REFACTOR: MUST EXIT
                                        }
                                    }
                                    else
                                    {
                                        mecStressTest.DebugOutputText = "\nall number of SimClient are in use";
                                        mecStressTest.debugOutput.ScrollToCaret();
                                    }

                                }
                                else
                                {
                                    mecStressTest.DebugOutputText = " wanted to create a conference, but the maximum # of connections is filled";
                                    mecStressTest.debugOutput.ScrollToCaret();
                                }
                            }
                        
                                // Create was desired, but the maximum # of conferences is already filled.
                                // This join is a "create-substitute"
                            else
                            {
                                mecStressTest.DebugOutputText = " but the maximum number of conferences has been reached";
                                mecStressTest.DebugOutputText = "\n\nAttempting a join instead...";
                                mecStressTest.debugOutput.ScrollToCaret();

                                if(NumberOfLiveConferences > 0)
                                {   
                                    if(NumberOfLiveConnections < maximumConnections)
                                    {
                                        int phoneNumber;
                                        if(rangeQueue.GrabANumber( out phoneNumber))
                                        {
                                            if( conferenceCommand.CreateJoin(  sessionsListSync, locationIdListListSync, mecStressTest, phoneNumber, rand.Next( 0, sessionsListSync.Count ), ref locationIdReturned, ref conferenceIdReturned ))
                                            {
                                                lookupTableForPhoneNumbers[locationIdReturned] = phoneNumber;
                                                NumberOfLiveConnections++;
                                                mecStressTest.DebugOutputText = " CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                                                mecStressTest.debugOutput.ScrollToCaret();
                                                lastLocationMade = locationIdReturned;
                                            }
                                            else
                                            {
                                                rangeQueue.ReturnANumber(phoneNumber);
                                                mecStressTest.ErrorOutputText = "\nJoin Conference failed";
                                                mecStressTest.errorOutput.ScrollToCaret();
                                                //REFACTOR: MUST EXIT
                                            }
                                        }
                                        else
                                        {
                                            mecStressTest.DebugOutputText = "\nall number of SimClient are in use";
                                            mecStressTest.debugOutput.ScrollToCaret();
                                        }
                                    }
                                    else
                                    {
                                        mecStressTest.DebugOutputText = " maximum # of connections being used";
                                        mecStressTest.debugOutput.ScrollToCaret();
                                    }
                                }
                                else
                                {
                                    mecStressTest.DebugOutputText = " no conferences avaiable to join to";
                                    mecStressTest.debugOutput.ScrollToCaret();
                                }
                            }
                        }
                            // False is Join
                        else
                        {
                            mecStressTest.DebugOutputText = "\n\nAttempting a join...";
                            // Make sure that there is a conference to join to
                            if(NumberOfLiveConferences > 0)
                            {
                                // Make sure we aren't going to surpass our maximum # of connections
                                if(NumberOfLiveConnections < maximumConnections)
                                {
                                    int phoneNumber;
                                    if(rangeQueue.GrabANumber( out phoneNumber))
                                    {
                                        // Randomly choose a conference to stick someone in
                                        if( conferenceCommand.CreateJoin( sessionsListSync, locationIdListListSync, mecStressTest, phoneNumber, rand.Next( 0, sessionsListSync.Count ), ref locationIdReturned, ref conferenceIdReturned) )
                                        {
                                            lookupTableForPhoneNumbers[locationIdReturned] = phoneNumber;

                                            NumberOfLiveConnections++;
                                            mecStressTest.DebugOutputText = " CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                                            mecStressTest.debugOutput.ScrollToCaret();
                                            lastLocationMade = locationIdReturned;
                                        }
                                        else
                                        {
                                            rangeQueue.ReturnANumber(phoneNumber);
                                            mecStressTest.ErrorOutputText = "\njoin failed";
                                            mecStressTest.errorOutput.ScrollToCaret();
                                            // REFACTOR:  must exit at this point.  
                                        }
                                    }
                                    else
                                    {
                                        mecStressTest.DebugOutputText = "\nall number of SimClient are in use";
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
                                mecStressTest.DebugOutputText = " but no conferences are open";
                            }
                        }
                    }
                        // False is Kick
                    else
                    {
                        mecStressTest.DebugOutputText = "\n\nAttempting a kick";
                        // Check that a conference even exists
                        if(sessionsListSync.Count > 0)
                        {
                            // This group of code chooses a random conference and location id to kick
                            int chooseRandomConference = rand.Next(0, ( sessionsListSync.Count ) );
                            ArrayList tempArrayList = (ArrayList) locationIdListListSync[chooseRandomConference];
                            int numberOfLocationsInChosenConference = tempArrayList.Count;
                            int chooseRandomLocationFromConference = rand.Next(0, (tempArrayList.Count) );
                            string randomLocationId = (string) tempArrayList[chooseRandomLocationFromConference];  
                            mecStressTest.DebugOutputText = " with LID: " + randomLocationId + "...";
                                                        
                            // REFACTOR: THIS IS MERELY A PATCH UNTIL THE MEC HANDLES KICKS BEFORE CALL ESTABLISHED
                            // DOUBLE REFACTOR:  removing situation in which the last location added can be kicked.
                            if(randomLocationId != lastLocationMade)
                            {  
                                if(conferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, randomLocationId, chooseRandomConference))
                                {
                                    NumberOfLiveConnections--;
                                    try
                                    {
                                        rangeQueue.ReturnANumber((int) lookupTableForPhoneNumbers[randomLocationId]);
                                    }
                                    catch
                                    {
                                    }
                                    mecStressTest.DebugOutputText = " kicked participant";
                                    mecStressTest.debugOutput.ScrollToCaret();
                                    // Update the locationIdListSync for that particular conference to reflect that location 
                                    // has been removed!
                                    tempArrayList.RemoveAt(chooseRandomLocationFromConference);
                        
                                    // This means we have to update the sessionsListSync and locationIdListListSync to reflect
                                    // that a conference has been destroyed
                                    if(numberOfLocationsInChosenConference == 1)
                                    {
                                        sessionsListSync.RemoveAt(chooseRandomConference);
                                        locationIdListListSync.RemoveAt(chooseRandomConference);
                                        NumberOfLiveConferences--;
                                        mecStressTest.DebugOutputText = "\nDestroyed a conference";
                                        mecStressTest.debugOutput.ScrollToCaret();
                                    }                 
                                }
                                else
                                {
                                    mecStressTest.ErrorOutputText = "\nkick participant of LID: " + randomLocationId + " failed";
                                    mecStressTest.errorOutput.ScrollToCaret();
                                    // REFACTOR: MUST EXIT
                                }
                            }
                        }
                        else
                        {
                            mecStressTest.DebugOutputText = " kick was attempted, but no conferences are open";
                            mecStressTest.debugOutput.ScrollToCaret();
                        }
                    }
                }
                else
                {
                    mecStressTest.DebugOutputText = "\n\nJoin/Kick event was blocked from firing";
                    mecStressTest.debugOutput.ScrollToCaret();
                }  
                joinKickTimerRandom.Start();
            }
            // Release the lock for the next timer
            blocker.Set();
        }
        
        // Spike creation timer
        public void CheckStatusRandomFired(object state , System.Timers.ElapsedEventArgs e)
        {         
            if(checkStatusRandom != null)
            {
                checkStatusRandom.Stop();
            }

            // Hang if any other timer is going off
            Blocker();

            if( !endOfTest )
            {
                // REFACTOR:  
                // Spikes could hypothetically come at same time, so eventually I need to move 
                // this over to the milliseconds 
                TimeSpan tempTimeSpan = new TimeSpan(0,0,rand.Next(1, this.averageSpike),0,0);
                checkStatusRandom.Interval = tempTimeSpan.TotalMilliseconds;

                // Go ahead and choose whether to create a new conference, or destroy one
                int trueOrFalse = rand.Next(0,2);

                switch(trueOrFalse)
                {
                    case 0:               
                        CreateOrDestroy = false;
                        break;
                    case 1:
                        CreateOrDestroy = true;
                        break;
                    case 2:
                        mecStressTest.ErrorOutputText = "\nCrazy Stuff here rand result";
                        mecStressTest.debugOutput.ScrollToCaret();
                        break;
                }
                // True is create
                if(CreateOrDestroy)
                {        
                    // First stop all other timers from executing, or intelligently block the part of the 
                    // locationIdListSync that we are using

                    /* Lots of smart stuff right here*/

                    // Determine out how many open connections we have,
                    // and spike up randomly possibly up to the maximum amount of unused connections.
                
                    // First ensure that a conference can be created in the first place
                    mecStressTest.DebugOutputText = "\n\nCreating a spike conference...";
                    if( NumberOfLiveConferences < maximumConferences )
                    {
                        int numberOfOpenConnections = maximumConnections - NumberOfLiveConnections;
                    
                        if(numberOfOpenConnections > 0)
                        {
                            int phoneNumber;
                            if(rangeQueue.GrabANumber( out phoneNumber))
                            {
                                if(conferenceCommand.CreateConference(sessionsListSync, locationIdListListSync, mecStressTest, phoneNumber, ref locationIdReturned, ref conferenceIdReturned ))
                                {
                                    lookupTableForPhoneNumbers[locationIdReturned] = phoneNumber;

                                    NumberOfLiveConnections++;
                                    NumberOfLiveConferences++;
                                    numberOfOpenConnections--;

                                    mecStressTest.DebugOutputText = " CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                                    mecStressTest.debugOutput.ScrollToCaret();
                                    lastLocationMade = locationIdReturned;

                                    // Chose a random number of connections to add to the conference,
                                    // based on the number of open connections
                                    if(numberOfOpenConnections > 0 )
                                    {
                                        int numberOfParticipantsToAddToSpike = rand.Next(1, numberOfOpenConnections + 1);

                                        for(int i = 0; i < numberOfParticipantsToAddToSpike;  i++)
                                        {
                                            System.Threading.Thread.Sleep(this.periodBetweenCalls);

                                            mecStressTest.DebugOutputText = "\nAdding participant " + (i+2) + " to spike conference...";
                                            // Stick someone in last made conference
                                            int phoneNumber2;
                                            if(rangeQueue.GrabANumber( out phoneNumber2))
                                            {
                                                if( conferenceCommand.CreateJoin( sessionsListSync, locationIdListListSync, mecStressTest, phoneNumber2, sessionsListSync.Count - 1, ref locationIdReturned, ref conferenceIdReturned ))
                                                {
                                                    lookupTableForPhoneNumbers[locationIdReturned] = phoneNumber;

                                                    mecStressTest.DebugOutputText = " CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                                                    mecStressTest.debugOutput.ScrollToCaret();
                                                    NumberOfLiveConnections++;
                                                    lastLocationMade = locationIdReturned;
                                                }
                                                else
                                                {
                                                    //Error output
                                                    rangeQueue.ReturnANumber(phoneNumber);
                                                    mecStressTest.ErrorOutputText = "\nUnable to join a participant to a spike conference";
                                                    mecStressTest.debugOutput.ScrollToCaret();
                                                }
                                                System.Threading.Thread.Sleep(this.periodBetweenCalls);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        mecStressTest.DebugOutputText = "\nCreated a spike conference with only one participant";
                                        mecStressTest.debugOutput.ScrollToCaret();
                                    }
                                }
                                else
                                {
                                    // Error output
                                    rangeQueue.ReturnANumber(phoneNumber);
                                    mecStressTest.ErrorOutputText = "\nFailure: Unable to create a spike conference";
                                    mecStressTest.debugOutput.ScrollToCaret();

                                }
                            }
                            else
                            {
                                
                                
                                mecStressTest.DebugOutputText = "\nall number of SimClient are in use";
                                mecStressTest.debugOutput.ScrollToCaret();
                            }
                        }
                        else
                        {
                            mecStressTest.DebugOutputText = " conference creation spike attempted, but the maximum # of connections has already been reached";
                            mecStressTest.debugOutput.ScrollToCaret();
                        }
                    }
                    else
                    {
                        mecStressTest.DebugOutputText = " conference creation spike attempted, but the maximum # of conferences has already been reached";
                        mecStressTest.debugOutput.ScrollToCaret();
                    }
                }
                    // CreateOrDestroy == false is destroy
                else
                {
                    mecStressTest.DebugOutputText = "\n\nAttempting to destroy spike conference...";
                    // First check to make sure that a conference even exists
                    if(NumberOfLiveConferences > 0)
                    {
                        // Randomly choose to destroy a conference 
                        int chooseRandomConference = rand.Next(0, ( sessionsListSync.Count ) );
                        ArrayList tempArrayList = (ArrayList) locationIdListListSync[chooseRandomConference];
                        int numberOfLocationsInChosenConference = tempArrayList.Count;
                        string locationIdToKick;
                        bool allLocationsSuccessfullyKicked = true;
                        int indexer = 0;

                        for(int i = 0; i < numberOfLocationsInChosenConference; i++)
                        {
                            locationIdToKick = (string) tempArrayList[indexer];
                            
                            mecStressTest.DebugOutputText = "\nAttempting to kick participant with LID: " + locationIdToKick + "...";

                            // DOUBLE REFACTOR:  removing situation in which the last location added can be kicked.
                            if(lastLocationMade != locationIdToKick && conferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, locationIdToKick, chooseRandomConference))
                            {
                                try
                                {
                                    rangeQueue.ReturnANumber((int) lookupTableForPhoneNumbers[locationIdToKick]);
                                }
                                catch
                                {
                                }
                                mecStressTest.DebugOutputText = " kicked participant from spike destroy conference";
                                mecStressTest.debugOutput.ScrollToCaret();
                                NumberOfLiveConnections--;
                                tempArrayList.RemoveAt(indexer);

                                // All positions ahead of this one should fall down.  Therefore, indexer,
                                // which is usually 0, doesn't need to move
                            }
                            else
                            {
                                // Keep up with the success status of kicking all connections from the conference
                                allLocationsSuccessfullyKicked = false;
                                
                                // If failed, indexer should be incremented, because the arraylist
                                // of locations will not have the first location removed
                                indexer++;

                                // Error output
                                mecStressTest.ErrorOutputText = "\nUnable to kick location " + locationIdToKick + " from a spike conference";
                                mecStressTest.debugOutput.ScrollToCaret();
                            }

                            System.Threading.Thread.Sleep(this.periodBetweenCalls);
                        }
                
                        if(allLocationsSuccessfullyKicked)
                        {
                            sessionsListSync.RemoveAt(chooseRandomConference);
                            locationIdListListSync.RemoveAt(chooseRandomConference);
                            mecStressTest.DebugOutputText = "\nDestroyed a spike conference";
                            mecStressTest.debugOutput.ScrollToCaret();
                            NumberOfLiveConferences--;
                        }
                        else
                        {
                            mecStressTest.ErrorOutputText = "\nWasn't able to destroy a conference due to "+ indexer + " failed kicks";
                            mecStressTest.errorOutput.ScrollToCaret();
                        }
                    }
                    else
                    {
                        mecStressTest.DebugOutputText = " a spike kick conference was attempted, but no conferences exist";
                        mecStressTest.debugOutput.ScrollToCaret();
                    }
                }
                mecStressTest.DebugOutputText = "\nNext scheduled spike: " + tempTimeSpan.Minutes + " minutes from now.";
                mecStressTest.debugOutput.ScrollToCaret();

                // Release the lock for the next timer
                       
                checkStatusRandom.Start();
            }
            blocker.Set();
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
                    Thread.Sleep(100);
                }
            }
        }
       
        public void CreateNewConferenceFired(object state, System.Timers.ElapsedEventArgs e)
        {
            
            if(createNewConference != null)
            {
                createNewConference.Stop();
            }

            // Hang if any other timer is going off
            Blocker();

            if( !endOfTest )
            {
                CreateInsteadOfJoin = true;

                // Since there are a ok number of conferences, just do the usual:
                // Randomly make a new conference every 1 - 30 minutes
                TimeSpan tempTimeSpan = new TimeSpan(0,0,rand.Next(1, 3),0,0);
                mecStressTest.DebugOutputText = "\nNext scheduled create a conference: " + tempTimeSpan.Minutes + " minutes from now.";
                mecStressTest.debugOutput.ScrollToCaret();
                createNewConference.Interval = tempTimeSpan.TotalMilliseconds;
                  
                // Release the lock for the next timer
            
                
                createNewConference.Start();
            }
            blocker.Set();
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
                
                mecStressTest.DebugOutputText = "\nCreating conference " + (conferenceIteration+1) + "... ";

                int phoneNumber;
                if(rangeQueue.GrabANumber( out phoneNumber))
                {
                    if( !endOfTest && conferenceCommand.CreateConference(sessionsListSync, locationIdListListSync, mecStressTest, phoneNumber, ref locationIdReturned, ref conferenceIdReturned ) == true)
                    {
                        lookupTableForPhoneNumbers[locationIdReturned] = phoneNumber;

                        NumberOfLiveConnections++;
                        NumberOfLiveConferences++;  
                        mecStressTest.DebugOutputText = "CID: " + conferenceIdReturned + " LID: " + locationIdReturned ;
                        mecStressTest.debugOutput.ScrollToCaret();
                        System.Threading.Thread.Sleep(this.periodBetweenCalls);
                    }
                    else
                    {
                        blocker.Set();
                        //rangeQueue.ReturnANumber(phoneNumber);
                        mecStressTest.DebugOutputText = "\nCreate conference failed on initiation";
                        return false;
                    }
                }
                else
                {
                    mecStressTest.DebugOutputText = "\nall number of SimClient are in use";
                    mecStressTest.debugOutput.ScrollToCaret();
                }
                
            }   

            // Very possibly there will be more connections to fill

            while( !endOfTest && NumberOfLiveConnections < desiredConnections)
            {
                // Randomly choose a conference to stick someone in
                mecStressTest.DebugOutputText = "\nAdded initial participant... ";
                int phoneNumber;
                if(rangeQueue.GrabANumber( out phoneNumber))
                {
                    if( conferenceCommand.CreateJoin( sessionsListSync, locationIdListListSync, mecStressTest, phoneNumber, rand.Next( 0, sessionsListSync.Count ), ref locationIdReturned, ref conferenceIdReturned ) )
                    {
                        lookupTableForPhoneNumbers[locationIdReturned] = phoneNumber;

                        mecStressTest.DebugOutputText = "CID: " + conferenceIdReturned + " LID: " + locationIdReturned;
                        mecStressTest.debugOutput.ScrollToCaret();
                        NumberOfLiveConnections++;

                        System.Threading.Thread.Sleep(this.periodBetweenCalls);
                    }
                    else
                    {
                        rangeQueue.ReturnANumber(phoneNumber);
                        mecStressTest.ErrorOutputText = "\nUnable to join a participant.  Exiting test";
                        mecStressTest.debugOutput.ScrollToCaret();
                        blocker.Set();
                        return false;
                    }            
                }
            }

            mecStressTest.DebugOutputText = "\nDone adding initial participants.";
            mecStressTest.debugOutput.ScrollToCaret();

            // Initiate all pertinent timers
            this.checkStatusRandom.Start();
            this.joinKickTimerRandom.Start();
            this.createNewConference.Start();
            this.howLongIsTest.Start();

            blockForDurationOfTest.WaitOne(durationOfTest, false);

            return true;
        }

        public bool TestCleanup()
        {
            joinKickTimerRandomTime = DateTime.Now;
            checkStatusRandomTime = DateTime.Now;
            joinKickTimerRandom.Stop();
            checkStatusRandom.Stop();
            createNewConference.Stop();

            // Clean up
            for(int i = 0; i < this.locationIdListListSync.Count; i++)
            {
                ArrayList tempArrayList = (ArrayList) locationIdListListSync[i];

                for(int j = 0; j < tempArrayList.Count; j++)
                {
                    if( conferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, (string) tempArrayList[j], i) )
                    {
                        try
                        {
                            rangeQueue.ReturnANumber((int) lookupTableForPhoneNumbers[(string) tempArrayList[j]]);
                        }
                        catch
                        {

                        }
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
            joinKickTimerRandom.Dispose();
            checkStatusRandom.Dispose();
            createNewConference.Dispose();
            howLongIsTest.Dispose();
            return true;

        }
        public void UpdateFormClock(object state, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan temp = DateTime.Now.Subtract(mecStressTest.testStartedAtThisTime);
            mecStressTest.testOutput_Dashboard_TotalTestTime.Text = "" + temp.Days + " : " + temp.Hours + " : " + temp.Minutes + " : " + temp.Seconds;
        }
    }
}
