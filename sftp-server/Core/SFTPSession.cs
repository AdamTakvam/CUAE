using System;
using System.Diagnostics;
using System.Collections;
using System.IO;

using Metreos.LoggingFramework;

using SBSSHHandlers;
using SBSftpHandler;
using SBSSHCommon;
using SBSftpCommon;

namespace Metreos.SftpServer
{
	/// <summary>Handles a single SFTP session</summary>
	public class SFTPSession
	{
        private readonly LogWriter log;

        private readonly SSHSession sshSession;
        private readonly TElSSHSubsystemThread thread;

        private string[] dirList = null;
        private string[] fileList = null;
        private int dirIndex = 0;
        private int fileIndex = 0;

        public SFTPSession(TElSSHTunnelConnection conn, SSHSession parent, LogWriter log)
		{
            Debug.Assert(conn != null, "Cannot create SFTP session with null connection");
            Debug.Assert(parent != null, "Cannot create SFTP session with null SSH session");

            this.sshSession = parent;
            this.log = log;

            this.thread = new TElSSHSubsystemThread(new TElSFTPSSHSubsystemHandler(conn, true), conn, true);
            TElSFTPSSHSubsystemHandler handler = (TElSFTPSSHSubsystemHandler)thread.Handler;
            handler.Server.OnClose += new SBUtils.TNotifyEvent(Server_OnClose);
            handler.Server.OnCloseHandle += new SBSftpCommon.TElSFTPServerCloseHandleEvent(Server_OnCloseHandle);
            handler.Server.OnCreateDirectory += new SBSftpCommon.TElSFTPServerCreateDirectoryEvent(Server_OnCreateDirectory);
            handler.Server.OnError += new SBSftpCommon.TSBSftpErrorEvent(Server_OnError);
            handler.Server.OnFindClose += new SBSftpCommon.TElSFTPServerFindCloseEvent(Server_OnFindClose);
            handler.Server.OnFindFirst += new SBSftpCommon.TElSFTPServerFindFirstEvent(Server_OnFindFirst);
            handler.Server.OnFindNext += new SBSftpCommon.TElSFTPServerFindNextEvent(Server_OnFindNext);
            handler.Server.OnOpen += new SBUtils.TNotifyEvent(Server_OnOpen);
            handler.Server.OnOpenFile += new SBSftpCommon.TElSFTPServerOpenFileEvent(Server_OnOpenFile);
            handler.Server.OnReadFile += new SBSftpCommon.TElSFTPServerReadEvent(Server_OnReadFile);
            handler.Server.OnRemove += new SBSftpCommon.TElSFTPServerRemoveEvent(Server_OnRemove);
            handler.Server.OnRenameFile += new SBSftpCommon.TElSFTPServerRenameFileEvent(Server_OnRenameFile);
            handler.Server.OnRequestAbsolutePath += new SBSftpCommon.TElSFTPServerRequestAbsolutePathEvent(Server_OnRequestAbsolutePath);
            handler.Server.OnRequestAttributes += new SBSftpCommon.TElSFTPServerRequestAttributesEvent(Server_OnRequestAttributes);
            handler.Server.OnWriteFile += new SBSftpCommon.TElSFTPServerWriteEvent(Server_OnWriteFile);
            handler.Server.Versions = SBSftpCommon.Unit.sbSFTP3 | SBSftpCommon.Unit.sbSFTP4;
            thread.Resume();

            log.Write(TraceLevel.Info, "SFTP session started for: " + sshSession.RemoteEP);
		}

        #region SFTP server event handlers

        /// <summary>
        /// Is fired when the SFTP connection is gracefully closed
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        private void Server_OnClose(object sender)
        {
            log.Write(TraceLevel.Verbose, "SFTP connection from {0} closed", sshSession.RemoteEP);
        }

        /// <summary>
        /// Is fired when 'Close handle' request is received from client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Data">User-defined Data associated with file operation</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnCloseHandle(object sender, object data, ref int errorCode, ref string comment)
        {
            try 
            {
                ((Stream)data).Close();
                errorCode = 0;
            } 
            catch(Exception e) 
            {
                errorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
                comment = e.Message;
            }
        }

