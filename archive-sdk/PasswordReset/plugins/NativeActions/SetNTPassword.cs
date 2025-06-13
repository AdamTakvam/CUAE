using System;
using System.Diagnostics;
using System.DirectoryServices;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Native.ActiveRelay
{
	/// <summary>
	/// Sets the password for the specified local WinNT user account
	/// </summary>
	[PackageDecl("Metreos.SDK.PasswordReset")]
	public class SetPassword : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("Username", true)]
		public string Username { set { username = value; } }
		private string username;

		[ActionParamField("NewPassword", true)]
		public string NewPassword { set { newPassword = value; } }
		private string newPassword;

		public SetPassword()
		{
			Clear();
		}
 
		public void Clear()
		{
			username            = null;
			newPassword			= null;
		}

		public bool ValidateInput()
		{
			return ((username != null) && (newPassword != null));
		}
 
		[Action("SetPassword", false, "Sets the password for the specified local WinNT user account", "Sets the password for the specified local WinNT user account")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
			bool success = true;
			try
			{
				DirectoryEntry entry = new DirectoryEntry("WinNT://" + Environment.MachineName + "/" + username);
				entry.Invoke("SetPassword", new object[] { newPassword });
			}
			catch (Exception e)
			{
				success = false;
				log.Write(TraceLevel.Warning, "Exception caught in SetNTPassword::Execute (Exception follows)\n" + e.Message);
			}

			return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
		}
	}
}