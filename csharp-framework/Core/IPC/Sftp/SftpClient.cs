using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

using SBUtils;
using SBSftpCommon;
using SBSimpleSftp;

using Metreos.Interfaces;

namespace Metreos.Core.IPC.Sftp
{
    public delegate void VoidDelegate();

	public class SftpClient
	{
        public abstract class Consts
        {
            public const int Port           = 22;
            public const int ConnectTimeout = 10;
        }

        private SBSimpleSftp.TElSimpleSFTPClient sftpClient;
        private Thread connectThread;
        private string connectError;
        private bool connected = false;

        private bool authSuccess = false;
        private AutoResetEvent authEvent;

        private string currentDir = ".";
        public string CurrentDir { get { return currentDir; } }

        private Hashtable files;
        public ArrayList Files { get { return new ArrayList(files.Keys); } }

        private Hashtable subDirs;
        public ArrayList SubDirectories { get { return new ArrayList(subDirs.Keys); } }

        public VoidDelegate onConnectionClosed;

        #region Construction/Open/Close

		public SftpClient()
		{
            SBUtils.Unit.SetLicenseKey(IConfig.LicenseKeys.SecureBlackBox);

            this.files = new Hashtable();
            this.subDirs = new Hashtable();
            this.authEvent = new AutoResetEvent(false);

            this.sftpClient = new SBSimpleSftp.TElSimpleSFTPClient();
            this.sftpClient.CompressionLevel = 6;
            this.sftpClient.ForceCompression = false;
            this.sftpClient.KeyStorage = null;
            this.sftpClient.NewLineConvention = null;
            this.sftpClient.SFTPExt = null;
            this.sftpClient.SoftwareName = "Metreos SFTP Client";
            this.sftpClient.Versions = (short)28;  // ??
            this.sftpClient.OnKeyValidate += new SBSSHCommon.TSSHKeyValidateEvent(this.sftpClient_OnKeyValidate);
            this.sftpClient.OnAuthenticationSuccess += new SBUtils.TNotifyEvent(this.sftpClient_OnAuthenticationSuccess);
            this.sftpClient.OnCloseConnection += new SBSSHCommon.TSSHCloseConnectionEvent(this.sftpClient_OnCloseConnection);
            this.sftpClient.OnError += new SBSSHCommon.TSSHErrorEvent(this.sftpClient_OnError);
            this.sftpClient.MessageLoop += new SBSftpCommon.TSBSftpMessageLoopEvent(this.sftpClient_MessageLoop);
            this.sftpClient.OnAuthenticationFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(this.sftpClient_OnAuthenticationFailed);
		}

        /// <summary>
        /// Connect in a child thread because the nubs at SecureBlackBox 
        /// didn't think to include a connect timeout.
        /// </summary>
        private void Open()
        {
            connectError = null;
            connected = false;

            try { sftpClient.Open(); } 
            catch(Exception e) 
            {
                connectError = e.Message;
                return;
            }

            if(authEvent.WaitOne(5000, false) == false)
            {
                connectError = "Did not receive authentication response";
                Close();
                return;
            }

            if(authSuccess == false)
            {
                connectError = "Invalid username or password";
                Close();
                return;
            }

            connected = true;
        }

        public bool Open(string ipAddress, int port, string username, string password, out string failReason)
        {
            return Open(ipAddress, port, username, password, Consts.ConnectTimeout, out failReason);
        }

        /// <summary>
        /// Connects to the specified SFTP server
        /// </summary>
        /// <param name="ipAddress">Server IP address</param>
        /// <param name="port">SFTP port</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="timeout">Connect timeout (in seconds)</param>
        /// <param name="failReason">Failure description, if this method returns false</param>
        /// <returns>Success</returns>
        public bool Open(string ipAddress, int port, string username, string password, int timeout, out string failReason)
        {
            failReason = null;

            if(sftpClient.Active) 
                Close();

            sftpClient.Username = username;
            sftpClient.Password = password;
            sftpClient.Address = ipAddress;
            sftpClient.Port = port != 0 ? port : Consts.Port;
            timeout = timeout != 0 ? timeout : Consts.ConnectTimeout;
			
            StartConnectThread();
            if(connectThread.Join(timeout * 1000) == false)
            {
                connectThread.Abort();
                failReason = "Connect timed out";
                return false;
            }

            if(connected == false)
            {
                failReason = connectError;
                return false;
            }

            RefreshData();
            return true;
        }

