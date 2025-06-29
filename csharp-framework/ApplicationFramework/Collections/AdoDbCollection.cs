using System;
using System.Data;
using System.Collections;

namespace Metreos.ApplicationFramework.Collections
{
	/// <summary>
	/// Maps database name to System.Data.IDbConnection object
	/// </summary>
	[Serializable]
	public class AdoDbCollection
	{
        private Hashtable dbs;

        public int Count { get {return dbs.Count; } }

        public IDbConnection this[string name] { get { return dbs[name] as IDbConnection; } }

        public AdoDbCollection()
        {
            dbs = new Hashtable();
        }

        public void Add(string name, IDbConnection db)
        {
            if(dbs != null)
            {
                dbs[name] = db;
            }
        }

        public bool Contains(string name)
        {
            return dbs.Contains(name);
        }

		public IDictionaryEnumerator GetEnumerator()
		{
			return dbs.GetEnumerator();
		}

		public void Clear()
		{
            foreach(IDbConnection conn in dbs.Values)
            {
                using(conn)
                {
                    conn.Close();
                }
            }

			dbs.Clear();
		}
	}
}
