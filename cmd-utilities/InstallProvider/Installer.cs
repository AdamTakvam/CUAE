using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml.Serialization;
using System.Collections.Specialized;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.IPC.Xml;
using Metreos.ProviderPackagerCore;

namespace Metreos.InstallProvider
{
	/// <summary>Installs provider packages.</summary>
	public sealed class Installer
	{
        internal abstract class Params
        {
            public const string HelpParam       = "h";
            public const string Uninstall       = "u";

            public abstract class Help
            {
                public const string HelpParam   = "-" + Params.HelpParam;
                public const string Uninstall   = "-" + Params.Uninstall;
                public const string File        = "<Filename>";
            }
        }

        internal abstract class Consts
        {
            public const string Address     = "127.0.0.1";
            public const int Port           = 8120;
            public const int InstallTimeout = 30000;  // 30 seconds
        }

		[STAThread]
		static void Main(string[] args)
		{
            // Print copyright info
            IConsoleApps.PrintHeaderText("Provider Installation Tool");

            // Secret debugger flag
            if(args != null && args.Length > 1 && args[0] == "-d")
                System.Diagnostics.Debugger.Launch();               

            CommandLineArguments clargs = new CommandLineArguments(args);

            if(clargs.IsParamPresent(Params.Help.HelpParam))
            {
                PrintHelp();
                return;
            }

            StringCollection saps = clargs.GetStandAloneParameters();
            if(saps == null || saps.Count != 1)
            {
                PrintHelp();
                Environment.ExitCode = 1;
                return;
            }

            Installer i = new Installer();

            if(clargs.IsParamPresent(Params.Uninstall))
            {
                i.UninstallProvider(saps[0]);
            }
            else  // Install provider
            {
                DirectoryInfo appServerDir = GetAppServerRootDir();
                if(appServerDir == null || appServerDir.Exists == false)
                {
                    Console.WriteLine("Error: Could not locate AppServer install directory. Please move me back to the CUAE framework directory");
                    Environment.ExitCode = 1;
                    return;
                }

                FileInfo file = new FileInfo(saps[0]);
                if(file.Exists == false)
                {
                    PrintHelp("Could not find file: " + saps[0]);
                    Environment.ExitCode = 1;
                    return;
                }
                
                if(file.Extension == IPackager.ProvFileExt)
                {
                    if(!i.PackageProvider(file, out file))
                    {
                        Environment.ExitCode = 1;
                        return;
                    }
                }

                if(file.Extension == IPackager.PackFileExt)
                {
                    if(!i.InstallPackage(file, appServerDir))
                        Environment.ExitCode = 1;
                }
                else
                {
                    Console.WriteLine("Error: Unknown file extension: " + file.Name);
                }
            }
		}

        private IpcXmlClient client;
        private XmlSerializer commandSerializer;
        private XmlSerializer responseSerializer;
        private AutoResetEvent done;
        private bool uninstall = false;

        public Installer()
        {
            this.done = new AutoResetEvent(false);

            this.commandSerializer = new XmlSerializer(typeof(commandType));
            this.responseSerializer = new XmlSerializer(typeof(responseType));

            this.client = new IpcXmlClient();
            this.client.onXmlMessageReceived +=new OnXmlMessageReceivedDelegate(OnMessageReceived);
        }

        public void UninstallProvider(string name)
        {
            this.uninstall = true;

            SendInstallCommand(name);
        }

        public bool PackageProvider(FileInfo providerFile, out FileInfo packageFile)
        {
            packageFile = null;

            Packager packager = new Packager(providerFile.FullName);
            try { packageFile = packager.GeneratePackage(); }
            catch(Exception e)
            {
                Console.WriteLine("Error: Failed to package provider file: " + e.Message);
                return false;
            }
            return true;
        }

        public bool InstallPackage(FileInfo packageFile, DirectoryInfo appServerDir)
        {
            // Locate deploy directory
            DirectoryInfo[] dirs = appServerDir.GetDirectories(IConfig.AppServerDirectoryNames.DEPLOY);
            if(dirs == null || dirs.Length != 1)
            {
                Console.WriteLine("Error: Unable to locate deploy directory");
                return false;
            }

            DirectoryInfo deployDir = dirs[0];
            string destFile = Path.Combine(deployDir.FullName, packageFile.Name);

            // Copy MCP file
            try { packageFile.CopyTo(destFile, true); }
            catch(Exception e)
            {
                Console.WriteLine("Error: Failed to copy '{0}' to '{1}': {2}", packageFile.Name, deployDir.FullName, e.Message);
                return false;
            }

            return SendInstallCommand(packageFile.Name);
        }

