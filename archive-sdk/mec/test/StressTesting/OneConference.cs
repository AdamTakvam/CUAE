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
	/// The implementation of running OneConference
	/// </summary>
	public class OneConference : TestBase
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
        public int maximumConnections;
        
        public delegate bool StartTestDelegate();
        StartTestDelegate startTestDelegate;

        private TimeSpan durationOfTest;
        public bool testIsDone;
        public bool endOfTest;
        public int numberOfLiveConnections;
        public int numberOfLiveConferences;
        public ProgressBar progressBar;
        public TimeSpan periodBetweenCalls;
        public int numberConnections;
        public TimeSpan periodBetweenHangups;
        public RichTextBox debugOutput;
        public RichTextBox errorOutput;
        public ProgressBar overallProgressBar;
        public string conferenceIdReturn;
        public string locationIdReturn;
        #endregion VariableDeclarations


        // No ramp constructor
        public OneConference(MecStressTest mecStressTest, int testType) : base(mecStressTest)
        {
            joinId = 1;
            endOfTest = false;
            locationIdList = new ArrayList();
            locationIdListList = new ArrayList();
            locationIdListListSync = ArrayList.Synchronized(locationIdList);
            sessionsList = new ArrayList();
            sessionsListSync = ArrayList.Synchronized(sessionsList);

            this.mecStressTest = mecStressTest;
            this.progressBar = mecStressTest.individualTestBar;
            this.overallProgressBar = mecStressTest.overallTestBar;
            
            switch(testType)
            {   
                // Case 0 is no ramp
                case 0:
                    this.periodBetweenCalls = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.oneConference_NoRampPeriodBetweenJoins.Text), 0);
                    this.periodBetweenHangups = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.oneConference_NoRamp_PeriodBetweenHangups.Text), 0);
                    this.durationOfTest = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.oneConference_NoRamp_DurationTest.Text), 0);
                    this.numberConnections = System.Int32.Parse(mecStressTest.oneConference_NoRamp_NumConnections.Text);

                    startTestDelegate += new StartTestDelegate( StartTestNoRamp );
                    break;

                // Case 1 is ramp
                case 1:
                    this.periodBetweenCalls = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.oneConference_Ramp_PeriodBetweenCalls.Text), 0);
                    this.periodBetweenHangups = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.oneConference_Ramp_PeriodBetweenHangups.Text), 0);
                    this.durationOfTest = new TimeSpan(0,0,0, System.Int32.Parse(mecStressTest.oneConference_Ramp_DurationTest.Text), 0);
                    this.maximumConnections = System.Int32.Parse(mecStressTest.oneConference_Ramp_MaximumConnections.Text);

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

            // Create the conference
            mecStressTest.DebugOutputText = "\n\nAttempting to create the test conference...";
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
    
            for(int i = 1; i < numberConnections; i++)
            {
                System.Threading.Thread.Sleep(periodBetweenCalls);

                if( !endOfTest )
                {
                    // Wait the user-determined amount of time
                    
                    mecStressTest.DebugOutputText = "\n\nAttempting to add a location to the conference...";

                    if(ConferenceCommand.CreateJoin(sessionsListSync, locationIdListListSync, mecStressTest, 0, ref locationIdReturn, ref conferenceIdReturn) == true)
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
                        mecStressTest.ErrorOutputText = " failed to add a location";
                        return false;
                    }
                }
            }
            
            // Allow conference to sit for specified time
            blockForDurationOfTest.WaitOne(durationOfTest, false);

            // Pull out from the locationListList the locationList for conference 0 (since there is only one conference)
            ArrayList tempLocationIdList = (ArrayList) locationIdListListSync[0];

            // Kick all participants
            // This particular line of code ensures that in the case that an end test signal was sent,
            // we base our number of kicks on how many connections were actually made
            int numberOfConnections = NumberOfLiveConnections;
            for(int i = 0; i < numberOfConnections; i++)
            {
                // Wait the user-determined amount of time
                System.Threading.Thread.Sleep(periodBetweenHangups);

                mecStressTest.DebugOutputText = "\n\nAttempting to kick a participant...";

                if(ConferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, (string) tempLocationIdList[i], 0) == true)
                {
                    progressBar.Step = 1;
                    overallProgressBar.Step = 1;
                    NumberOfLiveConnections--;
                    mecStressTest.DebugOutputText = " kicked participant";
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
                    mecStressTest.ErrorOutputText = " failed to kick participant";
                    return false;
                }
            }
            NumberOfLiveConferences--;
            locationIdList.Clear();
            locationIdListListSync.Clear();
            sessionsListSync.Clear();

            return true;
        }

        public bool StartTestRamp()
        {

            for(int conferenceIteration = 0; conferenceIteration < maximumConnections; conferenceIteration++)
            {
                if( !endOfTest )
                {
                    mecStressTest.DebugOutputText = "\n\nAttempting to create an initial conference...";

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
                        mecStressTest.ErrorOutputText = " failed to create an initial conference";
                        return false;
                    }


                    for(int i = 1; i < conferenceIteration + 1; i++)
                    {
                        // Wait the user-determined amount of time
                        System.Threading.Thread.Sleep(periodBetweenCalls);

                        if( !endOfTest )
                        {
                            mecStressTest.DebugOutputText = "\n\nAttempting to add a conference participant...";                     

                            if(ConferenceCommand.CreateJoin(sessionsListSync, locationIdListListSync, mecStressTest, 0, ref locationIdReturn, ref conferenceIdReturn) == true)
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
                                mecStressTest.ErrorOutputText = " failed to add participant";
                                return false;
                            }
                        }
                    }

            
                    //Allow conference to sit for specified time
                    blockForDurationOfTest.WaitOne(durationOfTest, false);


                    // Kick all participants
                    // Ensure that we are kicking the right number of participants
                    int numberOfConnections = NumberOfLiveConnections;
                    
                    ArrayList tempLocationIdList = (ArrayList) locationIdListListSync[0];

                    for(int i = 0; i < numberOfConnections; i++)
                    {
                        mecStressTest.DebugOutputText = "\n\n Attempting to kick a participant...";
                        // Wait the user-determined amount of time
                        System.Threading.Thread.Sleep(periodBetweenHangups);

                        if(ConferenceCommand.KickParticipant(sessionsListSync, locationIdListListSync, mecStressTest, (string) tempLocationIdList[i], 0) == true)
                        {
                            progressBar.Step = 1;
                            overallProgressBar.Step = 1;
                            NumberOfLiveConnections--;
                            
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
                            mecStressTest.ErrorOutputText = " failed to kick participant";
                            return false;
                        }
                    }
                }

                NumberOfLiveConnections = 0;
                NumberOfLiveConferences = 0;
                locationIdList.Clear();
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
