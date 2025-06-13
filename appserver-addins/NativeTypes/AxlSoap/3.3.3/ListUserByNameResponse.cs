using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap333;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333Types.Types.ListUserByNameResponse;

namespace Metreos.Types.AxlSoap333
{
    /// <summary>
    ///     Provides an interface to the information contained within the listUserByNameResponse from AXL SOAP 3.3.3
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class ListUserByNameResponse : IVariable
	{
        public listUserByNameResponse Response { get { return response; } set { response = value; } }
        private listUserByNameResponse response;

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
            if(obj is listUserByNameResponse)
            {
                response = obj as listUserByNameResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new listUserByNameResponse();
        }
	}
}
