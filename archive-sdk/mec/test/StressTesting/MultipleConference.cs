using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
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
	/// The implementation of running MultipleConference
	/// </summary>
	public class MultipleConference	: TestBase
    {
        #region VariableDeclarations

        public AutoResetEvent blockForDurationOfTest = new AutoResetEvent(false);
        public ArrayList locationIdListList;
        public ArrayList locationIdListListSync;
        public ArrayList sessionsList;
        public ArrayList sessionsListSync;
        public ArrayList locationIdList;
        public int joinId;
        public string sessionId;

        public delegate bool StartTestDelegate();
        StartTestDelegate startTestDelegate;

        private TimeSpan durationOfTest;
        public bool testIsDone;
        public int numberOfLiveConnections;
        public int numberOfLiveConferences;
        public ProgressBar progressBar;
        public TimeSpan periodBetweenCalls;
        public int numberConnections;
        public int numberConferences;
        public int maximumConnections;
        public int maximumConferences;
        public TimeSpan periodBetweenHangups;
        public RichTextBox debugOutput;
        public RichTextBox errorOutput;
        public ProgressBar overallProgressBar;
        public string conferenceIdReturn;
        public string locationIdReturn;
        public bool endOfTest;
        #endregion VariableDeclarations

        
        // No ramp constructor
        public MultipleConference(MecStressTest mecStressTest, int testType) : base(mecStressTest)
        {   
            
            joinId = 1;
            endOfTest = false;
            numberOfLiveConnections = 0;
            numberOfLiveConferences = 0;
            locationIdList = new ArrayList();
            locationIdListList = new ArrayList();
            locationIdListListSync = ArrayList.Synchronized(locationIdList);
            sessionsList = new ArrayList();
            sessionsListSync = ArrayList.Synchronized(sessionsList);

            this.progressBar = mecStressTest.individualTestBar;
            this.overallProgressBar = mecStressTest.overallTestBar;

            switch(testType)
            {
                case 0:

                    this.periodBetweenCalls = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.multipeConference_NoRamp_PeriodBetweenCalls.Text), 0);
                    this.periodBetweenHangups = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.multipleConference_NoRamp_PeriodBetweenHangups.Text), 0);
                    this.durationOfTest = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.multipleConference_NoRamp_DurationTest.Text), 0);
                    this.numberConnections = System.Int32.Parse(mecStressTest.multipleConference_NoRamp_numberConnections.Text);
                    this.numberConferences = System.Int32.Parse(mecStressTest.multipleConference_NoRamp_numberConferences.Text);

                    startTestDelegate += new StartTestDelegate( StartTestNoRamp );

                    break;

                case 1:

                    this.periodBetweenCalls = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.multipleConferences_Ramp_PeriodBetweenCalls.Text), 0);
                    this.periodBetweenHangups = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.multipleConferences_Ramp_PeriodBetweenHangups.Text), 0);
                    this.durationOfTest = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.multipleConferences_Ramp_DurationTest.Text), 0);
                    this.maximumConnections = System.Int32.Parse(mecStressTest.multipleConferences_Ramp_maximumConnections.Text);
                    this.maximumConferences = System.Int32.Parse(mecStressTest.multipleConference_Ramp_maximumConf.Text);

                    startTestDelegate += new StartTestDelegate( StartTestRamp );

                    break;
            }
        }

        public override bool Start()
        {
            return startTestDelegate();
        }

        public bool StartTestNoRamp()
        {
            for(int conferenceIteration = 0; conferenceIteration < numberConferences; conferenceIteration++)
            {
                if ( !endOfTest )
                {
                    mecStressTest.DebugOutputText = "\n\nCreating conference " + (conferenceIteration+1) + "...";
                    // Create the conference
                    if(ConferenceCommand.CreateConference(sessionsListSync, locationIdListListSync, mecStressTest, ref locationIdReturn, ref conferenceIdReturn) == true)
                    {
                        progressBar.Step = 1;
                        overallProgressBar.Step = 1;
                        NumberOfLiveConnections++;
                        NumberOfLiveConferences++;
                        mecStressTest.DebugOutputText = " CID: " + conferenceIdReturn + " LID: " + locationIdReturn;

                        try
                        {
                            progressBar.PerformStep();
                            overallProgressBar.PerformStep();
                        }
                        catch(Exception e)
                        {
                            mecStressTest.ErrorOutputText = "\n" + e.ToString();
                            return false;
                        }
                    }
                    else
                    {
                        mecStressTest.ErrorOutputText = " failed to create a conference";
                        return false;
                    }

                    for(int i = 1; i < numberConnections; i++)
                    {
                        // Wait the user-determined amount of time
                        System.Threading.Thread.Sleep(periodBetweenCalls);

                        if( !endOfTest )
                        {         
                            mecStressTest.DebugOutputText = "\n\nAttempting to add a participant...";

                            if(ConferenceCommand.CreateJoin(sessionsListSync, locationIdListListSync, mecStressTest, conferenceIteration, ref locationIdReturn, ref conferenceIdReturn) == true)
                            {
                                progressBar.Step = 1;
                                overallProgressBar.Step = 1;
                                NumberOfLiveConnections++;
                                mecStressTest.DebugOutputText = " CID: "+ conferenceIdReturn + " LID: " + locationIdReturn;

                                try
                                {
                                    progressBar.PerformStep();
                                    overallProgressBar.PerformStep();
                                }
                                catch(Exception e)
                                {
                                    mecStressTest.ErrorOutputText = "\n" + e.ToString();
                                    return false;
                                }
                            }
                            else
                            {
                                mecStressTest.ErrorOutputText = " failed to add participant";
                                return false;
                            }
                        }
                    }
                }
            }

            //Allow conferences to sit for specified time
            blockForDurationOfTest.WaitOne(durationOfTest, false);

            mecStressTest.DebugOutputText = "\nInitiating kicking";

            //Kick all participants
            for(int conferenceIteration = 0; conferenceIteration < sessionsListSync.Count; conferenceIteration++)
            {
                mecStressTest.DebugOutputText = "\n\nAttempting to kick a participant...";

                ArrayList tempLocationIdList = (ArrayList) locationIdListList[conferenceIteration];

                for(int i = 0; i < tempLocationIdList.Count; i++)
                {
                    // Wait the user-determined amount of time
                    System.Threading.Thread.Sleep(periodBetweenHangups);

                    //Pull out the locationIdList for that conference

                    if(ConferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, (string) tempLocationIdList[i], conferenceIteration) == true)
                    {
                        progressBar.Step = 1;
                        overallProgressBar.Step = 1;
                        NumberOfLiveConnections--;
                        mecStressTest.DebugOutputText = " kick successful";

                        try
                        {
                            progressBar.PerformStep();
                            overallProgressBar.PerformStep();
                        }
                        catch(Exception e)
                        {
                            mecStressTest.ErrorOutputText = "\n" + e.ToString();
                            return false;
                        }
                    }
                    else
                    {
                        mecStressTest.ErrorOutputText = " kick failed";
                        return false;
                    }
                }

                NumberOfLiveConferences--;
            }

            locationIdList.Clear();
            locationIdListListSync.Clear();
            sessionsListSync.Clear();
            return true;
        }

        public bool StartTestRamp()
        {
            for(int numberConferencesIteration = 1; numberConferencesIteration < maximumConferences; numberConferencesIteration++)
            {
                if( !endOfTest )
                {
                    mecStressTest.DebugOutputText = "\n Initiating testing for a conference sized " + numberConferencesIteration;                
                    
                    for(int conferenceIteration = 0; conferenceIteration < numberConferencesIteration; conferenceIteration++)
                    {
                        if( !endOfTest )
                        {
                            // Calculate the number of connections to distribute to each conference
                            float numberOfConnectionsForEachConference = (float) maximumConnections / (float) numberConferencesIteration;
                            int numberOfConnectionsForEachConferenceRounded = (int) Math.Floor(numberOfConnectionsForEachConference);

                            mecStressTest.DebugOutputText = "\n\nCreating conference " + (conferenceIteration + 1) + "...";

                            // Create the conference
                            if(ConferenceCommand.CreateConference(sessionsListSync, locationIdListListSync, mecStressTest, ref locationIdReturn, ref conferenceIdReturn) == true)
                            {
                                progressBar.Step = 1;
                                overallProgressBar.Step = 1;
                                NumberOfLiveConnections++;
                                NumberOfLiveConferences++;
                                mecStressTest.DebugOutputText = " CID: " + conferenceIdReturn + " LID: " + locationIdReturn;

                                try
                                {
                                    progressBar.PerformStep();
                                    overallProgressBar.PerformStep();
                                }
                                catch(Exception e)
                                {
                                    mecStressTest.ErrorOutputText = "\n" + e.ToString();
                                    return false;
                                }
                            }
                            else
                            {
                                mecStressTest.ErrorOutputText = " failed to create conference";
                                return false;
                            }

                            for(int i = 1; i < numberOfConnectionsForEachConferenceRounded; i++)
                            {
                                // Wait the user-determined amount of time
                                System.Threading.Thread.Sleep(periodBetweenCalls);

                                if( !endOfTest )
                                {                                                             
                                    mecStressTest.DebugOutputText = "\n\nAttempting to add a conference participant...";    

                                    if(ConferenceCommand.CreateJoin(sessionsListSync, locationIdListListSync, mecStressTest, conferenceIteration, ref locationIdReturn, ref conferenceIdReturn) == true)
                                    {
                                        progressBar.Step = 1;
                                        overallProgressBar.Step = 1;
                                        NumberOfLiveConnections++;
                                        mecStressTest.DebugOutputText = " CID: " + conferenceIdReturn + " LID: " + locationIdReturn;

                                        try
                                        {
                                            progressBar.PerformStep();
                                            overallProgressBar.PerformStep();
                                        }
                                        catch(Exception e)
                                        {
                                            mecStressTest.ErrorOutputText = "\n" + e.ToString();
                                            return false;
                                        }
                                    }
                                    else
                                    {   
                                        mecStressTest.DebugOutputText = " failed to add a participant";
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    //Allow conferences to sit for specified time
                    blockForDurationOfTest.WaitOne(durationOfTest, false);

                    mecStressTest.DebugOutputText = "\nInitiating kicking";

                    //Kick all participants
                    for(int conferenceIteration = 0; conferenceIteration < locationIdListList.Count; conferenceIteration++)
                    {
                        //Pull out the locationIdList for this conference
                        ArrayList tempLocationIdList = (ArrayList) locationIdListListSync[conferenceIteration];

                        for(int i = 0; i < tempLocationIdList.Count; i++)
                        {
                            // Wait the user-determined amount of time
                            System.Threading.Thread.Sleep(periodBetweenHangups);
            
                            mecStressTest.DebugOutputText = "\n\nAttempting to kick a participant from the conference...";

                            if(ConferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, (string) tempLocationIdList[i], conferenceIteration) == true)
                            {
                                progressBar.Step = 1;
                                overallProgressBar.Step = 1;
                                NumberOfLiveConnections--;
                                mecStressTest.DebugOutputText = " kick successful";

                                try
                                {
                                    progressBar.PerformStep();
                                    overallProgressBar.PerformStep();
                                }
                                catch(Exception e)
                                {
                                    mecStressTest.ErrorOutputText = "\n" + e.ToString();
                                    return false;
                                }
                            }
                            else
                            {
                                mecStressTest.ErrorOutputText = " kick failed";
                                return false;
                            }
                        }
                    } 
                    mecStressTest.DebugOutputText = "\nDestroying a conference";
                    NumberOfLiveConferences--;
                    this.locationIdListList.Clear();
                    this.sessionsList.Clear();
                }
            }
            locationIdList.Clear();
            locationIdListListSync.Clear();
            sessionsListSync.Clear();
            return true;    
        }
        public override bool EndTest()
        {
            blockForDurationOfTest.Set();
            endOfTest = true;
            return true;
        }

	}
}
