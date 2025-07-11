using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Web;

namespace HttpListenerSpike
{
    public delegate void delReceiveWebRequest(HttpListenerContext Context);

    public class HttpServer
    {

        protected HttpListener Listener;
        protected bool IsStarted = false;

        public event delReceiveWebRequest ReceiveWebRequest;

        public HttpServer()
        { }

        /// <summary>
        /// Starts the Web Service
        /// </summary>
        /// <param name="UrlBase">
        /// A Uri that acts as the base that the server is listening on.
        /// Format should be: http://127.0.0.1:8080/ or http://127.0.0.1:8080/somevirtual/
        /// Note: the trailing backslash is required! For more info see the
        /// HttpListener.Prefixes property on MSDN.
        /// </param>
        public void Start(string UrlBase)
        {
            // *** Already running - just leave it in place
            if(this.IsStarted)
                return;

            if(this.Listener == null)
            {
                this.Listener = new HttpListener();
            }

            this.Listener.Prefixes.Add(UrlBase);
            this.IsStarted = true;
            this.Listener.Start();

            IAsyncResult result = this.Listener.BeginGetContext(new AsyncCallback(WebRequestCallback), this.Listener);
        }

        /// <summary>
        /// Shut down the Web Service
        /// </summary>
        public void Stop()
        {
            if(Listener != null)
            {
                this.Listener.Close();
                this.Listener = null;
                this.IsStarted = false;
            }
        }

        protected void WebRequestCallback(IAsyncResult result)
        {
            if(this.Listener == null)
                return;

            // Get out the context object
            HttpListenerContext context = this.Listener.EndGetContext(result);

            // *** Immediately set up the next context
            this.Listener.BeginGetContext(new AsyncCallback(WebRequestCallback), this.Listener);

            if(this.ReceiveWebRequest != null)
                this.ReceiveWebRequest(context);

            this.ProcessRequest(context);
        }


        /// <summary>
        /// Overridable method that can be used to implement a custom hnandler
        /// </summary>
        /// <param name="Context"></param>
        protected virtual void ProcessRequest(HttpListenerContext context)
        {
            StreamWriter writer = new StreamWriter(context.Response.OutputStream);
            Console.WriteLine("Got request: {0} {1}", context.Request.HttpMethod, context.Request.Url);

            HttpResponse r = new HttpResponse(writer);
            r.StatusCode = 200;

            r.Flush();
        }
    }
}
