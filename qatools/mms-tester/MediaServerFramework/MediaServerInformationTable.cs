using System;
using System.Collections;

using Metreos.Samoa.Core;
using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.Core;

namespace Metreos.MmsTester.MediaServerFramework
{
	/// <summary>
	/// Stores all pertinent information relating to a media server
	/// </summary>
	public class MediaServerInformationTable
	{
        #region Non-Thread-Safe Members
        public string mediaServerMachineName;
        public string mediaServerQueueName;
        public int numberOfTotalPossibleConnections;
        public Connection[] connections;
        //public ArrayList connections;
        public ArrayList conferences;
        public ArrayList clients;
        public Hashtable unestablishedTransactions;
        public Hashtable UnestablishedTransactions;
        public Hashtable establishedTransactions;
        public Hashtable EstablishedTransactions;
        // Real time statistics
        private int numberOfConcurrentConnections;
        private int numberOfConcurrentConferences;
        private int numberOfConcurrentPlays;
        private int numberOfConcurrentPlayToConferences;
        private int numberOfConcurrentJoins;
        private int numberOfConcurrentKicks;
        private int numberOfConcurrentMuted;
        private int numberOfConcurrentRecords;
        private int numberOfConcurrentReceiveDigits;
        private int numberOfConcurrentStopMedias;

        // Lifetime statistics
        private int numberOfTotalConnections;
        private int numberOfTotalConferences;
        private int numberOfTotalPlays;
        private int numberOfTotalPlayToConferences;
        private int numberOfTotalJoins;
        private int numberOfTotalKicks;
        private int numberOfTotalMuted;
        private int numberOfTotalRecords;
        private int numberOfTotalReceiveDigits;
        private int numberOfTotalStopMedias;
        #endregion Non-Thread-Safe Members

        #region Helper Classes
        // Connection information class
        public class Connection
        {
            public int conferenceId;
            public int connectionId;

            // Status booleans
            public bool free = false;
            public bool reserved = false;
            public bool joining = false;
            public bool kicking = false;
            public bool playing = false;
            public bool muting = false;
            public bool recording = false;
            public bool receivingDigits = false;
            public bool stoppingMedia = false;

            // State booleans
            public bool isConnecting = false;
            public bool isConnectingToConference = false;
            public bool isConnectedToConference = false;
            public bool isConnected = false;
            public bool isDisconnecting = false;
            public bool isDisconnected = false;
            public bool isDisconnectingFromConference = false;
            public bool isDisconnectedFromConference = false;
            public bool isInConference = false;
            public bool isMuting = false;
            public bool isMuted = false;

			// Other connection specific information
			public int port;
			public string ipAddress;

            public Connection()
            {
                free = true;

                isDisconnected = true;
                isDisconnectedFromConference = true;

                conferenceId = -1;
                connectionId = -1;
            }
        }

        public class Conference
        {
            public ArrayList connections;

            public Conference()
            {
                connections = new ArrayList();
            }

            public Conference(Connection initialConnection)
            {
                connections = new ArrayList();
                connections.Add(initialConnection);
            }
        }

        public class Client
        {
            public enum Status
            {
                notConnected,
                connecting,
                connected,
                disconnecting
            }

            public Status status;
            public string clientQueueName;
            public string clientMachineName;
            public string initialTransactionId;

            public Client(string queueName, string machineName, Status status, string initialTransactionId)
            {
                status = Status.notConnected;
                clientQueueName = queueName;
                clientMachineName = machineName;
                this.status = status;
                this.initialTransactionId = initialTransactionId;
            }
        }
        #endregion Helper Classes      

        #region Thread-Safe Properties
        public int NumberOfConcurrentConnections
        {
            set
            {
                lock(numberOfConcurrentConnectionsLock)
                {
                    numberOfConcurrentConnections = value;
                }
            }
            get
            {
                lock(numberOfConcurrentConnectionsLock)
                {
                    return numberOfConcurrentConnections;
                }
            }
        }

        public int NumberOfConcurrentConferences
        {
            set
            {
                lock(numberOfConcurrentConferencesLock)
                {
                    numberOfConcurrentConferences = value;
                }
            }
            get
            {
                lock(numberOfConcurrentConferencesLock)
                {
                    return numberOfConcurrentConferences;
                }
            }
        }

        public int NumberOfConcurrentPlays
        {
            set
            {
                lock(numberOfConcurrentPlaysLock)
                {
                    numberOfConcurrentPlays = value;
                }
            }
            get
            {
                lock(numberOfConcurrentPlaysLock)
                {
                    return numberOfConcurrentPlays;
                }
            }
        }

