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
using System.Web;

using Metreos.Common.Mec;
using Metreos.Core;
using WebMessage = Metreos.Common.Mec;

namespace Metreos.Mec.StressApp
{
    /// <summary>
    /// The implementation of a second version of randomized conference
    /// </summary>
    public class RandomizedConference2 : TestBase
	{
        public delegate bool StartTestDelegate();
        public ConferenceManager conferenceManager;
        public StartTestDelegate startTestDelegate;
        public AutoResetEvent blockForDurationOfTest;
		public IMecTester testInterface;
        public Random rand;
        public StressTesting.Settings settings;

        public TimeSpan periodBetweenCalls;
        public TimeSpan periodBetweenHangups;
        public TimeSpan durationOfTest;
        public int connectionIntensity;
        public int averagePeriodBetweenCalls;
        public int averageSpike;
        public AutoResetEvent testIsDoneEvent;
        public int desiredConnections;
        public int desiredConferences;
        public int forcedSkewConnections;
        // Test Runtime data tables

        public RandomizedConference2(IMecTester testInterface, StressTesting.Settings settings) : base()
        {
            this.testInterface = testInterface;
            this.settings = settings;
            rand = new Random((int) DateTime.Now.Ticks);
            blockForDurationOfTest = new AutoResetEvent(false);
	        
            this.periodBetweenCalls = new TimeSpan(0,0,0, settings.minimumTimeBetweenCalls, 0);
            this.periodBetweenHangups = new TimeSpan(0,0,settings.testTime, 0, 0);
            this.maximumNumberOfConferences = settings.maximumConferences;
            this.maximumNumberOfConnections = settings.maximumConnections;
            this.connectionIntensity = settings.initialIntensity;
            this.averagePeriodBetweenCalls = settings.averageCallInterval;
            this.averageSpike = settings.averageSpike;

            startTestDelegate = new StartTestDelegate( RunTest );
            
            conferenceManager = new ConferenceManager(testInterface, this.maximumNumberOfConnections, this.maximumNumberOfConferences, this.averageSpike, this.settings);
		}
        
        public void RecomputeInitialIntensity()
        {
            this.desiredConnections = (int) ((float)maximumNumberOfConnections * (float)((float) connectionIntensity / (float) 21));
            
            // Ensure that we have at least 1 connections
            if(desiredConnections < 1)
            {
                desiredConnections = 1;
            }
            this.desiredConferences = (int) ((float)maximumNumberOfConferences * (float)((float) connectionIntensity / (float) 21));
            
            // Ensure that we have at least 1 conference desired
            if(desiredConferences < 1)
            {
                desiredConferences = 1;
            }
        }

        public bool RunTest()
        {
            Reset(); 

            RecomputeInitialIntensity();
            if(!conferenceManager.Initialize(this.desiredConnections, this.desiredConferences))
                return false;

            conferenceManager.Start();

            // Program hangs at this line of code for durationOfTest
            blockForDurationOfTest.WaitOne(new TimeSpan(0, 0, settings.testTime, 0, 0), false);
    
            conferenceManager.Shutdown();

            return true;
        }

        public override bool Start()
        {
            return startTestDelegate();
        }

        public override bool EndTest()
        {
            blockForDurationOfTest.Set();
            return true;
        }

        public bool StartTestRandom()
        {
            return true;
        }
        
    }
}
