using System;

namespace Metreos.LdapDirectory.Common
{
	/// <summary> Null Ldap Attribute</summary>
    public class NullLdapAttribute : LdapAttribute
    {
        #region Singleton
    
        public override string Value
        {
            get
            {
                return UndefinedValue;
            }
        }

        public static NullLdapAttribute Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NullLdapAttribute();
                }
                    
                return instance; 
            }
        }
        private static NullLdapAttribute instance;

        #endregion

        private const string ldapDoesntHaveThisAttrName = "oLsPeEdEqualsWhaT";
		
        private NullLdapAttribute() : base(ldapDoesntHaveThisAttrName)
		{
		}

        public override void AddValue(string attributeValue)
        {
            // Do nothin.  
        }

	}
}
