using System;
using HttpRequestGen;
namespace LdapQuery
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Class1
    {
        private static string baseUrl;
        private static int rateMs;

        private static int totalRequests;
        private static int failedRequests;
        private static int succeededRequests;

        private static int totalResponses;
        private static int failedResponses;
        private static int succeededResponses;

        private static int startTo;
        private static int callRange;

        private static int successCalls;
        private static int failedCalls;

        private static int currentBhca;
        private static int currentBhcc;
        private static DateTime startTime;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string appServerIp = args[0];
            int bhca = int.Parse(args[1]);

            baseUrl = String.Format("http://{0}:8000/EM/LNH", appServerIp);

            HttpRequestGen.HttpRequestGen gen = new HttpRequestGen.HttpRequestGen();
            gen.GotResponse += new HttpRequestGen.HttpRequestGen.Response(GotResponse);
            gen.MadeRequest += new HttpRequestGen.HttpRequestGen.Action(MadeRequest);
            gen.RequestUrl += new HttpRequestGen.HttpRequestGen.Event(RequestUrl);
            startTime = DateTime.Now;
            gen.Start(HttpRequestGen.HttpRequestGen.RequestType.GET, (int)((3600f / (float)bhca) * 1000f));

            while(true)
            {
                string response = System.Console.ReadLine();
                if(response.ToLower() == "q")
                {
                    gen.Stop();
                    gen.Dispose();
                    System.Console.WriteLine("{0} total requests", totalRequests);
                    System.Console.WriteLine("{0} succeeded requests", succeededRequests);
                    System.Console.WriteLine("{0} failed requests", failedRequests);
                    System.Console.WriteLine("{0} total responses", totalResponses);
                    System.Console.WriteLine("{0} succeeded responses", succeededResponses);
                    System.Console.WriteLine("{0} failed responses", failedResponses);
                    System.Console.WriteLine("{0} success calls", successCalls);
                    System.Console.WriteLine("{0} failed calls", failedCalls);
                    System.Console.WriteLine("{0} BHCA", currentBhca);
                    System.Console.WriteLine("{0} BHCC", currentBhcc);
  
                    break;
                }
                else
                {
                    System.Console.WriteLine("{0} total requests", totalRequests);
                    System.Console.WriteLine("{0} succeeded requests", succeededRequests);
                    System.Console.WriteLine("{0} failed requests", failedRequests);
                    System.Console.WriteLine("{0} total responses", totalResponses);
                    System.Console.WriteLine("{0} succeeded responses", succeededResponses);
                    System.Console.WriteLine("{0} failed responses", failedResponses);
                    System.Console.WriteLine("{0} success calls", successCalls);
                    System.Console.WriteLine("{0} failed calls", failedCalls);
                    System.Console.WriteLine("{0} BHCA", currentBhca);
                    System.Console.WriteLine("{0} BHCC", currentBhcc);
                }
            }
        }

        private static void MadeRequest(bool success)
        {
            totalRequests++;

            // Compute BHCA
            TimeSpan testTime = DateTime.Now.Subtract(startTime);
            currentBhca = (totalRequests * 3600) / (int)testTime.TotalSeconds;

            if (success)
            {
                succeededRequests++;
            }
            else
            {
                failedRequests++;
                System.Console.WriteLine("Failed request {0}", failedRequests);
            }

            if (succeededRequests % 100 == 0)
            {
                System.Console.WriteLine("{0} succeeded requests", succeededRequests);
            }
        }

        private static void GotResponse(bool success, string body)
        {
            totalResponses++;
            if (success)
            {
                succeededResponses++;
                if(body == "knittte knittte")
                {
                    // Compute BHCC
                    TimeSpan testTime = DateTime.Now.Subtract(startTime);
                    currentBhcc = (succeededResponses * 3600) / (int)testTime.TotalSeconds;

                    successCalls++;
                }
                else
                {
                    failedCalls++;
                    System.Console.WriteLine("The request failed :(");
                    System.Console.WriteLine("Failed call {0}", failedCalls);
                }
            }
            else
            {
                failedResponses++;
                System.Console.WriteLine("Failed response {0}", failedResponses);
            }

            if (succeededResponses % 100 == 0)
            {
                System.Console.WriteLine("{0} succeeded responses", succeededResponses);
            }
            
        }

        private static int counter = 0;
        private static string RequestUrl()
        {
            return baseUrl;
        }
    }
}
