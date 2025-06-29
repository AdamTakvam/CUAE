using System;
using System.Collections;

namespace Metreos.Utilities.Collections
{
	/// <summary>Hashtable optimized for both key and value searches</summary>
	/// <remarks>Fully thread-safe</remarks>
	public class TwoWayHash : IEnumerable
	{
        protected readonly Hashtable keyTable;
        protected readonly Hashtable valueTable;
        
        public virtual int Count { get { return keyTable.Count; } }

        public TwoWayHash()
		{
            this.keyTable = Hashtable.Synchronized(new Hashtable());
            this.valueTable = Hashtable.Synchronized(new Hashtable());
		}

        public virtual bool Add(object key, object Value)
        {
            if(key == null || Value == null)
                return false;

			lock(SyncRoot)
			{
				keyTable[key] = Value;
				valueTable[Value] = key;
			}
            return true;
        }

        public virtual void RemoveByKey(object key)
        {
            if(key == null)
                return;

			lock(SyncRoot)
			{
				object Value = GetByKey(key);
				if(Value != null)
				{
					keyTable.Remove(key);
					valueTable.Remove(Value);
				}
			}
        }

        public virtual void RemoveByValue(object Value)
        {
            if(Value == null)
                return;

			lock(SyncRoot)
			{
				object key = GetByValue(Value);
				if(key != null)
				{
					valueTable.Remove(Value);
					keyTable.Remove(key);
				}
			}
        }

		public bool ContainsKey(object key)
		{
			return keyTable.Contains(key);
		}

		public bool ContainsValue(object Value)
		{
			return valueTable.Contains(Value);
		}

        public virtual object GetByKey(object key)
        {
            return keyTable[key];
        }

        public virtual object GetByValue(object Value)
        {
            return valueTable[Value];
        }

        public virtual void Clear()
        {
			lock(SyncRoot)
			{
				keyTable.Clear();
				valueTable.Clear();
			}
        }

        #region IEnumerable Members

        public virtual object SyncRoot { get { return keyTable.SyncRoot; } }

        public virtual IEnumerator GetEnumerator()
        {
            return keyTable.GetEnumerator();
        }

        #endregion
    }
}
