using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.DebugFramework;

namespace Metreos.DebugConsole
{
	class ConsoleClient
	{
		#region Main
		[STAThread]
		static void Main(string[] args)
		{
            IConsoleApps.PrintHeaderText("Application Debugger");

			if(args.Length == 2)
			{
				int port = 0;
				try
				{
					port = int.Parse(args[1]);
				}
				catch {}

				if(port > 1024)
				{
					ConsoleClient client = new ConsoleClient();
					client.Start(args[0], port);
					return;
				}
			}
            
			Console.WriteLine("Error: You must specify an IP address and port to connect to");
			Console.WriteLine();
			Console.WriteLine("  Client <IP address> <port>");
		}
		#endregion

        private string ipAddress;
        private int port;
        private string appName;
        private string scriptName;

		private DebugClient client;
		private volatile bool shutdown = false;

		#region Construction/Startup
		public ConsoleClient()
		{
			client = new DebugClient();
			client.hitBreakpointHandler = new DebugCommandDelegate(HandleHitBreakpoint);
            client.stopDebuggingHandler = new DebugCommandDelegate(HandleStopDebugging);
			client.responseHandler = new DebugResponseDelegate(HandleResponse);
		}

		public void Start(string ipAddress, int port)
		{
            this.ipAddress = ipAddress;
            this.port = port;

			StringCollection apps = GetAppList(ipAddress);

			if(apps != null)
			{
				Console.WriteLine("Installed Applications:");

				if(apps.Count == 0)
				{
					Console.WriteLine("-- No Applications installed --");
				}
				else
				{
					foreach(string instAppName in apps)
					{
						Console.WriteLine("\t" + instAppName);
					}
				}

				Console.WriteLine();
			}

            Console.Write("Application name: ");
            appName = Console.ReadLine();

            Console.Write("Script name: ");
            scriptName = Console.ReadLine();

			if(client.Start(ipAddress, port) == false) { return; }

			while(shutdown == false) 
			{
				Menu();
			}

			client.Shutdown();
		}
		#endregion

		#region Menu/Commands
		private void Menu()
		{
			Console.WriteLine();
			Console.WriteLine("Debug Menu");
			Console.WriteLine("----------");
			Console.WriteLine(" 0. Quit");
            Console.WriteLine(" 1. Start Debugging");
            Console.WriteLine(" 2. Set Breakpoint");
			Console.WriteLine(" 3. Step Into");
            Console.WriteLine(" 4. Step Over");
            Console.WriteLine(" 5. Run");
            Console.WriteLine(" 6. Break");
            Console.WriteLine(" 7. Stop Debugging");
            Console.WriteLine("99. Disconnect/Reconnect");
			Console.Write("> ");
			string selection = Console.ReadLine();

			selection = selection.Replace("\n", "");
			switch(selection)
			{
				case "":
					break;
				case "0":
					shutdown = true;
					break;
				case "1":
                    HandleStartDebugging();
                    break;
                case "2":
                    HandleSetBreakpoint();
                    break;
                case "3":
					HandleStepInto();
					break;
                case "4":
                    HandleStepOver();
                    break;
                case "5":
					HandleRun();
					break;
                case "6":
                    HandleBreak();
                    break;
                case "7":
                    HandleStopDebugging();
                    break;
                case "99":
                    HandleReconnect();
                    break;
				default:
					Console.WriteLine("-- Invalid Entry --");
					Console.WriteLine();
					break;
			}
		}

        private void HandleStartDebugging()
        {
            client.StartDebugging(appName, scriptName, null);
        }

        private void HandleSetBreakpoint()
        {
            Console.Write("Action ID: ");
            string actionId = Console.ReadLine();

            client.SetBreakpoint(actionId, null);
        }

        private void HandleStepInto()
        {
            client.StepInto(null);
        }

        private void HandleStepOver()
        {
            client.StepOver(null);
        }

        private void HandleRun()
        {
            client.Run(null);
        }

        private void HandleBreak()
        {
            client.Break(null);
        }

        private void HandleStopDebugging()
        {
            client.StopDebugging(null);
        }

        private void HandleReconnect()
        {
            client.Shutdown();
            client.Start(ipAddress, port);
        }
		#endregion

		#region Event handlers
		internal void HandleHitBreakpoint(DebugCommand command)
		{
			Console.WriteLine("Hit Breakpoint on action: " + command.actionId);
		}

        internal void HandleStopDebugging(DebugCommand command)
        {
            Console.WriteLine("Server stopped debug session");
        }

		internal void HandleResponse(DebugResponse response)
		{
			if(response.success)
			{
				Console.WriteLine("Got success response");
			}
			else
			{
				Console.WriteLine("Got failure response.");

                if(response.failReason != null)
                {
                    Console.WriteLine("Reason: " + response.failReason);
                }
			}
		}
		#endregion

		#region Private helper methods

		private StringCollection GetAppList(string ipAddress)
		{
//			string serverURL = String.Format("http://{0}:8120/RemotingInterface", ipAddress);
//
//			SortedList channelProps = new SortedList();
//			channelProps["name"] = "RemotingInterface";
//
//			BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();
//			HttpChannel channel = new HttpChannel(channelProps, clientProv, null);  

//			if(ChannelServices.GetChannel(channel.ChannelName) == null)
//			{
//				ChannelServices.RegisterChannel(channel);
//			}
//
//			IManagement oamServer = null;
//			try
//			{
//				oamServer = (IManagement) Activator.GetObject(typeof(IManagement), serverURL);
//			}
//			catch
//			{
//				return null;
//			}

			// Log in
//			string md5pass = Metreos.Utilities.Security.EncryptPassword("metreos");
//
//			IConfig.Result result = oamServer.LogIn("Administrator", md5pass);
//			if(result != IConfig.Result.Success)
//			{
//				return null;
//			}
//
//			IConfig.ComponentInfo[] apps = null;
//			result = oamServer.GetApps(out apps);
//			if(result != IConfig.Result.Success)
//			{
//				return null;
//			}

//			StringCollection appNames = new StringCollection();
//			foreach(IConfig.ComponentInfo cInfo in apps)
//			{
//				appNames.Add(cInfo.name);
//			}
//
//			oamServer = null;
//			ChannelServices.UnregisterChannel(channel);
//
//			return appNames;

            return null;
		}

		#endregion
	}
}
