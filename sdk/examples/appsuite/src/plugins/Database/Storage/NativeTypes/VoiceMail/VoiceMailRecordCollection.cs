using System;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Types
{
    /// <summary>
    /// Summary description for VoiceMailRecord.
    /// </summary>
    public class VoiceMailRecordCollection : IVariable, IEnumerable, ICollection
    {
        public SortOrder SortingOrder
        {
            get { return sortingOrder; }
            set { sortingOrder = value; }
        }
        private SortOrder sortingOrder;

        private ArrayList records;
        private Hashtable recordMap;
        private object syncObject;

        public VoiceMailRecordCollection()
        {
            records = new ArrayList();
            recordMap = new Hashtable();
            syncObject = new object();
            this.sortingOrder = SortOrder.Increasing;
        }

        public VoiceMailRecord this[int index]
        {
            get
            {
                try
                {
                    return records[index] as VoiceMailRecord; 
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                records[index] = value;
            }
        }

        // using recordId will create problems, need to change this.
        public int Add(VoiceMailRecord record)
        {
            int index;
            if (recordMap.ContainsKey(record.Id))
            {
                index = Convert.ToInt32(recordMap[record.Id]);
                record.IsDirty = true;
                records[index] = record;
            }
            else
            {
                index = records.Add(record);
                record.IsNew = true;
                recordMap.Add(record.Id, index);
            }
            
            this.Sort();
            return index;
        }

        public void Sort()
        {
            this.Sort(sortingOrder);
        }

        public void Sort(SortOrder order)
        {
            VoiceMailRecord record;
            switch(order)
            {
                case SortOrder.Increasing :
                    records.Sort();
                    recordMap.Clear();

                    for (int i = 0; i < records.Count; i++)
                    {
                        record = records[i] as VoiceMailRecord;
                        recordMap.Add(record.Id, i);
                    }
                    break;
                case SortOrder.Decreasing :
                    records.Sort();
                    records.Reverse();
                    for (int i = 0; i < records.Count; i++)
                    {
                        record = records[i] as VoiceMailRecord;
                        recordMap.Add(record.Id, i);
                    }
                    break;
                default : break;
            }
        }

        #region IVariable Members
        
        [TypeInput("string", "Takes only another VoiceMailRecordCollection object, so this always returns false")]
        public bool Parse(string str)
        {
            return false;
        }

        [TypeInput("object", "Takes only another VoiceMailRecordCollection object")]
        public bool Parse(object obj)
        {
            if (!(obj is VoiceMailRecordCollection))
                return false;
            
            VoiceMailRecordCollection collection = obj as VoiceMailRecordCollection;
            records.Clear();
            recordMap.Clear();
            
            foreach (VoiceMailRecord record in collection)
            {
                this.Add(record);    
            }

            return true;
        }
        #endregion

        #region ICollection Members

        public bool IsSynchronized { get { return true; } }

        public int Count { get { return records.Count; } }

        public void CopyTo(Array array, int index)
        {
            lock(syncObject)
            {
                foreach(VoiceMailRecord record in records)
                {
                    array.SetValue(new VoiceMailRecord(record.Id, record.UserId, record.Filename, 
                                                            record.Status, record.Length, record.TimeStamp), index++);
                }
            }
        }

        public object SyncRoot { get { return syncObject; } }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return records.GetEnumerator();
        }

        #endregion
    }
}
