/*******************************************************************
 * SSH Local Port Forwarding Demo application                      *
 * EldoS SecureBlackbox library                                    *
 * Copyright (C) 2002-2006                                         *
 *******************************************************************/

using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using SBSSHCommon;
using SBSSHClient;
using SBUtils;

namespace LocalPortForwarding
{

	public enum SshForwardingInState { Active, Closing, Closed };
	public enum SshForwardingOutState { Establishing, Active, Closing, Closed };

	/**
	 * class: SshSession
	 * 
	 * Responsible for a single SSH session.
	 * */
	class SshSession
	{
		private Socket			m_clientSocket;
		private Socket			m_serverSocket;
		private string			m_sshHost;
		private int				m_sshPort;
		private int				m_forwardPort;
		private string			m_forwardToHost;
		private int				m_forwardToPort;
		private string			m_username;
		private string			m_password;
		private TElSSHClient	m_sshClient;
		private TElLocalPortForwardSSHTunnel m_tunnel;
		private TElSSHTunnelList m_tunnelList;
		private Thread			m_clientThread;
		private Thread			m_serverThread;
		private bool			m_error;
		private object			m_GUILock = new object();
		private object			m_sshClientLock = new object();

		public SshSession() 
		{
			m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_clientSocket.Blocking = true;
			SetupComponents();
		}

		public void Connect()
		{
			Log("Connecting to " + m_sshHost + "...", false);
			m_clientThread = new Thread(new ThreadStart(ClientThreadProc));
			m_clientThread.Start();
		}

		public void Disconnect()
		{
            m_error = true;
		}

		private void ClientThreadProc() 
		{
			// establishing TCP connection
			Log("Resolving host " + m_sshHost + "...", false);
			IPEndPoint ip = new IPEndPoint(Dns.Resolve(m_sshHost).AddressList[0], m_sshPort);
			m_error = false;
			Log("Connecting to " + ip.Address + "...", false);
			m_clientSocket.Connect(ip);
			Log("Connected", false);
			// establishing SSH connection
			Log("Establishing SSH connection...", false);
			SetupSSHClient();
			m_sshClient.Open();
			while ((!m_error) && (!m_sshClient.Active) && (m_clientSocket.Connected)) 
			{
				Monitor.Enter(m_sshClientLock);
				try 
				{
					m_sshClient.DataAvailable();
				} 
				finally 
				{
                    Monitor.Exit(m_sshClientLock);
				}
			}
			// starting listening thread
			m_serverThread = new Thread(new ThreadStart(ServerThreadProc));
			m_serverThread.Start();
			while ((!m_error) && (m_clientSocket.Connected)) 
			{
				Monitor.Enter(m_sshClientLock);
				try 
				{
					m_sshClient.DataAvailable();
				} 
				finally 
				{
					Monitor.Exit(m_sshClientLock);
				}
			}
			Log("Finalizing...", false);
			if (m_sshClient.Active) 
			{
				m_sshClient.Close(true);
			}
			if (m_clientSocket.Connected) 
			{
				m_clientSocket.Shutdown(SocketShutdown.Both);
                m_clientSocket.Close();
			}
			if (m_serverThread != null) 
			{
				Log("Killing listening thread...", false);
				m_serverThread.Abort();
			}			
			if (m_serverSocket != null) {
				m_serverSocket.Close();
			}
			Log("Connection shutdown", false);
		}