        /// <summary>
        /// Is fired when 'Create directory' request is received from client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Path">Path to create</param>
        /// <param name="Attributes">Desired attributes</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnCreateDirectory(object sender, string path, SBSftpCommon.TElSftpFileAttributes attributes, 
            ref int errorCode, ref string comment)
        {
            string realpath = CanonicalizePath(path, true);
            try
            {
                Directory.CreateDirectory(realpath);
                errorCode = 0;
            }
            catch(Exception e)
            {
                errorCode = SBSftpCommon.Unit.SSH_ERROR_PERMISSION_DENIED;
                comment = e.Message;
            }

            if(errorCode != 0)
                log.Write(TraceLevel.Error, "Failed to create directory '{0}': {1}", realpath, comment);
            else
                log.Write(TraceLevel.Info, "Directory created: {0}", realpath);
        }

        /// <summary>
        /// Is fired when some error occurs during SFTP communication
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="ErrorCode">Error code</param>
        /// <param name="Comment">Error comment</param>
        private void Server_OnError(object sender, int errorCode, string comment)
        {
            log.Write(TraceLevel.Error, "SFTP client error ({0}): {1} ", errorCode, comment);
        }

        /// <summary>
        /// Is fired when the directory browse operation is closed
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="SearchRec">User-specific data associated with browse session</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnFindClose(object sender, object searchRec, ref int errorCode, ref string comment)
        {
            errorCode = 0;
        }

