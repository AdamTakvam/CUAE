using System;
using System.Collections;

namespace Metreos.Types.Http
{
	/// <summary> Implements a simple one-to-one name-as-string/value-as-string collection </summary>
	[Serializable]
    public class NameValueCollection : CollectionBase, IEnumerable
    {
        public NameValuePair this[int i]
        {
            get
            {
                return this.InnerList[i] as NameValuePair;
            }
            set
            {
                this.InnerList[i] = value;
            }
        }

        public NameValuePair this[string name]
        {
         
            get
            {
                for(int i = 0; i < this.InnerList.Count; i++)
                {
                    NameValuePair pair = this.InnerList[i] as NameValuePair;
                    if(0 == String.Compare(pair.Name, name, true))
                    {
                        return pair;
                    }
                }

                return null;
            }
        }
   
        public NameValueCollection() : base()
        {
        }

        public bool Contains(string name)
        {
            foreach(NameValuePair pair in InnerList)
            {
                if(String.Compare(name, pair.Name, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }
        public void Add(NameValuePair nameAndValue)
        {
            this.InnerList.Add(nameAndValue);
        }
    }

    /// <summary> Holds name-value pair.</summary>
    public class NameValuePair
    {
        public string Name { get { return name; } set { name = value; } }
        public string Value {
            get 
            { 
                if(values == null || values.Count == 0)
                {
                    return null;
                }
                else
                {
                    return values[0] as string; 
                }
            } 
            set 
            {
                if(values != null)
                {
                    values.Add(value);
                }
                else
                {
                    values = new ArrayList(new string[] {value});
                }
            } 
        }

        public void AddValue(string value)
        {
            if(values != null)
            {
                values.Add(value);
            }
            else
            {
                values = new ArrayList(new string[] {value});
            }
        }

        public string[] GetValues()
        {
            if(values == null || values.Count == 0)
            {
                return null;
            }

            string[] stringValues = new string[values.Count];
            values.CopyTo(stringValues);
            return stringValues;
        }

        private string name;
        private ArrayList values;

        public NameValuePair(){}
        
        public NameValuePair(string name, string _value) : this()
        {
            this.name = name;
            this.values = new ArrayList(new string[] {_value});
        }
    }
}
