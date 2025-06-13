using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.StatusFile;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone Status Bar for the color phones
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class StatusFile : IVariable
    {
        public CiscoIPPhoneStatusFileType status;

        /// <summary>
        /// Initializes the StatusFile bar information
        /// </summary>
        public StatusFile()
        {
            Reset();
        }

        /// <summary>
        /// Not implemented.  A simple string wouldn't convert to Cisco IP Phone XML well
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string newValue)
        {
            if(newValue == null) { return false; }

            // Deserialize it
            System.IO.TextReader reader = new System.IO.StringReader(newValue);
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneStatusFileType));
                
            try
            {
                status = (CiscoIPPhoneStatusFileType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method can handle two types of objects.
        /// Type 1:  CiscoIPPhoneStatusFileType - the static content of a StatusFile bar
        /// </summary>
        /// <param name="obj">CiscoIPPhoneStatusFileType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneStatusFileType", "Initial status file data")]
        public bool Parse(object obj)
        {
            if(obj is CiscoIPPhoneStatusFileType)
            {
               status = obj as CiscoIPPhoneStatusFileType;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes all StatusFile bar information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            status = new CiscoIPPhoneStatusFileType();
        }

        /// <summary>
        /// Converts the native Image object to Cisco IP Phone compatible XML
        /// </summary>
        /// <returns>XML representation of the Image object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneStatusFileType));
            serializer.Serialize(writer, status);
            writer.Close();

            return ICiscoPhone.FormatXML(sb.ToString());
        }
    }
}
