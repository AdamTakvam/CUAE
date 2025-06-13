using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.Input;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone Input listing
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Input : IVariable
    {
        public object Value;

        // Value is a Hashtable of ArrayLists
        // Element name (string) -> Element XML (array of XML serializeable objects)
        public CiscoIPPhoneInputType menu;

        /// <summary>
        /// Initializes the Input information, softkeys, and input items to an empty set
        /// </summary>
        public Input()
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneInputType));
                
            try
            {
                menu = (CiscoIPPhoneInputType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            Hashtable hash = new Hashtable();
            ArrayList array = null;
            
            if(menu.InputItem != null)
            {
                array = new ArrayList();
                for(int i=0; i<menu.InputItem.Length; i++)
                {
                    array.Add(menu.InputItem[i]);
                }
                hash.Add(ICiscoPhone.SUBELEMENT_INPUT, array);
            }

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
        /// This method can handle three types of objects.
        /// Type 1:  CiscoIPPhoneInputType - the static content of a Input object
        /// Type 2:  CiscoIPPhoneInputItemType - passing this adds to the list of input items
        /// Type 3:  CiscoIPPhoneSoftKeyType - passing this adds to the list of softkeys
        /// </summary>
        /// <param name="obj">CiscoIPPhoneInputType |
        ///  CiscoIPPhoneInputItemType |
        ///   CiscoIPPhoneSoftKeyType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneInputType", "Initial input field data")]
        [TypeInput("CiscoIPPhoneInputItemType", "An item to add to this input field")]
        [TypeInput("CiscoIPPhoneSoftKeyType", "A soft key to add to this input field")]
        public bool Parse(object obj)
        {
            string elementName = null;
            if(obj is CiscoIPPhoneInputType)
            {
                CiscoIPPhoneInputType newMenu = (CiscoIPPhoneInputType) obj;
                menu.Prompt = newMenu.Prompt;
                menu.Title = newMenu.Title;
                menu.URL = newMenu.URL;

                obj = null;
                return true;
            }
            else if(obj is CiscoIPPhoneInputItemType)
            {
                elementName = ICiscoPhone.SUBELEMENT_INPUT;
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
        /// Removes all input item and softkey information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            menu = new CiscoIPPhoneInputType();

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
        /// Converts the native Input object to Cisco IP Phone compatible XML
        /// </summary>
        /// <returns>XML representation of the Input object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            Hashtable hash = (Hashtable) Value;
            
            ArrayList array = (ArrayList) hash[ICiscoPhone.SUBELEMENT_INPUT];
            if(array != null)
            {
                menu.InputItem = new CiscoIPPhoneInputItemType[array.Count];
                for(int i=0; i<array.Count; i++)
                {
                    menu.InputItem[i] = (CiscoIPPhoneInputItemType) array[i];
                }
            }

            array = (ArrayList) hash[ICiscoPhone.SUBELEMENT_SOFTKEY];
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneInputType));
            serializer.Serialize(writer, menu);
            writer.Close();

            return ICiscoPhone.FormatXML(sb.ToString());
        }
    }
}