        private void StartConnectThread()
        {
            this.connectThread = new Thread(new ThreadStart(Open));
            this.connectThread.Name = "SFTP client connect thread";
            this.connectThread.IsBackground = true;
            this.connectThread.Start();
        }

        public void Close()
        {
            if(sftpClient.Active) 
            {
                sftpClient.Close(true);
                sftpClient.Dispose();
            }

            currentDir = ".";
            files.Clear();
            subDirs.Clear();
        }
        #endregion

        public bool MakeDir(string dirName)
        {
            if(sftpClient.Active == false || dirName == null || dirName == String.Empty)
                return false;

            try 
            {
                sftpClient.MakeDirectory(Path.Combine(currentDir, dirName), null);
            } 
            catch
            {
                return false;
            }

            RefreshData();
            return true;
        }

        public bool Rename(string oldFilename, string newFilename)
        {
            if(sftpClient.Active == false || oldFilename == null || oldFilename == String.Empty ||
                newFilename == null || newFilename == String.Empty)
                return false;
                
            try 
            {
                sftpClient.RenameFile(Path.Combine(currentDir, oldFilename),
                    Path.Combine(currentDir, newFilename));
            } 
            catch
            {
                return false;
            }

            RefreshData();
            return true;
        }

        public bool Delete(string name)
        {
            if(sftpClient.Active == false || name == null || name == String.Empty)
                return false;

            try 
            {
                if(subDirs.ContainsKey(name)) 
                {
                    sftpClient.RemoveDirectory(Path.Combine(currentDir, name));
                } 
                else 
                {
                    sftpClient.RemoveFile(Path.Combine(currentDir, name));
                }
            } 
            catch
            {
                return false;
            }

            RefreshData();
            return true;
        }

        public bool Download(string filename, DirectoryInfo localDir, IProgressIndicator progressBar, 
            out string failReason)
        {
            failReason = null;

            if(sftpClient.Active == false || filename == null || filename == String.Empty)
                return false;

            if(localDir == null)
                localDir = new DirectoryInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string localFile = Path.Combine(localDir.FullName, filename);            

            FileStream stream;
            try 
            {
                stream = new System.IO.FileStream(localFile, FileMode.Create);
            } 
            catch
            {
                failReason = "Failed to create local file: " + localFile;
                return false;
            }

            byte[] fileHandle;
            try 
            {
                fileHandle = sftpClient.OpenFile(Path.Combine(currentDir, filename),
                    SBSftpCommon.Unit.fmRead, null);
            } 
            catch(Exception e) 
            {
                stream.Close();
                failReason = String.Format("Failed to open remote file '{0}': {1}", filename, e.Message);
                return false;
            }

            // Determine the range of empty space on the progress bar
            int progMinValue = 0, progRange = 0;
            if(progressBar != null)
            {
                progMinValue = progressBar.Value;
                progRange = progressBar.Maximum - progMinValue;
            }

            TElSftpFileInfo info = files[filename] as TElSftpFileInfo;
            if(info == null)
            {
                failReason = String.Format("File '{0}' does not exist in directory: {1}", filename, currentDir);
                return false;
            }

            int read = 0;
            long processed = 0;
            bool canceled = false;
            long size = info.Attributes.Size;
            byte[] buf = new byte[50000];
            try 
            {
                while ((processed < size) && (!canceled)) 
                {
                    read = sftpClient.Read(fileHandle, processed, buf, 0, buf.Length);
                    stream.Write(buf, 0, read);
                    processed += read;

                    if(progressBar != null)
                    {
                        progressBar.Value = GetProgressValue(progMinValue, progRange, processed, size);
                        canceled = progressBar.Canceled;
                    }
                }
            } 
            catch(Exception e) 
            {
                failReason = String.Format("Error downloading file '{0}': {1}", localFile, e.Message);
                return false;
            }
            finally 
            {
                stream.Close();
                sftpClient.CloseHandle(fileHandle);
            }
            return true;
        }

