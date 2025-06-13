using System;
using System.IO;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;

namespace Metreos.AppDeployTool
{
    public sealed class ConsoleDeploy
    {     
        [STAThread]
        static void Main(string[] args)
        {
            IConsoleApps.PrintHeaderText("Application Installation Tool");

			ConsoleDeploy cd = new ConsoleDeploy();

			string errorStr = null;
			if(cd.Initialize(args, out errorStr) == false)
			{
				if(errorStr != null)
				{
					Console.WriteLine(errorStr);
				}

				PrintHelp();
				return;
			}

			if(cd.DeployApp())
				Console.WriteLine("\nApplication deployed successfully.");
			else
				Console.WriteLine("\nUnable to deploy application.");
        }

        private string appName = null;
		private FileInfo package = null;
		private string appServerIp = IDeployTool.Defaults.AppServerIp;
		private int appServerPort = IDeployTool.Defaults.AppServerPort;
		private string appServerUsername = null;
		private string appServerPassword = null;
		private bool forceUninstall = false;
		private bool forceUpdate = false;

		public ConsoleDeploy() {}

		public bool Initialize(string[] args, out string error)
		{
			error = null;

			if(args == null || args.Length == 0)
				return false;

			CommandLineArguments parser = new CommandLineArguments(args);

			if(parser.IsParamPresent(IDeployTool.Params.Help))
				return false;

			if(parser.IsParamPresent(IDeployTool.Params.Debug))
			{
				Console.WriteLine("Attach the debugger and press Enter to continue");
				Console.ReadLine();
			}

            if(parser.IsParamPresent(IDeployTool.Params.AppName))
                appName = parser.GetSingleParam(IDeployTool.Params.AppName);

			if(parser.IsParamPresent(IDeployTool.Params.AppServerIp))
				appServerIp = parser.GetSingleParam(IDeployTool.Params.AppServerIp);
            
			if(parser.IsParamPresent(IDeployTool.Params.AppServerPort))
				appServerPort = Convert.ToInt32(parser.GetSingleParam(IDeployTool.Params.AppServerPort));

			if(parser.IsParamPresent(IDeployTool.Params.Username))
				appServerUsername = parser.GetSingleParam(IDeployTool.Params.Username);
			else
			{
				error = "No username specified";
				return false;
			}

			if(parser.IsParamPresent(IDeployTool.Params.Password))
				appServerPassword = parser.GetSingleParam(IDeployTool.Params.Password);
			else
			{
				error = "No password specified";
				return false;
			}

			string packagePath = parser.GetSingleParam(IDeployTool.Params.AppFile);
			if(packagePath == null || packagePath == String.Empty)
			{
				error = "No application package specified";
				return false;
			}

			try
			{
				package = new FileInfo(packagePath);
				if(!package.Exists)
				{
					error = "The specified application package does not exist";
					return false;
				}
			}
			catch
			{
				error = String.Format("{0} is not a valid path", packagePath);
				return false;
			}

			if(parser.IsParamPresent(IDeployTool.Params.Force))
			{
				string forceOption = parser.GetSingleParam(IDeployTool.Params.Force);
				if(String.Compare(forceOption, IDeployTool.ParamValues.Uninstall, true) == 0)
					forceUninstall = true;
				else if(String.Compare(forceOption, IDeployTool.ParamValues.Update, true) == 0)
					forceUpdate = true;
				else
				{
					error = String.Format("Force argument is invalid: {0}. Specify {1} or {2}.",
						forceOption, IDeployTool.ParamValues.Uninstall, IDeployTool.ParamValues.Update);
					return false;
				}
			}

			return true;
		}

		public bool DeployApp()
		{
			using(Metreos.Core.AppDeploy deployment = new AppDeploy())
			{
				deployment.LogOutput += new Message(LogOutput);
				deployment.ErrorMessage += new Message(LogError);
				deployment.StageElapse += new Step(StageElapse);
				deployment.UninstallPrompt += new Uninstall(UninstallPrompt);

				return deployment.Deploy(
                    appName,
					package, 
					appServerIp, 
					appServerUsername, 
					Metreos.Utilities.Security.EncryptPassword(appServerPassword),
					appServerPort, 
					IDeployTool.Defaults.SshPort, 0); 
			}
		}

        private void LogOutput(string message)
        {
            Console.WriteLine("Info: {0}", message);
        }

        private void LogError(string message)
        {
            Console.WriteLine("Error: {0}", message);
        }

        private void StageElapse(float stepAmount, string stepDescription)
        {
           Console.WriteLine("***** {0}", stepDescription);
        }

        private AppDeploy.DeployOption UninstallPrompt()
        {
            AppDeploy.DeployOption selection = AppDeploy.DeployOption.Cancel;

            if(forceUninstall)
				return AppDeploy.DeployOption.Uninstall;
			else if(forceUpdate)
				return AppDeploy.DeployOption.Update;

            bool selected = false;
            while(!selected)
            {
                Console.WriteLine(IDeployTool.PromptMessage);

                string input = Console.ReadLine();

                if(String.Compare(input, IDeployTool.ParamValues.Uninstall, true) == 0)
                {
                    selection = AppDeploy.DeployOption.Uninstall;
                    selected = true;
                }
                else if(String.Compare(input, IDeployTool.ParamValues.Update, true) == 0)
                {
                    selection = AppDeploy.DeployOption.Update;
                    selected = true;
                }
                else if(String.Compare(input, IDeployTool.ParamValues.Cancel, true) == 0)
                {
                    selection = AppDeploy.DeployOption.Cancel;
                    selected = true;
                }
            }

            return selection;
        }

		public static void PrintHelp()
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("  mcadeploy.exe {0} [{1}] [{2}] {3}\n\t\t{4} [{5}] [{6}]", 
				IDeployTool.ParamHelp.AppFile, IDeployTool.ParamHelp.AppServerIp,
				IDeployTool.ParamHelp.AppServerPort, IDeployTool.ParamHelp.Username,
				IDeployTool.ParamHelp.Password, IDeployTool.ParamHelp.Force, IDeployTool.ParamHelp.Help);
			Console.WriteLine();
			Console.WriteLine("Required Parameters:");
            Console.WriteLine("  {0,-20} Name of the application (used for update only)", IDeployTool.ParamHelp.AppName);
			Console.WriteLine("  {0,-20} File name of the application package (*.mca)", IDeployTool.ParamHelp.AppFile);
			Console.WriteLine("  {0,-20} Application Server Administrative Username", IDeployTool.ParamHelp.Username);
			Console.WriteLine("  {0,-20} Application Server Administrative Password", IDeployTool.ParamHelp.Password);
			Console.WriteLine();
			Console.WriteLine("Optional Parameters:");
			Console.WriteLine("  {0,-20} Application Server IP Address (default={1})", IDeployTool.ParamHelp.AppServerIp, IDeployTool.Defaults.AppServerIp);
			Console.WriteLine("  {0,-20} Application Server ManagementPort (default={1})", IDeployTool.ParamHelp.AppServerPort, IDeployTool.Defaults.AppServerPort);
			Console.WriteLine("  {0,-20} No prompt, force action ({1} | {2})", IDeployTool.ParamHelp.Force, IDeployTool.ParamValues.Uninstall, IDeployTool.ParamValues.Update);
			Console.WriteLine("  {0,-20} Print this help screen", IDeployTool.ParamHelp.Help);
        }
    }
}
