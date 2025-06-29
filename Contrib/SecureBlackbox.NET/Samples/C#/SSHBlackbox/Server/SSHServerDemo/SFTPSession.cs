using System;
using System.Collections;
using System.IO;
using SBSSHHandlers;
using SBSftpHandler;
using SBSSHCommon;
using SBSftpCommon;

namespace SSHServer.NET
{
	/// <summary>
	/// Responsible for a single SFTP session (processing ElSFTPServer requests, accessing file system)
	/// </summary>
	public class SFTPSession
	{
		public SFTPSession(TElSSHTunnelConnection conn)
		{
			// System.Type tp = typeof(TElSFTPSSHSubsystemHandler);
			m_thread = new TElSSHSubsystemThread(new TElSFTPSSHSubsystemHandler(conn, true), conn, true);
			TElSFTPSSHSubsystemHandler handler = (TElSFTPSSHSubsystemHandler)m_thread.Handler;
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
			Logger.Log("SFTP server started");
			m_thread.Resume();
		}

		#region SFTP server event handlers

		/// <summary>
		/// Is fired when the SFTP connection is gracefully closed
		/// </summary>
		/// <param name="Sender">ElSFTPServer object</param>
		private void Server_OnClose(object Sender)
		{
			Logger.Log("SFTP connection closed");
		}

