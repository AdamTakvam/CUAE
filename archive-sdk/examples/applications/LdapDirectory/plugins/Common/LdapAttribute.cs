using System;
using System.Xml.Serialization;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> 
	///           Encapsulates a single attribute returned by the LDAP server, containing single values
	///           or single values      
    /// </summary>
    [Serializable]
	public class LdapAttribute
	{
        public static string UndefinedValue = String.Empty;

        [XmlElement("Name")]
        public string Name { get { return name; } }
        public int ValueCount { get { return values.Count; } }
        public bool MultiValue { get { return values.Count > 1; } }

        public virtual string Value 
        { 
            get 
            { 
                if(values.Count != 0)
                {
                    return values[0];
                }
                else
                {
                    return UndefinedValue;
                }
            } 
        }
        
        public string[] Values
        {
            get{ return values.GetAllValues(); } 
        }

        private string name;

        [XmlElement("Values")]
        protected LdapAttributeValueCollection values;

        /// <summary> Serialization req constructor </summary>
        public LdapAttribute()
        {  
            values = new LdapAttributeValueCollection();
        } 

        public LdapAttribute(string name) : this()
        {
            this.name = name;
           
        }

		public LdapAttribute(string name, params string[] attributeValues) : this(name)
		{
            if(attributeValues == null) { return; }

            foreach(string attributeValue in attributeValues)
            {
                if(attributeValue == null) { continue; }

                values.Add(attributeValue);
            }
		}

        public virtual void AddValue(string attributeValue)
        {
            if(attributeValue == null) { return; }

            values.Add(attributeValue);
        }
	}
}
