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

using Metreos.Core;
using WebMessage = Metreos.Common.Mec;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for ConferenceManager.
	/// </summary>
    /// <summary>
    /// Handles the managament of resources between conferences, and the creation and destruction of conferences
    /// </summary>
    public class ConferenceManager : IConferenceCallbacks
    {       
        private StressTesting.Settings settings;
        private AutoResetEvent pendingCreateBlocker;
        private int numberLiveConferences;
        private int numberLiveConnections;
        private int pendingCreates;
        private int pendingJoins;
        private int pendingKicks;
        private int pendingBulkJoins;
        private int checkOutConferences;
        private int checkOutConnections;
        private IMecTester testInterface;
        private object updateViewerLock;
        private object pendingCreateLock;
        private object pendingJoinLock;
        private object pendingKickLock;
        private object pendingBulkJoinLock;
        private object runningConferencesLock;
        private object numberLiveConnectionsLock;
        private object numberLiveConferencesLock;
        private object freeConnectionLock;
        private object checkOutLock;
        private object checkOutConferencesLock;
        private object checkOutConnectionsLock;

        private System.Timers.Timer createConference;
        private System.Timers.Timer destroyConference;

        private Random rand;
            
        private ArrayList runningConferences;
        private ArrayList RunningConferences;

        private StressTesting.RangeQueue rangeQueue;

        #region Properties
        public int PendingJoins
        {
            set
            {
                lock(pendingJoinLock)
                {
                    pendingJoins = value;
                }
            }

            get
            {
                lock(pendingJoinLock)
                {
                    return pendingJoins;
                }
            }
        }

        public int PendingCreates
        {
            set
            {
                lock(pendingCreateLock)
                {
                    pendingCreates = value;
                }
            }

            get
            {
                lock(pendingCreateLock)
                {
                    return pendingCreates;
                }
            }
        }

        public int PendingKicks
        {
            set
            {
                lock(pendingKickLock)
                {
                    pendingKicks = value;
                }
            }

            get
            {
                lock(pendingKickLock)
                {
                    return pendingKicks;
                }
            }
        }

        public int PendingBulkJoins
        {
            set
            {
                lock(pendingBulkJoinLock)
                {
                    pendingBulkJoins = value;
                }
            }

            get
            {
                lock(pendingBulkJoinLock)
                {
                    return pendingBulkJoins;
                }
            }
        }

        public int NumberLiveConnections
        {
            set
            {
                lock(numberLiveConnectionsLock)
                {
                    numberLiveConnections = value;
                }
            }
            get
            {
                lock(numberLiveConnectionsLock)
                {
                    return numberLiveConnections;
                }
            }
        }

        public int NumberLiveConferences
        {
            set
            {
                lock(numberLiveConferencesLock)
                {
                    numberLiveConferences = value;
                }
            }
            get
            {
                lock(numberLiveConferencesLock)
                {
                    return numberLiveConferences;
                }
            }
        }

        public int CheckOutConferences
        {
            set
            {
                lock(checkOutConferencesLock)
                {
                    checkOutConferences = value;
                }
            }
            get
            {
                lock(checkOutConferencesLock)
                {
                    return checkOutConferences;
                }
            }
        }

        public int CheckOutConnections
        {
            set
            {
                lock(checkOutConnectionsLock)
                {
                    checkOutConnections = value;
                }
            }
            get
            {
                lock(checkOutConnectionsLock)
                {
                    return checkOutConnections;
                }
            }
        }
        #endregion Properties

        public ConferenceManager(IMecTester testInterface, int maximumNumberConnections, int maximumNumberConferences, int averageSpike, StressTesting.Settings settings)
        {
            this.testInterface = testInterface;

            RegisterWithInterface();

            rand = new Random((int) DateTime.Now.Ticks);
            pendingCreateBlocker = new AutoResetEvent(false);

            this.settings = settings;

            this.createConference = new System.Timers.Timer();
            this.createConference.Interval = rand.Next(10000, averageSpike * 1000);
            this.createConference.Elapsed += new System.Timers.ElapsedEventHandler(CreateConferenceElapsed);
            this.destroyConference = new System.Timers.Timer();
            this.destroyConference.Interval = rand.Next(10000, averageSpike * 1000);
            this.destroyConference.Elapsed += new System.Timers.ElapsedEventHandler(DestroyConferenceElapsed);   
        
            pendingCreates = 0;
            pendingJoins = 0;
            pendingKicks = 0;
            pendingBulkJoins = 0;
            numberLiveConnections = 0;
            numberLiveConferences = 0;
            checkOutConferences = 0;
            checkOutConnections = 0;

            this.runningConferences = new ArrayList();
            this.RunningConferences = ArrayList.Synchronized(runningConferences);
        
            this.pendingJoinLock = new object();
            this.pendingCreateLock = new object();
            this.pendingKickLock = new object();
            this.pendingBulkJoinLock = new object();
            this.updateViewerLock = new object();
            this.runningConferencesLock = new object();
            this.numberLiveConferencesLock = new object();
            this.numberLiveConnectionsLock = new object();
            this.freeConnectionLock = new object();
            this.checkOutLock = new object();
            this.checkOutConferencesLock = new object();
            this.checkOutConnectionsLock = new object();

            rangeQueue = new  StressTesting.RangeQueue(settings.numberOfRanges, settings.lowerBounds, settings.upperBounds);
        }

        #region Initialize(), Start(), Stop(), Reset()
        /// <summary>
        /// Entry point every time a "Start" is sent by the user
        /// </summary>
        /// <param name="initialNumberConnections"></param>
        /// <param name="initialNumberConferences"></param>
        /// <returns></returns>
        public bool Initialize(int initialNumberConnections, int initialNumberConferences)
        {
            Reset();

            int numberExtraConferencesToCreate = 0;
            int numberExtraConnectionsToCreate = 0;

            if(IsFreeLocation() && IsFreeConference())
            {
                testInterface.UpdateVerboseInfo("Creating the initial conferences");
            }
            else
            {
                testInterface.UpdateErrorInfo("Unable to start random test because there are no free connections or conferences");
                return false;
            }
            
            // for instance, say 8 initial - (2 live + 1 given + 1 PendingCreateConferences) = 4 conferences left to make
            numberExtraConferencesToCreate = rand.Next(0, initialNumberConferences - (NumberLiveConferences + 1 + PendingCreates));
            
            int numberUsedConnections = this.PendingJoins;
            numberUsedConnections += this.PendingBulkJoins;
            numberUsedConnections += this.NumberLiveConnections;

            // check that the randomly chosen number of conferences doesn't exceed the number of used connections
            if(numberExtraConferencesToCreate > settings.maximumConnections - numberUsedConnections)
            {
                numberExtraConferencesToCreate = settings.maximumConnections - numberUsedConnections;
            }
                
            numberExtraConnectionsToCreate = rand.Next(0, initialNumberConnections - (numberExtraConferencesToCreate + 1 - NumberLiveConnections + PendingJoins +  PendingBulkJoins));

            int[] conferencesAddQueue = new int[numberExtraConferencesToCreate + 1];
            
            for(int i = 0; i < numberExtraConnectionsToCreate; i++)
            {
                int whichConferenceToQueueTo = rand.Next(0, numberExtraConferencesToCreate + 1);

                conferencesAddQueue[whichConferenceToQueueTo]++;
            }
         
            lock(runningConferencesLock)
            {
                for(int i = 0; i < numberExtraConferencesToCreate + 1; i++)
                {
                    Conference conference = new Conference(testInterface, (IConferenceCallbacks)this, settings, ref pendingCreateBlocker, rangeQueue, true, true);
                    
                    for(int j = 0; j < conferencesAddQueue[i]; j++)
                    {
                           conference.AddsQueue.Enqueue("-1");
                    }

                    if(!conference.CreateConferenceTunnel())
                    {
                        testInterface.UpdateErrorInfo("Unable to create an initial auto-test conference");
                    }
                    else
                    {
                        RunningConferences.Add(conference);
                    }

                    Thread.Sleep(settings.minimumTimeBetweenCalls * 1000);
                }
            }         
            
            testInterface.UpdateVerboseInfo("Finished creating the initial conferences");

            return true;
        }

        /// <summary>
        /// Occurs after initialization
        /// </summary>
 
        public void Start()
        {
            this.createConference.Start();
            this.destroyConference.Start();
        }

        /// <summary>
        /// Occurs when "Stop" is sent by user, or timeout of test occurs
        /// </summary>
        public void Shutdown()
        {
            this.createConference.Stop();
            this.destroyConference.Stop();

            lock(runningConferencesLock)
            {
                for(int i = 0; i < RunningConferences.Count; i++)
                {
                    ((Conference) RunningConferences[i]).EndConference();
                }
            }
        }

        public void Reset()
        {
        /// hum...
        }

        #endregion Initialize(), Start(), Stop(), Reset()

        #region Timers
        public void CreateConferenceElapsed(object o, System.Timers.ElapsedEventArgs args)
        {
            createConference.Interval = rand.Next(10000, settings.averageSpike * 1000);

            Conference conference;

            lock(runningConferencesLock)
            {
                conference = new Conference(testInterface, (IConferenceCallbacks)this, settings, ref pendingCreateBlocker, rangeQueue, false, true); 

                if(!conference.CreateConferenceTunnel())
                {
                    testInterface.UpdateVerboseInfo("Create Conference timer elasped:  unable to initialize conference");
                    return;
                }
                else
                {
                    RunningConferences.Add(conference);
                }
            }

            // This is just a rough guess at how many open connects there are.  Though it may go over, it will be blocked inside the join tunnel
            int numberConnectionsToAdd = rand.Next(0, settings.maximumConnections - (PendingJoins + NumberLiveConnections));

            
            for(int i = 0; i < numberConnectionsToAdd; i++ )
            {
                Thread.Sleep(rand.Next(0, settings.averageCallInterval * 1000));

                    if(!conference.AddConnectionTunnel())
                    {
                        testInterface.UpdateErrorInfo("Create Conference timer elasped: failure in adding a connection");
                        return;
                    }
                    else
                    {
                    }
            }
        }

        public void DestroyConferenceElapsed(object o, System.Timers.ElapsedEventArgs args)
        {
            destroyConference.Interval = rand.Next(10000, settings.averageSpike * 1000);

            int destroyThisConference;

            if(!GrabEndableConference(out destroyThisConference))
            {
                testInterface.UpdateVerboseInfo("Destroy Conference timer elasped: no running conferences to destroy");
                return;
            }
            try
            {
                ((Conference) RunningConferences[destroyThisConference]).EndConference();
            }
            catch
            {
                // this is rather hard to not possibly have an exception, because responses from kicks can cause this array to shrink, throwing a array out of index exception.
                testInterface.UpdateVerboseInfo("Destined conference to destroy has disappeared from an internal conference's list. Abnormal, but not an indication of a true error");
            }

        }

        #endregion Timers

        #region Utilities
        public bool GrabEndableConference(out int runningConference)
        {
            runningConference = -1;

            ArrayList runningConferencesShortened = new ArrayList();

            lock(runningConferencesLock)
            {
                for(int i = 0; i < RunningConferences.Count; i++)
                {
                    if( ((Conference) RunningConferences[i]).STATUS != Conference.status.lost)
                    {
                        runningConferencesShortened.Add(i);
                    }
                }
            }

            if(runningConferencesShortened.Count == 0)
            {
                return false;
            }

            int whichConference = rand.Next(0, runningConferencesShortened.Count);

            runningConference = (int) runningConferencesShortened[whichConference];

            return true;
        }   
        #endregion Utilities 

        #region User Event Handlers
        public void CreateConference(string phoneNumber, bool allowRandom)
        {
            lock(runningConferencesLock)
            {

                if(phoneNumber == "-1")
                {

                    Conference conference = new Conference(testInterface, (IConferenceCallbacks)this, settings, ref pendingCreateBlocker, rangeQueue, true, allowRandom);

                    if(!conference.CreateConferenceTunnel())
                    {
                        testInterface.UpdateErrorInfo("Unable to create conference:  failure to initialize");
                        return;
                    }
                    else
                    {
                        RunningConferences.Add(conference);
                    }

                }
                else
                {
                    Conference conference = new Conference(testInterface, (IConferenceCallbacks)this, settings, ref pendingCreateBlocker, rangeQueue, true, allowRandom);

                    try
                    {
                        if(!conference.CreateConferenceTunnel(phoneNumber))
                        {
                            testInterface.UpdateErrorInfo("Unable to create conference:  failure to initialize");
                            return;
                        }
                        else
                        {
                            RunningConferences.Add(conference);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void EndConference(string conferenceId)
        {
            lock(runningConferencesLock)
            {
                for(int i = 0; i < RunningConferences.Count; i++)
                {
                    if(( (Conference) RunningConferences[i]).conferenceId == conferenceId) 
                    {
                        ((Conference) RunningConferences[i]).EndConference();
                        return;
                    }
                }
            }

            testInterface.UpdateErrorInfo("Unable to find a conference of that conference Id");
        }

        public void JoinLocation(string conferenceId, string phoneNumber)
        {
            lock(runningConferencesLock)
            {
                if(phoneNumber == "-1")
                {
                    for(int i = 0; i < RunningConferences.Count; i++)
                    {
                        if( ((Conference) RunningConferences[i]).conferenceId == conferenceId)
                        {
                            if(!((Conference) RunningConferences[i]).AddConnectionTunnel())
                            {
                                testInterface.UpdateErrorInfo("Join Location: unable to add a location");
                            }

                            return;
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < RunningConferences.Count; i++)
                    {
                        if(((Conference) RunningConferences[i]).conferenceId == conferenceId)
                        {
                            if(!((Conference) RunningConferences[i]).AddConnectionTunnel(phoneNumber))
                            {
                                testInterface.UpdateErrorInfo("Join Location: unable to add a location");
                            }
                            return;
                        }
                    }
                }
            }

            testInterface.UpdateErrorInfo("Join Location:  unable to find a conference of that conference Id");
        }

        public void KickLocation(string locationGuid, string conferenceId, string locationId)
        {
            lock(runningConferencesLock)
            {
                for(int i = 0; i < RunningConferences.Count; i++)
                {
                    if(((Conference) RunningConferences[i]).conferenceId == conferenceId)
                    {   
                        ((Conference) RunningConferences[i]).RemoveConnectionTunnel(locationId, locationGuid);
                        return;
                    }
                }
            }

            testInterface.UpdateErrorInfo("Kick Location:  unable to find a conference of that conference Id");
        }

        public void MuteLocation(string locationGuid, string conferenceId, string locationId)
        {
            lock(runningConferencesLock)
            {
                for(int i = 0; i < RunningConferences.Count; i++)
                {
                    if(((Conference) RunningConferences[i]).conferenceId == conferenceId)
                    {
                        ((Conference) RunningConferences[i]).MuteConnection(locationGuid, locationId);
                        return;
                    }
                }
            }

            testInterface.UpdateErrorInfo("Mute Location:  unable to find a conference of that conference Id");
        }

        public void TerminateAllLocations()
        {
            lock(runningConferencesLock)
            {
                for(int i = 0; i < RunningConferences.Count; i++)
                {
                    ((Conference) RunningConferences[i]).EndConference();
                }
            }
        }

        #endregion User Event Handlers

        #region IConferenceCallbacks Members

        public void UpdatePendingJoins(int newValue)
        {
            //lock(runningConferencesLock)
            //{
                PendingJoins += newValue;
            //}
        }

        public void UpdatePendingCreates(int newValue)
        {
            //lock(runningConferencesLock)
            //{
                PendingCreates += newValue;
            //}
        }

        public void UpdatePendingKicks(int newValue)
        {
            //lock(runningConferencesLock)
            //{
                PendingKicks += newValue;
            //}
        }

        public void UpdateLiveConnections(int newValue)
        {
            //lock(runningConferencesLock)
            //{
                this.NumberLiveConnections += newValue;
            //}
        }

        public void UpdateLiveConferences(int newValue)
        {
            //lock(runningConferencesLock)
            //{
                this.NumberLiveConferences += newValue;
            //}
        }

        public int QueryPendingJoins()
        {
            //lock(runningConferencesLock)
            //{
                return PendingJoins + PendingBulkJoins;
            //}
        }

        public int QueryPendingCreates()
        {
            //lock(runningConferencesLock)
            //{
                return PendingCreates;
            //}
        }

        public int QueryPendingKicks()
        {
            //lock(runningConferencesLock)
            //{
                return PendingKicks;
            //}
        }

        public int QueryLiveConnections()
        {
            //lock(runningConferencesLock)
            //{
                return NumberLiveConnections;
            //}
        }

        public void RemoveMe(Conference me)
        {
            lock(runningConferencesLock)
            {
                RunningConferences.Remove(me);
            }            
        }

        public bool IsFreeLocation()
        {
            lock(checkOutLock)
            {
                if(this.PendingJoins + this.NumberLiveConnections + CheckOutConnections <= settings.maximumConnections)
                {
                    CheckOutConnections++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        } 

        public void ReturnCheckOutLocation()
        {
            CheckOutConnections--;
        }
   
        public bool IsFreeConference()
        {
            lock(checkOutLock)
            {
                if(PendingCreates + NumberLiveConferences + CheckOutConferences <= settings.maximumConferences)
                {
                    CheckOutConferences++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ReturnCheckOutConference()
        {
            CheckOutConferences--;
        }
        #endregion

        #region Registering with IMecTester
        public void RegisterWithInterface()
        {
            testInterface.RegisterCreateConference(new IStressTesting.CreateConferenceDelegate( CreateConference ));
            testInterface.RegisterEndConference( new IStressTesting.EndConferenceDelegate( EndConference ));
            testInterface.RegisterJoinLocation( new IStressTesting.JoinLocationDelegate( JoinLocation ));
            testInterface.RegisterKickLocation( new IStressTesting.KickLocationDelegate( KickLocation ));
            testInterface.RegisterMuteLocation( new IStressTesting.MuteLocationDelegate( MuteLocation ));
            testInterface.RegisterTerminateAll( new IStressTesting.TerminateAllDelegate( TerminateAllLocations ));
        }
        #endregion Registering with IMecTester
    }
}