        public bool Upload(FileInfo localFile, IProgressIndicator progressBar, out string failReason)
        {
            failReason = null;

            if(sftpClient.Active == false || localFile == null)
                return false;

            FileStream stream;
            try 
            {
                stream = new System.IO.FileStream(localFile.FullName, FileMode.Open);
            } 
            catch(Exception e) 
            {
                failReason = "Failed to open source file: " + e.Message;
                return false;
            }

            byte[] fileHandle;
            try 
            {
                fileHandle = sftpClient.CreateFile(Path.Combine(currentDir, localFile.Name));
            } 
            catch(Exception e) 
            {
                failReason = "Failed to create remote file: " + e.Message;
                stream.Close();
                return false;
            }

            // Determine the range of empty space on the progress bar
            int progMinValue = progressBar != null ? progressBar.Value : 0;
            int progRange = progressBar != null ? progressBar.Maximum - progMinValue : 0;

            int read = 0;
            long processed = 0;
            bool canceled = false;
            long size = stream.Length;
            byte[] buf = new byte[4096];
            try 
            {
                while((processed < size) && (!canceled)) 
                {
                    read = stream.Read(buf, 0, buf.Length);
                    sftpClient.Write(fileHandle, processed, buf, 0, read);
                    processed += read;

                    if(progressBar != null)
                    {
                        progressBar.Value = GetProgressValue(progMinValue, progRange, processed, size);
                        canceled = progressBar.Canceled;
                    }
                }
            } 
            catch(Exception e) 
            {
                failReason = String.Format("Error uploading file '{0}': {1}", localFile.Name, e.Message);
                return false;
            }
            finally 
            {
                stream.Close();
                sftpClient.CloseHandle(fileHandle);
            }

            RefreshData();
            return true;
        }

        public bool ChangeDir(string dirName)
        {
            if(sftpClient.Active == false || dirName == null || dirName == String.Empty)
                return false;

            TElSftpFileInfo info = subDirs[dirName] as TElSftpFileInfo;
            if(info == null)
                return false;

            byte[] dirHandle;
            try 
            {
                string fqDirName = Path.Combine(currentDir, info.Name);
                dirHandle = sftpClient.OpenDirectory(fqDirName);
                sftpClient.CloseHandle(dirHandle);
                currentDir = sftpClient.RequestAbsolutePath(fqDirName);
            } 
            catch { return false; }

            RefreshData();
            return true;
        }

        #region Private helper methods

        private int GetProgressValue(int progMinValue, int progRange, long processed, long fileSize)
        {
            if(processed == 0 || fileSize == 0) 
                return progMinValue;

            float filePercent = processed / (float)fileSize;
            int progOffset = Convert.ToInt32(progRange * filePercent);
            return progMinValue + progOffset;
        }

        private void RefreshData()
        {
            subDirs.Clear();
            files.Clear();

            if(!sftpClient.Active)
                return;

            try { currentDir = sftpClient.RequestAbsolutePath(currentDir); } 
            catch { currentDir = "."; }

            byte[] dirHandle = null;
            try 
            {
                dirHandle = sftpClient.OpenDirectory(currentDir);

                ArrayList dirList = new ArrayList();
                sftpClient.ReadDirectory(dirHandle, dirList);

                foreach(TElSftpFileInfo info in dirList) 
                {
                    if(info.Attributes.Directory) 
                        subDirs[info.Name] = info;
                    else 
                        files[info.Name] = info;
                }
            } 
            finally 
            {
                if(dirHandle != null)
                    sftpClient.CloseHandle(dirHandle);
            }
        }
        #endregion

        #region Async event handlers

        private void sftpClient_OnAuthenticationFailed(object Sender, int AuthenticationType)
        {
            authSuccess = false;
            authEvent.Set();
        }

        private void sftpClient_OnAuthenticationSuccess(object Sender)
        {
            authSuccess = true;
            authEvent.Set();
        }

        private void sftpClient_OnCloseConnection(object Sender)
        {
            if(onConnectionClosed != null)
                onConnectionClosed();
        }

        private void sftpClient_OnError(object Sender, int ErrorCode)
        {
            // Do nothing
        }

        private void sftpClient_OnKeyValidate(object Sender, SBSSHKeyStorage.TElSSHKey ServerKey, ref bool Validate)
        {
            // Do nothing
        }

        private bool sftpClient_MessageLoop()
        {
            // Note: This is only for Windows Forms apps
            // Application.DoEvents();
            return true;
        }
        #endregion
    }
}