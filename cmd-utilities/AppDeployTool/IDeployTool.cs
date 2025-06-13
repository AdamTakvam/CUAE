using System;

using Metreos.Core;

namespace Metreos.AppDeployTool
{
	public abstract class IDeployTool
	{
		public const string PromptMessage	= "----> Old version exists. Would you like to '{0}', '{1}', or '{2}'?";

		public abstract class Params
		{
            public const string AppName         = "n";
			public const string AppFile         = "mca";
			public const string AppServerIp     = "ip";
			public const string AppServerPort   = "port";
			public const string Username		= "u";
			public const string Password		= "p";
			public const string Force			= "f";
			public const string Help            = "h";
			public const string Debug           = "d";
		}

		public abstract class ParamValues
		{
			public const string Update			= "update";
			public const string Uninstall		= "uninstall";
			public const string Cancel			= "cancel";
		}

		public abstract class Defaults
		{
			public const string AppServerIp     = "127.0.0.1";
			public const int AppServerPort		= 8120;
			public const string SshUsername		= "deploy"; 
			public const string SshPassword		= "metreos";
			public const int SshPort			= 22;
		}

		public abstract class ParamHelp
		{
            public const string AppName         = "-" + Params.AppName + ":<appname>";
			public const string AppFile         = "-" + Params.AppFile + ":<filename>";
			public const string AppServerIp     = "-" + Params.AppServerIp + ":<address>";
			public const string AppServerPort   = "-" + Params.AppServerPort + ":<port>";
			public const string Username		= "-" + Params.Username + ":<username>";
			public const string Password		= "-" + Params.Password + ":<password>";
			public const string Force			= "-" + Params.Force + ":<forceOption>";
			public const string Help			= "-" + Params.Help;
		}
	}
}
