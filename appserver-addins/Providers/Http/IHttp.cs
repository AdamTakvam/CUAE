using System;

namespace Metreos.Providers.Http
{
	public abstract class IHttp
	{
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
