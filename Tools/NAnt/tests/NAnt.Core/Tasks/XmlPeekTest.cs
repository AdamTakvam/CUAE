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
// Gert Driesen (gert.driesen@ardatis.com)

using System;
using System.IO;
using System.Globalization;
using System.Xml;

using NUnit.Framework;

using NAnt.Core;
using Tests.NAnt.Core.Util;

namespace Tests.NAnt.Core.Tasks {
    [TestFixture]    
    public class XmlPeekTest : BuildTestBase {
        #region Private Instance Fields

        private const string _projectXml = "<?xml version=\"1.0\"?>"
            + "<project>"
                + "<xmlpeek {0} property=\"configuration.server\" />"
                + "<echo message=\"configuration.server={1}\" />"
            + "</project>";

        private const string _validXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" 
            + "<configuration>" 
                + "<appSettings>"
                    + "<add key=\"server\" value=\"testhost.somecompany.com\" />"
                + "</appSettings>"
            + "</configuration>";
       
        #endregion Private Instance Fields

        #region Public Instance Methods

        [Test]
        public void Test_PeekValidXml() {
            // write xml content to file
            string xmlFile = TempFile.CreateWithContents(_validXml);

            // set-up task attributes
            string taskAttributes = string.Format(CultureInfo.InvariantCulture,
                "file=\"{0}\" xpath=\"/configuration/appSettings/add[@key ='server']/@value\"",
                xmlFile);

            // execute build
            string buildLog = RunBuild(string.Format(CultureInfo.InvariantCulture, _projectXml,
                taskAttributes, "${configuration.server}"));

            // ensure the correct node was read
            Assertion.Assert("Invalid node was retrieved.", buildLog.IndexOf("configuration.server=testhost.somecompany.com") != -1);
        }

        /// <summary>
        /// Ensures a <see cref="BuildException" /> is thrown when a nodeindex
        /// is set that is out of range.
        /// </summary>
        [Test]
        public void Test_PeekValidXmlNodeIndexOutOfRange() {
            // write xml content to file
            string xmlFile = TempFile.CreateWithContents(_validXml);

            // set-up task attributes
            string taskAttributes = string.Format(CultureInfo.InvariantCulture,
                "file=\"{0}\" xpath=\"/configuration/appSettings/add[@key ='server']/@value\"" +
                " nodeindex=\"2\"", xmlFile);

            try {
                // execute build
                RunBuild(string.Format(CultureInfo.InvariantCulture, _projectXml,
                    taskAttributes, "${configuration.server}"));
            } catch (TestBuildException ex) {
                // assert that a BuildException was the cause of the TestBuildException
                Assertion.Assert((ex.InnerException != null && ex.InnerException.GetType() == typeof(BuildException)));
            }
        }

        /// <summary>
        /// Ensures a <see cref="BuildException" /> is thrown when no nodes 
        /// match the XPath expression.
        /// </summary>
        [Test]
        public void Test_PeekValidXmlNoMatches() {
            // write xml content to file
            string xmlFile = TempFile.CreateWithContents(_validXml);

            // set-up task attributes
            string taskAttributes = string.Format(CultureInfo.InvariantCulture,
                "file=\"{0}\" xpath=\"/configuration/appSettings/add[@key ='anythingisok']/@value\"",
                xmlFile);

            try {
                // execute build
                RunBuild(string.Format(CultureInfo.InvariantCulture, _projectXml,
                    taskAttributes, "${configuration.server}"));
            } catch (TestBuildException ex) {
                // assert that a BuildException was the cause of the TestBuildException
                Assertion.Assert((ex.InnerException != null && ex.InnerException.GetType() == typeof(BuildException)));
            }
        }

        [Test]
        public void Test_PeekEmptyFile() {
            // create empty file
            string xmlFile = TempFile.Create();

            // set-up task attributes
            string taskAttributes = string.Format(CultureInfo.InvariantCulture,
                "file=\"{0}\" xpath=\"/configuration/appSettings/add[@key ='server']/@value\"",
                xmlFile);

            try {
                // execute build
                RunBuild(string.Format(CultureInfo.InvariantCulture, _projectXml,
                    taskAttributes, "${configuration.server}"));
            } catch (TestBuildException ex) {
                // assert that a BuildException was the cause of the TestBuildException
                Assertion.Assert((ex.InnerException != null && ex.InnerException.GetType() == typeof(BuildException)));
                // assert that an XmlException was the cause of the BuildException
                Assertion.Assert((ex.InnerException.InnerException != null && ex.InnerException.InnerException.GetType() == typeof(XmlException)));
            }
        }

        #endregion Public Instance Methods
   }
}
