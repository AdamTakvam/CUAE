using System;
using SBUtils;

namespace SSHServer.NET
{
	/// <summary>
	/// Provides high level interface to demo application functionality
	/// </summary>
	public class Globals
	{
		#region Public properties

		static public DemoSettings Settings 
		{
			get { return m_Settings; }
		}
		
		static public frmMain main 
		{
			get { return m_main; }
			set { m_main = value; }
		}
		
		static public bool ServerStarted 
		{
			get { return m_ServerStarted; }		
			set { m_ServerStarted = value; }
		}
		
		static ServerListener SSHListener 
		{
			get { return m_SSHListener; }
		}

		#endregion
		
		#region Public events

		public delegate void SessionStartedHandler(SSHSession session);
		public static event SessionStartedHandler SessionStarted;

		public delegate void SessionClosedHandler(SSHSession session);
		public static event SessionClosedHandler SessionClosed;

		public delegate void SessionInfoChangedHandler(SSHSession session);
		public static event SessionInfoChangedHandler SessionInfoChanged;

		#endregion

		#region Public methods

		static Globals()
		{
			m_Settings = new DemoSettings();
			m_Settings.LoadSettings();
		}
		
		public static void StartSSHListener()
		{
			SSHListener.SessionClosed += new SSHServer.NET.ServerListener.SessionClosedHandler(SSHListener_SessionClosed);
			SSHListener.SessionStarted += new SSHServer.NET.ServerListener.SessionStartedHandler(SSHListener_SessionStarted);
			SSHListener.SessionInfoChanged += new SSHServer.NET.ServerListener.SessionInfoChangedHandler(SSHListener_SessionInfoChanged);
			SSHListener.Start();
		}

		public static void StopSSHListener()
		{
			lock(SSHListener)
			{
				try
				{
					m_SSHListener.Dispose();
				}
				catch(Exception)
				{
				}
				m_SSHListener = new ServerListener();
			}
		}

		public static string AuthTypeToStr(int AuthType)
		{
			switch(AuthType)
			{
				case SBSSHConstants.Unit.SSH_AUTH_TYPE_RHOSTS : return "Rhosts";
				case SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY : return "PublicKey";
				case SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD : return "Password";
				case SBSSHConstants.Unit.SSH_AUTH_TYPE_HOSTBASED : return "Hostbased";
				case SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD : return "Keyboard-interactive";
				default :	return "Unknown";
			}

		}
		public Globals()
		{
			//
			// TODO: Add constructor logic here
			//
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("03351680203375C3F76A3A02DA03804A18AA4D01501D1B000D0236FC6B3467587853F832AD23607E9DA306B231791A3BB7AB9434ACC032D1F758BE7787A4D47D3CC6F483487CC7CA883DFB1546331A4917E479353316A2FE685D60A8E871AFBF7D60542640EC2D2C17BAC061E558C8205949F0115D627099E8685FF1914B2F69838A99EE9A782799842E2F5604845D5FE70307B6AA8F5EE06AC3CC8E036308E8C7B1695F9442911ED06541E9A4FD2E64D00C9B27E6F19FD28D1DCB0F58FC6A17C0711AAD241F3C46CA6C787CB195D18CCF477158416BA9E30444B4D6919A64AE08E2AC225AEA85CBA89034C4A8587227F5D2052184CAB8FC4F5695EE41707143"));
		}

		#endregion

		#region Event handlers

		private static void SSHListener_SessionClosed(SSHSession sender)
		{
            SessionClosed(sender);
		}

		private static void SSHListener_SessionStarted(SSHSession sender)
		{
            SessionStarted(sender);
		}

		private static void SSHListener_SessionInfoChanged(SSHSession sender)
		{
            SessionInfoChanged(sender);
		}

		#endregion

		#region Class members

		static private DemoSettings m_Settings = null;
		static private frmMain m_main = null;
		static private bool m_ServerStarted = false;
		static private ServerListener m_SSHListener = new ServerListener();

		#endregion

	}
}
