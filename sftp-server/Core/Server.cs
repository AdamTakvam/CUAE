using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Diagnostics;

using Metreos.Configuration;
using Metreos.LogSinks;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.SftpServer
{
	/// <summary>SFTP server implementation</summary>
    public class Server : Loggable
    {
        #region Public delegates/events

        //        public delegate void ClientConnected(IPEndPoint remoteEP);
        //        public delegate void ClientDisconnected(IPEndPoint remoteEP);
        //
        //        public event ClientConnected OnClientConnected;
        //        public event ClientDisconnected OnClientDisconnected;

        #endregion

        #region Constants

        public abstract class Consts
        {
            public const string Name            = "SftpServer";
            public const int MaxConnections     = 100;   // Just to help with DOS attacks
            public const int ThreadAbortTimeout = 2000;  // 2 seconds
            public const int EncryptionBits     = 1024;  // SSH key encryption strength
            public const string KeyFile         = "ssh.key";
        }
        #endregion

        private readonly LogServerSink logClient;

        // Remote IPAddress -> SSHSession object
        private readonly Hashtable sessions; 	

        private readonly TcpListener socketListener;
        private readonly Thread listenerThread;
        private readonly DBHelper config;
        private volatile bool shutdown = false;

        private byte[] sshKey;

        public Server(DBHelper config, int listenPort)
            : base(TraceLevel.Verbose, Consts.Name)
        {
            if(listenPort == 0 || listenPort > ushort.MaxValue)
                listenPort = IConfig.ConfigFileSettings.DefaultValues.SFTP_LISTEN_PORT;

            ushort logPort = Config.LogService.ListenPort;
            logPort = logPort > 0 ? logPort : IServerLog.Default_Port;
            logClient = new LogServerSink(Consts.Name, logPort, TraceLevel.Verbose);

            Assertion.Check(config != null, "Cannot start SFTP Server with null DBHelper");
            this.config = config;

            // Set SecureBlackBox license key
            SBUtils.Unit.SetLicenseKey(IConfig.LicenseKeys.SecureBlackBox);
               
            this.sessions = Hashtable.Synchronized(new Hashtable());

            this.socketListener = new TcpListener(IPAddress.Any, listenPort);

            this.listenerThread = new Thread(new ThreadStart(Listen));
            this.listenerThread.Name = "SFTP Server Listener Thread";
            this.listenerThread.IsBackground = true;
        }

        #region Start/Stop

        /// <summary>Starts listening for incoming connections</summary>
        /// <returns>true, if the listening socket was successfully allocated</returns>
        public bool Start()
        {
            // Verify that we can open a connection to the database
            if(!config.Test())
                return false;

            // Read or generate host SSH key
            this.sshKey = GetServerKey();

            if(this.sshKey == null)
                return false;
            else
                log.Write(TraceLevel.Verbose, "SSH Key: " + System.Text.Encoding.Default.GetString(sshKey));

            log.Write(TraceLevel.Info, "Starting SFTP server listener...");

            try
            {
                socketListener.Start();	
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "ServerListener.Start : " + e.Message);
                return false;
            }

            this.listenerThread.Start();
            
            int localPort = ((IPEndPoint)socketListener.LocalEndpoint).Port;
            log.Write(TraceLevel.Info, "SSH server listener started on port {0}.", localPort);
            return true;
        }

        /// <summary>Stops listening for incoming connections</summary>
        public void Stop()
        {
            log.Write(TraceLevel.Info, "Stopping server");	

            this.shutdown = true;

            if(!this.listenerThread.Join(Consts.ThreadAbortTimeout))
                this.listenerThread.Abort();

            try { socketListener.Stop(); }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Exception while stopping socket listener: " + e.Message);
            }

            log.Write(TraceLevel.Info, "Server stopped. Closing sockets...");

            lock(sessions.SyncRoot)
            {
                foreach(SSHSession session in sessions.Values)
                {
                    session.Dispose();
                }
                sessions.Clear();
            }

            log.Write(TraceLevel.Info, "All sockets closed. Shutdown complete");
        }

        public override void Dispose()
        {
            logClient.Dispose();

            base.Dispose();
        }
        #endregion

        #region Private methods

        private Socket AcceptSocket()
        {
            try
            {
                return socketListener.AcceptSocket();
            }
            catch(ThreadAbortException) {}
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Failed to accept incoming socket connection: " + e.Message);
            }
            return null;
        }

        private void Listen()
        {	
            while(!shutdown)
            {
                // Loop until a connection is pending
                if(!socketListener.Pending())
                {
                    Thread.Sleep(1);
                    continue;
                }

                Socket socket = AcceptSocket();			
                if(socket == null)
                    continue;

                IPAddress srcAddr = ((IPEndPoint)socket.RemoteEndPoint).Address;
                log.Write(TraceLevel.Verbose, "New connection offered: " + srcAddr);

                lock(sessions.SyncRoot)
                {
                    if(sessions.Count < Consts.MaxConnections)
                    {
                        // Create new session and start it in a new thread							
                        SSHSession session = new SSHSession(socket, config, sshKey, log);
                        session.SessionClosed += new SSHSession.SessionClosedHandler(OnSessionClosed);

                        // We only permit one connection per machine.
                        SSHSession oldSession = sessions[srcAddr] as SSHSession;
                        if(oldSession != null)
                            oldSession.Dispose();

                        sessions[srcAddr] = session;

                        log.Write(TraceLevel.Info, "Connection accepted: {0}({1})", srcAddr, sessions.Count);
                    }
                    else
                    {	
                        log.Write(TraceLevel.Error, "Rejecting incoming connection, maximum connection count exceeded");

                        try
                        {						
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                        }
                        catch {}						
                    }
                }
            }
        }
        #endregion

        #region SSH key management

        private byte[] GetServerKey()
        {
            byte[] key = null;            
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Consts.KeyFile);

            if(File.Exists(path))
            {
                FileStream fStream = null;

                try 
                { 
                    FileInfo fInfo = new FileInfo(path);
                    key = new byte[fInfo.Length];
                    fStream = File.OpenRead(path);
                    if(fStream.Read(key, 0, key.Length) == 0)
                        throw new ApplicationException();
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "Host SSH key file is corrupt. Regenerating...");
                    fStream.Close();
                    File.Delete(path);
                    return GetServerKey();  // Recurse to generate key and file
                }

                fStream.Close();
                log.Write(TraceLevel.Verbose, "Host SSH key read from file.");
            }
            else  // Generate key and save it to file
            {
                try 
                { 
                    FileStream fStream = File.Create(path); 

                    key = GenerateServerKey();
                    fStream.Write(key, 0, key.Length);

                    fStream.Flush();
                    fStream.Close();
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Cannot create SSH key file: " + e.Message);
                    return null;
                }

                log.Write(TraceLevel.Verbose, "Host SSH key file generated.");
            }

            return key;
        }

        private byte[] GenerateServerKey()
        {
            SBSSHKeyStorage.TElSSHKey Key = new SBSSHKeyStorage.TElSSHKey();
            Key.Generate(SBSSHKeyStorage.Unit.ALGORITHM_RSA, Consts.EncryptionBits);
            byte[] saveKey = null;
            int saveLen = 0;
            Key.SavePrivateKey(ref saveKey, ref saveLen, "");
            saveKey = new byte[saveLen];
            Key.SavePrivateKey(ref saveKey, ref saveLen, "");
            return saveKey;
        }
        #endregion

        #region Event handlers

        private void OnSessionClosed(SSHSession session)
        {
            if(!shutdown)
            {
                log.Write(TraceLevel.Info, "Client disconnected: " + session.RemoteEP.Address);

                sessions.Remove(session.RemoteEP.Address);
                session.Dispose();
            }
        }

        #endregion
    }
}
