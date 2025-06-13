using System;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.NativeType;
using Metreos.Max.Manager; 
using Metreos.Max.Drawing; 
using Metreos.Max.GlobalEvents; 
using Metreos.Max.Framework.Satellite;
using Metreos.Max.Framework.Satellite.Toolbox;
using Metreos.Max.Framework.Satellite.Explorer;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.Resources.XML;
using Metreos.AppArchiveCore;
using Northwoods.Go;
                                           


namespace Metreos.Max.Framework
{
    /// <summary>MaxMain utility methods</summary>
    public class MaxMainUtil
    {
        private MaxMain main;

        public MaxMainUtil(MaxMain main)
        {
            this.main = main; 
        }


        /// <summary>Save project to disk, non-incremental</summary>
        public bool SaveProjectFile()      
        {     
            return this.SaveProjectFile(false, true);
        }

        /// <summary>Save project to disk</summary>
        public bool SaveProjectFile(bool isIncremental, bool registerRecent)      
        {    
            if (!this.IsWritable(MaxMain.ProjectPath)) return false;

            string projectFileTempPath = Const.ProjectFileTempPath(MaxMain.ProjectFolder);
            FileStream projectFilestream = null;
            bool result = false;

            try 
            {
                projectFilestream = File.Open(projectFileTempPath, FileMode.Create);
                                             
                result = this.WriteProjectFile(projectFilestream, isIncremental);
            } 
            catch(Exception x) 
            {
                MessageBox.Show(Const.ProjectFileWriteErrorMsg(x.Message), MaxMain.ProjectPath);
            } 
             
            if (projectFilestream != null) projectFilestream.Close();

            if (result)                           // Copy temp file to project file
                result = Utl.SafeCopy(projectFileTempPath, MaxMain.ProjectPath, true, false);
      
            if (result)
            {                                      
                MaxMain.WriteStatusBarText(isIncremental || registerRecent? 
                    Const.projectSavedStatusMsg: Const.emptystr, 3000);

                if (registerRecent) Config.RecentFiles.Add(MaxMain.ProjectPath); 
            }

            Utl.SafeDelete(projectFileTempPath);
       
            return result;
        }


        /// <summary>Save project to disk</summary>
        /// <remarks>Writes project file as list of includes referencing the various
        /// app script (.app) files in the project folder. Merges existing toolbox 
        /// layout and Crownwood-serialized docking window layout into the file.
        /// Possible exceptions are many, and are handled by caller</remarks>  
        public bool WriteProjectFile(FileStream outstream, bool isIncremental)   
        {
            if  (outstream == null)  throw new Exception(Const.NullSerializationStreamMsg);
            DirectoryInfo projectDirInfo  = new DirectoryInfo(MaxMain.ProjectFolder); 
            FileInfo[]    projectDirFiles = projectDirInfo.GetFiles();  
            FileStream    tbxstream;
            XmlTextReader dockreader, tbxreader; 

            // The inner tool configuration serialization has been executed and saved  
            // to the .tbx file. We load that layout from file into a XML text reader. 
            // Any file or XML exception which might be thrown is handled by caller.
    
            string tooldatafile = Utl.GetTbxFilePath(MaxMain.ProjectPath);
            tbxstream = new FileStream(tooldatafile, FileMode.Open, FileAccess.Read);
            tbxreader = new XmlTextReader(tbxstream);
            tbxreader.WhitespaceHandling = WhitespaceHandling.None;

            // Next we load the docking window layout to a second XML text reader 

            // For interim saves we save the stale IDE layout to the project file,  
            // to avoid IDE flicker caused by Crownwood. We refresh the IDE layout 
            // prior to closing a project, and also prior to shutting down Max.

            try
            { 
                if (isIncremental)
                { 
                    try                                    
                    {      
                        dockreader = WriteStaleIdeLayout(); // Stale layout (no flicker)                     
                    }
                    catch                                 
                    {            
                        dockreader = WriteFreshIdeLayout(); // Fresh layout (flickers)
                    }
                }
                else dockreader = WriteFreshIdeLayout();
            }
            catch(Exception x)
            {
                if (tbxreader != null) tbxreader.Close();
                throw x;
            }

            if (dockreader == null) return false;

            dockreader.WhitespaceHandling = WhitespaceHandling.None;

            // Next we merge both XML streams into the project file          
    
            XmlTextWriter writer = new XmlTextWriter(outstream, Config.MaxEncoding);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement   (Const.xmlEltProject); // <MaxProject>

            writer.WriteAttributeString(Const.xmlAttrName,    MaxMain.ProjectFilename);
            writer.WriteAttributeString(Const.xmlAttrVersion, Const.CurrentProjectFileVersion);

            writer.WriteAttributeString(Const.xmlAttrCurrent, main.CurrentViewName);
            string subtype = Const.xmlValFileSubtypeApp;

            switch(main.CurrentViewType)
            {
                case MaxMain.ViewTypes.Installer: subtype = Const.xmlValFileSubtypeInstall; break;
                case MaxMain.ViewTypes.Locales:   subtype = Const.xmlValFileSubtypeLocales; break;
                case MaxMain.ViewTypes.Database:  subtype = Const.xmlValFileSubtypeDbase;   break;
                case MaxMain.ViewTypes.Media:     subtype = Const.xmlValFileSubtypeMedia;   break;
            }
            writer.WriteAttributeString(Const.xmlAttrType, subtype);

            // Finally we write all script file references to the project file 
            try 
            {
                tbxreader.MoveToContent(); 
                writer.WriteStartElement(Const.xmlEltMaxIDE); // <MaxIDE> 
                writer.WriteNode(tbxreader, false);           // <MaxTools />
        
                writer.WriteComment("//// begin IDE layout ////");        
                dockreader.MoveToContent();
                writer.WriteNode(dockreader,false);           // <DockingConfig />

                writer.WriteEndElement();                     // </MaxIDE>  
                              
                this.SerializeCustomInstallerTypes(writer);   // <CustomInstallerTypes>
                
                PmProxy.PropertiesManager.SerializeProperty   // <Properties>
                    (MaxProject.Instance.PmObjectType,            
                     MaxProject.Instance.MaxProperties, writer);

                this.SerializeFiles(writer, projectDirFiles); // <Files>

                MaxMain.wasProjectFileSaved = true;          
            }       
            finally // Exceptions are handled by caller
            {
                tbxreader.Close();
                dockreader.Close();
                writer.WriteEndElement();                     // </MaxProject>
                writer.Close();
            }
       
            return true;
        }


