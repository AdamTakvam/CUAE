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
// Aaron Anderson (gerry_shaw@yahoo.com)
// Ian MacLean (ian@maclean.ms)

using System.Collections.Specialized;
using System.IO;
using System.Text;

using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;
using NAnt.Core.Types;
using NAnt.Core.Util;

namespace NAnt.Win32.Tasks {
    /// <summary>
    /// Imports a type library to a .NET assembly (wraps Microsoft's <c>tlbimp.exe</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This task lets you easily create interop assemblies.  By default, it will 
    /// not reimport if the underlying COM TypeLib or reference has not changed.
    /// </para>
    /// <para>
    /// <see href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</see>
    /// </para>
    /// </remarks>
    /// <example>
    ///   <para>Import <c>LegacyCOM.dll</c> to <c>DotNetAssembly.dll</c>.</para>
    ///   <code>
    ///     <![CDATA[
    /// <tlbimp typelib="LegacyCOM.dll" output="DotNetAssembly.dll" />
    ///     ]]>
    ///   </code>
    /// </example>
    [TaskName("tlbimp")]
    [ProgramLocation(LocationType.FrameworkSdkDir)]
    public class TlbImpTask : ExternalProgramBase {
        #region Private Instance Fields

        private string _output = null;
        private string _namespace = null;
        private string _asmVersion = null; 
        private bool _delaySign = false;
        private bool _primary = false;
        private string _publicKey = null;
        private string _keyFile = null;
        private string _keyContainer = null;
        private FileSet _references = new FileSet();
        private bool _strictref = false;
        private bool _sysarray = false;
        private bool _unsafe = false;
        private string _typelib = null;
        private StringBuilder _argumentBuilder = null;

        #endregion Private Instance Fields

        #region Public Instance Properties

