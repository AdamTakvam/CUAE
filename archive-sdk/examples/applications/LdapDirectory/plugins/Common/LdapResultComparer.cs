using System;
using System.Collections;
using System.Xml.Serialization;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> Comparer used exclusively for LDAP search results, using attributes to sort with</summary>
	/// <remarks> 
	///           Right now we are doing only a text compare.  
	///           TODO: Some sort of system needs to be defined for comparing non-text attribute types
	/// </remarks>
	[Serializable]
	public class LdapResultComparer : IComparer
	{
        private const int lessThan = -1;
        private const int equalTo = 0;
        private const int greaterThan = 1;

        [XmlElement("IndividualComparers")]
        protected LdapCompareAttributeCollection comparers; 

        public LdapResultComparer() {}

		public LdapResultComparer(LdapCompareAttributeCollection comparers)
		{
	        this.comparers = comparers;
		}

        public int Compare(object x, object y)
        {
            LdapAttributeCollection result1 = x as LdapAttributeCollection;
            LdapAttributeCollection result2 = y as LdapAttributeCollection;

            foreach(LdapCompareAttribute compare in comparers)
            {
                // Pull out the attribute that corresponds to what we want to
                // compare at the moment, from the result row, for both items
                // to compare.
                LdapAttribute attribute1 = result1[compare.AttributeName];
                LdapAttribute attribute2 = result2[compare.AttributeName];
    
                // The first 3 conditional 'if's test for the special String.Empty case, because comparing
                // String.Empty ("") to a word results in string occurring first, which would probably
                // put a bunch of bs at the front of the results. (Then again, it could be a good thing...
                // it's sorta hard to say...)

                // If both have undefined values for the attribute, then we just gotta try the next
                // level
                if(attribute1.Value == LdapAttribute.UndefinedValue
                    && attribute2.Value == LdapAttribute.UndefinedValue)
                {
                    continue;
                }
                else if(attribute1.Value == LdapAttribute.UndefinedValue)
                {
                    return greaterThan;
                }
                else if(attribute2.Value == LdapAttribute.UndefinedValue)
                {
                    return lessThan;
                }
                else
                {
                    // The text-only compare culprit occuring (need to implement integer comparison
                    int compareResult = Comparer.DefaultInvariant.Compare(attribute1.Value, attribute2.Value);
                    
                    // Continue the searching.
                    if(compareResult == 0)
                    {
                        continue;
                    }
                    else
                    {
                        return compareResult;
                    }
                }
            }

            // If this point is reached, we know that the dern things are quite matching.
            return equalTo;
        }
	}
}
