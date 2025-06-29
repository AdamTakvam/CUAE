using System;
using System.Collections;

using Metreos.ApplicationFramework.ResultData;

namespace Metreos.ApplicationFramework.Collections
{
	public class ResultDataCollection
	{
        // Field -> Result data object
        private Hashtable resultData;

        public int Count { get { return resultData.Count; } }

        public ResultDataBase this[string field]
        {
            get { return resultData[field] as ResultDataBase; }
            set { Add(field, value); }
        }

        public ResultDataCollection()
        {
            resultData = new Hashtable();
        }

        public void Add(string field, ResultDataBase rd)
        {
            resultData.Add(field, rd);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return resultData.GetEnumerator();
        }

        public void Clear()
        {
            resultData.Clear();
        }

        public ResultDataCollection Clone()
        {
            ResultDataCollection _new = new ResultDataCollection();

            IDictionaryEnumerator de = resultData.GetEnumerator();
            while(de.MoveNext())    
            {
                string field = de.Key as String;
                ResultDataBase rd = de.Value as ResultDataBase;
                _new.resultData.Add(field, rd.Clone());
            }

            return _new;
        }
	}
}
