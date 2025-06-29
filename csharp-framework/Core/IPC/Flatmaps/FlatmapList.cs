using System;
using System.Collections;


namespace Metreos.Core.IPC.Flatmaps
{
    /// <summary>A collection which acts like a map which permits duplicate keys</summary>
    /// <remarks>
    /// A list of this type accepts only flatmap-legal data. A key must be a 32-bit integer,
    /// and a value must be one of integer, string, byte[], or a byte[] binary flatmap.   
    /// </remarks>
    /// <remarks>
    /// A list of this type can be converted to and from a binary flatmap. To create a 
    /// FlatmapList from a binary flatmap, pass the flatmap to this list's constructor. 
    /// To create a binary flatmap from a FlatmapList, build a list and invoke its 
    /// ToFlatmap method.  
    /// </remarks>
    [Serializable]
    public class FlatmapList: ICollection, IDictionary, IEnumerable 
    {
        /// <summary>Create an empty FlatmapList</summary>
        public FlatmapList() 
        { 
        }

        /// <summary>Create a read-only FlatmapList from a binary flatmap</summary>
        /// <exception cref="FlatmapException">If flatmap is invalid</exception>
        public FlatmapList(byte[] flatmap)
        {
            Flatmap.FromFlatmap(flatmap, this);
            this.immutable = true;
        }


        /// <summary>Create a possibly modifiable FlatmapList from a binary flatmap</summary>
        /// <exception cref="FlatmapException">If flatmap is invalid</exception>
        public FlatmapList(byte[] flatmap, bool isModifiable)
        {
            Flatmap.FromFlatmap(flatmap, this);
            this.immutable = !isModifiable;
        }


        /// <summary>Convert this list to a binary flatmap</summary>
        /// <exception cref="FlatmapException">If list cannot be converted</exception>
        public byte[] ToFlatmap()
        {
            return this.ToFlatmap(null);
        }


        /// <summary>Convert this list to a binary flatmap</summary>
        /// <param name="headerExtension">Extra header to be embedded if any</param>
        /// <exception cref="FlatmapException">If list cannot be converted</exception>
        public byte[] ToFlatmap(byte[] headerExtension)
        {
            return Flatmap.ToFlatmap(this, headerExtension);
        }