        /// <summary>Save stale layout on disk with project file</summary>
        private XmlTextReader WriteStaleIdeLayout()
        {
            string idefile = Utl.GetIdeFilePath(MaxMain.ProjectPath);
            FileStream stream = new FileStream(idefile, FileMode.Open, FileAccess.Read);
            return stream == null? null: new XmlTextReader(stream);
        }


        /// <summary>Save current IDE layout with project file (flickers)</summary>
        private XmlTextReader WriteFreshIdeLayout()
        {
            MemoryStream stream = null;
            byte[] layoutBytes  = null;

            try
            { 
                layoutBytes = MaxMain.DockMgr.SaveConfigToArray(Config.MaxEncoding);
            }
            catch { }  

            if (layoutBytes != null)
                stream = new MemoryStream (layoutBytes, 0, layoutBytes.Length);

            if (stream == null)
                throw new ApplicationException(Const.IdeSerializationErrorMsg);

            else return new XmlTextReader(stream);
        }


        /// <summary>Build the project</summary>
        public static void BuildProject(out int warningCount, out int errorCount)
        {
            ProjectBuilder builder = new ProjectBuilder();

            warningCount = 0;
            errorCount = 0;

            builder.GenerateApplicationXml(out warningCount, out errorCount);
            if (errorCount != 0) return;

            if (!builder.ValidateInstaller(ref errorCount))
            {
                return;
            }

            errorCount = builder.PackageApplication();  
            if (errorCount != 0) return;

            errorCount = builder.AssemblePackage();
        }


        /// <summary>Takes two arrays of strings and combines into one, removing duplicates</summary>
        private string[] CombineFolders(string[] array1, string[] array2)
        {
            ArrayList arrayOne = new ArrayList(array1);

            foreach(string otherString in array2)
            {
                foreach(string String in array1)
                {
                    if (otherString == String)
                    {
                        arrayOne.Remove(String);
                        break;
                    }
                }
            }

            arrayOne.AddRange(array2);

            string[] returnArray = new string[arrayOne.Count];
            arrayOne.CopyTo(returnArray);
            return returnArray;
        }


        /// <summary>Returns folder of a file path, while not allowing duplicate folders to occur</summary>
        /// <param name="references">Full paths to assemblies</param>
        /// <returns>Full path of folder containing assemblies</returns>
        public static string[] StripFileNames(string[] references)
        {
            if (references == null) return null;
      
            ArrayList folders = new ArrayList();

            foreach(string fullPath in references)
            {
                string folder = Path.GetDirectoryName(fullPath);

                // check that the folder isnt already accounted for.
                bool found = false;

                foreach(string submittedFolder in folders)
                {
                    if (0 == String.Compare(folder, submittedFolder))
                    {
                        found = true;
                        break;
                    }
                }

                if(!found)
                    folders.Add(folder);
            }

            string[] referencesFolders = new string[folders.Count];
            folders.CopyTo(referencesFolders);
            return referencesFolders;
        }


        /// <summary>Returns file name of a file path</summary>
        /// <param name="references">Full paths to assemblies</param>
        /// <returns>File name of assemblies</returns>
        public static string[] StripFolderNames(string[] references)
        {
            if (references == null)     return null;
            if (references.Length == 0) return null;

            string[] fileNames = new string[references.Length];

            for(int i = 0; i < references.Length; i++)
                fileNames[i] = Path.GetFileName(references[i]);

            return fileNames;
        }


        public bool DeployProject()
        {
            string[] scriptFileNames  = PeekProjectFileFiles(MaxMain.ProjectPath, Const.xmlValFileSubtypeApp);
            string[] mediaRel         = PeekProjectFileFiles(MaxMain.ProjectPath, Const.xmlValFileSubtypeMedia );
            string deployFileFolder   = Utl.GetDeployFilePath(MaxMain.ProjectPath);
            string deployFileName     = Path.ChangeExtension(MaxMain.ProjectName, Const.maxDeployFileExtension); 
            string deployFilePath     = Path.Combine(deployFileFolder, deployFileName);

            return AppDeployment.Instance.DeployAsync(
                deployFilePath, mediaRel == null ? 0 : mediaRel.Length, writer, status,
                new AppDeployment.FormInvoke(main.Invoke));
        }

        
        /// <summary>Indicate whether IP/user/pass could be determined</summary>
        public bool GetAuthenticationInfo()
        {
            string[] authresult = CheckAuthenticationInfo();
            int errcount = authresult.Length;
            if (errcount == 0) return true;            
            if (errcount >= 3) return this.PromptAuthenticationInfo(Const.DefaultAuthItemsMsg);

            StringBuilder s = new StringBuilder();

            for(int i=0; i < errcount; i++)
            {
                if (i > 0) s.Append(Const.commasp);
                s.Append(authresult[i]);
            }

            s.Append(Const.dot);

            string content = Const.MissingAuthItemsMsg + s.ToString();

            return this.PromptAuthenticationInfo(content);
        }


        /// <summary>Access and verify presence of configured IP, user name, and password</summary>
        /// <returns>Bitwise string of missing info where 1 = IP; 2 = name; 4 = password</returns>
        public string[] CheckAuthenticationInfo()
        {
            uint result = 0, errcount = 0;

            string usernamePersisted = Config.AppServerAdminUser;
            string userpassPersisted = Config.AppServerAdminPass;

            if (usernamePersisted == null || usernamePersisted.Length == 0) 
                result |= 0x1;

            if (userpassPersisted == null || userpassPersisted.Length == 0)
                result |= 0x2;

            string[] s = null; 
            string regIP = Config.AppServerIP;
            if (regIP != null) s = regIP.Split(new char[] { Const.cdot } );
            bool isIpOk = s != null && s.Length == 4;
            if (!isIpOk) result |= 0x4;

            for(uint mask = 1; mask <= 0x4; mask <<= 0x1)             
                if ((result & mask) != 0) errcount++;

            string[] errids = new string[errcount];
            if (errcount == 0) return errids;

            int ndx = 0;
            if ((result & 0x1) != 0) errids[ndx++] = Const.missingUser;
            if ((result & 0x2) != 0) errids[ndx++] = Const.missingPass;
            if ((result & 0x4) != 0) errids[ndx++] = Const.missingIP;
            return errids;
        }


