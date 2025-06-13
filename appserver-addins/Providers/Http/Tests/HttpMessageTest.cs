using System;

namespace Metreos.Providers.Http.Tests
{
	/// <summary>
	/// Summary description for HttpMessageTest.
	/// </summary>
	public class HttpMessageTest
	{
		public HttpMessageTest() {}

        public void testDeserializeNoBody()
        {
            HttpMessage msg = new HttpMessage();            
            string rawHttp = requestHeaders + fixedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals("www.remotehost.com", msg.headers["Host"]);
            csUnit.Assert.False(msg.HasBody);

            msg = new HttpMessage();            
            rawHttp = responseHeaders + fixedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals("www.myname.com", msg.headers["Server"]);
            csUnit.Assert.False(msg.HasBody);
        }

        public void testDeserializeChunkedBodyWhole()
        {
            HttpMessage msg = new HttpMessage();            
            string rawHttp = responseHeaders + chunkedBodyHead + chunkedBody + chunkedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals(fixedBody, msg.Body);

            msg = new HttpMessage();
            rawHttp = requestHeaders + chunkedBodyHead + chunkedBody + chunkedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals(fixedBody, msg.Body);
        }

        public void testDeserializeFixedBodyWhole()
        {
            HttpMessage msg = new HttpMessage();            
            string rawHttp = responseHeaders + fixedBodyHead + fixedBody + fixedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals(fixedBody, msg.Body);

            msg = new HttpMessage();
            rawHttp = requestHeaders + fixedBodyHead + fixedBody + fixedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals(fixedBody, msg.Body);
        }

        public void testDeserializeNoBodyFragmented()
        {
            HttpMessage msg = new HttpMessage();
            string rawHttp = requestHeaders;
            csUnit.Assert.False(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.False(msg.HasBody);

            rawHttp = moreHeaders + fixedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.False(msg.HasBody);

            msg = new HttpMessage();
            rawHttp = responseHeaders;
            csUnit.Assert.False(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.False(msg.HasBody);

            rawHttp = moreHeaders + fixedBodyTail;
            csUnit.Assert.True(msg.Parse(rawHttp));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.False(msg.HasBody);
        }

        public void testDeserializeChunkedBodyFragmented()
        {
            HttpMessage msg = new HttpMessage();

            csUnit.Assert.False(msg.Parse(responseHeaders));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(chunkedBodyHead));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(chunkedBody1));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(chunkedBody2));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.True(msg.Parse(chunkedBody3 + chunkedBodyTail));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals(fixedBody, msg.Body);
        }

        public void testDeserializeFixedBodyFragmented()
        {
            HttpMessage msg = new HttpMessage();

            csUnit.Assert.False(msg.Parse(responseHeaders));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(fixedBodyHead));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(fixedBody1));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(fixedBody2));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.True(msg.Parse(fixedBody3 + fixedBodyTail));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals(fixedBody, msg.Body);
        }

        public void testSuperChunk()
        {
            HttpMessage msg = new HttpMessage();

            csUnit.Assert.False(msg.Parse(responseHeaders));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(chunkedBodyHead));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(chunkedBody1));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(chunkedBody2_1));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.False(msg.Parse(chunkedBody2_2));
            csUnit.Assert.False(msg.ParseError);

            csUnit.Assert.True(msg.Parse(chunkedBody3 + chunkedBodyTail));
            csUnit.Assert.False(msg.ParseError);
            csUnit.Assert.Equals(fixedBody, msg.Body);
        }

        private const string responseHeaders = @"HTTP/1.1 200 OK
Content-Type: someType
Server: www.myname.com";

        private const string requestHeaders = @"POST /myDir/myfile.html HTTP/1.1
Content-Type: someType
Host: www.remotehost.com";
        
        private const string chunkedBody = chunkedBody1 + chunkedBody2 + chunkedBody3;

        private const string chunkedBodyHead = @"
Transfer-Encoding: chunked

";
        private const string chunkedBody1 = @"064
<?xml version=""1.0"" encoding=""iso-8859-1""?><?xml version=""1.0""?>
<CiscoIPPhoneResponse>
<Response";

        private const string chunkedBody2 = chunkedBody2_1 + chunkedBody2_2;

        private const string chunkedBody2_1 = @"
064
Item Status=""0"" Data="""" URL=""http://209.1";
        
        private const string chunkedBody2_2 = @"63.143.103/test.xml""/>
<ResponseItem Status=""0"" Data="""" U";

        private const string chunkedBody3 = @"
04e
RL=""""/>
<ResponseItem Status=""0"" Data="""" URL=""""/>
</CiscoIPPhoneResponse>";

        private const string chunkedBodyTail = "\r\n\r\n0";

        private const string fixedBody = fixedBody1 + fixedBody2 + fixedBody3;

        private const string fixedBodyHead = @"
Content-Length: 273

";
        private const string fixedBody1 = @"<?xml version=""1.0"" encoding=""iso-8859-1""?><?xml version=""1.0""?>
<CiscoIPPhoneResponse>";

        private const string fixedBody2 = @"
<ResponseItem Status=""0"" Data="""" URL=""http://209.163.143.103/test.xml""/>
<ResponseItem Status=""0"" Data="""" URL=""""/>";

        private const string fixedBody3 = @"
<ResponseItem Status=""0"" Data="""" URL=""""/>
</CiscoIPPhoneResponse>";

        private const string fixedBodyTail = "\r\n\r\n";

        private const string moreHeaders = @"
User-Agent: DaffyBrowser
Metreos-SessionID: 1234987139614
MyHeader: MyValue";

	}
}
