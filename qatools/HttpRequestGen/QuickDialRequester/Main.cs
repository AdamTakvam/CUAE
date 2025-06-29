using System;
using System.IO;
using System.Xml.Serialization;

namespace QuickDialRequester
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class QuickDialRequester
	{
        private static int rateMs;

        private static int totalRequests;
        private static int failedRequests;
        private static int succeededRequests;

        private static int totalResponses;
        private static int failedResponses;
        private static int succeededResponses;

        private static test testDefinition;
        private static int pointer = 0;

        private static string appServerIp;

        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length < 3)
            {
                System.Console.WriteLine("First param is request rate in ms, Second param is path to test definition file, 3rd is application server IP");
                return;
            }
       
            rateMs = int.Parse(args[0]);

            testDefinition = LoadDefiniton(args[1]);

            appServerIp = args[2];

            HttpRequestGen.HttpRequestGen gen = new HttpRequestGen.HttpRequestGen();
            gen.GotResponse += new HttpRequestGen.HttpRequestGen.Action(GotResponse);
            gen.MadeRequest += new HttpRequestGen.HttpRequestGen.Action(MadeRequest);
            gen.RequestUrl += new HttpRequestGen.HttpRequestGen.Event(RequestUrl);
            gen.Start(HttpRequestGen.HttpRequestGen.RequestType.GET, rateMs);

            while(true)
            {
                System.Console.WriteLine("Hit 'q' to stop test, or hit enter to see current stats");
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
                }
            }
        }

        private static test LoadDefiniton(string path)
        {
            try
            {
                XmlSerializer seri = new XmlSerializer(typeof(test));
                FileStream stream = new FileStream(path, FileMode.Open);
                return seri.Deserialize(stream) as test;
            }
            catch 
            {
                return null;
            }
        }

        private static void MadeRequest(bool success)
        {
            totalRequests++;
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

        private static void GotResponse(bool success)
        {
            totalResponses++;
            if (success)
            {
                succeededResponses++;
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
            pair pair = testDefinition.pairs[pointer++ % testDefinition.pairs.Length - 1];

            string url = "http://" + appServerIp + ":8000/quickdial?to=" + pair.to + "&confereeTo=" + pair.confereeTo;

            return url;
        }
	}
}
