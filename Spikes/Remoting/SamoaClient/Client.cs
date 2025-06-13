using System;
using System.IO;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Samoa.RemotableInterfaces;

namespace Client
{
	class ClientMain
	{
		private const int SERVER_PORT = 8120;
		private static string url;

		private static ArrayList mediaServers;
        private static ArrayList applications;
		private static IManagement server;

		[STAThread]
		static void Main(string[] args)
		{
			mediaServers = new ArrayList();
            applications = new ArrayList();

			if(args.Length > 0)
			{
				url = args[0];
			}
			else
			{
				url = "http://localhost:" + SERVER_PORT.ToString() + "/RemotingInterface";
			}

			HttpChannel c = null;

			try
			{
				c = new HttpChannel();  
				ChannelServices.RegisterChannel(c);
                server = (IManagement) Activator.GetObject(typeof(IManagement), url);
			}
			catch(Exception e)
			{
				Console.WriteLine("Client cannot start, exception is:\n" + e.ToString());
				return;
			}
            
            try
            {
                IConfig.Result result = server.LogIn("metreos", "metreos");
                if(result != IConfig.Result.Success)
                {
                    Console.WriteLine("Error logging into Samoa OAM interface. {0}. Exiting.", result);
                    return;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error logging into Samoa OAM interface. {0}", e.Message);
                return;
            }

			while(true)
			{
				switch(PrintMenu().ToUpper())
				{
					case "1":
						AddMediaServer();
						break;
					
                    case "2":
						RemoveMediaServer();
						break;
					
                    case "3":
						ListMediaServers();
						break;
					
                    case "4":
						StartMediaServer();
						break;
					
                    case "5":
						StopMediaServer();
						break;
                    
                    case "6":
                        InstallApplication();
                        break;
                    
                    case "7":
                        UninstallApplication();
                        break;
                    
                    case "8":
                        ListApplications();
                        break;
                    
                    case "9":
                        InstallProvider();
                        break;
                    
                    case "10":
                        InvokeExtension();
                        break;

                    case "Q":
						ChannelServices.UnregisterChannel(c);
						Console.WriteLine();
						Console.WriteLine("Later, homey...");
						return;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid selection, try again.");
                        break;
				}
			}
		}

        #region Media Server Methods
        
		public static void AddMediaServer()
		{
			Console.WriteLine();

			Console.Write("Hostname (localhost): ");
			string hostname = Console.ReadLine();
			
			if(hostname == "")
			{
				hostname = "localhost";
			}

			Console.Write("Port (8122): ");
			string portStr = Console.ReadLine();
			
			int port = 0;
			if(portStr == "")
			{
				port = 8122;
			}
			else
			{			
				try
				{
					port = int.Parse(portStr);
				}
				catch(Exception)
				{
					Console.WriteLine("Invalid port");
					return;
				}
			}

			string msId;
			IConfig.Result result = server.AddMediaServer(hostname, port, out msId);

			Console.WriteLine("AddMediaServer result: " + result);

			if(result == IConfig.Result.Success)
			{
				IConfig.ComponentInfo msInfo;
				result = server.GetMediaServerInfo(msId, out msInfo);

				Console.WriteLine("GetMediaServerInfo result: " + result);

				if(result == IConfig.Result.Success)
				{
					int index = mediaServers.Add(msInfo);
					Console.WriteLine("Successfully added new media server (ID = " + index + ", status = " + msInfo.status + ")");
				}
			}
		}


		public static void RemoveMediaServer()
		{
			Console.WriteLine();

			Console.Write("Enter ID of media server to remove: ");
			string id = Console.ReadLine();

			ushort index;
			try
			{
				index = ushort.Parse(id);
			}
            catch(Exception)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid ID");
                return;
            }

            if((index < 0) || (index >= mediaServers.Count))
            {
                Console.WriteLine();
                Console.WriteLine("ID out of range");
                return;
            }

			IConfig.ComponentInfo msInfo = (IConfig.ComponentInfo) mediaServers[index];
			
            Console.WriteLine("Removing media server {0}", msInfo.name);
            IConfig.Result result = server.RemoveMediaServer(msInfo.name);

			Console.WriteLine("RemoveMediaServer result: " + result);

			if(result == IConfig.Result.Success)
			{
				mediaServers.Remove(index);
			}
		}

		public static void ListMediaServers()
		{
			Console.WriteLine();
			
			IConfig.ComponentInfo[] msIds;
			IConfig.Result result = server.GetMediaServers(out msIds);
			if(result == IConfig.Result.ServerError)
			{
				Console.WriteLine("Could not get media server list.");
				return;
			}

			mediaServers.Clear();

			Console.WriteLine("Existing Media Servers");
			Console.WriteLine("----------------------");

			if((result == IConfig.Result.NotFound) || (msIds == null))
			{
				Console.WriteLine("<none>");
				return;
			}
			else if(msIds.Length == 0)
			{
				Console.WriteLine("<none>");
			}

            int i = 0;

            foreach(IConfig.ComponentInfo msInfo in msIds)
            {
                mediaServers.Add(msInfo);
                Console.WriteLine(i + ") " + msInfo.path + " (" + msInfo.status + ")");
                i++;
            }
		}

