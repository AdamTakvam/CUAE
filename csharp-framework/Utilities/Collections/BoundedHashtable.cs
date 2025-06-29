using System;
using System.Collections;


namespace Metreos.Utilities.Collections
{
	/// <summary>
	/// Fixed-length hash table which can be added to indefinitely. 
	/// When a new item is added after the collection is full, the oldest item is dropped.
	/// </summary>
	/// <remarks>This collection is threadsafe for multiple readers and writers.</remarks>
	public class BoundedHashtable : BoundedCollection
	{
		protected readonly Hashtable h;

		public BoundedHashtable(int size)
            : base(size)
		{
			this.h = new Hashtable(size);
		}

		/// <summary>Getter returns whether the specified key exists in the hash table</summary>
		/// <param name="obj">Key to find</param>
		/// <returns>the item for the key or null if doesn't exist</returns>
		/// 
		/// <summary>Setter adds an item to the hash table for the given key</summary>
		/// <param name="key">Key to the item to be added</param>
		/// <param name="obj">Item to be added</param>
		/// <returns>None</returns>
		public virtual object this[object key]
		{
			get
			{
				lock(SyncRoot)
				{
					return h[key];
				}
			}
			set
			{
				Add(key, value);
			}
		}

        public override object Add(object key)
        {
            return Add(key, null);
        }

        public object Add(object key, object _value)
        {
            lock(SyncRoot)
            {
                h[key] = _value;
                a.Add(key);

                if(a.Count > size)      //remove last one
                {
                    object k = a[0];
                    a.RemoveAt(0);
                    h.Remove(k);
                    return k;
                }
                return null;
            }
        }


        /// <summary>Removes the item with the specified key from the hash table</summary>
        public override void Remove(object key)
        {
            lock(SyncRoot)
            {
                h.Remove(key);
                base.Remove(key);
            }
        }

		/// <summary>Removes all items from the hash table</summary>
		public override void Clear()
		{
			lock(SyncRoot)
			{
				h.Clear();
                base.Clear();
			}
		}

		#region IEnumerable Members

		public override IEnumerator GetEnumerator()
		{
			return h.GetEnumerator();
		}

		#endregion
	}

}