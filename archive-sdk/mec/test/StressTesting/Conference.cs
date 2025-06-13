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

using MySql.Data.MySqlClient;

using Metreos.Core;
using Metreos.Utilities;
using WebMessage = Metreos.Common.Mec;
using Metreos.ApplicationSuite.Storage;
using MecParticipants = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.MecParticipants;


namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for Conference.
	/// </summary>
    public class Conference : IHttpCallbacks
    {
        public const string DbUsername = "root";
        public const string DbPassword = "metreos";

        public Hashtable lookUpPhoneNumbersTable;
        public AutoResetEvent endOfConferenceBlock;
        public AutoResetEvent pendingCreateBlocker;
        public Queue AddsQueue;
 
        private StressTesting.Settings settings;
        private StressTesting.RangeQueue rangeQueue;

        private int pendingJoins;
        private int liveConnections;
        
        private ArrayList pendingKicks;
        private ArrayList PendingKicks;
        private string conferenceGuid;
        private bool lastLocationFound;
        private bool randomAllowed;
        private object locationIdListLock;
        private object pendingKickLock;
        private object pendingJoinsLock;
        private object endingLock;
        private object tunnelLock;
        private object liveConnectionsLock;
        private object databaseLock;

        public IMecTester testInterface;
        public IConferenceCallbacks conferenceInterface;

        public AsynchronousHttp asyncHttp;

        private System.Timers.Timer joinTimer;
        private System.Timers.Timer kickTimer;

        private System.Timers.Timer spikeJoinTimer;
        private System.Timers.Timer spikeKickTimer;

        private Random randomNumber;

        private ArrayList locationIdList;
        private ArrayList LocationIdList;
        private Hashtable alreadyKickedList;
        private Hashtable AlreadyKickedList;

        private string sessionId;

        private MySqlConnection connection;

        public status STATUS;

        public bool userConference;
        private bool hadAtLeastOneSuccessfulConnect;
        public string conferenceId;

        #region Class Enums/Structs
        public enum status
        {
            ended,
            initializing,
            online,
            lost
        }

        public enum locationStatus
        {
            initializing, 
            online,
            kicking,
            kicked,
            lost,
            muted,
            unmuted,
            muteFailed
        }

        public class LocationGuids
        {
            public string locationGuid;
            public string locationId;
            public locationStatus STATUS;
            public bool pendingDestroy;
            public bool isMuted;

            public LocationGuids(string locationGuid, string locationId)
            {
                this.locationGuid = locationGuid;
                this.locationId = locationId;
                this.STATUS = locationStatus.initializing;
                this.pendingDestroy = false;
                this.isMuted = false;

            }
        }

        #endregion Class Enums/Structs

        #region Properties
        private int LiveConnections
        {
            set
            {
                lock(liveConnectionsLock)
                {
                    liveConnections = value;
                }
            }
            get
            {
                lock(liveConnectionsLock)
                {
                    return liveConnections;
                }
            }
        }
        public int PendingJoins
        {
            set
            {
                lock(pendingJoinsLock)
                {
                    pendingJoins = value;
                }
            }

            get
            {
                lock(pendingJoinsLock)
                {
                    return pendingJoins;
                }
            }
        }

        #endregion Properties

        public Conference(IMecTester testInterface, IConferenceCallbacks conferenceInterface, StressTesting.Settings settings, ref AutoResetEvent pendingCreateBlocker, StressTesting.RangeQueue rangeQueue, bool userConference, bool randomAllowed)
        {
            this.testInterface = testInterface;
            this.conferenceInterface = conferenceInterface;
            this.settings = settings;
            this.pendingCreateBlocker = pendingCreateBlocker;
            this.endOfConferenceBlock = new AutoResetEvent(false);
            this.rangeQueue = rangeQueue;
            this.userConference = userConference;
            this.randomAllowed = randomAllowed;
            lastLocationFound = false;
            locationIdListLock = new object();
            pendingKickLock = new object();
            pendingJoinsLock = new object();
            endingLock = new object();
            tunnelLock = new object();
            liveConnectionsLock = new object();
            databaseLock = new object();
            
            conferenceGuid = ConferenceGuidFactory.GetConferenceGuid();
            pendingJoins = 0;
            liveConnections = 0;
            conferenceId = "N/A";
            sessionId = "N/A";
            AddsQueue = new Queue();
            pendingKicks = new ArrayList();
            PendingKicks = ArrayList.Synchronized(pendingKicks);

            randomNumber = new Random((int) DateTime.Now.Ticks);

            kickTimer = new System.Timers.Timer(randomNumber.Next(10000, settings.averageLittleSpike * 1000));
            joinTimer = new System.Timers.Timer(randomNumber.Next(10000, settings.averageLittleSpike * 1000));
            spikeJoinTimer = new System.Timers.Timer(randomNumber.Next(10000, settings.averageLittleSpike * 1000));
            spikeKickTimer = new System.Timers.Timer(randomNumber.Next(10000, settings.averageLittleSpike * 1000));

            joinTimer.Elapsed += new System.Timers.ElapsedEventHandler(JoinTimerElapsed);
            kickTimer.Elapsed += new System.Timers.ElapsedEventHandler(KickTimerElapsed);
            spikeJoinTimer.Elapsed += new System.Timers.ElapsedEventHandler(SpikeJoinTimerElapsed);
            spikeKickTimer.Elapsed += new System.Timers.ElapsedEventHandler(SpikeKickTimerElapsed);


            STATUS = status.initializing;

            locationIdList = new ArrayList();
            LocationIdList = ArrayList.Synchronized(locationIdList);
            alreadyKickedList = new Hashtable();
            AlreadyKickedList = Hashtable.Synchronized(alreadyKickedList);

            lookUpPhoneNumbersTable = new Hashtable();

            asyncHttp = new AsynchronousHttp((IHttpCallbacks)this, testInterface, settings);

            hadAtLeastOneSuccessfulConnect = false;
        }               

        #region Start(), EndConference()

        public void Start()
        {
            joinTimer.Start();
            kickTimer.Start();
            spikeJoinTimer.Start();
            spikeKickTimer.Start();
        }

        public void EndConference()
        {
            MarkForShutdown();

            string[] locationIds;
            string[] locationGuids;
            int numberTimesToKick;

            lock(locationIdListLock)
            {
                numberTimesToKick = LocationIdList.Count;
                locationIds = new string[LocationIdList.Count];
                locationGuids = new string[LocationIdList.Count];

                for(int i = 0; i < LocationIdList.Count; i++)
                {
                    LocationGuids temp = (LocationGuids)LocationIdList[i];
                    locationIds[i] = temp.locationId;
                    locationGuids[i] = temp.locationGuid;
                }

            }

            for(int i = 0; i < numberTimesToKick; i++)
            {
                RemoveConnectionTunnel(locationIds[i], locationGuids[i]);

                // Helps with slower comps
                Thread.Sleep(50);
            }          
        }
        #endregion Start(), EndConference()

        #region Utilities

        public bool GetRemotePartyNumber(out string remotePartyNumber, out string purePhoneNumber)
        {
            remotePartyNumber = null;

            purePhoneNumber = "-1";

            if(settings.chooseSim)
            {
                string phoneNumber;

                if(rangeQueue.GrabANumber(out phoneNumber))
                {
                    remotePartyNumber = phoneNumber.ToString();
                    purePhoneNumber = phoneNumber;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if(settings.allowRoute)
                {
                    purePhoneNumber = settings.routingNumber;
                    remotePartyNumber = purePhoneNumber;
                }
                else
                {
                    purePhoneNumber = DateTime.Now.Ticks.ToString();
                    remotePartyNumber = purePhoneNumber;
                }

                return true;
            }

        }

        public bool IsLastLocation()
        {
            if(lastLocationFound == false)
            {
                if((LocationIdList.Count - PendingKicks.Count < 1) && PendingJoins <= 0)
                {
                    lastLocationFound = true;

                    return true;
                }
                else
                {
                    return false;
                }
        
            }
            else
            {
                MarkForShutdown();
                return false;
            }
        }

        public void AddLocationId(string locationGuid, string phoneNumber)
        {
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(phoneNumber != null, "phoneNumber is null");

            LocationGuids location = new LocationGuids(locationGuid, null);
            
            lock(locationIdListLock)
            {
                LocationIdList.Add(location);
            }

            lookUpPhoneNumbersTable[locationGuid] = phoneNumber;
        }

        public bool LinkLocationId(string locationGuid, string locationId, string remotePartyNumber)
        {
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(remotePartyNumber != null, "remotePartyNumber is null");

            lock(locationIdListLock)
            {
                for(int i = 0; i < LocationIdList.Count; i++)
                {
                    if(((LocationGuids)LocationIdList[i]).locationGuid == locationGuid)
                    {
                        LocationGuids temp = (LocationGuids )LocationIdList[i];
                        temp.locationId = locationId;
                        temp.STATUS = locationStatus.online;
                        LocationIdList[i] = temp;
                        LiveConnections++;
                        return temp.pendingDestroy;
                    }
                }
            }

            return false;
        }

        public bool RemoveLocationGuid(string locationGuid)
        {
            if(STATUS != status.lost)
            {
                Debug.Assert(locationGuid != null, "locationGuid is null");

                lock(locationIdListLock)
                {
                    for(int i = 0; i < LocationIdList.Count; i++)
                    {
                        if(((LocationGuids) LocationIdList[i]).locationGuid == locationGuid)
                        {
                            try
                            {
                                LocationIdList.RemoveAt(i);
                                LiveConnections--;
                            }
                            catch
                            {
                                testInterface.UpdateErrorInfo("Critcal error at RemoveLocationGuid");
                            }
                        }
                    } 
   
                }

                string phoneNumber = (string) lookUpPhoneNumbersTable[locationGuid];

                rangeQueue.ReturnANumber(phoneNumber);

                try
                {
                    lookUpPhoneNumbersTable.Remove(locationGuid);
                }
                catch
                {
                    testInterface.UpdateErrorInfo("Unable to remove locationGuid from phoneNumber table");
                }

                // Check to see if its last connection
                if(LocationIdList.Count == 0 && PendingJoins == 0)
                {  
                    testInterface.UpdateVerboseInfo("Conference " + conferenceId + " ended");
  
                    if(hadAtLeastOneSuccessfulConnect)
                    {
                        conferenceInterface.UpdateLiveConferences(-1); 
                        testInterface.UpdateLiveConferences(-1);
                    }

                    MarkForShutdown();

                    testInterface.RemoveConference(conferenceGuid);

                    conferenceInterface.RemoveMe(this);

                    return true;
                }  
                return false;
            }
            return false;
        }

        public void AddToPendingKicks(string locationId)
        {
            Debug.Assert(locationId != null, "locationId is null");

            PendingKicks.Add(locationId);
        }
    
        public bool RemoveFromPendingKicks(string locationId)
        {
            Debug.Assert(locationId != null, "locationId is null");

            if(PendingKicks.Contains(locationId))
            {
                PendingKicks.Remove(locationId);
                return true;

            }
            else
            {
                testInterface.UpdateErrorInfo("Already attempted to remove this locationId of: " + locationId + " and conferenceId: " + this.conferenceId + ".");
                return false;
            }
        }

        public void AddToPendingJoins()
        {
            PendingJoins++;
        }

        public void RemoveFromPendingJoins()
        {
            PendingJoins--;
        }

        public void AddToPendingCreates()
        {
            //this.STATUS = status.initializing;
        }

        public void RemoveFromPendingCreates()
        {
            pendingCreateBlocker.Set();
        }

        public void MarkForShutdown()
        {
          
            connection = null;

            this.STATUS = status.ended;

            joinTimer.Stop();
            kickTimer.Stop();
            spikeJoinTimer.Stop();
            spikeKickTimer.Stop();
        }

        public void ReadAddConnectionQueue()
        {
            for(int i = 0; i < AddsQueue.Count; i++)
            {
//                if(conferenceInterface.QueryPendingJoins() + conferenceInterface.QueryLiveConnections() < settings.maximumConnections)
//                {
                    string phoneNumber = (string)AddsQueue.Dequeue();
                    if(phoneNumber == "-1")
                    {
                        AddConnectionTunnel();
                        
                    }
                    else
                    {
                        AddConnectionTunnel(phoneNumber);
                    }
//                }
//                else
//                {
                    // additional adds simply get dropped, if there are no open connections
//                    testInterface.UpdateVerboseInfo("Can not add more connections to conference at this time. Maximum number of connections has been reached");
//                    if(randomAllowed == true)
//                    {
//                        this.Start();
//                    }
                    
//                }
                Thread.Sleep(settings.minimumTimeBetweenCalls * 1000);
            }

            if(randomAllowed == true)
            {
                this.Start();
            }
        }

        public void JoinReturned404(string locationGuid)
        {
            STATUS = status.lost;

            this.kickTimer.Stop();
            this.joinTimer.Stop();
            this.spikeJoinTimer.Stop();
            this.spikeKickTimer.Stop();

            RemoveFromPendingJoins();

            conferenceInterface.UpdatePendingJoins(-1);

            testInterface.UpdatePendingConnections(-1);
            testInterface.UpdateFailedConnections(1);

            testInterface.UpdateErrorInfo("Pending Join in conference " + conferenceId + " can not succeed because 404 returned by Join Request");

            testInterface.RemoveLocation(conferenceGuid, locationGuid);

            lock(locationIdListLock)
            {
                for(int i = 0; i < LocationIdList.Count; i++)
                {
                    LocationGuids temp = (LocationGuids) LocationIdList[i];
                    if(temp.locationGuid == locationGuid)
                    {
                        LocationIdList.RemoveAt(i);
                        LiveConnections--;
                    }

                    if(temp.locationId != null)
                    {
                        testInterface.UpdateEstablishedLocation(conferenceGuid, temp.locationGuid, conferenceId, temp.locationId, status.lost.ToString());
                        testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);
                    }
                }    
            }
        }

        public void KickReturned404(string locationGuid)
        {
            STATUS = status.lost;

            this.kickTimer.Stop();
            this.joinTimer.Stop();
            this.spikeJoinTimer.Stop();
            this.spikeKickTimer.Stop();

            RemoveFromPendingJoins();

            conferenceInterface.UpdatePendingKicks(-1);

            testInterface.UpdateFailedKicks(1);

            testInterface.UpdateErrorInfo("Pending Kick in conference " + conferenceId + " can not succeed because 404 returned by Kick Request");

            lock(locationIdListLock)
            {
                for(int i = 0; i < LocationIdList.Count; i++)
                {
                    LocationGuids temp = (LocationGuids) LocationIdList[i];

                    if(temp.locationGuid == locationGuid)
                    {
                        temp.STATUS = locationStatus.lost;
                        testInterface.UpdateEstablishedLocation(conferenceGuid, temp.locationGuid, conferenceId, temp.locationId, locationStatus.lost.ToString());
                        testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);
                    }

                    if(temp.locationId != null)
                    {
                        testInterface.UpdateEstablishedLocation(conferenceGuid, temp.locationGuid, conferenceId, temp.locationId, locationStatus.lost.ToString());
                        testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);
                    }
                }    
            }  
        }

     

        #endregion Utilities

        #region Send Async Http Routines

        public bool CreateConference()
        {
            string phoneNumber;
            string remotePartyNumber;

            string locationGuid = LocationGuidFactory.GetLocationGuid();

            if(this.GetRemotePartyNumber(out remotePartyNumber, out phoneNumber))
            {

            }
            else
            {
                return false;
            }


            if(asyncHttp.Initialize(locationGuid, phoneNumber, remotePartyNumber))
            {
                AddToPendingCreates();
                AddToPendingJoins();

                conferenceInterface.UpdatePendingCreates(1);
                conferenceInterface.UpdatePendingJoins(1);
                testInterface.UpdatePendingConferences(1);
                testInterface.UpdatePendingConnections(1);
                testInterface.NewConference(conferenceGuid, locationGuid, this.STATUS.ToString(), phoneNumber);

                AddLocationId(locationGuid, phoneNumber);

                return true;
                
            }
            else
            {
                testInterface.UpdateErrorInfo("Unable to create a conference");
                testInterface.UpdateFailedConferences(1);
                testInterface.UpdateFailedConnections(1);

                rangeQueue.ReturnANumber(phoneNumber);

                return false;
            }
   
        }



        /// <summary>
        /// A create conference with a user specified phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public bool CreateConference(string phoneNumber)
        {
            string locationGuid = LocationGuidFactory.GetLocationGuid();

            string append;

            append = settings.callManagerIp;

            
            if(!rangeQueue.CheckNumber(phoneNumber))
            {
                testInterface.UpdateVerboseInfo("Phone number already in use");
                return false;
            }

            rangeQueue.ReserveANumber(phoneNumber);

            if(asyncHttp.Initialize(locationGuid, phoneNumber, phoneNumber))
            {
                AddToPendingCreates();
                AddToPendingJoins();

                conferenceInterface.UpdatePendingCreates(1);
                conferenceInterface.UpdatePendingJoins(1);
                testInterface.UpdatePendingConferences(1);
                testInterface.UpdatePendingConnections(1);
                testInterface.NewConference(conferenceGuid, locationGuid, this.STATUS.ToString(), phoneNumber);

                AddLocationId(locationGuid, phoneNumber);

                return true;
            }
            else
            {
                testInterface.UpdateErrorInfo("Unable to create a conference");
                testInterface.UpdateFailedConferences(1);
                testInterface.UpdateFailedConnections(1);

                rangeQueue.ReturnANumber(phoneNumber);

                return false;
            }
 
        }

        public bool CreateConferenceTunnel()
        {
            lock(tunnelLock)
            {
                if(conferenceInterface.IsFreeLocation())
                {
                    if(conferenceInterface.IsFreeConference())
                    {                      
                        if(STATUS != status.ended || STATUS != status.lost)
                        {
                            if(CreateConference())
                            {
                                conferenceInterface.ReturnCheckOutLocation();
                                conferenceInterface.ReturnCheckOutConference();
                                return true;
                            }
                            else
                            {
                                conferenceInterface.ReturnCheckOutLocation();
                                conferenceInterface.ReturnCheckOutConference();
                                return false;
                            }

                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("Create location dropped due to conference ending");
                            conferenceInterface.ReturnCheckOutLocation();
                            conferenceInterface.ReturnCheckOutConference();
                            return false;
                        }

                    }
                    else
                    {
                        testInterface.UpdateVerboseInfo("No free conferences");
                        conferenceInterface.ReturnCheckOutLocation();
                        return false;
                    }
                }  
                else
                {
                    testInterface.UpdateVerboseInfo("No free connections");
                    return false;
                }
            }
        }

        public bool CreateConferenceTunnel(string phoneNumber)
        {
            lock(tunnelLock)
            {
                if(conferenceInterface.IsFreeLocation())
                {
                    if(conferenceInterface.IsFreeConference())
                    {
                        if(rangeQueue.CheckNumber(phoneNumber))
                        {
                            if(STATUS != status.ended || STATUS != status.lost)
                            {
                                if(CreateConference(phoneNumber))
                                {
                                    conferenceInterface.ReturnCheckOutLocation();
                                    conferenceInterface.ReturnCheckOutConference();
                                    return true;
                                }
                                else
                                {
                                    conferenceInterface.ReturnCheckOutLocation();
                                    conferenceInterface.ReturnCheckOutConference();
                                    return false;
                                }
                            }
                            else
                            {
                                testInterface.UpdateVerboseInfo("Create location dropped due to conference ending");
                                conferenceInterface.ReturnCheckOutLocation();
                                conferenceInterface.ReturnCheckOutConference();
                                return false;
                            }
                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("Number already in use. Dropping request");
                            conferenceInterface.ReturnCheckOutLocation();
                            conferenceInterface.ReturnCheckOutConference();
                            return false;
                        }
                    }
                    else
                    {   testInterface.UpdateVerboseInfo("No free conferences");
                        conferenceInterface.ReturnCheckOutLocation();
                        return false;
                    }
                }  
                else
                {   
                    testInterface.UpdateVerboseInfo("No free connections");
                    return false;
                }
            }
        }

        public bool AddConnectionTunnel()
        {
            lock(tunnelLock)
            {
                if(conferenceInterface.IsFreeLocation())
                {
                    if(STATUS == status.initializing)
                    {
                        AddsQueue.Enqueue("-1");
                        testInterface.UpdateVerboseInfo("Queueing add location for after conference is initialized");
                        conferenceInterface.ReturnCheckOutLocation();
                        return true;
                    }

                    if(LiveConnections - PendingKicks.Count > 0)
                    {
                        if(STATUS != status.ended || STATUS != status.lost)
                        {
                            if(AddConnection())
                            {
                                conferenceInterface.ReturnCheckOutLocation();
                                return true;
                            }
                            else
                            {
                                conferenceInterface.ReturnCheckOutLocation();
                                return false;
                            }
                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("Join location dropped due to conference ending");
                            conferenceInterface.ReturnCheckOutLocation();
                            return true;
                        }
                    }
                    else
                    {
                        testInterface.UpdateVerboseInfo("Thwarted error code 8 for join");
                        conferenceInterface.ReturnCheckOutLocation();
                        return true;
                    }
                }
                else
                {
                    testInterface.UpdateVerboseInfo("No free connections");
                    return true;
                }
            }
        }

        public bool AddConnection()
        {
            string phoneNumber;
            string remotePartyNumber;

            if(!GetRemotePartyNumber(out remotePartyNumber, out phoneNumber))
            {
                return true;
            }

            string locationGuid = LocationGuidFactory.GetLocationGuid();

            if(asyncHttp.AddConnection(this.sessionId, locationGuid, remotePartyNumber, phoneNumber, conferenceId))
            {
                AddLocationId(locationGuid, phoneNumber);
                AddToPendingJoins();

                conferenceInterface.UpdatePendingJoins(1);
                    
                testInterface.UpdatePendingConnections(1);

                testInterface.NewLocation(conferenceGuid, locationGuid, this.conferenceId, locationStatus.initializing.ToString() , phoneNumber.ToString());
                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);
                return true;
            }
            else
            {
                testInterface.UpdateErrorInfo("Could not create a new location");
                testInterface.UpdateFailedConnections(1);

                rangeQueue.ReturnANumber(phoneNumber);
                return false;
            }
        }
        public bool AddConnectionTunnel(string phoneNumber)
        {
            lock(tunnelLock)
            {
                if(conferenceInterface.IsFreeLocation())
                {
                    if(rangeQueue.CheckNumber(phoneNumber))
                    {
                        if(STATUS == status.initializing)
                        {
                            AddsQueue.Enqueue(phoneNumber);
                            testInterface.UpdateVerboseInfo("Queueing add location for after conference is initialized");
                            conferenceInterface.ReturnCheckOutLocation();
                            return true;
                        }
                        if(STATUS != status.ended || STATUS != status.lost)
                        {
                            if(LiveConnections - PendingKicks.Count > 0)
                            {
                                if(AddConnection(phoneNumber))
                                {
                                    conferenceInterface.ReturnCheckOutLocation();
                                    return true;
                                }
                                else
                                {
                                    conferenceInterface.ReturnCheckOutLocation();
                                    return false;
                                }
                            }
                            else
                            {
                                testInterface.UpdateVerboseInfo("Thwarted error code 8 for join");
                                conferenceInterface.ReturnCheckOutLocation();
                                return true;
                            }
                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("Join location dropped due to conference ending");
                            conferenceInterface.ReturnCheckOutLocation();
                            return true;
                        }
                    }
                    else
                    {
                        testInterface.UpdateErrorInfo("Number already in use. Dropping request");
                        conferenceInterface.ReturnCheckOutLocation();
                        return false;
                    }
                }
                else
                {   
                    testInterface.UpdateVerboseInfo("No free connections");
                    return true;
                }
            }
        }

        public bool AddConnection(string phoneNumber)
        {
            string locationGuid = LocationGuidFactory.GetLocationGuid();

            if(!rangeQueue.CheckNumber(phoneNumber))
            {
                testInterface.UpdateErrorInfo("Number already in use. Dropping request");
                return false;
            }
            else                    
            {
                rangeQueue.ReturnANumber(phoneNumber);
            }

            rangeQueue.ReserveANumber(phoneNumber);

            if(asyncHttp.AddConnection(this.sessionId, locationGuid, phoneNumber.ToString(), phoneNumber, conferenceId))
            {
                AddLocationId(locationGuid, phoneNumber);

                AddToPendingJoins();

                conferenceInterface.UpdatePendingJoins(1);
                
                testInterface.UpdatePendingConnections(1);

                testInterface.NewLocation(conferenceGuid, locationGuid, this.conferenceId, locationStatus.initializing.ToString(), phoneNumber.ToString());
                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);
                return true;

            }
            else
            {
                testInterface.UpdateErrorInfo("Could not join a new location");
                testInterface.UpdateFailedConnections(1);

                rangeQueue.ReturnANumber(phoneNumber);
                
                return false;
            }
        }

        public bool RemoveConnectionTunnel(string locationId, string locationGuid)
        {
            Debug.Assert(locationGuid != null, "locationGuid is null");

            LocationGuids tempGuid = null;
            lock(tunnelLock)
            {
				if(locationId == null)
				{
					testInterface.UpdateVerboseInfo("Threading logic error averted");
					return true;
				}
                if(STATUS == status.online)
                {
                        IsLastLocation();
                }

                lock(locationIdListLock)
                {
                    bool foundLocationGuid = false;

                    for(int i = 0; i < LocationIdList.Count; i++)
                    {
                        LocationGuids temp = (LocationGuids) LocationIdList[i];

                        if(temp.locationGuid  == locationGuid)
                        {
                            foundLocationGuid = true;
                            tempGuid = temp;

                            if(temp.STATUS == locationStatus.initializing)
                            {
                                temp = ((LocationGuids) LocationIdList[i]);
                                tempGuid = temp;
                                temp.pendingDestroy = true;
                                LocationIdList[i] = temp;
                                testInterface.UpdateVerboseInfo("Remove of location slated for after location is online");
                                return true;
                            }

                            if(temp.STATUS == locationStatus.kicked)
                            {
                                testInterface.UpdateVerboseInfo("Thwarted second attempt to kick a kicked location");
                                return true;
                            }
                        }
                    }

                    if(foundLocationGuid)
                    {

                    }
                    else
                    {
                        testInterface.UpdateVerboseInfo("Thwarted second attempt to kick a kicked location");
                        return true;
                    }
                }
                
                if(LocationIdList.Count - PendingKicks.Count > 0) 
                {
                    if(!PendingKicks.Contains(locationId))
                    {
                        if(!((LiveConnections - PendingKicks.Count <= 1) && PendingJoins > 0))
                        {
                            
                            return RemoveConnection(locationId, locationGuid, tempGuid);
                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("Thwarted error code 8");
                            return true;
                        }
                    }
                    else
                    {
                        testInterface.UpdateVerboseInfo("Already attempting to kick locationId " + locationId);
                        return false;
                    }
                }
                else
                {
                    testInterface.UpdateVerboseInfo("No more connections to kick");
                    return false;
                }
            }
        }

        public bool RemoveConnection(string locationId, string locationGuid, LocationGuids tempGuids)
        {
            //Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");

            if(asyncHttp.RemoveConnection(this.sessionId, locationGuid, locationId, conferenceId))
            { 
                tempGuids.STATUS = locationStatus.kicking;
                AddToPendingKicks(locationId);

                conferenceInterface.UpdatePendingKicks(1);

                testInterface.UpdatePendingKicks(1);

                testInterface.UpdateEstablishedLocation(conferenceGuid, locationGuid, conferenceId, locationId, locationStatus.kicking.ToString());
                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);
                return true;

            }
            else
            {
                tempGuids.STATUS = locationStatus.online;
                testInterface.UpdateErrorInfo("Unable to remove a location");
                testInterface.UpdateFailedKicks(1);

                return false;
            }
        }

        public bool MuteConnection(string locationGuid, string locationId)
        {  
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");

            if(asyncHttp.MuteConnection(this.sessionId, locationGuid, locationId, conferenceId))
            { 
                testInterface.UpdateEstablishedLocation(conferenceGuid, locationGuid, conferenceId, locationId, IStressTesting.MUTING);
                
                return true;
    
            }
            else
            {
                testInterface.UpdateErrorInfo("Unable to mute location");
                return false;
            }  
  
        }
        #endregion Send Async Http Routines

        #region Results From Async Http Routines
        
        public void AssignSessionId(string sessionId)
        {
            Debug.Assert(sessionId != null, "sessionId is null");
            this.sessionId = sessionId;
        }

        public void AssignConferenceId(string conferenceId)
        {
            //Debug.Assert(conferenceId != null, "conferenceId is null");

            this.conferenceId = conferenceId;
        }

        public void CreateConferenceFailure(string locationGuid)
        {
            lock(tunnelLock)
            {
                Debug.Assert(locationGuid != null, "locationGuid is null");

                RemoveFromPendingCreates();
                RemoveFromPendingJoins();

                conferenceInterface.UpdatePendingCreates(-1);
                conferenceInterface.UpdatePendingJoins(-1);   
    
                testInterface.UpdatePendingConferences(-1);
                testInterface.UpdatePendingConnections(-1);
                testInterface.UpdateFailedConferences(1);

                testInterface.RemoveLocation(conferenceGuid, locationGuid);
                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, Conference.status.ended.ToString(), PendingKicks.Count);

                string phoneNumber = (string) lookUpPhoneNumbersTable[locationGuid];

                RemoveLocationGuid(locationGuid);

                STATUS = status.ended;
           
            }
        }

        public void CreateConferenceSuccess(string locationGuid, string locationId, string remotePartyNumber)
        {
            bool pendingDestroy;

            lock(tunnelLock)
            {
                hadAtLeastOneSuccessfulConnect = true;

                Debug.Assert(locationId != null, "locationId is null");
                Debug.Assert(locationGuid != null, "locationGuid is null");
                Debug.Assert(remotePartyNumber != null, "remotePartyNumber is null");

                RemoveFromPendingCreates();
                RemoveFromPendingJoins();

                conferenceInterface.UpdatePendingCreates(-1);
                conferenceInterface.UpdatePendingJoins(-1);
                conferenceInterface.UpdateLiveConferences(1);
                conferenceInterface.UpdateLiveConnections(1);

                testInterface.UpdatePendingConferences(-1);
                testInterface.UpdatePendingConnections(-1);
                testInterface.UpdateLiveConferences(1);
                testInterface.UpdateLiveConnections(1);

                testInterface.UpdateLocation(conferenceGuid, locationGuid, conferenceId, locationId, Conference.status.online.ToString(), remotePartyNumber);
                
                pendingDestroy = LinkLocationId(locationGuid, locationId, remotePartyNumber);

                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, Conference.status.online.ToString(), PendingKicks.Count);

                STATUS = Conference.status.online;

            }

            if(pendingDestroy || STATUS == status.ended)
            {
                if(RemoveConnectionTunnel(locationId, locationGuid))
                {
                    return;
                }
                else
                {
                    testInterface.UpdateErrorInfo("Unable to destroy a conference.  Critical error. Restart necessary");
                    return;
                }
            }

            ReadAddConnectionQueue();
        }

        public void JoinConferenceFailure(string locationGuid)
        {
            lock(tunnelLock)
            {
                Debug.Assert(locationGuid != null, "locationGuid is null");

                RemoveFromPendingJoins();

                conferenceInterface.UpdatePendingJoins(-1);

                testInterface.UpdatePendingConnections(-1);
                testInterface.UpdateFailedConnections(1);
                testInterface.RemoveLocation(conferenceGuid, locationGuid);
                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);

                RemoveLocationGuid(locationGuid);
            }
        }

        public void JoinConferenceSuccess(string locationGuid, string locationId, string remotePartyNumber)
        {
            bool pendingDestroy;
            lock(tunnelLock)
            {
                hadAtLeastOneSuccessfulConnect = true;

                Debug.Assert(locationId != null, "locationId is null");
                Debug.Assert(locationGuid != null, "locationGuid is null");
                Debug.Assert(remotePartyNumber != null, "remotePartyNumber is null");

                RemoveFromPendingJoins();

                conferenceInterface.UpdatePendingJoins(-1);
                conferenceInterface.UpdateLiveConnections(1);

                testInterface.UpdatePendingConnections(-1);
                testInterface.UpdateLiveConnections(1);
                testInterface.UpdateLocation(conferenceGuid, locationGuid, conferenceId, locationId, locationStatus.online.ToString() , remotePartyNumber);
                
                pendingDestroy = LinkLocationId(locationGuid, locationId, remotePartyNumber);

                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, Conference.status.online.ToString(), PendingKicks.Count);
            }
            

            if(pendingDestroy || STATUS == status.ended)
            {
                if(RemoveConnectionTunnel(locationId, locationGuid))
                {

                }
                else
                {
                    //testInterface.UpdateErrorInfo("Unable to destroy a location.  Critical error. Restart necessary");
                    return;
                }
            }
           
        }

        public void KickConferenceFailure(string locationGuid, string locationId)
        {
            lock(tunnelLock)
            {
                Debug.Assert(locationId != null, "locationId is null");
                Debug.Assert(locationGuid != null, "locationGuid is null");

                bool foundId = RemoveFromPendingKicks(locationId);

                conferenceInterface.UpdatePendingKicks(-1);
            
                testInterface.UpdatePendingKicks(-1);
                testInterface.UpdateFailedKicks(1);
                testInterface.UpdateEstablishedLocation(conferenceGuid, locationGuid, conferenceId, locationId, "kick failed");
                testInterface.UpdateConference(conferenceGuid, LiveConnections, PendingJoins, STATUS.ToString(), PendingKicks.Count);
                
                lock(locationIdListLock)
                {
                    for(int i = 0; i < LocationIdList.Count; i++)
                    {

                        LocationGuids temp = (LocationGuids) LocationIdList[i];

                        if(temp.locationGuid == locationGuid)
                        {
                            temp.STATUS = locationStatus.online;
                            break;
                        }
                    }
                }
            }
        }

        public void KickConferenceSuccess(string locationGuid, string locationId)
        {
            lock(tunnelLock)
            {
                Debug.Assert(locationId != null, "locationId is null");
                Debug.Assert(locationGuid != null, "locationGuid is null");

                bool foundLocationId = RemoveFromPendingKicks(locationId); 
                bool nonDoubleKick = true;

                lock(locationIdListLock)
                {
                    for(int i = 0; i < LocationIdList.Count; i++)
                    {
                        LocationGuids temp = (LocationGuids) LocationIdList[i];
                        if(temp.locationGuid == locationGuid)
                        {
                            if(temp.STATUS != locationStatus.kicking)
                            {
                                nonDoubleKick = false;
                                break;
                            }
                        }
                    }
                }

                if(nonDoubleKick && foundLocationId)
                {
                    conferenceInterface.UpdatePendingKicks(-1);
                    conferenceInterface.UpdateLiveConnections(-1);
            
                    testInterface.UpdatePendingKicks(-1);
                    testInterface.UpdateLiveConnections(-1);

                    testInterface.RemoveLocation(conferenceGuid, locationGuid);             
            
                    bool lastLocationRemoved = RemoveLocationGuid(locationGuid);

                    if(!lastLocationRemoved)
                    {
                        testInterface.UpdateConference(conferenceGuid, LiveConnections , PendingJoins,STATUS.ToString(), PendingKicks.Count);
                    }
                }
            }
        }
   
        public void MuteConnectionSuccess(string locationGuid, string locationId)
        {
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");

            lock(tunnelLock)
            {
                lock(locationIdListLock)
                {
                    for(int i = 0; i < LocationIdList.Count; i++)
                    {
                        LocationGuids temp = (LocationGuids) LocationIdList[i];
                        if(temp.locationGuid == locationGuid)
                        {
                            temp.isMuted = !temp.isMuted;
                            if(temp.isMuted)
                            {
                                temp.STATUS = locationStatus.muted;
                                testInterface.UpdateEstablishedLocation(this.conferenceGuid, temp.locationGuid, this.conferenceId, temp.locationId, locationStatus.muted.ToString());
                            }
                            else
                            {
                                temp.STATUS = locationStatus.unmuted;
                                testInterface.UpdateEstablishedLocation(this.conferenceGuid, temp.locationGuid, this.conferenceId, temp.locationId, locationStatus.unmuted.ToString());
                            }

                            LocationIdList[i] = temp;
                            break;
                        }
                    }
                }
            }

        }

        public void MuteConnectionFailure(string locationGuid, string locationId)
        {
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");

            lock(tunnelLock)
            {
                lock(locationIdListLock)
                {
                    for(int i = 0; i < LocationIdList.Count; i++)
                    {
                        LocationGuids temp = (LocationGuids) LocationIdList[i];
                        if(temp.locationGuid == locationGuid)
                        {
                            temp.STATUS = locationStatus.muteFailed;
                            testInterface.UpdateEstablishedLocation(this.conferenceGuid, temp.locationGuid, this.conferenceId, temp.locationId, locationStatus.muteFailed.ToString());
                           
                            break;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Occurs when a join is initially sent to Samoa, and the conference has been destroyed
        /// </summary>
        public void Join404(string locationGuid)
        {
            lock(tunnelLock)
            {
                JoinReturned404(locationGuid);
            }

            
        }

        public void Kick404(string locationGuid)
        {
            lock(tunnelLock)
            {
                KickReturned404(locationGuid);
            }

            
        }

        public void Mute404(string locationGuid)
        {
            lock(tunnelLock)
            {
                
            }
        }

        public void CheckCreate404()
        {
            lock(tunnelLock)
            {
               
            }
        }

        public void CheckJoin404()
        {
            lock(tunnelLock)
            {
                
            }
        }

        public void CheckKick404()
        {
            lock(tunnelLock)
            {
                
            }
        }
        
        #endregion Results From Async Http Routines

        #region Timers

        public void JoinTimerElapsed(object o, System.Timers.ElapsedEventArgs args)
        {

            joinTimer.Interval = randomNumber.Next(10000, settings.averageLittleSpike);
            if(AddConnectionTunnel())
            {
            }
            else
            {
                testInterface.UpdateVerboseInfo("Singe Join timer elasped: couldn't join");
            }
        }

        public void KickTimerElapsed(object o, System.Timers.ElapsedEventArgs args)
        {
            kickTimer.Interval = randomNumber.Next(10000, settings.averageLittleSpike * 10000);

            string locationId;
            string locationGuid;
            ArrayList shortened = new ArrayList();

            // Choose random location
            lock(locationIdListLock)
            {
                for(int i = 0; i < LocationIdList.Count; i++)
                {
                    if(((LocationGuids) LocationIdList[i]).locationId != null)
                    {
                        shortened.Add(i);
                    }
                }
            
                if(shortened.Count == 0)
                {
                    testInterface.UpdateVerboseInfo("No running connections for random kick");
                    return;
                }

                int which = randomNumber.Next(0, shortened.Count);

                int whichLocationId = (int) shortened[which];

                locationId = ((LocationGuids)LocationIdList[whichLocationId]).locationId;
                locationGuid = ((LocationGuids)LocationIdList[whichLocationId]).locationGuid;
            }

            RemoveConnectionTunnel(locationId, locationGuid);
        }

        public void SpikeJoinTimerElapsed(object o, System.Timers.ElapsedEventArgs args)
        {
            spikeJoinTimer.Interval = randomNumber.Next(10000, settings.averageLittleSpike * 1000);

            int numberConnectionsToJoin =  randomNumber.Next(0, settings.maximumConnections - (conferenceInterface.QueryPendingJoins() + conferenceInterface.QueryLiveConnections()));

            for(int i = 0; i < numberConnectionsToJoin; i++)
            {
                if(STATUS != status.ended || STATUS != status.lost)
                {
                    if(AddConnectionTunnel())
                    {
                    }
                    else
                    {
                        testInterface.UpdateErrorInfo("Failed to join a location for a internal conference spike");
                        return;
                    }
                }

                Thread.Sleep(randomNumber.Next(0, settings.averageCallInterval * 1000));
            }
        }
  
        public void SpikeKickTimerElapsed(object o, System.Timers.ElapsedEventArgs args)
        {
            spikeKickTimer.Interval = randomNumber.Next(10000, settings.averageLittleSpike * 1000);

            ArrayList shortened = new ArrayList();

            lock(locationIdListLock)
            {
                for(int i = 0; i < LocationIdList.Count; i++)
                {
                    if(((LocationGuids) LocationIdList[i]).locationId != null)
                    {
                        shortened.Add(i);
                    }
                }
            }

            int howMany = randomNumber.Next(0, shortened.Count);

            for(int i = 0; i < shortened.Count; i++)
            {
                if(STATUS != status.ended || STATUS != status.lost)
                {
                    int whichLocationId = (int) shortened[i];
                    string locationId;
                    string locationGuid;

                    locationId = ((LocationGuids)LocationIdList[whichLocationId]).locationId;
                    locationGuid = ((LocationGuids)LocationIdList[whichLocationId]).locationGuid;

                    if(!RemoveConnectionTunnel(locationId, locationGuid))
                    {
                        return;
                    }
                }
    
                Thread.Sleep(randomNumber.Next(100, settings.averageCallInterval * 1000));
            }
               
            
        }

        #endregion Timers

        #region DatabaseChecking

        public void  CheckConferenceDatabase(string sessionId, string locationGuid, string locationId, string remotePartyNumber)
        {
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(remotePartyNumber != null, "remotePartyNumber is null");
            Debug.Assert(sessionId != null, "sessionId is null");

            double ratio;

            try
            {
                ratio = (System.Double.Parse(settings.createTimeout))/(System.Double.Parse(settings.createPoll));
            }
            catch
            {
                return;
            }  
          
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            try
            {
                connection = new MySqlConnection(Database.FormatDSN(settings.databaseName, settings.appServerIp, 3306, DbUsername, DbPassword, true));
                connection.Open();

                SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, MecParticipants.TableName);
                builder.fieldNames.Add(MecParticipants.Status);
                builder.where[MecParticipants.ConferenceId] = int.Parse(conferenceId);

                command = new MySqlCommand(builder.ToString(), connection);

                int i = 0;

                for(i = 0; i < (int) Math.Ceiling(ratio); i++)
                {
                    System.Threading.Thread.Sleep(Int32.Parse(settings.createPoll));
                    
                    reader = command.ExecuteReader();

                    int status;
                    bool isEmpty = true;
            
                    while(reader.Read())
                    {
                        status = reader.GetInt32(0);

                        if(status > (int) MecParticipantStatus.Connecting && status < (int) MecParticipantStatus.Disconnected)
                        {
                            isEmpty = false;
                        }
                    }

                    if(isEmpty == false)
                    {
                        testInterface.UpdateVerboseInfo("CHECK CREATE CONFERENCE: success");
                        CreateConferenceSuccess(locationGuid, locationId, remotePartyNumber);
                        reader.Close();
                        break;
                    }

                    reader.Close();
                }

                if(command != null)
                {
                    command.Dispose();
                }

                if(connection != null)
                {
                    connection.Close();
                }

                if( i == Math.Ceiling(ratio))
                {
                    testInterface.UpdateErrorInfo("CHECK CREATE CONFERENCE: failure");
                    CreateConferenceFailure(locationGuid);
                }
            }
            catch(Exception e)
            {
                e.ToString();
            }
            finally
            {
                if(connection != null)
                {
                    connection.Close();
                }

                if(reader != null)
                {
                    reader.Close();
                }

                if(command != null)
                {
                    command.Dispose();
                }
            }           
        }

        public void CheckJoinDatabase(string sessionId, string locationGuid, string locationId, string remotePartyNumber)
        {
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(remotePartyNumber != null, "remotePartyNumber is null");
            Debug.Assert(sessionId != null, "sessionId is null");

            double ratio;

            try
            {
                ratio = (System.Double.Parse(settings.joinTimeout))/(System.Double.Parse(settings.joinPoll));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }  
                       
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            try
            {
                connection= new MySqlConnection(Database.FormatDSN(settings.databaseName, settings.appServerIp, 3306, DbUsername, DbPassword, true));
                connection.Open();
         
                SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, MecParticipants.TableName);
                builder.fieldNames.Add(MecParticipants.Status);
                builder.where[MecParticipants.Id] = int.Parse(locationId);

                int i = 0;

                command = new MySqlCommand(builder.ToString(), connection);

                for(i = 0; i < (int) Math.Ceiling(ratio); i++)
                {
                    System.Threading.Thread.Sleep(Int32.Parse(settings.joinPoll));
                    reader = command.ExecuteReader();

                    int status;
                    bool isConnected = false;

                    if(reader.Read())
                    {
                        status = 0;

                        try
                        {
                            status = int.Parse(reader.GetString(0));
                        }
                        catch(Exception)
                        {}

                        if((status > (int) MecParticipantStatus.Connecting) && (status < (int) MecParticipantStatus.Disconnected))
                        {
                            isConnected = true;
                        }
                        else
                        {
                            isConnected = false;
                        }
                    }

                    if(isConnected == true)
                    {
                        testInterface.UpdateVerboseInfo("CHECK JOIN CONFERENCE: success");
                        JoinConferenceSuccess(locationGuid, locationId, remotePartyNumber);
                        reader.Close();
                        break;
                    }

                    reader.Close();
                    
                }
                
                if(command != null)
                {
                    command.Dispose();
                }

                if(connection != null)
                {
                    connection.Close();
                }

                if(i == (int) Math.Ceiling(ratio))
                {
                    testInterface.UpdateErrorInfo("CHECK JOIN CONFERENCE: failure");
                    JoinConferenceFailure(locationGuid);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            finally
            {
                if(connection != null)
                {
                    connection.Close();
                }

                if(reader != null)
                {
                    reader.Close();
                }

                if(command != null)
                {
                    command.Dispose();
                }
            }
        }

        public void CheckKickDatabase(string sessionId, string locationGuid, string locationId)
        {
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(sessionId != null, "sessionId is null");

            double ratio;

            try
            {
                ratio = (System.Double.Parse(settings.kickTimeout))/(System.Double.Parse(settings.kickPoll));
            }
            catch
            {
                return;
            }  
               
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            try
            {
                connection = new MySqlConnection(Database.FormatDSN(settings.databaseName, settings.appServerIp, 3306, DbUsername, DbPassword, true));
                connection.Open();     

                SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, MecParticipants.TableName);
                builder.fieldNames.Add(MecParticipants.Status);
                builder.where[MecParticipants.Id] = int.Parse(locationId);

                int i = 0;

                command = new MySqlCommand(builder.ToString(), connection);

                for(i = 0; i < (int) Math.Ceiling(ratio); i++)
                {
                    System.Threading.Thread.Sleep(Int32.Parse(settings.kickPoll));
                        
                    reader = command.ExecuteReader();

                    int status;
                    bool isConnected = false;

                    if(reader.Read())
                    {
                        status = 0;

                        try
                        {
                            status = int.Parse(reader.GetString(0));
                        }
                        catch(Exception)
                        {}

                        if((status > (int) MecParticipantStatus.None) && (status < (int) MecParticipantStatus.Disconnected))
                        {
                            isConnected = true;
                        }
                        else
                        {
                            isConnected = false;
                        }
                    }

                    if(isConnected == false)
                    {
                        testInterface.UpdateVerboseInfo("CHECK KICK LOCATION: success");
                        KickConferenceSuccess(locationGuid, locationId);
                        reader.Close();
                        break;
                    }

                    reader.Close();
                    
                }

                if(command != null)
                {
                    command.Dispose();
                }

                if(connection != null)
                {
                    connection.Close();
                }

               

                if(i == (int) Math.Ceiling(ratio))
                {
                    testInterface.UpdateErrorInfo("CHECK KICK LOCATION: failure");
                    KickConferenceFailure(locationGuid, locationId);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            finally
            {
                if(connection != null)
                {
                    connection.Close();
                }

                if(reader != null)
                {
                    reader.Close();
                }

                if(command != null)
                {
                    command.Dispose();
                }
            }
        }

        public bool CheckForConferenceExistenceOnMMS()
        {
            // Assuming conference is still in existence
            return true;
        }


        #endregion Database Checking
    }
}
