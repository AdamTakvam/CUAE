// 
/*  In testing GetUser with 3.3.3, found this error and gave up.
 * 
 * 20:49:11.997 W script1-1 Soap Fault.

<axl:error sequence="0" xmlns:axl="http://www.cisco.com/AXL/1.0">
<code>5005</code>
<message>
<![CDATA[Unexpected element. Found <userid>, expecting <ldapRN>..]]></message>
<request>getUser</request>
</axl:error>

*
*/
//using System;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Collections;
//
//using Metreos.ApplicationFramework;
//using Metreos.PackageGeneratorCore.Attributes;
//
//using Metreos.AxlSoap333;
//
//using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333Types.Types.GetUserResponse;

//namespace Metreos.Types.AxlSoap333
//{
//    /// <summary>
//    ///     Provides an interface to the information contained within the getUserResponse from AXL SOAP 3.3.3
//    /// </summary>
//    [Metreos.PackageGeneratorCore.Attributes.TypeDecl("AxlSoap.GetUserResponse (3.3.3)", 
//         "Encapsulates the data returned by the GetUser action", 
//         false)]
//    public sealed class GetUserResponse : IVariable
//	{
//        public getUserResponse Response { get { return response; } set { response = value; } }
//        private getUserResponse response;
//
//        /// <summary>
//        /// Not implemented for this complex Axl Soap type
//        /// </summary>
//        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]//        
//        public bool Parse(string newValue)
//        { 
//            return false;
//        }
//
//        [TypeInput("Object", Package.CustomMethods.Parse_Object.DESCRIPTION)]//        
//        public bool Parse(object obj)
//        {
//            if(obj is getUserResponse)
//            {
//                response = obj as getUserResponse;
//            }
//        
//            return true;
//        }
//
//        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]//        
//        public void Reset()
//        {
//            response = new getUserResponse();
//        }
//	}
//}
