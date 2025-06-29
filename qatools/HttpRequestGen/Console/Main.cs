using System;

namespace Conole
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Console
	{
        private static HttpRequestGen.HttpRequestGen.RequestType requestType;
        private static string url;
        private static int rateMs;

        private static int totalRequests;
        private static int failedRequests;
        private static int succeededRequests;

        private static int totalResponses;
        private static int failedResponses;
        private static int succeededResponses;

        private static DateTime startTime;
        private static int currentBhca;
        private static int currentBhcc;
        private static int start = 1000;

        private static object locker = new object();

        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length < 3)
            {
                System.Console.WriteLine("First param is GET or POST, second is URL, third is request rate in ms");
                return;
            }

            requestType = (HttpRequestGen.HttpRequestGen.RequestType) Enum.Parse(typeof(HttpRequestGen.HttpRequestGen.RequestType), args[0], true);

            url = args[1];

            rateMs = int.Parse(args[2]);

            HttpRequestGen.HttpRequestGen gen = new HttpRequestGen.HttpRequestGen();
            gen.GotResponse += new HttpRequestGen.HttpRequestGen.Response(GotResponse);
            gen.MadeRequest += new HttpRequestGen.HttpRequestGen.Action(MadeRequest);
            gen.RequestUrl += new HttpRequestGen.HttpRequestGen.Event(RequestUrl);
            gen.Start(requestType, rateMs);
            startTime = DateTime.Now;

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
                // Compute BHCC
                TimeSpan testTime = DateTime.Now.Subtract(startTime);
                currentBhcc = (succeededResponses * 3600) / (int)testTime.TotalSeconds;
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

        private static string RequestUrl()
        {
            lock(locker)
            {
                return url + "?to=" + start++;
            }
        }
    }
}