        /// <summary>Prompt for and persist authentication info</summary>
        public bool PromptAuthenticationInfo(string content)
        {
            return DialogResult.OK == new MaxAuthenticationDlg(content).ShowDialog();
        }


        public static void WriteMessage(string message)
        {
            MaxMain.MessageWriter.WriteLine(message);

            // We get no status indication back from deploy other than error logging
            // so there is no way to hook a failed deployment other than parsing the message
            // if (message.IndexOf("erver inacc") > 0)
            // {
            //     string s = "Connection to the application server could not be established. "
            //      + "Please verify the IP address and retry.";
            //
            //     new MaxAuthenticationDlg(s).ShowDialog();               
            // }
        }

        public static void StatusMessage(string message)
        {
            MaxMain.MessageWriter.WriteStatusBarText(message);
        }

        public void OutputWriteCallback(string message)
        {
            main.Invoke(writer, new object[] { message });
        }

        public void UpdateStatusCallback(string message)
        {
            main.Invoke(status, new object[] { message });        
        }

        public void SerializeCustomInstallerTypes(XmlTextWriter writer)
        {
            MaxProject project = MaxProject.Instance;

            writer.WriteStartElement(Const.xmlEltCustomTypes); // <CustomInstallerTypes>

            foreach(UserConfigType customConfigItems in project.CustomConfigItems)
            {
                writer.WriteStartElement(Const.xmlEltCustomType); // <CustomInstallerType>
                writer.WriteAttributeString(Const.xmlAttrName, customConfigItems.Name); // <CustomInstallerType name="">

                foreach(string @value in customConfigItems.Values)
                {
                    writer.WriteStartElement(Const.xmlEltCustomValue); // <Value>
                    writer.WriteString(@value);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }


        /// <summary>Serialize all project files: apps installer, databases, media</summary>
        public void SerializeFiles(XmlTextWriter writer, FileInfo[] files)
        {
            writer.WriteStartElement(Const.xmlEltFiles);  // <Files>
            writer.WriteStartElement(Const.xmlEltInclude);// <Include> 

            this.SerializeAppFiles  (writer, files);

            this.SerializeInstaller (writer, files);

            this.SerializeLocales   (writer, files);

            this.SerializeDatabases (writer, files);

            this.SerializeMediaFiles(writer, files);

            this.SerializeVoiceRecFiles(writer, files);

            this.SerializeReferences(writer);
 
            writer.WriteEndElement();                     // </Include>
            writer.WriteEndElement();                     // </Files>
        }


        /// <summary>Serialize app file references from a FileInfo list</summary>
        public void SerializeAppFiles(XmlTextWriter writer, FileInfo[] files)
        {  
            if (files != null) foreach(FileInfo file in files)
            {
                if (!main.Explorer.IsProjectAppScriptFile(file.FullName)) continue;
                if (!this.IsAppInProject(Path.GetFileNameWithoutExtension(file.Name))) continue;
                writer.WriteStartElement(Const.xmlEltFile); // <File>
                writer.WriteAttributeString(Const.xmlAttrRelPath, file.Name);
                writer.WriteAttributeString(Const.xmlAttrSubType, Const.xmlValFileSubtypeApp);
                writer.WriteEndElement();                   // </File>
            }
        }


        /// <summary>Write a reference to the project installer file if present</summary>
        public void SerializeInstaller(XmlTextWriter writer, FileInfo[] files)
        {
            if (!main.Explorer.InstallerPresent) return;
            writer.WriteStartElement(Const.xmlEltFile); // <File>
            writer.WriteAttributeString(Const.xmlAttrRelPath, 
                Utl.GetRelativePath(MaxMain.ProjectFolder, 
                Utl.GetInstallerFilePath(MaxMain.ProjectPath)));
            writer.WriteAttributeString(Const.xmlAttrSubType, Const.xmlValFileSubtypeInstall);
            writer.WriteEndElement();                   // </File>
        }


        /// <summary>Write a reference to the project locales file if present</summary>
        public void SerializeLocales(XmlTextWriter writer, FileInfo[] files)
        {
            if (!main.Explorer.LocalesPresent) return;
            writer.WriteStartElement(Const.xmlEltFile); // <File>
            writer.WriteAttributeString(Const.xmlAttrRelPath,
                Utl.GetRelativePath(MaxMain.ProjectFolder,
                Utl.GetLocalesFilePath(MaxMain.ProjectPath)));
            writer.WriteAttributeString(Const.xmlAttrSubType, Const.xmlValFileSubtypeLocales);
            writer.WriteEndElement();                   // </File>
        }


        /// <summary>Write a reference to each project database script</summary>
        public void SerializeDatabases(XmlTextWriter writer, FileInfo[] files)
        {
            MaxFolderTreeNode dbroot = main.Explorer.DbaseFolderNode;
            if (dbroot == null) return;

            foreach(object x in dbroot.Nodes)
            {        
                string path = null;
                MaxDbScriptTreeNode node = x as MaxDbScriptTreeNode;
                MaxExplorerWindow.MediaInfo info = node == null? null:
                    node.Tag as MaxExplorerWindow.MediaInfo;
                if (info != null) path = info.path as string; 
                if (path == null) continue;
                                               // Make reference path relative
                StringBuilder sOut = new StringBuilder(Const.MAX_PATH); 
                string dbPath = null;

                bool result = Utl.PathRelativePathTo (sOut, 
                    MaxMain.ProjectFolder, Const.FILE_ATTRIBUTE_DIRECTORY,
                    path, Const.FILE_ATTRIBUTE_NORMAL);

                if  (result)                               
                     dbPath = sOut.ToString();
                else         
                if  (Path.IsPathRooted(path))  // Ensure path is absolute
                     dbPath = path;
                else continue;                 // Skip problematic reference
          
                writer.WriteStartElement(Const.xmlEltFile); // <File>

                if  (result)
                     writer.WriteAttributeString(Const.xmlAttrRelPath, dbPath);
                else writer.WriteAttributeString(Const.xmlAttrPath, dbPath);

                writer.WriteAttributeString(Const.xmlAttrSubType, Const.xmlValFileSubtypeDbase);
                writer.WriteEndElement(); // </File>
            }
        }


        /// <summary>Check for already-added database scripts of the same name</summary>
        /// <remarks>Duplicates are not allowed because the file name {w/o ext) 
        /// is used to create the database name.</remarks>
        public bool DuplicateFileHandled(string path, string filename)
        {
            bool foundDuplicate = false;
            MaxExplorerWindow.MediaInfo info = null;

            MaxFolderTreeNode dbroot = main.Explorer.DbaseFolderNode;
            if (dbroot == null) return false;

            foreach(object x in dbroot.Nodes)
            {        
                string dbpath = null;
                MaxDbScriptTreeNode node = x as MaxDbScriptTreeNode;
                info = node == null? null:
                       node.Tag as MaxExplorerWindow.MediaInfo;
                if (info != null) dbpath = info.path as string; 
                if (dbpath == null) continue;

                if (String.Compare(Path.GetFileName(dbpath), filename, true) == 0)
                {
                    foundDuplicate = true;
                    break;
                }
            }

            if (foundDuplicate)
            {
                // Here we overwrite the old dbscript path;  we 'handled' this scenario. 
                if (Utl.ShowOverwriteDatabaseScriptRef(filename)) info.path = path;
                    return true;
            }

            return false;
        }


        /// <summary>Write a reference to each project media file or voice rec file</summary>
        public void SerializeVoiceRecFiles(XmlTextWriter writer, FileInfo[] files)
        {
            MaxFolderTreeNode mediaroot = main.Explorer.VrResxFolderNode;

            if (mediaroot == null) return;

            foreach(object x in mediaroot.Nodes)
            {        
                string path = null;
                MaxMediaTreeNode node = x as MaxMediaTreeNode;
                MaxExplorerWindow.MediaInfo info = node == null? null:
                    node.Tag as MaxExplorerWindow.MediaInfo;
                if (info != null) path = info.path as string; 
                if (path == null) continue;
                                                   // Make reference path relative
                StringBuilder sOut = new StringBuilder(Const.MAX_PATH); 
                string mediaPath = null;

                bool result = Utl.PathRelativePathTo (sOut, 
                     MaxMain.ProjectFolder, Const.FILE_ATTRIBUTE_DIRECTORY,
                     path, Const.FILE_ATTRIBUTE_NORMAL);

                if  (result)                               
                     mediaPath = sOut.ToString();
                else         
                if  (Path.IsPathRooted(path))      // Make sure that path is absolute
                     mediaPath = path;
                else continue;                     // Skip problematic reference
          
                writer.WriteStartElement(Const.xmlEltFile); // <File>

                if  (result)
                     writer.WriteAttributeString(Const.xmlAttrRelPath, mediaPath);
                else writer.WriteAttributeString(Const.xmlAttrPath, mediaPath);

                writer.WriteAttributeString(Const.xmlAttrSubType, Const.xmlValFileSubtypeVrResx);

                writer.WriteEndElement(); // </File>
            }
        }

        /// <summary>Write a reference to each project media file or voice rec file</summary>
        public void SerializeMediaFiles(XmlTextWriter writer, FileInfo[] files)
        {
            MaxFolderTreeNode mediaroot = main.Explorer.MediaFolderNode;
            if (mediaroot == null) return;

            foreach (MaxLocaleAudioFolder localeFolder in mediaroot.Nodes)
            {
                foreach (object x in localeFolder.Nodes)
                {
                    string path = null;
                    MaxMediaTreeNode node = x as MaxMediaTreeNode;
                    MaxExplorerWindow.MediaInfo info = node == null ? null :
                        node.Tag as MaxExplorerWindow.MediaInfo;
                    if (info != null) path = info.path as string;
                    if (path == null) continue;
                    // Make reference path relative
                    StringBuilder sOut = new StringBuilder(Const.MAX_PATH);
                    string mediaPath = null;

                    bool result = Utl.PathRelativePathTo(sOut,
                         MaxMain.ProjectFolder, Const.FILE_ATTRIBUTE_DIRECTORY,
                         path, Const.FILE_ATTRIBUTE_NORMAL);

                    if (result)
                        mediaPath = sOut.ToString();
                    else
                        if (Path.IsPathRooted(path))      // Make sure that path is absolute
                            mediaPath = path;
                        else continue;                     // Skip problematic reference

                    writer.WriteStartElement(Const.xmlEltFile); // <File>

                    if (result)
                        writer.WriteAttributeString(Const.xmlAttrRelPath, mediaPath);
                    else writer.WriteAttributeString(Const.xmlAttrPath, mediaPath);

                    writer.WriteAttributeString(Const.xmlAttrSubType, Const.xmlValFileSubtypeMedia);
                    writer.WriteAttributeString(Const.xmlAttrLocale, localeFolder.Text);

                    writer.WriteEndElement(); // </File>
                }
            }
        }


        /// <summary>Write a reference to each project reference</summary>
        public void SerializeReferences(XmlTextWriter writer)
        {
            MaxFolderTreeNode refsroot = main.Explorer.ReferencesFolderNode;
            if (refsroot == null) return;

            foreach(object x in refsroot.Nodes)
            {        
                string path = null;
                MaxReferenceTreeNode node = x as MaxReferenceTreeNode;
                MaxExplorerWindow.MediaInfo info = node == null? null:
                    node.Tag as MaxExplorerWindow.MediaInfo;
                if (info != null) path = info.path as string; 
                if (path == null) continue;
                                                   // Make reference path relative
                StringBuilder sOut = new StringBuilder(Const.MAX_PATH); 
                string referencePath = null;

                bool result = Utl.PathRelativePathTo (sOut, 
                    MaxMain.ProjectFolder, Const.FILE_ATTRIBUTE_DIRECTORY,
                    path, Const.FILE_ATTRIBUTE_NORMAL);

                if  (result)                               
                     referencePath = sOut.ToString();
                else         
                if  (Path.IsPathRooted(path))      // Make sure that path is absolute
                     referencePath = path;
                else continue;                     // Skip problematic reference
          
                writer.WriteStartElement(Const.xmlEltFile); // <File>

                if  (result)
                     writer.WriteAttributeString(Const.xmlAttrRelPath, referencePath);
                else writer.WriteAttributeString(Const.xmlAttrPath, referencePath);

                writer.WriteAttributeString(Const.xmlAttrSubType, Const.xmlValFileSubtypeRef);
                writer.WriteAttributeString(Const.xmlAttrRefType, Utl.SerializeAssemblyType(info.refType));
                writer.WriteEndElement(); // </File>
            }
        }


        /// <summary>Create project folder and/or delete existing project file</summary>
        public bool ValidateProjectDirectory(string name, string dir, string dlgtitle)
        {
            string fullname = name + Const.maxProjectFileExtension;
            string fullpath = dir  + Const.bslash + fullname;
            bool   result = false;
            try
            {   DirectoryInfo dirInfo = new DirectoryInfo(dir);
                bool folderAlreadyExists = dirInfo.Exists;

                if (folderAlreadyExists)
                {
                    if (this.IsViableMaxProject(dir))                     
                        if  (DialogResult.Yes != Utl.ShowAlreadyExistsDialog
                                 (Const.ProjectFolderText + dir, dlgtitle)) 
                             return false;

                    this.BackupProjectFolder(dir, Const.tilde + name, true);
                }

                dirInfo.Create();                            

                DirectoryInfo objDir = new DirectoryInfo(dir + Const.projectObjFolderName); 
                if (!objDir.Exists) objDir.Create();          
 
                result = true;
            }
            catch { }

            return result;
        }


        /// <summary>Determine if directory contains a viable max project</summary>
        /// <remarks>A project must contain one or more scripts and one project file</remarks>
        public bool IsViableMaxProject(string dir)
        {
            if (dir == null)  return false;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists) return false;
            int maxcount = 0, scriptcount = 0;

            FileInfo[] filesInfo = dirInfo.GetFiles();

            foreach(FileInfo fileInfo in filesInfo)
            {
                if (Utl.IsAppScriptFile(fileInfo.Name))  scriptcount++;
                else
                if (Utl.IsMaxProjectFile(fileInfo.Name)) maxcount++;
            }

            return (maxcount != 0 && scriptcount != 0); 
        }


        /// <summary>Create a backup copy of the specified project folder</summary>
        public bool BackupProjectFolder(string dir, string newname, bool replace)
        {
            if (dir == null) return false;
            string newpath = Directory.GetParent(dir) + Const.bslash + newname;

            DirectoryInfo dirInfo = new DirectoryInfo(newpath);
            if (dirInfo.Exists && !replace) return false;

            return Utl.SafeDirectoryMove(dir, newpath, true);
        }


        /// <summary>Validate selected script file as OK to add to project</summary>    
        public bool ValidateExistingScriptFile(string appfilePath)      
        {
            if (appfilePath == null) return false;      

            // Determine if valid app script file, extracting app and trigger names
            string appTrigger  = MaxMainUtil.PeekAppFile(appfilePath);
            string appRealName = MaxMainUtil.peekedAppName;

            if (appTrigger == null || appRealName == null)
            {
                Utl.ShowBadScriptFileDlg(appfilePath);
                return false;
            }

            // Check if app, whose name is embedded in the file, is already in project
            if (this.IsAppInProject(appRealName))
            {
                Utl.ShowExistingNameDlg(appRealName);
                return false;
            }

            string dlgtitle = Const.AppAddExistDlgTitle;
            bool   isInProjectFolder = Utl.IsSameDirectory(MaxMain.ProjectPath, appfilePath);

            string newfilePath = isInProjectFolder? appfilePath:          
                Utl.MakeSameDirectoryAs(MaxMain.ProjectPath, appfilePath);

            // If for some reason it's not a *.app extension, ensure we end up with one 
            newfilePath = Path.ChangeExtension(newfilePath, Const.maxScriptFileExtension);
      
            if (!isInProjectFolder) // If not in project directory, copy it there       
            {    
                if (File.Exists(newfilePath))
                    return Utl.ShowNameExistsDlg(newfilePath, null, dlgtitle);           
             
                try   { File.Copy(appfilePath, newfilePath); }
                catch { return Utl.ShowCannotCopyDlg(appfilePath, newfilePath, dlgtitle);}
            }
        
            MaxMain.prompted.AppName = newfilePath;   
            MaxMain.prompted.Trigger = appTrigger;
            return true;
        }


        /// <summary>Validate selected installer file as OK to add to project</summary>    
        public bool ValidateExistingInstallerFile(string filePath)      
        {
            string newpath = ValidateTextFile(filePath, Const.maxInstallerFileExtension, 
                Const.InstalAddExistDlgTitle, MaxProject.ProjectName);

            if (newpath == null) return false;
        
            MaxMain.prompted.InstallerPath = newpath;   
            return true;
        }


        /// <summary>Validate selected locales file as OK to add to project</summary>    
        public bool ValidateExistingLocalesFile(string filePath)      
        {
            string newpath = ValidateTextFile(filePath, Const.maxLocalesFileExtension, 
                Const.LocalesAddExistDlgTitle, MaxProject.ProjectName);

            if (newpath == null) return false;
        
            MaxMain.prompted.LocalesPath = newpath;   
            return true;
        }


        /// <summary>Validate selected database file as OK to add to project</summary>    
        public bool ValidateExistingDatabaseFile(string filePath)      
        {
            string newpath = ValidateTextFile
                (filePath, Const.maxDatabaseFileExtension, Const.DbaseAddExistDlgTitle, null);

            if (newpath == null) return false;
        
            MaxMain.prompted.DatabasePath = newpath;   
            return true;
        }


        /// <summary>Validate text file as OK to add to project</summary>    
        public string ValidateTextFile
        ( string filePath, string extension, string dlgtitle, string newname)      
        {
            return ValidateNonspecificFile(filePath, extension, dlgtitle, newname);
        }


        /// <summary>Validate media file as OK to add to project</summary>    
        public string ValidateMediaFile(string filepath, string dlgtitle, string newname)      
        {
            return ValidateNonspecificFile(filepath, null, dlgtitle, newname);
        }


        /// <summary>Validate general file as OK to add to project</summary>    
        public string ValidateNonspecificFile
        ( string filePath, string extension, string dlgtitle, string newname)      
        {
            if (filePath == null) return null;
            string newfilePath = Utl.MakeSameDirectoryAs(MaxMain.ProjectPath, filePath);

            if  (extension != null)  // If not the expected extension, make it so 
                 newfilePath = Path.ChangeExtension(newfilePath, extension);

            if  (newname != null) 
                 newfilePath = Utl.ChangePathFileName(newfilePath, newname);

            bool newfileIsOldfile = 0 == String.Compare(filePath, newfilePath, true);

            if (!newfileIsOldfile)   // If not in project directory, copy it there       
            {   
                if (File.Exists(newfilePath))         
                    if  (DialogResult.Yes == Utl.ShowAlreadyExistsDialog(newfilePath, dlgtitle)) 
                         Utl.SafeDelete(newfilePath);
                    else return null;
                                  
                try   { File.Copy(filePath, newfilePath); }
                catch { Utl.ShowCannotCopyDlg(filePath, newfilePath, dlgtitle); return null; }
            }

            return newfilePath;
        }


        /// <summary>Makes a best effort to return the temporary directory.</summary>
        /// <returns>Path to temporary directory</returns>
        public static string GetTempDir()
        {
            try
            {
                return Environment.GetEnvironmentVariable("TEMP");
            }
            catch
            {
                return Environment.CurrentDirectory;
            }
        }


        /// <summary>Check if project file is writable</summary>
        protected bool IsWritable(string path)
        {
            if (path == null)       return false; // No project open
            if (!File.Exists(path)) return true;  // New project 
            bool result = false;
            try
            {
                FileAttributes fa = File.GetAttributes(MaxMain.ProjectPath);     
                if ((fa & FileAttributes.ReadOnly) != FileAttributes.ReadOnly) 
                    result = true;
            }
            catch { }
            return result;
        }


        /// <summary>Determine if name is that of an app currently in project</summary>
        public bool IsAppInProject(string nameOrPath)
        {      
            string name  = Path.GetFileNameWithoutExtension(nameOrPath); 
            return null != main.Explorer.FindByAppName(name);
        }


        /// <summary>Extract list of triggers from tbx file -- deprecated 1009</summary>
        public string[] GetTriggersList()
        {
            string tbxfile = Utl.GetTbxFilePath(MaxMain.ProjectPath);
            if (tbxfile == null) return null;
            ArrayList triggers = new ArrayList();

            XmlTextReader reader = null;
            try             
            {
                    reader = new XmlTextReader(tbxfile);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while(reader.Read())
                {                                   // <Triggers>
                    if (!reader.IsStartElement(Const.xmlEltTriggers)) continue;

                    while(reader.Read())
                    {                                 // <Trigger name="x.y.z" />
                        if (!reader.IsStartElement (Const.xmlEltTrigger)) break;
                        if (!reader.MoveToAttribute(Const.xmlAttrName))   break;

                        reader.ReadAttributeValue();
                        if (reader.Value != null) triggers.Add(reader.Value); 
                    } 
                    break;        
                }
            }
            catch { }

            if (reader != null) reader.Close();
            if (triggers.Count == 0) return null;

            string[] triggersList = new string[triggers.Count];
            triggers.ToArray().CopyTo(triggersList,0);    

            return triggersList;
        }


        /// <summary>Given a fully qualified package and action name, return just the action name</summary>
        public static string GetActionName(string fullyQualifiedActionName)
        {
            return fullyQualifiedActionName.Substring(fullyQualifiedActionName.LastIndexOf(Const.dot) + 1);
        }

        /// <summary>Given a fully qualifed package and action name, return just the package name</summary>
        public static string GetPackageName(string fullyQualifiedActionName)
        {
            return fullyQualifiedActionName.Substring(0, fullyQualifiedActionName.LastIndexOf(Const.dot));
        }

        /// <summary>Peek alleged app file, extracting trigger, app name, singleton</summary>
        public static string PeekAppFile(string appfilePath)
        {
            XmlTextReader reader  = null;
            peekedAppName    = null;
            peekedAppTrigger = null;
            peekedAppSingle  = false;

            try 
            {   reader = new XmlTextReader(appfilePath);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while(reader.Read())
                {  // <Application trigger="x.y.z" name="xxx" single="false">
                    if (!reader.IsStartElement (Const.xmlEltApplication)) continue;                                            
                    peekedAppTrigger = Utl.XmlReadAttr(reader, Const.xmlAttrTrigger);     
                    peekedAppName    = Utl.XmlReadAttr(reader, Const.xmlAttrName);;
                    string boolval   = Utl.XmlReadAttr(reader, Const.xmlAttrSingleton);
                    peekedAppSingle  = XmlConvert.ToBoolean(boolval);              
                    break;
                }      
            } 
            catch {}
        
            if (reader != null) reader.Close();
            return peekedAppTrigger;
        }


        /// <summary>Peek alleged app file, returning app name</summary>
        public static string PeekAppFileName(string appfilePath)
        {
            PeekAppFile(appfilePath);
            return peekedAppName;
        }

        /// <summary> Use when the filetype is type 'ref', and you only want a certain AssemblyType </summary>
        public static string[] PeekProjectFileFiles(string projectpath, string filetype, AssemblyType type)
        {
            ArrayList files = new ArrayList();
            XmlTextReader rdr = null;
            int state = 0; 
            string filesubtype = null;

            try             
            {
                rdr = new XmlTextReader(projectpath);

                while(rdr.Read())
                {                                   
                    switch(state)
                    {
                       case 0: if (rdr.Name == Const.xmlEltFiles)   state++;  break;
                       case 1: if (rdr.Name == Const.xmlEltInclude) state++;  break;
                       case 2: 
                            if  (rdr.Name != Const.xmlEltFile)                break;
                            if (!rdr.MoveToAttribute(Const.xmlAttrSubType))   break;
                            if  (rdr.Value == null || rdr.Value != filetype)  break;
                            else filesubtype = rdr.Value;
                            if  (filesubtype == Const.xmlValFileSubtypeRef &&
                                !rdr.MoveToAttribute(Const.xmlAttrRefType))   break;
                            else 
                            if  ((Utl.DeserializeAssemblyType(rdr.Value) & type) == 0) break;
                            if  (rdr.MoveToAttribute(Const.xmlAttrRelPath) ||
                                 rdr.MoveToAttribute(Const.xmlAttrPath)) 
                                 files.Add(rdr.Value);                
                            break;
                    }               
                }
            }
            catch(Exception x) { MaxMain.MessageWriter.WriteLine(x.Message); }

            if (rdr != null) rdr.Close();
            string[] filelist = new string[files.Count];
            files.ToArray().CopyTo(filelist,0);    
            return filelist;
        }


        /// <summary> Use when the filetype is type 'ref' and subtype 'media',  </summary>
        public static string[] PeekProjectMediaFiles(string projectpath, out CultureInfo[] cultures)
        {
            ArrayList files = new ArrayList();
            ArrayList culturesList = new ArrayList();
            cultures = null;

            XmlTextReader rdr = null;
            int state = 0;
            string filetype = Const.xmlValFileSubtypeMedia;
            string filesubtype = null;

            try             
            {
                rdr = new XmlTextReader(projectpath);

                while(rdr.Read())
                {
                    string path = null;
                    CultureInfo info = null;
                    switch(state)
                    {
                       case 0: if (rdr.Name == Const.xmlEltFiles)   state++;  break;
                       case 1: if (rdr.Name == Const.xmlEltInclude) state++;  break;
                       case 2: 
                            if  (rdr.Name != Const.xmlEltFile)                break;
                            if (!rdr.MoveToAttribute(Const.xmlAttrSubType))   break;
                            if  (rdr.Value == null || rdr.Value != filetype)  break;
                            else filesubtype = rdr.Value;
                            if  (filesubtype == Const.xmlValFileSubtypeRef &&
                                !rdr.MoveToAttribute(Const.xmlAttrRefType))   break;
                           
                            if  (rdr.MoveToAttribute(Const.xmlAttrRelPath) ||
                                 rdr.MoveToAttribute(Const.xmlAttrPath)) 
                                 path = rdr.Value; 
                            if  (rdr.MoveToAttribute(Const.xmlAttrLocale))
                                 try { info = new CultureInfo(rdr.Value); } catch { }

                             if (path != null)
                             {
                                 // if no locale is specified in the XML, we assume en_us
                                 if (info == null)
                                 {
                                     info = Const.DefaultLocale;
                                 }

                                 files.Add(path);
                                 culturesList.Add(info);
                             }
                                 
                            break;
                    }               
                }
            }
            catch(Exception x) { MaxMain.MessageWriter.WriteLine(x.Message); }

            if (rdr != null) rdr.Close();
            string[] filelist = new string[files.Count];
            files.CopyTo(filelist, 0);
            cultures = new CultureInfo[culturesList.Count];
            culturesList.CopyTo(cultures, 0);    
            return filelist;
        }


        public static AssemblyType DetermineReferenceType(string projectPath, string referencePath)
        {
            AssemblyType type = AssemblyType.None;
            XmlTextReader rdr = null;
            int state = 0; 

            try             
            {
                rdr = new XmlTextReader(projectPath);

                while(rdr.Read())
                {
                    switch(state)
                    {
                        case 0: if (rdr.Name == Const.xmlEltFiles)   state++;    break;
                        case 1: if (rdr.Name == Const.xmlEltInclude) state++;    break;
                        case 2: 
                            if   (rdr.Name != Const.xmlEltFile)                  break;
                            if   (!rdr.MoveToAttribute(Const.xmlAttrSubType))    break;
                            if   (rdr.Value != Const.xmlValFileSubtypeRef)       break;
                            if   (!rdr.MoveToAttribute(Const.xmlAttrRelPath))    break;
                            if   (rdr.Value == null || rdr.Value == String.Empty)break;
                            if   (Path.GetFileName(referencePath) != Path.GetFileName(rdr.Value))                    break;
                            if   (!rdr.MoveToAttribute(Const.xmlAttrRefType))    break;
                            else type = Utl.DeserializeAssemblyType(rdr.Value);
                            break;
                    }               
                }
            }
            catch(Exception x) { MaxMain.MessageWriter.WriteLine(x.Message); }

            if (rdr != null) rdr.Close();
            return type;
        }


        /// <summary>Get list of files of specific type from project file</summary>
        public static string[] PeekProjectFileFiles(string projectpath, string filetype)
        {    
            return PeekProjectFileFiles(projectpath, filetype, 
                AssemblyType.NativeAction | AssemblyType.NativeType | 
                AssemblyType.Other | AssemblyType.Provider);
        }


        /// <summary>Extract project file packages list to an array</summary>
        public static string[] PeekProjectFilePackages(string projectfilepath)
        {
            if (projectfilepath == null) return new string[0];
            ArrayList projectPackages = new ArrayList();
            XmlTextReader reader = null;

            try                                    
            {   reader = new XmlTextReader(projectfilepath); 

                while(reader.Read())
                {                                   // <Packages>
                    if (!reader.IsStartElement(Const.xmlEltPackages)) continue;

                    while(reader.Read())
                    {                               // <Package name="x.y.z" />
                        if (!reader.IsStartElement (Const.xmlEltPackage)) break;
                        if (!reader.MoveToAttribute(Const.xmlAttrName))   break;
                        reader.ReadAttributeValue();
                        if (reader.Value != null) projectPackages.Add(reader.Value); 
                    } 
                    break;        
                }
            }
            catch { }

            if (reader != null) reader.Close();
            string[] packages = new string[projectPackages.Count];
            projectPackages.ToArray().CopyTo(packages,0); 
            return packages;
        }


        /// <summary>Extract docking configuration from project file to a temp file</summary>
        /// <remarks>XML was formatted by, and will be read by, the Crownwood framework</remarks>
        public string GetSavedDockingConfiguration()
        {
            if(MaxMain.ProjectPath == null) return null;
            XmlTextReader reader = null;
            XmlTextWriter writer = null;
            bool   isWrittenOK   = false;                                
            string idefilepath   = Utl.GetIdeFilePath(MaxMain.ProjectPath);     

            try             
            {   reader = new XmlTextReader(MaxMain.ProjectPath);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while(reader.Read())
                {                 
                    if (!reader.IsStartElement(Const.xmlEltDocking)) continue;
                    writer = new XmlTextWriter(idefilepath, Config.MaxEncoding);   
                    writer.WriteStartDocument();
                    writer.Formatting = Formatting.Indented;
                    writer.WriteNode(reader, false); 
                    isWrittenOK = true;
                    break;
                }
            }
            catch(Exception x) 
            { 
                MaxMain.OnStatusBarText(x.Message);
            }
            finally
            {        
                if (writer != null) writer.Close();
                if (reader != null) reader.Close();
            }

            return isWrittenOK? idefilepath: null;
        }


        /// <summary>Actions on response received to toolbox data request</summary>
        /// <remarks>If successful we configure the toolbox using said data</remarks>
        public void OnToolboxDataReady(MaxPackages packages)
        {
            // Here we'll want eventually to get the toolbox XML out of the project
            // file, and only if not available, will we use the default xml as below:

            // What we'll do *for now* is to have Max load the toolbox, and return
            // null packages here so that this method does nothing. As we implement
            // the Visual Studio part, we'll change this so that the toolbox data
            // is serialized from package data, along with the icon bitmaps; 
            // and written to a temp file, with the file path passed back to the  
            // framework (in a windows message), which then constructs the toolbox.

            MemoryStream stream = null;
            XmlNode root = null;

            try  
            {   string xml = (new MaxEmbeddedXmlResource(MaxXmlResource.DefaultToolboxXmlFileName)).Load(); 
                byte[] xmlBytes = System.Text.Encoding.ASCII.GetBytes(xml);
                stream = new System.IO.MemoryStream(xmlBytes);
      
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(stream);
                root = xdoc.DocumentElement;
            }
            catch { } 
            finally { if (stream != null) stream.Close(); }
      
            if  (root == null) return; 

            MaxToolboxHelper.Deserialize(root, packages, main.Toolbox);
        }


        /// <summary>Clears out the properties associated with the project</summary>
        public void ClearProjectSpecificProperties()
        {
            MaxProject.Instance.ClearProperties();
        }


        /// <summary>Unloads any project specific packages</summary>
        public void UnloadProjectSpecificPackages()
        {
            MaxNativeTypes.Instance.ClearProjectPackages();
            MaxPackages.Instance.ClearProjectPackages();
            MaxToolboxHelper.Instance.ClearProjectTools();
        }


        /// <summary>Remove project folder if it contains no app files</summary>
        /// <remarks>Ensures that project folders which contain no usable 
        /// project or resource files, are removed.</remarks>
        public void CleanupProjectFolder()
        {
            if (MaxMain.ProjectFolder == null) return;
            DirectoryInfo info  = new DirectoryInfo(MaxMain.ProjectFolder);
            if (!info.Exists) return;

            int maxcount=0, appcount=0,  tbxcount=0,  inscount=0,  lclcount=0; 
            int idecount=0, tmpcount=0,  othcount=0;
            int fdbcount=0, fbincount=0, fobjcount=0, fmedcount=0, fbakcount=0, fothcount=0;

            foreach(DirectoryInfo subdir in info.GetDirectories())
            {
                switch(Const.bslash + subdir.Name)
                {
                    case Const.projectDbFolderName:  fdbcount++;  break;
                    case Const.projectBinFolderName: fbincount++; break;
                    case Const.projectObjFolderName: fobjcount++; break;
                    case Const.projectMedFolderName: fmedcount++; break;
                    case Const.projectBakFolderName: fbakcount++; break;
                    default: fothcount++; break;
                }
            }

            foreach(FileInfo fi in info.GetFiles())                 
            {   
                switch(Path.GetExtension(fi.Name))
                {
                    case Const.maxProjectFileExtension:   maxcount++; break;
                    case Const.maxScriptFileExtension:    appcount++; break;
                    case Const.maxInstallerFileExtension: inscount++; break;
                    case Const.maxLocalesFileExtension:   lclcount++; break;
                    case Const.maxIdeFileExtension:       idecount++; break;
                    case Const.maxToolboxFileExtension:   tbxcount++; break;
                    case Const.tempfileExtension:         tmpcount++; break;
                    default: othcount++; break;               
                }
            }
                
            // Delete the project directory if there is nothing in it of use
            if ((appcount + inscount + lclcount + othcount + fothcount) == 0) 
                 info.Delete(true);       
        }


        /// <summary>Identify Content of satellite window under the mouse</summary>
        public Content SatelliteUnderXY(Point pt)
        {
            Rectangle mousePos = new Rectangle(pt, new Size(1,1));  
            Content   content  = null;                

            foreach(Content c in MaxMain.DockMgr.Contents)
            {
                WindowContent wc = c.ParentWindowContent;
                if  (wc == null) continue;          // Get content's window
                                            
                Rectangle rect = c.Docked?  
                    wc.RectangleToScreen(wc.DisplayRectangle):
                    new Rectangle(c.DisplayLocation, c.DisplaySize);

                if (rect.Contains(mousePos))        
                {
                    content = c;
                    break;
                }  
            }   
      
            return content == null? null:
                   content.Control is MaxSatelliteWindow? content: null;
        }
 
        public static MessageWriterCallback writer = new MessageWriterCallback(WriteMessage);
        public static MessageWriterCallback status = new MessageWriterCallback(StatusMessage);
        public static string peekedAppName, peekedAppTrigger;
        public static bool   peekedAppSingle;
    } // class MaxMainUtil
}   // namespace
