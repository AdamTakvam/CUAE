using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Specialized;

using Metreos.Utilities;

namespace InstallerConverter
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// EXECUTE IN A DIR; will iterate in current and all sub looking for extensions of .installer, updating them.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
		    CommandLineArguments parsed = new CommandLineArguments(args);
            string recurseDirs = parsed.GetSingleParam("d");
	        StringCollection files = parsed.GetStandAloneParameters();

            if(recurseDirs != null && Directory.Exists(recurseDirs))
            {
                RecurseDirectory(Directory.CreateDirectory(recurseDirs));
            }
            else if(recurseDirs == String.Empty)
            {
                RecurseDirectory(new DirectoryInfo(System.Environment.CurrentDirectory));
            }
//
//            if(files != null)
//            {
//                foreach(string filename in files)
//                {
//                    if(File.Exists(filename))
//                    {
//                        ProcessFile(filename);
//                    }
//                }
//            }
   		}


        public static XmlSerializer oldDeserializer = new XmlSerializer(typeof(OldInstaller.installTypeA));
        public static XmlSerializer newSerializer = new XmlSerializer(typeof(NewInstaller.installType));

        public static void RecurseDirectory(DirectoryInfo dir)
        {
           FileInfo[] files = dir.GetFiles();

            if(files != null)
            {
                foreach(FileInfo fileInfo in files)
                {
                    ProcessFile(fileInfo.FullName);
                }
            }

            DirectoryInfo[] directories = dir.GetDirectories();
            
            if(directories != null)
            {
                foreach(DirectoryInfo dirInfo in directories)
                {
                    RecurseDirectory(dirInfo);
                }
            }
        }

        public static void ProcessFile(string filename)
        {
            if(!filename.EndsWith(".installer")) return;

            FileStream stream = null;
            OldInstaller.installTypeA oldVersion = null;
            NewInstaller.installType newVersion = null;
        
            try
            {
                stream = File.Open(filename, FileMode.Open);

                oldVersion = (OldInstaller.installTypeA) oldDeserializer.Deserialize(stream);

                newVersion = new InstallerConverter.NewInstaller.installType();

                if(oldVersion.configuration == null) return;
                
                newVersion.configuration = new InstallerConverter.NewInstaller.configurationType[oldVersion.configuration.Length];

                for(int j = 0; j < oldVersion.configuration.Length; j++)
                {
                    newVersion.configuration[j] = new InstallerConverter.NewInstaller.configurationType();

                    if(oldVersion.configuration[j].configValue == null) continue;

                    newVersion.configuration[j].configValue = new NewInstaller.configValueType[oldVersion.configuration[j].configValue.Length];

                    for(int i = 0; i < oldVersion.configuration[j].configValue.Length; i++)
                    {
                        newVersion.configuration[j].configValue[i] = new NewInstaller.configValueType();

                        OldInstaller.configValueTypeA oldConfig = oldVersion.configuration[j].configValue[i];
                        NewInstaller.configValueType newConfig = newVersion.configuration[j].configValue[i];
                 
                        
                        newConfig.defaultValue = oldConfig.Value;
                        newConfig.description = oldConfig.description;
                        newConfig.displayName = oldConfig.name;
                        newConfig.format = oldConfig.format;
                        newConfig.maxValue = oldConfig.maxValue;
                        newConfig.maxValueSpecified = oldConfig.maxValueSpecified;
                        newConfig.minValue = oldConfig.minValue;
                        newConfig.minValueSpecified = oldConfig.minValueSpecified;
                        newConfig.name = oldConfig.name;
                        newConfig.readOnly = oldConfig.readOnly;
                        newConfig.readOnlySpecified = oldConfig.readOnlySpecified;
                        newConfig.required = true;
                        newConfig.requiredSpecified = true;
                    }
                }

                stream.Close();
                stream = null;
                
                stream = File.Open(filename, FileMode.Create);

                newSerializer.Serialize(stream,  newVersion);
            }
            catch(Exception e)
            {
                Console.WriteLine(Exceptions.FormatException(e));
            }
            finally
            {
                if(stream != null)
                {
                    stream.Close();
                }
            }
        }

        private static XmlAttribute Create(XmlDocument doc, string name, string value)
        {
            XmlAttribute attr = doc.CreateAttribute(String.Empty, name, String.Empty);
            attr.Value = value;
            return attr;
        }
	}

    public class OldInstaller
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
            [System.Xml.Serialization.XmlRootAttribute("install", Namespace="http://metreos.com/AppInstaller.xsd", IsNullable=false)]
            public class installTypeA
        {
        
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("configuration")]
            public configurationTypeA[] configuration;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
            public class configurationTypeA 
        {
        
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("configValue")]
            public configValueTypeA[] configValue;
        
            /// <remarks/>
            public string unused;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
            public class configValueTypeA 
        {
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string format;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string description;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int minValue;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool minValueSpecified;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int maxValue;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool maxValueSpecified;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string @enum;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool readOnly;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool readOnlySpecified;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value;
        }
    }

    /// <summary>
    /// Post 2-25-05
    /// </summary>
    public class NewInstaller
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
            [System.Xml.Serialization.XmlRootAttribute("install", Namespace="http://metreos.com/AppInstaller.xsd", IsNullable=false)]
            public class installType 
        {
        
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("configuration")]
            public configurationType[] configuration;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
            public class configurationType 
        {
        
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("configValue")]
            public configValueType[] configValue;
        
            /// <remarks/>
            public string unused;
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
            public class configValueType 
        {
        
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("EnumItem")]
            public string[] EnumItem;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string displayName;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string format;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string description;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int minValue;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool minValueSpecified;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int maxValue;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool maxValueSpecified;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute("defaultValue")]
            public string defaultValue;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool readOnly;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool readOnlySpecified;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool required;
        
            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool requiredSpecified;
        }
    }
}
