using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;

namespace HttpListenerSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer s = new HttpServer();
            s.Start("http://*:32323/");
            Console.ReadLine();
            s.Stop();
        }
    }
}
