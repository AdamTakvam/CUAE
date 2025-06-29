using System;
using System.Collections;

namespace Metreos.Utilities.Collections
{
	/// <summary>This is a hashtable which requires two string keys to access any value</summary>
	/// <remarks>Fully thread-safe</remarks>
	public class TwoKeyHash
	{
        private const string DefaultDelimiter = "|";
        private readonly string delimiter;

        /// <summary>
        /// appName + DELIMITER + partName (string) -> value
        /// </summary>
        private readonly Hashtable hash;

        public object this[string key1, string key2]
        {
            get { return hash[MakeKey(key1, key2)]; }
        }

        public TwoKeyHash()
            : this(DefaultDelimiter) {}

        public TwoKeyHash(string delimiter)
        {
            this.delimiter = delimiter;

            this.hash = Hashtable.Synchronized(new Hashtable());
        }

        public bool Add(string key1, string key2, object Value)
        {
            if (key1 == null || key2 == null || Value == null)
                return false;

            this.hash[MakeKey(key1, key2)] = Value;
            return true;
        }

        public void Remove(string key1, string key2)
        {
            this.hash.Remove(MakeKey(key1, key2));
        }

        public bool Contains(string key1, string key2)
        {
            return this.hash.ContainsKey(MakeKey(key1, key2));
        }

        public void Clear()
        {
            this.hash.Clear();
        }

        public ICollection Values
        {
            get { return hash.Values; }
        }

        private string MakeKey(string key1, string key2)
        {
            if(key1 == null || key2 == null)
                return String.Empty;

            return key1 + delimiter + key2;
        }
	}
}