        private bool SendInstallCommand(string providerName)
        {
            // Connect to application server
            using(client)
            {
				client.RemoteEp = new IPEndPoint(IPAddress.Parse(Consts.Address), Consts.Port);
                try { client.Open(); }
                catch(Exception e)
                {
                    Console.WriteLine("Cannot connect to '{0}:{1}': {2}", Consts.Address, Consts.Port, e.Message);
                    return false;
                }

                // Build command object
                commandType command = new commandType();

                if(uninstall)
                    command.name = IManagement.Commands.UninstallProvider.ToString();
                else
                    command.name = IManagement.Commands.InstallProvider.ToString();
                
                command.param = new paramType[1];
                command.param[0] = new paramType();
                command.param[0].name = IManagement.ParameterNames.NAME;
                command.param[0].Value = providerName;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter sw = new System.IO.StringWriter(sb);
                commandSerializer.Serialize(sw, command);

                client.Write(sb.ToString());

                if(uninstall)
                    Console.WriteLine("Provider uninstall command sent to AppServer...");
                else
                    Console.WriteLine("Provider install command sent to AppServer...");

                // Wait for response
                if(done.WaitOne(Consts.InstallTimeout, false) == false)
                {
                    Console.WriteLine("Error: No response received from AppServer");
                    return false;
                }
            }
            return true;
        }

        private void OnMessageReceived(IpcXmlClient ipcClient, string message)
        {
            responseType response = null;
            try
            {
                System.IO.StringReader sr = new System.IO.StringReader(message);
                response = (responseType) responseSerializer.Deserialize(sr);
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not read data from network. Error: " + e.Message);
                Environment.ExitCode = 1;
                done.Set();
                return;
            }

            if(response.type == IConfig.Result.Success)
            {
                if(uninstall)
                    Console.WriteLine("Provider uninstalled successfully");
                else
                    Console.WriteLine("Provider installed successfully");
            }
            else
            {
                if(response.Value == null || response.Value == String.Empty)
                    response.Value = "No Description";

                Console.WriteLine("Got {0} response: {1}", response.type.ToString(), response.Value);

                if(response.resultList != null && response.resultList.Length > 0)
                {
                    Console.WriteLine("Error details:");

                    foreach(string result in response.resultList)
                    {
                        Console.WriteLine(result);
                    }
                }
                Environment.ExitCode = 1;
            }
            done.Set();
        }

        private static DirectoryInfo GetAppServerRootDir()
        {
            // Find the AppServer directory relative to this one.
            // Woe unto he who moves this utility somewhere else!
            DirectoryInfo dir = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory);
            if(dir == null) { return null; }

            dir = dir.Parent;
            if(dir == null) { return null; }

            dir = dir.Parent;
            if(dir == null) { return null; }

            DirectoryInfo[] dirs = dir.GetDirectories(IConfig.AppServerDirectoryNames.ROOT);
            if(dirs == null || dirs.Length != 1) { return null; }
            return dirs[0];
        }

        public static void PrintHelp()
        {
            PrintHelp(null);
        }

        private static void PrintHelp(string error)
        {
            if(error != null)
            {
                Console.WriteLine("Error: " + error);
                Console.WriteLine();
            }

            Console.WriteLine("Usage: INSTPROV.EXE [{0}] [{1}] {2}", 
                Params.Help.HelpParam, Params.Help.Uninstall, Params.Help.File);
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("  {0,-20} Provider name", Params.Help.File);
            Console.WriteLine();
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("  {0,-20} Print this help screen", Params.Help.HelpParam);
            Console.WriteLine("  {0,-20} Uninstall provider", Params.Help.Uninstall);
            Console.WriteLine();
            Console.WriteLine("Notes:");
            Console.WriteLine("  1. This utility must be run locally on the target server.");
            Console.WriteLine("  2. The name of the provider is not the same for install and uninstall.");
            Console.WriteLine("     For provider installations, the filename of the assembly ({0}) or package \n\t({1}) containing the provider is required.", IPackager.ProvFileExt, IPackager.PackFileExt);
            Console.WriteLine("     For uninstallation, the name of the provider as specified in the provider \n\tcode must be supplied.");            
        }
	}
}
