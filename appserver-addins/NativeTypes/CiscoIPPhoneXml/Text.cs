using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.Text;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone Text Object
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Text : IVariable
    {
        public object Value;

        // Value is a Hashtable of ArrayLists
        // Element name (string) -> Element XML (array of XML serializeable objects)
        public CiscoIPPhoneTextType menu;

        /// <summary>
        /// Initializes the Text information and softkeys to an empty set
        /// </summary>
        public Text()
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneTextType));
                
            try
            {
                menu = (CiscoIPPhoneTextType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            Hashtable hash = new Hashtable();
            ArrayList array = null;

            if(menu.SoftKeyItem != null)
            {
                array = new ArrayList();
                for(int i=0; i<menu.SoftKeyItem.Length; i++)
                {
                    array.Add(menu.SoftKeyItem[i]);
                }
                hash.Add(ICiscoPhone.SUBELEMENT_SOFTKEY, array);
            }

            Value = hash;

            return true;
        }

        /// <summary>
        /// This method can handle two types of objects.
        /// Type 1:  CiscoIPPhoneTextType - the static content of a Text object
        /// Type 2:  CiscoIPPhoneSoftKeyType - passing this adds to the list of softkeys
        /// </summary>
        /// <param name="obj">CiscoIPPhoneTextType |
        ///   CiscoIPPhoneSoftKeyType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneTextType", "Initial text data")]
        [TypeInput("CiscoIPPhoneSoftKeyType", "A soft key to add to this text")]
        public bool Parse(object obj)
        {
            string elementName = null;
            if(obj is CiscoIPPhoneTextType)
            {
                CiscoIPPhoneTextType newMenu = (CiscoIPPhoneTextType) obj;
                menu.Prompt = newMenu.Prompt;
                menu.Title = newMenu.Title;
                menu.Text = newMenu.Text;

                obj = null;
                return true;
            }
            else if(obj is CiscoIPPhoneSoftKeyType)
            {
                elementName = ICiscoPhone.SUBELEMENT_SOFTKEY;
            }
            else
            {
                return false;
            }
 
            Hashtable hash = (Hashtable) Value;
            ArrayList list = (ArrayList) hash[elementName];

            if(list != null)
            {
                list.Add(obj);
            }
            else
            {
                list = new ArrayList();
                list.Add(obj);
                hash.Add(elementName, list);
            }

            return true;
        }

        /// <summary>
        /// Removes all Text and softkey information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            menu = new CiscoIPPhoneTextType();

			if(Value == null)
			{
				Value = new Hashtable();
			}
			else
			{
				Hashtable hash = (Hashtable) Value;
				hash.Clear();
			}
        }

        /// <summary>
        /// Converts the native Text object to Cisco IP Phone compatible XML
        /// </summary>
        /// <returns>XML representation of the Text object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            Hashtable hash = (Hashtable) Value;

            ArrayList array = (ArrayList) hash[ICiscoPhone.SUBELEMENT_SOFTKEY];
            if(array != null)
            {
                menu.SoftKeyItem = new CiscoIPPhoneSoftKeyType[array.Count];
                for(int i=0; i<array.Count; i++)
                {
                    menu.SoftKeyItem[i] = (CiscoIPPhoneSoftKeyType) array[i];
                }
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneTextType));
            serializer.Serialize(writer, menu);
            writer.Close();

            return ICiscoPhone.FormatXML(sb.ToString());
        }
    }
}
