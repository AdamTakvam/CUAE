using System;
using System.Collections;

namespace Metreos.ApplicationFramework.Collections
{
	/// <summary>
	/// Collection of Variable objects.
	/// Names must be unique.
	/// Index by name.
	/// </summary>
	[Serializable]
	public class VariableCollection
	{
        Hashtable variables;

        public Variable this[string name]
        {
            get { return variables[name] as Variable; }
            set { Add(name, value); }
        }

        public int Count { get { return variables.Count; } }

        public ICollection Values { get { return variables.Values; } }

		public VariableCollection()
		{
            variables = new Hashtable();
		}

        public void Add(string name, Variable variable)
        {
            variables[name] = variable;
        }

        public bool Contains(string name)
        {
            return variables.Contains(name);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return variables.GetEnumerator();
        }

        public void Clear()
        {
            variables.Clear();
        }

        public void Reset(Metreos.LoggingFramework.LogWriter log)
        {
            foreach(Variable var in variables.Values)
            {
                var.Reset(log);
            }
        }

        public VariableCollection Clone()
        {
            VariableCollection _new = new VariableCollection();
            
            IDictionaryEnumerator de = variables.GetEnumerator();
            while(de.MoveNext())
            {
                string id = de.Key as String;
                Variable variable = de.Value as Variable;

                _new.variables.Add(id, variable.Clone());
            }

            return _new;
        }
	}
}
