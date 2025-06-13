using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Metreos.Utilities;
using Metreos.LoggingFramework;

using SBSSHCommon;
using SBSSHServer;
using SBSSHKeyStorage;
using SBSSHHandlers;
using SBStringList;
using SBUtils;
using SBSSHConnectionHandler;
using SBSftpHandler;

namespace Metreos.SftpServer
{
	/// <summary>Handles a single SSH session</summary>
	public class SSHSession : IDisposable
	{
        #region Public events 

        public delegate void SessionClosedHandler(SSHSession sender);
        public event SessionClosedHandler SessionClosed;

        #endregion

        #region Constants

        private abstract class Consts
        {
            public const string SftpSubsystem       = "sftp";
            public const string SoftwareName        = "SSHBlackbox(.NET)";
            public const bool ForceCompression      = false;
        }

        #endregion

        private readonly DBHelper config;
        private readonly LogWriter log;

        private readonly Socket socket;
        private readonly Thread readThread; 
        private readonly TElSSHServer sshServer;
        private readonly TElSSHKey keyUtil;
        private readonly TElSSHMemoryKeyStorage hostKeys;
   
        private SFTPSession sftpSession;
        private volatile bool shutdown = false;

        public IPEndPoint RemoteEP { get { return remoteEP; } }
        private readonly IPEndPoint remoteEP;

        public DirectoryInfo HomeDirectory { get { return homeDir; } }
        private DirectoryInfo homeDir;
        
        public string ClientSoftware 
        {
            get { return sshServer.ClientSoftwareName; }
        }

        public SSHSession(Socket socket, DBHelper config, byte[] sshKey, LogWriter log)
		{
            Assertion.Check(socket != null, "Cannot create SSH session with null socket");
            Assertion.Check(config != null, "Cannot create SSH session with null DBHelper");

            this.socket = socket;
            this.config = config;
            this.log = log;

            this.sshServer = new TElSSHServer();
            this.keyUtil = new TElSSHKey();
            this.hostKeys = new TElSSHMemoryKeyStorage();

            try
            {
                this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, new LingerOption(true, 300));
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Failed to set socket option: " + e.Message);
            }

            this.remoteEP = (IPEndPoint)socket.RemoteEndPoint;

            SetupServer(sshKey);

            this.readThread = new Thread(new ThreadStart(ReadThread));
            this.readThread.IsBackground = true;
            this.readThread.Name = "Socket Read Thread: " + remoteEP;
            this.readThread.Start();
        }

        #region Socket read thread

        /// <summary>Socket read thread</summary>
        private void ReadThread()
        {
            sshServer.Open();

            try
            {
                while(!shutdown)
                {
                    Thread.Sleep(1);
                    sshServer.DataAvailable();
                }
            }
            catch(ThreadAbortException) {}
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Socket read error:\n" + e);
            }

            CloseSession();
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
        private void SSHServer_OnAuthAttempt(object sender, string username, int authType, ref bool accept)
        {
            log.Write(TraceLevel.Verbose, "Incoming connection from {0}: username={1}, authType={2}",
                this.remoteEP, username, AuthTypeToStr(authType));

            accept = true;
        }

        /// <summary>
        /// Is fired when user authentication attempt fails
        /// </summary>
        /// <param name="Sender">ElSSHServer objects</param>
        /// <param name="AuthenticationType">Authentication type that failed</param>
        private void SSHServer_OnAuthFailed(object sender, int authType)
        {
            log.Write(TraceLevel.Warning, "Authentication attempt from {0} failed", remoteEP);
        }

        /// <summary>
        /// Is fired when user tries password authentication
        /// </summary>
        /// <param name="Sender">ElSSHServer object</param>
        /// <param name="Username">User login name</param>
        /// <param name="Password">User password</param>
        /// <param name="Accept">Set to true, if the provided password is valid</param>
        /// <param name="ForceChangePassword">Set to true to force user to change his password</param>
        private void SSHServer_OnAuthPassword(object sender, string username, string password, 
            ref bool accept, ref bool forceChangePassword) 
        {
            accept = Authorize(username, password);
            forceChangePassword = false;

            log.Write(TraceLevel.Info, "Password authentication from {0} ({1}) {2}", 
                remoteEP, username, accept ? "accepted." : "denied.");
        }

