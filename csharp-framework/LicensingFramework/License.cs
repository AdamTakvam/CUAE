using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.LicensingFramework
{
    /// <summary>
    /// A data structure that corresponds to the structure of the FLEXlm FEATURE/INCREMENT format
    /// </summary>
    public class LicenseFeature
    {
        #region Required fields
        public string Name
        {
            get { return Name; }
        }
        private string name;

        public string VendorDaemonName
        {
            get { return vendorDaemonName; }
        }
        private string vendorDaemonName;

        public string FeatureVersion
        {
            get { return featureVersion; }
        }
        private string featureVersion;

        public string ExpirationDate
        {
            get { return expirationDate; }
        }
        private string expirationDate; 

        public uint LicensedInstances
        {
            get { return instances; }
        }
        private uint instances;
        #endregion

        #region Optional fields
        public string VendorString
        {
            get { return vendorString; }
        }
        private string vendorString; 
        #endregion
    }
}
