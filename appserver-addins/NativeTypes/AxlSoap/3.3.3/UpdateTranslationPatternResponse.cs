using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap333;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333Types.Types.UpdateTranslationPatternResponse;

namespace Metreos.Types.AxlSoap333
{
    /// <summary>
    ///     Provides an interface to the information contained within the updateTransPatternResponse from AXL SOAP 3.3.3
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class UpdateTranslationPatternResponse : IVariable
	{
        public updateTransPatternResponse Response { get { return response; } set { response = value; } }
        private updateTransPatternResponse response;

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
            if(obj is updateTransPatternResponse)
            {
                response = obj as updateTransPatternResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new updateTransPatternResponse();
        }
	}
}
