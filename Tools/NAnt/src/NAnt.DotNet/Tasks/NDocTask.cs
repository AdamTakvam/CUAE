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
// Gerry Shaw (gerry_shaw@yahoo.com)
// Ian MacLean (ian_maclean@another.com)

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

using NDoc.Core;

using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using NAnt.Core.Util;

namespace NAnt.DotNet.Tasks {
    /// <summary>
    /// Runs NDoc to create documentation.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   See the <see href="http://ndoc.sf.net">NDoc home page</see> for more 
    ///   information.
    ///   </para>
    /// </remarks>
    /// <example>
    ///   <para>
    ///   Document two assemblies using the MSDN documenter. The namespaces are 
    ///   documented in <c>NamespaceSummary.xml</c>.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <ndoc>
    ///     <assemblies basedir="${build.dir}">
    ///         <includes name="NAnt.exe" />
    ///         <includes name="NAnt.Core.dll" />
    ///     </assemblies>
    ///     <summaries basedir="${build.dir}">
    ///         <includes name="NamespaceSummary.xml" />
    ///     </summaries>
    ///     <documenters>
    ///         <documenter name="MSDN">
    ///             <property name="OutputDirectory" value="doc\MSDN" />
    ///             <property name="HtmlHelpName" value="NAnt" />
    ///             <property name="HtmlHelpCompilerFilename" value="hhc.exe" />
    ///             <property name="IncludeFavorites" value="False" />
    ///             <property name="Title" value="An NDoc Documented Class Library" />
    ///             <property name="SplitTOCs" value="False" />
    ///             <property name="DefaulTOC" value="" />
    ///             <property name="ShowVisualBasic" value="True" />
    ///             <property name="ShowMissingSummaries" value="True" />
    ///             <property name="ShowMissingRemarks" value="True" />
    ///             <property name="ShowMissingParams" value="True" />
    ///             <property name="ShowMissingReturns" value="True" />
    ///             <property name="ShowMissingValues" value="True" />
    ///             <property name="DocumentInternals" value="False" />
    ///             <property name="DocumentProtected" value="True" />
    ///             <property name="DocumentPrivates" value="False" />
    ///             <property name="DocumentEmptyNamespaces" value="False" />
    ///             <property name="IncludeAssemblyVersion" value="False" />
    ///             <property name="CopyrightText" value="" />
    ///             <property name="CopyrightHref" value="" />
    ///          </documenter>
    ///     </documenters> 
    /// </ndoc>
    ///     ]]>
    ///   </code>
    ///   <para>Content of <c>NamespaceSummary.xml</c> :</para>
    ///   <code>
    ///     <![CDATA[
    /// <namespaces>
    ///     <namespace name="Foo.Bar">
    ///         The <b>Foo.Bar</b> namespace reinvents the wheel.
    ///     </namespace>
    ///     <namespace name="Foo.Bar.Tests">
    ///         The <b>Foo.Bar.Tests</b> namespace ensures that the Foo.Bar namespace reinvents the wheel correctly.
    ///     </namespace>
    /// </namespaces>
    ///     ]]>
    ///   </code>
    /// </example>
    [TaskName("ndoc")]
    public class NDocTask : Task {
        #region Private Instance Fields

        private XmlNodeList _docNodes;
        private FileSet _assemblies = new FileSet();
        private FileSet _summaries = new FileSet();

        #endregion Private Instance Fields

        #region Public Instance Properties

        /// <summary>
        /// The set of assemblies to document.
        /// </summary>
        [BuildElement("assemblies")]
        public FileSet Assemblies {
            get { return _assemblies; }
            set { _assemblies = value; }
        }

        /// <summary>
        /// The set of namespace summary files.
        /// </summary>
        [BuildElement("summaries")]
        public FileSet Summaries {
            get { return _summaries; }
            set { _summaries = value; }
        }

        #endregion Public Instance Properties

        #region Override implementation of Task