        /// <summary>
        /// Specifies the name of the output file.
        /// </summary>
        /// <value>
        /// The name of the output file.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("output", Required=true)]
        public string Output {
            get { return (_output != null) ? Project.GetFullPath(_output) : null; }
            set { _output = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies the namespace in which to produce the assembly.
        /// </summary>
        /// <value>
        /// The namespace in which to produce the assembly.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("namespace")]
        public string Namespace {
            get { return _namespace; }
            set { _namespace = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies the version number of the assembly to produce.
        /// </summary>
        /// <remarks>
        /// <value>
        /// The version number of the assembly to produce.
        /// </value>
        /// <para>
        /// The version number should be in the format major.minor.build.revision.
        /// </para>
        /// <para>
        /// <a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a>
        /// </para>
        /// </remarks>
        [TaskAttribute("asmversion")]
        public string AsmVersion {
            get { return _asmVersion; }
            set { _asmVersion = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies whether the resulting assembly should be signed with a 
        /// strong name using delayed signing. The default is <see langword="false" />.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the resulting assembly should be signed 
        /// with a strong name using delayed signing; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("delaysign")]
        [BooleanValidator()]
        public bool DelaySign {
            get { return _delaySign; }
            set { _delaySign = value; }
        }

        /// <summary>
        /// Specifies whether a primary interop assembly should be produced for 
        /// the specified type library. The default is <see langword="false" />.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if a primary interop assembly should be 
        /// produced; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("primary")]
        [BooleanValidator()]
        public bool Primary {
            get { return _primary; }
            set { _primary = value; }
        }

        /// <summary>
        /// Specifies the file containing the public key to use to sign the 
        /// resulting assembly.
        /// </summary>
        /// <value>
        /// The file containing the public key to use to sign the resulting
        /// assembly.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("publickey")]
        public string PublicKey {
            get { return (_publicKey != null) ? Project.GetFullPath(_publicKey) : null; }
            set { _publicKey = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies the publisher's official public/private key pair with which 
        /// the resulting assembly should be signed with a strong name.
        /// </summary>
        /// <value>
        /// The keyfile to use to sign the resulting assembly with a strong name.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("keyfile")]
        public string KeyFile {
            get { return (_keyFile != null) ? Project.GetFullPath(_keyFile) : null; }
            set { _keyFile = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies the key container in which the public/private key pair 
        /// should be found that should be used to sign the resulting assembly
        /// xith a strong name.
        /// </summary>
        /// <value>
        /// The key container containing a public/private key pair that should
        /// be used to sign the resulting assembly.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("keycontainer")]
        public string KeyContainer {
            get { return _keyContainer; }
            set {_keyContainer = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies the assembly files to use to resolve references to types 
        /// defined outside the current type library. 
        /// </summary>
        /// <value>
        /// The assembly files to use to resolve references to types defined 
        /// outside the current type library.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [BuildElement("references")]
        public FileSet References {
            get { return _references; }
            set { _references = value; }
        }

        /// <summary>
        /// Specifies whether a type library should not be imported if all 
        /// references within the current assembly or the reference assemblies 
        /// cannot be resolved. The default is <see langword="false" />.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if a type library should not be imported if 
        /// all references cannot be resolved; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("strictref")]
        [BooleanValidator()]
        public bool StrictRef {
            get { return _strictref; }
            set { _strictref = value; }
        }

        /// <summary>
        /// Specifies whether to import a COM style SafeArray as a managed 
        /// <see cref="System.Array" /> class type. The default is <see langword="false" />.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if a COM style SafeArray should be imported 
        /// as a managed <see cref="System.Array" /> class type; otherwise, 
        /// <see langword="false" />.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("sysarray")]
        [BooleanValidator()]
        public bool SysArray {
            get { return _sysarray; }
            set { _sysarray = value; }
        }

        /// <summary>
        /// Specifies the source type library that gets passed to the type 
        /// library importer.
        /// </summary>
        /// <value>
        /// The source type library that gets passed to the type library 
        /// importer.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("typelib", Required=true)]
        public string TypeLib {
            get { return (_typelib != null) ? Project.GetFullPath(_typelib) : null; }
            set { _typelib = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies whether interfaces should be produced without .NET Framework 
        /// security checks. The default is <see langword="false" />.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if interfaces without .NET Framework security 
        /// checks should be produced; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks><a href="ms-help://MS.NETFrameworkSDK/cptools/html/cpgrftypelibraryimportertlbimpexe.htm">See the Microsoft.NET Framework SDK documentation for details.</a></remarks>
        [TaskAttribute("unsafe")]
        [BooleanValidator()]
        public bool Unsafe {
            get { return _unsafe; }
            set { _unsafe = value; }
        }

        #endregion Public Instance Properties

        #region Override implementation of ExternalProgramBase

        /// <summary>
        /// Gets the command line arguments for the external program.
        /// </summary>
        /// <value>
        /// The command line arguments for the external program.
        /// </value>
        public override string ProgramArguments {
            get { 
                if (_argumentBuilder != null) {
                    return _argumentBuilder.ToString();
                } else {
                    return null;
                }
            }
        }

        /// <summary>
        /// Imports the type library to a .NET assembly.
        /// </summary>
        protected override void ExecuteTask() {
            // check to see if any of the underlying interop dlls or the typelibs have changed
            // otherwise, it's not necessary to reimport.
            if (NeedsCompiling()) {
                // using a stringbuilder vs. StreamWriter since this program will not accept response files.
                _argumentBuilder = new StringBuilder();

                if (References.BaseDirectory == null) {
                    References.BaseDirectory = BaseDirectory;
                }

                _argumentBuilder.Append("\"" + _typelib + "\"");

                // any option that specifies a file name must be wrapped in quotes
                // to handle cases with spaces in the path.
                _argumentBuilder.AppendFormat(" /out:\"{0}\"", Output);

                // suppresses the Microsoft startup banner display
                _argumentBuilder.Append(" /nologo");

                if (AsmVersion != null) {
                    _argumentBuilder.AppendFormat(" /asmversion:{0}", AsmVersion);
                }

                if (Namespace != null) {
                    _argumentBuilder.AppendFormat(" /namespace:{0}", Namespace);
                }

                if (Primary) {
                    _argumentBuilder.Append(" /primary");
                }

                if (Unsafe) {
                    _argumentBuilder.Append(" /unsafe");
                }

                if (DelaySign) {
                    _argumentBuilder.Append(" /delaysign");
                }

                if (PublicKey != null) {
                    _argumentBuilder.AppendFormat(" /publickey:\"{0}\"", _publicKey);
                }

                if (KeyFile != null) {
                    _argumentBuilder.AppendFormat(" /keyfile:\"{0}\"", _keyFile);
                }

                if (KeyContainer != null) {
                    _argumentBuilder.AppendFormat(" /keycontainer:{0}", _keyContainer);
                }

                if (StrictRef) {
                    _argumentBuilder.Append(" /strictref");
                }

                if (SysArray) {
                    _argumentBuilder.Append(" /sysarray");
                }

                if (Verbose) {
                    // displays extra information
                    _argumentBuilder.Append(" /verbose");
                } else {
                    // suppresses all output except for errors
                    _argumentBuilder.Append(" /silent");
                }

                foreach (string fileName in References.FileNames) {
                    _argumentBuilder.AppendFormat(" /reference:\"{0}\"", fileName);
                }

                // call base class to do the work
                base.ExecuteTask();
            }
        }

        #endregion Override implementation of ExternalProgramBase

        #region Protected Instance Methods

        /// <summary>
        /// Determines whether the type library needs to be imported again.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the type library needs to be imported; 
        /// otherwise, <see langword="false" />.
        /// </returns>
        protected virtual bool NeedsCompiling() {
            // return true as soon as we know we need to compile
            FileInfo outputFileInfo = new FileInfo(Output);
            if (!outputFileInfo.Exists) {
                return true;
            }

            // check if the type library was updated since the interop assembly was generated
            string fileName = FileSet.FindMoreRecentLastWriteTime(TypeLib, outputFileInfo.LastWriteTime);
            if (fileName != null) {
                Log(Level.Info, LogPrefix + "{0} is out of date, recompiling.", fileName);
                return true;
            }

            // check if the reference assemblies were updated since the interop assembly was generated
            fileName = FileSet.FindMoreRecentLastWriteTime(References.FileNames, outputFileInfo.LastWriteTime);
            if (fileName != null) {
                Log(Level.Info, LogPrefix + "{0} is out of date, recompiling.", fileName);
                return true;
            }

            // if we made it here then we don't have to reimport the typelib.
            return false;
        }

        #endregion Protected Instance Methods
    }
}