        /// <summary>
        /// Is fired when the directory browse operation is initiated by client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Path">Directory to be browsed</param>
        /// <param name="Data">User-specific data associated with browse session</param>
        /// <param name="Info">Fill this object with directory item (either file or directory) properties</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnFindFirst(object sender, string path, ref object data, 
            SBSftpCommon.TElSftpFileInfo info, ref int errorCode, ref string comment)
        {
            string localpath = CanonicalizePath(path, true);
            try 
            { 
                string[] lst = System.IO.Directory.GetDirectories(localpath);
                dirList = new string[lst.Length+2];
                dirList[0] = ".";
                dirList[1] = "..";
                
                for(int i=0; i<lst.Length; i++) 
                {
                    dirList[i+2] = lst[i];
                }
                
                fileList = System.IO.Directory.GetFiles(localpath);
                fileIndex = 0;
                dirIndex = 0;

                FillNextFileInfo(info, ref errorCode);
                data = null;
            } 
            catch(Exception e) 
            {
                errorCode = SBSftpCommon.Unit.SSH_ERROR_NO_SUCH_FILE;
                comment = e.Message;
            }
        }

        /// <summary>
        /// Is consequently fired until the whole directory is browsed
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Data">User-specific data associated with browse session</param>
        /// <param name="Info">Fill this object with directory item (either file or directory) properties</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnFindNext(object eender, object data, 
            SBSftpCommon.TElSftpFileInfo info, ref int errorCode, ref string comment)
        {
            FillNextFileInfo(info, ref errorCode);
        }

        /// <summary>
        /// Is fired when SFTP session is established
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        private void Server_OnOpen(object sender)
        {
            log.Write(TraceLevel.Verbose, "SFTP connection established from: " + sshSession.RemoteEP);
        }

        /// <summary>
        /// Is fired when file open operation is requested by client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Path">File to open</param>
        /// <param name="Modes">File open modes</param>
        /// <param name="Access">Specifies file blocking parameters</param>
        /// <param name="DesiredAccess">Desired file access</param>
        /// <param name="Attributes">Initial file attributes</param>
        /// <param name="Data">User-specific data associated with file operation</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnOpenFile(object sender, string path, int modes, int access, uint desiredAccess, 
            SBSftpCommon.TElSftpFileAttributes attributes, ref object data, ref int errorCode, ref string comment)
        {
            string realPath = CanonicalizePath(path, true);
            FileMode mode;
            FileAccess fAccess;
            if (((modes & SBSftpCommon.Unit.fmCreate) == SBSftpCommon.Unit.fmCreate) &&
                ((modes & SBSftpCommon.Unit.fmTruncate) == SBSftpCommon.Unit.fmTruncate))
            {
                mode = FileMode.Create;
            } 
            else if(((modes & SBSftpCommon.Unit.fmCreate) == SBSftpCommon.Unit.fmCreate) &&
                ((modes & SBSftpCommon.Unit.fmExcl) == SBSftpCommon.Unit.fmExcl))
            {
                if(File.Exists(realPath)) 
                {
                    comment = "Cannot open file";
                    errorCode = SBSftpCommon.Unit.SSH_ERROR_FILE_ALREADY_EXISTS;
                    return;
                } 
                else 
                {
                    mode = FileMode.Create;
                }
            } 
            else 
            {
                if ((modes & SBSftpCommon.Unit.fmCreate) == SBSftpCommon.Unit.fmCreate) 
                {
                    mode = FileMode.Create;
                } 
                else 
                {
                    mode = FileMode.Open;
                }
            }
            if (((modes & SBSftpCommon.Unit.fmRead) == SBSftpCommon.Unit.fmRead) &&
                ((modes & SBSftpCommon.Unit.fmWrite) == SBSftpCommon.Unit.fmWrite))
            {
                fAccess = FileAccess.ReadWrite;
            } 
            else if ((modes & SBSftpCommon.Unit.fmWrite) == SBSftpCommon.Unit.fmWrite) 
            {
                fAccess = FileAccess.Write;
            }
            else 
            {
                fAccess = FileAccess.Read;
            }
            try 
            {
                data = new FileStream(realPath, mode, fAccess);
                if ((modes & SBSftpCommon.Unit.fmAppend) == SBSftpCommon.Unit.fmAppend) 
                {
                    ((FileStream)data).Position = ((FileStream)data).Length;
                }
                errorCode = 0;
            } 
            catch(Exception e) 
            {
                errorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
                comment = e.Message;
            }

            log.Write(TraceLevel.Info, "Connection '{0}' opened file '{1}' for {2}",
                sshSession.RemoteEP, realPath, fAccess);
        }

        /// <summary>
        /// Is fired when a 'Read file' command is received from user
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Data">User-specific data associated with file operation</param>
        /// <param name="Offset">File start offset</param>
        /// <param name="Buffer">Place where to put file data</param>
        /// <param name="BufferOffset">Buffer offset</param>
        /// <param name="Count">Maximal count of bytes to read from file</param>
        /// <param name="Read">Actual bytes read</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnReadFile(object sender, object data, long offset, ref byte[] buffer, 
            int bufferOffset, int count, ref int read, ref int errorCode, ref string comment)
        {
            errorCode = SBSftpCommon.Unit.SSH_ERROR_OP_UNSUPPORTED;
            comment = "Read access is not supported";

//            try 
//            {
//                Stream strm = (Stream)data;
//                read = strm.Read(buffer, bufferOffset, count);
//                errorCode = 0;
//            } 
//            catch(Exception e) 
//            {
//                errorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
//                comment = e.Message;
//            }
        }

        /// <summary>
        /// Is fired, when 'Remove' request is received from client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Path">Path to file or directory to remove</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnRemove(object sender, string path, ref int errorCode, ref string comment)
        {
            errorCode = SBSftpCommon.Unit.SSH_ERROR_OP_UNSUPPORTED;
            comment = "Deletion is not supported";

//            bool success = false;
//            string localpath = CanonicalizePath(Path, true);
//            try 
//            {
//                if (System.IO.Directory.Exists(localpath)) 
//                {
//                    System.IO.Directory.Delete(localpath);
//                    success = true;
//                } 
//                if ((!success) && (System.IO.File.Exists(localpath))) 
//                {
//                    System.IO.File.Delete(localpath);
//                    success = true;
//                }
//                if (!success) 
//                {
//                    throw new Exception("File does not exist");
//                } 
//                ErrorCode = 0;
//            } 
//            catch(Exception ex) 
//            {
//                ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;           
//                Comment = ex.Message;
//            }
        }

        /// <summary>
        /// Is fired when the 'Rename file' is requested from client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="OldPath">Path to file/directory to rename</param>
        /// <param name="NewPath">New name</param>
        /// <param name="Flags">Rename flags</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnRenameFile(object sender, string oldPath, string newPath, int flags, 
            ref int errorCode, ref string comment)
        {
            errorCode = SBSftpCommon.Unit.SSH_ERROR_OP_UNSUPPORTED;
            comment = "Renaming is not supported";

//            bool success = false;
//            string localold = CanonicalizePath(OldPath, true);
//            string localnew = CanonicalizePath(NewPath, true);
//            try 
//            {
//                if (System.IO.File.Exists(localold)) 
//                {
//                    System.IO.File.Move(localold, localnew);
//                    success = true;
//                }
//                if ((!success) && (System.IO.Directory.Exists(localold))) 
//                {
//                    System.IO.Directory.Move(localold, localnew);
//                    success = true;
//                }
//                if (!success) 
//                {
//                    throw new Exception("File does not exist");
//                }
//                ErrorCode = 0;
//            } 
//            catch(Exception ex) 
//            {
//                ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
//                Comment = ex.Message;
//            }
        }

        /// <summary>
        /// Is fired when a client requests to canonicalize relative path to absolute
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Path">Relative path</param>
        /// <param name="AbsolutePath">Converted absolute path</param>
        /// <param name="Control">Path canonicalization parameters</param>
        /// <param name="ComposePath"></param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnRequestAbsolutePath(object Sender, string Path, ref string AbsolutePath, 
            SBSftpCommon.TSBSftpRealpathControl Control, SBStringList.TElStringList ComposePath, 
            ref int ErrorCode, ref string Comment)
        {
            AbsolutePath = CanonicalizePath(Path, false);
            ErrorCode = 0;
        }

        /// <summary>
        /// Is fired when file attributes are requested by client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Path">Path to file/directory</param>
        /// <param name="FollowSymLinks">True, if symbolic links should be processed</param>
        /// <param name="Attributes">Object to put file attributes</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnRequestAttributes(object sender, string path, bool followSymLinks, 
            SBSftpCommon.TElSftpFileAttributes attributes, ref int errorCode, ref string comment)
        {
            string localpath = CanonicalizePath(path, true);
            try 
            {
                if(Directory.Exists(localpath)) 
                {
                    DirectoryInfo info = new DirectoryInfo(localpath);
                    attributes.Size = 0;
                    attributes.Directory = true;
                    attributes.MTime = info.LastWriteTime;
                } 
                else if(File.Exists(localpath)) 
                {
                    FileInfo info = new FileInfo(localpath);
                    attributes.Size = info.Length;
                    attributes.Directory = false;
                    attributes.MTime = info.LastWriteTime;
                } 
                else 
                {
                    throw new Exception("File does not exist");
                }

                attributes.IncludedAttributes = SBSftpCommon.Unit.saSize |
                    SBSftpCommon.Unit.saMTime;
                errorCode = 0;
            } 
            catch(Exception e) 
            {
                errorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
                comment = e.Message;
            }
        }

        /// <summary>
        /// Is fired when 'Write' operation is requested by client
        /// </summary>
        /// <param name="Sender">ElSFTPServer object</param>
        /// <param name="Data">User-specific data associated with file operation</param>
        /// <param name="Offset">File write offset</param>
        /// <param name="Buffer">Data to write</param>
        /// <param name="BufferOffset">Data start index</param>
        /// <param name="Count">Length of data to write</param>
        /// <param name="ErrorCode">Operation result code (please see the documentation)</param>
        /// <param name="Comment">Operation result comment (please see the documentation)</param>
        private void Server_OnWriteFile(object sender, object data, long offset, byte[] buffer, 
            int bufferOffset, int count, ref int errorCode, ref string comment)
        {
            try 
            {
                Stream strm = (Stream)data;
                strm.Write(buffer, bufferOffset, count);
                errorCode = 0;
            } 
            catch(Exception e) 
            {
                errorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
                comment = e.Message;
            }
        }

        #endregion
		
        #region Private methods

        /// <summary>
        /// Canonicalizes relative path
        /// </summary>
        /// <param name="relpath">Path to canonicalize</param>
        /// <param name="local">If true, the path will be converted to local ('C:\...'), otherwise to remote ('/...') form</param>
        /// <returns>the converted path</returns>
        private string CanonicalizePath(string relPath, bool local)
        {
            if(relPath == ".")
                return sshSession.HomeDirectory.FullName;

            return relPath.Replace("/", "\\");
        }

        /// <summary>
        /// Return the UNIX-style file information string
        /// </summary>
        /// <param name="info">file information object</param>
        /// <returns>the UNIX-style string</returns>
        private string UnixPath(TElSftpFileInfo info) 
        {
            string result = "";
            if (info.Attributes.Directory) 
            {
                result = "drwxrwxrwx   1 user     group    ";
            } 
            else 
            {
                result = "-rwxrwxrwx   1 user     group    ";
            }
            string szstr = info.Attributes.Size.ToString();
            while (szstr.Length < 8) 
            {
                szstr = "0" + szstr;
            }
            result = result + szstr + " ";
            result = result + " " + info.Attributes.MTime.ToString("MMM dd HH:mm") + " " + info.Name;
            return result;
        }

        /// <summary>
        /// Fills ElSftpFileInfo object with file information
        /// </summary>
        /// <param name="info">object to fill</param>
        /// <param name="ErrorCode">operation result code</param>
        private void FillNextFileInfo(TElSftpFileInfo info, ref int errorCode)
        {
            TElSftpFileAttributes attrs = info.Attributes;
            if (dirIndex < dirList.Length) 
            {
                info.Name = System.IO.Path.GetFileName(dirList[dirIndex]);
                attrs.CTime = System.IO.Directory.GetCreationTime(dirList[dirIndex]);
                attrs.ATime = System.IO.Directory.GetLastAccessTime(dirList[dirIndex]);
                attrs.MTime = System.IO.Directory.GetLastWriteTime(dirList[dirIndex]);
                attrs.Size = 0;
                attrs.Directory = true;
                attrs.FileType = SBSftpCommon.TSBSftpFileType.ftDirectory;
                dirIndex++;
            } 
            else if (fileIndex < fileList.Length) 
            {
                info.Name = System.IO.Path.GetFileName(fileList[fileIndex]);
                attrs.CTime = System.IO.File.GetCreationTime(fileList[fileIndex]);
                attrs.ATime = System.IO.File.GetLastAccessTime(fileList[fileIndex]);
                attrs.MTime = System.IO.File.GetLastWriteTime(fileList[fileIndex]);
                System.IO.FileInfo i = new FileInfo(fileList[fileIndex]);
                attrs.Size = i.Length;
                attrs.Directory = false;
                attrs.FileType = SBSftpCommon.TSBSftpFileType.ftFile;
                fileIndex++;
            } 
            else 
            {
                errorCode = SBSftpCommon.Unit.SSH_ERROR_EOF;
                return;
            }
            attrs.UserExecute = true;
            attrs.UserRead = true;
            attrs.UserWrite = true;
            attrs.GroupExecute = true;
            attrs.GroupRead = true;
            attrs.GroupWrite = true;
            attrs.OtherExecute = true;
            attrs.OtherRead = true;
            attrs.OtherWrite = true;
            attrs.IncludedAttributes = SBSftpCommon.Unit.saPermissions | SBSftpCommon.Unit.saSize |
                SBSftpCommon.Unit.saMTime | SBSftpCommon.Unit.saATime | SBSftpCommon.Unit.saCTime;
            info.LongName = UnixPath(info);
        }

        #endregion
	}
}
