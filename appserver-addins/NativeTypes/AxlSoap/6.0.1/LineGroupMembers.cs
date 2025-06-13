using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601Types.Types.LineGroupMembers;

namespace Metreos.Types.AxlSoap601
{
    /// <summary>
    ///     Stores information about multiple lines, for use with 'updateLineGroup' or 'addLineGroup' from the AXL SOAP 6.0.1
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class LineGroupMembers : IVariable
	{
        public XLineGroupMember[] Data { get { return data; } set { data = value; } }
        private XLineGroupMember[] data;

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
            if(obj is XLineGroupMember)
            {
                if(data == null || data.Length == 0)
                {
                    data = new XLineGroupMember[1];
                    data[0] = obj as XLineGroupMember;
                }
                else
                {
                    XLineGroupMember[] expanded = new XLineGroupMember[data.Length + 1];
                    data.CopyTo(expanded, 0);
                    expanded[expanded.Length -1] = obj as XLineGroupMember;
                    data = expanded;
                }
            }
            else if(obj is XLineGroupMember[])
            {
                XLineGroupMember[] newLines = obj as XLineGroupMember[];
                if(data == null || data.Length == 0)
                {
                    data = new XLineGroupMember[newLines.Length];
                    Array.Copy(newLines,
                        data, data.Length);
                }
                else
                {
                    XLineGroupMember[] expanded = new XLineGroupMember[data.Length + newLines.Length];
                    data.CopyTo(expanded, 0);
                    newLines.CopyTo(expanded, expanded.Length - newLines.Length);
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
