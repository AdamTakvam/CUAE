using System;
using System.Collections;

namespace Metreos.MmsTester.Core
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
        public ArrayList connections;
        public ArrayList conferences;

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
            ArrayList connectionStatus;
            ArrayList connectionState;

            public enum ConnectionStatus
            {
                nothing,
                joining,
                kicking,
                playing,
                muting,
                recording,
                receivingDigits,
                stoppingMedia    
            }

            public enum ConnectionState
            {
                isAvailable,
                isConnected,
                isInConference,
                isMuted
            }

            public Connection()
            {
                connectionStatus = new ArrayList();
                connectionStatus.Add(ConnectionStatus.nothing);

                connectionState = new ArrayList();
                connectionState.Add(ConnectionState.isAvailable);
            }
        }

        public class Conference
        {
            ArrayList connections;

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
            connections = new ArrayList();

            for(int i = 0; i < numberOfTotalPossibleConnections; i++)
            {
                connections.Add(new Connection());
            }
            #endregion Initializing Connections

            #region Initializing Conferences
            conferences = new ArrayList();
            #endregion Initializing Conferences
		}
	}
}