		private void ServerThreadProc()
		{
			Socket acceptedSocket;
			m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), m_forwardPort));
			m_serverSocket.Listen(5);
			try 
			{
				while (!m_error) 
				{
					acceptedSocket = m_serverSocket.Accept();
					Log("Client connection accepted", false);
					SshForwarding f = new SshForwarding(acceptedSocket);
					f.OnFinish += new LocalPortForwarding.SshForwarding.ForwardingEvent(f_OnFinish);
					f.OnChange += new LocalPortForwarding.SshForwarding.ForwardingEvent(f_OnChange);
					f.OnLog += new LocalPortForwarding.SshForwarding.LogEvent(f_OnLog);
					f.OnDataSend += new LocalPortForwarding.SshForwarding.DataSendEvent(f_OnDataSend);
					f.OnClose += new LocalPortForwarding.SshForwarding.CloseEvent(f_OnClose);
					ConnectionAdd(f);
					Monitor.Enter(m_sshClientLock);
					try 
					{
						m_tunnel.Open(f);				
					} 
					finally 
					{
						Monitor.Exit(m_sshClientLock);
					}
				}
			} 
			finally 
			{
				m_serverSocket.Close();
			}
		}

		private void SetupComponents()
		{
			// setting up tunnels
            m_tunnel = new TElLocalPortForwardSSHTunnel();
			m_tunnel.OnClose += new TTunnelEvent(m_tunnel_OnClose);
			m_tunnel.OnError += new TTunnelErrorEvent(m_tunnel_OnError);
			m_tunnel.OnOpen += new TTunnelEvent(m_tunnel_OnOpen);
			m_tunnelList = new TElSSHTunnelList();
			m_tunnel.TunnelList = m_tunnelList;
			// setting up ssh client
			m_sshClient = new TElSSHClient();
			m_sshClient.OnAuthenticationFailed += new TSSHAuthenticationFailedEvent(m_sshClient_OnAuthenticationFailed);
			m_sshClient.OnAuthenticationSuccess += new TNotifyEvent(m_sshClient_OnAuthenticationSuccess);
			m_sshClient.OnCloseConnection += new TSSHCloseConnectionEvent(m_sshClient_OnCloseConnection);
			m_sshClient.OnError += new TSSHErrorEvent(m_sshClient_OnError);
			m_sshClient.OnKeyValidate += new TSSHKeyValidateEvent(m_sshClient_OnKeyValidate);
			m_sshClient.OnOpenConnection += new TSSHOpenConnectionEvent(m_sshClient_OnOpenConnection);
			m_sshClient.OnReceive += new TSSHReceiveEvent(m_sshClient_OnReceive);
			m_sshClient.OnSend += new TSSHSendEvent(m_sshClient_OnSend);
			m_sshClient.TunnelList = m_tunnelList;
		}

		private void SetupSSHClient()
		{
			m_sshClient.UserName = m_username;
			m_sshClient.Password = m_password;
            m_sshClient.AuthenticationTypes = SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD;
			m_tunnel.Port = m_forwardPort;
			m_tunnel.ToHost = m_forwardToHost;
			m_tunnel.ToPort = m_forwardToPort;
		}

		#region SshSession events and auxiliary event methods

		public delegate void LogEvent(object sender, string s, bool error);
		public delegate void ConnectionEvent(object sender, SshForwarding connection);
		public event LogEvent OnLog;
		public event ConnectionEvent OnConnectionAdd;
		public event ConnectionEvent OnConnectionRemove;
		public event ConnectionEvent OnConnectionChange;

		private void Log(string s, bool error) 
		{
			Monitor.Enter(m_GUILock);
			try 
			{
				if (OnLog != null) 
				{
					OnLog(this, s, error);
				}
			} 
			finally 
			{
				Monitor.Exit(m_GUILock);
			}
		}

		private void ConnectionAdd(SshForwarding conn)
		{
			Monitor.Enter(m_GUILock);
			try 
			{
				if (OnConnectionAdd != null) 
				{
					OnConnectionAdd(this, conn);
				}
			} 
			finally 
			{
                Monitor.Exit(m_GUILock);
			}
		}

		private void ConnectionRemove(SshForwarding conn)
		{
			Monitor.Enter(m_GUILock);
			try 
			{
				if (OnConnectionRemove != null) 
				{
					OnConnectionRemove(this, conn);
				}
			} 
			finally 
			{
				Monitor.Exit(m_GUILock);
			}
		}

		private void ConnectionChange(SshForwarding conn)
		{
			Monitor.Enter(m_GUILock);
			try 
			{
				if (OnConnectionChange != null) 
				{
					OnConnectionChange(this, conn);
				}
			} 
			finally 
			{
				Monitor.Exit(m_GUILock);
			}
		}

		#endregion

		#region Public properties

		public TElSSHClient SshClient 
		{
			get { return m_sshClient; }
		}

		public string SshHost 
		{
			get { return m_sshHost; }
			set { m_sshHost = value; }
		}

		public int SshPort 
		{
			get { return m_sshPort; }
			set { m_sshPort = value; }
		}
		
		public int ForwardPort 
		{
			get { return m_forwardPort; }
			set { m_forwardPort = value; }
		}
		
		public string ForwardToHost 
		{
			get { return m_forwardToHost; }
			set { m_forwardToHost = value; }
		}
		
		public int ForwardToPort 
		{
			get { return m_forwardToPort; }
			set { m_forwardToPort = value; }
		}

		public string Username
		{
			get { return m_username; }
			set { m_username = value; }
		}

		public string Password
		{
			get { return m_password; }
			set { m_password = value; }
		}

		#endregion

		#region m_sshClient event handlers

		private void m_sshClient_OnAuthenticationFailed(object Sender, int AuthenticationType)
		{
            Log("Authentication " + AuthenticationType.ToString() + " failed", true);
		}

		private void m_sshClient_OnAuthenticationSuccess(object Sender)
		{
            Log("Authentication succeeded", false);
		}

		private void m_sshClient_OnCloseConnection(object Sender)
		{
            Log("SSH connection closed", false);
			m_error = true;
		}

		private void m_sshClient_OnError(object Sender, int ErrorCode)
		{
			Log("SSH protocol error " + ErrorCode.ToString(), true);
			m_error = true;
		}

		private void m_sshClient_OnKeyValidate(object Sender, SBSSHKeyStorage.TElSSHKey ServerKey, ref bool Validate)
		{
			Log("Server key received", false);
			Validate = true;
		}

		private void m_sshClient_OnOpenConnection(object Sender)
		{
            Log("SSH connection established", false);
		}

		private void m_sshClient_OnReceive(object Sender, ref byte[] Buffer, int MaxSize, out int Written)
		{
			if (!m_clientSocket.Connected) 
			{
                Written = 0;
				return;
			}
			ArrayList socketList = new ArrayList();
			socketList.Add(m_clientSocket);
			Socket.Select(socketList, null, null, 1000);
			if (socketList.Count > 0)
			{
				Written = m_clientSocket.Receive(Buffer, 0, MaxSize, SocketFlags.None);
				if (Written == 0) 
				{
                    m_error = true;
				}
			} 
			else 
			{
				Thread.Sleep(0);
				Written = 0;
			}
		}

		private void m_sshClient_OnSend(object Sender, byte[] Buffer)
		{
			if (m_clientSocket.Connected) 
			{
				m_clientSocket.Send(Buffer, 0, Buffer.Length, SocketFlags.None);
			} 
		}

		#endregion

		#region m_tunnel event handlers

		private void m_tunnel_OnClose(object Sender, TElSSHTunnelConnection TunnelConnection)
		{
            Log("Secure channel closed", false);
			SshForwarding fwd = (SshForwarding)TunnelConnection.Data;
			fwd.Close();
		}

		private void m_tunnel_OnError(object Sender, int Error, object Data)
		{
			Log("Failed to open secure channel, error " + Error.ToString(), true);
			SshForwarding fwd = (SshForwarding)Data;
			fwd.Close();
		}

		private void m_tunnel_OnOpen(object Sender, TElSSHTunnelConnection TunnelConnection)
		{
			SshForwarding fwd = (SshForwarding)TunnelConnection.Data;
			Log("Secure channel opened", false);
			fwd.SshConnection = TunnelConnection;
		}

		#endregion

		#region SshForwarding event handlers

		private void f_OnFinish(object sender)
		{
			ConnectionRemove((SshForwarding)sender);
		}

		private void f_OnChange(object sender)
		{
            ConnectionChange((SshForwarding)sender);
		}

		private void f_OnLog(object sender, string s, bool error)
		{
			Log(s, error);
		}

		private void f_OnDataSend(object sender, byte[] buffer, int offset, int len)
		{
			Monitor.Enter(m_sshClientLock);
			try 
			{
				SshForwarding f = (SshForwarding)sender;
				f.SshConnection.SendData(buffer, offset, len);
			} 
			finally 
			{
				Monitor.Exit(m_sshClientLock);
			}
		}

		private void f_OnClose(object sender)
		{
			Monitor.Enter(m_sshClientLock);
			try 
			{
				SshForwarding f = (SshForwarding)sender;
				f.SshConnection.Close(true);
			} 
			finally 
			{
				Monitor.Exit(m_sshClientLock);
			}
		}

		#endregion

	}

	/**
	 * class: SshForwarding
	 * 
	 * Responsible for a single forwarded connection.
	 * */
	class SshForwarding
	{
		TElSSHTunnelConnection	m_sshConnection;
		Socket					m_socket;
		byte[]					m_socketToChannel = new byte[0];
		byte[]					m_channelToSocket = new byte[0];
		bool					m_error;
		Thread					m_forwardingLoop;
		private					SshForwardingInState m_inState;
		private					SshForwardingOutState m_outState;
		private string			m_host;
		private int				m_received = 0;
		private int				m_sent = 0;
		object					m_channelLock = new object();
		object					m_socketLock = new object();

		public SshForwarding(Socket socket) 
		{
			m_socket = socket;
			m_host = socket.RemoteEndPoint.ToString();
            m_sshConnection = null;
			m_error = false;
			m_inState = SshForwardingInState.Active;
			m_outState = SshForwardingOutState.Establishing;
			m_forwardingLoop = new Thread(new ThreadStart(ForwardingThreadFunc));
            m_forwardingLoop.Start();			
		}

		void ForwardingThreadFunc() 
		{
			byte[] buf = new byte[8192];
			int len, index, sent;
			ArrayList socketList = new ArrayList();

			while ((!m_error) && (!((m_inState == SshForwardingInState.Closed) && (m_outState == SshForwardingOutState.Closed)))) 
			{
				bool changed = false;
				// socket operations
				if ((m_socket.Connected) && (m_inState == SshForwardingInState.Active))
				{
					// reading data from socket
					try 
					{
						socketList.Clear();
						socketList.Add(m_socket);
						len = 0;
						Socket.Select(socketList, null, null, 1000);
						if (socketList.Count > 0) 
						{
							len = m_socket.Receive(buf, 0, buf.Length, SocketFlags.None);
							if (len > 0) 
							{
								WriteToChannelBuffer(buf, 0, len);
							} 
							else 
							{
								m_inState = SshForwardingInState.Closed;
							}
							changed = true;
						} 
						else 
						{
                            Thread.Sleep(0);
						}
						// writing pending data to socket
						bool received;
						do 
						{
							len = ReadFromSocketBuffer(buf, 0, buf.Length);
							received = (len > 0);
							index = 0;
							while (len > 0) 
							{
								sent = m_socket.Send(buf, index, len, SocketFlags.None);
								index += sent;
								len -= sent;
							}
							if (received) 
							{
                                changed = true;
							}
						} while (received);
					} 
					catch(Exception ex) 
					{
						Log(ex.Message, true);
						m_inState = SshForwardingInState.Closing;
						changed = true;
					}
				} 
				else if (!m_socket.Connected)
				{
					m_inState = SshForwardingInState.Closed;
					changed = true;
				}
				// channel operations
				if (m_sshConnection != null) 
				{
					do 
					{
						len = ReadFromChannelBuffer(buf, 0, buf.Length);
						if (len > 0) 
						{
							OnDataSend(this, buf, 0, len);
							m_sent += len;
							changed = true;
						}
					} while (len > 0);
				}						
				// re-adjusting states
				if ((m_inState == SshForwardingInState.Active) && ((m_outState == SshForwardingOutState.Closed) || (m_outState == SshForwardingOutState.Closing))) 
				{
					m_inState = SshForwardingInState.Closing;
					m_socket.Shutdown(SocketShutdown.Both);
					m_socket.Close();
					changed = true;
				} 
				else if (((m_outState == SshForwardingOutState.Active)) && ((m_inState == SshForwardingInState.Closing) || (m_inState == SshForwardingInState.Closed))) 
				{
					m_outState = SshForwardingOutState.Closing;
					OnClose(this);
					changed = true;
				} 
				else if ((m_inState == SshForwardingInState.Closing) && (!(m_socket.Connected)))
				{
                    m_inState = SshForwardingInState.Closed;
					changed = true;
				}
				if (changed) 
				{
					OnChange(this);
				}
				Thread.Sleep(0);
			}
			if (m_socket.Connected) 
			{
				m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Close();
			}
			OnFinish(this);
		}

		public void Close() 
		{
            m_outState = SshForwardingOutState.Closed;
		}

		private void SetupConnection() 
		{
            m_sshConnection.OnClose += new TSSHChannelCloseEvent(m_sshConnection_OnClose);
			m_sshConnection.OnData += new TSSHDataEvent(m_sshConnection_OnData);
		}

		#region Public properties

		public TElSSHTunnelConnection SshConnection 
		{
			get { return m_sshConnection; }
			set { 
				m_sshConnection = value; 
				SetupConnection();
				m_outState = SshForwardingOutState.Active;
				OnChange(this);
			}
		}

		public SshForwardingInState InState 
		{
			get { return m_inState; }
		}

		public SshForwardingOutState OutState 
		{
			get { return m_outState; }
		}

		public string Host 
		{
			get { return m_host; }
		}

		public int Received
		{
			get { return m_received; }
		}

		public int Sent 
		{
			get { return m_sent; }
		}

		#endregion

		#region Thread-safe buffer access routines

		private void WriteToChannelBuffer(byte[] buffer, int offset, int length) 
		{
			Monitor.Enter(m_channelLock);
			try 
			{
				byte[] newBuf = new byte[m_socketToChannel.Length + length];
				Array.Copy(m_socketToChannel, 0, newBuf, 0, m_socketToChannel.Length);
				Array.Copy(buffer, 0, newBuf, m_socketToChannel.Length, length);
				m_socketToChannel = newBuf;
			} 
			finally 
			{
				Monitor.Exit(m_channelLock);
			}
		}

		private void WriteToSocketBuffer(byte[] buffer, int offset, int length)
		{
			Monitor.Enter(m_socketLock);
			try 
			{
				byte[] newBuf = new byte[m_channelToSocket.Length + length];
				Array.Copy(m_channelToSocket, 0, newBuf, 0, m_channelToSocket.Length);
				Array.Copy(buffer, 0, newBuf, m_channelToSocket.Length, length);
				m_channelToSocket = newBuf;
			} 
			finally 
			{
                Monitor.Exit(m_socketLock);
			}
		}

		private int ReadFromChannelBuffer(byte[] buffer, int offset, int length)
		{
			int read = 0;
			Monitor.Enter(m_channelLock);
			try 
			{
				read = Math.Min(length, m_socketToChannel.Length);
				Array.Copy(m_socketToChannel, 0, buffer, offset, read);
				byte[] newBuf = new byte[m_socketToChannel.Length - read];
				Array.Copy(m_socketToChannel, read, newBuf, 0, newBuf.Length);
				m_socketToChannel = newBuf;
			} 
			finally 
			{
				Monitor.Exit(m_channelLock);
			}
			return read;
		}

		private int ReadFromSocketBuffer(byte[] buffer, int offset, int length)
		{
			int read = 0;
			Monitor.Enter(m_socketLock);
			try 
			{
				read = Math.Min(length, m_channelToSocket.Length);
				Array.Copy(m_channelToSocket, 0, buffer, offset, read);
				byte[] newBuf = new byte[m_channelToSocket.Length - read];
				Array.Copy(m_channelToSocket, read, newBuf, 0, newBuf.Length);
				m_channelToSocket = newBuf;
			} 
			finally 
			{
				Monitor.Exit(m_socketLock);
			}
			return read;
		}

		#endregion

		#region m_sshConnection event handlers

		private void m_sshConnection_OnClose(object Sender, TSSHCloseType CloseType)
		{
            m_outState = SshForwardingOutState.Closed;
		}

		private void m_sshConnection_OnData(object Sender, byte[] Buffer)
		{
			m_received += Buffer.Length;
            WriteToSocketBuffer(Buffer, 0, Buffer.Length);
		}

		#endregion

		#region Events and auxiliary event methods

		public delegate void ForwardingEvent(object sender);
		public delegate void LogEvent(object sender, string s, bool error);
		public delegate void DataSendEvent(object sender, byte[] buffer, int offset, int len);
		public delegate void CloseEvent(object sender);
		public event ForwardingEvent OnFinish;
		public event ForwardingEvent OnChange;
		public event LogEvent OnLog;
		public event DataSendEvent OnDataSend;
		public event CloseEvent OnClose;
		private void Log(string s, bool error)
		{
			OnLog(this, s, error);
		}

		#endregion
	}
}
