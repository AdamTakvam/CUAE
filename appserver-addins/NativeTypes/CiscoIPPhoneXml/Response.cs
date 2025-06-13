using System;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhoneTypes.Types.Response;

namespace Metreos.Types.CiscoIpPhone
{
    /// <summary>
    /// The native implementation of a Cisco IP Phone response message.
    /// </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public sealed class Response : IVariable
    {
        /// <summary> If an HTTP Error, the reason code </summary>
        public HttpStatusCode HttpCode { get { return httpCode; } }
        private HttpStatusCode httpCode;

        /// <summary> Basic type of error </summary>
        public ErrorTypeCode ErrorType { get { return errorType; } }
        private ErrorTypeCode errorType;

        /// <summary>  If an IPPhone-specific error...</summary>
        public int IPPhoneError { get { return ipPhoneError; } }
        private int ipPhoneError;

        /// <summary> In case of generic error, the response from the client </summary>
        public string ErrorResponse { get { return errorResponse; } }
        private string errorResponse;

        public object Value;

        public Response() 
        {
            Reset();
        }

        /// <summary>
        /// Not implemented.  
        /// A simple string cannot be converted to a Cisco IP phone response.
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string newValue)
        {
            // Assigning an unqualified string to this type doesn't make sense.
            // Assign this type the return data from a Cisco "Send..." action.
            return true;
        }

        /// <summary>
        /// This method can handle two types of objects.
        /// Type 1:  CiscoIPPhoneResponseType - Normal response
        /// Type 2:  CiscoIPPhoneErrorType - Error response
        /// Use the IsError property to determine the type of response
        /// </summary>
        /// <param name="obj">CiscoIPPhoneResponseType |
        ///  CiscoIPPhoneErrorType</param>
        /// <returns>False if assignment fails, true if the assignment was successful</returns>
        [TypeInput("CiscoIPPhoneResponseType", "Normal response data")]
        [TypeInput("CiscoIPPhoneErrorType", "Error response data")]
        public bool Parse(object obj)
        {
            if(obj is CiscoIPPhoneResponseType)
            {
                Value = obj;
            }
            else if(obj is CiscoIPPhoneErrorType)
            {
                CiscoIPPhoneErrorType error = obj as CiscoIPPhoneErrorType;
                ipPhoneError = (int) error.Number;
                errorType = ErrorTypeCode.IPPhone;
                Value = obj;
            }
            else if(obj is SendExecuteGenericError)
            {
                Value = obj;
                SendExecuteGenericError error = obj as SendExecuteGenericError;
                errorType = error.ErrorType;
                httpCode = error.HttpCode;
                errorResponse = error.ErrorMessage;
            }
            else
            {
                Value = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clears data
        /// </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            errorType = ErrorTypeCode.None;
            httpCode = HttpStatusCode.OK;
            ipPhoneError = 0;
            Value = null;
            errorResponse = null;
        }

        /// <summary>
        /// Converts the response object to a displayable XML string
        /// </summary>
        /// <returns>XML representation of the response object</returns>
        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            XmlSerializer serializer = null;

            if(Value is CiscoIPPhoneResponseType)
            {
                serializer = new XmlSerializer(typeof(CiscoIPPhoneResponseType));
            }
            else if(Value is CiscoIPPhoneErrorType)
            {
                serializer = new XmlSerializer(typeof(CiscoIPPhoneErrorType));
            }
            else if(Value is SendExecuteGenericError)
            {
                SendExecuteGenericError error = Value as SendExecuteGenericError;
                return error.ErrorMessage == null ? String.Empty : error.ErrorMessage;
            }
            else
            {
                return "Unknown error";
            }

            serializer.Serialize(writer, Value);
            writer.Close();

            return sb.ToString();
        }

        public class SendExecuteGenericError
        {
            public HttpStatusCode HttpCode { get { return httpCode; } }
            private HttpStatusCode httpCode;

            public ErrorTypeCode ErrorType { get { return errorType; } }
            private ErrorTypeCode errorType;

            public string ErrorMessage { get { return errorMessage; } }
            private string errorMessage;

            public SendExecuteGenericError(HttpStatusCode code, ErrorTypeCode errorType, string errorMessage)
            {
                this.errorType = errorType;
                this.httpCode = code;
                this.errorMessage = errorMessage;
            }
        }

        public enum ErrorTypeCode
        {
            None,
            Unknown,
            IPPhone,
            Http,
            Connectivity
        }
    }
}

