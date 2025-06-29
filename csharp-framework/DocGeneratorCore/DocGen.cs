using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.Diagnostics;

using Metreos.Interfaces;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Utilities;

namespace Metreos.DocGeneratorCore
{
    public class DocGen
    {
        #region Book XML sections
        // Yes, it's not clean.  The API Reference isn't really meant to read like a book,
        // So I have a hard time justifying spending alot of time on the book-generation code for the entire API document.

        private string bookXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE book PUBLIC ""-//OASIS//DTD DocBook XML V4.4//EN""
""http://www.oasis-open.org/docbook/xml/4.4/docbookx.dtd"" [
{2}
]>
<book id=""{0}"">
  <bookinfo>
    <title>{1}</title>
  </bookinfo>

{3}
</book>";

        #endregion

        public delegate void Log(TraceLevel level, string message);
        public event Log Logged;

        public DocGen(){}

        public void Generate(string pathToPackages, string outputDir, string xsltFilepath, string bookname, string bookDisplayName, string glossaryPath)
        {
            if (xsltFilepath == null || !File.Exists(xsltFilepath))
            {
                WriteLog(TraceLevel.Error, "Unable to find XSLT file for transformation at {0}", xsltFilepath);
                return;
            }

            List<PackageInfo> packages = CollectPackages(pathToPackages);

            if (packages != null)
            {
                DirectoryInfo outputDirectory = null;

                if (!Directory.Exists(outputDir))
                {
                    try
                    {
                        outputDirectory = Directory.CreateDirectory(outputDir);
                    }
                    catch(Exception e)
                    {
                        WriteLog(TraceLevel.Error, "Unable to create output directory {0}.  {1}", outputDir, e);
                    }
                }
                else
                {
                    try
                    {
                        outputDirectory = new DirectoryInfo(outputDir);
                    }
                    catch(Exception e)
                    {
                        WriteLog(TraceLevel.Error, "Unable to access output directory {0}.  {1}", outputDir, e);
                    }
                }

                string bookDir = Path.Combine(outputDir, bookname);
                DirectoryInfo bookDirectory = null;

                if (outputDirectory != null)
                {
                    if (!Directory.Exists(bookDir))
                    {
                        try
                        {
                            bookDirectory = Directory.CreateDirectory(bookDir);
                        }
                        catch (Exception e)
                        {
                            WriteLog(TraceLevel.Error, "Unable to create book directory {0}.  {1}", bookDir, e);
                        }
                    }
                    else
                    {
                        try
                        {
                            bookDirectory = new DirectoryInfo(bookDir);
                        }
                        catch (Exception e)
                        {
                            WriteLog(TraceLevel.Error, "Unable to access book directory {0}.  {1}", bookDir, e);
                        }
                    }
                }


                if (bookDirectory != null)
                {
                    List<ConvertedPackageInfo> convertedPackages = new List<ConvertedPackageInfo>();

                    foreach (PackageInfo package in packages)
                    {
                        // Create directory structure as we go.
                        string packageName = package.package is packageType ? (package.package as packageType).name : (package.package as nativeTypePackageType).name;
                        string chapterDir = Path.Combine(bookDir, packageName);
                        DirectoryInfo chapterDirectory = null;

                        if (!Directory.Exists(chapterDir))
                        {
                            try
                            {
                                chapterDirectory = Directory.CreateDirectory(chapterDir);
                            }
                            catch (Exception e)
                            {
                                WriteLog(TraceLevel.Error, "Unable to create chapter directory {0}.  {1}", chapterDir, e);
                            }
                        }
                        else
                        {
                            try
                            {
                                chapterDirectory = new DirectoryInfo(chapterDir);
                            }
                            catch (Exception e)
                            {
                                WriteLog(TraceLevel.Error, "Unable to access chapter directory {0}.  {1}", chapterDir, e);
                            }
                        }

                        if (chapterDirectory != null)
                        {
                            string chapterFilename = GenerateChapter(packageName, package.packageFilepath, xsltFilepath, chapterDirectory);

                            if (chapterFilename != null)
                            {
                                convertedPackages.Add(new ConvertedPackageInfo(chapterDirectory, chapterFilename, packageName, package.packageFilepath));
                            }
                        }
                    }

                    if(glossaryPath != null)
                    {
                        WriteLog(TraceLevel.Info, "Adding glossary at {0}.", glossaryPath);

                        if(File.Exists(glossaryPath))
                        {
                            // Create directory structure as we go.
                            string glossDir = Path.Combine(bookDir, Path.GetFileNameWithoutExtension(glossaryPath));
                            DirectoryInfo glossaryDirectory = null;

                            if(!Directory.Exists(glossDir))
                            {
                                try
                                {
                                    glossaryDirectory = Directory.CreateDirectory(glossDir);
                                }
                                catch(Exception e)
                                {
                                    WriteLog(TraceLevel.Error, "Unable to create glossary directory {0}.  {1}", glossDir, e);
                                }
                            }
                            else
                            {
                                try
                                {
                                    glossaryDirectory = new DirectoryInfo(glossDir);
                                }
                                catch(Exception e)
                                {
                                    WriteLog(TraceLevel.Error, "Unable to access glossary directory {0}.  {1}", glossDir, e);
                                }
                            }

                            if(glossaryDirectory != null)
                            {
                                string glossaryFilename = GenerateGlossary(Path.GetFileNameWithoutExtension(glossaryPath), glossaryPath, glossaryDirectory);

                                if(glossaryFilename != null)
                                {
                                    convertedPackages.Add(new ConvertedPackageInfo(glossaryDirectory, glossaryFilename, Path.GetFileNameWithoutExtension(glossaryPath), glossaryPath));
                                }
                            }
                        }
                    }

                    StringBuilder declarations = new StringBuilder();
                    StringBuilder escapes = new StringBuilder();

                    foreach(ConvertedPackageInfo convertedPackage in convertedPackages)
                    {
                        string packageName = convertedPackage.packageName;
                        // fixup packageName to fit confines of entity decl
                        packageName = packageName.Replace(" ", "");
                        packageName = packageName.Replace("-", "_");
                        packageName = packageName.Replace("@", "at");
                        packageName = packageName.Replace(".", "_");

                        string relPathToBook = Path.Combine(convertedPackage.packageName, Path.GetFileName(convertedPackage.chapterFilepath));
                        

                        string entityDecl = String.Format(@"<!ENTITY {0} SYSTEM ""{1}"">", packageName, relPathToBook);
                        string entityEscape = "&" + packageName + ";";

                        declarations.AppendLine(entityDecl);
                        escapes.AppendLine(entityEscape);
                    }

                    string formattedBookXml = String.Format(bookXml, bookname, bookDisplayName, declarations, escapes);

                    // Write book to disk

                    FileStream bookStream = null;
                    string bookPath = Path.Combine(bookDir, Path.ChangeExtension(bookname, ".xml"));

                    try
                    {
                        bookStream = File.Create(bookPath);
                    }
                    catch (Exception e)
                    {
                        WriteLog(TraceLevel.Error, "Unable to create the book file {0}.  {1}", bookPath, e);
                    }

                    if (bookStream != null)
                    {
                        StreamWriter writer = new StreamWriter(bookStream);
                        writer.Write(formattedBookXml);
                        writer.Close();
                        writer.Dispose();

                        WriteLog(TraceLevel.Info, "Book xml created at {0}", bookPath);
                    }
                }
            }
        }

