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
// Ian MacLean (ian@maclean.ms)

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Xml;

using NAnt.Core.Attributes;
using NAnt.Core.Util;

namespace NAnt.Core.Types {
    /// <summary>
    /// Filesets are groups of files.  These files can be found in a directory 
    /// tree starting in a base directory and are matched by patterns taken 
    /// from a number of patterns.  Filesets can appear inside tasks that support 
    /// this feature or at the project level, i.e., as children of <c>&lt;project&gt;</c>.
    /// </summary>
    /// <remarks>
    /// <h3>Patterns</h3>
    /// <para>
    /// As described earlier, patterns are used for the inclusion and exclusion. 
    /// These patterns look very much like the patterns used in DOS and UNIX:
    /// </para>
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             <para>'<c>*</c>' matches zero or more characters</para>
    ///             <para>For example:</para>
    ///             <para>
    ///             <c>*.cs</c> matches <c>.cs</c>, <c>x.cs</c> and <c>FooBar.cs</c>, 
    ///             but not <c>FooBar.xml</c> (does not end with <c>.cs</c>).
    ///             </para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <para>'<c>?</c>' matches one character</para>
    ///             <para>For example:</para>
    ///             <para>
    ///             <c>?.cs</c> matches <c>x.cs</c>, <c>A.cs</c>, but not 
    ///             <c>.cs</c> or <c>xyz.cs</c> (both don't have one character
    ///             before <c>.cs</c>).
    ///             </para>
    ///         </description>
    ///     </item>
    /// </list>
    /// <para>
    /// Combinations of <c>*</c>'s and <c>?</c>'s are allowed.
    /// </para>
    /// <para>
    /// Matching is done per-directory. This means that first the first directory 
    /// in the pattern is matched against the first directory in the path to match. 
    /// Then the second directory is matched, and so on. For example, when we have 
    /// the pattern <c>/?abc/*/*.cs</c> and the path <c>/xabc/foobar/test.cs</c>, 
    /// the first <c>?abc</c> is matched with <c>xabc</c>, then <c>*</c> is matched 
    /// with <c>foobar</c>, and finally <c>*.cs</c> is matched with <c>test.cs</c>. 
    /// They all match, so the path matches the pattern.
    /// </para>
    /// <para>
    /// To make things a bit more flexible, we added one extra feature, which makes 
    /// it possible to match multiple directory levels. This can be used to match a 
    /// complete directory tree, or a file anywhere in the directory tree. To do this, 
    /// <c>**</c> must be used as the name of a directory. When <c>**</c> is used as 
    /// the name of a directory in the pattern, it matches zero or more directories. 
    /// For example: <c>/test/**</c> matches all files/directories under <c>/test/</c>, 
    /// such as <c>/test/x.cs</c>, or <c>/test/foo/bar/xyz.html</c>, but not <c>/xyz.xml</c>.
    /// </para>
    /// <para>
    /// There is one "shorthand" - if a pattern ends with <c>/</c> or <c>\</c>, then 
    /// <c>**</c> is appended. For example, <c>mypackage/test/</c> is interpreted as 
    /// if it were <c>mypackage/test/**</c>.
    /// </para>
    /// </remarks>
    /// <history>
    /// <change date="20030224" author="Brian Deacon (bdeacon at vidya dot com">Added support for the failonempty attribute</change>
    /// </history>
    /// <example>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Pattern</term>
    ///         <description>Match</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>**/CVS/*</c></term>
    ///         <description>
    ///             <para>
    ///             Matches all files in <c>CVS</c> directories that can be 
    ///             located anywhere in the directory tree.
    ///             </para>
    ///             <para>Matches:</para>
    ///             <list type="bullet">
    ///                 <item>
    ///                     <description>CVS/Repository</description>
    ///                 </item>
    ///                 <item>
    ///                     <description>org/apache/CVS/Entries</description>
    ///                 </item>
    ///                 <item>
    ///                     <description>org/apache/jakarta/tools/ant/CVS/Entries</description>
    ///                 </item>
    ///             </list>
    ///             <para>But not:</para>
    ///             <list type="bullet">
    ///                 <item>
    ///                     <description>org/apache/CVS/foo/bar/Entries (<c>foo/bar/</c> part does not match)</description>
    ///                 </item>
    ///             </list>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><c>org/apache/jakarta/**</c></term>
    ///         <description>
    ///             <para>
    ///             Matches all files in the <c>org/apache/jakarta</c> directory 
    ///             tree.
    ///             </para>
    ///             <para>Matches:</para>
    ///             <list type="bullet">
    ///                 <item>
    ///                     <description>org/apache/jakarta/tools/ant/docs/index.html</description>
    ///                 </item>
    ///                 <item>
    ///                     <description>org/apache/jakarta/test.xml</description>
    ///                 </item>
    ///             </list>
    ///             <para>But not:</para>
    ///             <list type="bullet">
    ///                 <item>
    ///                     <description>org/apache/xyz.java (<c>jakarta/</c> part is missing)</description>
    ///                 </item>
    ///             </list>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><c>org/apache/**/CVS/*</c></term>
    ///         <description>
    ///             <para>
    ///             Matches all files in <c>CVS</c> directories that are located 
    ///             anywhere in the directory tree under <c>org/apache</c>.
    ///             </para>
    ///             <para>Matches:</para>
    ///             <list type="bullet">
    ///                 <item>
    ///                     <description>org/apache/CVS/Entries</description>
    ///                 </item>
    ///                 <item>
    ///                     <description>org/apache/jakarta/tools/ant/CVS/Entries</description>
    ///                 </item>
    ///             </list>
    ///             <para>But not:</para>
    ///             <list type="bullet">
    ///                 <item>
    ///                     <description>org/apache/CVS/foo/bar/Entries (<c>foo/bar/</c> part does not match)</description>
    ///                 </item>
    ///             </list>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term><c>**/test/**</c></term>
    ///         <description>
    ///             <para>
    ///             Matches all files that have a <c>test</c> element in their 
    ///             path, including <c>test</c> as a filename.
    ///             </para>
    ///         </description>
    ///     </item>
    /// </list>
    /// </example>
    [Serializable()]
    [ElementName("fileset")]
    public class FileSet : DataTypeBase {
        #region Private Instance Fields

