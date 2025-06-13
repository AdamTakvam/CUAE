using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap504;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504Types.Types.Speeddials;

namespace Metreos.Types.AxlSoap504
{
    /// <summary>
    ///     Stores information about multiple speeddials, for use with 'updatePhone' from the AXL SOAP 5.0.4
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Speeddials : IVariable
	{
        public XSpeeddial[] Data { get { return data; } set { data = value; } }
        private XSpeeddial[] data;

        /// <summary>
        /// Not implemented for this complex Axl Soap type
        /// </summary>
        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string newValue)
        { 
            return false;
        }

        [TypeInput("Object", Package.CustomMethods.Parse_Object.DESCRIPTION)]        
        public bool Parse(object obj)
        {
            if(obj is XSpeeddial)
            {
                if(data == null || data.Length == 0)
                {
                    data = new XSpeeddial[1];
                    data[0] = obj as XSpeeddial;
                }
                else
                {
                    XSpeeddial[] expanded = new XSpeeddial[data.Length + 1];
                    data.CopyTo(expanded, 0);
                    expanded[expanded.Length -1] = obj as XSpeeddial;
                    data = expanded;
                }
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            data = null;
        }
	}
}
