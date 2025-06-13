using System;
using System.IO;
using System.Net;

namespace HttpSender
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Sender
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            string uri = "http://localhost:8000";
            string sessionId = null;
            string numToDialOrLocationId = null;

            if(args.Length > 0)
            {
                uri = args[0];

                if(args.Length > 1)
                {
                    sessionId = args[1];
                }

                if(args.Length > 2)
                {
                    numToDialOrLocationId = args[2];
                }
            }

            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(uri);

            string content = "";
            if(uri.EndsWith("create"))
            {
				numToDialOrLocationId = sessionId;
				
				sessionId = null;

                if(numToDialOrLocationId == null)
                {
                    numToDialOrLocationId = "2246@192.168.1.250";
                }

                content =
                @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                  <conferenceRequest type=""create"" xmlns=""http://tempuri.org/ConferenceCommand.xsd"">
                  <location address=""" + numToDialOrLocationId + @""" description=""Ben Dover"" />                  
                  </conferenceRequest>";
            }
            else if(uri.EndsWith("join"))
            {
                if(numToDialOrLocationId == null)
                {
                    numToDialOrLocationId = "1017@192.168.1.250";
                }

                content =
                @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                  <conferenceRequest type=""join"" xmlns=""http://tempuri.org/ConferenceCommand.xsd"">
                  <location address=""" + numToDialOrLocationId + @""" description=""Poppa Woody"" />
                  </conferenceRequest>";
            }
            else if(uri.EndsWith("kick"))
            {
                content =
                 @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                  <conferenceRequest type=""kick"" xmlns=""http://tempuri.org/ConferenceCommand.xsd"">
                  <location>" + numToDialOrLocationId + @"</location>
                  </conferenceRequest>";
            }
            else if(uri.EndsWith("mute"))
            {
                content =
                    @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                  <conferenceRequest type=""mute"" xmlns=""http://tempuri.org/ConferenceCommand.xsd"">
                  <location>" + numToDialOrLocationId + @"</location>
                  </conferenceRequest>";
            }

            if(sessionId != null)
            {
                request.Headers.Add("Metreos-SessionID:" + sessionId);
            }

            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = content.Length;
            request.TransferEncoding = null;
            
            Stream writeStream = request.GetRequestStream();
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(content);
            writeStream.Write(buffer, 0, buffer.Length);

            Console.WriteLine("Sent:\n" + content + "\nTo: " + uri);

            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch(Exception)
            {
                Console.WriteLine("Something weird's goin' on. I'm tired of waiting on you.");
                return;
            }

            Console.WriteLine();

            Console.WriteLine(response.Headers);

            if(response.ContentLength > 0)
            {
                Stream readStream = response.GetResponseStream();
                buffer = new Byte[512];
                int length = readStream.Read(buffer, 0, 512);
                string responseStr = System.Text.Encoding.ASCII.GetString(buffer, 0, length);

                Console.WriteLine(responseStr);
                readStream.Close();
                readStream = null;
            }

            
            writeStream.Close();

            
            writeStream = null;
		}
	}
}
