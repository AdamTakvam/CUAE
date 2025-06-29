using System;
using System.IO;
using System.Collections;
using SBSSHKeyStorage;

namespace SSHServer.NET
{
	/// <summary>
	/// Stores and provides interface to SSH server demo configuration
	/// </summary>
	public class DemoSettings
	{
		public DemoSettings()
		{
			// empty constructor
		}

		#region Public properties

		/// <summary>
		/// Indicates if demo settings are loaded
		/// </summary>
		public bool Empty 
		{
			get { return m_Empty; }
		}

		/// <summary>
		/// Stores information about registered users
		/// </summary>
		public ArrayList Users 
		{
			get { return m_Users; }
		}

		/// <summary>
		/// Private key of the host
		/// </summary>
		public string ServerKey 
		{
			get { return m_ServerKey; }
			set { m_ServerKey = value; }
		}

		/// <summary>
		/// Server socket address
		/// </summary>
		public string ServerHost 
		{
			get { return m_ServerHost; }
			set { m_ServerHost = value; }
		}

		/// <summary>
		/// Server port
		/// </summary>
		public int ServerPort 
		{
			get { return m_ServerPort; }
			set { m_ServerPort = value; }
		}

		/// <summary>
		/// Compression flag
		/// </summary>
		public bool ForceCompression 
		{
			get { return m_ForceCompression; }
			set { m_ForceCompression = value; }
		}
		
		#endregion
		
		#region Public methods
		/// <summary>
		/// Searches for user in the registered user list
		/// </summary>
		/// <param name="user">search result</param>
		/// <param name="UserName">user login name</param>
		/// <returns>true if the user with UserName account was found in the list, false otherwise</returns>
		public bool FindUser(ref UserInfo user, string UserName)
		{
			if (UserName == null) return false;
			for (int i = 0; i < Users.Count; i++) 
			{
				if (UserName.Equals(((UserInfo)Users[i]).Name)) 
				{
					user = (UserInfo)Users[i];
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds new user information record to the list
		/// </summary>
		/// <returns>true on success</returns>
		public bool AddUser()
		{
			Users.Add(new UserInfo());
			return true;
		}

		/// <summary>
		/// Removes user information record from the list
		/// </summary>
		/// <param name="UserNumber">record index in the list</param>
		/// <returns>true on success, false otherwise</returns>
		public bool RemoveUser(int UserNumber)
		{
			try
			{
				Users.RemoveAt(UserNumber);
				return true;
			}
			catch(Exception) 
			{
				return(false);
			}
		}

		/// <summary>
		/// displays user information edit window
		/// </summary>
		/// <param name="Number">index of user information record in the list</param>
		/// <returns>true on success, false otherwise</returns>
		public bool EditUser(int Number)
		{
			if (Number < 0) 
			{
				return false;
			}
			UserInfo info = (UserInfo)Users[Number];
			bool Result = frmUser.EditUserParameters(ref info);
			if (Result) 
			{
				SaveSettings();
			}
			return Result;
		}

		/// <summary>
		/// Sets the new server host key
		/// </summary>
		/// <param name="HostKey">private host key string</param>
		/// <returns>true if the key was set successfully, false otherwise</returns>
		public bool SetHostKey(string HostKey)
		{
			try
			{
				TElSSHKey Key = new TElSSHKey();
				int Result = Key.LoadPrivateKey(SBUtils.Unit.BytesOfString(HostKey),HostKey.Length,"");
				return Result == 0;
			} 
			catch(Exception exc) 
			{
				Logger.Log("SetHostKey : " + exc.Message,true);
			}
			return false;
		}
		
		/// <summary>
		/// Loads demo settings from configuration files
		/// </summary>
		/// <returns>true on success</returns>
		public bool LoadSettings()
		{
			try
			{
				string AppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				AppPath = AppPath + "\\SecureBlackbox\\";
				if (!Directory.Exists(AppPath)) Directory.CreateDirectory(AppPath);
				m_UsersFile = AppPath + "sshsrvusers";
				m_ServerKeyFile = AppPath + "SSHSrvHostkey";
				m_IPFile = AppPath + "sshbindings";
				if ((!File.Exists(m_UsersFile)) || (!File.Exists(m_ServerKeyFile)))
					m_Empty = true;
				else
					m_Empty = false;
				// Read user information
				StreamReader sr = new StreamReader(m_UsersFile);
				string line = null;
				while ((line = sr.ReadLine()) != null)
				{
					if (line.Length > 0) 
					{
						UserInfo ui = new UserInfo(line);
						Users.Add(ui);
					}
				}
				sr.Close();
				// Read server public key
				try
				{
					sr = new StreamReader(m_ServerKeyFile);
					string AServerKey = sr.ReadToEnd();
					sr.Close();
					TElSSHKey Key = new TElSSHKey();
					int Result = Key.LoadPrivateKey(m_ServerKeyFile, "");
					if (Result == 0) 
						m_ServerKey = AServerKey;
				}
				catch(Exception exc) 
				{
					Logger.Log("Unable to load server key " + exc.Message, true);
					return false;
				}
				// Read IP address
				try
				{
					sr = new StreamReader(m_IPFile);
					string AServerHost = sr.ReadToEnd();
					sr.Close();
					string[] par = AServerHost.Split(':');
					m_ServerHost = par[0];
					m_ServerPort = int.Parse(par[1]);
				}
				catch(Exception exc) 
				{
					Logger.Log("Unable to read host:posrt " + exc.Message, true);
					m_ServerHost = "127.0.0.1";
					m_ServerPort = 22;
				}
				return !Empty;
			}
			catch(Exception exc) 
			{
				Logger.Log("Error reading settings " + exc.Message,true);
				return false;
			}
		}

		/// <summary>
		/// Saves server configuration settings to files
		/// </summary>
		public void SaveSettings()
		{
			try
			{
				// Users
				StreamWriter sw = new StreamWriter(m_UsersFile, false);
				foreach (UserInfo user in Users)
				{
					sw.WriteLine(user.ToString());
				}
				sw.Close();
				// Server private key
				sw = new StreamWriter(m_ServerKeyFile, false);
				sw.Write(ServerKey);
				sw.Close();
				// Server ip bindings
				sw = new StreamWriter(m_IPFile, false);
				sw.Write(ServerHost + ":" + ServerPort.ToString());
				sw.Close();
			} 
			catch(Exception exc) 
			{
				Logger.Log("SaveSettings : " + exc.Message,true);
			}
		}

		#endregion

		#region Class members

		// User information file
		private string m_UsersFile = "";
		// Server private key file
		private string m_ServerKeyFile = "";
		// Network settings file
		private string m_IPFile = "";
		// Server configuration
		private bool m_Empty = true;
		private ArrayList m_Users = new ArrayList();
		private string m_ServerKey = "";
		private string m_ServerHost = "127.0.0.1";
		private int m_ServerPort = 22;
		private bool m_ForceCompression = false;

		#endregion
	}
}
