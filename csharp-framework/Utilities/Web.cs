using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Metreos.Utilities
{
	/// <summary> 
	///     Encapsulates common functionality for communication across the net 
	/// </summary>
	/// <remarks> 
	///     MSC: The downloading needs to be tested against many different URLs, 
	///     as I've seen it fail once before.
	///     I think it may have to do with 'chunked' transfer, but I'm really not sure 
	/// </remarks>
    public class Web
    {
        /// <summary> 
        ///     Downloads a file over http
        /// </summary>
        /// <param name="url">
        ///     The url from which to fetch file
        /// </param>
        /// <returns>
        ///     boolean indicating if successfully fetched local
        /// </returns>
        public static UrlStatus Download(string url, string path) 
        {
            Uri uri = null;
            bool result = false;
            try   { uri = new Uri(url); result = true; }
            catch { }
            return !result || uri.IsFile? UrlStatus.Invalid: Download(uri, path);
        }
 
        /// <summary> Download content to a open MemoryStream, positioned at 0 </summary>
        /// <remarks>
        ///     From a design standpoint, this is the lowest common denominator in all
        ///     the download related methods in this utility
        /// </remarks>
        /// <param name="url"> 
        ///     The url of the data 
        /// </param>
        /// <param name="data"> 
        ///     The data in an open MemoryStream
        /// </param>
        /// <returns> Status of the download </returns>
        public static UrlStatus Download(string url, out MemoryStream data) 
        {
            UrlStatus result = UrlStatus.Success;
            HttpWebRequest request    = null;
            HttpWebResponse response  = null;
            Stream stream             = null;
            data                      = null;

            try 
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = false;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                byte[] buffer = new byte[2048];
                data = new MemoryStream(2048);
                
                int n = 0;
                int bytesRead = 0;

                while (true) 
                {
                    n = stream.Read(buffer, 0, buffer.Length);
                    if (n == 0) break;
          
                    data.Write(buffer, 0, n);
                    bytesRead += n;
                }
            }
            catch(WebException e) 
            {
                if(e.Status == WebExceptionStatus.ConnectFailure || 
                    e.Status == WebExceptionStatus.ConnectionClosed ||
                    e.Status == WebExceptionStatus.Timeout ||
                    e.Status == WebExceptionStatus.NameResolutionFailure)
                    result = UrlStatus.Unreachable;
                else  result = UrlStatus.CommunicationError;
                if(data != null) data.Close();
            }
            catch 
            {
                result = UrlStatus.CommunicationError;    
                if(data != null) data.Close();
            }
            finally
            {
                if(stream != null) 
                {      
                    stream.Flush(); 
                    stream.Close(); 
                    if(response != null) response.Close();
                }
            }

            data.Seek(0, SeekOrigin.Begin);
            return result;
        }

        /// <summary>
        ///     Downloads content from a URI to a file specified by path. Will overwrite
        /// </summary>
        /// <param name="uri"> 
        ///     The URI to download from 
        /// </param>
        /// <param name="path">
        ///     The path of the file to save to
        /// </param>
        /// <returns>
        ///     The status of the download
        /// </returns>
        public static UrlStatus Download(System.Uri uri, string path) 
        {  
            MemoryStream localStorage = null;
            UrlStatus result = Download(uri.ToString(), out localStorage);

            if(result != UrlStatus.Success) return result;
      
            bool wroteFile = false;  // Construct temp file path
            FileInfo tempFile = new FileInfo(path);
            FileStream fileStream = null;

            try 
            {
                fileStream = tempFile.Open(FileMode.Create);
                localStorage.WriteTo(fileStream);
                wroteFile = true;
            }
            catch   { }
            finally { localStorage.Close(); }
     
            if (fileStream != null) fileStream.Close();
            return wroteFile? UrlStatus.Success : UrlStatus.TempFileError;
        }  

        
        /// <summary> Download content to byte[] of data</summary>
        /// <param name="url"> The url of the data </param>
        /// <param name="data">The response data, if the result is UrlStatus.Success</param>
        /// <returns>The success status of the download.</returns>
        public static UrlStatus Download(string url, out byte[] data)
        {
            data = null;
            MemoryStream dataStream;
            UrlStatus status = Download(url, out dataStream);

            if(status == UrlStatus.Success)
            {
                data = dataStream.ToArray();
            }

            return status;
        }


        /// <summary> 
        ///     Given a url and Serializable type, download the XML 
        ///     and parse into a .NET type 
        /// </summary>
        /// <param name="url"> 
        ///     The location of the valid XML that corresponds 
        ///     to xmlSerializableType Type
        /// </param>
        /// <param name="xmlSerializableType"> 
        ///     The Type to deserialize 
        /// </param>
        /// <param name="result"> 
        ///     The resulting serializable Type 
        /// </param>
        /// <returns>
        ///     The result of accessing the remote stream 
        /// </returns>
        public static UrlStatus XmlDeserialize(string url, Type xmlSerializableType, out object result)
        {
            result = null;
            Debug.Assert(xmlSerializableType.IsSerializable, 
                "Unable to deserialize a type that is not serializable.");

            UrlStatus status = UrlStatus.Success;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(xmlSerializableType);
                status = XmlDeserialize(url, deserializer, out result);
            }
            catch { status = UrlStatus.Invalid; }

            return status;
        }

        /// <summary> 
        ///     Given a url and xml deserializer, download the XML 
        ///     and parse into a .NET type over HTTP
        /// </summary>
        /// <param name="url">
        ///     The location of the valid XML that corresponds to the XmlSeerializer
        /// </param>
        /// <param name="serializer"> 
        ///     The correct XmlSerializer to use in parsing the data 
        /// </param>
        /// <param name="result"> 
        ///     The resulting serializable Type 
        /// </param>
        /// <returns> 
        ///     The result of accessing the remote stream 
        /// </returns>
        public static UrlStatus XmlDeserialize(string url, XmlSerializer deserializer, out object result)
        {
            // Download content
            result              = null;
            MemoryStream data   = null;        
            UrlStatus status = Download(url, out data);
            if(status != UrlStatus.Success) return status;

             // Deserialize content
            try
            {              
                result = deserializer.Deserialize(data);
            }
            catch { status = UrlStatus.Invalid; }
            finally { if(data != null) data.Close(); }

            return status;
        }
        
        public static UrlStatus XmlSerialize(string url, object requestData, out string responseData)
        {
            Debug.Assert(requestData != null,                   "A request can not be null in XmlSerialize");
            Debug.Assert(requestData.GetType().IsSerializable,  "The request data must be serializable");

            UrlStatus result = UrlStatus.Success;
            HttpWebRequest request    = null;
            HttpWebResponse response  = null;
            Stream requestStream      = null;
            Stream stream             = null;
            MemoryStream data         = null;
            responseData              = null;
            StreamReader memWrapper   = null; // I need this because XmlDeserialize straight from MemoryStream doesn't work with UTF-16

            try 
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = false;
                request.ContentType = "text/xml";
                request.Method = "POST";
                requestStream = request.GetRequestStream();
                XmlSerializer serializer = new XmlSerializer(requestData.GetType());
                serializer.Serialize(requestStream, requestData);

                if(requestStream != null)
                {
                    requestStream.Close();
                    requestStream = null;
                }

                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                data = GetResponseData(stream);
            }
            catch(WebException e) 
            {
                if(e.Status == WebExceptionStatus.ConnectFailure || 
                    e.Status == WebExceptionStatus.ConnectionClosed ||
                    e.Status == WebExceptionStatus.Timeout ||
                    e.Status == WebExceptionStatus.NameResolutionFailure)
                    result = UrlStatus.Unreachable;
                else  result = UrlStatus.CommunicationError;

                try
                {
                    if(e.Response != null)
                    {
                        data = GetResponseData(e.Response.GetResponseStream());
                    }
                    else if(data != null) data.Close();
                }
                catch { }
            }
            catch 
            {
                result = UrlStatus.CommunicationError;    
                if(data != null) data.Close();
            }
            finally
            {
                if(requestStream != null) requestStream.Close();
                if(stream != null) 
                {      
                    stream.Flush(); 
                    stream.Close(); 
                    if(response != null) response.Close();
                }
            }

            try
            {
                data.Seek(0, SeekOrigin.Begin);
                memWrapper = new StreamReader(data);
                responseData = memWrapper.ReadToEnd();
            } 
            catch { result = UrlStatus.Invalid; }
            finally 
            {
                if(memWrapper != null)
                {
                    // Closes underlying data stream
                    memWrapper.Close();
                }
                else if(data != null)
                {
                    // Seek() must have failed
                    data.Close();
                }
            } 

            return result;
        }

        /// <summary>
        ///     Sends an XML message over HTTP, and deserializes the response
        /// </summary>
        /// <param name="url">
        ///     Url to send to
        /// </param>
        /// <param name="requestData"> 
        ///     The serializable request 
        /// </param>
        /// <param name="responseData"> 
        ///     The response data 
        /// </param>
        /// <param name="responseType">
        ///     The serializable Type of the response
        /// </param>
        /// <returns></returns>
        public static UrlStatus XmlTransaction(
            string url, 
            object requestData, 
            Type responseType, 
            out object responseData)
        {
            Debug.Assert(requestData != null,                   "A request can not be null in XmlTransaction");
            Debug.Assert(requestData.GetType().IsSerializable,  "The request data must be serializable");
            Debug.Assert(responseType.IsSerializable,           "The response data must be serializable");

            UrlStatus result = UrlStatus.Success;
            HttpWebRequest request    = null;
            HttpWebResponse response  = null;
            Stream requestStream      = null;
            Stream stream             = null;
            MemoryStream data         = null;
            responseData              = null;
            StreamReader memWrapper   = null; // I need this because XmlDeserialize straight from MemoryStream doesn't work with UTF-16

            try 
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = false;
                request.ContentType = "text/xml";
                request.Method = "POST";
                requestStream = request.GetRequestStream();
                XmlSerializer serializer = new XmlSerializer(requestData.GetType());
                serializer.Serialize(requestStream, requestData);

                if(requestStream != null)
                {
                    requestStream.Close();
                    requestStream = null;
                }

                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                data = GetResponseData(stream);
            }
            catch(WebException e) 
            {
                if(e.Status == WebExceptionStatus.ConnectFailure || 
                    e.Status == WebExceptionStatus.ConnectionClosed ||
                    e.Status == WebExceptionStatus.Timeout ||
                    e.Status == WebExceptionStatus.NameResolutionFailure)
                    result = UrlStatus.Unreachable;
                else  result = UrlStatus.CommunicationError;

                try
                {
                    if(e.Response != null)
                    {
                        data = GetResponseData(e.Response.GetResponseStream());
                    }
                    else if(data != null) data.Close();
                }
                catch { }
            }
            catch 
            {
                result = UrlStatus.CommunicationError;    
                if(data != null) data.Close();
            }
            finally
            {
                if(requestStream != null) requestStream.Close();
                if(stream != null) 
                {      
                    stream.Flush(); 
                    stream.Close(); 
                    if(response != null) response.Close();
                }
            }

            try
            {
                data.Seek(0, SeekOrigin.Begin);
                memWrapper = new StreamReader(data);
                XmlSerializer deserializer = new XmlSerializer(responseType);
                responseData = deserializer.Deserialize(memWrapper);
            } 
            catch { result = UrlStatus.Invalid; }
            finally 
            {
                if(memWrapper != null)
                {
                    // Closes underlying data stream
                    memWrapper.Close();
                }
                else if(data != null)
                {
                    // Seek() must have failed
                    data.Close();
                }
            } 

            return result;
        }

        /// <summary>
        ///     Sends an XML message over HTTP, and returns the response as a string
        /// </summary>
        /// <param name="url">
        ///     Url to send to
        /// </param>
        /// <param name="requestData"> 
        ///     The serializable request 
        /// </param>
        /// <param name="responseData"> 
        ///     The response data 
        /// </param>
        public static UrlStatus UpXmlDownStringTransaction(
            string url, 
            object requestData, 
            out string responseData)
        {
            Debug.Assert(requestData != null,                   "A request can not be null in XmlTransaction");
            Debug.Assert(requestData.GetType().IsSerializable,  "The request data must be serializable");
            
            UrlStatus result = UrlStatus.Success;
            HttpWebRequest request    = null;
            HttpWebResponse response  = null;
            Stream requestStream      = null;
            Stream stream             = null;
            MemoryStream data         = null;
            responseData              = null;

            try 
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
                request.ContentType = "text/xml";
                request.KeepAlive = false;
                request.Method = "POST";
                requestStream = request.GetRequestStream();
                XmlSerializer serializer = new XmlSerializer(requestData.GetType());
                serializer.Serialize(requestStream, requestData);

                if(requestStream != null)
                {
                    requestStream.Close();
                    requestStream = null;
                }

                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                data = GetResponseData(stream);
            }
            catch(WebException e) 
            {
                if(e.Status == WebExceptionStatus.ConnectFailure || 
                    e.Status == WebExceptionStatus.ConnectionClosed ||
                    e.Status == WebExceptionStatus.Timeout ||
                    e.Status == WebExceptionStatus.NameResolutionFailure)
                    result = UrlStatus.Unreachable;
                else  result = UrlStatus.CommunicationError;
                
                try
                {
                    if(e.Response != null)
                    {
                        data = GetResponseData(e.Response.GetResponseStream());
                    }
                    else if(data != null) data.Close();
                }
                catch { }
            }
            catch 
            {
                result = UrlStatus.CommunicationError;    
                if(data != null) data.Close();
            }
            finally
            {
                if(requestStream != null) requestStream.Close();
                if(stream != null) 
                {      
                    stream.Flush(); 
                    stream.Close(); 
                    if(response != null) response.Close();
                }
            }

            try
            {
                data.Seek(0, SeekOrigin.Begin);
                byte[] responseBuffer = new byte[data.Length];
                data.Write(responseBuffer, 0, responseBuffer.Length);
                responseData = System.Text.Encoding.Default.GetString(responseBuffer);
            } 
            catch { result = UrlStatus.Invalid; }
            finally { } 

            return result;
        }

        public static MemoryStream GetResponseData(Stream stream)
        {
            byte[] buffer = new byte[2048];
            MemoryStream data = new MemoryStream(2048);
                
            int n = 0;
            int bytesRead = 0;

            while (true) 
            {
                n = stream.Read(buffer, 0, buffer.Length);
                if (n == 0) break;
          
                data.Write(buffer, 0, n);
                bytesRead += n;
            }

            return data;
        }

        /// <summary>
        ///     Posts a XML message over HTTP using URL encoding, and deserializes the response. 
        ///     Posts the XML message with no <?xml version="1.0" ?>
        ///     Posts the XML message with no xmlns attributes on root node
        ///     Expects the <?xml version="1.0"> on the receiving end, however.
        /// </summary>
        /// <param name="url">
        ///     Url to POST to
        /// </param>
        /// <param name="requestData"> 
        ///     The serializable request 
        /// </param>
        /// <param name="responseData"> 
        ///     The response data 
        /// </param>
        /// <param name="responseType">
        ///     The serializable Type of the response
        /// </param>
        /// <returns></returns>
        public static UrlStatus CiscoPostXmlTransaction(
            string url, 
            object requestData, 
            Type responseType, 
            out object responseData)
        {
            Debug.Assert(requestData != null,                   "A request can not be null in XmlTransaction");
            Debug.Assert(requestData.GetType().IsSerializable,  "The request data must be serializable");
            Debug.Assert(responseType.IsSerializable,           "The response data must be serializable");

            UrlStatus result          = UrlStatus.Success;
            HttpWebRequest request    = null;
            HttpWebResponse response  = null;
            Stream requestStream      = null;
            Stream stream             = null;
            MemoryStream data         = null;
            MemoryStream hackOffXml   = null;
            MemoryStream requestWriter= null;
            XmlDeclarationRemover rem = null;
            StreamReader streamer     = null;
            responseData              = null;

            try 
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
                request.ContentType = "application/x-www-form-urlencoded";
 
                request.KeepAlive = false;
                request.Method = "POST";
                
                // Hack off the xml declaration element <?xml version="1.0" ?>
                hackOffXml = new MemoryStream(200);
                rem = new XmlDeclarationRemover(hackOffXml, System.Text.Encoding.ASCII);
                XmlSerializer serializer = new XmlSerializer(requestData.GetType());

                // Hack off namespace attributes
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(String.Empty, String.Empty);
                serializer.Serialize(rem, requestData, namespaces);
                hackOffXml.Seek(0, SeekOrigin.Begin);
                
                #region Debugging Code
                // START WRITE TO FILE TEMP
//                FileStream stream3 = File.Open("request.xml", FileMode.Create);
//                hackOffXml.WriteTo(stream3);
//                stream3.Close();
//                hackOffXml.Seek(0, SeekOrigin.Begin);   
                // END TEMP WRITE TO FILE
                #endregion Debugging Code

                // Conform to form encoding
                streamer = new StreamReader(rem.BaseStream); 
                string encodedformContent = System.Web.HttpUtility.UrlEncode(streamer.ReadToEnd());
                encodedformContent = "xml=" + encodedformContent;
                request.ContentLength = encodedformContent.Length;     
                
                streamer.Close();
                rem.Close();
                streamer = null;
                rem = null;

                requestStream = request.GetRequestStream();
                byte[] formData = System.Text.Encoding.ASCII.GetBytes(encodedformContent);
                requestWriter = new MemoryStream(formData.Length);
                requestWriter.Write(formData, 0, formData.Length);
                requestWriter.WriteTo(requestStream);
                requestWriter.Close();
                requestWriter = null;

                if(requestStream != null)
                {
                    requestStream.Close();
                    requestStream = null;
                }

                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                byte[] buffer = new byte[2048];
                data = new MemoryStream(2048);
                
                int n = 0;
                int bytesRead = 0;

                while (true) 
                {
                    n = stream.Read(buffer, 0, buffer.Length);
                    if (n == 0) break;
          
                    data.Write(buffer, 0, n);
                    bytesRead += n;
                }
            }
            catch(WebException e) 
            {
                if(e.Status == WebExceptionStatus.ConnectFailure || 
                    e.Status == WebExceptionStatus.ConnectionClosed ||
                    e.Status == WebExceptionStatus.Timeout ||
                    e.Status == WebExceptionStatus.NameResolutionFailure)
                    result = UrlStatus.Unreachable;
                else  result = UrlStatus.CommunicationError;
                if(data != null) data.Close();    
            }
            catch 
            {
                result = UrlStatus.CommunicationError;    
                if(data != null) data.Close();
            }
            finally
            {
                if(rem != null) rem.Close();
                if(streamer != null) rem.Close();
                if(requestWriter != null) requestWriter.Close();
                if(requestStream != null) requestStream.Close();
                if(stream != null) 
                {      
                    stream.Flush(); 
                    stream.Close(); 
                    if(response != null) response.Close();
                }
            }

            if(result != UrlStatus.Success) return result;

            try
            {
                #region Debugging Code
                // Start TEMP
//                data.Seek(0, SeekOrigin.Begin);   
//                FileStream stream2 = File.Open("response.xml", FileMode.Create);
//                data.WriteTo(stream2);
//                stream2.Close();
                // End TEMP
                #endregion Debugging Code
                
                data.Seek(0, SeekOrigin.Begin);   
                XmlSerializer deserializer = new XmlSerializer(responseType, String.Empty);
                responseData = deserializer.Deserialize(data);     
            } 
            catch { result = UrlStatus.Invalid; }
            finally { data.Close(); } 

            return result;
        }
    }

    /// <summary>
    ///     A simple wrapper around XmlTextWriter to not write the xml declaration
    /// </summary>
    public class XmlDeclarationRemover : XmlTextWriter
    {
        public XmlDeclarationRemover(Stream stream, System.Text.Encoding encoding) :
            base (stream, encoding) { }
        
        /// <summary>
        ///     Do not write XML declaration
        /// </summary>
        public override void WriteStartDocument() { }

        /// <summary>
        ///     Do not write XML declaration
        /// </summary>
        public override void WriteStartDocument(bool standalone) { }
    }
    /// <summary> Defines the different communication statuses that occur  </summary>
    public enum UrlStatus 
    {
        Success, 
        Invalid, 
        Unreachable, 
        FileNotFound, 
        CommunicationError, 
        TempFileError
    }
}
