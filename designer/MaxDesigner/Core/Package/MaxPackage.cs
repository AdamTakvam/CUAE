using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Max.Resources.Images;
using Metreos.Max.Manager;
using PropertyGrid.Core;


namespace Metreos.Max.Core.Package
{
    /// <summary>Represents a samoa action/event package</summary>
    public class MaxPackage
    {
        #region IndexResult Struct
        public class IndexResult
        {
            public MaxActionTool ActionTool 
            {
                get { return actionTool; } 
                set { actionTool = value; }
            }

            public MaxEventTool EventTool
            {
                get { return eventTool; } 
                set { eventTool = value; }
            }

            private MaxActionTool actionTool;
            private MaxEventTool eventTool;

            public IndexResult () {}
        }
        #endregion

        private static XmlSerializer packageDeserializer  
                 = new XmlSerializer(typeof(packageType));

        protected string name;
        protected string description;

        private   MaxImageList imagesSm;
        private   MaxImageList imagesLg;
        private   int          imageIndexSm;
        private   int          imageIndexLg;
        private   long         packageID;
        private   bool         imagesLocked;
        private   bool         isDefaultFolder;
        private   bool         isFrameworkPackage;
        private   packageType  package;

        private   string       path;
        private   IconInfo     iconInfo;
        private   MaxPackages  parent;

        // Inner boolean to indicated how MaxPackage is constructed. Refactoring 
        // the two constructors into seperate derived classes might be more elegant 
        protected bool         dllImported;
        protected ArrayList    tools;
        public    ArrayList    Tools    { get { return tools;       } }
        public    string       FilePath { get { return path; } }
        public    long         PackageID{ get { return packageID;   } }
        public    IconInfo     Iconinfo { get { return iconInfo;    } }
        public MaxImageList    ImagesSm { get { return imagesSm;    } }
        public MaxImageList    ImagesLg { get { return imagesLg;    } }
        public string Name              { get { return name;        } }
        public string Description       { get { return description; } }
        public bool   ImagesLocked      { get { return imagesLocked;} }
        public bool   IsFrameworkPackage{ get { return isFrameworkPackage; } }

        public MaxPackage(MaxPackages parent, string path)
        {
            this.dllImported    = false;
            this.parent         = parent;
            this.path           = path;      
            this.packageID      = Const.Instance.NextNodeID;     
            MaxToolImport.Package   = this;
            this.isDefaultFolder    =  Utl.IsSameDirectory(path, Config.PackagesFolder + Path.DirectorySeparatorChar);
            this.isFrameworkPackage = isDefaultFolder;
            SetDefaultProperties();
            SetDefaultPackageImages();
        } 


        public MaxPackage(MaxPackages parent, packageType package, string path)
        {
            this.dllImported  = true;
            this.parent       = parent;
            this.path         = path;  
            this.package      = package;
            this.packageID    = Const.Instance.NextNodeID;     
            MaxToolImport.Package   = this;
            this.isDefaultFolder    =  Utl.IsSameDirectory(path, Config.PackagesFolder + Path.DirectorySeparatorChar);
            this.isFrameworkPackage = isDefaultFolder;
            SetDefaultProperties();
            SetDefaultPackageImages();
        }

        /// <summary>Ctor used for default stub package only</summary>
        public MaxPackage(MaxPackages parent)
        {
            this.parent = parent;       
            SetDefaultProperties();
        }


        /// <summary>Create a MaxPackage from XML</summary>
        public int Load()
        {
            if (!dllImported)
                this.package = DeserializePackage(path);

            return this.LoadPackage();
        }
        
        /// <summary>Convert the deserialized package object into MaxTools</summary>
        private int LoadTools(packageType package)
        {
            this.name        = package.name;
            this.description = package.description;

            int  actionCount = package.actionList == null? 0: package.actionList.Length;
            int  eventCount  = package.eventList  == null? 0: package.eventList.Length;

            iconInfo.Reset();

            #if(false)                      // Why is this disabled?
            if(package.icon != null)        // Get package icons
            { 
                for(int i = 0; i < package.icon.Length; i++)
                {
                    GetCurrentIcon(package.icon[i]);
                }

                this.setPackageImages();
            }
            #endif
            
            for(int i = 0; i < actionCount; i++) // Get package actions
                this.LoadActionTool(package.actionList[i]);
            
            for(int i = 0; i < eventCount; i++)  // Get package events
                this.LoadEventTool(package.eventList[i]);

            package = null;
            return actionCount + eventCount;
        }


