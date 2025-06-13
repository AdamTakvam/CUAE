using System;

using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Providers.Http;

namespace Metreos.Providers.Http.Tests
{
    public class HttpProviderTest
    {
        private HttpProvider httpProvider;
        
        public HttpProviderTest()
        {}
 
        [csUnit.FixtureSetUp]
        public void FixtureSetUp()
        {
            httpProvider = new HttpProvider();
        }

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            httpProvider.Cleanup();
            httpProvider = null;
        }

        [csUnit.Test]
        public void testCreateInternalMessageFromHttpMessage()
        {
            string rawHttpMsg = @"GET http//www.metreos.com/TestPage?someKey=someValue&someKey2=someValue2 HTTP/1.1\r\n";
            
            HttpMessage httpMessage = new HttpMessage();
            httpMessage.Parse(rawHttpMsg);
            
            InternalMessage im = httpProvider.CreateInternalMessageFromHttpMessage(httpMessage);

            string method;

            csUnit.Assert.NotNull(im);
            csUnit.Assert.True(im.GetFieldByName(HttpProvider.FIELD_METHOD, out method));
            csUnit.Assert.Equals("GET", method);
        }

        [csUnit.Test]
        public void testCreateHttpMessageFromInternalMessage()
        {
        }
    }
}