        public int NumberOfConcurrentPlayToConferences
        {
            set
            {
                lock(numberOfConcurrentPlayToConferencesLock)
                {
                    numberOfConcurrentPlayToConferences = value;
                }
            }
            get
            {
                lock(numberOfConcurrentPlayToConferencesLock)
                {
                    return numberOfConcurrentPlayToConferences;
                }
            }
        }

        public int NumberOfConcurrentJoins
        {
            set
            {
                lock(numberOfConcurrentJoinsLock)
                {
                    numberOfConcurrentJoins = value;
                }
            }
            get
            {
                lock(numberOfConcurrentJoinsLock)
                {
                    return numberOfConcurrentJoins;
                }
            }
        }

        public int NumberOfConcurrentKicks
        {
            set
            {
                lock(numberOfConcurrentKicksLock)
                {
                    numberOfConcurrentKicks = value;
                }
            }
            get
            {
                lock(numberOfConcurrentKicksLock)
                {
                    return numberOfConcurrentKicks;
                }
            }
        }

        public int NumberOfConcurrentMuted
        {
            set
            {
                lock(numberOfConcurrentMutedLock)
                {
                    numberOfConcurrentMuted = value;
                }
            }
            get
            {
                lock(numberOfConcurrentMutedLock)
                {
                    return numberOfConcurrentMuted;
                }
            }
        }

        public int NumberOfConcurrentRecords
        {
            set
            {
                lock(numberOfConcurrentRecordsLock)
                {
                    numberOfConcurrentRecords = value;
                }
            }
            get
            {
                lock(numberOfConcurrentRecordsLock)
                {
                    return numberOfConcurrentRecords;
                }
            }
        }

        public int NumberOfConcurrentReceiveDigits
        {
            set
            {
                lock(numberOfConcurrentReceiveDigitsLock)
                {
                    numberOfConcurrentReceiveDigits = value;
                }
            }
            get
            {
                lock(numberOfConcurrentReceiveDigitsLock)
                {
                    return numberOfConcurrentReceiveDigits;
                }
            }
        }

        public int NumberOfConcurrentStopMedias
        {
            set
            {
                lock(numberOfConcurrentStopMediasLock)
                {
                    numberOfConcurrentStopMedias = value;
                }
            }
            get
            {
                lock(numberOfConcurrentStopMediasLock)
                {
                    return numberOfConcurrentStopMedias;
                }
            }
        }

        public int NumberOfTotalConnections
        {
            set
            {
                lock(numberOfTotalConnectionsLock)
                {
                    numberOfTotalConnections = value;
                }
            }
            get
            {
                lock(numberOfTotalConnectionsLock)
                {
                    return numberOfTotalConnections;
                }
            }
        }

        public int NumberOfTotalConferences
        {
            set
            {
                lock(numberOfTotalConferencesLock)
                {
                    numberOfTotalConferences = value;
                }
            }
            get
            {
                lock(numberOfTotalConferencesLock)
                {
                    return numberOfTotalConferences;
                }
            }
        }

        public int NumberOfTotalPlays
        {
            set
            {
                lock(numberOfTotalPlaysLock)
                {
                    numberOfTotalPlays = value;
                }
            }
            get
            {
                lock(numberOfTotalPlaysLock)
                {
                    return numberOfTotalPlays;
                }
            }
        }

        public int NumberOfTotalPlayToConferences
        {
            set
            {
                lock(numberOfTotalPlayToConferencesLock)
                {
                    numberOfTotalPlayToConferences = value;
                }
            }
            get
            {
                lock(numberOfTotalPlayToConferencesLock)
                {
                    return numberOfTotalPlayToConferences;
                }
            }
        }

        public int NumberOfTotalJoins
        {
            set
            {
                lock(numberOfTotalJoinsLock)
                {
                    numberOfTotalJoins = value;
                }
            }
            get
            {
                lock(numberOfTotalJoinsLock)
                {
                    return numberOfTotalJoins;
                }
            }
        }

        public int NumberOfTotalKicks
        {
            set
            {
                lock(numberOfTotalKicksLock)
                {
                    numberOfTotalKicks = value;
                }
            }
            get
            {
                lock(numberOfTotalKicksLock)
                {
                    return numberOfTotalKicks;
                }
            }
        }

