using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SBSSHCommon;
using SBSSHServer;
using SBSSHKeyStorage;
using SBSSHHandlers;
using SBStringList;
using SBUtils;
using SBSSHConnectionHandler;
using SBSftpHandler;

namespace SSHServer.NET
{
	/// <summary>
	/// Responsible for a single SSH session
	/// </summary>
	public class SSHSession : IDisposable
	{
		#region Public properties

		public object Data 
		{
			get { return m_Data; }
			set { m_Data = value; }
		}
		
		public string Status 
		{
			get { return m_Status; }
		}
		
		public string Username 
		{
			get { return m_Username; }
		}
		
		public string Host 
		{
			get { return m_Host; }
		}
		
		public string ClientSoftware 
		{
			get { return m_ClientSoftware; }
		}
		
		public DateTime StartTime 
		{
			get { return m_StartTime; }
		}

		#endregion

		#region Public events 

		public delegate void SessionClosedHandler(SSHSession sender);
		public event SessionClosedHandler SessionClosed;

		public delegate void SessionInfoChangedHandler(SSHSession sender);
		public event SessionInfoChangedHandler SessionInfoChanged;

		#endregion

		#region Public methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="socket">Socket object for accepted TCP connection</param>
		/// <param name="id">Session identifier</param>
		public SSHSession(Socket socket)
		{
			m_Socket = socket;
			try
			{
				m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, new LingerOption(true, 300));
			}
			catch(Exception exc) 
			{
				Logger.Log("SSHSession() : " + exc.Message);
			}

			m_Host = ((IPEndPoint)(socket.RemoteEndPoint)).Address.ToString() + ":" + ((IPEndPoint)(socket.RemoteEndPoint)).Port.ToString();
			m_StartTime = DateTime.Now;

			m_SocketSessionTimer = new System.Timers.Timer(); 
			m_SocketSessionTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnSocketSessionTimeout);
			m_SocketSessionTimer.AutoReset = true;				

			m_synch = new object();

			SetupServer();

