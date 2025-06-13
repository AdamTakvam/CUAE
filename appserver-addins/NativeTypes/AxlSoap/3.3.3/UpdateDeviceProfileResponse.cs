using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap333;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333Types.Types.UpdateDeviceProfileResponse;

namespace Metreos.Types.AxlSoap333
{
    /// <summary>
    ///     Provides an interface to the information contained within the updatePhoneResponse from AXL SOAP 3.3.3
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class UpdateDeviceProfileResponse : IVariable
	{
        public updateDeviceProfileResponse Response { get { return response; } set { response = value; } }
        private updateDeviceProfileResponse response;

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
            if(obj is updateDeviceProfileResponse)
            {
                response = obj as updateDeviceProfileResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new updateDeviceProfileResponse();
        }
	}
}