        public int NumberOfTotalMuted
        {
            set
            {
                lock(numberOfTotalMutedLock)
                {
                    numberOfTotalMuted = value;
                }
            }
            get
            {
                lock(numberOfTotalMutedLock)
                {
                    return numberOfTotalMuted;
                }
            }
        }

        public int NumberOfTotalRecords
        {
            set
            {
                lock(numberOfTotalRecordsLock)
                {
                    numberOfTotalRecords = value;
                }
            }
            get
            {
                lock(numberOfTotalRecordsLock)
                {
                    return numberOfTotalRecords;
                }
            }
        }

        public int NumberOfTotalReceiveDigits
        {
            set
            {
                lock(numberOfTotalReceiveDigitsLock)
                {
                    numberOfTotalReceiveDigits = value;
                }
            }
            get
            {
                lock(numberOfTotalReceiveDigitsLock)
                {
                    return numberOfTotalReceiveDigits;
                }
            }
        }

        public int NumberOfTotalStopMedias
        {
            set
            {
                lock(numberOfTotalStopMediasLock)
                {
                    numberOfTotalStopMedias = value;
                }
            }
            get
            {
                lock(numberOfTotalStopMediasLock)
                {
                    return numberOfTotalStopMedias;
                }
            }
        }
        #endregion Thread-Safe Properties

        #region Locks
        private object numberOfConcurrentConnectionsLock;
        private object numberOfConcurrentConferencesLock;
        private object numberOfConcurrentPlaysLock;
        private object numberOfConcurrentPlayToConferencesLock;
        private object numberOfConcurrentJoinsLock;
        private object numberOfConcurrentKicksLock;
        private object numberOfConcurrentMutedLock;
        private object numberOfConcurrentRecordsLock;
        private object numberOfConcurrentReceiveDigitsLock;
        private object numberOfConcurrentStopMediasLock;

        private object numberOfTotalConnectionsLock;
        private object numberOfTotalConferencesLock;
        private object numberOfTotalPlaysLock;
        private object numberOfTotalPlayToConferencesLock;
        private object numberOfTotalJoinsLock;
        private object numberOfTotalKicksLock;
        private object numberOfTotalMutedLock;
        private object numberOfTotalRecordsLock;
        private object numberOfTotalReceiveDigitsLock;
        private object numberOfTotalStopMediasLock;
        #endregion Locks

		public MediaServerInformationTable(string mediaServerMachineName, string mediaServerQueueName, int numberOfTotalPossibleConnections)
		{
            #region Initializing Members
            this.mediaServerMachineName = mediaServerMachineName;
            this.mediaServerQueueName = mediaServerQueueName;
            this.numberOfTotalPossibleConnections = numberOfTotalPossibleConnections;
            establishedTransactions = new Hashtable();
            EstablishedTransactions = Hashtable.Synchronized(establishedTransactions);
            unestablishedTransactions = new Hashtable();
            UnestablishedTransactions = Hashtable.Synchronized(unestablishedTransactions);

            numberOfConcurrentConnections = 0;
            numberOfConcurrentConferences = 0;
            numberOfConcurrentPlays = 0;
            numberOfConcurrentPlayToConferences = 0;
            numberOfConcurrentJoins = 0;
            numberOfConcurrentKicks = 0;
            numberOfConcurrentMuted = 0;
            numberOfConcurrentRecords = 0;
            numberOfConcurrentReceiveDigits = 0;
            numberOfConcurrentStopMedias = 0;
            #endregion Initializing Members

            #region Initializing Locks
            numberOfConcurrentConnectionsLock           = new object();
            numberOfConcurrentConferencesLock           = new object();
            numberOfConcurrentPlaysLock                 = new object();
            numberOfConcurrentPlayToConferencesLock     = new object();
            numberOfConcurrentJoinsLock                 = new object();
            numberOfConcurrentKicksLock                 = new object();
            numberOfConcurrentMutedLock                 = new object();
            numberOfConcurrentRecordsLock               = new object();
            numberOfConcurrentReceiveDigitsLock         = new object();
            numberOfConcurrentStopMediasLock            = new object();

            numberOfTotalConnectionsLock                = new object();
            numberOfTotalConferencesLock                = new object();
            numberOfTotalPlaysLock                      = new object();
            numberOfTotalPlayToConferencesLock          = new object();
            numberOfTotalJoinsLock                      = new object();
            numberOfTotalKicksLock                      = new object();
            numberOfTotalMutedLock                      = new object();
            numberOfTotalRecordsLock                    = new object();
            numberOfTotalReceiveDigitsLock              = new object();
            numberOfTotalStopMediasLock                 = new object();
            #endregion Intializing Locks

            #region Initializing Connections

            connections = new Connection[numberOfTotalPossibleConnections];

            for(int i = 0; i < numberOfTotalPossibleConnections; i++)
            {
                connections[i] = new Connection();
            }

            #endregion Initializing Connections

            #region Initializing Conferences

            conferences = new ArrayList();

            #endregion Initializing Conferences

            #region Initializing Clients

            clients = new ArrayList();

            #endregion Initializing Clients
		}

