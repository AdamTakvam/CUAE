using System;
using System.Diagnostics;
using System.Collections;
using System.Reflection;

namespace Metreos.ApplicationFramework
{
    /// <summary>
    ///     If a native action is to wrap a collection, and behave like a collection, 
    ///     a common paradigm would be to make the collection an inner member, and by way
    ///     of manual, repetitive programming, one could make the native type 
    ///     act and feel like a collection.  This abstract class makes it so that one 
    ///     does not have to do the implentation-of-IList leg work.
    /// </summary>
    public abstract class CollectionVariableBase : IList, IVariable
    {
        protected IList collection;
        private ConstructorInfo defaultConstructor;
        private Type collectionType;

        public CollectionVariableBase(Type collectionType)
        {
            this.collectionType     = collectionType;
            this.defaultConstructor = collectionType.GetConstructor(System.Type.EmptyTypes);            
            this.InitializeCollection();
        }

        /// <summary>
        ///     In two cases this method is invoked. Once when the IVariable is
        ///     constructed, as well as when the assigned ICollection is null.
        ///     
        ///     The default behavior of this method is to invoke default constructor of
        ///     the collection.  If there is not a default constructor, this should be 
        ///     overridden.
        /// </summary>
        public virtual void InitializeCollection()
        {
            Debug.Assert(defaultConstructor != null,
                String.Format(
                    "The collection {0} has no default constructor.\n" + 
                    "Override InitializeCollection() to initialize the inner collection according to proper default behavior", 
                        collectionType.FullName ));

            this.collection = (IList) defaultConstructor.Invoke(null);
        }

        public bool Parse(IList collection)
        {
            if(collection != null)
            {
                this.collection = collection;
            }
            else
            {
                InitializeCollection();
            }
            return true;
        }

        #region IVariable Members

        public bool Parse(string value)
        {
            return false;
        }
        #endregion

        #region ICollection Members

        public bool IsSynchronized { get { return collection.IsSynchronized; } }

        public int Count { get { return collection.Count; } }

        public void CopyTo(Array array, int index)
        {
            collection.CopyTo(array, index);
        }

        public object SyncRoot { get { return collection.SyncRoot; } }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {   
            return collection.GetEnumerator();
        }

        #endregion

        #region IList Members

        public bool IsReadOnly { get { return collection.IsReadOnly; } }

        public object this[int index]
        { 
            get { return collection[index]; }
            set { collection[index] = value; }
        }

        public void RemoveAt(int index)
        {
            collection.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            collection.Insert(index, value);
        }

        public void Remove(object value)
        {
            collection.Remove(value);
        }

        public bool Contains(object value)
        {   
            return collection.Contains(value);
        }

        public void Clear()
        {
            collection.Clear();
        }

        public int IndexOf(object value)
        {
            return collection.IndexOf(value);
        }

        public int Add(object value)
        {
            return collection.Add(value);
        }

        public bool IsFixedSize { get { return collection.IsFixedSize; } }

        #endregion
    }

}