        /// <summary>
        /// Initializes the taks and verifies the parameters.
        /// </summary>
        /// <param name="taskNode"><see cref="XmlNode" /> containing the XML fragment used to define this task instance.</param>
        protected override void InitializeTask(XmlNode taskNode) {
            // Expand and store the xml node
            _docNodes = taskNode.Clone().SelectNodes("nant:documenters/nant:documenter", Project.NamespaceManager);
            ExpandPropertiesInNodes(_docNodes);
            // check for valid documenters (any other validation can be done by NDoc itself at project load time)
            foreach (XmlNode node in _docNodes) {
                //skip non-nant namespace elements and special elements like comments, pis, text, etc.
                if (!(node.NodeType == XmlNodeType.Element) || !node.NamespaceURI.Equals(Project.Document.DocumentElement.NamespaceURI)) {
                    continue;
                }
                
                string documenterName = node.Attributes["name"].Value;
                IDocumenter documenter = CheckAndGetDocumenter(null, documenterName);
            }
        }

        /// <summary>
        /// Generates an NDoc project and builds the documentation.
        /// </summary>
        protected override void ExecuteTask() {
            // fix references to system assemblies
            if (Project.CurrentFramework != null) {
                foreach (string pattern in Assemblies.Includes) {
                    if (Path.GetFileName(pattern) == pattern) {
                        string frameworkDir = Project.CurrentFramework.FrameworkAssemblyDirectory.FullName;
                        string localPath = Path.Combine(Assemblies.BaseDirectory, pattern);
                        string fullPath = Path.Combine(frameworkDir, pattern);

                        if (!File.Exists(localPath) && File.Exists(fullPath)) {
                            // found a system reference
                            Assemblies.FileNames.Add(fullPath);
                        }
                    }
                }
            }

            // Make sure there is at least one included assembly.  This can't
            // be done in the InitializeTask() method because the files might
            // not have been built at startup time.
            if (Assemblies.FileNames.Count == 0) {
                string msg = "There must be at least one included assembly.";
                throw new BuildException(msg, Location);
            }

            // write documenter project settings to temp file
            string projectFileName = Path.GetTempFileName(); //@"c:\work\nant\nant.xdp";
            Log(Level.Verbose, LogPrefix + "Writing project settings to '{0}'.", projectFileName);

            XmlTextWriter writer = new XmlTextWriter(projectFileName, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("project");

            // write assemblies section
            writer.WriteStartElement("assemblies");
            foreach (string assemblyPath in Assemblies.FileNames) {
                string docPath = Path.ChangeExtension(assemblyPath, ".xml");
                writer.WriteStartElement("assembly");
                writer.WriteAttributeString("location", assemblyPath);
                writer.WriteAttributeString("documentation", docPath);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // write summaries section
            StringBuilder sb = new StringBuilder();
            foreach (string summaryPath in Summaries.FileNames) {
                // write out the namespace summary nodes
                try {
                    XmlTextReader tr = new XmlTextReader(summaryPath);
                    tr.MoveToContent();   // skip XmlDeclaration  and Processing Instructions                                               
                    sb.Append(tr.ReadOuterXml());
                    tr.Close();
                } catch (IOException e) {
                    string msg = string.Format(CultureInfo.InvariantCulture, "Failed to read ndoc namespace summary file {0}.", summaryPath);
                    throw new BuildException(msg, Location, e);
                }
            }
            writer.WriteRaw(sb.ToString());

            // write out the documenters section
            writer.WriteStartElement("documenters");
            foreach (XmlNode node in _docNodes) {
                //skip non-nant namespace elements and special elements like comments, pis, text, etc.
                if (!(node.NodeType == XmlNodeType.Element) || !node.NamespaceURI.Equals(Project.Document.DocumentElement.NamespaceURI)) {
                    continue;
                }
                writer.WriteRaw(node.OuterXml);
            }
            writer.WriteEndElement();

            // end project element
            writer.WriteEndElement();
            writer.Close();

            Log(Level.Verbose, "NDoc project file: file://{0}", Path.GetFullPath(projectFileName));

            // create Project object
            NDoc.Core.Project project = null;
            try {
                project = new NDoc.Core.Project();
            } catch (Exception ex) {
                throw new BuildException("Could not create NDoc Project.", Location, ex);
            }
            project.Read(projectFileName);

            foreach (XmlNode node in _docNodes) {
                //skip non-nant namespace elements and special elements like comments, pis, text, etc.
                if (!(node.NodeType == XmlNodeType.Element) || !node.NamespaceURI.Equals(Project.Document.DocumentElement.NamespaceURI)) {
                    continue;
                }
            
                string documenterName = node.Attributes["name"].Value;
                IDocumenter documenter =  CheckAndGetDocumenter(project, documenterName);

                // hook up events for feedback during the build
                documenter.DocBuildingStep += new DocBuildingEventHandler(OnDocBuildingStep);
                documenter.DocBuildingProgress += new DocBuildingEventHandler(OnDocBuildingProgress);

                // build documentation
                try {
                    documenter.Build(project);
                } catch (Exception ex) {
                    throw new BuildException(LogPrefix + "Error building documentation.", Location, ex);
                }
            }
        }

        #endregion Override implementation of Task

        #region Private Instance Methods

        /// <summary>
        /// Represents the method that will be called to update the overall 
        /// percent complete value and the current step name.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="ProgressArgs" /> that contains the event data.</param>
        private void OnDocBuildingStep(object sender, ProgressArgs e) {
            Log(Level.Info, LogPrefix + e.Status);
        }

        /// <summary>
        /// Represents the method that will be called to update the current
        /// step's precent complete value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="ProgressArgs" /> that contains the event data.</param>
        private void OnDocBuildingProgress(object sender, ProgressArgs e) {
            Log(Level.Verbose, LogPrefix + e.Progress + "% complete");
        }

        /// <summary>
        /// Returns the documenter for the given project.
        /// </summary>
        /// <exception cref="BuildException">Documenter <paramref name="documenterName" /> is not found.</exception>
        private IDocumenter CheckAndGetDocumenter(NDoc.Core.Project project, string documenterName){
            IDocumenter documenter = null;

            if (project == null) {
                project = new NDoc.Core.Project();
            }
            StringCollection documenters = new StringCollection();
            foreach (IDocumenter d in project.Documenters) {
                documenters.Add(d.Name);

                // ignore case when comparing documenter names
                if (string.Compare(d.Name, documenterName, true, CultureInfo.InvariantCulture) == 0) {
                    documenter = (IDocumenter) d;
                    break;
                }
            }

            //throw an exception if the documenter could not be found.
            if (documenter == null) {
                if (documenters.Count == 0) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "Error loading documenter '{0}'.  There are no NDoc documenters available.", documenterName), Location);
                } else {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "Error loading documenter '{0}' from available documenters ({1})." +
                        " Is the NDoc documenter assembly available?", documenterName, 
                        StringUtils.Join(", ", documenters)), Location);
                }
            }
            return documenter;
        }
 
        /// <summary>
        /// Performs macro expansion for the given nodes.
        /// </summary>
        /// <param name="nodes"><see cref="XmlNodeList" /> for which expansion should be performed.</param>
        private void ExpandPropertiesInNodes(XmlNodeList nodes) {
            foreach (XmlNode node in nodes) {
                // do not process comment nodes, or entities and other internal element types.
                if (node.NodeType == XmlNodeType.Element) {
                    ExpandPropertiesInNodes(node.ChildNodes);
                    foreach (XmlAttribute attr in node.Attributes) {
                        attr.Value = Project.ExpandProperties(attr.Value, Location);
                    }

                    // convert output directory to full path relative to project base directory
                    XmlNode outputDirProperty = (XmlNode) node.SelectSingleNode("property[@name='OutputDirectory']");
                    if (outputDirProperty != null) {
                        XmlAttribute valueAttribute = (XmlAttribute) outputDirProperty.Attributes.GetNamedItem("value");
                        if (valueAttribute != null) {
                            valueAttribute.Value = Project.GetFullPath(valueAttribute.Value);
                        }
                    }
                }
            }
        }

        #endregion Private Instance Methods
    }
}
