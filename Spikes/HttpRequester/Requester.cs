using System;
using System.Net;
using System.Threading;
using System.Collections;

namespace HttpRequester
{
	class Requester
	{
        private abstract class Defaults
        {
            public const string Uri = "http://127.0.0.1:80/";
            public const long NumReqs = 10;
			public const int ReqsAtATime = 1;
			public const int ReqRate = 0;
        }

        private Uri uri;
        private long numReqsDesired = 0;
        private long numReqsComplete = 0;
        private long numReqsErrored = 0;
        private long numTransErrors = 0;
        private long totalReqTime = 0;

        private long startTime;

		public Requester(string uriStr, long numReqs, int interval)
        {
			this.uriStr = uriStr;
			this.numReqs = numReqs;
			this.interval = interval;
			// Console.WriteLine( "request interval = {0}", interval );
        }

		private string uriStr;

		private long numReqs;

		private int interval;

        public void Go()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 24;

            try { uri = new Uri(uriStr); }
            catch
            {
                Console.WriteLine("Error: Invalid address: " + uriStr);
                PrintHelp();
                return;
            }

            numReqsDesired = numReqs;

            Thread thread = new Thread(new ThreadStart(SendRequests));
            thread.IsBackground = true;
            thread.Start();

            Console.WriteLine("Press \"q\" to quit");
            while(!thread.Join(500))
            {
				if(Console.ReadLine() == "q")
				{
					Console.Write("Calculating metrics...");
					thread.Abort();
					while(!thread.Join(1000))
						Console.Write(".");
					break;
				}

                PrintResults();
            }
			PrintResults();
        }

        private void SendRequests()
        {
			startTime = Now();

            try
			{
				for(long i=0; i<numReqsDesired; i++)
				{
					long t0 = Now();
					if (!SendRequest())
						break;
					long delay = Now() - t0;

					int sinterval = (int) Math.Max( 1, interval - delay );
					// Console.WriteLine( "delay {0} sleeping for {1}", delay, sinterval );
					Thread.Sleep( sinterval );
				}
            }
            catch(ThreadAbortException)
            {
                // ignore
            }
        }

		public static long Now()
		{
			// DateTime.Now.Ticks returns 100-nanosecond precision
			return DateTime.Now.Ticks / 10000;
		}

        private bool SendRequest()
        {
            long reqStartTime = Now();

			HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;

			HttpWebResponse resp = null;
            try
			{
				resp = req.GetResponse() as HttpWebResponse;
			}
            catch(ThreadAbortException)
            {
				Console.WriteLine("HTTP stopped");
				return false;
            }
            catch(WebException we)
            {
                resp = we.Response as HttpWebResponse;
            }
            catch(Exception e) 
            {
                Console.WriteLine("HTTP Error: " + e.Message);
                numTransErrors++;
                return false;
            }

            if(resp == null || resp.StatusCode != HttpStatusCode.OK)
                numReqsErrored++;

            if(resp != null)
                resp.Close();

            numReqsComplete++;
			long reqTime = Now() - reqStartTime;

            totalReqTime += reqTime;

			// Console.WriteLine( "req done in {0}: {1}/{2}", numReqsComplete, numReqsComplete, numReqsErrored );
			return true;
        }

        private void PrintResults()
        {
            long totalTime = Now() - startTime;

			if (totalTime > 0 && numReqsComplete > 0)
			{
				double avgReqsPerSec = numReqsComplete * 1000.0 / totalTime;
				double avgMs = (double)totalReqTime / numReqsComplete;
				int errorPercentage = (int) (numReqsErrored * 100 / numReqsComplete);
				int bhca = (int) (avgReqsPerSec * 3600);

				Console.WriteLine();
				Console.WriteLine("Number of completed requests: {0}", numReqsComplete);
				Console.WriteLine("Number of error responses: {0} ({1}%)", numReqsErrored, errorPercentage);
				Console.WriteLine("Transport errors (no response): {0}", numTransErrors);
				Console.WriteLine("Avg request time: {0} ms", avgMs);
				Console.WriteLine("Avg requests per second: {0}", avgReqsPerSec);
				Console.WriteLine("BHCA: " + bhca);
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("Number of completed requests: {0}", numReqsComplete);
			}
        }

		[STAThread]
		static void Main(string[] args)
		{
            string uri = Defaults.Uri;
            long numReqs = Defaults.NumReqs;
			int reqsAtATime = Defaults.ReqsAtATime;
			int reqRate = Defaults.ReqRate;

            if(args.Length > 0 && args[0] != null)
            {
                if(args[0] == "?" || args[0] == "-h" || args[0] == "--help")
                {
                    PrintHelp();
                    return;
                }

                uri = args[0];
            }
            
            if(args.Length > 1 && args[1] != null)
            {
                numReqs = Convert.ToInt64(args[1]);
                if(numReqs == 0)
                {
                    PrintHelp();
                    return;
                }
				if (numReqs < 0)
					numReqs = long.MaxValue;
            }

			if(args.Length > 2 && args[2] != null)
			{
				reqsAtATime = Convert.ToInt32(args[2]);
				if(reqsAtATime <= 0 || reqsAtATime > 32)
				{
					PrintHelp();
					return;
				}
			}

			if(args.Length > 3 && args[3] != null)
			{
				// rate is reqs per hour, like bhca
				reqRate = Convert.ToInt32(args[3]);
				if(reqRate < 0 || reqRate > 3600000) // 3600000 is 1000 per second
				{
					PrintHelp();
					return;
				}
			}

			if(args.Length > 4)
			{
				PrintHelp();
				return;
			}

			Thread[] t = new Thread[reqsAtATime];
			for (int i = 0; i < reqsAtATime; i++)
			{
				Requester r = new Requester(uri, numReqs/reqsAtATime, reqRate > 0 ? 3600000 / reqRate : 0 );
				t[i] = new Thread(new ThreadStart(r.Go));
			}

			for (int i = 0; i < reqsAtATime; i++)
			{
				t[i].Start();
			}

			for (int i = 0; i < reqsAtATime; i++)
			{
				t[i].Join();
			}
		}

        public static void PrintHelp()
        {
            Console.WriteLine("Usage: HttpRequester [<uri> [<numReqs> [<reqsAtATime> [<reqRate>]]]]");
            Console.WriteLine();
            Console.WriteLine("Parameters:");
            Console.WriteLine("\turi\t\tHTTP URI (default: {0})", Defaults.Uri);
			Console.WriteLine("\tnumReqs\tNumber of requests to send. -1 = infinite. (default: {0})", Defaults.NumReqs);
			Console.WriteLine("\treqsAtATime\tNumber of threads sending requests. max 32. (default: {0})", Defaults.ReqsAtATime);
			Console.WriteLine("\treqRate\tRequests to send per hour. 0 = as fast as possible (default: {0})", Defaults.ReqRate);
		}
	}
}
