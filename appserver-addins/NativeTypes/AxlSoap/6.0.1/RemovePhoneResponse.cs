using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601Types.Types.RemovePhoneResponse;

namespace Metreos.Types.AxlSoap601
{
    /// <summary>
    ///     Provides an interface to the information contained within the removePhoneResponse from AXL SOAP 6.0.1
    /// </summary>
    [Metreos.PackageGeneratorCore.Attributes.TypeDecl("AxlSoap.RemovePhoneResponse (6.0.1)", 
         "Encapsulates the data returned by the RemovePhone action", 
         false)]
    public sealed class RemovePhoneResponse : IVariable
	{
        public removePhoneResponse Response { get { return response; } set { response = value; } }
        private removePhoneResponse response;

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
            if(obj is removePhoneResponse)
            {
                response = obj as removePhoneResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new removePhoneResponse();
        }
	}
}
