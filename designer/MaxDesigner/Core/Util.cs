using System;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.InteropServices;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.AppArchiveCore;
using Metreos.PackageGeneratorCore;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;


namespace Metreos.Max.Core
{
    /// <summary>Utility methods</summary>
    public sealed class Utl    
    {
        #region singleton
        private static readonly Utl instance = new Utl();
        public  static Utl Instance { get { return instance; }}
        private Utl() {} 
        #endregion

        /// <summary>Toggle wait cursor</summary>
        public static void WaitCursor(bool onoff)
        {
            Cursor.Current = onoff? Cursors.WaitCursor: Cursors.Default;
        }


        /// <summary>Write to output window (not intended for release use)</summary>
        public static void Trace(string msg)
        {
            Max.Framework.Satellite.Output.MaxOutputWindow.Trace(msg);
        }


        /// <summary>Write to output window (not intended for release use)</summary>
        public static void Out(string msg)
        {
            Max.Framework.Satellite.Output.MaxOutputWindow.Trace(msg);
        }


        /// <summary>Do a hardware beep</summary>
        public static void Beep()     
        {
            Console.WriteLine(Const.beep);  
        }


        /// <summary>Edit user input string for rudimetary validity</summary>
        public static bool ValidateRawStringInput(string s)
        {
            return s != null && s.Length > 0 && !s.StartsWith(Const.blank);
        }


        /// <summary>Edit wsdl service name for validity</summary>
        public static bool ValidateWsdlServiceName(string s)
        {
            // This is an educated guess as to what is a valid service name
            if (!ValidateRawStringInput(s))  return false;
            if (s.IndexOf(Const.blank) >= 0) return false;
            return s.IndexOfAny(Path.GetInvalidPathChars(), 0, s.Length) < 0;
        }


        /// <summary>Edit name string input, such as a project or script name</summary>
        public static bool ValidateMaxName(string s)
        {
            if (!ValidateRawStringInput(s)) return false;

            // Unsure if max can handle blanks in names, so disallow for now
            if (s.IndexOf(Const.blank) >= 0) return false;

            // Since project and script names get turned into identifiers in the app server,
            // these names can contain only letters, digits, and underscores. 
            if (!System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(s))            
                return false;

            return (!Utl.IsReservedWord(s));
        }   


        /// <summary>Edit path string input, such as a project file path</summary>
        public static bool ValidateMaxPath(string s)
        {
            if (!ValidateRawStringInput(s)) return false;
            if (s.IndexOfAny(Path.GetInvalidPathChars(), 0, s.Length) >= 0) return false;
            return true;
        }  


        /// <summary>Common link initialization</summary>
        public static IMaxLink MaxLinkConfig(IMaxLink link, IMaxNode node)
        {
            if (link != null && node != null)
            {
                link.Node = node;
                link.Canvas = node.Canvas;
            }
            return link;
        } 


        // <summary>Constructs an event handler name which does not exist in the document</summary>
        public static string MakeUniqueHandlerName(string eventName, MaxCanvas canvas)
        {
            string proposedName = MakeHandlerName(eventName);
            if (proposedName == null) return null;
            string originalName = proposedName;
            int i = 0;

            while(true)
            {
                if (canvas.CanNameNode(NodeTypes.Function, proposedName)) break;
                proposedName = originalName + ++i;
            }

            return proposedName;
        }


        /// <summary>Constructs an event handler name from a qualified event name</summary>
        public static string MakeHandlerName(string qualifiedEventName)
        {
            string name = StripQualifiers(qualifiedEventName);
            if (name != null)  
                name  = Const.on + name.Replace(Const.blank, Const.emptystr);
            return name;
        }


        /// <summary>Constructs function name which does not exist in the application</summary>
        public static string MakeUniqueFunctionName(MaxCanvas canvas)
        {
            string baseName = Const.defaultFunctionName;
            string proposedName = null;
            int i = 0;

            while(true)
            {
                proposedName = baseName + ++i;
                if (canvas.CanNameNode(NodeTypes.Function, proposedName)) break;        
            }

            return proposedName;
        }


        /// <summary>Construct arrary of link label choices</summary>
        public static ArrayList MakeLinkLabelChoices(string[] initialChoices)
        {
            string[] currentChoices = initialChoices == null? 
                Const.DefaultActionLinkLabelChoices: initialChoices;

            ArrayList choices = new ArrayList(currentChoices);

            if (Config.EnableUnconditionalLinks) 
                choices.Add(Const.LinkLabelUnconditional);
                
            return choices;
        }


        /// <summary>Strips the namespace part</summary>
        public static string StripQualifiers(string qualifiedName)
        {
            if  (qualifiedName == null) return null;
            string s = qualifiedName;
            int  lastDot  = s.LastIndexOf(Const.dot);
            if  (lastDot >= 0) s = s.Substring(lastDot+1); 
            return s;     
        }


        /// <summary>Strips the file part</summary>
        public static string StripPathFilespec(string path)
        {
            return Path.GetDirectoryName(path);
        }


        /// <summary>Strips the directory path part</summary>
        public static string StripPathFolder(string path)
        {
            return Path.GetFileName(path);
        }


        /// <summary>Strips the file extension part</summary>
        public static string StripPathExtension(string path)
        {
            return Path.GetDirectoryName(path) + Const.bslash 
                 + Path.GetFileNameWithoutExtension(path);
        }


        /// <summary>Strips the file extension part</summary>
        public static string StripPathFolderPlusExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }


        /// <summary>Strips the file extension part</summary>
        public static string StripFileExtension(string nameplusext)
        {
            if  (nameplusext == null) return null;
            string s = nameplusext;
            int  firstDot  = s.IndexOf(Const.dot);
            if  (firstDot >= 0) s = s.Substring(0, firstDot); 
            return s; 
        }


        /// <summary>Strips directory from path returning relative path</summary>
        public static string GetRelativePath(string dir, string path)
        {
            dir += Const.bslash;
            string relpath = null;
            int n = path.IndexOf(dir, 0, path.Length);
            if (n >= 0) relpath = path.Remove(n, dir.Length);
            return relpath;
        }


        /// <summary>Change the name part</summary>
        public static string ChangePathFileName(string path, string newname)
        {
            String ext = Path.GetExtension(path);
            String dir = Path.GetDirectoryName(path);
            return dir + Const.bslash + newname + ext;
        }


        /// <summary>Strips deepest directory, optionally retaining file part</summary>
        /// <remarks>We added some Max-specific logic to strip the directory only
        /// if the directory name is the same as the file name. This helps us
        /// handle Max-style project paths in recent file lists, while retaining
        /// the ability to open projects outside of a project directory.</remarks>
        public static string StripLastDirectory(string path, bool keepfilespec)
        {
            string filespec = Path.GetFileName(path); 
            string filename = StripFileExtension(filespec);
            string directory= Path.GetDirectoryName(path);
            int  lastSlash  = directory.LastIndexOf(Const.bslash);

            if  (lastSlash >= 0) 
            {    
                string dirname = directory.Substring(lastSlash+1, (directory.Length - (lastSlash+1)));

                if (dirname.ToLower() == filename.ToLower())
                    directory = directory.Substring(0, lastSlash);            
            }

            return keepfilespec? directory + Const.bslash + filespec: directory;
        }


