using System;
using System.Collections;
using System.Xml.Serialization;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> Contains the values of a single attribute </summary>
	[Serializable]
    public class LdapAttributeValueCollection : CollectionBase
    {
        public string this[int i]
        {
            get { return this.InnerList[i] as string; }
        }

        /// <summary> Created for XmlSerialization </summary>
        public string this[string index]
        {
            get
            {
                if(Count == 0)
                {
                    return null;
                }

                foreach(string oneValue in this.InnerList)
                {
                    if(oneValue == index)
                    {
                        return oneValue;
                    }
                }

                return null;
            }
        }

        public new int Count { get { return this.InnerList.Count; } }

		public LdapAttributeValueCollection() : base()
		{
		}

        public void Add(string value)
        {
            this.InnerList.Add(value);
        }
        
        /// <summary> Returns all values defined by this attribute </summary>
        /// <returns> null if count == 0</returns>
        public string[] GetAllValues()
        {
            if(this.InnerList.Count == 0) 
            {
                return null;
            }

            string[] values = new string[this.InnerList.Count];

            this.InnerList.CopyTo(values);

            return values;
        }

	}
}
