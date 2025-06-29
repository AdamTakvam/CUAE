using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Tar;

using Metreos.AppArchiveCore.Xml;

namespace Metreos.AppArchiveCore
{
    public abstract class ApplicationPackage
    {
        public static bool CreateApplicationPackage(string baseDirectoryName, string packageName)
        {
            return CreateApplicationPackage(".", baseDirectoryName, packageName);
        }


        public static bool CreateApplicationPackage(string scratchDirName, 
            string baseDirectoryName, string packageName)
        {
            Debug.Assert(scratchDirName != null, "Cannot create application package with null scratchDirName");
            Debug.Assert(baseDirectoryName != null, "Cannot create application package with null baseDirectoryName");
            Debug.Assert(packageName != null, "Cannot create application package with null packageName");

            try
            {
                // Create the zip archive.
                FastZip archive = new FastZip();
                archive.CreateEmptyDirectories = true;
                archive.CreateZip(packageName, scratchDirName, true, null);
            }
            catch 
            { 
                return false; 
            }

            return true;
        }


        public static bool ExtractApplicationPackage(string packageName)
        {
            return ExtractApplicationPackage(".", packageName);
        }


        public static bool ExtractApplicationPackage(string baseDirectoryName, string packageName)
        {
            Debug.Assert(baseDirectoryName != null, "Cannot extract package with null baseDirectoryName");
            Debug.Assert(packageName != null, "Cannot extract package with null packageName");

            try
            {
                FastZip archive = new FastZip();
                archive.CreateEmptyDirectories = true;
                archive.ExtractZip(packageName, baseDirectoryName, FastZip.Overwrite.Always, null, null, null, false);
            }
            catch
            {
                return ExtractLegacyApplicationPackage(baseDirectoryName, packageName);
            }

            return true;
        }

        private static bool ExtractLegacyApplicationPackage(string baseDirectoryName, string packageName)
        {
            Debug.Assert(baseDirectoryName != null, "Cannot extract package with null baseDirectoryName");
            Debug.Assert(packageName != null, "Cannot extract package with null packageName");

            // Save the standard Console.Out TextWriter.
            System.IO.TextWriter oldConsoleOut = Console.Out;

            System.IO.FileStream inputFileStream = null;
            TarArchive archive = null;

            try
            {
                // Open a file stream for our output file archive.
                inputFileStream = new System.IO.FileStream(packageName, System.IO.FileMode.Open);
            }
            catch(Exception) { return false; }

            try
            {
                // Set the Console's output stream to null. We do this because
                // SharpZipLib's tar implementation has Console.WriteLine()'s 
                // throughout that we don't want to display.
                Console.SetOut(System.IO.TextWriter.Null);

                // Create the tar archive.
                archive = TarArchive.CreateInputTarArchive(inputFileStream);
                archive.SetKeepOldFiles(false);

                archive.ExtractContents(baseDirectoryName);
            }
            catch(Exception) { return false; }
            finally
            {
                if(archive != null) { archive.Close(); }
                if(inputFileStream != null) { inputFileStream.Close(); }

                // Reset the old Console.Out.
                Console.SetOut(oldConsoleOut);
            }

            return true;
        }


        public static manifestType LoadManifestFromFile(string manifestFilename)
        {
            Debug.Assert(manifestFilename != null, "Cannot load manifest with null manifestFilename");

            manifestType manifest = null;

            XmlSerializer serializer = null;
            System.Xml.XmlReader reader = null;

            try
            {
                serializer = new XmlSerializer(typeof(manifestType));
                reader = new System.Xml.XmlTextReader(manifestFilename);
                manifest = (manifestType)serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return null;
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }

                serializer = null;
                reader = null;
            }
                
            return manifest;
        }


        public static installType LoadApplicationInstallerFromFile(string installerFilename)
        {
            Debug.Assert(installerFilename != null, "Cannot load application with null installerFilename");

            XmlSerializer serializer = null;
            System.Xml.XmlReader reader = null;

            installType installer;

            try
            {
                serializer = new XmlSerializer(typeof(installType));
                reader = new System.Xml.XmlTextReader(installerFilename);
                installer = (installType)serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return null;
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }

                serializer = null;
                reader = null;
            }

            return installer;
        }
    }
}
