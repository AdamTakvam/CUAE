using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> Wrapper for the sortedlist, created primarily for serialization </summary>
    public class LdapResultSortedList : SortedList
    {
        public LdapResultComparer Comparer { get { return comparer; } }

        private LdapResultComparer comparer;

        private string firstNameAttr;
        private string lastNameAttr;

        public LdapResultSortedList() : base()
        {
        }

        /// <summary> Requires a comparer indicating which indicates how to treat the inner attributes </summary>
        public LdapResultSortedList(LdapResultComparer comparer, int capacity, string firstNameAttr, string lastNameAttr)
            : base(comparer, capacity)
        {
            this.firstNameAttr = firstNameAttr;
            this.lastNameAttr = lastNameAttr;
            this.comparer = comparer;	
        }

        public string GetFriendlyName(LdapAttributeCollection singleRow)
        {
            string firstName = singleRow[firstNameAttr].Value;
            string lastName = singleRow[lastNameAttr].Value;

            if(firstName != NullLdapAttribute.UndefinedValue && lastName != NullLdapAttribute.UndefinedValue)
            {
                return lastName + ", " + firstName;
            }
            else if(firstName == NullLdapAttribute.UndefinedValue)
            {
                return lastName + ", ???";
            }
            else if(lastName == NullLdapAttribute.UndefinedValue)
            {
                return "???, " + firstName;
            }
            else
            {
                return "???, ???";
            }
            
        }
    }
}
