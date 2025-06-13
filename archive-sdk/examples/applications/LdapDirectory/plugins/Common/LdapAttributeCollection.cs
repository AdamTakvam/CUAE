using System;
using System.Xml.Serialization;
using System.Collections;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> Encapsulates a row returned by a Ldap Search request </summary>
	[Serializable]
	public class LdapAttributeCollection : CollectionBase
	{
        public new int Count { get { return this.InnerList.Count; } }

        public LdapAttribute this[int i] { get { return this.InnerList[i] as LdapAttribute; } }

        public LdapAttribute this[string attributeName] 
        {
            get
            {
                foreach(LdapAttribute attribute in this.InnerList)
                {
                    if(attribute.Name == attributeName)
                    {
                        return attribute;
                    }
                }

                return NullLdapAttribute.Instance;
            }
        }

        /// <summary> Used for XmlSerialization </summary>
        public LdapAttribute this[LdapAttribute index]
        {
            get
            {
                if(Count == 0)
                {
                    return null;
                }

                foreach(LdapAttribute attribute in this.InnerList)
                {
                    if(attribute == index)
                    {
                        return attribute;
                    }
                }

                return null;
            }
        }

        public LdapAttributeCollection() : base()
        {
        }

        public void Add(LdapAttribute attribute)
        {
            this.InnerList.Add(attribute);
        }
	}
}