        /// <summary>Create a MaxActionTool from the specified deserialized action</summary>
        private MaxTool LoadActionTool(actionType xmlaction)
        {
            MaxPmAction pmAction = MaxToolImport.ImportAction(xmlaction);

            MaxActionTool tool   = new MaxActionTool(this, pmAction);  
            tool.PathIsDefault   = this.isDefaultFolder;
          
            this.Tools.Add(tool);       
             
            return tool;
        }


        /// <summary>Create a MaxEventTool from the specified deserialized event</summary>
        private MaxTool LoadEventTool(eventType xmlevent)
        {
            MaxPmEvent pmEvent = MaxToolImport.ImportEvent(xmlevent);

            MaxEventTool tool  = new MaxEventTool(this, pmEvent);
            tool.PathIsDefault = this.isDefaultFolder; 
           
            this.Tools.Add(tool);           
             
            return tool;
        }


        /// <summary>Provides a package given a file path.</summary>
        /// <returns>Null if failed to load package</returns>
        public static packageType DeserializePackage(string filePath)
        {
            FileStream file = null;
            XmlReader xmlReader = null;

            try
            {
                file = new FileStream(filePath, FileMode.Open);

                xmlReader = new XmlTextReader(file);
        
                if (packageDeserializer.CanDeserialize(xmlReader))                 
                    return (packageType) packageDeserializer.Deserialize(xmlReader);                 
            }
            catch(Exception e)
            {
                if(!MaxMain.autobuild)
                    MaxManager.Instance.SignalFrameworkTextMessage(e.Message, true, false); 
            }
            finally
            {
                if (xmlReader != null) xmlReader.Close();                
                if (file      != null) file.Close();   
            }

            return null;
        }


        public void GetCurrentIcon(iconType icon)
        {
            switch(icon.type)
            {
                case iconTypeType.Item16x16x8:
                case iconTypeType.Item16x16x32:
                     iconInfo.small = BinaryToBitmap(icon);
                     break;

                case iconTypeType.Item32x32x8:
                case iconTypeType.Item32x32x32:
                     iconInfo.large = BinaryToBitmap(icon);
                     break;
            }
        }


        private Bitmap BinaryToBitmap(iconType icon)
        {    
            Bitmap bmp = null; 
 
            try
            {
                byte[] iconBytes = System.Convert.FromBase64String(icon.Value);
                        
                System.IO.MemoryStream tempImageData = new MemoryStream(iconBytes);

                bmp = new Bitmap(tempImageData);
               
                bmp.MakeTransparent(bmp.GetPixel(0,0));        
            }
            catch(Exception e)
            {
                if (!MaxMain.autobuild)
                     MaxManager.Instance.SignalFrameworkTextMessage(e.Message, true, true);     
            } 

            return bmp; 
        }


        public class IconInfo
        {
            public static readonly Bitmap defaultSmall = null; 
            public static readonly Bitmap defaultLarge = null; 
            public Bitmap small;
            public Bitmap large;
            public IconInfo()   { Reset(); }
            public void Reset() { small = defaultSmall; large = defaultLarge; }
        }


        /// <summary> Loads the tool set </summary>
        /// <returns> The number of tools found in this package </returns>
        public int LoadPackage()
        {
            int toolcount = 0;

            if (package  != null)
                toolcount = this.LoadTools(package);

            return toolcount;
        }


        /// <summary>Insert supplied package icon into package image list</summary>
        private void setPackageImages()
        {
            if (this.ImagesLocked) return;

            if (iconInfo.small != null)   
                if (this.imagesSm.Remove(0))
                    this.imagesSm.Add(iconInfo.small);

            if (iconInfo.large != null)   
                if (this.imagesLg.Remove(0))
                    this.imagesLg.Add(iconInfo.large);
        }


