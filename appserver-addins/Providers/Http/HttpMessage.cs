using System;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps;

namespace Metreos.Providers.Http
{
    public sealed class HttpMessage : Loggable
    {
        public const string Name    = "HttpMessage";

        // HTTP message type
        public enum Type { Request, Response, Unspecified }

        public Type type;

        private string stackListenInterface;
        private int stackListenPort;

		// Apache module data
		public string uniqueId;
		public string remoteHost;

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

		// Apache generated UUID?
		private bool apacheUuid;
		public bool ApacheUuid { get { return apacheUuid; } }

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
                    headers.Add(IHttp.Headers.CONTENT_LENGTH, body.Length.ToString());
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

        public HttpMessage() : this(TraceLevel.Warning, "", 0)
        {}

        public HttpMessage(string stackListenInterface, int stackListenPort) : 
            this(TraceLevel.Info, stackListenInterface, stackListenPort)
        {}

        public HttpMessage(TraceLevel level, string stackListenInterface, int stackListenPort) : base(level, Name)
        {
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
			uniqueId = null;
			remoteHost = null;
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
			apacheUuid = false;
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
            
            if(lineBits.Length != 3)
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
                    chunkSize--;  // adjust for zero-based indexing
                }
                catch(Exception)
                {
                    log.Write(TraceLevel.Warning, 
                        "Invalid chunked message body:\n" + rawBody);
                    parseError = true;
                    return false;
                }
                