        /// <summary>
        /// Is fired when user tries public key authentication
        /// </summary>
        /// <param name="Sender">ElSSHServer object</param>
        /// <param name="Username">User login name</param>
        /// <param name="Key">User's public key</param>
        /// <param name="Accept">Set to true if the provided public key is valid</param>
        private void SSHServer_OnAuthPublicKey(object sender, string username, TElSSHKey key, ref bool accept)
        {
            log.Write(TraceLevel.Verbose, "Public key authentication from {0} denied: Mode not supported.", remoteEP);
            accept = false;
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
        private void SSHServer_OnAuthKeyboard(object sender, string username, TElStringList submethods, 
            ref string name, ref string instruction, TElStringList requests, TElBits echoes)
        {
            name = "Keyboard-interactive authentication";
            instruction = "Please enter the following information";
            requests.Add("Username: ");
            requests.Add("Password: ");
            echoes.Size = 2;
            echoes.set_Bits(0, true);
            echoes.set_Bits(1, false);
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
        private void SSHServer_OnAuthKeyboardResponse(object sender, TElStringList requests, TElStringList responses, 
            ref string name, ref string instruction, TElStringList newRequests, TElBits echoes, ref bool accept) 
        {
            accept = false;
            string username = "";
            string password = "";

            if ((responses != null) && (responses.Count == 2)) 
            {
                username = responses[0];
                password = responses[1];

                accept = Authorize(username, password);
            }

            log.Write(TraceLevel.Info, "Interactive authentication from {0} ({1}) {2}", 
                remoteEP, username, accept ? "accepted." : "denied.");
        }

        /// <summary>
        /// Queries if further client authentication is needed
        /// </summary>
        /// <param name="Sender">ElSSHServer object</param>
        /// <param name="Username">User login name</param>
        /// <param name="Needed">Set to true if further authentication is needed, or to false if the authentication stage is completed</param>
        private void SSHServer_OnFurtherAuthNeeded(object sender, string username, ref bool needed)
        {
            needed = false;
        }

        #endregion SSH Server authentication handling

        #region SSH Server socket-related processing

        /// <summary>
        /// Is fired when ElSSHServer has data to write to socket
        /// </summary>
        /// <param name="Sender">ElSSHServer object</param>
        /// <param name="Buffer">Data to write to socket</param>
        private void SSHServer_OnSend(object sender, byte[] buffer)
        {
            try 
            {
                socket.Send(buffer);
            }
            catch(ObjectDisposedException) {}
            catch(Exception e)
            {
                log.Write(TraceLevel.Warning, "Socket send operation failed: " + e.Message);
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
        private void SSHServer_OnReceive(object sender, ref byte[] buffer, int maxSize, out int written)
        {
            try 
            {
                written = socket.Receive(buffer, maxSize, SocketFlags.None);
            } 
            catch(ThreadAbortException) 
            {
                written = 0;
            }
            catch(Exception e) 
            {
                written = 0;
                CloseSession();

                log.Write(TraceLevel.Warning, "Socket receive operation failed: " + e.Message);
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
            log.Write(TraceLevel.Error, "SSH stack error encountered. Error code: " + ErrorCode.ToString());
        }

        #endregion

        #region SSH Server connection-layer event handlers

        /// <summary>
        /// Is fired when a client requests SSH subsystem
        /// </summary>
        /// <param name="Sender">ElSSHServer object</param>
        /// <param name="Connection">Logical connection object</param>
        /// <param name="Subsystem">Subsystem name</param>
        private void SSHServer_OnOpenSubsystem(object sender, TElSSHTunnelConnection connection, string subsystem)
        {
            if(subsystem == Consts.SftpSubsystem) 
            {
                log.Write(TraceLevel.Info, "Connection from {0} opening SFTP subsystem", remoteEP);

                sftpSession = new SFTPSession(connection, this, log);
            }
            else
            {
                log.Write(TraceLevel.Info, "Connection from {0} attempting to open unknown subsystem: {1}", remoteEP, subsystem);
            }
        }

        /// <summary>
        /// Is fired when a client requests shell
        /// </summary>
        /// <param name="Sender">ElSSHServer object</param>
        /// <param name="Connection">Logical connection object</param>
        private void SSHServer_OnOpenShell(object sender, TElSSHTunnelConnection connection)
        {
            log.Write(TraceLevel.Info, "Connection from {0} opening shell", remoteEP);

            TElSSHSubsystemThread thread = new TElSSHSubsystemThread(new TElShellSSHSubsystemHandler(connection, true), connection, true);
            thread.Resume();
        }

        #endregion

        #region Private methods

        /// <summary>Sets up server properties</summary>
        private void SetupServer(byte[] key)
        {
            try
            {
                if(keyUtil.LoadPrivateKey(key, key.Length, "") == 0)
                    hostKeys.Add(keyUtil);
                else
                    log.Write(TraceLevel.Error, "Failed to load private SSH key");
            } 
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Invalid private SSH key: " + e.Message);
            }

            sshServer.KeyStorage = hostKeys;
            sshServer.AllowedSubsystems.Add(Consts.SftpSubsystem);
            sshServer.SoftwareName = Consts.SoftwareName;
            sshServer.ForceCompression = Consts.ForceCompression;
            sshServer.OnAuthAttempt += new TSSHAuthAttemptEvent(SSHServer_OnAuthAttempt);
            sshServer.OnAuthFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(SSHServer_OnAuthFailed);
            sshServer.OnAuthPassword += new TSSHAuthPasswordEvent(SSHServer_OnAuthPassword);
            sshServer.OnAuthPublicKey += new TSSHAuthPublicKeyEvent(SSHServer_OnAuthPublicKey);
            sshServer.OnAuthKeyboard += new TSSHAuthKeyboardEvent(SSHServer_OnAuthKeyboard);
            sshServer.OnAuthKeyboardResponse += new TSSHAuthKeyboardResponseEvent(SSHServer_OnAuthKeyboardResponse);
            sshServer.OnFurtherAuthNeeded += new TSSHFurtherAuthNeededEvent(SSHServer_OnFurtherAuthNeeded);
            sshServer.OnSend += new SBSSHCommon.TSSHSendEvent(SSHServer_OnSend);
            sshServer.OnReceive += new SBSSHCommon.TSSHReceiveEvent(SSHServer_OnReceive);
            sshServer.OnCloseConnection += new SBSSHCommon.TSSHCloseConnectionEvent(SSHServer_OnCloseConnection);
            sshServer.OnError += new SBSSHCommon.TSSHErrorEvent(SSHServer_OnError);
            sshServer.OnOpenSubsystem += new TSSHOpenSubsystemEvent(SSHServer_OnOpenSubsystem);
            sshServer.OnOpenShell += new TSSHOpenShellEvent(SSHServer_OnOpenShell);
        }

        private bool Authorize(string username, string password)
        {
            if(config.IsAuthorized(username, password))
            {
                this.homeDir = config.GetHomeDirectory(username);

                if(this.homeDir != null)
                {
                    log.Write(TraceLevel.Verbose, "Home directory: " + homeDir.FullName);
                    return true;
                }
                else
                {
                    log.Write(TraceLevel.Error, "No home directory set for user: " + username);
                }
            }
            return false;
        }

        private void CloseSession()
        {
            if(this.SessionClosed != null)
                SessionClosed(this);
        }

        private static string AuthTypeToStr(int authType)
        {
            switch(authType)
            {
                case SBSSHConstants.Unit.SSH_AUTH_TYPE_RHOSTS: 
                    return "Rhosts";
                case SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY: 
                    return "PublicKey";
                case SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD: 
                    return "Password";
                case SBSSHConstants.Unit.SSH_AUTH_TYPE_HOSTBASED: 
                    return "Hostbased";
                case SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD: 
                    return "Keyboard-interactive";
                default:	
                    return "Unknown";
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if(this.shutdown)
                return;     // We've already been disposed.

            this.shutdown = true;

            if(!this.readThread.Join(Server.Consts.ThreadAbortTimeout))
                readThread.Abort();
			
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
					
                log.Write(TraceLevel.Verbose, "Socket closed: " + remoteEP);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Error closing socket ({0}): {1}", remoteEP, e.Message);
            }

            this.sshServer.Close(true);
            this.sshServer.Destroy();

            this.keyUtil.Destroy();
            this.hostKeys.Destroy();
        }
        #endregion
    }
}
