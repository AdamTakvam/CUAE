using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

using Metreos.Interfaces;
using Metreos.ProviderPackagerCore.Xml;

using ICSharpCode.SharpZipLib.Zip;

namespace Metreos.ProviderPackagerCore
{
    /// <summary>Creates a provider package file</summary>
    /// <remarks>
    /// Provider.dll
    /// \Service
    /// \Service\manifest.xml
    /// \Docs
    /// \Docs\manifest.xml
    /// \Resources
    /// \Web
    /// </remarks>
    public class Packager
    {
        private ServiceManifestType serviceManifest;
        private DocumentsType docManifest;

        private readonly FileInfo providerFile;
        private readonly List<FileInfo> serviceFiles;
        private readonly List<FileInfo> referenceFiles;
        private readonly List<FileInfo> serviceRefFiles;
        private readonly List<FileInfo> resourceFiles;
        private readonly List<FileInfo> docFiles;
        private readonly List<FileInfo> webFiles;

        private string outputFile;
        public string OutputFile
        {
            get { return outputFile; }
            set { outputFile = value; }
        }

        public Packager(string providerFileStr)
        {
            this.providerFile = new FileInfo(providerFileStr);
            if(!providerFile.Exists)
                throw new FileNotFoundException("File not found: " + providerFileStr);

            this.referenceFiles = new List<FileInfo>();
            this.serviceFiles = new List<FileInfo>();
            this.serviceRefFiles = new List<FileInfo>();
            this.resourceFiles = new List<FileInfo>();
            this.docFiles = new List<FileInfo>();
            this.webFiles = new List<FileInfo>();

            this.docManifest = new DocumentsType();
            this.serviceManifest = new ServiceManifestType();
        }

        #region Create package

