using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap413;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap413Types.Types.AddPhoneResponse;

namespace Metreos.Types.AxlSoap413
{
    /// <summary>
    ///     Provides an interface to the information contained within the addPhoneResponse from AXL SOAP 4.1.3
    /// </summary>
    [Metreos.PackageGeneratorCore.Attributes.TypeDecl("AxlSoap.AddPhoneResponse (4.1.3)", 
         "Encapsulates the data returned by the UpdatePhone action", 
         false)]
    public sealed class AddPhoneResponse : IVariable
	{
        public AddPhoneResponse Response { get { return response; } set { response = value; } }
        private AddPhoneResponse response;

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
            if(obj is AddPhoneResponse)
            {
                response = obj as AddPhoneResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new AddPhoneResponse();
        }
	}
}
