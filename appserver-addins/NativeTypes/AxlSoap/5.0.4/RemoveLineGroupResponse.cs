using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap504;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504Types.Types.RemoveLineGroupResponse;

namespace Metreos.Types.AxlSoap504
{
    /// <summary>
    ///     Provides an interface to the information contained within the removeLineGroupResponse from AXL SOAP 5.0.4
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class RemoveLineGroupResponse : IVariable
	{
        public removeLineGroupResponse Response { get { return response; } set { response = value; } }
        private removeLineGroupResponse response;

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
            if(obj is removeLineGroupResponse)
            {
                response = obj as removeLineGroupResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new removeLineGroupResponse();
        }
	}
}
