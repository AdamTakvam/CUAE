using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601Types.Types.GetPartitionResponse;

namespace Metreos.Types.AxlSoap601
{
    /// <summary>
    ///     Provides an interface to the information contained within the getPartitionResponse from AXL SOAP 6.0.1
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class GetPartitionResponse : IVariable
	{
        public getRoutePartitionResponse Response { get { return response; } set { response = value; } }
        private getRoutePartitionResponse response;

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
            if(obj is getRoutePartitionResponse)
            {
                response = obj as getRoutePartitionResponse;
            }
        
            return true;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            response = new getRoutePartitionResponse();
        }
	}
}
