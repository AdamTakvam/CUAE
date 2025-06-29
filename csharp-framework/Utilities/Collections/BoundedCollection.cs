using System;
using System.Collections;

namespace Metreos.Utilities.Collections
{
	/// <summary>
	/// Fixed-length collection which can be added to indefinitely. 
	/// When a new item is added after the collection is full, the oldest item is dropped.
	/// </summary>
	/// <remarks>This collection is threadsafe for multiple readers and writers.</remarks>
	public class BoundedCollection : IEnumerable
	{
        protected readonly int size;
        public virtual int Size { get { return size; } }

        protected readonly ArrayList a;

        public object SyncRoot { get { return a.SyncRoot; } }

		public BoundedCollection(int size)
		{
            if(size <= 0)
                throw new ArgumentException("Invalid collection size specified", "size");

            this.size = size;
            this.a = ArrayList.Synchronized(new ArrayList(size));
		}

        /// <summary>Adds an item to the collection</summary>
        /// <param name="obj">Item to add</param>
        /// <returns>Item which was dropped or null</returns>
        public virtual object Add(object obj)
        {
            lock(SyncRoot)
            {
                a.Add(obj);

                if(a.Count > size)
                {
                    object item = a[0];
                    a.RemoveAt(0);
                    return item;
                }
                return null;
            }
        } 

        /// <summary>Removes the item with the specified key from the hash table</summary>
        public virtual void Remove(object key)
        {
            a.Remove(key);
        }
       
        /// <summary>Returns whether the specified item exists in the collection</summary>
        /// <param name="obj">Item to find</param>
        /// <returns>true or false</returns>
        public virtual bool Contains(object obj)
        {
            return a.Contains(obj);
        }

        /// <summary>Removes all items from the collection</summary>
        public virtual void Clear()
        {
            a.Clear();
        }

        #region IEnumerable Members

        public virtual IEnumerator GetEnumerator()
        {
            return a.GetEnumerator();
        }

        #endregion
    }
}
