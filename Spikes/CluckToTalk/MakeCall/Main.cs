using System;
using System.IO;
using System.Net;

namespace MakeCall
{
	class MainClass
	{
		[STAThread]
		static void Main(string[] args)
		{
            if(args.Length == 0)
            {
                PrintUsage();
                return;
            }

            string samoaAddr = "http://192.168.1.112:8000/click-to-talk/initiateCall";

            StreamReader fileReader = null;
            try
            {
                fileReader = new StreamReader(args[0]);
            }
            catch(Exception e)
            {
                PrintUsage();
                Console.WriteLine("Error: {0}", e.Message);
                return;
            }

            string xmlStr = fileReader.ReadToEnd();
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(xmlStr);

            HttpWebRequest webRequest = (HttpWebRequest) HttpWebRequest.Create(samoaAddr);
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml";
            webRequest.ContentLength = buffer.Length;
            webRequest.TransferEncoding = null;

            Stream reqStream = webRequest.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Close();

            HttpWebResponse webResponse = null;
            try
            {
                webResponse = (HttpWebResponse) webRequest.GetResponse();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Request sent successfully.");
		}

        static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: MakeCall xmlFile");
            Console.WriteLine();
        }
	}
}
