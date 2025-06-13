using System;
using System.Xml.Serialization;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> 
	///           Encaspulates an LDAP attribute, and whether that attribute should be 
	///           used to sort ascending or descending
	/// </summary>
	[Serializable]
	public class LdapCompareAttribute
	{
        [XmlElement("Name")]
        public string AttributeName { get { return attribute; } set { attribute = value; } }

        [XmlElement("Order")]
        public SearchOrder Order { get { return order; } set { order = value; } }

        private string attribute;
        private SearchOrder order;

        /// <summary> 
        ///           Creates a compare unit, which is shares a 1:1 relationship to a LDAP attribute. 
        ///           The order is assumed to be Ascending      
        /// </summary>
        /// <param name="attribute"> true indicates to sort ascending using this attribute </param>
        public LdapCompareAttribute(string attribute)
        {
            this.attribute = attribute;
            this.order = SearchOrder.Ascending;
        }

        /// <summary> Creates a compare unit, which is shares a 1:1 relationship to a LDAP attribute </summary>
        /// <param name="attribute"> The attribute </param>
        /// <param name="order"> true indicates to sort ascending using this attribute s</param>
		public LdapCompareAttribute(string attribute, SearchOrder order)
		{
            this.attribute = attribute;
            this.order = order;
        }
	}

    public enum SearchOrder
    {
        Ascending,
        Descending,
    }
}