			m_Event = new ManualResetEvent(false);
			m_Thread = new Thread(new ThreadStart(ReadThread));
			m_Thread.Name = "CFU_BinarySession_Thread";
			m_Thread.Start();
		}
		
		/// <summary>
		/// Starts session
		/// </summary>
		public void Start()
		{
			SessionInfoChanged(this);
			RestartSocketSessionTimer();
			m_Event.Set();			
		}

		/// <summary>
		/// Triggers SessionClosed event
		/// </summary>
		public void CloseSession()
		{
			if (this.SessionClosed != null) 
			{
				this.SessionClosed(this);
			}
		}

		#endregion

		#region SSH Server authentication processing
		
		/// <summary>
		/// Is fired when user performs an authentication attempt
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Username">User login name</param>
		/// <param name="AuthType">Used authentication type</param>
		/// <param name="Accept">Set to true, if user is allowed to perform this type of authentication</param>
		private void SSHServer_OnAuthAttempt(object Sender, string Username, int AuthType, ref bool Accept)
		{
			UserInfo user = null;
			if (Globals.Settings.FindUser(ref user, Username))
			{
				Accept = (user.AuthTypes | AuthType) > 0;
				m_Username = Username;
				m_ClientSoftware = m_SSHServer.ClientSoftwareName;
				SessionInfoChanged(this);
			} 
			else 
			{
				Accept = false;
			}
		}

		/// <summary>
		/// Is fired when user authentication attempt fails
		/// </summary>
		/// <param name="Sender">ElSSHServer objects</param>
		/// <param name="AuthenticationType">Authentication type that failed</param>
		private void SSHServer_OnAuthFailed(object Sender, int AuthenticationType)
		{
			Logger.Log("Authentication attempt (" + Globals.AuthTypeToStr(AuthenticationType) + ") failed", true);
		}

		/// <summary>
		/// Is fired when user tries password authentication
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Username">User login name</param>
		/// <param name="Password">User password</param>
		/// <param name="Accept">Set to true, if the provided password is valid</param>
		/// <param name="ForceChangePassword">Set to true to force user to change his password</param>
		private void SSHServer_OnAuthPassword(object Sender, string Username, string Password, ref bool Accept, ref bool ForceChangePassword) 
		{
			UserInfo user = null;
			if (Globals.Settings.FindUser(ref user, Username))
			{
				Accept = (user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD) > 0;
				Accept = Accept & user.PasswordValid(Password);
				if (Accept) 
				{
					int authFlag;
					if (m_authInfo[Username] == null) 
					{
                        authFlag = 0;
					} 
					else 
					{
                        authFlag = (int)m_authInfo[Username];
					}
					authFlag = authFlag | SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD;
					if ((user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD) > 0) 
					{
						authFlag = authFlag | SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD;
					}
					m_authInfo[Username] = authFlag;
				}
			} 
			else 
			{
				Accept = false;
			}
		}

		/// <summary>
		/// Is fired when user tries public key authentication
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Username">User login name</param>
		/// <param name="Key">User's public key</param>
		/// <param name="Accept">Set to true if the provided public key is valid</param>
		private void SSHServer_OnAuthPublicKey(object Sender, string Username, TElSSHKey Key, ref bool Accept)
		{
			UserInfo user = null;
			if (Globals.Settings.FindUser(ref user, Username))
			{
				Accept = (user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY) > 0;
				Accept = Accept & user.KeyValid(Key);
				if (Accept) 
				{
					int authFlag;
					if (m_authInfo[Username] == null) 
					{
						authFlag = 0;
					} 
					else 
					{
						authFlag = (int)m_authInfo[Username];
					}
					authFlag = authFlag | SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY;
					m_authInfo[Username] = authFlag;
				}
			} 
			else 
			{
				Accept = false;
			}
		}

		/// <summary>
		/// Is fired when user tries keyboard-interactive authentication
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Username">User login name</param>
		/// <param name="Submethods">Names of submethods that that the client wishes to use</param>
		/// <param name="Name">Set this to authentication title</param>
		/// <param name="Instruction">Set this to authentication instruction</param>
		/// <param name="Requests">Add the desired requests to this list</param>
		/// <param name="Echoes">Set the bits of this object depending on the corresponding responses should be echoed</param>
		private void SSHServer_OnAuthKeyboard(object Sender, string Username, TElStringList Submethods, 
			ref string Name, ref string Instruction, TElStringList Requests, TElBits Echoes)
		{
			UserInfo user = null;
			if ((Globals.Settings.FindUser(ref user, Username)) && ((user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD) > 0))
			{
                Name = "Keyboard-interactive authentication";
				Instruction = "Please enter the following information";
				Requests.Add("Username: ");
				Requests.Add("Password: ");
				Echoes.Size = 2;
				Echoes.set_Bits(0, true);
				Echoes.set_Bits(1, false);
			} 
		}

		/// <summary>
		/// Is fired when the keyboard-interactive response is received from client
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Requests">Requests list from the last keyboard-interactive request</param>
		/// <param name="Responses">User's responses</param>
		/// <param name="Name">Set this to next authentication stage title</param>
		/// <param name="Instruction">Set this to next authentication stage instructions</param>
		/// <param name="NewRequests">Add requests for next authentication stage to this list</param>
		/// <param name="Echoes">Set echo bits accordingly</param>
		/// <param name="Accept">Set to true if the responses are valid, or to false if the authentication process should be continued</param>
		private void SSHServer_OnAuthKeyboardResponse(object Sender, TElStringList Requests, TElStringList Responses, 
			ref string Name, ref string Instruction, TElStringList NewRequests, TElBits Echoes, ref bool Accept) 
		{
			Accept = false;
			if ((Responses != null) && (Responses.Count == 2)) 
			{
				string Username = Responses[0];
				string Password = Responses[1];
				UserInfo user = null;
				if (Globals.Settings.FindUser(ref user, Username))
				{
					Accept = (user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD) > 0;
					Accept = Accept & user.PasswordValid(Password);
					if (Accept) 
					{
						int authFlag;
						if (m_authInfo[Username] == null) 
						{
							authFlag = 0;
						} 
						else 
						{
							authFlag = (int)m_authInfo[Username];
						}
						authFlag = authFlag | SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD;
						if ((user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD) > 0) 
						{
							authFlag = authFlag | SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD;
						}
						m_authInfo[Username] = authFlag;
					}
				} 
			}
		}

		/// <summary>
		/// Queries if further client authentication is needed
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Username">User login name</param>
		/// <param name="Needed">Set to true if further authentication is needed, or to false if the authentication stage is completed</param>
		private void SSHServer_OnFurtherAuthNeeded(object Sender, string Username, ref bool Needed)
		{
			UserInfo user = null;
            Needed = true;			
			if (Globals.Settings.FindUser(ref user, Username)) {
				if (m_authInfo[Username] != null) 
				{
					int authFlag = (int)m_authInfo[Username];
					if ((user.AuthAll) && (authFlag == user.AuthTypes))
					{
						Needed = false;
					} 
					else if ((!user.AuthAll) && ((authFlag & user.AuthTypes) != 0))
					{
						Needed = false;
					}
				} 
			}
		}

		#endregion SSH Server authentication handling

		#region SSH Server socket-related processing

		/// <summary>
		/// Is fired when ElSSHServer has data to write to socket
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Buffer">Data to write to socket</param>
		private void SSHServer_OnSend(object Sender, byte[] Buffer)
		{
			try 
			{
				m_Socket.Send(Buffer);
			} 
			catch(Exception ex)
			{
                Logger.Log("Socket send operation failed: " + ex.Message);
				CloseSession();
			}
		}

		/// <summary>
		/// Is fired when ElSSHServer needs some data to be read from socket
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Buffer">Place where to put received data</param>
		/// <param name="MaxSize">Maximal amount of data to receive</param>
		/// <param name="Written">Number of bytes actually written</param>
		private void SSHServer_OnReceive(object Sender, ref byte[] Buffer, int MaxSize, out int Written)
		{
			try 
			{
				Written = m_Socket.Receive(Buffer, MaxSize, SocketFlags.None);
			} 
			catch(Exception ex) 
			{
				Written = 0;
                Logger.Log("Socket receive operation failed: " + ex.Message);
				CloseSession();
			}
		}
		
		#endregion

		#region SSH Server general-purpose event handlers

		/// <summary>
		/// Is fired when SSH session is closed
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		private void SSHServer_OnCloseConnection(object Sender)
		{
			CloseSession();
		}

		/// <summary>
		/// Is fired if some error occurs during SSH communication
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="ErrorCode">Error code</param>
		private void SSHServer_OnError(object Sender, int ErrorCode)
		{
			Logger.Log("Error #" + ErrorCode.ToString());
		}

		#endregion

		#region SSH Server connection-layer event handlers

		/// <summary>
		/// Is fired when a client requests SSH subsystem
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Connection">Logical connection object</param>
		/// <param name="Subsystem">Subsystem name</param>
		private void SSHServer_OnOpenSubsystem(object Sender, TElSSHTunnelConnection Connection, string Subsystem)
		{
			Logger.Log("Subsystem " + Subsystem + " opened");
			if (Subsystem == "sftp") 
			{
				SFTPSession sess = new SFTPSession(Connection);
				m_Status += "SFTP ";
				SessionInfoChanged(this);
			}
		}

		/// <summary>
		/// Is fired when a client requests shell
		/// </summary>
		/// <param name="Sender">ElSSHServer object</param>
		/// <param name="Connection">Logical connection object</param>
		private void SSHServer_OnOpenShell(object Sender, TElSSHTunnelConnection Connection)
		{
			Logger.Log("Shell requested");
			// System.Type tp = typeof(TElShellSSHSubsystemHandler);
			TElSSHSubsystemThread thread = new TElSSHSubsystemThread(new TElShellSSHSubsystemHandler(Connection, true), Connection, true);
			thread.Resume();
			m_Status += "Shell ";
			SessionInfoChanged(this);
		}

		#endregion

		#region Private methods
		/// <summary>
		/// Sets up server properties
		/// </summary>
		private void SetupServer()
		{
			lock (Globals.Settings)
			{
				try
				{
					TElSSHKey Key = new TElSSHKey();
					byte [] BServerKey = SBUtils.Unit.BytesOfString(Globals.Settings.ServerKey);
					int Result = Key.LoadPrivateKey(BServerKey,BServerKey.Length, "");
					if (Result == 0) 
					{
						m_HostKeys.Add(Key);
					}
				} 
				catch(Exception exc) 
				{
					Logger.Log("SSHSession.SetupServer - invalid private key" 
										  + exc.Message,true);
				}
			}
			m_SSHServer.KeyStorage = m_HostKeys;
			m_SSHServer.AllowedSubsystems.Add("sftp");
			m_SSHServer.SoftwareName = "SSHBlackbox(.NET)";
			m_SSHServer.ForceCompression = Globals.Settings.ForceCompression;
			m_SSHServer.OnAuthAttempt += new TSSHAuthAttemptEvent(SSHServer_OnAuthAttempt);
			m_SSHServer.OnAuthFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(SSHServer_OnAuthFailed);
			m_SSHServer.OnAuthPassword += new TSSHAuthPasswordEvent(SSHServer_OnAuthPassword);
			m_SSHServer.OnAuthPublicKey += new TSSHAuthPublicKeyEvent(SSHServer_OnAuthPublicKey);
			m_SSHServer.OnAuthKeyboard += new TSSHAuthKeyboardEvent(SSHServer_OnAuthKeyboard);
			m_SSHServer.OnAuthKeyboardResponse += new TSSHAuthKeyboardResponseEvent(SSHServer_OnAuthKeyboardResponse);
			m_SSHServer.OnFurtherAuthNeeded += new TSSHFurtherAuthNeededEvent(SSHServer_OnFurtherAuthNeeded);
			m_SSHServer.OnSend += new SBSSHCommon.TSSHSendEvent(SSHServer_OnSend);
			m_SSHServer.OnReceive += new SBSSHCommon.TSSHReceiveEvent(SSHServer_OnReceive);
			m_SSHServer.OnCloseConnection += new SBSSHCommon.TSSHCloseConnectionEvent(SSHServer_OnCloseConnection);
			m_SSHServer.OnError += new SBSSHCommon.TSSHErrorEvent(SSHServer_OnError);
			m_SSHServer.OnOpenSubsystem += new TSSHOpenSubsystemEvent(SSHServer_OnOpenSubsystem);
			m_SSHServer.OnOpenShell += new TSSHOpenShellEvent(SSHServer_OnOpenShell);
		}

		/// <summary>
		/// Session thread function
		/// </summary>
		private void ReadThread()
		{
			try
			{	
				m_SSHServer.Open();
				while(m_Event.WaitOne())
				{
					while (true)
					{
						Thread.Sleep(5);
						m_SSHServer.DataAvailable();
					}
				}
			}
			catch(Exception ex)
			{
				Logger.Log("ReadThread : " + ex.Message);
				CloseSession();
			}
		}

		private void RestartSocketSessionTimer()
		{
			m_SocketSessionTimer.Enabled = false;
			m_SocketSessionTimer.Interval = 5 * 60 * 1000;
			m_SocketSessionTimer.Enabled = true;
		}
		
		#endregion
		
		#region IDisposable Members

		public void Dispose()
		{
			if(m_Thread!=null)
			{			
			
				m_SocketSessionTimer.Enabled=false;
				m_SocketSessionTimer.Elapsed-=new System.Timers.ElapsedEventHandler(OnSocketSessionTimeout);	
				m_SocketSessionTimer.Close();

				m_Event.Reset();
				DoSocketShutdown();

				//kill thread
				m_Thread.Abort();
				m_Event.Close();						
				
				CloseSocket(); //close socket																
					
				m_Thread.Join();	
				m_Thread=null;											
			}	
		}

		private void DoSocketShutdown()
		{
			try
			{	
				m_Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);	//disable socket interchange
				Logger.Log("DoSocketShutdown()");

			}
			catch(Exception ex)
			{
				Logger.Log("DoSocketShutdown : " + ex.Message);
			}
		}

		private void CloseSocket()
		{
			if(m_Socket != null)
			{
				try
				{	
					m_Socket.Shutdown(SocketShutdown.Both);
					m_Socket.Close();							
					Logger.Log("Listening sockiet stopped");
				}
				catch(Exception ex)
				{
					Logger.Log("CloseSocket() : " + ex.Message,true);
				}
				m_Socket = null;		
			}
		}

		private void OnSocketSessionTimeout(object sender, System.Timers.ElapsedEventArgs e)
		{
			Logger.Log("Socket closed by timeout");
			CloseSession();
		}

		#endregion

		#region Class members

		private Socket m_Socket = null;
		private ManualResetEvent m_Event = null;
		private Thread m_Thread = null; 
		private object m_synch = null;
		private System.Timers.Timer m_SocketSessionTimer = null; 
		private Hashtable m_authInfo = new Hashtable();
		private TElSSHServer m_SSHServer = new TElSSHServer();
		private TElSSHMemoryKeyStorage m_HostKeys = new TElSSHMemoryKeyStorage();
		private object m_Data = null;
		private string m_Status = "";
		private string m_Username = "";
		private string m_Host = "";
		private string m_ClientSoftware = "";
		private DateTime m_StartTime;

		#endregion

	}
}
