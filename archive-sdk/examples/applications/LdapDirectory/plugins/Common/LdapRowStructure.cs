using System;
using System.Xml.Serialization;

namespace Metreos.LdapDirectory.Common
{
    /// <summary> Holds the entire query </summary>
    [XmlRoot("SearchResult")]
    public class LdapRows
    {
        public LdapRows() {}

        [XmlElement("Comparer")]
        public LdapResultComparer comparer;

        [XmlElement("Rows")]
        public LdapRowStructure[] ldapRows;
    }

	/// <summary> Holder for information pertaining to a single result row of a person/telephone number </summary>
	[XmlRoot("Row")]
	public class LdapRowStructure
	{
		public LdapRowStructure(){}

        [XmlElement("AttributeInfo")]
        public LdapAttributeCollection attributes;

        [XmlElement("teleNum")]
        public string LdapTelephoneNumber;
	}
}
