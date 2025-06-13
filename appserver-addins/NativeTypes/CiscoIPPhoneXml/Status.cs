using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.Status;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone Status Bar    
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Status : IVariable
    {
        public CiscoIPPhoneStatusType status;

        /// <summary>
        /// Initializes the Status information
        /// </summary>
        public Status()
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneStatusType));
                
            try
            {
                status = (CiscoIPPhoneStatusType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method can handle two types of objects.
        /// Type 1:  CiscoIPPhoneStatusType - the static content of a Status bar
        /// </summary>
        /// <param name="obj">CiscoIPPhoneStatusType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneStatusType", "Initial status data")]
        public bool Parse(object obj)
        {
            if(obj is CiscoIPPhoneStatusType)
            {
               status = obj as CiscoIPPhoneStatusType;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes all Status bar information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            status = new CiscoIPPhoneStatusType();
        }

        /// <summary>
        /// Converts the native Status Bar object to Cisco IP Phone compatible XML
        /// </summary>
        /// <returns>XML representation of the Status object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneStatusType));
            serializer.Serialize(writer, status);
            writer.Close();

            return ICiscoPhone.FormatXML(sb.ToString());
        }
    }
}
