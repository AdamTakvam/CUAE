using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.Directory;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone Directory listing
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Directory : IVariable
    {
        public object Value;

        // Value is a Hashtable of ArrayLists
        // Element name (string) -> Element XML (array of XML serializeable objects)
        public CiscoIPPhoneDirectoryType menu;

        /// <summary>
        /// Initializes the Directory information and menu items to an empty set
        /// </summary>
        public Directory()
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneDirectoryType));
                
            try
            {
                menu = (CiscoIPPhoneDirectoryType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            Hashtable hash = new Hashtable();
            ArrayList array = null;
            
            if(menu.DirectoryEntry != null)
            {
                array = new ArrayList();
                for(int i=0; i<menu.DirectoryEntry.Length; i++)
                {
                    array.Add(menu.DirectoryEntry[i]);
                }
                hash.Add(ICiscoPhone.SUBELEMENT_DIRECTORY, array);
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
        /// Type 1:  CiscoIPPhoneDirectoryType - the static content of a Directory
        /// Type 2:  CiscoIPPhoneDirectoryEntryType - passing this adds to the list of directory items
        /// Type 3:  CiscoIPPhoneSoftKeyType - passing this adds to the list of softkeys
        /// </summary>
        /// <param name="obj">CiscoIPPhoneDirectoryType |
        ///  CiscoIPPhoneDirectoryEntryType |
        ///   CiscoIPPhoneSoftKeyType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneDirectoryType", "Initial directory data")]
        [TypeInput("CiscoIPPhoneDirectoryEntryType", "An entry to add to this directory")]
        [TypeInput("CiscoIPPhoneSoftKeyType", "A softkey to add to this directory")]
        public bool Parse(object obj)
        {
            string elementName = null;
            if(obj is CiscoIPPhoneDirectoryType)
            {
                menu = (CiscoIPPhoneDirectoryType) obj;
                return true;
            }
            else if(obj is CiscoIPPhoneDirectoryEntryType)
            {
                elementName = ICiscoPhone.SUBELEMENT_DIRECTORY;
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
        /// Removes all directory item and menu item information
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            menu = new CiscoIPPhoneDirectoryType();

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
        /// Converts the native Directory object to Cisco IP Phone compatible XML
        /// </summary>
        /// <returns>XML representation of the Directory object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            Hashtable hash = (Hashtable) Value;
            
            ArrayList array = (ArrayList) hash[ICiscoPhone.SUBELEMENT_DIRECTORY];
            if(array != null)
            {
                menu.DirectoryEntry = new CiscoIPPhoneDirectoryEntryType[array.Count];
                for(int i=0; i<array.Count; i++)
                {
                    menu.DirectoryEntry[i] = (CiscoIPPhoneDirectoryEntryType) array[i];
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
            XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneDirectoryType));
            serializer.Serialize(writer, menu);
            writer.Close();

            return ICiscoPhone.FormatXML(sb.ToString());
        }
    }
}
