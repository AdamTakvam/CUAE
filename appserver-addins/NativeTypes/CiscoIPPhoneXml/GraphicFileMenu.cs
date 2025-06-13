using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.GraphicFileMenu;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone Graphic File Menu
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class GraphicFileMenu : IVariable
    {
        public object Value;

        // Value is a Hashtable of ArrayLists
        // Element name (string) -> Element XML (array of XML serializeable objects)
        public CiscoIPPhoneGraphicFileMenuType menu;

        /// <summary>
        /// Initializes the Graphical Menu information, softkeys, and menu list to an empty set
        /// </summary>
        public GraphicFileMenu()
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneGraphicFileMenuType));
                
            try
            {
                menu = (CiscoIPPhoneGraphicFileMenuType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            Hashtable hash = new Hashtable();
            ArrayList array = null;
            
            if(menu.MenuItem != null)
            {
                array = new ArrayList();
                for(int i=0; i<menu.MenuItem.Length; i++)
                {
                    array.Add(menu.MenuItem[i]);
                }
                hash.Add(ICiscoPhone.SUBELEMENT_MENU, array);
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
        /// Type 1:  CiscoIPPhoneGraphicFileMenuType - the static content of a Graphic Image Map
        /// Type 2:  CiscoIPPhoneMenuItemType - passing this adds to the list of menu items
        /// Type 3:  CiscoIPPhoneSoftKeyType - passing this adds to the list of softkeys
        /// </summary>
        /// <param name="obj">CiscoIPPhoneGraphicFileMenuType |
        ///  CiscoIPPhoneMenuItemType |
        ///   CiscoIPPhoneSoftKeyType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneGraphicFileMenuType", "Initial graphic file menu data")]
        [TypeInput("CiscoIPPhoneMenuItemType", "An item to add to this menu")]
        [TypeInput("CiscoIPPhoneSoftKeyType", "A soft key to add to this menu")]
        public bool Parse(object obj)
        {
            string elementName = null;
            if(obj is CiscoIPPhoneGraphicFileMenuType)
            {
                CiscoIPPhoneGraphicFileMenuType newMenu = (CiscoIPPhoneGraphicFileMenuType) obj;
                menu.Prompt = newMenu.Prompt;
                menu.Title = newMenu.Title;
                menu.LocationX = newMenu.LocationX;
                menu.LocationY = newMenu.LocationY;
                menu.URL = newMenu.URL;

                obj = null;
                return true;
            }
            else if(obj is CiscoIPPhoneMenuItemType)
            {
                elementName = ICiscoPhone.SUBELEMENT_MENU;
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
        /// Removes all softkey and menu information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            menu = new CiscoIPPhoneGraphicFileMenuType();

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
        /// Converts the native GraphicMenu object to Cisco IP Phone compatible XML
        /// </summary>
        /// <returns>XML representation of the GraphicMenu object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            Hashtable hash = (Hashtable) Value;
            
            ArrayList array = (ArrayList) hash[ICiscoPhone.SUBELEMENT_MENU];
            if(array != null)
            {
                menu.MenuItem = new CiscoIPPhoneMenuItemType[array.Count];
                for(int i=0; i<array.Count; i++)
                {
                    menu.MenuItem[i] = (CiscoIPPhoneMenuItemType) array[i];
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneGraphicFileMenuType));
            serializer.Serialize(writer, menu);
            writer.Close();

            return ICiscoPhone.FormatXML(sb.ToString());
        }
    }
}
