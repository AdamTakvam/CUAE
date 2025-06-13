using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.Execute;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone execute request
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public sealed class Execute : IVariable
    {
        public object Value;

        public CiscoIPPhoneExecuteType menu;

        /// <summary>
        /// Intializes the menu information to an empty set
        /// </summary>
        public Execute()
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneExecuteType));
                
            try
            {
                menu = (CiscoIPPhoneExecuteType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method can handle one type of object.
        /// CiscoIPPhoneExecuteType - the static content of the execute statement to send to the Cisco IP Phone
        /// </summary>
        /// <param name="obj">CiscoIPPhoneExecuteType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneExecuteType", "An execute item")]
        public bool Parse(object obj)
        {
            if(obj is CiscoIPPhoneExecuteType)
            {
                CiscoIPPhoneExecuteType newMenu = (CiscoIPPhoneExecuteType) obj;
                menu.ExecuteItem = newMenu.ExecuteItem;
                obj = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Convert this type into a string.
        /// </summary>
        /// <remarks>
        /// This is a bit more complicated for CiscoIpPhoneExecute because we must
        /// fully compress and escape the string for transport as a form item in
        /// an HTTP POST request.
        /// </remarks>
        /// <returns>A string containing the properly HTTP escaped CiscoIpPhoneExecute item.</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneExecuteType));
            serializer.Serialize(writer, menu);
            writer.Close();

            return ICiscoPhone.FormatXML(sb.ToString());       
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            menu = new CiscoIPPhoneExecuteType();
        }
    }
}
