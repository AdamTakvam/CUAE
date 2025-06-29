using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SendResponseTest = Metreos.TestBank.Provider.Provider.SendResponse;

namespace Metreos.FunctionalTests.SingleProvider.Http
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class SendResponse : FunctionalTestBase
    {
        private const string headerName1    = "blah";
        private const string headerValue1   = "yes?";
        private const string headerName2    = "blahism";
        private const string headerValue2   = "no!!";
        private const string headerName3    = "AcceptMuchoCookies";
        private const string headerValue3   = "CookiesYum";

        private const string otherHeaderName1   = "serverHeader1";
        private const string otherHeaderValue1  = "serverValue1";
        private const string otherHeaderName2   = "serverHeader2";
        private const string otherHeaderValue2  = "serverValue2";
        private const string otherHeaderName3   = "serverHeader3";
        private const string otherHeaderValue3  = "serverValue3";

        private const string clientBody = "This is the precise body of a http request sent by the client.";
        private const string serverBody = "This is a precise body of a http request returned by the server.";

        private bool success;
        private string reason;

        public SendResponse() : base(typeof( SendResponse ))
        {

        }

        public override bool Execute()
        {
            HttpWebRequest request = WebRequest.Create("http://" + settings.AppServerIps[0] + ':' + 8000 + "/SendResponseTest") as HttpWebRequest;

            byte[] bodyBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(clientBody);

            request.Headers[headerName1] = headerValue1;
            request.Headers[headerName2] = headerValue2;
            request.Headers[headerName3] = headerValue3;
            request.ContentType = "text/html";
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentLength = bodyBytes.Length;
            request.TransferEncoding = null;
            request.KeepAlive = false;
            request.Expect = "";

            Stream requestStream = request.GetRequestStream();
            
            requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            requestStream.Close();

            if(!WaitForSignal(SendResponseTest.script1.S_Success.FullName))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The test " + SendResponseTest.Name + " did not send an on load signal.");
                return false;
            }

            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The application detected an error in the incoming response.  The reason is: " + reason);
                return false;
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            string responseBody = reader.ReadToEnd();
            if(responseBody != serverBody)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The response body from the server did not percisely match the body sent in the application");
                return false;
            }

            if(response.Headers[otherHeaderName1] != otherHeaderValue1)
            {
                string errorLine1 = String.Format("A header was not present. The header name expected was {0} with the expected value of {1}", otherHeaderName1, otherHeaderValue1);
                string errorLine2 = String.Format("This header actually had the value of {0}.", response.Headers[otherHeaderName1]);
                log.Write(System.Diagnostics.TraceLevel.Info, errorLine1);
                log.Write(System.Diagnostics.TraceLevel.Info, errorLine2);
                return false;
            }
            
            if(response.Headers[otherHeaderName2] != otherHeaderValue2)
            {
                string errorLine1 = String.Format("A header was not present. The header name expected was {0} with the expected value of {1}", otherHeaderName2, otherHeaderValue2);
                string errorLine2 = String.Format("This header actually had value of {0}.", response.Headers[otherHeaderName2]);
                log.Write(System.Diagnostics.TraceLevel.Info, errorLine1);
                log.Write(System.Diagnostics.TraceLevel.Info, errorLine2);
                return false;
            }

            if(response.Headers[otherHeaderName1] != otherHeaderValue1)
            {
                string errorLine1 = String.Format("A header was not present. The header name expected was {0} with the expected value of {1}", otherHeaderName3, otherHeaderValue3);
                string errorLine2 = String.Format("This header actually had the value of {0}.", response.Headers[otherHeaderName3]);
                log.Write(System.Diagnostics.TraceLevel.Info, errorLine1);
                log.Write(System.Diagnostics.TraceLevel.Info, errorLine2);
                return false;
            }

            return true;

        }

        private void Success(ActionMessage im)
        {
            string successString = im["success"] as string;

            if(successString != null)
            {
                try
                {
                    success = bool.Parse(successString);

                    if(!success)
                    {
                        reason = im["reason"] as string;
                        return;
                    }
                }
                catch
                {
                    success = false;
                    reason = "Success bool unparseable.";
                    return;
                }
            }
            else
            {
                success = false;
                reason = "No success string sent from application.";
                return;
            }
        }

        public override void Initialize()
        {
            success = false;
            reason = null;
        }

        public override void Cleanup()
        {
            success = false;
            reason = null;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( SendResponseTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( SendResponseTest.script1.S_Success.FullName ,
                new FunctionalTestSignalDelegate(Success))
            };
        }
    } 
}