		public static void StartMediaServer()
		{
			Console.WriteLine();

			Console.Write("Enter ID of media server to start: ");
			string id = Console.ReadLine();

			ushort index;
			try
			{
				index = ushort.Parse(id);
			}
            catch(Exception)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid ID");
                return;
            }

            if((index < 0) || (index >= mediaServers.Count))
            {
                Console.WriteLine();
                Console.WriteLine("ID out of range");
                return;
            }

			IConfig.ComponentInfo msInfo = (IConfig.ComponentInfo) mediaServers[index];
			
            Console.WriteLine("Starting media server {0}", msInfo.name);
            IConfig.Result result = server.StartMediaServer(msInfo.name);

			Console.WriteLine("StartMediaServer result: " + result);

			if(result == IConfig.Result.Success)
			{
				msInfo.status = IConfig.Status.Enabled_Running;
			}
		}

		public static void StopMediaServer()
		{
			Console.WriteLine();

			Console.Write("Enter ID of media server to stop: ");
			string id = Console.ReadLine();

			ushort index;
			try
			{
				index = ushort.Parse(id);
			}
			catch(Exception)
			{
                Console.WriteLine();
				Console.WriteLine("Invalid ID");
				return;
			}

			if((index < 0) || (index >= mediaServers.Count))
			{
                Console.WriteLine();
				Console.WriteLine("ID out of range");
				return;
			}

			IConfig.ComponentInfo msInfo = (IConfig.ComponentInfo) mediaServers[index];
			
            Console.WriteLine("Stopping media server {0}", msInfo.name);
            IConfig.Result result = server.StopMediaServer(msInfo.name);

			Console.WriteLine("StopMediaServer result: " + result);

			if(result == IConfig.Result.Success)
			{
				msInfo.status = IConfig.Status.Enabled_Stopped;
			}
		}

        #endregion

        #region Application Methods

        public static void InstallApplication()
        {
            byte[] data;

            Console.WriteLine();
            Console.Write("Enter application filename: ");

            string filename = Console.ReadLine();

            System.IO.BinaryReader reader  = null;
            System.IO.FileInfo info = null;

            try
            {
                info = new System.IO.FileInfo(filename);
                System.IO.FileStream stream = info.OpenRead();
                reader = new System.IO.BinaryReader(stream);
               
                data = new byte[info.Length];
                reader.Read(data, 0, data.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                return;
            }
            finally
            {
                reader.Close();
            }

            IConfig.Result result = server.InstallApp(data, info.Name);

            Console.WriteLine("InstallApplication result: " + result);
        }

        
        public static void UninstallApplication()
        {
            Console.WriteLine();

            Console.Write("Enter ID of application to uninstall: ");
            string id = Console.ReadLine();

            ushort index;
            try
            {
                index = ushort.Parse(id);
            }
            catch(Exception)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid ID");
                return;
            }

            if((index < 0) || (index >= applications.Count))
            {
                Console.WriteLine();
                Console.WriteLine("ID out of range");
                return;
            }

            IConfig.ComponentInfo appInfo = (IConfig.ComponentInfo) applications[index];

            server.DisableApp(appInfo.name);

            Console.WriteLine("Uninstalling application {0}", appInfo.name);
            IConfig.Result result = server.UninstallApp(appInfo.name);

            Console.WriteLine("UninstallApp result: " + result);

            if(result == IConfig.Result.Success)
            {
                applications.Remove(index);
            }
        }


        public static void ListApplications()
        {
            Console.WriteLine();
			
            IConfig.ComponentInfo[] appIds;
            IConfig.Result result = server.GetApps(out appIds);
            
            if(result == IConfig.Result.ServerError)
            {
                Console.WriteLine("Could not get application list.");
                return;
            }

            applications.Clear();

            Console.WriteLine("Currently Installed Applications");
            Console.WriteLine("--------------------------------");

            if((result == IConfig.Result.NotFound) || (appIds == null))
            {
                Console.WriteLine("<none>");
                return;
            }
            else if(appIds.Length == 0)
            {
                Console.WriteLine("<none>");
            }

            int i = 0;

            foreach(IConfig.ComponentInfo appInfo in appIds)
            {
                Console.WriteLine("{0}) {1,-13} {2}", i, appInfo.name, appInfo.status);
                applications.Add(appInfo);
                i++;
            }
        }

        #endregion

        #region Provider Methods