        public FileInfo GeneratePackage()
        {
            // Create temporary directory
            DirectoryInfo tempRoot = new DirectoryInfo(IPackager.TempDir);
            if(tempRoot.Exists)
                tempRoot.Delete(true);

            tempRoot.Create();

            try
            {
                string pName = providerFile.Name.Replace(IPackager.ProvFileExt, "");
                DirectoryInfo tempDir = tempRoot.CreateSubdirectory(pName);

                DirectoryInfo serviceDir = tempDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Service);
                DirectoryInfo docsDir = tempDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Docs);
                DirectoryInfo resDir = tempDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Resources);
                DirectoryInfo webDir = tempDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Web);

                // Copy files
                providerFile.CopyTo(Path.Combine(tempDir.FullName, providerFile.Name));

                string pdbFileStr = providerFile.FullName.Replace(providerFile.Extension, IPackager.DebugFileExt);
                FileInfo pdbFile = new FileInfo(pdbFileStr);
                if(pdbFile.Exists)
                    pdbFile.CopyTo(Path.Combine(tempDir.FullName, pdbFile.Name));

                foreach(FileInfo fInfo in referenceFiles)
                {
                    fInfo.CopyTo(Path.Combine(tempDir.FullName, fInfo.Name));
                }

                foreach(FileInfo fInfo in resourceFiles)
                {
                    fInfo.CopyTo(Path.Combine(resDir.FullName, fInfo.Name));
                }

                foreach(FileInfo fInfo in webFiles)
                {
                    fInfo.CopyTo(Path.Combine(webDir.FullName, fInfo.Name));
                }

                // Package services
                if(serviceFiles.Count > 0)
                {
                    foreach(FileInfo serviceFile in serviceFiles)
                    {
                        serviceFile.CopyTo(Path.Combine(serviceDir.FullName, serviceFile.Name));
                    }

                    if(serviceRefFiles != null)
                    {
                        foreach(FileInfo srFile in serviceRefFiles)
                        {
                            srFile.CopyTo(Path.Combine(serviceDir.FullName, srFile.Name));
                        }
                    }

                    // Build a manifest if they didn't supply one
                    if(serviceManifest.Service == null)
                    {
                        serviceManifest.Service = new ServiceType[serviceFiles.Count];
                        for(int i=0; i<serviceFiles.Count; i++)
                        {
                            FileInfo serviceFile = serviceFiles[i];
                            serviceManifest.Service[i] = new ServiceType();
                            serviceManifest.Service[i].Filename = serviceFile.Name;
                            serviceManifest.Service[i].Name = serviceFile.Name;
                            serviceManifest.Service[i].DisplayName = serviceFile.Name;
                        }                        
                    }

                    // Generate manifest file
                    string manFile = Path.Combine(serviceDir.FullName, IPackager.ManifestFile);
                    XmlSerializer serializer = new XmlSerializer(typeof(ServiceManifestType));
                    using(FileStream fStream = File.OpenWrite(manFile))
                    {
                        serializer.Serialize(fStream, serviceManifest);
                    }
                }

                // Package docs
                if(docFiles.Count > 0)
                {
                    foreach(FileInfo docFile in docFiles)
                    {
                        docFile.CopyTo(Path.Combine(docsDir.FullName, docFile.Name));
                    }

                    // Build a manifest if they didn't supply one
                    if(docManifest.Documents == null)
                    {
                        docManifest.Documents = new DocumentType[docFiles.Count];
                        for(int i=0; i<docFiles.Count; i++)
                        {
                            FileInfo docFile = docFiles[i];
                            docManifest.Documents[i] = new DocumentType();
                            docManifest.Documents[i].Filename = docFile.Name;
                            docManifest.Documents[i].DisplayName = docFile.Name;
                            docManifest.Documents[i].Size = docFile.Length;
                        }
                    }

                    // Generate manifest file
                    string manFile = Path.Combine(docsDir.FullName, IPackager.ManifestFile);
                    XmlSerializer serializer = new XmlSerializer(typeof(DocumentsType));
                    using(FileStream fStream = File.OpenWrite(manFile))
                    {
                        serializer.Serialize(fStream, docManifest);
                    }
                }

                // Format output file
                if(outputFile == null || outputFile == String.Empty)
                {
                    outputFile = providerFile.Name.Replace(providerFile.Extension, "");
                    outputFile = outputFile + IPackager.PackFileExt;
                }

                FastZip zip = new FastZip();
                zip.CreateEmptyDirectories = true;
                zip.CreateZip(outputFile, tempRoot.FullName, true, null);

                return new FileInfo(outputFile);
            }
            finally
            {
                tempRoot.Delete(true);
            }
        }
        #endregion

        #region Extract package

        /// <summary>Extracts the specified package file</summary>
        /// <param name="packageFile">Package file (.mcp)</param>
        /// <returns>Handle to provider file (.dll)</returns>
        public static FileInfo Extract(FileInfo packageFile)
        {
            return Extract(packageFile, packageFile.Directory);
        }

        /// <summary>Extracts the specified package file</summary>
        /// <param name="packageFile">Package file (.mcp)</param>
        /// <returns>Handle to provider file (.dll)</returns>
        public static FileInfo Extract(FileInfo packageFile, DirectoryInfo target)
        {
            FastZip zip = new FastZip();
            zip.CreateEmptyDirectories = true;
            zip.ExtractZip(packageFile.FullName, target.FullName, FastZip.Overwrite.Always, null, null, null, false);

            string provDirName = GetProviderDirectoryName(packageFile);
            DirectoryInfo provDir = new DirectoryInfo(Path.Combine(target.FullName, provDirName));
            if(!provDir.Exists)
                throw new DirectoryNotFoundException("Could not navigate to output directory: " + provDir.FullName);
            
            string provFilename = provDirName + IPackager.ProvFileExt;
            FileInfo provFile = new FileInfo(Path.Combine(provDir.FullName, provFilename));
            if(!provFile.Exists)
                throw new FileNotFoundException("Could not locate provider file in package: " + provFile.FullName);

            return provFile;
        }

        private static string GetProviderDirectoryName(FileInfo packageFile)
        { 
            // Inspect zip file for provider directory name
            ZipFile zipFile = new ZipFile(packageFile.FullName);
            foreach(ZipEntry entry in zipFile)
            {
                if(entry.IsDirectory)
                {
                    // Rip out the first part of any directory name we find
                    string[] dirNames = entry.Name.Split('/');
                    return dirNames[0];
                }
            }
            return null;
        }

        #endregion

        #region Add files

        public void SetDocManifest(string fileStr)
        {
            FileInfo manFile = new FileInfo(fileStr);
            if(!manFile.Exists)
                throw new FileNotFoundException("File not found: " + fileStr);

            FileStream fStream = manFile.OpenRead();
            XmlSerializer serializer = new XmlSerializer(typeof(DocumentsType));
            this.docManifest = (DocumentsType) serializer.Deserialize(fStream);
        }

        public void SetServiceManifest(string fileStr)
        {
            FileInfo manFile = new FileInfo(fileStr);
            if(!manFile.Exists)
                throw new FileNotFoundException("File not found: " + fileStr);

            FileStream fStream = manFile.OpenRead();
            XmlSerializer serializer = new XmlSerializer(typeof(ServiceManifestType));
            this.serviceManifest = (ServiceManifestType) serializer.Deserialize(fStream);
        }

        public bool AddServiceFile(string fileStr)
        {
            if(File.Exists(fileStr))
            {
                this.serviceFiles.Add(new FileInfo(fileStr));
                return true;
            }
            return false;
        }

        public bool AddReferenceFile(string fileStr)
        {
            if(File.Exists(fileStr))
            {
                this.referenceFiles.Add(new FileInfo(fileStr));
                return true;
            }
            return false;
        }

        public bool AddServiceReferenceFile(string fileStr)
        {
            if(File.Exists(fileStr))
            {
                this.serviceRefFiles.Add(new FileInfo(fileStr));
                return true;
            }
            return false;
        }

        public bool AddResourceFile(string fileStr)
        {
            if(File.Exists(fileStr))
            {
                this.resourceFiles.Add(new FileInfo(fileStr));
                return true;
            }
            return false;
        }

        public bool AddDocFile(string fileStr)
        {
            if(File.Exists(fileStr))
            {
                this.docFiles.Add(new FileInfo(fileStr));
                return true;
            }
            return false;
        }

        public bool AddWebFile(string fileStr)
        {
            if(File.Exists(fileStr))
            {
                this.webFiles.Add(new FileInfo(fileStr));
                return true;
            }
            return false;
        }
        #endregion
    }
}