        #region Incoming Connection Manipulation

        public bool IsAvailableDisconnectedConnection(out int handle)
        {
            handle = -1;

            for(int i = 0; i  < this.numberOfTotalPossibleConnections; i++)
            {
                if(connections[i].isDisconnected)
                {
                    handle = i;
                    return true;
                }
            }

            return false;
        }

        public bool ConnectingToServer(InternalMessage im)
        {
            string queueName;
            string machineName;
            string transactionId;

            im.GetFieldByName(MmsProtocol.FIELD_MS_CLIENT_QUEUE_NAME, out queueName);
            im.GetFieldByName(MmsProtocol.FIELD_MS_CLIENT_MACHINE_NAME, out machineName);
            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);

            clients.Add(new Client(queueName, machineName, Client.Status.connecting, transactionId));

            return true;
        }

        public bool MoveToConnectingAConnection(InternalMessage im, int handle)
        {
            string transactionId;
            string port;
            string ipAddress;

			im.GetFieldByName(MmsProtocol.FIELD_MS_PORT, out port);
			im.GetFieldByName(MmsProtocol.FIELD_MS_IP_ADDRESS, out ipAddress);

			connections[handle].port = System.Int32.Parse(port);
			connections[handle].ipAddress = ipAddress;
			connections[handle].isConnecting = true;

            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);
           
            if(PushToTransactionTable(transactionId, handle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveToConnectingToConference(InternalMessage im)
        {
            string transactionId;
            string port;
            string ipAddress;
            string connectionId;
            string conferenceId;

            im.GetFieldByName(MmsProtocol.FIELD_MS_IP_ADDRESS, out ipAddress);
            im.GetFieldByName(MmsProtocol.FIELD_MS_PORT, out port);
            im.GetFieldByName(MmsProtocol.FIELD_MS_CONNECTION_ID, out connectionId);
            im.GetFieldByName(MmsProtocol.FIELD_MS_CONFERENCE_ID, out conferenceId);
            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);
            
            int whichConnection;

            if(GrabConnection(connectionId, out whichConnection))
            {
                connections[whichConnection].isConnectingToConference = true;

                if(PushToTransactionTable(transactionId, connectionId))
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Couldn't add to the transactionId table in MoveToConnectionToConference");
            }

            return false;
        }

        public bool MoveToDisconnectionAConnection(InternalMessage im)
        {
            string transactionId;
            string connectionId;

            im.GetFieldByName(MmsProtocol.FIELD_MS_CONNECTION_ID, out connectionId);
            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);

            int whichConnection;

            if(GrabConnection(connectionId, out whichConnection))
            {
                connections[whichConnection].isDisconnecting = true;

                if(PushToTransactionTable(transactionId, connectionId))
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Couldn't add to the transactionId table in MoveToDisconnectionAConnection");
            }

            return false;
        }

        #endregion Incoming Connection Manipulation

        #region Outgoing Connection Manipulation

        public bool MoveToConnectedToMediaServer(InternalMessage im)
        {
            string transactionId;
            string resultCode;

            im.GetFieldByName(MmsProtocol.FIELD_MS_RESULT_CODE, out resultCode);
            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);

            int whichClient;
            if(GrabClient(transactionId, out whichClient))
            {
                if(resultCode == MmsProtocol.MS_RESULT_OK)
                {
                    ( (Client) clients[whichClient]).status = Client.Status.connected;
                }
                else
                {
                    ( (Client) clients[whichClient]).status = Client.Status.notConnected;
                }

                return true;
            }

