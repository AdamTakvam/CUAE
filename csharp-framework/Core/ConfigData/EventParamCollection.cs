using System;
using System.Collections;

namespace Metreos.Core.ConfigData
{
    /// <summary>
    /// Collection of EventParam objects.
    /// Supports multiple parameters with the same name.
    /// Index by name.
    /// </summary>
    public class EventParamCollection : IEnumerable
    {
        private ArrayList _params;

        public EventParam this[int index]
        {
            get { return _params[index] as EventParam; }
        }

        public object this[string paramName]
        {
            get { return GetParam(paramName); }
        }

        public int Count { get { return _params.Count; } }

        public EventParamCollection()
        {
            _params = new ArrayList();
        }

        public void Add(string name, object Value)
        {
            Add(new EventParam(EventParam.Type.Literal, name, Value));
        }

        public void Add(EventParam param)
        {
            _params.Add(param);
        }

        public object GetParam(string name)
        {
            EventParam eParam;
            for(int i=0; i<_params.Count; i++)
            {
                eParam = (EventParam) _params[i];
                if(String.Compare(eParam.name, name, true) == 0)
                {
                    return eParam.Value;
                }
            }
            return null;
        }

        public void Clear()
        {
            _params.Clear();
        }

        public EventParamCollection Clone()
        {
            EventParamCollection _new = new EventParamCollection();
            
            EventParam param = null;
            for(int i=0; i<_params.Count; i++)
            {
                param = _params[i] as EventParam;
                _new._params.Add(param.Clone());
            }

            return _new;
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _params.GetEnumerator();
        }

        #endregion
    }
}