        /// <summary>Precalculate byte length of a flatmap created from current list</summary>
        /// <param name="extraHeaderLength">If a header extension is to be included 
        /// in the calculation, the length of that extra header; otherwise zero</param>
        public int BinaryFlatmapLength(int extraHeaderLength)
        {
            return Flatmap.FlatmapLengthFromList(this, extraHeaderLength);               
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // List maintenance 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add list entry for specified key and value</summary>
        /// <exception cref="ArgumentException">If object value is not a flatmap type</exception>
        /// <exception cref="InvalidOperationException">If list is read-only</exception>
        public void Add(uint key, object xval)
        {
            if (this.immutable) throw new InvalidOperationException();

            Flatmap.ValueType valueType = Flatmap.ValidateValue(xval);
            if (valueType == Flatmap.ValueType.None)
                throw new ArgumentException("FlatmapList: valueType is None");

            Flatmap.MapEntry entry = new Flatmap.MapEntry(key, valueType, xval);             
            list.Add(entry);
            this.sorted = false;
        }


        /// <summary>Add list entry for specified key and value</summary>
        /// <exception cref="ArgumentException">If object key is not a uint</exception>
        /// <exception cref="ArgumentException">If object value is not a flatmap type</exception>
        /// <exception cref="InvalidOperationException">If list is read-only</exception>
        public void Add(object xkey, object xval)
        {
            if (this.immutable) throw new InvalidOperationException();

            uint key = this.UintKey(xkey);  

            this.Add(key, xval);          
        }


        /// <summary>Remove first list entry for specified key</summary>
        /// <exception cref="ArgumentException">If object key is not a uint</exception>
        /// <exception cref="IndexOutOfRangeException">If entry nonexistent</exception>
        /// <exception cref="InvalidOperationException">If list is read-only</exception>
        public void Remove(object xkey)
        {
            if (this.immutable) throw new InvalidOperationException();

            uint key = this.UintKey(xkey);  
            Flatmap.MapEntry entry = this.Find(key,1);
            if (entry.dataType == Flatmap.ValueType.None)
                throw new IndexOutOfRangeException();

            this.list.Remove(entry);
        }


        /// <summary>Remove list entry at specified index</summary>
        /// <exception cref="IndexOutOfRangeException">If entry nonexistent</exception>
        /// <exception cref="InvalidOperationException">If list is read-only</exception>
        public void RemoveAt(int index)
        {
            if (this.immutable) throw new InvalidOperationException();

            Flatmap.MapEntry entry = this.GetAt(index);
            if (entry.dataType == Flatmap.ValueType.None)
                throw new IndexOutOfRangeException();
          
            this.list.Remove(entry);
        }   


        /// <summary>Remove list entry for specified key and occurence</summary>
        /// <exception cref="IndexOutOfRangeException">If entry nonexistent</exception>
        /// <exception cref="InvalidOperationException">If list is read-only</exception>
        public void Remove(uint key, int occurrence)
        {
            if (this.immutable) throw new InvalidOperationException();

            Flatmap.MapEntry entry = this.Find(key, occurrence);          
            if (entry.dataType == Flatmap.ValueType.None)
                throw new IndexOutOfRangeException();

            this.list.Remove(entry);
        }


        /// <summary>Remove list entry for specified key, value, and occurence</summary>
        /// <exception cref="IndexOutOfRangeException">If entry nonexistent</exception>
        /// <exception cref="InvalidOperationException">If list is read-only</exception>
        public void Remove(uint key, object val, int occurrence)
        {
            if (this.immutable) throw new InvalidOperationException();

            Flatmap.MapEntry entry = this.Find(key, val, occurrence);          
            if (entry.dataType == Flatmap.ValueType.None)
                throw new IndexOutOfRangeException();

            this.list.Remove(entry);
        }


        public void Clear() 
        {   
            this.list.Clear(); 
            this.immutable = this.sorted = false;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // List content query 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Indicate if list contains specified key</summary>  
        public bool Contains(uint key)
        {
            foreach(Flatmap.MapEntry entry in this.list)             
                if (entry.key == key) return true;             

            return false;
        }


        /// <summary>Indicate if list contains specified value</summary>
        public bool Contains(object xval)
        {
            foreach(Flatmap.MapEntry entry in this.list)             
                 if(entry.dataValue == xval) return true;
              
            return false;           
        }


        /// <summary>Indicate if list contains specified occurrence of specified key</summary>
        public bool Contains(uint key, int occurrence)
        {
            Flatmap.MapEntry entry = this.Find(key, occurrence);          
            return entry.dataType != Flatmap.ValueType.None;
        }


        /// <summary>Indicate if list contains occurrence of specified key and value</summary>
        public bool Contains(uint key, object val, int occurrence)
        {
            Flatmap.MapEntry entry = this.Find(key, val, occurrence);            
            return entry.dataType != Flatmap.ValueType.None;
        }


        /// <summary>Return list entry for specified key and occurence</summary>
        /// <returns>Entry values, or { 0, 0, null } </returns>
        public Flatmap.MapEntry Find(uint key, int occurrence)
        {
            int  occurrencesThisKey = 0;
            int  desiredOccurrence  = occurrence > 0? occurrence: 1;
            this.Sort();

            foreach(Flatmap.MapEntry entry in this.list) 
            {
                if (entry.key < key) continue;
                if (entry.key > key) break;
                if (++occurrencesThisKey < desiredOccurrence) continue; 
                return entry;
            } 

            return new Flatmap.MapEntry();
        }


        /// <summary>Return list entry for specified key, value, and occurence</summary>
        /// <returns>Entry values, or { 0, 0, null } </returns>
        public Flatmap.MapEntry Find(uint key, object val, int occurrence)
        {
            int  occurrencesThisKey = 0;
            int  desiredOccurrence  = occurrence > 0? occurrence: 1;
            this.Sort();

            foreach(Flatmap.MapEntry entry in this.list) 
            {
                if (entry.key < key) continue;
                if (entry.key > key) break;
                if (entry.dataValue != val) continue;
                if (++occurrencesThisKey < desiredOccurrence) continue; 
                return entry;
            } 

            return new Flatmap.MapEntry();
        }


        /// <summary>Return list entry at specified index</summary>
        /// <returns>Entry values, or { 0, 0, null } </returns>
        public Flatmap.MapEntry GetAt(int n)
        {
            this.Sort();
            IEnumerator ix = this.list.GetEnumerator();
            int i = 0;

            while(ix.MoveNext()) 
            {  
                if (i++ != n) continue; 
                Flatmap.MapEntry entry = (Flatmap.MapEntry) ix.Current;
                return entry;
            }

            return new Flatmap.MapEntry();
        }


        public void CopyTo(Array array, int index)
        {
            this.list.CopyTo(array, index);
        }


        public ICollection Keys 
        {   get 
            {   ArrayList keylist = new ArrayList();
                foreach(Flatmap.MapEntry entry in this.list) keylist.Add(entry.key);
                return keylist;
            }
        }


        public ICollection Values 
        {   get 
            {   ArrayList vlist = new ArrayList();
                foreach(Flatmap.MapEntry entry in this.list) vlist.Add(entry.dataValue);
                return vlist;
            }
        }


        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new FlatmapListEnumerator(this.list);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FlatmapListEnumerator(this.list);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // List sort 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public class MapEntryComparer: IComparer  
        {
            int IComparer.Compare(Object x, Object y)  
            {
                Flatmap.MapEntry a = (Flatmap.MapEntry) x;
                Flatmap.MapEntry b = (Flatmap.MapEntry) y;
                return a.key < b.key? -1: a.key > b.key? 1: 0;
            }
        }


        /// <summary>Sorts the list if not currently sorted</summary>
        public void Sort()
        {
            if (!this.sorted) this.list.Sort(new MapEntryComparer());
            this.sorted = true;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Support methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Convert object key to uint</summary>
        /// <exception cref="ArgumentException">If object is not a uint</exception>
        protected uint UintKey(object xkey)
        {
            uint key = 0;
            try  { key = (uint)xkey; }  
            catch{ throw new ArgumentException( "xkey is not uint" ); }
            return key;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected ArrayList list = new ArrayList();  
        protected bool immutable, sorted;

        public bool IsFixedSize     { get { return false;} }
        public bool IsSynchronized  { get { return false;} }       
        public object SyncRoot      { get { return this; } } 
        public int  Count           { get { return this.list.Count;} }
        public bool IsReadOnly      { get { return this.immutable; } }  
        public object this[int i]   { get { return this.GetAt((int)i); } }
        public object this[object k]
        { get { return null; }  set { throw new InvalidOperationException();} }

        /// <summary>Flatmap header extension</summary>
        /// <remarks>If a custom header extension was serialized with a flatmap,
        /// the header is placed here when the flatmap is deserialized</remarks>
        protected byte[] headerExtension;
        public    byte[] HeaderExtension 
        { get { return headerExtension;  }
          set { headerExtension = value; } 
        }

    } // class FlatmapList



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // FlatmapListEnumerator
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class FlatmapListEnumerator: IDictionaryEnumerator
    {
        private int index = -1;
        private ArrayList list;

        internal FlatmapListEnumerator(ArrayList list) { this.list = list; }        

        public bool MoveNext()  { return ++index < list.Count; }
        public void Reset()     { index = -1; }

        public object Current 
        {
            get { return index < 0 || index >= list.Count? null: list[index]; }
        }

        public DictionaryEntry Entry 
        {   get 
            {   Flatmap.MapEntry entry = (Flatmap.MapEntry) this.Current;
                return entry.dataType  == Flatmap.ValueType.None?
                    new DictionaryEntry(null, null):
                    new DictionaryEntry(entry.key, entry.dataValue);            
            }
        }

        public object Key   { get { return Entry.Key;   } }
        public object Value { get { return Entry.Value; } }

    }   //  class FlatmapListEnumerator

}   // namespace