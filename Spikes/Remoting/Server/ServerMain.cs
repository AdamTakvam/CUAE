using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

using ServerInterface;

namespace Server
{
	class ServerMain
	{
        private const int PORT = 8020;
        private const int PSEUDO_CLIENT_PORT = 8030;
        private static string url;

		[STAThread]
		static void Main(string[] args)
		{
            try
            {
                HttpChannel c = new HttpChannel(PORT);
                ChannelServices.RegisterChannel(c);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingServer), 
                    "RemotingServer", 
                    WellKnownObjectMode.Singleton);
            }
            catch(Exception e)
            {
                Console.WriteLine("Server cannot start, exception is:\n" + e.ToString());
                return;
            }

            url = "http://192.168.1.100:" + PORT.ToString() + "/RemotingServer";

//            if(!InitializeServer(url))
//            {
//                return;
//            }

            Console.WriteLine("Server ON at "+ PORT.ToString());
            Console.WriteLine("Press enter to stop the server...");
            Console.ReadLine();
		}

        static bool InitializeServer(string url)
        {
            try
            {
                HttpChannel c = new HttpChannel(PSEUDO_CLIENT_PORT);  
                ChannelServices.RegisterChannel(c);
            }
            catch(Exception e)
            {
                Console.WriteLine("Server cannot initialize, exception is:\n" + e.ToString());
                return false;
            }

            IRemotingServer server = (IRemotingServer) Activator.GetObject(typeof(IRemotingServer), url); 
            server.SetName("The Persistent Remoting Server");

            return true;
        }
	}
}
