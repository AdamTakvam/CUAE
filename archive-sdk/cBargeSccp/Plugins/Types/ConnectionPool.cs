using System;
using System.Collections;

namespace Metreos.Applications.cBarge
{
	public class ConnectionPool
	{
        public int Size
        {
            get { return connections.Count; }
        }
        
        public void RemoveConnection(int callRef)
        {
            connections.Remove(callRef);
        }

        public bool AddConnection(int callRef, Connection newConnection)
        {
            if (newConnection == null)
                return false;

            connections.Add(callRef, newConnection);
            return true;
        }

        private Hashtable connections;
        
        public ConnectionPool()
		{
		    connections = new Hashtable();
        }
	}
}