        private bool _hasScanned;
        private bool _defaultExcludes = true;
        private bool _failOnEmpty;
        private string _baseDirectory;
        private DirectoryScanner _scanner = new DirectoryScanner();
        private StringCollection _asis = new StringCollection();
        private PathScanner _pathFiles = new PathScanner();

        #endregion Private Instance Fields

        #region Private Static Fields

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion Private Static Fields

        #region Public Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSet" /> class.
        /// </summary>
        public FileSet() {
        }

        #endregion Public Instance Constructors

        #region Protected Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSet" /> class from
        /// the specified <see cref="FileSet" />.
        /// </summary>
        /// <param name="source">The <see cref="FileSet" /> that should be used to create a new instance of the <see cref="FileSet" /> class.</param>
        protected FileSet(FileSet source) {
            _defaultExcludes = source._defaultExcludes;
            Location = source.Location;
            Parent = source.Parent;
            Project = source.Project;
            if ( XmlNode != null ) {
                XmlNode = source.XmlNode.Clone();
            }
        }

        #endregion Protected Instance Constructors

        #region Public Instance Properties

        /// <summary>
        /// When set to <see langword="true" />, causes the fileset element to 
        /// throw a <see cref="ValidationException" /> when no files match the 
        /// includes and excludes criteria. The default is <see langword="false" />.
        /// </summary>
        [TaskAttribute("failonempty")]
        [BooleanValidator()]
        public bool FailOnEmpty {
            get { return _failOnEmpty; }
            set { _failOnEmpty = value; }
        }

        /// <summary>
        /// Indicates whether default excludes should be used or not. 
        /// The default is <see langword="true" />.
        /// </summary>
        [TaskAttribute("defaultexcludes")]
        [BooleanValidator()]
        public bool DefaultExcludes {
            get { return _defaultExcludes; }
            set { _defaultExcludes = value; }
        }

