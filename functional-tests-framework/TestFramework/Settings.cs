namespace Metreos.Samoa.FunctionalTestFramework {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd", IsNullable=false)]
    public class Settings 
    {
        public bool NeverUninstallExistingApps
        {
            get
            {
                return useFile ? neverUninstallExistingBox : neverUninstallExistingBoxTemp;
            }
            set
            {
                if(useFile)
                {
                    neverUninstallExistingBox = value;
                }
                else
                {
                    neverUninstallExistingBoxTemp = value;
                }
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string SamoaPort
        {
            get
            {
                return useFile ? samoaPort : samoaPortTemp;
            }
            set
            {
                if(useFile)
                {
                    samoaPort = value;
                }
                else
                {
                    samoaPortTemp = value;
                }
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string TestPort
        {
            get
            {
                return useFile ? testPort : testPortTemp;
            }
            set
            {
                if(useFile)
                {
                    testPort = value;
                }
                else
                {
                    testPortTemp = value;
                }
            }
        }

        public string TestSettings
        {
            get
            {
                return useFile ? testSettings : testSettingsTemp;
            }
            set
            {
                if(useFile)
                {
                    testSettings = value;
                }
                else
                {
                    testSettingsTemp = value; 
                }
            }
        }

		[XmlElementAttribute("FrameworkDir")]
		public string FrameworkDir
		{
			get
			{
				return useFile ? frameworkDir:frameworkDirTemp;
			}
			set
			{
				if(useFile)
				{
					frameworkDir=value;
				}
				else
				{
					frameworkDirTemp=value;
				}
			}
		}



		[XmlElementAttribute("DllDir")]
        public string DllFolder
        {
            get
            {
                return useFile ? dllDir : dllDirTemp;
            }
            set
            {
                if(useFile)
                {
                    dllDir = value;
                }
                else
                {
                    dllDirTemp = value;
                }
            }
        }

        [XmlElement("CompiledTestsDir")]
        public string CompiledMaxTestsDir
        {
            get
            {
                return useFile ? compiledMaxTestsDir : compiledMaxTestsDirTemp;
            }
            set
            {
                if(useFile)
                {
                    compiledMaxTestsDir = value;
                }
                else
                {
                    compiledMaxTestsDirTemp = value;
                }
            }
        }

        [System.NonSerialized()]
        public  bool useFile = true;

        public  int    PhoneStartRange;        
        public  int    PhoneEndRange;
        public string[] AppServerIps;
        public  string CallManagerIp;
        public  string Username;
        public  string Password;
        public  string PollTimes;
        private string samoaPort;
        private string samoaPortTemp;
        private string testPort;
        private string testPortTemp;
        private string testSettings;
        private string testSettingsTemp; 
		private string frameworkDir;
		private string frameworkDirTemp;
        private string dllDir;
        private string dllDirTemp;    
        private string compiledMaxTestsDir;
        private string compiledMaxTestsDirTemp;
        private bool neverUninstallExistingBox;
        private bool neverUninstallExistingBoxTemp;
    }
}