        private List<PackageInfo> CollectPackages(string pathtoPackages)
        {
            List<PackageInfo> packages = new List<PackageInfo>();
            
            if (Directory.Exists(pathtoPackages))
            {
                string[] files = Directory.GetFiles(pathtoPackages);

                foreach (string pathToPackageXml in files)
                {
                    FileStream stream = null;

                    try
                    {
                        stream = File.OpenRead(pathToPackageXml);
                    }
                    catch (Exception e)
                    {
                        WriteLog(TraceLevel.Error, "Exception in opening file {0} for reading. {1}", pathToPackageXml, e);
                    }

                    object package = null;
                    try
                    {
                        if (stream != null)
                        {
                            XmlSerializer seri = new XmlSerializer(typeof(packageType));
                            package = seri.Deserialize(stream);
                        }
                    }
                    catch
                    {
                        if (stream != null)
                        {
                            try
                            {
                                stream.Position = 0;
                                XmlSerializer seri = new XmlSerializer(typeof(nativeTypePackageType));
                                package = seri.Deserialize(stream);
                            }
                            catch (Exception exp)
                            {
                                WriteLog(TraceLevel.Error, "Exception in deserializing package XML in file {0}.  {1}", pathToPackageXml, exp);
                            }
                        }
                    }

                    if(package != null)
                    {
                        packages.Add(new PackageInfo(package, pathToPackageXml));
                    }
                }
            }
            else
            {
                WriteLog(TraceLevel.Error, "Unable to find the packages directory at location: {0}", pathtoPackages);
            }
                
            return packages;
        }


        #region Logging
        public void WriteLog(TraceLevel level, string message)
        {
            if (Logged != null)
            {
                Logged(level, message);
            }
        }

        public void WriteLog(TraceLevel level, string message, params object[] args)
        {
            if (Logged != null)
            {
                Logged(level, String.Format(message, args));
            }
        }
        #endregion
            
        #region Package to Docbook

        private string GenerateChapter(string packageName, string packagePath, string xsltFilepath, DirectoryInfo chapterDirectory)
        {
            try
            {
                string convertedDocPath = Path.Combine(chapterDirectory.FullName, packageName + ".xml");

                XslCompiledTransform xslt = new XslCompiledTransform(true);
                xslt.Load(xsltFilepath);
                xslt.Transform(packagePath, convertedDocPath);

                return convertedDocPath;
            }
            catch (Exception e)
            {
                WriteLog(TraceLevel.Error, "Unable to perform translation on {0}.  {1}", packagePath, e);
                return null;
            }
        }

        private string GenerateGlossary(string glossaryName, string glossaryFilepath, DirectoryInfo glossaryChapter)
        {
            try
            {
                string convertedDocPath = Path.Combine(glossaryChapter.FullName, glossaryName + ".xml");

                File.Copy(glossaryFilepath, convertedDocPath, true);

                return convertedDocPath;
            }
            catch (Exception e)
            {
                WriteLog(TraceLevel.Error, "Unable to file copy {0}.  {1}", glossaryFilepath, e);
                return null;
            }
        }

        #endregion
    }


    public class ConvertedPackageInfo
    {
        public ConvertedPackageInfo(DirectoryInfo chapterDirectory, string chapterFilepath, string packageName, string packageFilepath)
        {
            this.chapterDirectory = chapterDirectory;
            this.chapterFilepath = chapterFilepath;
            this.packageName = packageName;
            this.packageFilepath = packageFilepath;
        }

        public DirectoryInfo chapterDirectory;
        public string chapterFilepath;
        public string packageName;
        public string packageFilepath;
    }

    public class PackageInfo
    {
        public PackageInfo(object package, string packageFilepath)
        {
            this.package = package;
            this.packageFilepath = packageFilepath;
        }

        public object package;
        public string packageFilepath;
    }

}
