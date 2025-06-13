using System;
using System.Collections;
using System.Xml.Serialization;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> 
	///           Encaspulates an LDAP attribute collection.
	/// </summary>
	[Serializable]
	public class LdapCompareAttributeCollection : CollectionBase
	{
        public LdapCompareAttribute this[int i]
        {
            get 
            {
                return this.List[i] as LdapCompareAttribute;
            }
        }

        /// <summary> Implemented for XmlSerialization </summary>
        public LdapCompareAttribute this[LdapCompareAttribute index]
        {
            get
            {
                if(Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach(LdapCompareAttribute attribute in this.InnerList)
                    {
                        if(attribute == index)
                        {
                            return attribute;
                        }
                    }

                    return null;
                }
            }
        }

        public new int Count { get { return this.List.Count; } }

        public LdapCompareAttributeCollection() : base()
        {
        }

        // TODO : USE ORDER TO SORT
        public void Add(LdapCompareAttribute attribute)
        {
            this.List.Add(attribute);
        }

        
	}
}
