using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601Types.Types.CallForward;

namespace Metreos.Types.AxlSoap601
{
    /// <summary>
    ///     Stores information about a forward object, for use with 'updatePhone' from the AXL SOAP 6.0.1
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class CallForward : IVariable
	{
        public XCallForwardInfo Data { get { return data; } set { data = value; } }
        private XCallForwardInfo data;

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
            if(obj is XCallForwardInfo)
            {
                data = obj as XCallForwardInfo;
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
