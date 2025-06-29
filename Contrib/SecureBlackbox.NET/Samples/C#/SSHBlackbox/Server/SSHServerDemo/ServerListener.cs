using System;
using System.Threading;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using SBSSHKeyStorage;

namespace SSHServer.NET
{
	/// <summary>
	/// Responsible for listening for incoming connections and processing accepted ones
	/// </summary>
	internal class ServerListener : IDisposable
	{
		public ServerListener()
		{
			m_synch = new object();
			m_arrSessions = new ArrayList();
			m_Event = new ManualResetEvent(false);
			m_Thread = new Thread(new ThreadStart(Listen));
			m_Thread.Name = "SSH_Server_Listener_Thread";
		}				

		#region Public events

		public delegate void SessionStartedHandler(SSHSession sender);
		public event SessionStartedHandler SessionStarted;

		public delegate void SessionClosedHandler(SSHSession sender);
		public event SessionClosedHandler SessionClosed;

		public delegate void SessionInfoChangedHandler(SSHSession sender);
		public event SessionInfoChangedHandler SessionInfoChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(m_Thread!=null)
			{		
				m_Event.Reset();
				Stop();

				//kill thread
				m_Thread.Abort();
				m_Thread.Join(1000);												
				m_Event.Close();

				//kill sockets (sessions)
				lock(m_synch)
				{				
					for(int i=0,N=m_arrSessions.Count;i<N;++i)
					{//stop all session
						SSHSession session=(SSHSession)m_arrSessions[i];
						System.Diagnostics.Debug.Assert(session!=null);
						session.SessionClosed-=new SSHSession.SessionClosedHandler(OnSessionClosed);
						session.Dispose();
					}
					m_arrSessions.Clear();
				}
				m_Thread=null;
			}
		}

		#endregion
	
		#region Public methods
		/// <summary>
		/// Starts listening for incoming connections
		/// </summary>
		/// <returns>true, if the listening socket was successfully allocated</returns>
		public bool Start()
		{
			try
			{			
				Logger.Log("Starting SSH server listener...");
				m_SocketListener = new TcpListener(IPAddress.Parse(Globals.Settings.ServerHost),
					Globals.Settings.ServerPort);
				m_SocketListener.Start();	
				m_Thread.Start();	
				m_Event.Set();
				if (Globals.main != null)
				{
					lock(Globals.main)
					{
						Globals.main.tbTop.Buttons[0].Enabled = false;
						Globals.main.tbTop.Buttons[1].Enabled = true;
					}
				}
				Globals.ServerStarted = true;
				Logger.Log("SSH server listener started.");
				return true;
			}
			catch(Exception exc)
			{
				Logger.Log("ServerListener.Start : " + exc.Message,true);
				return false;
			}
		}

		/// <summary>
		/// Stops listening for incoming connections
		/// </summary>
		public void Stop()
		{
			try
			{
				//stop listener
				Logger.Log("Stopping socket listener");	
				m_SocketListener.Stop();
				if (Globals.main != null)
				{
					lock(Globals.main)
					{
						Globals.main.tbTop.Buttons[0].Enabled = true;
						Globals.main.tbTop.Buttons[1].Enabled = false;
					}
				}
				Globals.ServerStarted = true;
				Logger.Log("Socket listener stopped");	
			}
			catch(Exception ex)
			{
				Logger.Log("Exception while stopping socket listener : " + ex.Message);
			}
		}

		#endregion

		#region Private methods

		private Socket AcceptSocket()
		{
			Socket socket = null;
			try
			{
				socket = m_SocketListener.AcceptSocket();
			}
			catch(Exception ex)
			{
				Logger.Log("AcceptSocket() : " + ex.Message,true);
			}
			return socket;
		}

		private void Listen()
		{	
			while(m_Event.WaitOne())
			{//listen server socket				
				Socket socket = AcceptSocket();			
				if(socket != null)
				{				
					Logger.Log("ServerListener.Listen : New connection available");
					lock(m_synch)
					{					
						if(m_arrSessions.Count < m_MaxSocketSessions)
						{
							//create new session and start it in a new thread							
							SSHSession session = new SSHSession(socket);
							session.SessionClosed += new SSHSession.SessionClosedHandler(OnSessionClosed);	
							session.SessionInfoChanged += new SSHServer.NET.SSHSession.SessionInfoChangedHandler(OnSessionInfoChanged);
							m_arrSessions.Add(session);
							Logger.Log("Connection accepted. Active connections "  + m_arrSessions.Count.ToString() + " from " + m_MaxSocketSessions.ToString());
							session.Start();
							this.SessionStarted(session);
						}
						else
						{	
							Logger.Log("New connection rejected");
							try
							{						
								socket.Shutdown(SocketShutdown.Both);
								socket.Close();
							}
							catch(Exception)
							{
								;
							}						
						}
					}
				}
			}
		}

		#endregion

		#region Event handlers

		private void OnSessionClosed(SSHSession sender)
		{				
			lock(m_synch)
			{	
				m_arrSessions.Remove(sender);
				Logger.Log("ServerListener.OnSessionClosed()");			
			}
			if (this.SessionClosed != null) 
			{
                this.SessionClosed(sender);
			}
			sender.SessionClosed -= new SSHSession.SessionClosedHandler(OnSessionClosed);					
			sender.SessionInfoChanged -= new SSHSession.SessionInfoChangedHandler(OnSessionInfoChanged);
			sender.Dispose();			
		}

		private void OnSessionInfoChanged(SSHSession sender)
		{
            this.SessionInfoChanged(sender);
		}

		#endregion

		#region Class members
		
		private object m_synch = null;
		private ArrayList m_arrSessions = null; 	
		private TcpListener m_SocketListener = null;
		private ManualResetEvent m_Event = null;
		private Thread m_Thread = null;
		private int m_MaxSocketSessions = 20;
		
		#endregion

	}
}
