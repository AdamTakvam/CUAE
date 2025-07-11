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

using System.Globalization;
using System.IO;

using NUnit.Framework;

using Tests.NAnt.Core;
using Tests.NAnt.Core.Util;

using NAnt.Core;

using NAnt.DotNet.Tasks;
using NAnt.DotNet.Types;

namespace Tests.NAnt.DotNet.Tasks {
    [TestFixture]
    public class CscTaskTest : BuildTestBase {
        #region Private Instance Fields

        private string _sourceFileName;

        #endregion Private Instance Fields

        #region Private Static Fields

        private const string _format = @"<?xml version='1.0'?>
            <project>
                <csc target='exe' output='{0}.exe' {2}>
                    <sources basedir='{1}'>
                        <includes name='{0}'/>
                    </sources>
                </csc>
            </project>";

        private const string _sourceCode = @"
            public class HelloWorld { 
                static void Main() { 
                    System.Console.WriteLine(""Hello World using C#""); 
                }
            }";

        private const string _sourceCodeWithNamespace = @"
            namespace ResourceTest {
                public class HelloWorld { 
                    static void Main() { 
                        System.Console.WriteLine(""Hello World using C#""); 
                    }
                }
            }";

        #endregion Private Static Fields

        #region Override implementation of BuildTestBase

        [SetUp]
        protected override void SetUp() {
            base.SetUp();
            _sourceFileName = Path.Combine(TempDirName, "HelloWorld.cs");
            TempFile.CreateWithContents(_sourceCode, _sourceFileName);
        }

        #endregion Override implementation of BuildTestBase

        #region Public Instance Methods

        /// <summary>
        /// Test to make sure debug option works.
        /// </summary>
        [Test]
        public void Test_DebugBuild() {
            string result = RunBuild(FormatBuildFile("debug='true'"));
            Assertion.Assert(_sourceFileName + ".exe does not exists, program did compile.", File.Exists(_sourceFileName + ".exe"));
            Assertion.Assert(_sourceFileName + ".pdb does not exists, program did compile with debug switch.", File.Exists(_sourceFileName + ".pdb"));
        }

        /// <summary>
        /// Test to make sure debug option works.
        /// </summary>
        [Test]
        public void Test_ReleaseBuild() {
            string result = RunBuild(FormatBuildFile("debug='false'"));
            Assertion.Assert(_sourceFileName + ".exe does not exists, program did compile.", File.Exists(_sourceFileName + ".exe"));
            Assertion.Assert(_sourceFileName + ".pdb does exists, program did compiled with debug switch.", !File.Exists(_sourceFileName + ".pdb"));
        }

        [Test]
        [ExpectedException(typeof(BuildException))]
        public void Test_ManifestResourceName_NonExistingResource() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.DynamicPrefix = true;

            cscTask.GetManifestResourceName(resources, "I_dont_exist.txt");
        }

        [Test]
        public void Test_ManifestResourceName_Resx_StandAlone_DynamicPrefix() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.DynamicPrefix = true;

            // holds the path to the resource file
            string resourceFile = null;

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("ResourceFile.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("ResourceFile.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("SubDir" + "." + "ResourceFile.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("SubDir" + "." + "ResourceFile.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.dunno.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("SubDir" + "." + "ResourceFile.en-US.dunno.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));
        }

        [Test]
        public void Test_ManifestResourceName_Resx_StandAlone_DynamicPrefix_With_Prefix() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.Prefix = "TestNamespace";
            resources.DynamicPrefix = true;

            // holds the path to the resource file
            string resourceFile = null;

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "SubDir" + "." 
                + "ResourceFile.resources", cscTask.GetManifestResourceName(
                resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "SubDir" + "." 
                + "ResourceFile.en-US.resources", cscTask.GetManifestResourceName(
                resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.dunno.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "SubDir" + "." 
                + "ResourceFile.en-US.dunno.en-US.resources", cscTask.GetManifestResourceName(
                resources, resourceFile));
        }

        [Test]
        public void Test_ManifestResourceName_Resx_StandAlone_Prefix() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.Prefix = "TestNamespace";
            resources.DynamicPrefix = false;

            // holds the path to the resource file
            string resourceFile = null;

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.dunno.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.en-US.dunno.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));
        }

        [Test]
        public void Test_ManifestResourceName_Resx_DynamicPrefix() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.DynamicPrefix = true;

