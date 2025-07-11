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
// Shawn Van Ness (nantluver@arithex.com)
// Gerry Shaw (gerry_shaw@yahoo.com)
// Ian MacLean (ian@maclean.ms)
// Eric V. Smith (ericsmith@windsor.com)
//
// TODO: review interface for future compatibility/customizations issues

using System;
using System.IO;

using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;
using NAnt.Core.Types;

namespace NAnt.VisualCpp.Tasks {
    /// <summary>
    /// Links files using <c>link.exe</c>, Microsoft's Incremental Linker.
    /// </summary>
    /// <remarks>
    ///   <para>This task is intended for version 7.00.9466 of <c>link.exe</c>.</para>
    /// </remarks>
    /// <example>
    ///   <para>
    ///   Combine all object files in the current directory into <c>helloworld.exe</c>.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <link output="helloworld.exe">
    ///     <sources>
    ///         <includes name="*.obj" />
    ///     </sources>
    /// </link>
    ///     ]]>
    ///   </code>
    /// </example>
    [TaskName("link")]
    public class LinkTask : ExternalProgramBase {
        #region Private Instance Fields

        private string _responseFileName;
        private string _output = null;
        private FileSet _sources = new FileSet();
        private FileSet _libdirs = new FileSet();
        private string _options = null;

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
        /// The output file name.
        /// </summary>
        [TaskAttribute("output", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string Output {
            get { return _output; }
            set { _output = value; }
        }

        /// <summary>
        /// The list of files to combine into the output file.
        /// </summary>
        [BuildElement("sources")]
        public FileSet Sources {
            get { return _sources; }
        }

        /// <summary>
        /// The list of additional library directories to search.
        /// </summary>
        [BuildElement("libdirs")]
        public FileSet LibDirs {
            get { return _libdirs; }
            set { _libdirs = value; }
        }

        #endregion Public Instance Properties

        #region Override implementation of ExternalProgramBase

        /// <summary>
        /// Gets the filename of the external program to start.
        /// </summary>
        /// <value>The filename of the external program.</value>
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
            get { return "@" + "\"" + _responseFileName + "\""; }
        }

        #endregion Override implementation of ExternalProgramBase
         
        /// <summary>
        /// Determines if the output needs linking.
        /// </summary>
        protected virtual bool NeedsLinking() {
            // return true as soon as we know we need to compile

            FileInfo outputFileInfo = new FileInfo(Path.Combine(BaseDirectory, Output));
            if (!outputFileInfo.Exists) {
                return true;
            }

            //Sources Updated?
            string fileName = FileSet.FindMoreRecentLastWriteTime(Sources.FileNames, outputFileInfo.LastWriteTime);
            if (fileName != null) {
                Log(Level.Verbose, LogPrefix + "{0} is out of date, relinking.", fileName);
                return true;
            }

            return false;
        }
        #region Override implementation of Task

        /// <summary>
        /// Links the sources.
        /// </summary>
        protected override void ExecuteTask() {
            if (NeedsLinking()) {
               if (Sources.BaseDirectory == null) {
                   Sources.BaseDirectory = BaseDirectory;
               }
  
               Log(Level.Info, LogPrefix + "Linking {0} files to {1}.", Sources.FileNames.Count, Path.Combine(BaseDirectory, Output));
  
               // create temp response file to hold compiler options
               _responseFileName = Path.GetTempFileName();

               StreamWriter writer = new StreamWriter(_responseFileName);
  
               try {
                   // specify the output file
                   writer.WriteLine("/OUT:\"{0}\"", Path.Combine(BaseDirectory, Output));
  
                   // write user provided options
                   if (_options != null) {
                       writer.WriteLine(_options);
                   }
  
                   // write each of the filenames
                   foreach (string filename in Sources.FileNames) {
                       writer.WriteLine("\"{0}\"", filename);
                   }
  
                   // write each of the libdirs
                   foreach (string libdir in LibDirs.DirectoryNames) {
                       writer.WriteLine("/LIBPATH:\"{0}\"", libdir);
                   }

                   // suppresses display of the sign-on banner                    
                   writer.WriteLine("/nologo");

                   writer.Close();
  
                   if (Verbose) {
                       // display response file contents
                       Log(Level.Info, LogPrefix + "Contents of {0}.", _responseFileName);
                       StreamReader reader = File.OpenText(_responseFileName);
                       Log(Level.Info, reader.ReadToEnd());
                       reader.Close();
                   }

                   // call base class to do the actual work
                   base.ExecuteTask();
               } finally {
                   // make sure we delete response file even if an exception is thrown
                   writer.Close(); // make sure stream is closed or file cannot be deleted
                   File.Delete(_responseFileName);
                   _responseFileName = null;
               }
           }
            
        }

        #endregion Override implementation of Task
    }
}
#if unused
Microsoft (R) Incremental Linker Version 7.00.9466
Copyright (C) Microsoft Corporation.  All rights reserved.

 usage: LINK [options] [files] [@commandfile]

   options:

      /ALIGN:#
      /ALLOWBIND[:NO]
      /ASSEMBLYMODULE:filename
      /ASSEMBLYRESOURCE:filename
      /BASE:{address|@filename,key}
      /DEBUG
      /DEF:filename
      /DEFAULTLIB:library
      /DELAY:{NOBIND|UNLOAD}
      /DELAYLOAD:dll
      /DLL
      /DRIVER[:{UPONLY|WDM}]
      /ENTRY:symbol
      /EXETYPE:DYNAMIC
      /EXPORT:symbol
      /FIXED[:NO]
      /FORCE[:{MULTIPLE|UNRESOLVED}]
      /HEAP:reserve[,commit]
      /IDLOUT:filename
      /IGNOREIDL
      /IMPLIB:filename
      /INCLUDE:symbol
      /INCREMENTAL[:NO]
      /LARGEADDRESSAWARE[:NO]
      /LIBPATH:dir
      /LTCG[:{NOSTATUS|PGINSTRUMENT|PGOPTIMIZE|STATUS}]
             (PGINSTRUMENT and PGOPTIMIZE are only available for IA64)
      /MACHINE:{AM33|ARM|IA64|M32R|MIPS|MIPS16|MIPSFPU|MIPSFPU16|MIPSR41XX|
                PPC|PPCFP|SH3|SH3DSP|SH4|SH5|THUMB|TRICORE|X86}
      /MAP[:filename]
      /MAPINFO:{EXPORTS|LINES}
      /MERGE:from=to
      /MIDL:@commandfile
      /NOASSEMBLY
      /NODEFAULTLIB[:library]
      /NOENTRY
      /NOLOGO
      /OPT:{ICF[=iterations]|NOICF|NOREF|NOWIN98|REF|WIN98}
      /ORDER:@filename
      /OUT:filename
      /PDB:filename
      /PDBSTRIPPED:filename
      /PGD:filename
      /RELEASE
      /SECTION:name,[E][R][W][S][D][K][L][P][X][,ALIGN=#]
      /STACK:reserve[,commit]
      /STUB:filename
      /SUBSYSTEM:{CONSOLE|EFI_APPLICATION|EFI_BOOT_SERVICE_DRIVER|
                  EFI_ROM|EFI_RUNTIME_DRIVER|NATIVE|POSIX|WINDOWS|
                  WINDOWSCE}[,#[.##]]
      /SWAPRUN:{CD|NET}
      /TLBOUT:filename
      /TSAWARE[:NO]
      /TLBID:#
      /VERBOSE[:LIB]
      /VERSION:#[.#]
      /VXD
      /WINDOWSCE:{CONVERT|EMULATION}
      /WS:AGGRESSIVE
#endif
