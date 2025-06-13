using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601Types.Types.Services;

namespace Metreos.Types.AxlSoap601
{
    /// <summary>
    ///     Stores information about multiple sevices, for use with 'updatePhone' from the AXL SOAP 6.0.1
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Services : IVariable
	{
        public XSubscribedService[] Data { get { return data; } set { data = value; } }
        private XSubscribedService[] data;

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
            if(obj is XSubscribedService)
            {
                if(data == null || data.Length == 0)
                {
                    data = new XSubscribedService[1];
                    data[0] = obj as XSubscribedService;
                }
                else
                {
                    XSubscribedService[] expanded = new XSubscribedService[data.Length + 1];
                    data.CopyTo(expanded, 0);
                    expanded[expanded.Length -1] = obj as XSubscribedService;
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