            return false;
        }

        public bool MoveToConnectedAConnection(InternalMessage im)
        {
            string transactionId;
            string resultCode;
            string connectionId;

            im.GetFieldByName(MmsProtocol.FIELD_MS_RESULT_CODE, out resultCode);
            im.GetFieldByName(MmsProtocol.MS_RESULT_E_CONNECTION_ID, out connectionId);
            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);

            int handle;
            
            if(resultCode == MmsProtocol.MS_RESULT_OK)
            {
                if(PopFromUnestablishedTransactionTable(transactionId, out handle))
                {

                    connections[handle].connectionId = Int32.Parse(connectionId);
                    connections[handle].isConnected = true;
                    connections[handle].isConnecting = false;

                    return true;
                }
            }
            else
            {
                if(PopFromUnestablishedTransactionTable(transactionId, out handle))
                {
                    connections[handle].isConnecting = false;
                }

            }

            return false;
            
        }

        public bool MoveToDisconnectedAConnection(InternalMessage im)
        {
            string transactionId;
            string resultCode;

            im.GetFieldByName(MmsProtocol.FIELD_MS_RESULT_CODE, out resultCode);
            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);

            int whichConnection;

            if(PopFromEstablishedTransactionTable(transactionId, out whichConnection))
            {
                if(resultCode == MmsProtocol.FIELD_MS_RESULT_CODE)
                {
                    connections[whichConnection].connectionId = -1;
                    connections[whichConnection].isDisconnecting = false;
                    connections[whichConnection].isDisconnected = true;

                    return true;
                }
                else
                {
                    connections[whichConnection].isDisconnecting = false;
                }
            }

            return false;
        }

        public bool MoveToConnectedToConference(InternalMessage im)
        {
            string transactionId;
            string resultCode;
            string conferenceId;

            im.GetFieldByName(MmsProtocol.FIELD_MS_RESULT_CODE, out resultCode);
            im.GetFieldByName(MmsProtocol.FIELD_MS_CONFERENCE_ID, out conferenceId);
            im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);

            int whichConnection;

            if(PopFromEstablishedTransactionTable(transactionId, out whichConnection))
            {
                if(resultCode == MmsProtocol.MS_RESULT_OK)
                {
                    connections[whichConnection].conferenceId = Int32.Parse(conferenceId);
                    connections[whichConnection].isConnectedToConference = true;
                    connections[whichConnection].isConnectingToConference = false;

                    return true;
                }
                else
                {
                    connections[whichConnection].isConnectingToConference = false;
                    
                    return false;
                }
            }

            return false;
            
        }

        #endregion Outgoing Connection Manipulation

        #region General Utility Classes

        public bool PushToTransactionTable(string transactionId, int handle)
        {
            if(UnestablishedTransactions.ContainsKey(transactionId))
            {
                Console.WriteLine("TransactionId has already been added.  Thread conflict in MMS Info Table");
                return false;
            }
            else
            {
                UnestablishedTransactions[transactionId] = handle;
                return true;
            }
        }

        
        public bool PushToTransactionTable(string transactionId, string connectionId)
        {
            if(EstablishedTransactions.ContainsKey(transactionId))
            {
                Console.WriteLine("TransactionId has already been added.  Critical conflict in MMS Info Table");
                return false;
            }
            else
            {
                EstablishedTransactions[transactionId] = connectionId;
                return true;
            }
        }

        public bool PopFromUnestablishedTransactionTable(string transactionId, out int handle)
        {
            handle = -1;

            if(UnestablishedTransactions.ContainsKey(transactionId))
            {
                handle = (int) UnestablishedTransactions[transactionId];
                return true;
            }
            
            Console.WriteLine("Couldn't find the new connection handle");
            return false;
        }

        public bool PopFromEstablishedTransactionTable(string transactionId, out int whichConnection)
        {
            whichConnection = -1;

            if(EstablishedTransactions.ContainsKey(transactionId))
            {
                string connectionId = (string) EstablishedTransactions[transactionId];
                
                for(int i = 0; i < numberOfTotalPossibleConnections; i++)
                {
                    if(connections[i].connectionId.ToString() == connectionId)
                    {
                        whichConnection = i;
                        return true;
                    }
                }

                return false;
            }
            else
            {
                Console.WriteLine("Transaction Id not found. Critical Error in MMS Info Table");
                return false;
            }
        }

        public bool GrabConnection(string connectionId, out int whichConnection)
        {
            whichConnection = -1;

            for(int i = 0; i < connections.Length; i++)
            {
                if(connections[i].connectionId.ToString() == connectionId)
                {
                    whichConnection = i;
                    return true;
                }
            }
            return false;
        }

        public bool GrabClient(string transactionId, out int whichClient)
        {
            whichClient = -1;

            for(int i = 0; i < clients.Count; i++)
            {
                if(((Client)clients[i]).initialTransactionId == transactionId)
                {
                    whichClient = i;
                    return true;
                }
            }

            Console.WriteLine("Can't find client");
            return false;
        }

        #endregion General Utility Classes
	}
}
