// NAnt - A .NET build tool
// Copyright (C) 2001-2002 Gerry Shaw
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
// Kevin Connors (connors@karmet.com)
// Ian MacLean (ian_maclean@another.com)

using System.Globalization;
using System.IO;

using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;
using NAnt.Core.Util;

namespace NAnt.VisualCpp.Tasks {

    /// <summary>
    /// Compiles messages using mc.exe, Microsoft's Win32 message compiler.
    /// </summary>
    /// <example>
    ///   <para>
    ///   Compile <c>text.mc</c> using the default options.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <mc mcfile="text.mc"/>
    ///     ]]>
    ///   </code>
    ///   <para>
    ///   Compile <c>text.mc</c>, passing a path to store the header, the rc 
    ///   file and some additonal options.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <mc mcfile="text.mc" headerpath=".\build" rcpath=".\build" options="-v -c -u"/>
    ///     ]]>
    ///   </code>
    /// </example>
    [TaskName("mc")]
    public class McTask : ExternalProgramBase {
        #region Private Instance Fields
        
        private string _headerpath = null;
        private string _rcpath = null;
        private string _options = null;
        private string _mcfile = null;

        #endregion Private Instance Fields
        
        #region Public Instance Properties
         
        /// <summary>
        /// Options to pass to the compiler.
        /// </summary>
        [TaskAttribute("options")]
        public string Options {
            get { return _options; }
            set { _options = value; }
        }

        /// <summary>
        /// Path to store Header file.
        /// </summary>
        [TaskAttribute("headerpath")]
        public string HeaderPath {
            get { return _headerpath; }
            set { _headerpath = value; }
        }

        /// <summary>
        /// Path to store RC file.
        /// </summary>
        [TaskAttribute("rcpath")]
        public string RCPath {
            get { return _rcpath; }
            set { _rcpath = value; }
        }

        /// <summary>
        /// Input filename.
        /// </summary>
        [TaskAttribute("mcfile", Required=true)]
        public string McFile {
            get { return _mcfile; }
            set { _mcfile = value; }
        }

        #endregion Public Instance Properties

        #region Override implementation of ExternalProgramBase
        
        /// <summary>
        /// Gets the filename of the external program to start.
        /// </summary>
        /// <value>
        /// The filename of the external program.
        /// </value>
        public override string ProgramFileName {
            get { return Name; }
        }

        /// <summary>
        /// Gets the command-line arguments for the external program.
        /// </summary>
        /// <value>
        /// The command-line arguments for the external program.
        /// </value>
        public override string ProgramArguments {
            get {
                string str = "";

                if (Verbose) {
                    str += "/v ";
                }

                if (_headerpath != null) {
                    str += string.Format(CultureInfo.InvariantCulture, "-h \"{0}\" ", HeaderPath);
                }

                if (_rcpath != null) {
                    str += string.Format(CultureInfo.InvariantCulture, "-r \"{0}\" ", RCPath);
                }

                if (_options != null) {
                    str += string.Format(CultureInfo.InvariantCulture, "{0} ", _options);
                }

                str += _mcfile;

                return str.ToString();
            }
        }

        #endregion Override implementation of ExternalProgramBase

        #region Override implementation of Task

        /// <summary>
        /// Compiles the sources.
        /// </summary>
        protected override void ExecuteTask() {
            string header = Path.Combine(HeaderPath, Path.GetFileNameWithoutExtension(McFile)) + ".h";
            string rc = Path.Combine(HeaderPath, Path.GetFileNameWithoutExtension(McFile)) + ".rc";
            if (!NeedsCompiling(header) && !NeedsCompiling(rc)) {
                Log(Level.Info, LogPrefix + "Target(s) up-to-date, not compiling: {0}", McFile);
            } else {
                Log(Level.Info, LogPrefix + "Target out of date compiling {0}", McFile);
                if (HeaderPath != null) {
                    Log(Level.Info, LogPrefix + "Header file to {0}", HeaderPath);
                }
                if (RCPath != null) {
                    Log(Level.Info, LogPrefix + "RC file to {0}", RCPath);
                }
                Log(Level.Info, "");
                base.ExecuteTask();
            }
        }

        #endregion Override implementation of Task

        #region Private Instance Methods

        /// <summary>
        /// Determine if source files need re-building.
        /// </summary>
        private bool NeedsCompiling(string DestinationFile) {
            FileInfo srcInfo = new FileInfo(Project.GetFullPath(McFile));
            if (srcInfo.Exists) {
                string dstFile = DestinationFile;

                FileInfo dstInfo = new FileInfo(Project.GetFullPath(dstFile));
                if ((!dstInfo.Exists) || (srcInfo.LastWriteTime > dstInfo.LastWriteTime)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                // if the source file doesn't exist, let the compiler throw the error
                Log(Level.Info, LogPrefix + "Source file doesn't exist!  Compiler may whine: {0}", srcInfo.FullName);
                return true;
            }
        }

        #endregion Private Instance Methods
    }
}
#if unused
Microsoft (R) Message Compiler  Version 1.00.5239
Copyright (c) Microsoft Corp 1992-1995. All rights reserved.

usage:
MC [-?vcdwso] [-m maxmsglen] [-h dirspec] [-e extension] [-r dirspec] [-x dbgFileSpec] [-u] [-U] filename.mc
-? - displays this message
-v - gives verbose output.
-c - sets the Customer bit in all the message Ids.
-d - FACILTY and SEVERITY values in header file in decimal.
Sets message values in header to decimal initially.
-w - warns if message text contains non-OS/2 compatible inserts.
-s - insert symbolic name as first line of each message.
-o - generate OLE2 header file (use HRESULT definition instead of
                                status code definition)
    -m maxmsglen - generate a warning if the size of any message exceeds
    maxmsglen characters.
    -h pathspec - gives the path of where to create the C include file
    Default is .\
    -e extension - Specify the extension for the header file.
        From 1 - 3 chars.
        -r pathspec - gives the path of where to create the RC include file
        and the binary message resource files it includes.
        Default is .\
        -x pathspec - gives the path of where to create the .dbg C include
        file that maps message Ids to their symbolic name.
        -u - input file is Unicode.
        -U - messages in .BIN file should be Unicode.
        filename.mc - gives the names of a message text file
        to compile.
        Generated files have the Archive bit cleared.
#endif