        /// <summary>Create the custom image list for the package</summary>
        private void SetDefaultPackageImages()
        {
            // We always create a custom image list for the package, even if the
            // package is to use default images. This is so that if the package
            // image is not supplied, but one or more tool images are supplied,
            // then the tools will have an image list to which to append.

            this.imagesLocked = false;      // Indicates if tools can add images

            MaxImageList defaultImagesSm = MaxImageIndex.Instance.StockToolImages16x16;
            MaxImageList defaultImagesLg = MaxImageIndex.Instance.StockToolImages32x32;
            // Copy image[0] from default images
            MaxImageList imagesSm = new MaxImageList(MaxImageList.ImagesSizes.Small);
            int imageindex = imagesSm.Add(defaultImagesSm[0]);

            if (imageindex >= 0)
            {
                this.imagesSm     = imagesSm;
                this.imageIndexSm = imageindex;
            }
            else                            // Some error creating image list
            {           
                this.imagesSm     = defaultImagesSm;
                this.imageIndexSm = MaxImageIndex.stockTool16x16IndexToolGroup;
                this.imagesLocked = true;   // Don't allow tools to overwrite defaults
            }
             
            // Copy image[0] from default images
            MaxImageList imagesLg = new MaxImageList(MaxImageList.ImagesSizes.Large);
            imageindex = imagesLg.Add(defaultImagesLg[0]);

            if (imageindex >= 0)
            {
                this.imagesLg     = imagesLg;
                this.imageIndexLg = imageindex;
            }
            else                            // Some error creating image list
            {           
                this.imagesLg     = defaultImagesLg;
                this.imageIndexLg = MaxImageIndex.stockTool32x32IndexToolGroup;
                this.imagesLocked = true;   // Don't allow tools to overwrite defaults
            }  
        }   // SetDefaultPackageImages()


        /// <summary>Return the MaxTool from this package having the specified name</summary>
        public IndexResult this[string fullyQualifedToolName] 
        {   
            get 
            {
                IndexResult result = new IndexResult();
                if(this.Tools == null) return result;

                foreach(MaxTool tool in this.tools)
                {
                    if (tool != null && tool.FullQualName == fullyQualifedToolName && tool.IsAction)
                        result.ActionTool = tool as MaxActionTool;

                    else 
                    if (tool != null && tool.FullQualName == fullyQualifedToolName && tool.IsEvent)
                        result.EventTool = tool as MaxEventTool;

                        // For stock tools
                    else 
                    if (tool != null && tool.FullQualName == fullyQualifedToolName && !tool.IsEvent && !tool.IsAction)
                        result.ActionTool = tool as MaxActionTool;
                }

                return result;
            }
        }     


        /// <summary>Return the action MaxTool from this package having specified name</summary>
        public MaxTool ActionTool(string fullyQualifedToolName)
        {   
            foreach(MaxTool tool in this.tools)
                if (tool != null && tool.FullQualName == fullyQualifedToolName && tool.IsAction) 
                    return tool;

            return null;            
        }


        /// <summary>Return the event MaxTool from this package having specified name</summary>
        public MaxTool EventTool(string fullyQualifedToolName)
        {   
            foreach(MaxTool tool in this.tools)
                if (tool != null && tool.FullQualName== fullyQualifedToolName && tool.IsEvent) 
                    return tool;

            return null;            
        }


        /// <summary>Return the MaxTool from this package having the specified index</summary>
        public MaxTool this[int index]
        {   
            get { return (index < 0 || index >= this.tools.Count)? null: (MaxTool)this.tools[index]; }
        }


        /// <summary>Ctor common initialization</summary>
        private void SetDefaultProperties()
        {
            this.iconInfo    = new IconInfo();
            this.tools       = new ArrayList();

            this.imagesSm = MaxImageIndex.Instance.StockToolImages16x16;
            this.imagesLg = MaxImageIndex.Instance.StockToolImages32x32;
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexToolGroup;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexToolGroup;
        }


        /// <summary>Write this package info to project file</summary>
        public void MaxSerialize(XmlTextWriter writer)
        {
            writer.WriteStartElement(Const.xmlEltPackage);       
            writer.WriteAttributeString(Const.xmlAttrName, this.name);
            writer.WriteEndElement();
        }
    }       // class MaxPackage

}           // namespace
             















