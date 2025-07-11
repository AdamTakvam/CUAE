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

// Gerry Shaw (gerry_shaw@yahoo.com)
// Scott Hernandez (ScottHernandez@hotmail.com)

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Globalization;

using NUnit.Framework;

using Tests.NAnt.Core.Util;

namespace Tests.NAnt.Core.Tasks {
	[TestFixture]
    public class NAntTaskTest : BuildTestBase {

        const string _format = @"
            <project>
                <nant buildfile='{0}' {1}/>
            </project>";

        const string _externalBuildFile = @"
            <project>
                <echo message='External build file executed'/>
                <target name='test'>
                    <echo message='External target executed'/>
                </target>
                <target name='t2'>
                    <echo message='Second target executed'/>
                </target>
                <target name='t3'>
                    <echo message='Third target executed'/>
                </target>
                <target name='prop'>
                    <if propertyexists='test'>
                        <echo message='testprop=${test}'/>
                    </if>
                    <property name='test' value='2nd'/>
                    <echo message='testprop=${test}'/>
                </target>
            </project>";

        string _externalBuildFileName;

		[SetUp]
        protected override void SetUp() {
            base.SetUp();
            _externalBuildFileName = Path.Combine(TempDirName, "external.build");
            TempFile.CreateWithContents(_externalBuildFile, _externalBuildFileName);
        }

		[Test]		
        public void Test_Simple() {
            string result = RunBuild(FormatBuildFile(""));
            Assertion.Assert("External build should have executed." + Environment.NewLine + result, result.IndexOf("External build file executed") != -1);
            Assertion.Assert("External target should not have executed." + Environment.NewLine + result, result.IndexOf("External target executed") == -1);
        }

		[Test]
        public void Test_SingleTarget() {
            string result = RunBuild(FormatBuildFile("target='test'"));
            Assertion.Assert("External build should have executed." + Environment.NewLine + result, result.IndexOf("External build file executed") != -1);
            Assertion.Assert("External target should have executed." + Environment.NewLine + result, result.IndexOf("External target executed") != -1);
        }

		[Test]
        public void Test_MultipleTargets() {
            string result = RunBuild(FormatBuildFile("target='test t2 t3'"));
            Assertion.Assert("External build should have executed." + Environment.NewLine + result, result.IndexOf("External build file executed") != -1);
            Assertion.Assert("External target should have executed." + Environment.NewLine + result, result.IndexOf("External target executed") != -1);
            Assertion.Assert("Second target should have executed." + Environment.NewLine + result, result.IndexOf("Second target executed") != -1);
            Assertion.Assert("Third target should have executed." + Environment.NewLine + result, result.IndexOf("Third target executed") != -1);
        }

		[Test]
        public void Test_PropertyInherit() {
            string _xml = @"
                <project>
                    <property name='test' value='1st'/>
                    <nant buildfile='{0}' target='prop' {1}/>
                    <echo message='after={2}' />
                </project>";

            string result = null;
            
            //check inheritance.
            result = RunBuild(String.Format(CultureInfo.InvariantCulture, _xml, _externalBuildFileName, "inheritall='true'","${test}"));
            Assertion.Assert("Property should be inherited into nant project." + Environment.NewLine + result, result.IndexOf("testprop=1st") != -1);
            Assertion.Assert("Property should be inherited, and updatable." + Environment.NewLine + result, result.IndexOf("testprop=2nd") != -1);
            Assertion.Assert("Property should not be changed by inherited nant project." + Environment.NewLine + result, result.IndexOf("after=1st") != -1);

            //check to make sure inheritance isn't working.
            result = RunBuild(String.Format(CultureInfo.InvariantCulture, _xml, _externalBuildFileName, "inheritall='false'","${test}"));
            Assertion.Assert("Property should not be inherited into nant project." + Environment.NewLine + result, result.IndexOf("testprop=1st") == -1);
            Assertion.Assert("Property is definable." + Environment.NewLine + result, result.IndexOf("testprop=2nd") != -1);
            Assertion.Assert("Property defined in called project should not affect the caller." + Environment.NewLine + result, result.IndexOf("after=1st") != -1);
            

        }

        private string FormatBuildFile(string attributes) {
            return String.Format(CultureInfo.InvariantCulture, _format, _externalBuildFileName, attributes);
        }
    }
}
