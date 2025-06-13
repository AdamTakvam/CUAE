using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;
using Metreos.Types.CiscoIpPhone;
using Metreos.Configuration;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Actions.SendExecute;

namespace Metreos.Native.CiscoIpPhone
{
    /// <summary>
    /// Summary description for SendExecute.
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoIpPhone.Globals.PACKAGE_DESCRIPTION)]
    public class SendExecute : INativeAction
    {
        public const string CONTENT_TYPE    = "application/x-www-form-urlencoded";

        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public object ResultData { get { return resultData; } }
        private object resultData;

        [ResultDataField(Package.Results.ErrorType.DISPLAY, Package.Results.ErrorType.DESCRIPTION)]
        public string ErrorType { get { return errorType; } }
        private string errorType;

        [ActionParamField(Package.Params.Message.DISPLAY, Package.Params.Message.DESCRIPTION, true, Package.Params.Message.DEFAULT)]
        public string Message { set { msg = value; } }
        private string msg;

        [ActionParamField(Package.Params.URL.DISPLAY, Package.Params.URL.DESCRIPTION, true, Package.Params.URL.DEFAULT)]
        public string URL { set { url = value; } }
        private string url;

        [ActionParamField(Package.Params.Username.DISPLAY, Package.Params.Username.DESCRIPTION, false, Package.Params.Username.DEFAULT)]
        public string Username { set { username = value; } }
        private string username;

        [ActionParamField(Package.Params.Password.DISPLAY, Package.Params.Password.DESCRIPTION, false, Package.Params.Password.DEFAULT)]
        public string Password { set { password = value; } }
        private string password;

        public SendExecute() { Clear(); }

        private static Regex doubleXmlMatcher       = new Regex(@"(<\?xml.*\?>){2}", RegexOptions.Multiline | RegexOptions.Compiled);
        private static Regex noDeclMatcher          = new Regex(@"(<\?xml\s+version=""1.0""\s*encoding="".*""\s*\?>)", RegexOptions.Multiline | RegexOptions.Compiled); 
        private static Regex ampersandReplacer      = new Regex("&(?!amp;)", RegexOptions.Compiled);
        private static Regex emptyEncodingMatcher   = new Regex(@"(<\?xml\s+version=""1.0""\s*encoding="""".*\s*\?>)", RegexOptions.Multiline | RegexOptions.Compiled);

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            resultData = null;
            errorType = Response.ErrorTypeCode.None.ToString();
            username = null;
            password = null;
            url = null;
            msg = null;
        }

        
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            string response = null;
            HttpStatusCode httpCode;
            Response.ErrorTypeCode requestErrorType;

            // If SendExecuteHelper fails, either the phone could not be reached, or an HTTP error occured
            // If it succeeds, then a response was received
            if(!SendExecuteHelper(url, msg, username, password, out response, out requestErrorType, out httpCode))
            {
                resultData = new Response.SendExecuteGenericError(httpCode, requestErrorType, response);
                errorType = requestErrorType.ToString();
                return IApp.VALUE_FAILURE;
            }
          
            // The phone responded with some sort of message, so determine information about it
            if(!ParseResponse(response, ref resultData))
            {
                // To make a consistent API for developers, we now check the HTTP Status Code
                // of the request.  If it isn't 200, and if the message back from this HTTP Server
                // is not a parsable IP Phone response, let's ins
                log.Write(TraceLevel.Error, 
                    "Could not interpret response from Cisco phone:\n" + response);
                resultData = new Response.SendExecuteGenericError(httpCode, Response.ErrorTypeCode.Unknown, response);
                errorType = Response.ErrorTypeCode.Unknown.ToString(); // This isn't a fundamental error type, but certainly bad behavior
                return IApp.VALUE_FAILURE;
            }

            #region Cisco Error Codes
            //Error 1 = Error parsing CiscoIPPhoneExecute object 
            //Error 2 = Error framing CiscoIPPhoneResponse object 
            //Error 3 = Internal file error 
            //Error 4 = Authentication error 
            #endregion

            // Cisco 7970 IP phones will not respond with a 401 Unauthorized HTTP response.
            // Instead, they return am Cisco IP Phone Error XML blob containing the reason
            // for the failure.
            if((resultData is CiscoIPPhoneErrorType) || (resultData == null))
            {
                errorType = Response.ErrorTypeCode.IPPhone.ToString();
                log.Write(TraceLevel.Error, "Phone was able to process request, but replied with an error: {0}", response);
                return IApp.VALUE_FAILURE;
            }
            else
            {
                errorType = Response.ErrorTypeCode.None.ToString();
                return IApp.VALUE_SUCCESS;
            }
        }

        /// <summary> Sends a preauthenticated request to a Cisco IP Phone, handling any further negotiation </summary>
        /// <param name="url">IPAddress, http://IPAddress, IPAddress/CGI/Execute, or http://IPAddress/CGI/Execute</param>
        /// <param name="body">A url-encoded message with an XML=</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="response">The response string from the final phone response</param>
        /// <returns> True if a final response could be obtained and no HTTP violations occur (400, 500, malformed), 
        /// otherwise false</returns>
        public bool SendExecuteHelper(string url, string body, string username, string password, out string response, out Response.ErrorTypeCode errorType, out HttpStatusCode httpCode)
        {
            errorType = Response.ErrorTypeCode.None;
            httpCode = HttpStatusCode.OK;

            response = null;
            Socket socket = null;
            MemoryStream data =null;    // Not needed except for debugging. We store all the data of the HTTP responses from 
                                        // the phone for debug output
            try
            {
                // The API allows the user to simply enter
                // IPAddress, http://IPAddress, IPAddress/CGI/Execute, or http://IPAddress/CGI/Execute
                if(!url.StartsWith("http://"))
                {
                    url = "http://" + url;
                }
                if(!url.EndsWith("/CGI/Execute"))
                {
                    url += "/CGI/Execute";
                }

                Uri uri = new Uri(url);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    socket.Connect(new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port));
                }
                catch
                {
                    log.Write(TraceLevel.Error, "Unable to open a connection to the remote host in SendExecute.  Check the IP address of the device.");
                    errorType = Response.ErrorTypeCode.Connectivity;
                    return false;
                }

                // The body string should be sent as the form field 'XML'
                // JDL, localization, prepend valid XML header before body
                if (Config.Instance.ShouldIncludeXMLHeader())
                    body = "XML=<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Metreos.Utilities.Url.Encode(body);
                else
                    body = Metreos.Utilities.Url.Encode(body);

                IPEndPoint localEP = socket.LocalEndPoint as IPEndPoint;
                
                // Construct initial SendExecute request, which uses preauthentication
                HttpMessage message = new HttpMessage(log, localEP.Address.ToString(), localEP.Port);
                message.headers = new Hashtable();
                message.requestMethod = "POST";
                message.username = username;
                message.password = password;
                message.headers["Content-Type"] = "application/x-www-form-urleencoded";
                message.headers["Accept"] = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                message.headers["Accept-Language"] = "en-us,en;q=0.5";
                message.headers["Accept-Encoding"] = "gzip,deflate";
                message.headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                message.headers["Keep-Alive"] = "300";
                message.headers["Connection"] = "keep-alive";
                message.version = "1.1";
                message.requestUri = uri;
                message.type = HttpMessage.Type.Request;
                message.Body = body;
  
                string preauthenticatedRequest = message.ToString(true);
                // JDL, localization, force UTF8 encoding
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(preauthenticatedRequest);
                socket.Send(buffer);

                int n = 0;
                int bytesRead = 0;

                buffer = new byte[2048];
                message = new HttpMessage(log, localEP.Address.ToString(), localEP.Port);
                data = new MemoryStream();

                // Read from the phone, while incrementally parsing the HTTP message.  If the 
                // phone closes the connection, or if the message is complete, we stop reading from the socket.
                while (true)
                {
                    n = socket.Receive(buffer);
                    if (n == 0) break;
                    
                    data.Write(buffer, 0, n);
                    // JDL, localization, force UTF8 encoding
                    string appendMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, n);
                    bool done = message.Parse(appendMessage);
                    if(done) break;

                    bytesRead += n;
                }

                // Unable to communicate with the phone because of HTTP parsing problems
                if(message.ParseError)
                {
                    string failedResponse = "No response from the phone";
                    if(data != null)
                   { 
                        data.Position = 0;
                        StreamReader reader = new StreamReader(data);
                        failedResponse = reader.ReadToEnd();
                        if(failedResponse == null || failedResponse == String.Empty)
                        {
                            failedResponse = "No response from the phone";
                        }
                        reader.Close();
                        data = new MemoryStream();
                    }

                    log.Write(TraceLevel.Error,
                        "Unable to parse the HTTP response from the phone." +  
                        "\nHTTP request sent from Application Server: {0}\nHTTP response from phone: \n{1}", 
                        preauthenticatedRequest, failedResponse);
                    errorType = Response.ErrorTypeCode.Unknown;
                    return false;
                }

                // Clean up our temporary storage of debug info
                if(data != null)
                {
                    data.Position = 0;
                    data.SetLength(0);
                }

                try
                {
                    // Attempt to assign the response code to an HttpStatusCode
                    httpCode = (HttpStatusCode) Enum.Parse(typeof(HttpStatusCode), message.responseCode, true);
                }
                catch{}

                // Incorrect credentials.  This exception will be caught
                // when communicating with Cisco 7940/60 IP phones.  Cisco
                // 7970 IP phones do not respond with a 401.
                if(IsErrorCode(message.responseCode))
                {
                    log.Write(TraceLevel.Error, "Phone declined SendExecute request.\nHTTP request sent from Application Server: {0}\nHTTP response from phone: \n{1}",
                        preauthenticatedRequest, message.ToString());
                    response = message.Body;
                    errorType = Response.ErrorTypeCode.Http;
                    return false;
                }

                string gotoOther = message.headers["Location"] as string;   
                if(gotoOther == null && message.responseCode == "200")
                {
                    // Observed: this occurs when the phone responds directly to a request with a 
                    // CiscoIPPhoneError message
                    response = message.Body;
                    return true;
                }
                if(gotoOther == null)
                {
                    // Unknown possibility.  Output the message to/from the phone 
                    // for diagnotistics, and assume failure
                    log.Write(TraceLevel.Error, "Unknown phone response.\nHTTP request sent from Application Server: {0}\nHTTP response from phone: {1}",
                        preauthenticatedRequest, message.ToString());
                    response = message.Body;
                    errorType = Response.ErrorTypeCode.Unknown;
                    return false;
                }


                // Check if goto is different port, or not
                Uri goToOtherUri = new Uri(gotoOther);
                if(goToOtherUri.Port != uri.Port)
                {
                    if(socket != null && socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    if(socket != null)
                    {
                        socket.Close();
                    }

                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        socket.Connect(new IPEndPoint(IPAddress.Parse(goToOtherUri.Host), goToOtherUri.Port));
                        localEP = socket.LocalEndPoint as IPEndPoint;
                    }
                    catch
                    {
                        log.Write(TraceLevel.Error, "Unable to open a connection to the remote host in SendExecute redirect.  Check the IP address of the device.");
                        errorType = Response.ErrorTypeCode.Connectivity;
                        return false;
                    }
                }

                // Build goto request, which contains the XML response
                message = new HttpMessage(log, localEP.Address.ToString(), localEP.Port);
                message.headers = new Hashtable();
                message.headers["Accept"] = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                message.headers["Accept-Language"] = "en-us,en;q=0.5";
                message.headers["Accept-Encoding"] = "gzip,deflate";
                message.headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                message.headers["Keep-Alive"] = "300";
                message.headers["Connection"] = "keep-alive";
                message.version = "1.1";
                message.type = HttpMessage.Type.Request;
                message.requestUri = new Uri(gotoOther);
                message.requestMethod = "GET";

                string responseXmlRequest = message.ToString();
                // JDL, localization, force UTF8 encoding
                buffer = System.Text.Encoding.UTF8.GetBytes(responseXmlRequest);
                socket.Send(buffer);

                n = 0;
                bytesRead = 0;

                buffer = new byte[2048];
                message = new HttpMessage(log, localEP.Address.ToString(), localEP.Port);

                while (true) 
                {  
                    n = socket.Receive(buffer);
                    if (n == 0) break;

                    // JDL, localization, force UTF8 encoding
                    string appendMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, n);
                    bool done = message.Parse(appendMessage);
                    if(done) break;

                    bytesRead += n;
                }

                response = message.Body;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "SendExecute failure: " + Metreos.Utilities.Exceptions.FormatException(e));
                log.Write(TraceLevel.Error, "Url: " + url);
                errorType = Response.ErrorTypeCode.Unknown;
                return false;
            }
            finally
            {
                if(socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
                if(socket != null)
                {
                    socket.Close();
                }
            }

            return true;
        }


        /// <summary> A valid 400-500 error code is checked for, 
        /// as well as a malformed errorcode (which does count as an error code)</summary>
        public bool IsErrorCode(string codeStr)
        {
            bool isError = true;
            if(codeStr == null || codeStr == null)
            {
                isError = true;
                return isError;
            }

            try
            {
                int code = int.Parse(codeStr);
                if(code >= 400 && code <= 599)
                {
                    isError = true;
                }
                else
                {
                    isError = false;
                }
            }
            catch { isError = true; }

            return isError;
        }

        public bool ParseResponse(string responseXml, ref object resultData)
        {
            // Take out the damn second <?xml version="1.0"?> statement
            responseXml = PruneDoubleXmlDeclaration(responseXml);
            // Add an xml declaration if none exists
            responseXml = AddXmlDeclaration(responseXml);
            // Take out XML invalid characters.  This is a really big problem, because 
            // simply escaping all invalid characters is not correct.  Hopefully, the & is the only common one.
            responseXml = ReplaceInvalidXmlChars(responseXml);
            responseXml = CorrectEmptyEncoding(responseXml);

            System.IO.TextReader reader = new System.IO.StringReader(responseXml);

            // Find out if this is a response type or an error type
            if(responseXml.IndexOf("<CiscoIPPhoneResponse") != -1)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneResponseType));
                    resultData = (CiscoIPPhoneResponseType) serializer.Deserialize(reader);
                    return true;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "The CiscoIPPhoneResponse XML found does not conform to what was expected.  {0}", Exceptions.FormatException(e));
                    return false;
                }
            }
            else if(responseXml.IndexOf("<CiscoIPPhoneError") != -1)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CiscoIPPhoneErrorType));
                    resultData = (CiscoIPPhoneErrorType) serializer.Deserialize(reader);
                    return true;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "The CiscoIPPhoneError XML found does not conform to what was expected.  {0}", Exceptions.FormatException(e));
                    return false;
                }
            }

            return false;
        }

        public string PruneDoubleXmlDeclaration(string xml)
        {
            bool foundDouble = doubleXmlMatcher.IsMatch(xml);

            if(!foundDouble)
            {
                return xml;
            }

            int p = 0;
            int q = xml.IndexOf("?>");
            p = xml.IndexOf("<?xml", q+2);
            if(p != -1)
            {
                q = xml.IndexOf("?>", p);
                q += 2;
                xml = xml.Remove(p, (q-p));
            }

            return xml;
        }

        public string AddXmlDeclaration(string xml)
        {
            bool foundDecl = noDeclMatcher.IsMatch(xml);

            if(foundDecl)
            {
                return xml;
            }
            else
            {
                xml = "<?xml version=\"1.0\" encoding=\"iso-8859-1\" ?>\r\n" + xml;
                return xml;
            }
        }

        public string CorrectEmptyEncoding(string xml)
        {
            return emptyEncodingMatcher.Replace(xml, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>", 1);
        }

        /// <summary>
        ///     Currently replaces  '&' with &amp; 
        ///     Does not currently escape '"' with &quot;   
        /// </summary>
        /// <param name="xml">
        ///     The string to replace characters for
        /// </param>
        /// <returns>
        ///     The replaced xml
        /// </returns>
        public string ReplaceInvalidXmlChars(string xml)
        {
            // Replace '&' with &amp;
            xml = ampersandReplacer.Replace(xml, "&amp;");

            return xml;
        }
    }


    public sealed class HttpMessage
    {
        private LogWriter log;

        // HTTP message type
        public enum Type { Request, Response, Unspecified }

        public Type type;

        private string stackListenInterface;
        private int stackListenPort;

        // Request header stuff
        public string requestMethod;
        public Uri requestUri;
        public string queryParams;

        // Response header stuff
        public string responseCode;
        public string responsePhrase;

        // Common stuff
        public string version;
        public Hashtable headers;   // NOTE: Case insensitive hash table
        public Hashtable cookies;   // NOTE: Case insensitive hash table
        public string username;
        public string password;

        // Body stuff
        private bool hasBody;
        public bool HasBody { get { return hasBody; } }
    
        private string body;
        public string Body
        {
            get { return body; }
            set 
            {
                if(value != null)
                {
                    hasBody = true;
                    body = value;
                    // JDL, localization, force UTF8 encoding
                    headers.Add(IHttp.Headers.CONTENT_LENGTH, System.Text.Encoding.UTF8.GetBytes(body).Length.ToString());
                }
            }
        }

        // Error flag
        private bool parseError;
        public bool ParseError { get { return parseError; } }

        // Private partial-parse stuff
        private enum State { Init, MidHeaders, GotHeaders, MidBody, Complete }
        private State state;
        private string orphanBuffer;
        private string relativeUri;
        private string rawBody;
        private int contentLength;
        private bool chunked;
        private int chunkSize;

        public HttpMessage(LogWriter log, string stackListenInterface, int stackListenPort)
        {
            this.log = log;
            this.stackListenInterface = stackListenInterface;
            this.stackListenPort = stackListenPort;

            Initialize();
        }

        private void Initialize()
        {
            // Create a case insensitive hash table for
            // the headers and cookies collection.
            headers = CollectionsUtil.CreateCaseInsensitiveHashtable();
            cookies = CollectionsUtil.CreateCaseInsensitiveHashtable();

            requestUri = null;
            version = "1.1";
            requestMethod = null;
            responseCode = null;
            responsePhrase = null;
            hasBody = false;
            body = null;
            username = null;
            password = String.Empty;
            queryParams = String.Empty;
            type = Type.Unspecified;
            parseError = false;
            //---
            state = State.Init;
            orphanBuffer = null;
            relativeUri = null;
            rawBody = String.Empty;
            contentLength = 0;
            chunked = false;
            chunkSize = 0;
        }

        #region Raw HTTP parser

        public bool Parse(string rawHttp)
        {            
            if((rawHttp == null) || (parseError))
            {
                return false;
            }

            // Where are we at?
            switch(state)
            {
                case State.Init:
                    return StartParse(rawHttp);
                case State.MidHeaders:
                    return MidHeaderParse(rawHttp);
                case State.GotHeaders:
                    return StartBodyParse(rawHttp);
                case State.MidBody:
                    return MidBodyParse(rawHttp);
                case State.Complete:
                    return true;
                default:
                    return false;
            }
        }

        private bool StartParse(string rawHttp)
        {
            if(orphanBuffer != null)
            {
                rawHttp = orphanBuffer + rawHttp;
                orphanBuffer = null;
            }

            rawHttp = rawHttp.TrimStart(' ', '\r', '\n');

            int start = 0;
            int end = rawHttp.IndexOf("\r\n");

            if(end == -1)
            {
                orphanBuffer = rawHttp;
                return false;
            }

            string line = "";
            try
            {
                line = rawHttp.Substring(start, (end-start));
            }
            catch(Exception)
            {
                log.Write(TraceLevel.Warning, "Malformed HTTP request");
                parseError = true;
                return false;
            }

            // Save info from the request line
            string[] lineBits = line.Split(new char[] { ' ' });
        
            // OK && SEE OTHER
            if(lineBits.Length != 3 && lineBits.Length != 4)
            {
                log.Write(TraceLevel.Warning, "Malformed HTTP request");
                parseError = true;
                return false;
            }

            // Is it a request or response?
            lineBits[0] = lineBits[0].Trim('\n', '\r', ' ');
            if(lineBits[0].StartsWith("HTTP/"))
            {
                // It's a response
                type = HttpMessage.Type.Response;
                version = lineBits[0].Trim();
                responseCode = lineBits[1].Trim();
                responsePhrase = lineBits[2].Trim();
                if(lineBits.Length == 4)
                {
                    responsePhrase += " " + lineBits[3];
                }
            }
            else
            {
                // It's a request
                type = HttpMessage.Type.Request;
                requestMethod = lineBits[0].Trim();
                relativeUri = lineBits[1].Trim();
                version = lineBits[2].Trim();
            }

            // Parse out the "HTTP/" bit of the version
            version = version.Trim('H', 'T', 'P', '/', ' ');

            // Update the state
            state = State.MidHeaders;

            end += 2;
            if((rawHttp.Length-end) > 2)
            {
                return MidHeaderParse(rawHttp.Substring(end, (rawHttp.Length-end)));
            }
            else
            {
                // If all we got was a complete header line,
                //   we have no reason to assume there will be any headers
                state = State.Complete;
                PostParse();
                return true;
            }
        }

        private bool MidHeaderParse(string rawHttp)
        {
            if(orphanBuffer != null)
            {
                rawHttp = orphanBuffer + rawHttp;
                orphanBuffer = null;
            }

            bool fragment = false;

            // Determine whether we can see the end of the headers
            int headerEnd = rawHttp.IndexOf("\r\n\r\n");
            if(headerEnd == -1)
            {
                headerEnd = rawHttp.Length;
                fragment = true;
            }

            // Get the headers
            string headerline;
            string[] headerBits;
            string headerName;
            int offset = 0;
            int end;

            while(offset < headerEnd)
            {
                end = rawHttp.IndexOf("\r\n", offset);
                if(end == -1)
                {
                    orphanBuffer = rawHttp.Substring(offset, (rawHttp.Length-offset));
                    return false;
                }
                headerline = rawHttp.Substring(offset, (end-offset));
                headerBits = headerline.Split(new char[] {':'}, 2);
                if(headerBits.Length < 2)
                {
                    log.Write(TraceLevel.Warning, 
                        "Error parsing HTTP. Trying to recover.");
                    offset = end+2;
                    continue;
                }
                headerName = headerBits[0].Trim();
                if(string.Compare(headerName, IHttp.Headers.CONTENT_LENGTH, true) == 0)
                {
                    try
                    {
                        contentLength = int.Parse(headerBits[1].Trim());
                    }
                    catch(Exception)
                    {
                        log.Write(TraceLevel.Warning, 
                            "Invalid Content-Length specified in HTTP message");
                        parseError = true;
                        return false;
                    }
                }
                else if(string.Compare(headerName, IHttp.Headers.ENCODING, true) == 0)
                {
                    string encoding = headerBits[1].Trim();
                    if(encoding.IndexOf(IHttp.Headers.ENCODING_CHUNKED) != -1)
                    {
                        chunked = true;
                    }
                    else
                    {
                        log.Write(TraceLevel.Warning, 
                            "Unsupported Transfer-Encoding: " + encoding);
                        parseError = true;
                        return false;
                    }
                }
                else if(string.Compare(headerName, IHttp.Headers.HEADER_COOKIE, true) == 0)
                {
                    string[] cookieList = headerBits[1].Trim().Split(';');
                    string cookieName;
                    string cookieValue;

                    if(cookieList == null) continue;

                    foreach(string cookie in cookieList)
                    {
                        string[] cookieBits = cookie.Split('=');
                        if(cookieBits.Length == 2)
                        {
                            cookieName = cookieBits[0];
                            cookieValue = cookieBits[1];
                            cookies.Add(cookieName, cookieValue);
                        }
                    }
                }
                else
                {
                    headers.Add(headerName, headerBits[1].Trim());
                }
                offset = end+2;
            }

            // Update state
            state = State.GotHeaders;

            // Are we done?
            if(!chunked && !fragment && (contentLength == 0))
            {
                state = State.Complete;
                PostParse();
                return true;
            }

            if(!fragment)
            {
                offset += 2;
                return StartBodyParse(rawHttp.Substring(offset, (rawHttp.Length-offset)));
            }
            return false;
        }

        private bool StartBodyParse(string rawHttp)
        {
            if((contentLength == 0) && (!chunked))
            {
                state = State.Complete;
                PostParse();
                return true;
            }

            if(orphanBuffer != null)
            {
                rawHttp = orphanBuffer + rawHttp;
                orphanBuffer = null;
            }

            if(rawHttp.Length <= 2)  // No body yet (leave room for CRLF error)
            {
                return false;
            }
        
            // Save raw body for posterity
            rawBody = rawHttp;
            
            if(!chunked)
            {
                // Get the rawHttp body
                if(rawHttp.Length < contentLength)
                {
                    orphanBuffer = rawHttp;

                    // Update state
                    state = State.MidBody;
                    return false;
                }

                Body = rawHttp.Substring(0, contentLength);

                state = State.Complete;
                PostParse();
                return true;
            }

            return MidBodyParse(rawHttp);
        }

        private bool MidBodyParse(string rawHttp)
        {
            if(orphanBuffer != null)
            {
                rawHttp = orphanBuffer + rawHttp;
                orphanBuffer = null;
            }

            if(!chunked)
            {
                if(rawHttp.Length < contentLength)
                {
                    orphanBuffer = rawHttp;
                    return false;
                }

                Body = rawHttp.Substring(0, contentLength);

                state = State.Complete;
                PostParse();
                return true;
            }

            // Decode the body
            int p = 0;
            int q = 0;

            rawHttp = rawHttp.TrimStart(' ', '\r', '\n');

            while(rawHttp.Length >= p)
            {    
                // Get next chunk size
                p += chunkSize;
                if(p == rawHttp.Length)  // end of chunk, but more are expected later
                {
                    chunkSize = 0;
                    return false;
                }
                q = rawHttp.IndexOf("\r\n", p);
                if(q == p)
                {
                    p += 2;
                    q = rawHttp.IndexOf("\r\n", p);
                }
                if(q == -1)
                {
                    q = rawHttp.Length;
                }
                try
                {
                    string chunkStr = rawHttp.Substring(p, (q-p));
                    chunkSize = System.Convert.ToInt32(chunkStr, 16);
                    if(chunkSize == 0)
                    {
                        break;
                    }
                    //chunkSize--;  // adjust for zero-based indexing // SETh
                }
                catch(Exception)
                {
                    log.Write(TraceLevel.Warning, 
                        "Invalid chunked message body:\n" + rawBody);
                    parseError = true;
                    return false;
                }

                p = q + 2;  // walk 

                if(rawHttp.Length < p+chunkSize)
                {
                    p -= 5;         // back up to save the chunk size
                    orphanBuffer = rawHttp.Substring(p, (rawHttp.Length-p));
                    chunkSize = 0;
                    return false;
                }

                // Save chunk
                body += rawHttp.Substring(p, chunkSize);
            }
        
            // Finish up
            body = body.Trim();

            // Update state
            state = State.Complete;
            PostParse();
            return true;
        }
        
        void PostParse()
        {            
            // Determine the fully-qualified URI
            if(type == Type.Request)
            {
                string host = headers[IHttp.Headers.HOST] as string;
                try
                {
                    if((host != null) && (host != String.Empty) && (relativeUri != null))
                    {
                        if(host.EndsWith(":" + this.stackListenPort.ToString()) == false)
                        {
                            // REFACTOR: Note, this does not handle the case of when
                            //           a bogus port is sent in.
                            host = host + ":" + this.stackListenPort.ToString();
                        }
                        requestUri = new Uri(Url.FormatUri(host) + relativeUri);
                    }
                    else if(version == "1.0")
                    {
                        requestUri = 
                            new Uri("http://" + this.stackListenInterface + ":" + this.stackListenPort.ToString() + relativeUri);
                    }
                    else
                    { 
                        // 1.1 can use relative uri's, but the Uri class cannot store relative uri's.
                        requestUri = 
                            new Uri("http://" + this.stackListenInterface + ":" + this.stackListenPort.ToString() + relativeUri);
                    }

                    queryParams = requestUri.Query.TrimStart('?');
                }
                catch(UriFormatException)
                {
                    log.Write(TraceLevel.Warning, "Received malformed URI");
                }
            }

            // Sanity check
            if(contentLength > 0)
            {
                if((contentLength != body.Length) &&
                    (contentLength-1 != body.Length) &&
                    (contentLength+1 != body.Length))
                {
                    log.Write(TraceLevel.Warning, "Error parsing HTTP. Content length does not match content found.");
                    log.Write(TraceLevel.Warning, "Content-Length: " + contentLength.ToString() +
                        "\nBody.Length: " + body.Length);
                }
            }
        }
        #endregion

        #region HTTP Serializer
        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool useAuthentication)
        {
            StringBuilder sb = new StringBuilder(256);

            if(type == Type.Request)
            {
                // Build request-line
                sb.Append(requestMethod);
                sb.Append(" ");
                sb.Append(requestUri.AbsolutePath);
                sb.Append(" HTTP/");
                sb.Append(version);
                sb.Append("\r\n");
            }
            else if(type == Type.Response)
            {
                // Build status-line
                sb.Append("HTTP/");
                sb.Append(version);
                sb.Append(" ");
                sb.Append(responseCode);
                sb.Append(" ");
                if(responsePhrase == null)
                {
                    responsePhrase = GetResponsePhrase(responseCode);
                    if(responsePhrase == null)
                    {
                        responsePhrase = "Unspecified";
                    }
                }
                sb.Append(responsePhrase);
                sb.Append("\r\n");
            }
            else
            {
                return "";
            }

            // Insert authentication info
            if(useAuthentication && (username != null))
            {
                string userPass = username + ":" + password;
                // JDL, localization, force UTF8 encoding
                string userPassEncoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userPass));
                sb.Append("Authorization: Basic ");
                sb.Append(userPassEncoded);
                sb.Append("\r\n");                
            }

            // Insert Host
            if(type == Type.Request)
            {
                sb.Append("Host: ");
                sb.Append(requestUri.IsDefaultPort ? requestUri.Host : requestUri.Host + ':' + requestUri.Port);
                sb.Append("\r\n");
            }

            // Insert Server or User-Agent, as appropriate
            if(type == Type.Response)
            {
                sb.Append("Server: Metreos-Samoa/1.0\r\n");
            }
            else
            {
                sb.Append("User-Agent: Metreos-Samoa/1.0\r\n");
            }

            // Insert Session ID


            // Insert misc headers
            IDictionaryEnumerator de = headers.GetEnumerator();
            while(de.MoveNext() != false)
            {
                sb.Append((string)de.Key);
                sb.Append(": ");
                sb.Append((string)de.Value);
                sb.Append("\r\n");
            }

            // Add an extra CRLF to indicate end of message or start of body
            sb.Append("\r\n");

            // Insert body
            if(body != null)
            {
                sb.Append(body);

                // Ensure adequate CRLFs at end of message
                if(!body.EndsWith("\r\n"))
                {
                    sb.Append("\r\n\r\n");
                }
                else if(!body.EndsWith("\r\n\r\n"))
                {
                    sb.Append("\r\n");
                }
            }

            return sb.ToString();
        }

        private string GetResponsePhrase(string code)
        {
            switch(code)
            {
                case "100":
                    return "Continue";
                case "101":
                    return "Switching Protocols";
                case "200":
                    return "OK";
                case "201":
                    return "Created";
                case "202":
                    return "Accepted";
                case "203":
                    return "Non-Authoritative Information";
                case "204":
                    return "No Content";
                case "205":
                    return "Reset Content";
                case "206":
                    return "Partial Content";
                case "300":
                    return "Multiple Choices";
                case "301":
                    return "Moved Permanently";
                case "302":
                    return "Found";
                case "303":
                    return "See Other";
                case "304":
                    return "Not Modified";
                case "305":
                    return "Use Proxy";
                case "307":
                    return "Temporary Redirect";
                case "400":
                    return "Bad Request";
                case "401":
                    return "Unauthorized";
                case "402":
                    return "Payment Required";
                case "403":
                    return "Forbidden";
                case "404":
                    return "Not Found";
                case "405":
                    return "Method Not Allowed";
                case "406":
                    return "Not Acceptable";
                case "407":
                    return "Proxy Authentication Required";
                case "408":
                    return "Request Time-out";
                case "409":
                    return "Conflict";
                case "410":
                    return "Gone";
                case "411":
                    return "Length Required";
                case "412":
                    return "Precondition Failed";
                case "413":
                    return "Request Entity Too Large";
                case "414":
                    return "Request-URI Too Large";
                case "415":
                    return "Unsupported Media Type";
                case "416":
                    return "Requested range not satisfiable";
                case "417":
                    return "Expectation Failed";
                case "500":
                    return "Internal Server Error";
                case "501":
                    return "Not Implemented";
                case "502":
                    return "Bad Gateway";
                case "503":
                    return "Service Unavailable";
                case "504":
                    return "Gateway Time-out";
                case "505":
                    return "HTTP Version not supported";
            }

            return null;
        }
        #endregion
    }

    public abstract class IHttp
    {
        public const string ACTION_SEND_RESPONSE    = "Metreos.Providers.Http.SendResponse";
        public const string EVENT_GOT_REQUEST       = "Metreos.Providers.Http.GotRequest";
        public const string EVENT_SESSION_EXPIRED   = "Metreos.Providers.Http.SessionExpired";
        public const string COOKIE_SESSION_ID   = "metreosSessionId";
        public const string QUERY_SESSION_ID    = "metreossessionid";

        public abstract class Fields
        {
            public const string METHOD          = "method";
            public const string URL             = "url";
            public const string RESPONSE_CODE   = "responseCode";
            public const string PHRASE          = "responsePhrase";
            public const string VERSION         = "httpVersion";
            public const string BODY            = "body";
            public const string SOURCE          = "source";
            public const string HOST            = "host";
            public const string HOSTNAME        = "hostname";
            public const string PORT            = "port";
            public const string REMOTE_HOST     = "remoteHost";
            public const string QUERY           = "query";
            public const string USERNAME        = "username";
            public const string PASSWORD        = "password";
            public const string REMOTE_IP       = "remoteIpAddress";
            public const string CONTENT_TYPE    = "Content-Type";
        }

        public abstract class Headers
        {
            // Standard Headers
            public const string CONTENT_LENGTH   = "Content-Length";
            public const string CONTENT_TYPE     = "Content-Type";
            public const string ENCODING         = "Transfer-Encoding";
            public const string HOST             = "Host";
            public const string ENCODING_CHUNKED = "chunked";
            public const string HEADER_COOKIE    = "Cookie";

            // Custom Header
            public const string SESSION_ID          = "Metreos-SessionID";
        }
    }
}
