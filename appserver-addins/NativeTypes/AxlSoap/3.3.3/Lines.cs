using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap333;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333Types.Types.Lines;

namespace Metreos.Types.AxlSoap333
{
    /// <summary>
    ///     Stores information about multiple lines, for use with 'updatePhone' from the AXL SOAP 3.3.3
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Lines : IVariable
	{
        public XLine[] Data { get { return data; } set { data = value; } }
        private XLine[] data;

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
            if(obj is XLine)
            {
                if(data == null || data.Length == 0)
                {
                    data = new XLine[1];
                    data[0] = obj as XLine;
                }
                else
                {
                    XLine[] expanded = new XLine[data.Length + 1];
                    data.CopyTo(expanded, 0);
                    expanded[expanded.Length -1] = obj as XLine;
                    data = expanded;
                }
            }
            else if(obj is XLine[])
            {
                XLine[] newLines = obj as XLine[];
                if(data == null || data.Length == 0)
                {
                    data = new XLine[newLines.Length];
                    Array.Copy(newLines,
                        data, data.Length);
                }
                else
                {
                    XLine[] expanded = new XLine[data.Length + newLines.Length];
                    data.CopyTo(expanded, 0);
                    newLines.CopyTo(expanded, expanded.Length - newLines.Length);
                    data = expanded;
                }
            }
            else if (obj is UpdatePhoneReqLines) // Presence of this object is a flag indicating a ClearLines action
            {
                data = new XLine[0];
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