        /// <summary>
        /// The base of the directory of this fileset. The default is the project 
        /// base directory.
        /// </summary>
        [TaskAttribute("basedir")]
        public string BaseDirectory {
            get { return (Project == null) ? _baseDirectory : Project.GetFullPath(_baseDirectory); }
            set { _baseDirectory = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Gets the collection of include patterns.
        /// </summary>
        public StringCollection Includes {
            get { return _scanner.Includes; }
        }

        /// <summary>
        /// Gets the collection of exclude patterns.
        /// </summary>
        public StringCollection Excludes {
            get { return _scanner.Excludes; }
        }

        /// <summary>
        /// Gets the collection of files that will be added to the 
        /// <see cref="FileSet" /> without pattern matching or checking if the 
        /// file exists.
        /// </summary>
        public StringCollection AsIs {
            get { return _asis; }
        }

        public PathScanner PathFiles {
            get { return _pathFiles; }
        }

        /// <summary>
        /// Gets the collection of file names that match the fileset.
        /// </summary>
        /// <value>
        /// A collection that contains the file names that match the 
        /// <see cref="FileSet" />.
        /// </value>
        public StringCollection FileNames {
            get {
                if (!_hasScanned) {
                    Scan();
                }
                return _scanner.FileNames;
            }
        }

        /// <summary>
        /// Gets the collection of directory names that match the fileset.
        /// </summary>
        /// <value>
        /// A collection that contains the directory names that match the 
        /// <see cref="FileSet" />.
        /// </value>
        public StringCollection DirectoryNames {
            get { 
                if (!_hasScanned) {
                    Scan();
                }
                return _scanner.DirectoryNames;
            }
        }

        /// <summary>
        /// Gets the collection of directory names that were scanned for files.
        /// </summary>
        /// <value>
        /// A collection that contains the directory names that were scanned for
        /// files.
        /// </value>
        public StringCollection ScannedDirectories {
            get { 
                if (!_hasScanned) {
                    Scan();
                }
                return _scanner.ScannedDirectories;
            }
        }

        /// <summary>
        /// The items to include in the fileset.
        /// </summary>
        [BuildElementArray("includes", ElementType=typeof(IncludesElement))]
        public IncludesElement[] SetIncludes {
            set {
                foreach(IncludesElement include in value) {
                    if (include.IfDefined && !include.UnlessDefined) {
                        if (include.AsIs) {
                            logger.Debug(string.Format(CultureInfo.InvariantCulture, "Including AsIs=", include.Pattern));
                            AsIs.Add(include.Pattern);
                        } else if (include.FromPath) {
                            logger.Debug(string.Format(CultureInfo.InvariantCulture, "Including FromPath=", include.Pattern));
                            PathFiles.Add(include.Pattern);
                        } else {
                            logger.Debug(string.Format(CultureInfo.InvariantCulture, "Including pattern", include.Pattern));
                            Includes.Add(include.Pattern);
                        }
                    }
                    }
                    }
                    }
        /// <summary>
        /// The items to exclude from the fileset.
        /// </summary>
        [BuildElementArray("excludes", ElementType=typeof(ExcludesElement))]
        public ExcludesElement[] SetExcludes {
            set {
                foreach(ExcludesElement exclude in value) {
                    if (exclude.IfDefined && !exclude.UnlessDefined) {
                        logger.Debug(string.Format(CultureInfo.InvariantCulture, "Excluding pattern", exclude.Pattern));
                        Excludes.Add(exclude.Pattern);
                    }
                }
            }
            }
        /// <summary>
        /// The items to include in the fileset.
        /// </summary>
        [BuildElementArray("includesList", ElementType=typeof(IncludesListElement))]
        public IncludesListElement[] SetIncludesList {
            set {
                foreach (IncludesListElement includeList in value){
                    if (includeList.IfDefined && !includeList.UnlessDefined) {
                        foreach (string s in includeList.Files) {
                            AsIs.Add(s);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Determines the most recently modified file in the fileset (by LastWriteTime of the <see cref="FileInfo"/>).
        /// </summary>
        /// <returns>
        /// The <see cref="FileInfo"/> of the file that has the newest (closest to present) last write time.
        /// </returns>
        public FileInfo MostRecentLastWriteTimeFile {
            get{
                FileInfo newestFile = null;

                foreach (string fileName in FileNames) {
                    FileInfo fileInfo = new FileInfo(fileName);
                    if(newestFile == null && fileInfo.Exists) {
                        newestFile = fileInfo;
                    }
                    if (!fileInfo.Exists) {
                        logger.Info(string.Format(CultureInfo.InvariantCulture, "File '{0}' does not exist (and is not newer than {1})", fileName, newestFile));
                        continue;
                    }
                    if (newestFile != null && fileInfo.LastWriteTime > newestFile.LastWriteTime) {
                        logger.Info(string.Format(CultureInfo.InvariantCulture, "'{0}' was newer than {1}", fileName, newestFile));
                        newestFile = fileInfo;
                    }
                }
                return newestFile;
            }
        }
        #endregion Public Instance Properties

        #region Override implementation of Element

        protected override void InitializeElement(XmlNode elementNode) {
            if (DefaultExcludes) {
                // add default exclude patterns
                Excludes.Add("**/*~");
                Excludes.Add("**/#*#");
                Excludes.Add("**/.#*");
                Excludes.Add("**/%*%");
                Excludes.Add("**/CVS");
                Excludes.Add("**/CVS/**");
                Excludes.Add("**/.cvsignore");
                Excludes.Add("**/SCCS");
                Excludes.Add("**/SCCS/**");
                Excludes.Add("**/vssver.scc");
                Excludes.Add("**/_vti_cnf/**");
            }
            base.InitializeElement(elementNode);
        }

        #endregion Override implementation of Element

        #region Override implementation of DataTypeBase

        public override void Reset() {
            // ensure that scanning will happen again for each use
            _hasScanned = false;
        }

        #endregion Override implementation of DataTypeBase

        #region Public Instance Methods

        public void Scan() {
            try {
                _scanner.BaseDirectory = BaseDirectory;

                _scanner.Scan();

                // add all the as-is patterns to the scanned files.
                foreach (string name in AsIs) {
                    if (Directory.Exists(name)) {
                        _scanner.DirectoryNames.Add(name);
                    } else {
                        _scanner.FileNames.Add(name);
                    }
                }

                // add all the path-searched patterns to the scanned files.
                foreach (string name in PathFiles.Scan()) {
                    _scanner.FileNames.Add(name);
                }

                _hasScanned = true;
            } catch (Exception ex) {
                throw new BuildException("Error creating FileSet.", Location, ex);
            }

            if (FailOnEmpty && _scanner.FileNames.Count == 0) {
                throw new ValidationException(string.Format(CultureInfo.InvariantCulture, "The fileset specified is empty after scanning '{0}' for: {1}", _scanner.BaseDirectory, _scanner.Includes.ToString()), Location);
            }
        }
        #endregion Public Instance Methods

        #region Public Static Methods

        /// <summary>
        /// Determines if a file has a more recent last write time than the 
        /// given time.
        /// </summary>
        /// <param name="fileName">A file to check the last write time against.</param>
        /// <param name="targetLastWriteTime">The datetime to compare against.</param>
        /// <returns>
        /// The name of the file that has a last write time greater than 
        /// <paramref name="targetLastWriteTime" />; otherwise, null.
        /// </returns>
        public static string FindMoreRecentLastWriteTime(string fileName, DateTime targetLastWriteTime) {
            StringCollection fileNames = new StringCollection();
            fileNames.Add(fileName);
            return FileSet.FindMoreRecentLastWriteTime(fileNames, targetLastWriteTime);
        }

        /// <summary>
        /// Determines if one of the given files has a more recent last write 
        /// time than the given time.
        /// </summary>
        /// <param name="fileNames">A collection of filenames to check the last write time against.</param>
        /// <param name="targetLastWriteTime">The datetime to compare against.</param>
        /// <returns>
        /// The name of the first file that has a last write time greater than 
        /// <paramref name="targetLastWriteTime" />; otherwise, null.
        /// </returns>
        public static string FindMoreRecentLastWriteTime(StringCollection fileNames, DateTime targetLastWriteTime) {
            foreach (string fileName in fileNames) {
                // only check fully file names that have a full path
                if (Path.IsPathRooted(fileName)) {
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Exists) {
                        logger.Info(string.Format(CultureInfo.InvariantCulture, "File '{0}' does not exist (and is not newer than {1})", fileName, targetLastWriteTime));
                        return fileName;
                    }
                    if (fileInfo.LastWriteTime > targetLastWriteTime) {
                        logger.Info(string.Format(CultureInfo.InvariantCulture, "'{0}' was newer than {1}", fileName, targetLastWriteTime));
                        return fileName;
                    }
                }
            }
            return null;
        }
        
        #endregion Public Static Methods

        // These classes provide a way of getting the Element task to initialize
        // the values from the build file.

        public class ExcludesElement : Element {
            #region Private Instance Fields

            private string _pattern;
            private bool _ifDefined = true;
            private bool _unlessDefined = false;

            #endregion Private Instance Fields

            #region Public Instance Properties

            /// <summary>
            /// The pattern or file name to include.
            /// </summary>
            [TaskAttribute("name", Required=true)]
            [StringValidator(AllowEmpty=false)]
            public string Pattern {
                get { return _pattern; }
                set { _pattern= value; }
            }

            /// <summary>
            /// If <see langword="true" /> then the pattern will be included; 
            /// otherwise, skipped. The default is <see langword="true" />.
            /// </summary>
            [TaskAttribute("if")]
            [BooleanValidator()]
            public bool IfDefined {
                get { return _ifDefined; }
                set { _ifDefined = value; }
            }

            /// <summary>
            /// Opposite of <see cref="IfDefined" />. If <see langword="false" /> 
            /// then the pattern will be included; otherwise, skipped. The default 
            /// is <see langword="false" />.
            /// </summary>
            [TaskAttribute("unless")]
            [BooleanValidator()]
            public bool UnlessDefined {
                get { return _unlessDefined; }
                set { _unlessDefined = value; }
            }

            #endregion Public Instance Properties
        }

        public class IncludesElement : ExcludesElement {
            #region Private Instance Fields

            private bool _asIs = false;
            private bool _fromPath = false;

            #endregion Private Instance Fields

            #region Public Instance Properties

            /// <summary>
            /// If <see langword="true" /> then the file name will be added to 
            /// the <see cref="FileSet" /> without pattern matching or checking 
            /// if the file exists.  The default is <see langword="false" />.
            /// </summary>
            [TaskAttribute("asis")]
            [BooleanValidator()]
            public bool AsIs {
                get { return _asIs; }
                set { _asIs = value; }
            }

            /// <summary>
            /// If <see langword="true" /> then the file will be searched for 
            /// on the path. The default is <see langword="false" />.
            /// </summary>
            [TaskAttribute("frompath")]
            [BooleanValidator()]
            public bool FromPath {
                get { return _fromPath; }
                set { _fromPath = value; }
            }

            #endregion Public Instance Properties
        }
        
        public class IncludesListElement : ExcludesElement {
            #region Private Instance Fields

            private StringCollection _files = new StringCollection();

            #endregion Private Instance Fields

            #region Public Instance Properties

            public StringCollection Files {
                get { return _files; }
            }

            #endregion Public Instance Properties

            #region Override implementation of Element

            protected override void InitializeElement(XmlNode elementNode) {
                using (Stream file = File.OpenRead(Pattern)) {
                    if (file == null) {
                        throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                            "'{0}' list could not be opened.", Pattern));
                    }
                    StreamReader rd = new StreamReader(file);
                    while (rd.Peek() > -1) {
                        _files.Add(rd.ReadLine());
                    }
                }
            }

            #endregion Override implementation of Element
        }
    
        public override string ToString() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if(!_hasScanned){
                sb.AppendFormat("Base path: {0}", _baseDirectory);
                sb.Append(Environment.NewLine);
                
                sb.Append("AsIs:");
                sb.Append(Environment.NewLine);
                sb.Append(_asis.ToString());
                sb.Append(Environment.NewLine);

                sb.Append("Files:");
                sb.Append(Environment.NewLine);
                sb.Append(_scanner.ToString());
                sb.Append(Environment.NewLine);

                sb.Append("PathFiles:");
                sb.Append(Environment.NewLine);
                sb.Append(_pathFiles.ToString());
                sb.Append(Environment.NewLine);

            } else {
                sb.Append("Files:");
                sb.Append(Environment.NewLine);
                foreach(string file in this.FileNames) {
                    sb.Append(file);
                    sb.Append(Environment.NewLine);
                }
                sb.Append("Dirs:");
                sb.Append(Environment.NewLine);
                foreach(string dir in this.DirectoryNames) {
                    sb.Append(dir);
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}
