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
// Ian McLean (ianm@activestate.com)
// Mitch Denny (mitch.denny@monash.net)

using System;
using System.Globalization;
using System.IO;
using System.Xml;

using NAnt.Core;
using NAnt.Core.Attributes;

namespace NAnt.Core.Tasks {
    /// <summary>
    /// Replaces text in an XML file at the location specified by an XPath 
    /// expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The location specified by the XPath expression must exist, it will
    /// not create the parent elements for you. However, provided you have
    /// a root element you could use a series of the tasks to build the
    /// XML file up if necessary.
    /// </para>
    /// </remarks>
    /// <example>
    ///     <para>
    ///     The example provided assumes that the following XML file (App.config)
    ///     exists in the current build directory.
    ///     </para>
    ///     <code>
    ///         <![CDATA[
    /// <?xml version="1.0" encoding="utf-8" ?>
    /// <configuration>
    ///     <appSettings>
    ///         <add key="server" value="testhost.somecompany.com" />
    ///     </appSettings>
    /// </configuration>
    ///         ]]>
    ///     </code>
    ///     <para>
    ///     The example will change the <c>server</c> setting in the above 
    ///     configuration from <c>testhost.somecompany.com</c> to
    ///     <c>productionhost.somecompany.com</c>.
    ///     </para>
    ///     <code>
    ///         <![CDATA[
    /// <xmlpoke
    ///     file="App.config"
    ///     xpath="/configuration/appSettings/add[@key = 'server']/@value"
    ///     value="productionhost.somecompany.com" />
    ///         ]]>
    ///     </code>
    /// </example>
    [TaskName("xmlpoke")]
    public class XmlPokeTask : Task {
        #region Private Instance Fields
        
        private string _fileName = null;
        private string _value = null;
        private string _xPathExpression = null;
        
        #endregion Private Instance Fields

        #region Public Instance Properties

        /// <summary>
        /// The file name of the file that contains the XML document that is
        /// going to be poked.
        /// </summary>
        [TaskAttribute("file", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string FileName {
            get { return (_fileName != null) ? Project.GetFullPath(_fileName) : null; }
            set { _fileName = value; }
        }

        /// <summary>
        /// The XPath expression used to select which nodes are to be modified.
        /// </summary>
        [TaskAttribute("xpath", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string XPath  {
            get { return _xPathExpression; }
            set { _xPathExpression = value; }
        }

        /// <summary>
        /// The value that replaces the contents of the selected nodes.
        /// </summary>
        [TaskAttribute("value", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion Public Instance Properties
        
        #region Override implementation of Task
         
        /// <summary>
        /// Executes the XML poke task.
        /// </summary>
        protected override void ExecuteTask() {
            try  {  
                XmlDocument document = LoadDocument(FileName);
                XmlNodeList nodes = SelectNodes(XPath, document);

                // Don't bother trying to update any nodes or save the
                // file if no nodes were found in the first place.
                if (nodes.Count > 0) {
                    UpdateNodes(nodes, Value);
                    SaveDocument(document, FileName);
                } 
            } catch (BuildException ex) {
                throw ex;
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Could not poke at XML file '{0}'.", FileName), Location, ex);
            }
        }
        #endregion Override implementation of Task
        
        #region Private Instance Methods

        /// <summary>
        /// Loads an XML document from a file on disk.
        /// </summary>
        /// <param name="fileName">
        /// The file name of the file to load the XML document from.
        /// </param>
        /// <returns>
        /// An <see cref="System.Xml.XmlDocument" /> containing
        /// the document object model representing the file.
        /// </returns>
        private XmlDocument LoadDocument(string fileName) {
            XmlDocument document = null;

            try {
                Log(Level.Verbose, LogPrefix + "Attempting to load XML document" 
                    + " in file '{0}'.", fileName);

                document = new XmlDocument();
                document.Load(fileName);

                Log(Level.Verbose, LogPrefix + "XML document in file '{0}' loaded" 
                    + " successfully.", fileName);
                return document;
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to load the XML document '{0}'.", fileName), Location,
                    ex);
            }
        }

        /// <summary>
        /// Given an XML document and an expression, returns a list of nodes
        /// which match the expression criteria.
        /// </summary>
        /// <param name="xpath">
        /// The XPath expression used to select the nodes.
        /// </param>
        /// <param name="document">
        /// The XML document that is searched.
        /// </param>
        /// <returns>
        /// An <see cref="XmlNodeList" /> containing references to the nodes 
        /// that matched the XPath expression.
        /// </returns>
        private XmlNodeList SelectNodes(string xpath, XmlDocument document) {
            XmlNodeList nodes = null;

            try {
                Log(Level.Verbose, LogPrefix + "Selecting nodes with XPath" 
                    + " expression '{0}'.", xpath);

                nodes = document.SelectNodes(xpath);

                // Report back how many we found if any. If not then
                // log a warning saying we didn't find any.
                if (nodes.Count != 0) {
                    Log(Level.Info, LogPrefix + "Found '{0}' nodes matching" 
                        + " XPath expression '{1}'.", nodes.Count, xpath);
                } else {
                    Log(Level.Warning, LogPrefix + "No matching nodes were found" 
                        + " with XPath expression '{0}'.", xpath);
                }
                return nodes;
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to select nodes with XPath expression '{0}'.",
                    xpath), Location, ex);
            }
        }

        /// <summary>
        /// Given a node list, replaces the XML within those nodes.
        /// </summary>
        /// <param name="nodes">
        /// The list of nodes to replace the contents of.
        /// </param>
        /// <param name="value">
        /// The text to replace the contents with.
        /// </param>
        private void UpdateNodes(XmlNodeList nodes, string value) {
            Log(Level.Verbose, LogPrefix + "Updating nodes with value '{0}'.",
                value);
                
            int index = 0;
            foreach (XmlNode node in nodes)  {
                Log(Level.Verbose, LogPrefix + "Updating node '{0}'.", index);
                node.InnerXml = value;
                index ++;
            }

            Log( Level.Verbose, LogPrefix + "Updated all nodes successfully.",
                value);
        }
        
        /// <summary>
        /// Saves the XML document to a file.
        /// </summary>
        /// <param name="document">The XML document to be saved.</param>
        /// <param name="fileName">The file name to save the XML document under.</param>
        private void SaveDocument(XmlDocument document, string fileName) {
            try {
                Log(Level.Verbose, LogPrefix + "Attempting to save XML document" 
                    + " to '{0}'.", fileName);

                document.Save(fileName);
                
                Log(Level.Verbose, LogPrefix + "XML document successfully saved" 
                    + " to '{0}'.", fileName);
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to save the XML document to '{0}'.", fileName), 
                    Location, ex);
            }
        }

        #endregion Private Instance Methods
    }
}