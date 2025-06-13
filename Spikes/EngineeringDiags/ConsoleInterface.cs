using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

using Metreos.Samoa.Interfaces;

namespace EngineeringDiags
{
	/// <summary>
	/// Console interface for Samoa engineering diagnostics
	/// </summary>
	class ConsoleInterface
	{
		private const int PORT = 8010;
		private const int SERVER_PORT = 8120;
		private static string url;
		private static string appName = null;

		private static IEngMgmt server;
		private static Timer fetchTimer;

		[STAThread]
		static void Main(string[] args)
		{
			if(args.Length > 0)
			{
				url = args[0];

				if(args.Length > 1)
				{
					appName = args[1];
				}
			}
			else
			{
				url = "http://localhost:" + SERVER_PORT.ToString() + "/RemotingInterface";
			}

			try
			{
				HttpChannel c = new HttpChannel(PORT);  
				ChannelServices.RegisterChannel(c);
			}
			catch(Exception e)
			{
				Console.WriteLine("Client cannot start, exception is:\n" + e.ToString());
				return;
			}

			server = (IEngMgmt) Activator.GetObject(typeof(IEngMgmt), url); 

			// Set a timer
			fetchTimer = new Timer(new TimerCallback(GetInfo), null, 0, 300000);

			Console.WriteLine("Press any key to exit");
			Console.Read();
		}
		
		private static void GetInfo(object state)
		{
			string userId = server.LogIn("username", "password", IOam.ENCODING.PlainText);

			if(userId == null)
			{
				Console.WriteLine("Access denied");
				return;
			}

			int Value;
			server.GetRtrTriggerTableSize(userId, out Value);
			Console.WriteLine("Router trigger table size: " + Value);

			server.GetRtrResponseTableSize(userId, out Value);
			Console.WriteLine("Router response table size: " + Value);

			server.GetRtrAppTableSize(userId, out Value);
			Console.WriteLine("Router application table size: " + Value);

			server.GetRtrAppQTableSize(userId, out Value);
			Console.WriteLine("Router application queue table size: " + Value);

			server.GetRtrAliasTableSize(userId, out Value);
			Console.WriteLine("Router alias table size: " + Value);

			server.GetPalProviderTableSize(userId, out Value);
			Console.WriteLine("Number of providers loaded: " + Value);

			server.GetPalNsTableSize(userId, out Value);
			Console.WriteLine("Number of provider namespaces registered: " + Value);

			server.GetSamAppDomainTableSize(userId, out Value);
			Console.WriteLine("Application manager domain table size: " + Value);

			if(appName != null)
			{
				server.GetSamAppPoolSize(appName, userId, out Value);
				Console.WriteLine("Pool size of app " + appName + ": " + Value);
			}

			Console.WriteLine();
		}
	}
}
