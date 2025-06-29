using System;
using System.IO;
using System.Web;
using System.Net;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ContinuousSendResponseTest = Metreos.TestBank.Provider.Provider.ContinuousSendResponse;

namespace Metreos.FunctionalTests.SingleProvider.Http
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class ContinuousSendResponse : FunctionalTestBase
    {
        private const string rateRequests = "rate";
        private const string checkRequestBodyStr = "checkRequestBody";
        private const string checkResponseBodyStr = "checkResponseBody";
        private const string incomingBodyStr = "incomingBody";
        private const string outgoingBodyStr = "outgoingBody";
        private const int defaultRate = 1000;
        private int rate;
        private bool checkRequestBody;
        private bool checkResponseBody;
        private string incomingBody;
        private string outgoingBody;
        private volatile bool end;
        private int countFailed;
        private int count;

        public ContinuousSendResponse() : base(typeof( ContinuousSendResponse ))
        {

        }

        public override bool Execute()
        {         
            rate = ParseInt(input[rateRequests] as string, defaultRate, rateRequests);
            checkRequestBody = (bool) input[checkRequestBodyStr];
            checkResponseBody = (bool) input[checkResponseBodyStr];
            incomingBody = ParseString(input[incomingBodyStr] as string, String.Empty, incomingBodyStr);
            outgoingBody = ParseString(input[outgoingBodyStr] as string, String.Empty, outgoingBodyStr);
          
            updateAppConfig(
                ContinuousSendResponseTest.Name, 
                IConfig.ComponentType.Application,
                "checkBody", 
                checkRequestBody, 
                "boringDesc", 
                IConfig.StandardFormat.String);

            Thread.Sleep(1000);

            updateAppConfig(
                ContinuousSendResponseTest.Name,
                IConfig.ComponentType.Application,
                "outgoingBody", 
                outgoingBody, 
                "boringDesc", 
                IConfig.StandardFormat.String);

            Thread.Sleep(1000);

            updateAppConfig(
                ContinuousSendResponseTest.Name,
                IConfig.ComponentType.Application,
                "expectedIncomingBody", 
                incomingBody, 
                "boringDesc", 
                IConfig.StandardFormat.String);

            Thread.Sleep(1000);

            while(!end)
            {
                HttpWebRequest request = WebRequest.Create("http://" + settings.AppServerIps[0] + ':' + 8000 + "/ContinuousSendResponse") as HttpWebRequest;
                
                byte[] bodyBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(incomingBody);

                request.ContentType = "text/html";
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";
                request.ContentLength = bodyBytes.Length;
                request.TransferEncoding = null;
                request.KeepAlive = false;
                request.Expect = "";

                State state = new State(request, bodyBytes, ++count);

                request.BeginGetRequestStream(new AsyncCallback(PushRequest), state);

                request.BeginGetResponse(new AsyncCallback(ReceiveResponse), state);

                Thread.Sleep(rate);
            }

            return countFailed == 0;
        }

        private void PushRequest(IAsyncResult result)
        {
            try
            {
                State state = result.AsyncState as State;
                Stream writeStream = state.request.EndGetRequestStream(result);
                writeStream.Write(state.bodyBytes, 0, state.bodyBytes.Length);
                writeStream.Close();
            }
            catch
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Send request failed.");
            }
        }

        private void ReceiveResponse(IAsyncResult result)
        {
            State state = result.AsyncState as State;
            HttpWebResponse response = state.request.EndGetResponse(result) as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();

            if(checkResponseBody)
            {
                StreamReader reader = new StreamReader(responseStream);
                string responseBody = reader.ReadToEnd();
                responseStream.Close();

                if(responseBody != outgoingBody)
                {
                    countFailed++;

                    bool success = ParseBool(response.Headers["success"], false, "Success response from server.");

                    if(!success)
                    {
                        string reason = ParseString(response.Headers["reason"], String.Empty, "Reason for failure");
                        log.Write(System.Diagnostics.TraceLevel.Info, "Request " + state.count + " failed. Reason: " + reason + ". " + countFailed + " failures.");
                    }

                    log.Write(System.Diagnostics.TraceLevel.Info, "Request " + state.count + " failed. Reason: Body in response is invalid. " + countFailed + " failures.");
                    
                    return;
                }
            }
            else
            {
                responseStream.Close();

                bool success = ParseBool(response.Headers["success"], false, "Success response from server.");

                if(!success)
                {
                    countFailed++;
                    string reason = ParseString(response.Headers["reason"], String.Empty, "Reason for failure");
                    log.Write(System.Diagnostics.TraceLevel.Info, "Request " + state.count + " failed. Reason: " + reason + ". " + countFailed + " failures.");
                }
            }   
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData rateReqs = new TestTextInputData("Rate to send requests(ms)", 
                "The rate at which requests will be sent to the application server.", rateRequests, 80);

            TestBooleanInputData checkIncomingBody = new TestBooleanInputData("Check body in request?",
                "If enabled, the test will check that the request body is valid.", checkRequestBodyStr, false);

            TestBooleanInputData checkOutgoingBody = new TestBooleanInputData("Check body in response?",
                "If enabled, the test will check that the response body is valid.", checkResponseBodyStr, false);

            TestMultiLineTextInputData incomingBody = new TestMultiLineTextInputData("Incoming body:",
                "This is the body that will be sent into the request.", incomingBodyStr, 800, String.Empty);

            TestMultiLineTextInputData outgoingBody = new TestMultiLineTextInputData("Outgoing body:",
                "This is the body that will be sent with the response.", outgoingBodyStr, 800, String.Empty);

            TestUserEvent endTest = new TestUserEvent("Stop making requests.",
                "Press to stop making requests.", "endTest", "End", new CommonTypes.AsyncUserInputCallback(EndTest));

            ArrayList inputs = new ArrayList();
            inputs.Add(rateReqs);
            inputs.Add(checkIncomingBody);
            inputs.Add(checkOutgoingBody);
            inputs.Add(incomingBody);
            inputs.Add(outgoingBody);
            inputs.Add(endTest);
            return inputs;
        }

        private bool EndTest(string name, string _value)
        {
            end = true;
            return true;
        }


        public override void Initialize()
        {
            rate = defaultRate;
            checkRequestBody = false;
            checkResponseBody = false;
            incomingBody = String.Empty;
            outgoingBody = String.Empty;
            end = false;
            countFailed = 0;
            count = 0;
        }

        public override void Cleanup()
        {
            rate = defaultRate;
            checkRequestBody = false;
            checkResponseBody = false;
            incomingBody = String.Empty;
            outgoingBody = String.Empty;
            end = false;
            countFailed = 0;
            count = 0;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( ContinuousSendResponseTest.FullName ) };
        }
    }   

    public class State
    {
        public HttpWebRequest request;
        public byte[] bodyBytes;
        public int count;

        public State(HttpWebRequest request, byte[] bodyBytes, int count)
        {
            this.request = request;
            this.bodyBytes = bodyBytes;
            this.count = count;
        }
    }
}
