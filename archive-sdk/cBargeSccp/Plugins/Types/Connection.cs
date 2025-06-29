using System;

namespace Metreos.Applications.cBarge
{
	/// <summary>
	/// Summary description for Connection.
	/// </summary>
	public class Connection
	{
        /// <summary>
        /// MMS ConferenceId in which the connectionId associated with this endpoint is located
        /// </summary>
        public string ConferenceId
        {
            get { return conferenceId; }
            set { conferenceId = value;}
        }
        private string conferenceId;

        /// <summary>
        /// MmsId of the media server on which all mms connections for this object are located
        /// </summary>
        public uint MmsId 
        { 
            get { return mmsId; }
            set { mmsId = value; }
        }
        private uint mmsId;

        public EndPoint Local
        {
            get { return local; }
            set { local = value; }
        }
        private EndPoint local;

        public EndPoint Remote
        {
            get { return remote; }
            set { remote = value; }
        }
        private EndPoint remote;

        public Connection()
		{
            conferenceId = "0";
            mmsId = 0;
            remote = new EndPoint();
            local = new EndPoint();
		}
	}
}