        /// <summary>Returns deepest directory</summary>
        public static string GetLastDirectory(string dirpath)
        {
            int    lastSlash = dirpath.LastIndexOf(Const.bslash);
            return lastSlash < 0? null:           
                   dirpath.Substring(lastSlash+1, (dirpath.Length - (lastSlash+1)));
        }


        /// <summary>Swap path from path A into path B</summary>
        public static string MakeSameDirectoryAs(string pathA, string pathB)
        {
            string dirA  = Path.GetDirectoryName(pathA);
            string fileB = Path.GetFileName(pathB);
            return dirA + Const.bslash + fileB;
        }


        public static string ResolvePackageTypePath(string referencePath)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(referencePath);
            string directoryName = System.IO.Path.GetDirectoryName(referencePath);
            string packageName = fileName + Const.maxPackageExtension;
            return Path.Combine(directoryName, packageName);
        }


        /// <summary>Determines if two paths reference the same directory</summary>
        public static bool IsSameDirectory(string dirA, string dirB)
        {
            return Path.GetDirectoryName(dirB).ToLower().Equals(Path.GetDirectoryName(dirA).ToLower());
        }


        /// <summary>Gets the qualifiers part</summary>
        public static string GetQualifiers(string qualifiedName)
        {
            if (qualifiedName == null) return null;
            string s = qualifiedName;
            int lastDot  = s.LastIndexOf(Const.dot);
            if (lastDot >= 0) s = s.Substring(0, lastDot); 
            return s;     
        }


        /// <summary>Constructs a qualified name</summary>
        public static string GetQualifiedName(string namspace, string name)
        {
            return (name == null || namspace == null)? null:
                   (name.IndexOf(Const.dot) >= 0)? name:
                    namspace + Const.dot + name;
        }

    
        /// <summary>Convert bool-parseable string to bool, with default value specified<summary>
        public static bool ConvertStringToBool(string parseString, bool defaultValue)
        {
            bool result = defaultValue;

            if (parseString != null)      
                try   { result = bool.Parse(parseString); }
                catch { }

            return result;
        }


        /// <summary>Returns the first link in a GoPort</summary>
        public static GoLink FirstLink(GoPort port)
        {
            if (port == null || port.LinksCount == 0) return null;
            Northwoods.Go.GoPortLinkEnumerator i = port.Links.GetEnumerator();
            return i.MoveNext()? i.Current as GoLink: null;
        }


        /// <summary>Returns the first port in a max node</summary>
        public static IGoPort FirstPort(IMaxNode maxnode)
        {
            IGoNode node = maxnode as IGoNode;
            if (node == null) return null; 
            IEnumerator i = node.Ports.GetEnumerator();
            return i.MoveNext()? i.Current as GoPort: null;     
        }


        /// <summary>Return bounds of a graph node's icon</summary>
        public static RectangleF GetNodeIconBounds(IMaxNode node)
        {
            if (node == null) return Const.nullRect;
            MaxIconicMultiTextNode groupNode  = node as MaxIconicMultiTextNode;
            GoIconicNode           iconicNode = node as GoIconicNode;
            GoNode                 goNode     = node as GoNode;

            RectangleF rect 
                = (groupNode  != null)? groupNode.Pnode.Icon.Bounds: 
                  (iconicNode != null)? iconicNode.Icon.Bounds: 
                  (goNode     != null)? goNode.Bounds:
                   Const.nullRect;

            return rect;
        }


        /// <summary>First item in an IEnumerable</summary>
        public static object First(IEnumerable e)
        {
            IEnumerator i = e.GetEnumerator();
            return i.MoveNext()? i.Current: null; 
        }


        /// <summary>Return midpoint of rect</summary>
        public static PointF Midpoint(RectangleF rect)
        {
            return new PointF(rect.X + rect.Width/2, rect.Y + rect.Height/2);
        }


        /// <summary>Determine the media filename without the culture</summary>
        public static string PureMediaFileName(string rawMediaFilePath)
        {
            string pureName = null;

            if (rawMediaFilePath != null)
            {
                try
                {
                    if (File.Exists(rawMediaFilePath))
                    {
                        pureName = Path.GetFileNameWithoutExtension(rawMediaFilePath);
                        // attempt to parse file for embedded culture
                        int lastUnderscorePos = pureName.LastIndexOf('_');
                        if (lastUnderscorePos > 0 && lastUnderscorePos < pureName.Length) // Don't care about filenames starting in underscore after all
                        {
                            CultureInfo embeddedCulture = null;
                            try
                            {
                                string embeddedCultureName = pureName.Substring(lastUnderscorePos + 1);
                                embeddedCulture = new CultureInfo(embeddedCultureName);
                            }
                            catch { }

                            if (embeddedCulture != null)
                            {
                                pureName = pureName.Substring(0, lastUnderscorePos);
                            }
                        }
                        pureName += Path.GetExtension(rawMediaFilePath);
                    }
                }
                catch { }
            }

            return pureName;
        }

        /// <summary>Determines if in general a valid name for a samoa script</summary>
        public static bool IsValidAppName(string proposed)
        {
            return proposed != null && proposed.Length > 0 && proposed.IndexOf(Const.blank) < 0;
        }


        /// <summary>Determines if in general a valid name for a samoa function</summary>
        public static bool IsValidFunctionName(string proposed)
        {
            return proposed != null && proposed.Length > 0 && ValidateMaxName(proposed) && proposed.IndexOf(Const.blank) < 0;
        }


        /// <summary>Determines if in general a valid name for a samoa variable</summary>
        public static bool IsValidVariableName(string proposed)
        {
            return proposed != null && proposed.Length > 0 && proposed.IndexOf(Const.blank) < 0;
        }


        /// <summary>Determines if the proposed node name is a reserved keyword</summary>
        public static bool IsReservedWord(string proposed)
        {
            if (proposed == null) return false;
            if (0 <= Array.IndexOf(Const.csharpKeywords, proposed)) return true;
            if (0 <= Array.IndexOf(Const.areKeywords,    proposed)) return true;
            return false;
        }


        /// <summary>Gets a unique sequencer for a directory within specified directory</summary>
        public static int GetUniqueDirectorySequencer(string folder, string root, int start)
        {
            string rootpath = folder + Const.bslash + root;    
            int  i = start >= 0? start: 0;

            while(true) 
            {
                string path = rootpath + ++i;
                if (!Directory.Exists(path)) break;
            }

            return i;
        }


        /// <summary>Gets a unique sequencer for a file in specified directory</summary>
        public static int GetUniqueFilenameSequencer(string folder, string root, string ext, int start)
        {
            string rootpath = folder + Const.bslash + root;    
            int  i = start >= 0? start: 0;

            while(true) 
            {
                string path = rootpath + ++i + ext;
                if (!File.Exists(path)) break;
            }
             
            return i;
        }


        private static long priorTickCount = System.DateTime.Now.Ticks;

        /// <summary>Tick count is ~10ms resolution so get one which is unique</summary>
        public static long GetUniqueTickCount()
        {
            long newTickCount = System.DateTime.Now.Ticks;

            while(newTickCount == Utl.priorTickCount)
                  newTickCount = System.DateTime.Now.Ticks;

            Utl.priorTickCount = newTickCount;
            return newTickCount;
        }


        public static string GetTriggeringEventHandler(TreeNodeCollection treenodes)
        {
            if (treenodes == null) return null;

            foreach(TreeNode node in treenodes)
            {
                if (!(node is MaxAppTreeNodeEVxEH)) continue;
        
                MaxAppTreeNodeEVxEH eveh = node as MaxAppTreeNodeEVxEH;
        
                if (eveh.IsProjectTrigger)                 
                    return eveh.CanvasNodeFunction.NodeName;                 
            }

            return null;
        }


        /// <summary>Compare strings case insensitive</summary>
        public static bool CompareCaseInsensitive(string a, string b)
        {
            return a.ToLower().Equals(b.ToLower());
        }


        /// <summary>Return property having specified name</summary>
        public static MaxProperty GetProperty(PropertyDescriptorCollection properties, string propname)
        {
            if (properties != null)  
                foreach(MaxProperty property in properties)       
                    if (property != null && property.Name == propname) return property;
  
            return null;
        }


        /// <summary>Set property value to specified string value</summary>
        public static void SetProperty(PropertyDescriptorCollection properties, string propname, string propval)
        {
            if (properties != null)  
                foreach(MaxProperty property in properties)       
                    if (property != null && property.Name == propname) property.Value = propval; 
        }


        /// <summary>Validate version under which project file was written</summary>
        public static bool ValidateProjectFileVersion(string fileVersion)
        {
            float  projectFileVersion  = Convert.ToSingle(fileVersion);
            return projectFileVersion >= Const.LowestSupportedProjectFileVersionF;
        }


        /// <summary>Validate version under which app script file was written</summary>
        public static bool ValidateAppFileVersion(float appFileVersion)
        {
            return appFileVersion >= Const.LowestSupportedAppFileVersionF;
        }


        /// <summary>Extract xml attribute to string</summary>
        public static string XmlAttr(XmlNode node, string attrname)
        {
            XmlAttribute attr = node.Attributes[attrname];
            return attr == null? null: attr.Value;
        }


        /// <summary>Extract xml attribute to int</summary>
        public static int XmlAttrInt(XmlNode node, string attrname, int badvalue)
        {
            int retval = badvalue;
            XmlAttribute attr = node.Attributes[attrname];
            if (attr  != null) try { retval = XmlConvert.ToInt32(attr.Value); }  
                               catch { }
            return retval;
        }


        /// <summary>Extract xml attribute to bool</summary>
        public static bool XmlAttrBool(XmlNode node, string attrname, bool defaultvalue)
        {
            bool retval = defaultvalue;
            XmlAttribute attr = node.Attributes[attrname];
            if (attr != null) try { retval = XmlConvert.ToBoolean(attr.Value); }  
                              catch { }
            return retval;
        }


        /// <summary>Extract xml attribute to long</summary>
        public static long XmlAttrLong(XmlNode node, string attrname, long badvalue)
        {
            long retval = badvalue;
            XmlAttribute attr = node.Attributes[attrname];
            if (attr  != null) try { retval = XmlConvert.ToInt64(attr.Value); }  
                               catch { }
            return retval;
        }


        /// <summary>Extract xml attribute to float</summary>
        public static float XmlAttrFloat(XmlNode node, string attrname, float badvalue)
        {
            float retval = badvalue;
            XmlAttribute attr = node.Attributes[attrname];
            if (attr  != null) try { retval = XmlConvert.ToSingle(attr.Value); } 
                               catch { }
            return retval;
        }


        /// <summary>Read xml attribute to string</summary>
        public static string XmlReadAttr(XmlTextReader rdr, string attrName)
        {
            if (rdr.MoveToAttribute(attrName)) { rdr.ReadAttributeValue(); return rdr.Value; }
            return null;
        }


        /// <summary>Validate a user name</summary>
        public static bool EditUsername(string s)
        {
            if (s == null || s == String.Empty) return false;
            if (s.IndexOfAny(Const.invalidPasswordCharacters) != -1) return false;
            return !IsBogusCharacter(s);            
        }


        /// <summary>Get a short node ID for the node (for debugging display)/summary>
        public static string snid(IMaxNode node)
        {
            string s = node == null? String.Empty: node.NodeID.ToString();
            string s2 = s.Length < 5? s: s.Substring(s.Length - 5, 5);
            return s2;
        }


        /// <summary>Validate passwords</summary>
        public static bool EditPassword(string s)
        {
            if (s == null || s.Length < 3) return false;
            if (Utl.atoi(s) != 0) return false; // no all-numeric
            if (Utl.isZeros(s))   return false; // catch the zero case as well
            if (s.IndexOfAny(Const.invalidPasswordCharacters) != -1) return false;
            return !IsBogusCharacter(s);                     
        }


        /// <summary>Check string for non-keyboard characters</summary>
        public static bool IsBogusCharacter(string s)
        {
            if (s == null) return true;
            bool ok = false;
            
            for(int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                int  n = 0;
                try{ n = Convert.ToInt32(c); } 
                catch { }
                if (n > 0x7e || n < 0x20) ok = true;
            }

            return ok;
        }


        /// <summary>Determine if string evaluates to zero</summary>
        public static bool isZeros(string s)
        {
            int    n = 0; bool ok = false;
            try  { n = System.Convert.ToInt32(s); ok = true; } 
            catch{ }
            return ok && n == 0;
        }


        /// <summary>Convert alpha to int with behavior as c library atoi</summary>
        public static int atoi(string s)
        {
            int    n = 0;
            try  { n = System.Convert.ToInt32(s); } 
            catch{ }
            return n;
        }


        /// <summary>Convert alpha to long with behavior as c library atol</summary>
        public static long atol(string s)
        {
            long   n = 0;
            try  { n = System.Convert.ToInt64(s); } 
            catch{ }
            return n;
        }


        /// <summary>Convert float to int</summary>
        public static int ftoi(float f)
        {
            int    n = 0;
            try  { n = System.Convert.ToInt32(f); } 
            catch{ }
            return n;
        }


        /// <summary>One-way encryption for passwords</summary>
        public static string Md5Encode(string plainText)
        {
            string sresult = null;
            if (plainText != null && plainText.Length > 0) 
            { 
                System.Security.Cryptography.MD5 md5 = new
                System.Security.Cryptography.MD5CryptoServiceProvider();
                try  
                {
                    byte[] b = md5.ComputeHash(System.Text.Encoding.Unicode.GetBytes(plainText));
                    sresult  = System.Convert.ToBase64String(b);
                }
                catch { }
            }
            return sresult;     
        }


        /// <summary>Given project path return path to project toolbox file</summary>
        public static string GetTbxFilePath(string projectPath)
        {
            if (projectPath == null) return null;
            string s = GetObjDirectoryPath(projectPath);  
            s += (Const.bslash + Path.GetFileNameWithoutExtension(projectPath) + Const.maxToolboxFileExtension);
            return s;
        }


        /// <summary>Given project path return path to project .ide file</summary>
        public static string GetIdeFilePath(string projectPath)
        {
            if (projectPath == null) return null;
            string s = GetObjDirectoryPath(projectPath);  
            s += (Const.bslash + Path.GetFileNameWithoutExtension(projectPath) + Const.maxIdeFileExtension);
            return s;
        }


        /// <summary>Given project path return installer file directory path</summary>
        public static string GetInstallerFileFolder(string projectPath)
        {
            return projectPath == null? null: Path.GetDirectoryName(GetInstallerFilePath(projectPath));
        }


        /// <summary>Given project path return path to project installer file</summary>
        public static string GetInstallerFilePath(string projectPath)
        {
            return projectPath == null? null:
                Path.GetDirectoryName(projectPath) + Const.bslash 
                + Path.GetFileNameWithoutExtension(projectPath) 
                + Const.maxInstallerFileExtension;
        }


        /// <summary>Given project path return installer file directory path</summary>
        public static string GetLocalesFileFolder(string projectPath)
        {
            return projectPath == null ? null : Path.GetDirectoryName(GetLocalesFilePath(projectPath));
        }


        /// <summary>Given project path return path to project locales file</summary>
        public static string GetLocalesFilePath(string projectPath)
        {
            return projectPath == null? null:
                Path.GetDirectoryName(projectPath) + Const.bslash 
                + Path.GetFileNameWithoutExtension(projectPath) 
                + Const.maxLocalesFileExtension;
        }
        

        /// <summary>Given project path return path to database script file</summary>
        public static string GetDatabaseFilePath(string projectPath, string name)
        {
            if (projectPath == null) return null;
            if (name == null) name = Path.GetFileNameWithoutExtension(projectPath);
            string dbDir = GetDbDirectoryPath(projectPath);
            return dbDir + Const.bslash + name + Const.maxDatabaseFileExtension;
        }

		
        /// <summary>Given project path return path to project database (db) folder.
        /// Db folder is created if nonexsistent</summary>
        public static string GetDbDirectoryPath(string projectPath)
        {
            return GetSubdirectoryPath(projectPath, Const.projectDbFolderName);
        }


        /// <summary>Given project path return path to media file</summary>
        public static string GetMediaFilePath(string projectPath, string ext)
        {
            return projectPath == null? null:
                Path.GetDirectoryName(projectPath) + Const.bslash 
              + Path.GetFileNameWithoutExtension(projectPath) + ext; // TODO lose this 1223b

            // return GetMedDirectoryPath(projectPath) + Const.bslash + name + ext; // TODO replace w this
        }


        /// <summary>Given project path return path to project media (med) folder.
        /// Med folder is created if nonexsistent</summary>
        public static string GetMediaDirectoryPath(string projectPath)
        {
            return GetSubdirectoryPath(projectPath, Const.projectMedFolderName);
        }


        /// <summary>Given project path return path to project deployment file</summary>  
        public static string GetDeployFilePath(string projectPath)
        { 
            return GetBinDirectoryPath(projectPath);
        }


        /// <summary>Given project path return path to project build file</summary>  
        public static string GetBuildFilePath(string projectPath)
        {
            string filename = Path.GetFileNameWithoutExtension(projectPath);
            filename += Const.maxBuildFileExtension;
            return GetObjDirectoryPath(projectPath, filename);
        }


        /// <summary>Given project path return path to project build file</summary>  
        public static string GetBuildFilePath(string projectPath, string appname)
        {
            string filename = appname + Const.maxBuildFileExtension;
            return GetObjDirectoryPath(projectPath, filename);
        }


        /// <summary>Given project path and a file name, return path to project obj directory. 
        /// Obj folder is created if nonexsistent</summary>
        public static string GetObjDirectoryPath(string projectPath)
        {
            return GetSubdirectoryPath(projectPath, Const.projectObjFolderName);
        }


        /// <summary>Given project path and a file name, return path to file within
        /// project obj directory. Obj folder is created if nonexsistent</summary>
        public static string GetObjDirectoryPath(string projectPath, string filename)
        {
            string objFolderPath = GetSubdirectoryPath(projectPath, Const.projectObjFolderName);
            return Path.Combine(objFolderPath, filename);
        }


        /// <summary>Given project path and a file name, return path to file within
        /// project bin directory. Bin folder is created if nonexsistent</summary>
        public static string GetBinDirectoryPath(string projectPath)
        {
            return GetSubdirectoryPath(projectPath, Const.projectBinFolderName);
        }


        /// <summary>Given project path and a file name, return path to file within
        /// project bak directory. Bak folder is created if nonexsistent</summary>
        public static string GetBakDirectoryPath(string projectPath)
        {
            return GetSubdirectoryPath(projectPath, Const.projectBakFolderName);
        }


        /// <summary>Given project path, a subdirectory preceded by backslash, and a 
        /// file name, return path to file within that subdirectory. Subdirectory is 
        /// created if nonexsistent</summary>
        public static string GetSubdirectoryPath(string projectPath, string subdir)
        {
            if (projectPath == null) return null;
            string returnPath = null;

            // if (filename.IndexOf(Const.dot) == 0) filename += Const.tempfileExtension;
            string projectFolder = Path.GetDirectoryName(projectPath);
            string subpath = projectFolder + subdir;
            try   { Directory.CreateDirectory(subpath); returnPath = subpath; }
            catch {}

            return returnPath;
        }


        /// <summary>For each non-full path in array, make path full relative to project</summary> 
        public static string[] MakeAbsolute(string[] relAndFullPaths, string relToDir)
        {
            string[] absReferences = new string[relAndFullPaths.Length];

            for(int i = 0; i < relAndFullPaths.Length; i++)
                absReferences[i] = Path.IsPathRooted(relAndFullPaths[i])?
                     relAndFullPaths[i]:
                     PathRelativeToAbsolute(relAndFullPaths[i], relToDir);

            return absReferences;
        }


        /// <summary>Return absolute path given relative path and base directory</summary>
        public static string PathRelativeToAbsolute(string relpath, string relToDir)
        {
            if (relpath == null || relToDir == null) return null;
            string currentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(relToDir);
            string absolute = Path.GetFullPath(relpath);
            Directory.SetCurrentDirectory(currentDir);
            return absolute;
        }


        /// <summary> Designed with Flags enumerations in mind, with this we can see the 
        ///  string values of all the enumerations present in a Flags() marked enum
        /// </summary>
        public static string[] GetEnumValues(Type enumeration, Enum enumInstance)
        {
            string[] names = Enum.GetNames(enumeration);

            if (names == null || names.Length == 0) return null;

            ArrayList values = new ArrayList();
            foreach(string name in names)
            {
                try
                {
                    Enum enumType = (Enum)Enum.Parse(enumeration, name);
                    int enumValueCheckAgainst = (int)System.Convert.ChangeType(enumType, typeof(int));
                    int enumValue = (int)System.Convert.ChangeType(enumInstance, typeof(int));

                    if(!((enumValueCheckAgainst & enumValue) == 0))
                        values.Add(name);
                }
                catch { } 
            }

            if (values.Count > 0)
            {
                string[] allEnumValues = new string[values.Count];
                values.CopyTo(allEnumValues);
                return allEnumValues;
            }
            else return null;
        }


        /// <summary> Given the values in the AssemblyType enumeration 
        ///  returns an xml-formatted attribute value </summary>        
        public static string SerializeAssemblyType(AssemblyType type)
        {
            switch(type)
            {
                case AssemblyType.Other:
                    return Const.xmlValRefTypeOther;

                case AssemblyType.NativeAction:
                    return Const.xmlValRefTypeNativeAct;

                case AssemblyType.NativeType:
                    return Const.xmlValRefTypeNativeType;

                case AssemblyType.Provider:
                    return Const.xmlValRefTypeProvider; 
            }

            return Const.xmlValRefTypeOther;
        }


        /// <summary> From an xml-formatted string value, returns AssemblyType enum </summary>
        public static AssemblyType DeserializeAssemblyType(string type)
        {
            switch(type)
            {
                case Const.xmlValRefTypeOther:
                    return AssemblyType.Other;

                case Const.xmlValRefTypeNativeAct:
                    return AssemblyType.NativeAction;

                case Const.xmlValRefTypeNativeType:
                    return AssemblyType.NativeType;

                case Const.xmlValRefTypeProvider:
                    return AssemblyType.Provider;
            }

            return AssemblyType.Other;
        }


        public static bool ValidatePackage(string projectPath, string libraryPath, AssemblyPeeker peeker, 
        out packageType package, out nativeTypePackageType typePackage)
        {
            bool success = true;
            // Fully load the package. Do basic checking
            string[] references = MaxMainUtil.PeekProjectFileFiles(projectPath, Const.xmlValFileSubtypeRef);
            references = Utl.MakeAbsolute( references, new FileInfo(projectPath).DirectoryName );
      
            XmlGenerator typesLoader = new XmlGenerator(new FileInfo(libraryPath), references);
            typesLoader.Parse();
        
            package = null;
            typePackage = null;

            switch(peeker.Type)
            {
                case AssemblyType.NativeAction:
                case AssemblyType.Provider:
                    package = typesLoader.Package;
                    if (package.name == null || package.name == String.Empty) 
                        success = false;
                    break;

                case AssemblyType.NativeType:
                    typePackage = typesLoader.TypePackage;
                    if (typePackage.name == null || typePackage.name == String.Empty)                    
                        success = false;
                    break;
            }
           
            return success;
        }


        public static bool ValidateLocalizableStringName(string text)
        {
            if (text == null ||
                text == String.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary> Combines arrays, excluding duplicate entries</summary>
        /// <param name="arrays"> Arrays to combines</param>
        /// <returns> The same type of array</returns>
        public static string[] CombineArrays(params string[][] arrays)
        {
            if (arrays == null || arrays.Length == 0) return null;
            ArrayList combiner = new ArrayList();

            foreach(string[] array in arrays)
            {
                if (array == null) continue;

                foreach(string @value in array)
                    if(!combiner.Contains(@value))
                        combiner.Add(@value);
            }

            if (combiner.Count == 0) return null;

            string[] returnArray = new string[combiner.Count];
            combiner.CopyTo(returnArray);
            return returnArray;
        }


        /// <summary>Determine if path names a max project file</summary>
        public static bool IsMaxProjectFile(string path)
        {
            string ext  = path == null? null: Path.GetExtension(path);
            return ext != null && ext == Const.maxProjectFileExtension;
        }


        /// <summary>Determine if path names an application script</summary>
        public static bool IsAppScriptFile(string path)
        {
            string ext  = path == null? null: Path.GetExtension(path);
            return ext != null && ext == Const.maxScriptFileExtension;
        }


        /// <summary>Determine if path names a database script</summary>
        public static bool IsDatabaseScriptFile(string path)
        {
            string ext  = path == null? null: Path.GetExtension(path);
            return ext != null && ext == Const.maxDatabaseFileExtension;
        }


        /// <summary>Determine if path names a media file</summary>
        public static bool IsMediaFile(string path)
        {
            return IsAudioFile(path) || IsVideoFile(path);
        }


        /// <summary>Determine if path names an audio file</summary>
        public static bool IsAudioFile(string path)
        {
            string ext  = path == null? null: Path.GetExtension(path);
            return ext != null 
                &&(ext == Const.WavFileExtension || ext == Const.VoxFileExtension);
        }


        /// <summary>Determine if path names a video file</summary>
        public static bool IsVideoFile(string path)
        {
            return false;
        }


        /// <summary>Determine if path names a grammar file</summary>
        public static bool IsGrammarFile(string path)
        {
            string ext  = path == null? null: Path.GetExtension(path);
            return ext != null 
                &&(ext == Const.GrammarFileExtension);
        }


        /// <summary>Determine if path names a voice recognition resource file</summary>
        public static bool IsVoiceRecFile(string path)
        {
            string ext  = path == null? null: Path.GetExtension(path);
            return ext != null 
                &&(ext == Const.GrammarFileExtension || ext == Const.UserdictFileExtension);
        }


        /// <summary>Delete file without exceptions, returning success boolean</summary>
        public static bool SafeDelete(string path)
        {  
            bool result = false; 
            try
            { if (path != null && File.Exists(path))                                  
              {
                  File.Delete(path);
                  result = true;
              }
            }
            catch { }
            return result;
        }


        /// <summary>Delete directory without exception, returning success boolean</summary>
        public static bool SafeDirectoryDelete(string path)
        {
            bool result = false;
            try
            { if (path != null && Directory.Exists(path))
              {
                  Directory.Delete(path, true);
                  result = true;
              }
            }
            catch { }
            return result;
        }


        /// <summary>Copy directory without exception, returning success boolean</summary>
        public static bool SafeDirectoryMove(string sourcepath, string destpath, bool replace)
        {
            if (sourcepath == null || destpath == null) return false;
            bool result = false;

            try
            { if (Directory.Exists(sourcepath) && (replace || !Directory.Exists(destpath)))
              {
                  Directory.Move(sourcepath,destpath);
                  result = true;
              }
            }
            catch { }
            return result;
        }


        /// <summary>Copy or move without exceptions, returning success boolean</summary>
        public static bool SafeCopy(string sourcepath, string destpath, bool replace, bool move)
        {  
            bool result = false; 
            try
            { if (sourcepath != null && File.Exists(sourcepath))                                  
              {
                  File.Copy(sourcepath, destpath, replace);
                  if (move) File.Delete(sourcepath);
                  result = true;
              }
            }
            catch { }
            return result;
        }


        /// <summary>Accumulate or display missing packages</summary>
        public static void MissingPackagesProc(MissingPackageActions action)
        {
            MissingPackagesProc(action, null);
        }


        public static void MissingPackagesProc(MissingPackageActions action, string package)
        {
            switch(action)
            {
               case MissingPackageActions.Clear: MissingPackages.Clear(); break;
               case MissingPackageActions.Add:
                    if (!MissingPackages.Contains(package)) MissingPackages.Add(package);
                    break;

               case MissingPackageActions.Show:
                    if (MissingPackages.Count == 0) return;
                    string[] packages = new string[MissingPackages.Count];
                    MissingPackages.ToArray().CopyTo(packages,0);
                    new Framework.MaxPassiveListDlg(Const.FileNotFoundDlgTitle, 
                        Const.MissingPackagesBlurb,packages, null).ShowDialog();  
                    MissingPackages.Clear();                 
                    break;
            }
        }


        public  enum   MissingPackageActions { Clear, Add, Show }
        private static ArrayList MissingPackages = new ArrayList();
        private static ArrayList MissingPackageTypes = new ArrayList();


        public static void MissingPackagesTypeProc(MissingPackageActions type)
        {
            MissingPackagesTypeProc(type, null);
        }


        public static void MissingPackagesTypeProc(MissingPackageActions type, string package)
        {
            switch(type)
            {
                case MissingPackageActions.Clear: MissingPackageTypes.Clear(); break;
                case MissingPackageActions.Add:
                    if (!MissingPackageTypes.Contains(package)) MissingPackageTypes.Add(package);
                    break;

                case MissingPackageActions.Show:
                    if (MissingPackageTypes.Count == 0) return;
                    string[] packages = new string[MissingPackageTypes.Count];
                    MissingPackageTypes.CopyTo(packages);
                    new Framework.MaxPassiveListDlg(Const.PackageNotFoundDlgTitle, 
                        Const.MissingTypePackagesBlurb, packages, null).ShowDialog();
                    MissingPackageTypes.Clear();
                    break;
            }
        }


        /// <summary>Determine if a file is an Action/Event Package</summary>
        public static bool IsActionEventPackage(string packagePath)
        {
            FileStream stream = null;
            XmlTextReader reader = null;
            bool isActionEventPackage = false;

            try
            {   stream = new FileStream(packagePath, FileMode.Open, FileAccess.Read);
                reader = new XmlTextReader(stream);
                isActionEventPackage = aePackageSeri.CanDeserialize(reader);
            }
            catch { }
     
            if(reader != null) reader.Close();
            if(stream != null) stream.Close();
      
            return isActionEventPackage;
        }


        /// <summary>Determine if a file is a Native Type Package</summary>
        public static bool IsNativeTypePackage(string packagePath)
        {
            FileStream stream = null;
            XmlTextReader reader = null;
            bool isNativeTypePackage = false;
            try
            {   stream = new FileStream(packagePath, FileMode.Open, FileAccess.Read);
                reader = new XmlTextReader(stream);
                isNativeTypePackage = ntPackageSeri.CanDeserialize(reader);
            }
            catch { }     
            if(reader != null) reader.Close();
      
            return isNativeTypePackage;
        } 


        public static void WriteExceptionToFile(string errorsFilePath, Exception e)
        {
            StreamWriter writer = null;      
            try
            {   writer = new StreamWriter(errorsFilePath, false);
                writer.WriteLine(Utl.FormatException(e));
            }
            catch { }
            if(writer != null) writer.Close();     
        }


        public static void HandleGenericException(Exception e)
        {
            if (e is MissingFieldException  ||
                e is MissingMemberException ||
                e is MissingMethodException ||
                e is System.TypeLoadException) // MSC: are there any more of these??
            {
                // GAC is out of WHAC
                Utl.ShowFrameworkCorruptedDlg(e);
            }
            else
            {
                string errorFilePath = GetObjDirectoryPath(MaxMain.ProjectPath, Const.BuildErrorsFilename);
                Utl.ShowGenericErrorDlg(e); 
                MaxMain.MessageWriter.WriteLine(Const.MoreErrorInfo); // look in errors.dat for error info
                WriteExceptionToFile(errorFilePath, e); // write to errors.dat full error info
            }
        }


        private static string FormatException(Exception exception)
        {
            StringBuilder errorMessage = new StringBuilder();

            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append(Const.ErrorFileDeclaration);
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append(Const.ErrorSource); 
            errorMessage.Append(exception.Source);
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append(Const.ErrorMessage);
            errorMessage.Append(exception.Message);
      
            if(exception is PackagerException) // Custom error we use 
            {
                PackagerException pe = exception as PackagerException;
                if (pe.ErrorMessages != null)
                    foreach(string msg in pe.ErrorMessages)
                    {
                        errorMessage.Append(Const.PackagerMessage);
                        errorMessage.Append(msg);
                        errorMessage.Append(System.Environment.NewLine);
                    }
                else
                    errorMessage.Append(System.Environment.NewLine);
            }

            errorMessage.Append(Const.ErrorStackTrace);
            errorMessage.Append(exception.TargetSite == null ? String.Empty : exception.TargetSite.ToString());
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append(exception.StackTrace);
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append(Const.ErrorDateTime);
            errorMessage.Append(DateTime.Now.ToShortDateString());
            errorMessage.Append(Const.blank);
            errorMessage.Append(DateTime.Now.ToLongTimeString());

            if (exception.InnerException != null)
            {
                errorMessage.Append(System.Environment.NewLine);
                errorMessage.Append(FormatException(exception.InnerException));
            }
 
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append(System.Environment.NewLine);

            return errorMessage.ToString();
        }

        /// <summary>Show dialog prompting for variable delete confirmation</summary>
        public static bool ShowRemoveVariableDlg(string varname)
        {
            return DialogResult.OK == MessageBox.Show(Manager.MaxManager.Instance,
                Const.DeleteVariableMessage(varname), Const.variableDeleteDlgTitle,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
        }


        /// <summary>Show dialog informing that new name is not valid for node</summary>
        public static void ShowCannotRenameNodeDialog(bool isFunction)
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.CannotRenameMsg(isFunction), Const.NodeRenameDialogTitle, 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
        } 


        /// <summary>Show "xxx already exists. Do you want to replace it?</summary>
        public static DialogResult ShowAlreadyExistsDialog(string path, string title)
        {
            return MessageBox.Show(Manager.MaxManager.Instance, 
                Const.AlreadyExistsMessage(path), title, 
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation); 
        }   


        /// <summary>Show "xxx is already installed. Do you want to replace it?</summary>
        public static DialogResult ShowAlreadyInstalledDialog(string type, string path, string title)
        {
            return MessageBox.Show(Manager.MaxManager.Instance, 
                Const.AlreadyInstalledMessage(type, path), title, 
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation); 
        }   


        /// <summary>Show "Package x could not be installed. Ensure that ..."</summary>
        public static bool ShowPackageLoadErrorMsg(string name, string path)
        {
            MessageBox.Show(Manager.MaxManager.Instance, Const.PackageLoadErrMsg(name, path),
                Const.PackageInstallErrorDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }


        /// <summary>Show "Do you want to save changes to xxx?"</summary>
        public static DialogResult PromptSaveChangesTo(string name, string title)
        {
            return MessageBox.Show(Manager.MaxManager.Instance,
                Const.GetPromptSaveChangesTo(name), title,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }


        /// <summary>Show "No triggering events found in any package"</summary>
        public static bool ShowNoTriggersDlg()
        {
            MessageBox.Show(Manager.MaxManager.Instance, Const.NoTriggersMsg, 
                Const.NoTriggersDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }


        /// <summary>Show "xxx is not a valid Max Designer application script"</summary>
        public static bool ShowBadScriptFileDlg(string path)
        {
            MessageBox.Show(Manager.MaxManager.Instance, Const.BadScriptFileMsg(path), 
                Const.AppOpenDlgTitle);
            return false;
        }


        /// <summary>Show "Script xxx already exists in the project directory"</summary>
        public static bool ShowNameExistsDlg(string path, string filetype, string title)
        {
            string name = Path.GetFileName(path);
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.CannotNameAppMsgA(name,filetype), title, 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            return false;
        }


        /// <summary>Show "xxx is not a valid script name"</summary>
        public static bool ShowInvalidNameDlg(string name)
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.CannotNameAppMsgB(name), Const.NewAppDlgTitle, 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            return false;
        }


        /// <summary>Show "Script xxx already exists in project"</summary>
        public static bool ShowExistingNameDlg(string name)
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.CannotOpenAppMsgA(name), Const.OpenAppDlgTitle, 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            return false;
        }


        /// <summary>Show "Cannot copy aaa/bbb.ccc to xxx/yyy.zzz"</summary>
        public static bool ShowCannotCopyDlg(string source, string dest, string title)
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.CannotCopyMsgA(source, dest), title, 
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }


        /// <summary>Show "xxx is not a valid Max Designer project file"</summary>
        public static bool ShowBadProjectFileDlg(string projectPath)
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.BadProjectFileMsg(projectPath), Const.ProjectOpenDlgTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }


        /// <summary>Show "A reference to xxx already exists in the project"</summary>
        public static bool ShowReferenceExistsDlg(string path)
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.ReferenceExistsMsg(Path.GetFileNameWithoutExtension(path)), 
                Const.AddReferenceDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }


        /// <summary>Show "Assembly contains multiple types: ..."</summary>
        public static bool ShowMalformedAssemblyDlg(string[] typesFound)
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.MalformedAssemblyDesc(typesFound), 
                Const.MalformedAssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);    
            return false;
        }


        /// <summary> The framework has become corrupted </summary>
        public static bool ShowFrameworkCorruptedDlg(Exception e)
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.FrameworkCorruptedDesc(e.Message),
                Const.FrameworkCorruptedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }


        /// <summary> Used when an unhandled exception has been caught by MaxMain </summary>
        public static bool ShowGenericErrorDlg(Exception e)
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.GenericErrorDesc(e),
                Const.GenericErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }


        /// <summary>Show "Selected package has no name</summary>
        public static bool ShowMissingPackageNameDlg()
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.NoPackageName,
                Const.MalformedAssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }


        /// <summary>Show "Dependency not found"</summary>
        public static bool ShowDependentAssemblyDlg(string dependencyFilename)
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                String.Format("{0} must be added to project before adding this reference", dependencyFilename),
                Const.DependencyNotFound, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }


        /// <summary>Show "Could not attach debugger ..."</summary>
        public static bool ShowCouldNotAttachDebuggerDlg()
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.CouldNotAttachDebugger(Config.AppServerIP, Config.AppServerPort), 
                Const.dialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }


        /// <summary>Show "Could not start debug session ..."</summary>
        public static bool ShowCouldNotStartDebuggerDlg(string scriptName)
        {
            MessageBox.Show(Manager.MaxManager.Instance, Const.CouldNotStartDebugger
               (Config.AppServerIP, Config.AppServerPort, scriptName), 
                Const.dialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        /// <summary>Warn that continuing replaces an old db script reference 
        /// due to a naming conflict</summary>
        public static bool ShowOverwriteDatabaseScriptRef(string dbscriptname)
        {
            string cleanScriptname = Path.GetFileNameWithoutExtension(dbscriptname);
            return DialogResult.OK == MessageBox.Show(Manager.MaxManager.Instance,
                String.Format(Const.DbScriptWriteWarning + Const.blank +
                Const.squote + "{0}" + Const.squote + Const.blank + Const.qmark, cleanScriptname), 
                Const.ReplaceDbScript,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }


        /// <summary>Show "Component xxx will be removed from project"</summary>
        public static DialogResult ShowRemoveFromProjectConfirmDlg(string type, string name)
        {
            return MessageBox.Show(Manager.MaxManager.Instance,
                type + name + Const.WillBeRemovedProject, Const.RemoveFromProjectTitle,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }


        /// <summary>Show one of the file not found messages</summary>
        public static bool ShowFileNotFoundDlg(string msg)
        {
            MessageBox.Show(Manager.MaxManager.Instance, msg,
                Const.FileNotFoundDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }


        /// <summary>Show unsupported project file version message</summary>
        public static bool ShowUnsupportedVersionDlg(string path, string version)
        {
            MessageBox.Show(Manager.MaxManager.Instance, Const.UnsupportedVersionMsg(path, version), 
                Const.CannotOpenScriptMsg, MessageBoxButtons.OK, MessageBoxIcon.Stop);  
            return false;
        }


        /// <summary>Show multiple references so cannot delete/rename node message</summary>
        public static bool ShowMultipleReferencesAlert(bool deleting)
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.RenameHandlerAtTreeMsg(deleting),
                Const.RenameHandlerDlgTitle, MessageBoxButtons.OK); 
            return false; 
        }


        public static bool ShowDuplicateResourceDlg()
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.DuplicateResxNameMsg, Const.DuplicateResxDlgTitle, MessageBoxButtons.OK); 
            return false; 
        }


        public static bool ShowPrintFailedDlg()
        {
            MessageBox.Show(Manager.MaxManager.Instance, Const.CouldNotPrintScriptMsg, 
                   Const.CouldNotPrintScriptDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop); 
            return false; 
        }


        public static bool ShowDebuggerConnectionLostDlg()
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.DebuggerConnectionLostMsg(Config.AppServerIP, Config.AppServerPort),
                Const.dialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop); 
            return false; 
        }


        /// <summary>Show user that nodecount number of actions will be deleted from toolbox</summary>
        public static bool ShowDeleteActionsToolboxMsg(string name, int nodecount)
        {
            return DialogResult.OK == MessageBox.Show(Const.deleteNativeActionsToolboxMsg(name, nodecount),
                Const.deleteNativeActionsToolboxTitle(nodecount),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        /// <summary>Show "Versions of VisDev and Framework out of sync"</summary>
        public static bool OutOfSyncMsg()
        {
            MessageBox.Show(Manager.MaxManager.Instance, 
                Const.outOfSync, Const.outOfSyncDlg, 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            return false;
        }

        /// <summary>Show 'Deployment was unsuccessful'</summary>
        public static bool DeploymentFailed()
        {
            MessageBox.Show(Manager.MaxManager.Instance,
                Const.deployFailed, Const.deployFailedDlg,
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }


        /// <summary>Show "Loop and n nodes will be deleted"</summary>
        public static DialogResult PromptDeleteLoop(int nodecount)
        {
            return MessageBox.Show(Manager.MaxManager.Instance, Const.DeleteLoopMsg(nodecount), 
                Const.DeleteLoopTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }


        /// <summary>Show "Removing all locales will cause all strings (rows) to dissappear.
        ///          Continue?"</summary>
        public static DialogResult WarnDeletionAllStrings()
        {
            return MessageBox.Show(
                Const.WarnDeleteLastLocale, Const.WarnDeleteLastLocaleTitle,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }


        /// <summary> Notify that this locale is already defined</summary>
        public static void DuplicateLocaleDefined()
        {
            MessageBox.Show( Const.LocaleDefined, Const.LocaleDefinedTitle,
                             MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


         /// <summary> Notify that this localizable string is already defined</summary>
        public static void DuplicateStringDefined()
        {
            MessageBox.Show( Const.LocalizableStringDefined, Const.LocalizableStringDefinedTitle,
                             MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        /// <summary>Compacts a path string for display on menu</summary>
        public static string CompactPath(string path, int size)
        {
            System.Text.StringBuilder shortPath = new System.Text.StringBuilder(260);
            PathCompactPathEx(shortPath, path, size, 0); 
            return shortPath.ToString();
        }

        [DllImport(Const.shlwapi, CharSet=CharSet.Auto)]
        public static extern bool PathCompactPathEx(
            [MarshalAs(UnmanagedType.LPTStr)]
            System.Text.StringBuilder pszOut,
            [MarshalAs(UnmanagedType.LPTStr)]
            string pszSource,
            [MarshalAs(UnmanagedType.U4)]
            int cchMax,
            [MarshalAs(UnmanagedType.U4)]
            int dwReserved); 


        [DllImport(Const.shlwapi, CharSet=CharSet.Auto)]
        public static extern bool PathRelativePathTo(
            System.Text.StringBuilder pszPath,
            string pszFrom,
            [MarshalAs(UnmanagedType.U4)] int dwAttrFrom,
            string pszTo,
            [MarshalAs(UnmanagedType.U4)] int dwAttrTo);

        #region PathRelativePathToExample
        // StringBuilder sOut = new StringBuilder(Const.MAX_PATH); 
        // PathRelativePathTo(sOut,
        //  @"c:\temp\myfile.tmp", Const.FILE_ATTRIBUTE_NORMAL,
        //  @"c:\program files",   Const.FILE_ATTRIBUTE_DIRECTORY);
        #endregion
                  
        [DllImport(Const.user32)]
        public static extern IntPtr GetFocus();                                                 

        [DllImport(Const.user32, SetLastError=true, CharSet=CharSet.Auto)]
        public static extern IntPtr FindWindow(
            [MarshalAs(UnmanagedType.LPTStr)] string className,
            [MarshalAs(UnmanagedType.LPTStr)] string windowName);

        [DllImport(Const.user32)]
        public static extern int GetScrollPos(IntPtr hwnd, int whichbar);
 
        [DllImport(Const.user32)]
        public static extern int SendMessage(IntPtr hwnd, int msg, int wparam, int lparam);

        [DllImport(Const.user32)]
        public static extern int PostMessage(IntPtr hwnd, int msg, int wparam, int lparam);

        public delegate int HookProc(int nCode, IntPtr wParam, int lParam);

        [DllImport(Const.user32, EntryPoint="SetWindowsHookEx", CharSet=CharSet.Auto)]
        public static extern int SetWindowsHookEx(
            int idHook, 
            HookProc lpfn,   
            IntPtr hmod, 
            int dwThreadId);

        [DllImport(Const.user32, EntryPoint="CallNextHookEx", CharSet=CharSet.Auto )]
        public static extern  int CallNextHookEx(int hHook, int nCode, IntPtr wParam, int lParam);

        [DllImport(Const.user32, EntryPoint="UnhookWindowsHookEx", CharSet=CharSet.Auto )]
        public static extern int UnhookWindowsEx(int hHook);

        public struct KeyInfo
        {
            public int  repeatCount;
            public int  scanCode;
            public bool isExtendedKey;
            public bool isAltPress;
            public bool isAlreadyDown;
            public bool isCurrentlyPressed;

            public KeyInfo(int repeatCount, int scanCode, bool isExtendedKey, 
                bool isAltPress, bool isAlreadyDown, bool isCurrentlyPressed)
            {
                this.repeatCount = repeatCount;
                this.scanCode = scanCode;
                this.isExtendedKey = isExtendedKey;
                this.isAltPress = isAltPress;
                this.isAlreadyDown = isAlreadyDown;
                this.isCurrentlyPressed = isCurrentlyPressed;
            }
        }
    
        public static KeyInfo ParseKeyProcLParam(int lParam)
        {           
            int  repeatCount        =  lParam & 0x0000FFFF;
            int  scanCode           = (lParam & 0x00FF0000) >> 16;
            bool isExtendedKey      = (lParam & 0x01000000) == 0x01000000;
            bool isAltPress         = (lParam & 0x20000000) == 0x20000000;
            bool isAlreadyDown      = (lParam & 0x40000000) == 0x40000000; 
            bool isCurrentlyPressed = (lParam & 0x80000000) == 0x80000000;

            return new KeyInfo(repeatCount, scanCode, isExtendedKey, 
                isAltPress, isAlreadyDown, isCurrentlyPressed);
        }

        #region Msdn lparam key info

        //    0-15
        //    Specifies the repeat count. The value is the number of times the keystroke is repeated as a result of the user’s holding down the key.
        //
        //    16-23
        //    Specifies the scan code. The value depends on the original equipment manufacturer (OEM).
        //
        //    24
        //    Specifies whether the key is an extended key, such as a function key or a key on the numeric keypad. The value is 1 if the key is an extended key; otherwise, it is 0.
        //
        //    25-28
        //    Reserved.
        //
        //    29
        //    Specifies the context code. The value is 1 if the ALT key is down; otherwise, it is 0.
        //
        //    30
        //    Specifies the previous key state. The value is 1 if the key is down before the message is sent; it is 0 if the key is up.
        //
        //    31
        //    Specifies the transition state. The value is 0 if the key is being pressed and 1 if it is being released. 

        #endregion

        private static XmlSerializer aePackageSeri 
            = new XmlSerializer(typeof(Metreos.PackageGeneratorCore.PackageXml.packageType));
        private static XmlSerializer ntPackageSeri 
            = new XmlSerializer(typeof(Metreos.PackageGeneratorCore.PackageXml.nativeTypePackageType));

    } // class Utl

}   // namespace