        public static void InstallProvider()
        {
            byte[] data;

            Console.WriteLine();
            Console.Write("Enter provider filename: ");

            string filename = Console.ReadLine();

            System.IO.BinaryReader reader  = null;
            System.IO.FileInfo info = null;

            try
            {
                info = new System.IO.FileInfo(filename);
                System.IO.FileStream stream = info.OpenRead();
                reader = new System.IO.BinaryReader(stream);
               
                data = new byte[info.Length];
                reader.Read(data, 0, data.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                return;
            }
            finally
            {
                reader.Close();
            }

            IConfig.Result result = server.InstallProvider(data, info.Name);

            Console.WriteLine("InstallProvider result: " + result);
        }

        public static void InvokeExtension()
        {
            ArrayList extInfo = new ArrayList();
            ArrayList extProvNames = new ArrayList();

            IConfig.ComponentInfo[] providers;
            if(server.GetProviders(out providers) != IConfig.Result.Success)
            {
                Console.WriteLine("Error: Could not get provider list.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Provider Extensions");
            Console.WriteLine("-------------------");

            IConfig.Extension[] extensions;
            IConfig.ComponentInfo cInfo = null;
            for(int i=0; i<providers.Length; i++)
            {
                cInfo =  providers[i];
                if(server.GetExtensions(cInfo.name, out extensions) != IConfig.Result.Success)
                {
                    Console.WriteLine("Error: Failed to get extension list for " + cInfo.name);
                    continue;
                }

                if(extensions == null) { continue; }

                for(int x=0; x<extensions.Length; x++)
                {
                    int index = extInfo.Add(extensions[x]);
                    extProvNames.Add(cInfo.name);
                    Console.WriteLine(index + ") " + cInfo.name + ": " + extensions[x].name);
                }
            }

            Console.WriteLine();
            Console.Write("> ");

            string selection = Console.ReadLine();

            int sel = -1;
            try
            {
                sel = int.Parse(selection);
            }
            catch(Exception) 
            {
                Console.WriteLine("Invalid selection");
                return;
            }
    
            
            IConfig.Result result = server.InvokeExtension((string)extProvNames[sel], 
                                   (IConfig.Extension)extInfo[sel]);

            if(result != IConfig.Result.Success)
            {
                Console.WriteLine("Failed invoking extension");
                return;
            }

            Console.WriteLine("Extension invoked successfully.");
        }

        #endregion

		public static string PrintMenu()
		{
			Console.WriteLine();

			Console.WriteLine("         Menu");
			Console.WriteLine("----------------------");
			Console.WriteLine(" 1. Add Media Server");
			Console.WriteLine(" 2. Remove Media Server");
			Console.WriteLine(" 3. List Media Servers");
			Console.WriteLine(" 4. Start Media Server");
			Console.WriteLine(" 5. Stop Media Server");
            Console.WriteLine(" 6. Install Application");
            Console.WriteLine(" 7. Uninstall Application");
            Console.WriteLine(" 8. List Applications");
            Console.WriteLine(" 9. Install Provider");
            Console.WriteLine("10. Invoke Provider Extension");
            Console.WriteLine();
            Console.WriteLine(" Q. Quit");
			Console.WriteLine();
			Console.Write("> ");
			return Console.ReadLine();
		}
	}
}
			
//
//			CODE GRAVEYARD
//

//			ArrayList configValues;
//			IOam.RESULT result = server.GetConfig(userId, out configValues);
//				
//			if(result != IOam.RESULT.Success)
//			{
//				Console.WriteLine("Could not get config, server returned: " + result.ToString());
//				return;
//			
//			Console.WriteLine();
//			Console.WriteLine("Samoa config");
//			Console.WriteLine("------------");
//			
//			IOam.ConfigValue cValue = null;
//			for(int i=0; i<configValues.Count; i++)
//			{
//				cValue = (IOam.ConfigValue) configValues[i];
//				Console.WriteLine(cValue.name + " = " + cValue.Value + " (type: " + cValue.type + ")");
//			}
//			
//			ArrayList providers;
//			IOam.RESULT result = server.GetProviders(userId, out providers);
//			
//			if(result != IOam.RESULT.Success)
//			{
//				Console.WriteLine("Could not get providers, server returned: " + result.ToString());
//				return;
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Providers");
//			Console.WriteLine("---------");
//
//			IOam.ProviderInfo pInfo = null;
//			for(int i=0; i<providers.Count; i++)
//			{
//				pInfo = (IOam.ProviderInfo) providers[i];
//				Console.WriteLine("Name: " + pInfo.name);
//				Console.WriteLine("State: " + pInfo.state);
//				Console.WriteLine("Version: " + pInfo.version);
//				Console.WriteLine();
//			}
//
//			ArrayList apps;
//			result = server.GetApps(userId, out apps);
//			
//			if(result != IOam.RESULT.Success)
//			{
//				Console.WriteLine("Could not get apps, server returned: " + result.ToString());
//				return;
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Applications");
//			Console.WriteLine("------------");
//
//			IOam.AppInfo aInfo = null;
//			for(int i=0; i<apps.Count; i++)
//			{
//				aInfo = (IOam.AppInfo) apps[i];
//				Console.WriteLine("Name: " + aInfo.name);
//				Console.WriteLine("# Active Sessions: " + aInfo.numActiveSessions);
//				Console.WriteLine("# Total Sessions: " + aInfo.numTotalSessions);
//				Console.WriteLine("State: " + aInfo.state);
//				Console.WriteLine("Version: " + aInfo.version);
//				Console.WriteLine();
//			}

