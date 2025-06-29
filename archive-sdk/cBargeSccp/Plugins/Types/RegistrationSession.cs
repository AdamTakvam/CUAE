using System;
using System.Collections;
using Metreos.ApplicationFramework;

namespace Metreos.Applications.cBarge
{
	/// <summary>
	/// This is the top-level container for objects relating to any occurances on this particular devices. 
	/// </summary>
	public sealed class RegistrationSession : IVariable
	{
        #region CallSessionCollection
        /// <summary>
        /// Wrapper around a Hashtable of CallSession instances
        /// </summary>
        public class CallSessionCollection
        {
            public virtual CallSession this[int callRef]
            {
                get 
                {
                    if ( ! ( callSessionMap.Contains(callRef) ))
                        callSessionMap[callRef] = new CallSession(callRef);
                
                    return callSessionMap[callRef] as CallSession;
                }
                
                set 
                {
                    callSessionMap[callRef] = value;
                }
            }
            private Hashtable callSessionMap;

            public void RemoveSession(int callRef)
            {
                callSessionMap.Remove(callRef);
            }

            public CallSessionCollection()
            {
                callSessionMap = new Hashtable();
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns a CallSessionCollection associated with a device.
        /// The collection contains CallSession objects that are indexed
        /// </summary>
        public CallSessionCollection CallSessions
        {
            get { return callSessions; }
            set { callSessions = value; }
        }
        private CallSessionCollection callSessions;

        public ConnectionPool @ConnectionPool
        {
            get { return connectionPool; }
        }
        private ConnectionPool connectionPool;

        /// <summary>
        /// StationID
        /// </summary>
        public string Sid 
        {
            get { return sid; }
            set { sid = value; }
        }
        private string sid;

        public Hashtable IsLineInstanceBargedMap
        {
            get { return isLineInstanceBargedMap; }
        }
        private Hashtable isLineInstanceBargedMap;
        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            // TODO:  Add RegistrationSession.Parse implementation
            return true;
        }

        #endregion

        public RegistrationSession()
		{
            callSessions = new CallSessionCollection();
            isLineInstanceBargedMap = new Hashtable();
            connectionPool = new ConnectionPool();
        }



    }
}
