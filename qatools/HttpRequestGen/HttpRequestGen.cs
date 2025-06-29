using System;
using System.Collections;
using System.Threading;
using System.Net;
using System.IO;

using Metreos.Utilities;
using ThreadPool = Metreos.Utilities.ThreadPool;
namespace HttpRequestGen
{
	/// <summary>
	/// Summary description for HTTPPoller.
	/// </summary>
    public class HttpRequestGen : IDisposable
    {
        public enum RequestType
        {
            GET,
            POST
        }

        public delegate void Action (bool success);
        public delegate void Response (bool success, string body);
        public delegate string Event();
        private delegate void GetRequestDelegate();
        
        public event Action MadeRequest;
        public event Response GotResponse;
        public event Event RequestUrl;

        private GetRequestDelegate getRequestMethod ;
        private ThreadPool pool;
        private volatile bool stop;
        private RequestType request;
        private int rateInMs;

        public HttpRequestGen()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 24;

            stop = true;
            pool = new ThreadPool(5 , 500, this.GetType().FullName ) ;
            pool.Priority = ThreadPriority.Normal;
            pool.NewThreadTrigger = 400;

            getRequestMethod = new GetRequestDelegate(Get);
        }

        public void Start (RequestType request, int rateInMs)
        {   
            if(!stop)   return;

            this.request = request;
            this.rateInMs = rateInMs;

            stop = false;

            Thread starterThread = new Thread(new ThreadStart(Run));
            starterThread.Start();
            starterThread.IsBackground = true;
        }

        private void Run()
        {
            //pool.Started += new ThreadPoolDelegate(pool_Started);
            pool.Start();
            Console.WriteLine("ThreadPool is initializing...");
            Thread.Sleep(4000);
            while (!stop)
            {    
                if (RequestType.GET == request)
                {
                    pool.PostRequest(getRequestMethod, null);
                    Thread.Sleep(rateInMs);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false, "POST not implemented");
                }
            }
        }
        private void Get()
        {
            if (RequestUrl == null)
            {
                if (MadeRequest != null) MadeRequest(false);
                return;
            }
           
            string urlString = RequestUrl();

            HttpWebRequest request = null;
            try
            {
                request = WebRequest.Create(urlString) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = false;
                request.ContentType = "text/html";
                request.Method = "GET";
    
                if (MadeRequest != null) MadeRequest(true);
            }
            catch(Exception e)
            {
                if (MadeRequest != null) MadeRequest(false);
                return;
            }

            HttpWebResponse response    = null;
            Stream stream               = null;
            StreamReader reader         = null;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("hh:mm:ss:fff"));
                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                reader = new StreamReader(stream);
                string newText = reader.ReadToEnd();
                stream.Close();
                reader.Close();
                response.Close();

                GotResponse(true, newText);
            }
            catch
            {
                GotResponse(false, null);

                if (response != null) response.Close();
                if (stream != null)   stream.Close();
                if (reader != null)   reader.Close();
            }
        }

        public void Stop()
        {
            stop = true;
            if(pool.IsStarted) pool.StopAndWait(60000);

        }

        public void Dispose ()

        {
            stop = true;
            if(pool.IsStarted) pool.StopAndWait(60000);
            pool.Close();
        }

        private void pool_Started()
        {
            Console.WriteLine("Pool started");
        }
    }
}
