using System;  
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Xml;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;


namespace Metreos.Max.Manager
{
    /// <summary>MaxProject extension</summary>
    public class MaxProjectUtil
    {
        private MaxProject project;

        public MaxProjectUtil()
        {
            this.project = MaxProject.Instance;
        }


        /// <summary>Load project from supplied project file</summary>
        public bool Load(string projectPath)
        {
            project.SetPathInfo(projectPath);
            XmlTextReader reader = null;
            Exception exception  = null;
            bool result = false;
            MaxManager.Deserializing = true;
            Utl.WaitCursor(true);

            try 
            {
                reader = new XmlTextReader(projectPath);
                reader.MoveToContent(); 
          
                result = project.Load(reader);        
            } 
            catch (InvalidCastException) { /* GoDiagram throws on add link to doc */ }
            catch (Exception x) { exception = x; } 
            finally 
            { 
                if (reader != null) reader.Close(); 
                Utl.WaitCursor(false);
                MaxManager.Deserializing = false;
            }

            if  (result)
                this.ActivateCurrentView();
            else
            {
                if  (exception != null)
                     MessageBox.Show(exception.Message, projectPath);  
                else Utl.ShowBadProjectFileDlg(projectPath);

                project.MarkProjectClosed();          
            }
     
            return result;
        }


        /// <summary>Load script from supplied script file</summary>
        public bool LoadScript(MaxApp app, string scriptfilePath)
        {
            XmlTextReader reader = null;
            Exception exception  = null;
            bool result = false;
            MaxManager.Deserializing = true;
            Utl.WaitCursor(true);

            try 
            {
                reader = new XmlTextReader(scriptfilePath);
                reader.MoveToContent();           
                result = app.Load(reader);       
            } 
            catch (InvalidCastException) { /* GoDiagram throws on add link to doc */ }
            catch (Exception x) { exception = x; } 
            finally 
            { 
                if (reader != null) reader.Close(); 
                Utl.WaitCursor(false);
                MaxManager.Deserializing = false;
            }

            if  (result) 
                this.ActivateCurrentView();
            else     
            if  (exception != null) 
                 MessageBox.Show(exception.Message, scriptfilePath); 
            else Utl.ShowBadScriptFileDlg(scriptfilePath);
    
            return result;
        }


        /// <summary>Post-open activation for all tab content types</summary>
        public void ActivateCurrentView()
        {
            // We have auto-arrange turned off during load, so we need to do the
            // initial arrange as soon as we have turned serializing back on.
            // MaxCanvas canvas = MaxManager.Instance.CurrentTabContent() as MaxCanvas;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Project files I/O
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Save graph layouts to app source file</summary>
        public bool SaveAppFile(string path)
        {         
            MaxApp app = MaxProject.CurrentApp; if (app == null) return false;
            string appfilepath = path == null? app.AppfilePath: path;
            string tempfilepath = Path.ChangeExtension(appfilepath, Const.tempfileExtension);
            FileStream stream = null;
            bool result = false;

            try 
            {
                stream = File.Open(tempfilepath, FileMode.Create);
                this.WriteAppFile(stream);
                result = true;
            } 
            catch (Exception x) { MessageBox.Show(x.Message, appfilepath); } 
            finally { if (stream != null) stream.Close(); }   

            if (result)
                result = Utl.SafeCopy(tempfilepath, appfilepath, true, false);

            Utl.SafeDelete(tempfilepath);   

            return result;
        }


        /// <summary>Save app graph layouts to stream</summary>
        private void WriteAppFile(FileStream stream)
        {
            XmlTextWriter writer = new XmlTextWriter(stream, Config.MaxEncoding);
            writer.Formatting = Formatting.Indented;

            project.Serializer.SerializeScript(writer);

            writer.Close();
        }


        /// <summary>Save IDE layouts to IDE source file</summary>
        public bool SaveIdeLayout(bool force)
        {         
            string tbxFilePath = Utl.GetTbxFilePath(MaxProject.ProjectPath);
            if (tbxFilePath == null) return false;

            if (!force && File.Exists(tbxFilePath)) return true;

            FileStream stream = null;
            bool result = false;

            try 
            {
                stream = File.Open(tbxFilePath, FileMode.Create);
                this.WriteIdeLayout(stream);
                result = true;
            } 
            catch (Exception x) { MessageBox.Show(x.Message, tbxFilePath); } 
            finally { if (stream != null) stream.Close(); }

            return result;
        }


        /// <summary>Save IDE layouts to stream</summary>
        private void WriteIdeLayout(FileStream stream)
        {
            XmlTextWriter writer = new XmlTextWriter(stream, Config.MaxEncoding);
            writer.Formatting = Formatting.Indented;

            project.Serializer.SerializeIDE(writer);

            writer.Close();
        }


        /// <summary>Peek at an app file to extract the app trigger name</summary>
        public string PeekAppFileTrigger(string appfilePath)
        {
            XmlTextReader reader = null;
            string trigger = null;

            try 
            {
                reader = new XmlTextReader(appfilePath);

                while(reader.Read())
                {                 
                    if (!reader.IsStartElement (Const.xmlEltApplication)) continue;
                    trigger = Utl.XmlReadAttr(reader, Const.xmlAttrTrigger);                  
                    break;
                }      
            } 
            catch {}
        
            if (reader != null) reader.Close();
            return trigger;
        }


        /// <summary>Temporary project backup thing</summary>
        public void Backup(int maxbackups)
        {
            try 
            {
                DirectoryInfo projDirInfo = new DirectoryInfo(MaxProject.ProjectFolder);
                FileInfo[] projfiles = projDirInfo.GetFiles();

                string bakfolderpath = Utl.GetBakDirectoryPath(MaxProject.ProjectPath);
                DirectoryInfo   bakDirInfo  = new DirectoryInfo(bakfolderpath);
                DirectoryInfo[] bakDirsInfo = bakDirInfo.GetDirectories();
                string lowestx = null; string fullx = null; long lowest = 0; int count = 0;

                foreach(DirectoryInfo bakdir in bakDirsInfo)
                {
                    string dirname = bakdir.Name; 
                    long n = Utl.atol(dirname);
                    if (count++ == 0) lowest = n;
                   
                    if (n > 0 && n <= lowest) { lowest = n; lowestx = dirname; fullx = bakdir.FullName; }
                }

                if (fullx != null && count >= maxbackups) Directory.Delete(fullx, true);
       
                DirectoryInfo newDirInfo = bakDirInfo.CreateSubdirectory(System.DateTime.Now.Ticks.ToString());
                string newdir = newDirInfo.FullName;

                foreach(FileInfo fileinfo in projfiles)
                {
                    string bakpath = newdir + Const.bslash + Path.GetFileName(fileinfo.Name);
                    fileinfo.CopyTo(bakpath);
                }
            } 
            catch(Exception x) { MaxManager.Instance.Trace(x.Message); }
        }

    }  // class MaxProjectUtil
}    // namespace