            // holds the path to the resource file
            string resourceFile = null;

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // create dependent file
            TempFile.CreateWithContents(_sourceCodeWithNamespace, Path.Combine(
                resources.BaseDirectory, "ResourceFile." + cscTask.Extension));
            // assert manifest resource name
            Assertion.AssertEquals("ResourceTest.HelloWorld.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // create dependent file
            TempFile.CreateWithContents(_sourceCodeWithNamespace, Path.Combine(
                resources.BaseDirectory, "ResourceFile.cs"));
            // assert manifest resource name
            Assertion.AssertEquals("ResourceTest.HelloWorld.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // create dependent file
            TempFile.CreateWithContents(_sourceCodeWithNamespace, Path.Combine(
                resources.BaseDirectory, "SubDir" + Path.DirectorySeparatorChar 
                + "ResourceFile." + cscTask.Extension));
            // assert manifest resource name
            Assertion.AssertEquals("ResourceTest.HelloWorld.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // create dependent file
            TempFile.CreateWithContents(_sourceCodeWithNamespace, Path.Combine(
                resources.BaseDirectory, "SubDir" + Path.DirectorySeparatorChar 
                + "ResourceFile." + cscTask.Extension));
            // assert manifest resource name
            Assertion.AssertEquals("ResourceTest.HelloWorld.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.dunno.en-US.resx");
            // create resource file
            CreateTempFile(resourceFile);
            // create dependent file
            TempFile.CreateWithContents(_sourceCodeWithNamespace, Path.Combine(
                resources.BaseDirectory, "SubDir" + Path.DirectorySeparatorChar 
                + "ResourceFile.en-US.dunno." + cscTask.Extension));
            // assert manifest resource name
            Assertion.AssertEquals("ResourceTest.HelloWorld.en-US.resources", 
                cscTask.GetManifestResourceName(resources, resourceFile));
        }

        [Test]
        public void Test_ManifestResourceName_NonResx_DynamicPrefix() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.DynamicPrefix = true;

            // holds the path to the resource file
            string resourceFile = null;

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("SubDir" + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("SubDir" + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.dunno.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals("SubDir" + "." + "ResourceFile.en-US.dunno.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));
        }

        [Test]
        public void Test_ManifestResourceName_NonResx_Prefix_With_DynamicPrefix() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.Prefix = "TestNamespace";
            resources.DynamicPrefix = true;

            // holds the path to the resource file
            string resourceFile = null;

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "SubDir" + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "SubDir" + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.dunno.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "SubDir" + "." 
                + "ResourceFile.en-US.dunno.txt", cscTask.GetManifestResourceName(
                resources, resourceFile));
        }

        [Test]
        public void Test_ManifestResourceName_NonResx_Prefix() {
            CscTask cscTask = new CscTask();
            cscTask.Project = CreateEmptyProject();
            
            ResourceFileSet resources = new ResourceFileSet();
            resources.BaseDirectory = TempDirName;
            resources.Prefix = "TestNamespace";
            resources.DynamicPrefix = false;

            // holds the path to the resource file
            string resourceFile = null;

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "ResourceFile.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));

            // initialize resource file
            resourceFile = Path.Combine(resources.BaseDirectory, "SubDir" 
                + Path.DirectorySeparatorChar + "ResourceFile.en-US.txt");
            // create resource file
            CreateTempFile(resourceFile);
            // assert manifest resource name
            Assertion.AssertEquals(resources.Prefix + "." + "ResourceFile.txt", 
                cscTask.GetManifestResourceName(resources, resourceFile));
        }

        #endregion Public Instance Methods

        #region Private Instance Methods

        private string FormatBuildFile(string attributes) {
            return string.Format(CultureInfo.InvariantCulture, _format, 
                Path.GetFileName(_sourceFileName), 
                Path.GetDirectoryName(_sourceFileName), 
                attributes);
        }

        #endregion Private Instance Methods
        
        /// <summary>
        /// Unit tests for ResourceLinkage
        /// </summary>
        [TestFixture]
        public class TestResourceLinkage {
            /// <summary>
            /// Uses a representative sampling of classname inputs to verify that the classname line can be found
            /// </summary>
            [Test]
            public void TestFindClassname() {
                // Positive test cases - classname should be found
                VerifyFindClassname( "public abstract class CompilerBase\r\n{} \r\n}", "CompilerBase" );
                VerifyFindClassname( "public abstract class Conference \r\n{}", "Conference" );
                VerifyFindClassname( "public class AssemblyAttributeEnumerator : IEnumerator {\r\n", "AssemblyAttributeEnumerator" );
                VerifyFindClassname( "internal class FolderCollection : IFolderCollection\r\n{}", "FolderCollection" );
                VerifyFindClassname( "class InstallTool\r\n{}", "InstallTool" );
                VerifyFindClassname( "internal abstract class FSObject\r\n{}", "FSObject" );
                VerifyFindClassname( "private class Enumerator : IEnumerator, ILevelCollectionEnumerator\r\n{}", "Enumerator" );
                VerifyFindClassname( "private class Enumerator: IEnumerator, ILevelCollectionEnumerator\r\n{}", "Enumerator" );
                VerifyFindClassname( "private class Enumerator:IEnumerator, ILevelCollectionEnumerator\r\n{}", "Enumerator" );
                VerifyFindClassname( "public sealed class FrameworkInfoDictionary : IDictionary, ICollection, IEnumerable, ICloneable {\r\n}", "FrameworkInfoDictionary" );
                VerifyFindClassname( "\tclass InstallTool\r\n{}", "InstallTool" );
                VerifyFindClassname( " class InstallTool\r\n{}", "InstallTool" );
                VerifyFindClassname( "internal abstract class FSObject\r\n{}", "FSObject" );
        
                // Negative test cases - no classname should be found
                VerifyFindClassname( "// this is some class here\r\n", "" );
                //VerifyFindClassname( "/* this is some class here\r\n", null );
            }
                
            /// <summary>
            /// Parses the input, ensuring the class name is found
            /// </summary>
            public void VerifyFindClassname(string input, string expectedClassname) {
                CscTask cscTask = new CscTask();
                StringReader reader = new StringReader(input);
                CompilerBase.ResourceLinkage linkage = cscTask.PerformSearchForResourceLinkage( reader );
            
                Assertion.AssertNotNull("no resourcelinkage found for " + input, linkage);
                string message = string.Format( "Failed to find expected class name {0}. Found {1} instead.", expectedClassname , linkage.ClassName ); 
                Assertion.Assert( message, (expectedClassname == linkage.ClassName ) );
            }
        }
    }
}
