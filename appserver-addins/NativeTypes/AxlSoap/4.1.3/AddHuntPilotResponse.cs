using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap413;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap413Types.Types.AddHuntPilotResponse;

namespace Metreos.Types.AxlSoap413
{
    /// <summary>
    ///     Provides an interface to the information contained within the addHuntPilotResponse from AXL SOAP 4.1.3
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class AddHuntPilotResponse : IVariable
	{
        public addHuntPilotResponse Response { get { return response; } set { response = value; } }
        private addHuntPilotResponse response;

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
            if(obj is addHuntPilotResponse)
            {
                response = obj as addHuntPilotResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new addHuntPilotResponse();
        }
	}
}
