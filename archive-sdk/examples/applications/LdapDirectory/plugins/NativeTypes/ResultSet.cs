using System;
using System.Collections;
using System.Xml.Serialization;

using Metreos.LdapDirectory.Common;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework;

namespace Metreos.Types.LdapDirectory
{
	/// <summary> Contains the results of a query to LDAP </summary>
	[Serializable]
	public class ResultSet : IVariable
	{
        public int Count { get { return _value.Count; } }

        public string GetNameAtIndex(int index)
        {
            LdapAttributeCollection ldapAttributeCollection = _value.GetKey(index) as LdapAttributeCollection;

            return _value.GetFriendlyName(ldapAttributeCollection);
        }

        public string GetNumberAtIndex(int index)
        {
            return _value.GetByIndex(index) as string;
        }

        private LdapResultSortedList _value;

        /// <summary> Prepares the inner sorted class for use </summary>
        public ResultSet()
        {
            Reset();
        }

        [TypeInput("LdapResultSortedList", "List of users")]
        public bool Parse(object obj)
        {
            if(obj is LdapResultSortedList)
            {
                _value = obj as LdapResultSortedList;

                return true;
            }

            return false;
        }

        /// <summary> Removes previous information </summary>
        public void Reset()
        {
            if(_value != null)
            {
                _value.Clear();
            }
            else
            {
                _value = new LdapResultSortedList();
            }
        }

        public bool Parse(string incomingValue)
        {
            return true;
        }

	}
}
