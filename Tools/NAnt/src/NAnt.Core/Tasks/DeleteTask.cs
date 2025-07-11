// NAnt - A .NET build tool
// Copyright (C) 2001-2003 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Gerry Shaw (gerry_shaw@yahoo.com)

using System;
using System.IO;
using System.Globalization;

using NAnt.Core.Attributes;
using NAnt.Core.Types;

namespace NAnt.Core.Tasks {
    /// <summary>
    /// Deletes a file, fileset or directory.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   Deletes either a single file, all files in a specified directory and 
    ///   its sub-directories, or a set of files specified by one or more filesets.
    ///   </para>
    ///   <note>
    ///   If the <see cref="FileName" /> attribute is set then the fileset contents 
    ///   will be ignored. To delete the files in the fileset ommit the 
    ///   <see cref="FileName" /> attribute in the <c>&lt;delete&gt;</c> element.
    ///   </note>
    ///   <note>
    ///   Read-only files cannot be deleted.  Use the <see cref="AttribTask" /> first 
    ///   to remove the read-only attribute.
    ///   </note>
    /// </remarks>
    /// <example>
    ///   <para>Delete a single file.</para>
    ///   <code>
    ///     <![CDATA[
    /// <delete file="myfile.txt" />
    ///     ]]>
    ///   </code>
    /// </example>
    /// <example>
    ///   <para>
    ///   Delete a directory and the contents within. If the directory does not 
    ///   exist, the task does nothing.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <delete dir="${build.dir}" failonerror="false" />
    ///     ]]>
    ///   </code>
    /// </example>
    /// <example>
    ///   <para>
    ///   Delete a set of files.  Note the lack of <see cref="FileName" /> 
    ///   attribute in the <c>&lt;delete&gt;</c> element.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <delete>
    ///     <fileset>
    ///         <includes name="${basename}-??.exe" />
    ///         <includes name="${basename}-??.pdb" />
    ///     </fileset>
    /// </delete>
    ///     ]]>
    ///   </code>
    /// </example>
    [TaskName("delete")]
    public class DeleteTask : Task {
        #region Private Instance Fields

        private string _file = null;
        private string _dir = null;
        private FileSet _fileset = new FileSet();

        #endregion Private Instance Fields

        #region Public Instance Properties

        /// <summary>
        /// The file to delete.
        /// </summary>
        [TaskAttribute("file")]
        public string FileName {
            get { return _file; }
            set { _file = value; }
        }
        
        /// <summary>
        /// The directory to delete.
        /// </summary>
        [TaskAttribute("dir")]
        public string DirectoryName {
            get { return _dir; }
            set { _dir = value; }
        }

        /// <summary>
        /// All the files in the file set will be deleted.
        /// </summary>
        [BuildElement("fileset")]
        public FileSet DeleteFileSet {
            get { return _fileset; }
            set { _fileset = value; }
        }

        #endregion Public Instance Properties

        #region Override implementation of Task

        protected override void ExecuteTask() {
            // limit task to deleting either a file, directory, or file set
            if (FileName != null && DirectoryName != null) {
                throw new BuildException("Cannot specify both 'file' and 'dir'" 
                    + " attribute in the same <delete> task.", Location);
            }

            // delete a single file
            if (FileName != null) {
                string filePath = null;

                try {
                    // determine full path
                    filePath = Project.GetFullPath(FileName);
                } catch (Exception ex) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "Could not determine path from {0}.", FileName), 
                        Location, ex);
                }

                // delete the file in verbose mode
                DeleteFile(filePath, true);
            }

            // delete the directory
            if (DirectoryName != null) {
                string dirPath = null;

                try {
                    dirPath = Project.GetFullPath(DirectoryName);
                } catch (Exception ex) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "Could not determine path from {0}.", DirectoryName), 
                        Location, ex);
                }

                if (!Directory.Exists(dirPath)) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "Cannot delete directory {0}. The directory does not exist.", 
                        dirPath), Location);
                }
                
                Log(Level.Info, LogPrefix + "Deleting directory {0}.", dirPath);
                RecursiveDeleteDirectory(dirPath);
            } else {
                if (DeleteFileSet.DirectoryNames.Count == 0) {
                    Log(Level.Info, LogPrefix + "Deleting {0} files.", DeleteFileSet.FileNames.Count);
                } else if (DeleteFileSet.FileNames.Count == 0) {
                    Log(Level.Info, LogPrefix + "Deleting {0} directories.", DeleteFileSet.DirectoryNames.Count);
                } else {
                    Log(Level.Info, LogPrefix + "Deleting {0} files and {1} directories.", DeleteFileSet.FileNames.Count, DeleteFileSet.DirectoryNames.Count);
                }

                foreach (string path in DeleteFileSet.FileNames) {
                    DeleteFile(path, Verbose);
                }

                foreach (string path in DeleteFileSet.DirectoryNames) {
                    RecursiveDeleteDirectory(path);
                }
            }
        }

        #endregion Override implementation of Task

        #region Private Instance Methods

        private void RecursiveDeleteDirectory(string path) {
            try {
                // skip the directory if it doesn't exist
                if (!Directory.Exists(path)) {
                    return;
                }

                // first, recursively delete all directories in the directory
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs) {
                    RecursiveDeleteDirectory(dir);
                }

                // next, delete all files in the directory
                string[] files = Directory.GetFiles(path);
                foreach (string file in files) {
                    try {
                        File.SetAttributes(file, FileAttributes.Normal);
                        Log(Level.Verbose, LogPrefix + "Deleting file {0}.", file);
                        File.Delete(file);
                    } catch (Exception ex) {
                        string msg = string.Format(CultureInfo.InvariantCulture, 
                            "Cannot delete file {0}.", file);
                        if (FailOnError) {
                            throw new BuildException(msg, Location, ex);
                        }
                        Log(Level.Verbose, LogPrefix + msg);
                    }
                }

                // finally, delete the directory
                File.SetAttributes(path, FileAttributes.Normal);
                Log(Level.Verbose, LogPrefix + "Deleting directory {0}.", path);
                Directory.Delete(path);
            } catch (BuildException ex) {
                throw ex;
            } catch (Exception ex) {
                string msg = string.Format(CultureInfo.InvariantCulture, 
                    "Cannot delete directory {0}.", path);
                if (FailOnError) {
                    throw new BuildException(msg, Location, ex);
                }
                Log(Level.Verbose, LogPrefix + msg);
            }
        }

        private void DeleteFile(string path, bool verbose) {
            try {
                FileInfo deleteInfo = new FileInfo(path);
                if (deleteInfo.Exists) {
                    if (verbose) {
                        Log(Level.Info, LogPrefix + "Deleting file {0}.", path);
                    }
                    if (deleteInfo.Attributes != FileAttributes.Normal) {
                        File.SetAttributes(deleteInfo.FullName, FileAttributes.Normal);
                    }
                    File.Delete(path);
                } else {
                    throw new FileNotFoundException();
                }
            } catch (Exception ex) {
                string msg = string.Format(CultureInfo.InvariantCulture, 
                    "Cannot delete file {0}.", path);
                if (FailOnError) {
                    throw new BuildException(msg, Location, ex);
                }
                Log(Level.Verbose, LogPrefix + msg);
            }
        }

        #endregion Private Instance Methods
    }
}
