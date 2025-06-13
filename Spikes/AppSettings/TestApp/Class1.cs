using System;
using System.Configuration;
using System.Xml;

namespace TestApp
{
    class Class1
    {
        public const string METREOS_FW_LOCATION_KEY = "MetreosFrameworkLocation";

        [STAThread]
        static void Main(string[] args)
        {
            string newValue = args.Length >= 1 ? args[0] : "DefaultNewValue";

            string fwKey = ConfigurationSettings.AppSettings[METREOS_FW_LOCATION_KEY];
            string commandLine = System.Environment.CommandLine.Split(' ')[0];
            string configFile = commandLine + ".config";

            Console.WriteLine("Editing File  : {0}", configFile);

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(configFile);

            System.Xml.XmlNode appSettingsNode = xmlDoc.SelectSingleNode("//configuration/appSettings");

            System.Xml.XmlNodeList list = appSettingsNode.ChildNodes;
            
            System.Xml.XmlNode parentNode = null;
            System.Xml.XmlNode keyNode = null;
            System.Xml.XmlNode valueNode = null;
            for(int i = 0; i < list.Count; i++)
            {
                parentNode = list[i];
                keyNode = parentNode.Attributes.GetNamedItem("key");
                if(keyNode.Value == METREOS_FW_LOCATION_KEY)
                {
                    valueNode = parentNode.Attributes.GetNamedItem("value");
                    break;
                }

                parentNode = null;
                keyNode = null;
                valueNode = null;
            }

            if( (parentNode == null) ||
                (keyNode == null) ||
                (valueNode == null))
            {
                Console.WriteLine("{0} key not found in application settings", METREOS_FW_LOCATION_KEY);
            }
            else
            {
                Console.WriteLine("Found Key     : '{0}'", keyNode.Value);
                Console.WriteLine("Current Value : '{0}'", valueNode.Value);
                Console.WriteLine("New Value     : '{0}'", newValue);

                valueNode.Value = newValue;
            }
            
            XmlTextWriter writer = new XmlTextWriter(configFile, System.Text.Encoding.ASCII);
            writer.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(writer);
            writer.Flush();
            writer.Close();
        }
    }
}
