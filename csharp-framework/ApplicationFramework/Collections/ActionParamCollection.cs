using System;
using System.Collections;

using Metreos.ApplicationFramework.ActionParameters;

namespace Metreos.ApplicationFramework.Collections
{
    /// <summary>
    /// Collection of object which inherit from ActionParamBase.
    /// Supports multiple parameters with the same name.
    /// Index by name.
    /// </summary>
	public class ActionParamCollection : IEnumerable
	{
        /// <summary>Parameter name (string) -> ArrayList of ActionParamBase</summary>
        private Hashtable _params;

        public object this[string paramName]
        {
            get { return GetParam(paramName); }
        }

		public ActionParamCollection()
		{
            _params = new Hashtable();
		}

        public void Add(ActionParamBase param)
        {
            ArrayList a = _params[param.name] as ArrayList;
            
            if(a == null)
                a = new ArrayList();

            a.Add(param);
            _params[param.name] = a;
        }

        public bool Contains(string name)
        {
            return _params.Contains(name);
        }

        /// <summary>Returns the single value of a paramter</summary>
        /// <remarks>If the parameter has multiple values, null is returned</remarks>
        /// <param name="name">parameter name</param>
        /// <returns>parameter value</returns>
        public object GetParam(string name)
        {
            ArrayList a = _params[name] as ArrayList;
            if(a != null && a.Count == 1)
            {
                ActionParamBase aParam = a[0] as ActionParamBase;
                return aParam.Value;
            }
            return null;
        }

        public ArrayList GetParams(string name)
        {
            return _params[name] as ArrayList;
        }

        public string GetParamAsString(string name)
        {
            return GetParam(name) as string;
        }

        public void Remove(string name)
        {
            _params.Remove(name);
        }

        public void Reset()
        {
            foreach(ArrayList a in _params.Values)
            {
                foreach(ActionParamBase aParam in a)
                {
                    aParam.Reset();
                }
            }
        }

        public void Clear()
        {
            _params.Clear();
        }

        public ActionParamCollection Clone()
        {
            ActionParamCollection _new = new ActionParamCollection();

            foreach(DictionaryEntry de in _params)
            {
                ArrayList a = new ArrayList();

                foreach(ActionParamBase aParam in de.Value as ArrayList)
                {
                    a.Add(aParam.Clone());
                }

                _new._params[de.Key] = a;
            }
            return _new;
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            ArrayList fullList = new ArrayList();

            foreach(ArrayList a in _params.Values)
            {
                fullList.AddRange(a);
            }

            return fullList.GetEnumerator();
        }

        #endregion
    }
}
