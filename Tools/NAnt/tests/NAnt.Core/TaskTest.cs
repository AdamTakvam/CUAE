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
// Scott Hernandez (ScottHernandez@hotmail.com)

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Globalization;

using NUnit.Framework;

using NAnt.Core;
using NAnt.Core.Attributes;

namespace Tests.NAnt.Core {
    /// <summary>
    /// A simple task for testing Task class.
    /// </summary>
    [TaskName("test")]
    public class TestTask : Task {
        #region Private Instance Methods

        private bool _fail = false;

        #endregion Private Instance Methods

        #region Public Instance Properties

        [TaskAttribute("fail", Required=false)]
        [BooleanValidator()]
        public bool Fail {
            get { return _fail; }
            set { _fail = value; }
        }

        [TaskAttribute("required", Required=true)]
        [StringValidatorAttribute(AllowEmpty=true)]
        public string RequiredProperty {
            get { return ""; }
            set { }
        }

        [TaskAttribute("requirednotempty", Required=true)]
        [StringValidatorAttribute(AllowEmpty=false)]
        public string RequiredNotEmptyProperty {
            get { return ""; }
            set { }
        }

        #endregion Public Instance Properties

        #region Override implementation of Task

        protected override void ExecuteTask() {
            Log(Level.Info, LogPrefix + "TestTask executed");
            Log(Level.Verbose, LogPrefix + "Verbose message");
            if (Fail) {
                throw new BuildException("TestTask failed");
            }
        }

        #endregion Override implementation of Task
    }

	[TestFixture]
    public class TaskTest : BuildTestBase {
        #region Private Static Fields

        private const string _format = @"<?xml version='1.0' ?>
           <project name='testing' default='test'>
                <!--<taskdef assembly='{0}'/>-->
                <target name='test'>
                    <test {1}/>
                </target>
            </project>";

        #endregion Private Static Fields

        #region Public Instance Methods
        
        [Test]
        public void Test_Simple() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\""));
            Assertion.Assert("Task should have executed." + Environment.NewLine + result, result.IndexOf("TestTask executed") != -1);
        }

        [Test]
        public void Test_Verbose() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" verbose='true'"));
            Assertion.Assert("Verbose message should have been displayed." + Environment.NewLine + result, result.IndexOf("Verbose message") != -1);
        }

		[Test]
        public void Test_FailOnError() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" fail=\"true\" failonerror=\"false\""));
            Assertion.Assert("Task should have failed." + Environment.NewLine + result, result.IndexOf("TestTask failed") != -1);
        }

        [Test]
        public void Test_NoFailOnError() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" fail=\"false\" failonerror=\"false\""));
            Assertion.Assert("Task should not have failed." + Environment.NewLine + result, result.IndexOf("TestTask failed") == -1);
        }

		[Test]
        public void Test_If_True() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" if=\"true\""));
            Assertion.Assert("Task should have executed." + Environment.NewLine + result, result.IndexOf("TestTask executed") != -1);
        }

		[Test]
        public void Test_If_False() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" if=\"false\""));
            Assertion.Assert("Task should not have executed." + Environment.NewLine + result, result.IndexOf("TestTask executed") == -1);
        }

		[Test]
        public void Test_Unless_False() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" unless=\"false\""));
            Assertion.Assert("Task should have executed." + Environment.NewLine + result, result.IndexOf("TestTask executed") != -1);
        }

		[Test]
        public void Test_Unless_True() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" unless=\"true\""));
            Assertion.Assert("Task should not have executed." + Environment.NewLine + result, result.IndexOf("TestTask executed") == -1);
        }

		[Test]
        public void Test_Mixture() {
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"ok\" verbose=\"true\" if=\"true\" unless=\"false\""));
            Assertion.Assert("Task should have executed." + Environment.NewLine + result, result.IndexOf("TestTask executed") != -1);
        }

        [Test]
        [ExpectedException(typeof(TestBuildException))]
        public void Test_MissingRequiredAttribute() {
            // required attribute named 'required' is missing
            string result = RunBuild(FormatBuildFile("requirednotempty=\"ok\""));
        }

        [Test]
        [ExpectedException(typeof(TestBuildException))]
        public void Test_EmptyRequiredAttribute() {
            // requirednotempty attribute does not allow empty value
            string result = RunBuild(FormatBuildFile("required=\"ok\" requirednotempty=\"\""));
        }

/*
        public void Test_UnknownAttribute() {
            try {
                string result = RunBuild(FormatBuildFile("FaIL='false'"));
                Assert("Task should have caused build error." + Environment.NewLine + result, result.IndexOf("BUILD ERROR") != -1);
            } catch (BuildException e) {
                Assert("Task should have caused build error from unknown attribute." + Environment.NewLine + e.Message, e.Message.IndexOf("Unknown attribute 'FaIl'") != -1);
            }
        }
*/

        #endregion Public Instance Methods

        #region Protected Instance Methods

        [SetUp]
        protected override void SetUp() {
            base.SetUp();
        }

        #endregion Protected Instance Methods

        #region Private Instance Methods

        private string FormatBuildFile(string attributes) {
            return string.Format(CultureInfo.InvariantCulture, _format, Assembly.GetExecutingAssembly().Location, attributes);
        }

        #endregion Private Instance Methods
    }
}