                p = q+2;  // walk

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
                string userPassEncoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userPass));
                sb.Append("Authorization: Basic ");
                sb.Append(userPassEncoded);
                sb.Append("\r\n");                
            }

            // Insert Host
            if(type == Type.Request)
            {
                sb.Append("Host: ");
                sb.Append(requestUri.Host);
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

        #region InternalMessage Conversions

        public HttpMessage(InternalMessage im, string sessionId)
            : this(TraceLevel.Error, "", 0)
        {			
            Assertion.Check(im != null, "Cannot create HTTP message from null InternalMessage");

			// Retrieve unique id to support Apache module
			this.uniqueId = im.RoutingGuid;

            if(sessionId != null)
            {
                this.headers.Add(IHttp.Headers.SESSION_ID, sessionId);
            }

            im.RemoveField(IApp.NAME_LOOP_INDEX);

            string token = im[IHttp.Fields.METHOD] as string;
            if(token != null)
            {
                this.type = HttpMessage.Type.Request;
                this.requestMethod = token;
                im.RemoveField(IHttp.Fields.METHOD);
            }

            token = im[IHttp.Fields.URL] as string;
            if(token != null)
            {
                token = Url.FormatUri(token);
                this.requestUri = new Uri(token);
                im.RemoveField(IHttp.Fields.URL);
            }

            token = im[IHttp.Fields.RESPONSE_CODE] as string;
            if(token != null)
            {
                this.type = HttpMessage.Type.Response;
                this.responseCode = token;
                im.RemoveField(IHttp.Fields.RESPONSE_CODE);
            }

            token = im[IHttp.Fields.PHRASE] as string;
            if(token != null)
            {
                this.responsePhrase = token;
                im.RemoveField(IHttp.Fields.PHRASE);
            }

            token = im[IHttp.Fields.VERSION] as string;
            if(token != null)
            {
                this.version = token;
                im.RemoveField(IHttp.Fields.VERSION);
            }

            token = im[IHttp.Fields.BODY] as string;
            if(token != null)
            {
                this.Body = token;
                im.RemoveField(IHttp.Fields.BODY);
            }

            token = im[IHttp.Fields.USERNAME] as string;
            if(token != null)
            {
                this.username = token;
                im.RemoveField(IHttp.Fields.USERNAME);
            }

            token = im[IHttp.Fields.PASSWORD] as string;
            if(token != null)
            {
                this.password = token;
                im.RemoveField(IHttp.Fields.PASSWORD);
            }

            // Make sure we got something reasonable
            if(this.type == Type.Unspecified)
            {
                throw new ArgumentException("Could not create an HTTP message from this InternalMessage");
            }

            // Insert everything else as miscellaneous headers
            ArrayList fields = im.Fields;  // Get it once, there's a lot of overhead here
            if(fields != null)
            {
                for(int i = 0; i < fields.Count; i++)
                {
                    Field field = (Field) fields[i];
                    this.headers.Add(field.Name, field.Value);
                }
            }
        }

        /// <summary>
        ///     Converts an HTTP Message to an internal message.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public InternalMessage PopulateInternalMessage(InternalMessage msg)
        {
            Assertion.Check(msg != null, "Cannot populate null message");

            if(type == Type.Request)
            {
                // Grab the query string and remove the leading '?'
                string query = requestUri.Query.TrimStart('?');

                msg.AddField(IHttp.Fields.URL, requestUri.AbsolutePath);
                msg.AddField(IHttp.Fields.HOST, requestUri.Authority);
                msg.AddField(IHttp.Fields.HOSTNAME, requestUri.Host);
                msg.AddField(IHttp.Fields.PORT, requestUri.Port.ToString());
                msg.AddField(IHttp.Fields.QUERY, query);
                msg.AddField(IHttp.Fields.METHOD, requestMethod);
            }
            else if(type == Type.Response)
            {
                msg.AddField(IHttp.Fields.RESPONSE_CODE, responseCode);
                msg.AddField(IHttp.Fields.PHRASE, responsePhrase);
            }
            else
            {
                return null;
            }
            
            // Insert common fields
            if(username != null)
            {
                msg.AddField(IHttp.Fields.USERNAME, username);
                msg.AddField(IHttp.Fields.PASSWORD, password);
            }

            msg.AddField(IHttp.Fields.VERSION, version);

            // See what the situation is with the body
            if(HasBody)
            {
                string cLength = (string) headers[IHttp.Headers.CONTENT_LENGTH];
                if(cLength != null)
                {
                    msg.AddField(IHttp.Fields.BODY, Body);
                }
                else
                {
                    log.Write(TraceLevel.Warning, "Received badly formatted HTTP message.\n" +
                        "Content was present, but no content length was specified.\n" +
                        "Content will not be forwarded to application.");
                }
            }
            else
            {
                msg.AddField(IHttp.Fields.BODY, "");
            }

            // Put in the rest of the headers
            IDictionaryEnumerator de = headers.GetEnumerator();
            while(de.MoveNext() != false)
            {
                string headerName = (string)de.Key;
                string headerValue = (string)de.Value;

                if(0 == String.Compare(headerName, IHttp.Fields.HOST, true) && type == Type.Request)
                {         
                    // In this case, the 'host' field was already populated higher up by extracting
                    // 'host' from the request Uri.  We will skip adding the header value for host,
                    // because doing so creates an event message with two 'host' parameter
                    // values of the same value.

                    // Since we flatten all special case values and headers into internal message fields, 
                    // nothing is lost by checking for 'host', and discarding it from the headers collection 
                    // if we have already populated our 'host' field in the internal message from the 
                    // special case 'host' Apache value.
                    continue;
                }
                msg.AddField(headerName, headerValue);
            }

            // Put in cookies
            de = cookies.GetEnumerator();
            while(de.MoveNext() != false)
            {
                string headerName = (string)de.Key;
                string headerValue = (string)de.Value;

                msg.AddField(headerName, headerValue);
            }

            return msg;
        }

        #endregion

		#region Flatmap loader
		public bool LoadFromFlatmap(FlatmapList message)
		{
			string host = null;
			for (int i=0; i<message.Count; i++)
			{
				// Key values are defined in FlatmapIpcClientWrapper.h inside c-wrapper project
				switch(message.GetAt(i).key)
				{
					case 100:		// UUID
						uniqueId = message.GetAt(i).dataValue.ToString();
						break;

					case 101:		// VERSION
						version = message.GetAt(i).dataValue.ToString();
						version = version.Trim('H', 'T', 'P', '/', ' ');
						break;

					case 102:		// METHOD
						requestMethod = message.GetAt(i).dataValue.ToString();
						break;

					case 103:		// URI
						relativeUri = message.GetAt(i).dataValue.ToString();
						break;

					case 104:		// QUERY PARAMETERS
						queryParams = message.GetAt(i).dataValue.ToString();
						break;

					case 105:		// HAS BODY, ignore it, check it at the end by actual body data.
						break;

					case 106:		// BODY
						body = message.GetAt(i).dataValue.ToString();
						break;

					case 107:		// HEADER
						string skey = null;
						string sval = null;
						string headerLine = message.GetAt(i).dataValue.ToString();
						string[] headerBits = headerLine.Split(new char[] {':'}, 2);
						skey = headerBits[0].Trim();
						sval = headerBits[1].Trim();

                        if (string.Compare(skey, IHttp.Headers.HEADER_COOKIE, true) == 0)
                        {
                            if(sval != null && sval != String.Empty)
                            {
                                string[] cookieList = sval.Trim().Split(';');
                                string cookieName;
                                string cookieValue;

                                if(cookieList != null)
                                {
                                    foreach(string cookie in cookieList)
                                    {
                                        string[] cookieBits = cookie.Split('=');
                                        if(cookieBits != null && cookieBits.Length == 2)
                                        {
                                            cookieName = cookieBits[0].Trim();
                                            cookieValue = cookieBits[1].Trim();
                                            cookies.Add(cookieName, cookieValue);
                                        }
                                    }
                                }
                            }
                        }
						else
							headers.Add(skey, sval);
						break;

					case 108:		// COOKIE
						break;

					case 109:		// CONTENT TYPE
						break;

					case 110:		// CONTENT LENGTH, ignore it, check it at the end by actual body data.
						break;

					case 111:		// USER NAME
						username = message.GetAt(i).dataValue.ToString();
						break;

					case 112:		// PASSWORD
						password = message.GetAt(i).dataValue.ToString();
						break;

					case 113:		// RESPONSE CODE ??
						break;

					case 114:		// RESPONSE PHRASE ??
						break;

					case 115:		// HOST
						host = message.GetAt(i).dataValue.ToString();
						break;

					case 116:		// REMOTE HOST
						remoteHost = message.GetAt(i).dataValue.ToString();
						break;

					case 117:		// Apache UUID
						if (message.GetAt(i).dataValue.ToString() == "1")
							apacheUuid = true;
						else
							apacheUuid = false;
						break;

					default:
						break;
				}
			}

            // Apache sends 'host' as a special value, but it is also contained in the headers.
            // We check in both locations before giving up on this HTTP request
			if (uniqueId == null || (host == null && remoteHost == null && headers[IHttp.Fields.HOST] as string == null))
				return false;

			type = Type.Request;		// can it be response??

			// Under provider, relativeUri includes query parameters
			if (queryParams != null && queryParams.Length > 0)
				relativeUri = relativeUri + "?" + queryParams;

			requestUri = new Uri("http://" + host + relativeUri);

			if (body != null && body.Length > 0)
			{
				hasBody = true;
				contentLength = body.Length;
			}

			return true;
		}

		public bool PopulateFlatmap(ref FlatmapList message)
		{
			if (uniqueId == null)
				return false;

			// UUID
			message.Add(100, uniqueId);

			// Content Type ???

			if (body != null)
			{
				// Body
                // JDL, localization, force UTF8 encoding
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(body);
				message.Add(106, buffer);

				// Content Length
                // JDL, localization, force UTF8 encoding
                message.Add(110, buffer.Length);
			}
			else
			{
				// Content Length
				message.Add(110, 0);
			}

			// Headers
			IDictionaryEnumerator de = headers.GetEnumerator();
			while(de.MoveNext() != false)
			{
				string s = (string)de.Key + ":" + (string)de.Value;
				message.Add(107, s);
			}

			// Cookies
			de = cookies.GetEnumerator();
			while(de.MoveNext() != false)
			{				
				string s = (string)de.Key + ":" + (string)de.Value;
				message.Add(108, s);
			}

			// User Name
			if (username != null)
				message.Add(111, username);

			// Password
			if (password != null)
				message.Add(112, password);

			// Response Code
			if (responseCode != null)
				message.Add(113, responseCode);

			// Response Phrase
			if (responsePhrase != null)
				message.Add(114, responsePhrase);

			return true;
		}
		#endregion
    }
}
