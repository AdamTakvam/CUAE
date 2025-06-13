using System;
using System.Collections;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Native.CBridge
{
	public class BridgeParticipants : IVariable, ICollection, IList
	{
		private ArrayList participants;

        public BridgeParticipants()
		{
            participants = new ArrayList();
        }
       
        #region IVariable Members

        public bool Parse(string str)
        {
            // TODO:  Add BridgeParticipants.Parse implementation
            return false;
        }

        public bool Parse(object obj)
        {
            BridgeParticipants pParticipants = obj as BridgeParticipants;
            foreach (Participant p in pParticipants)
                this.Add(p);

            return (pParticipants != null);
        }
        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add BridgeParticipants.IsSynchronized getter implementation
                return participants.IsSynchronized;
            }
        }

        public int Count
        {
            get
            {
                // TODO:  Add BridgeParticipants.Count getter implementation
                return participants.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            foreach (Participant p in participants)
            {
                array.SetValue(new Participant(p), index++);
            }
        }

        public object SyncRoot
        {
            get
            {
                // TODO:  Add BridgeParticipants.SyncRoot getter implementation
                return participants.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            // TODO:  Add BridgeParticipants.GetEnumerator implementation
            return participants.GetEnumerator();
        }

        #endregion

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                // TODO:  Add BridgeParticipants.IsReadOnly getter implementation
                return participants.IsReadOnly;
            }
        }

        public object this[int index]
        {
            get
            {
                // TODO:  Add BridgeParticipants.this getter implementation
                return participants[index];
            }
            set
            {
                participants[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            participants.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            participants.Insert(index, value);
        }

        public void Remove(object value)
        {
            participants.Remove(value);
        }

        public bool Contains(object value)
        {
            return participants.Contains(value);
        }

        public void Clear()
        {
            participants.Clear();
        }

        public int IndexOf(object value)
        {
            return participants.IndexOf(value);
        }

        public int Add(object value)
        {
            return participants.Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                // TODO:  Add BridgeParticipants.IsFixedSize getter implementation
                return participants.IsFixedSize;
            }
        }

        #endregion
    }
}
