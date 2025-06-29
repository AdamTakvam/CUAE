using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using NUnit.Framework;

using Metreos.AppArchiveCore;

namespace Metreos.UnitTest.AppArchiveCoreTest
{
    [TestFixture]
    public class ApplicationPackageTest : UnitTestBase
    {
        public static string tempTxtFile = "test.txt";
        public string tempZipFile, tempTarFile;

        public ApplicationPackageTest() : base()
        {
            tempZipFile = Path.Combine(TempDir, "test.zip");
            tempTarFile = Path.Combine(TempDir, "test.tar");
        }

        #region Test Fixture Setup / Teardown
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            CreateTempDirectory();
        }

        [TestFixtureTearDown]
        public void TeardownFixture()
        {
            DeleteTempDirectory();
        }

        #endregion+

        [Test]
        public void TestExtractApplicationPackageZip()
        {
            ExtractPackage(tempZipFile);
        }

        [Test]
        public void TestExtractApplicationPackageTar()
        {
            ExtractPackage(tempTarFile);
        }

        private void ExtractPackage(string mcaFile)
        {
            // Create a temporary text file for our archiving fun
            FileInfo tempTextFile = new FileInfo(Path.Combine(TempDir, tempTxtFile));
            StreamWriter writer = new StreamWriter(tempTextFile.FullName);
            writer.WriteLine("testing");
            writer.Close();

            // Are we doing a TAR or ZIP archive?
            if(mcaFile.EndsWith("tar"))
            {
                using(nsoftware.IPWorksZip.Tar zipArchive = new nsoftware.IPWorksZip.Tar())
                {
                    zipArchive.ArchiveFile = tempTarFile;
                    zipArchive.IncludeFiles(tempTextFile.FullName);
                    zipArchive.Compress();
                }
            }
            else
            {
                using(nsoftware.IPWorksZip.Zip zipArchive = new nsoftware.IPWorksZip.Zip())
                {
                    zipArchive.ArchiveFile = tempZipFile;
                    zipArchive.IncludeFiles(tempTextFile.FullName);
                    zipArchive.Compress();
                }
            }

            tempTextFile.Delete();

            Assert.IsFalse(tempTextFile.Exists);

            // Whack the temporary file so we can re-create it again by extracting our archive
            bool extractResult = ApplicationPackage.ExtractApplicationPackage(Path.GetDirectoryName(mcaFile), mcaFile);

            Assert.IsTrue(extractResult);

            tempTextFile.Refresh();
            Assert.IsTrue(tempTextFile.Exists);
        }
    }
}
