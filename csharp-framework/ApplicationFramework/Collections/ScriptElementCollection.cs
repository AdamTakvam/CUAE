using System;
using System.Collections;

namespace Metreos.ApplicationFramework.Collections
{
	/// <summary>
	/// Collection of objects which inherit from ScriptElementBase.
	/// Indexed by ID.
	/// </summary>
	public class ScriptElementCollection
	{
        private Hashtable elements;

        public int Count { get { return elements.Count; } }

        public ScriptElementBase this[string id]
        {
            get { return elements[id] as ScriptElementBase; }
            set { Add(id, value); }
        }

        public ScriptElementCollection()
        {
            elements = new Hashtable();
        }

        public void Add(string id, ScriptElementBase element)
        {
            elements.Add(id, element);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public bool Contains(string id)
        {
            return elements.Contains(id);
        }

        public void Clear()
        {
            elements.Clear();
        }

        public ScriptElementCollection Clone()
        {
            ScriptElementCollection _new = new ScriptElementCollection();
           
            IDictionaryEnumerator de = elements.GetEnumerator();
            while(de.MoveNext())    
            {
                string id = de.Key as String;
                ScriptElementBase element = de.Value as ScriptElementBase;

                _new.elements.Add(id, element.Clone());
            }

            return _new;
        }
	}
}