		/// <summary>
		/// Is fired when 'Close handle' request is received from client
		/// </summary>
		/// <param name="Sender">ElSFTPServer object</param>
		/// <param name="Data">User-defined Data associated with file operation</param>
		/// <param name="ErrorCode">Operation result code (please see the documentation)</param>
		/// <param name="Comment">Operation result comment (please see the documentation)</param>
		private void Server_OnCloseHandle(object Sender, object Data, ref int ErrorCode, ref string Comment)
		{
			try 
			{
				((Stream)Data).Close();
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
                ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
				Comment = ex.Message;
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
		private void Server_OnCreateDirectory(object Sender, string Path, SBSftpCommon.TElSftpFileAttributes Attributes, ref int ErrorCode, ref string Comment)
		{
            string realpath = CanonicalizePath(Path, true);
			try 
			{
				System.IO.Directory.CreateDirectory(realpath);
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
				ErrorCode = SBSftpCommon.Unit.SSH_ERROR_PERMISSION_DENIED;
				Comment = ex.Message;
			}
		}

		/// <summary>
		/// Is fired when some error occurs during SFTP communication
		/// </summary>
		/// <param name="Sender">ElSFTPServer object</param>
		/// <param name="ErrorCode">Error code</param>
		/// <param name="Comment">Error comment</param>
		private void Server_OnError(object Sender, int ErrorCode, string Comment)
		{
            Logger.Log("SFTP client error: " + ErrorCode.ToString(), true);
		}

		/// <summary>
		/// Is fired when the directory browse operation is closed
		/// </summary>
		/// <param name="Sender">ElSFTPServer object</param>
		/// <param name="SearchRec">User-specific data associated with browse session</param>
		/// <param name="ErrorCode">Operation result code (please see the documentation)</param>
		/// <param name="Comment">Operation result comment (please see the documentation)</param>
		private void Server_OnFindClose(object Sender, object SearchRec, ref int ErrorCode, ref string Comment)
		{
            ErrorCode = 0;
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
		private void Server_OnFindFirst(object Sender, string Path, ref object Data, SBSftpCommon.TElSftpFileInfo Info, ref int ErrorCode, ref string Comment)
		{
			string localpath = CanonicalizePath(Path, true);
			try 
			{ 
				string[] lst = System.IO.Directory.GetDirectories(localpath);
				m_dirList = new string[lst.Length + 2];
				m_dirList[0] = ".";
				m_dirList[1] = "..";
				for (int i = 0; i < lst.Length; i++) 
				{
					m_dirList[i + 2] = lst[i];
				}
				m_fileList = System.IO.Directory.GetFiles(localpath);
				m_fileIndex = 0;
				m_dirIndex = 0;
				FillNextFileInfo(Info, ref ErrorCode);
				Data = null;
			} 
			catch(Exception ex) 
			{
                ErrorCode = SBSftpCommon.Unit.SSH_ERROR_NO_SUCH_FILE;
				Comment = ex.Message;
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
		private void Server_OnFindNext(object Sender, object Data, SBSftpCommon.TElSftpFileInfo Info, ref int ErrorCode, ref string Comment)
		{
            FillNextFileInfo(Info, ref ErrorCode);
		}

		/// <summary>
		/// Is fired when SFTP session is established
		/// </summary>
		/// <param name="Sender">ElSFTPServer object</param>
		private void Server_OnOpen(object Sender)
		{
			Logger.Log("SFTP connection established");
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
		private void Server_OnOpenFile(object Sender, string Path, int Modes, int Access, uint DesiredAccess, SBSftpCommon.TElSftpFileAttributes Attributes, ref object Data, ref int ErrorCode, ref string Comment)
		{
			string realpath = CanonicalizePath(Path, true);
			FileMode mode;
			FileAccess access;
			if (((Modes & SBSftpCommon.Unit.fmCreate) == SBSftpCommon.Unit.fmCreate) &&
				((Modes & SBSftpCommon.Unit.fmTruncate) == SBSftpCommon.Unit.fmTruncate))
			{
				mode = FileMode.Create;
			} 
			else if (((Modes & SBSftpCommon.Unit.fmCreate) == SBSftpCommon.Unit.fmCreate) &&
				((Modes & SBSftpCommon.Unit.fmExcl) == SBSftpCommon.Unit.fmExcl))
			{
				if (System.IO.File.Exists(realpath)) 
				{
					Comment = "Cannot open file";
					ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FILE_ALREADY_EXISTS;
					return;
				} 
				else 
				{
					mode = FileMode.Create;
				}
			} 
			else 
			{
				if ((Modes & SBSftpCommon.Unit.fmCreate) == SBSftpCommon.Unit.fmCreate) 
				{
					mode = FileMode.Create;
				} 
				else 
				{
					mode = FileMode.Open;
				}
			}
			if (((Modes & SBSftpCommon.Unit.fmRead) == SBSftpCommon.Unit.fmRead) &&
				((Modes & SBSftpCommon.Unit.fmWrite) == SBSftpCommon.Unit.fmWrite))
			{
				access = FileAccess.ReadWrite;
			} 
			else if ((Modes & SBSftpCommon.Unit.fmWrite) == SBSftpCommon.Unit.fmWrite) 
			{
				access = FileAccess.Write;
			}
			else 
			{
				access = FileAccess.Read;
			}
			try 
			{
				Data = new FileStream(realpath, mode, access);
				if ((Modes & SBSftpCommon.Unit.fmAppend) == SBSftpCommon.Unit.fmAppend) 
				{
					((FileStream)Data).Position = ((FileStream)Data).Length;
				}
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
				ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
				Comment = ex.Message;
			}
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
		private void Server_OnReadFile(object Sender, object Data, long Offset, ref byte[] Buffer, int BufferOffset, int Count, ref int Read, ref int ErrorCode, ref string Comment)
		{
			try 
			{
				Stream strm = (Stream)Data;
				Read = strm.Read(Buffer, BufferOffset, Count);
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
				ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
                Comment = ex.Message;
			}
		}

		/// <summary>
		/// Is fired, when 'Remove' request is received from client
		/// </summary>
		/// <param name="Sender">ElSFTPServer object</param>
		/// <param name="Path">Path to file or directory to remove</param>
		/// <param name="ErrorCode">Operation result code (please see the documentation)</param>
		/// <param name="Comment">Operation result comment (please see the documentation)</param>
		private void Server_OnRemove(object Sender, string Path, ref int ErrorCode, ref string Comment)
		{
			bool success = false;
            string localpath = CanonicalizePath(Path, true);
			try 
			{
				if (System.IO.Directory.Exists(localpath)) 
				{
					System.IO.Directory.Delete(localpath);
					success = true;
				} 
				if ((!success) && (System.IO.File.Exists(localpath))) 
				{
					System.IO.File.Delete(localpath);
					success = true;
				}
				if (!success) 
				{
                    throw new Exception("File does not exist");
				} 
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
                ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;           
				Comment = ex.Message;
			}
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
		private void Server_OnRenameFile(object Sender, string OldPath, string NewPath, int Flags, ref int ErrorCode, ref string Comment)
		{
			bool success = false;
			string localold = CanonicalizePath(OldPath, true);
			string localnew = CanonicalizePath(NewPath, true);
			try 
			{
				if (System.IO.File.Exists(localold)) 
				{
                    System.IO.File.Move(localold, localnew);
                    success = true;
				}
				if ((!success) && (System.IO.Directory.Exists(localold))) 
				{
					System.IO.Directory.Move(localold, localnew);
					success = true;
				}
				if (!success) 
				{
                    throw new Exception("File does not exist");
				}
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
                ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
				Comment = ex.Message;
			}
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
		private void Server_OnRequestAbsolutePath(object Sender, string Path, ref string AbsolutePath, SBSftpCommon.TSBSftpRealpathControl Control, SBStringList.TElStringList ComposePath, ref int ErrorCode, ref string Comment)
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
		private void Server_OnRequestAttributes(object Sender, string Path, bool FollowSymLinks, SBSftpCommon.TElSftpFileAttributes Attributes, ref int ErrorCode, ref string Comment)
		{
            string localpath = CanonicalizePath(Path, true);
			try 
			{
				if (System.IO.Directory.Exists(localpath)) 
				{
					DirectoryInfo info = new DirectoryInfo(localpath);
					Attributes.Size = 0;
					Attributes.Directory = true;
					Attributes.MTime = info.LastWriteTime;
				} 
				else if (System.IO.File.Exists(localpath)) 
				{
					FileInfo info = new FileInfo(localpath);
					Attributes.Size = info.Length;
					Attributes.Directory = false;
					Attributes.MTime = info.LastWriteTime;
				} 
				else 
				{
                    throw new Exception("File does not exist");
				}

				Attributes.IncludedAttributes = SBSftpCommon.Unit.saSize |
					SBSftpCommon.Unit.saMTime;
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
                ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
				Comment = ex.Message;
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
		private void Server_OnWriteFile(object Sender, object Data, long Offset, byte[] Buffer, int BufferOffset, int Count, ref int ErrorCode, ref string Comment)
		{
			try 
			{
				Stream strm = (Stream)Data;
				strm.Write(Buffer, BufferOffset, Count);
				ErrorCode = 0;
			} 
			catch(Exception ex) 
			{
				ErrorCode = SBSftpCommon.Unit.SSH_ERROR_FAILURE;
				Comment = ex.Message;
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
		private string CanonicalizePath(string relpath, bool local)
		{
			char[] delims = new char[1];
			delims[0] = '/';
			string[] pathelems = relpath.Split(delims, 1024);
			ArrayList lst = new ArrayList();
			for (int i = 0; i < pathelems.Length; i++) 
			{
				string elem = (string)pathelems[i];
				if (elem == "..") 
				{
					if (lst.Count > 0) 
					{
						lst.RemoveAt(lst.Count - 1);
					}
				} 
				else if ((elem != ".") && (elem != ""))
				{
					lst.Add(elem);
				}
			}
			string result;
			string separator;
			if (local) 
			{
				result = "C:\\";
				separator = "\\";
			} 
			else 
			{
				result = "/";
				separator = "/";
			}
			for (int i = 0; i < lst.Count; i++) 
			{
				result = result + (string)lst[i];
				if (i != lst.Count - 1) 
				{
					result = result + separator;
				}
			}
			return result;
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
		private void FillNextFileInfo(TElSftpFileInfo info, ref int ErrorCode)
		{
			TElSftpFileAttributes attrs = info.Attributes;
			if (m_dirIndex < m_dirList.Length) 
			{
				info.Name = System.IO.Path.GetFileName(m_dirList[m_dirIndex]);
				attrs.CTime = System.IO.Directory.GetCreationTime(m_dirList[m_dirIndex]);
				attrs.ATime = System.IO.Directory.GetLastAccessTime(m_dirList[m_dirIndex]);
				attrs.MTime = System.IO.Directory.GetLastWriteTime(m_dirList[m_dirIndex]);
				attrs.Size = 0;
				attrs.Directory = true;
				attrs.FileType = SBSftpCommon.TSBSftpFileType.ftDirectory;
				m_dirIndex++;
			} 
			else if (m_fileIndex < m_fileList.Length) 
			{
				info.Name = System.IO.Path.GetFileName(m_fileList[m_fileIndex]);
				attrs.CTime = System.IO.File.GetCreationTime(m_fileList[m_fileIndex]);
				attrs.ATime = System.IO.File.GetLastAccessTime(m_fileList[m_fileIndex]);
				attrs.MTime = System.IO.File.GetLastWriteTime(m_fileList[m_fileIndex]);
				System.IO.FileInfo i = new FileInfo(m_fileList[m_fileIndex]);
				attrs.Size = i.Length;
				attrs.Directory = false;
				attrs.FileType = SBSftpCommon.TSBSftpFileType.ftFile;
				m_fileIndex++;
			} 
			else 
			{
				ErrorCode = SBSftpCommon.Unit.SSH_ERROR_EOF;
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

		#region Class members

		private TElSSHSubsystemThread m_thread;
		private string[] m_dirList = null;
		private string[] m_fileList = null;
		private int m_dirIndex = 0;
		private int m_fileIndex = 0;

		#endregion

	}
}
