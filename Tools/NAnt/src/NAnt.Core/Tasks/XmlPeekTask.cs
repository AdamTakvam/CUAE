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
    /// Extracts text from an XML file at the location specified by an XPath 
    /// expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the XPath expression specifies multiple nodes the node index is used 
    /// to determine which of the nodes' text is returned.
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
    ///     The example will read the server value from the above
    ///     configuration file.
    ///     </para>
    ///     <code>
    ///         <![CDATA[
    /// <xmlpeek
    ///     file="App.config"
    ///     xpath="/configuration/appSettings/add[@key = 'server']/@value"
    ///     property="configuration.server" />
    ///         ]]>
    ///     </code>
    /// </example>
    [TaskName("xmlpeek")]
    public class XmlPeekTask : Task  {
        #region Private Instance Fields

        private string _fileName = null;
        private int _nodeIndex = 0;
        private string _property = null;
        private string _xPath = null;

        #endregion Private Instance Fields

        #region Public Instance Properties
        
        /// <summary>
        /// The name of the file that contains the XML document
        /// that is going to be peeked at.
        /// </summary>
        [TaskAttribute("file", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string FileName {
            get { return (_fileName != null) ? Project.GetFullPath(_fileName) : null; }
            set { _fileName = value; }
        }

        /// <summary>
        /// The index of the node that gets its text returned when the query 
        /// returns multiple nodes.
        /// </summary>
        [TaskAttribute("nodeindex", Required=false)]
        [Int32Validator(0, Int32.MaxValue)]
        public int NodeIndex {
            get { return _nodeIndex; }
            set { _nodeIndex = value; }
        }

        /// <summary>
        /// The property that receives the text representation of the XML inside 
        /// the node returned from the XPath expression.
        /// </summary>
        [TaskAttribute("property", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string Property {
            get { return _property; }
            set { _property = value; }
        }

        /// <summary>
        /// The XPath expression used to select which node to read.
        /// </summary>
        [TaskAttribute("xpath", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string XPath  {
            get { return _xPath; }
            set { _xPath = value; }
        }
        
        #endregion Public Instance Properties

        #region Override implementation of Task

        /// <summary>
        /// Executes the XML peek task.
        /// </summary>
        protected override void ExecuteTask() {
           Log(Level.Info, LogPrefix + "Peeking at '{0}' with XPath expression '{1}'.", 
               FileName,  XPath);

           try {
                XmlDocument document = LoadDocument(FileName);
                string contents = GetNodeContents(XPath, document, NodeIndex);
                Properties[Property] = contents;
            } catch (BuildException ex) {
                throw ex; // Just re-throw the build exceptions.
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Could not peek at XML file '{0}'.", FileName), Location, ex);
            }
        }
        
        #endregion Override implementation of Task
        
        #region private Instance Methods

        /// <summary>
        /// Loads an XML document from a file on disk.
        /// </summary>
        /// <param name="fileName">The file name of the file to load the XML document from.</param>
        /// <returns>
        /// A <see cref="XmlDocument">document</see> containing
        /// the document object representing the file.
        /// </returns>
        private XmlDocument LoadDocument(string fileName)  {
            XmlDocument document = null;

            try {
                document = new XmlDocument();
                document.Load(FileName);
                return document;
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to load the XML document '{0}'.", fileName), Location, 
                    ex);
            }
        }

        /// <summary>
        /// Gets the contents of the node specified by the XPath expression.
        /// </summary>
        /// <param name="xpath">The XPath expression used to determine which nodes to choose from.</param>
        /// <param name="document">The XML document to select the nodes from.</param>
        /// <param name="nodeIndex">The node index in the case where multiple nodes satisfy the expression.</param>
        /// <returns>
        /// The contents of the node specified by the XPath expression.
        /// </returns>
        private string GetNodeContents(string xpath, XmlDocument document, int nodeIndex ) {
            string contents = null;
            XmlNodeList nodes;

            try {
                nodes = document.SelectNodes(xpath);
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to select node with XPath expression '{0}'.", xpath), 
                    Location, ex);
            }

            if (nodes == null || nodes.Count == 0) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                    "No matching nodes found for XPath expression '{0}'.", xpath), 
                    Location);
            }

            Log(Level.Info, LogPrefix + "Found '{0}' nodes with the XPath expression '{1}'.",
                nodes.Count, xpath);
          
            if (nodeIndex >= nodes.Count){
                throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                    "Nodeindex '{0}' is out of range.", nodeIndex), Location);
            }
            
            XmlNode selectedNode = nodes[nodeIndex];
            contents = selectedNode.InnerXml;
            return contents;
        }

        #endregion private Instance Methods
    }
}