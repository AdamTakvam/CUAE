//using System;
//
//namespace QuickDialSim
//{
//    /// <summary>
//    /// Summary description for Class1.
//    /// </summary>
//    public class QuickDialSim
//    {
//        private static HttpRequestGen.HttpRequestGen.RequestType requestType;
//        private static int startRange;
//        private static int stopRange;
//        private static int rateInMs;
//
//        private static int totalRequests;
//        private static int failedRequests;
//        private static int succeededRequests;
//
//        private static int totalResponses;
//        private static int failedResponses;
//        private static int succeededResponses;
//
//        [STAThread]
//        static void Main(string[] args)
//        {
//            if(args.Length < 3)
//            {
//                System.Console.WriteLine("First param is 'to' start range, second param is 'to' stop range, third is request rate in ms");
//                return;
//            }
//
//            rateMs = int.Parse(args[2]);
//
//            HttpRequestGen.HttpRequestGen gen = new HttpRequestGen.HttpRequestGen();
//            gen.GotResponse += new HttpRequestGen.HttpRequestGen.Action(GotResponse);
//            gen.MadeRequest += new HttpRequestGen.HttpRequestGen.Action(MadeRequest);
//            gen.RequestUrl += new HttpRequestGen.HttpRequestGen.Event(RequestUrl);
//            gen.Start(requestType, rateMs);
//
//            while(true)
//            {
//                string response = System.Console.ReadLine();
//                if(response.ToLower() == "q")
//                {
//                    gen.Stop();
//                    gen.Dispose();
//                    System.Console.WriteLine("{0} total requests", totalRequests);
//                    System.Console.WriteLine("{0} succeeded requests", succeededRequests);
//                    System.Console.WriteLine("{0} failed requests", failedRequests);
//                    System.Console.WriteLine("{0} total responses", totalResponses);
//                    System.Console.WriteLine("{0} succeeded responses", succeededResponses);
//                    System.Console.WriteLine("{0} failed responses", failedResponses);
//                    break;
//                }
//                else
//                {
//                    System.Console.WriteLine("{0} total requests", totalRequests);
//                    System.Console.WriteLine("{0} succeeded requests", succeededRequests);
//                    System.Console.WriteLine("{0} failed requests", failedRequests);
//                    System.Console.WriteLine("{0} total responses", totalResponses);
//                    System.Console.WriteLine("{0} succeeded responses", succeededResponses);
//                    System.Console.WriteLine("{0} failed responses", failedResponses);
//                }
//            }
//        }
//
//        private static void MadeRequest(bool success)
//        {
//            totalRequests++;
//            if (success)
//            {
//                succeededRequests++;
//            }
//            else
//            {
//                failedRequests++;
//                System.Console.WriteLine("Failed request {0}", failedRequests);
//            }
//
//            if (succeededRequests % 100 == 0)
//            {
//                System.Console.WriteLine("{0} succeeded requests", succeededRequests);
//            }
//        }
//
//        private static void GotResponse(bool success)
//        {
//            totalResponses++;
//            if (success)
//            {
//                succeededResponses++;
//            }
//            else
//            {
//                failedResponses++;
//                System.Console.WriteLine("Failed response {0}", failedResponses);
//            }
//
//            if (succeededResponses % 100  == 0)
//            {
//                System.Console.WriteLine("{0} succeeded responses", succeededResponses);
//            }
//            
//        }
//
//        private static string RequestUrl()
//        {
//            return url;
//        }
//    }
//}
