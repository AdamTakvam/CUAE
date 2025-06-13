using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Resources.Images;
using Metreos.Max.GlobalEvents;



namespace Metreos.Max.Framework.Satellite.Explorer
{
    ///<summary>A node in the explorer tree view</summary>
    public abstract class MaxTreeNode: TreeNode
    {
        protected MaxTreeNode(string s): base(s)
        {
        }

        public enum NodeTypes 
        { 
            None, Folder, Project, App, Canvas, Node, Installer, 
            Locales, DbScript, Audio, AudioLocale, VrResx, TtsText, Text, Image, 
            References, Reference 
        }

        public bool isCanvasNode() { return nodetype == NodeTypes.Canvas;   }
        public bool isNodeNode()   { return nodetype == NodeTypes.Node;     }

        public bool isSingleton()  
        {
            switch(nodetype) 
            {
                case NodeTypes.Installer: 
                case NodeTypes.Locales:
                case NodeTypes.DbScript: 
                case NodeTypes.Audio: 
                    return true; 
            }
            return false;
        }

        public bool isPrimaryCanvas()  
        {
            if (nodetype != NodeTypes.Canvas || this.Tag == null) return false;
            MaxExplorerWindow.CanvasInfo info = this.Tag as MaxExplorerWindow.CanvasInfo;
            return info != null && info.isPrimary;
        }

        public bool CanDelete()
        {
            switch(nodetype)
            {
                case NodeTypes.Installer: 
                case NodeTypes.Locales: 
                case NodeTypes.DbScript: 
                case NodeTypes.Audio: 
                    return true;
            }
            return false;
        }

        protected NodeTypes nodetype;
   
        public  string    NodeName  { get { return Text;    } }
        public  NodeTypes NodeType  { get { return nodetype;} }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxProjectTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>The first node in the explorer tree view at level zero</summary>
    public class MaxProjectTreeNode: MaxTreeNode
    {
        public MaxProjectTreeNode(string text): base(text)
        {
            // We draw over this node in explorer tree view WM_PAINT, so we don't want
            // to show icon pixels or text. Should we revert to using the placeholder  
            // node as is, we'll want to use the project icon node here instead.
       
            nodetype = MaxTreeNode.NodeTypes.Project;
                                                  
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexTransparent;
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxFolderTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A folder node intended as a parent node</summary>
    public class MaxFolderTreeNode: MaxTreeNode
    {
        public enum FolderTypes 
        { AppScript, DbScript, Media, Audio, VrResource, TtsText, References 
        }
        private FolderTypes foldertype;
        public  FolderTypes FolderType { get { return foldertype; } }

        public MaxFolderTreeNode(string name, FolderTypes foldertype): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.Folder;
            this.foldertype = foldertype;
            int imageIndex  = 0;

            switch(foldertype)
            {
              case FolderTypes.DbScript:          
                   imageIndex = MaxImageIndex.framework16x16IndexDbScripts;
                   break; 
                         
              case FolderTypes.Media:   
                   imageIndex = MaxImageIndex.framework16x16IndexMediaFolder; 
                   break;  

              case FolderTypes.VrResource:
                   imageIndex = MaxImageIndex.framework16x16IndexVrResources; 
                   break; 

               case FolderTypes.TtsText:
                   imageIndex = MaxImageIndex.framework16x16IndexTtsTextFiles; 
                   break;           

              case FolderTypes.References:   
                   imageIndex = MaxImageIndex.framework16x16IndexRefFolder; 
                   break;  

              case FolderTypes.AppScript:          
              case FolderTypes.Audio:             
              default:
                   imageIndex = MaxImageIndex.framework16x16IndexFolderOpen;
                   break;
            }

            this.ImageIndex = this.SelectedImageIndex = imageIndex;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxAppScriptTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>An application script node in the explorer tree view</summary>
    public class MaxAppScriptTreeNode: MaxTreeNode
    {
        public MaxAppScriptTreeNode(string name, MaxExplorerWindow.AppInfo info): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.App;
            this.Tag = info;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexApplication;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxCanvasTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A canvas node in the explorer tree view</summary>
    public class MaxCanvasTreeNode: MaxTreeNode
    {
        public MaxCanvasTreeNode(string name, MaxExplorerWindow.CanvasInfo info): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.Canvas;
            this.Tag = info;   
            this.ImageIndex = this.SelectedImageIndex = info.isPrimary?
                MaxImageIndex.framework16x16IndexCanvas:
                MaxImageIndex.framework16x16IndexFunction;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxItemTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    ///<summary>A subnode in the explorer tree view</summary>
    public class MaxItemTreeNode: MaxTreeNode
    {
        public MaxItemTreeNode(string name, MaxExplorerWindow.ToolInfo info): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.Node;
            this.Tag = info;

            switch(info.toolType)
            {
               case Max.Drawing.NodeTypes.Action:
                    this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexAction;
                    break;

               case Max.Drawing.NodeTypes.Event:
                    this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexEvent;
                    break;

               case Max.Drawing.NodeTypes.Function:
                    this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexFunction;
                    break;

               case Max.Drawing.NodeTypes.Variable:
                    this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexVariable;
                    break;
            }
        }   
    } 


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxInstallerTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>An installer script node in the explorer tree view</summary>
    public class MaxInstallerTreeNode: MaxTreeNode
    {
        public MaxInstallerTreeNode(string name): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.Installer;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexInstaller;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxLocalesTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A locales script node in the explorer tree view</summary>
    public class MaxLocalesTreeNode : MaxTreeNode
    {
        public MaxLocalesTreeNode(string name)
            : base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.Locales;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexLocaleEd;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxDbScriptTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A database script node in the explorer tree view</summary>
    public class MaxDbScriptTreeNode: MaxTreeNode
    {
        public MaxDbScriptTreeNode(string name): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.DbScript;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexDbScript;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxMediaTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A media file node in the explorer tree view</summary>
    public class MaxMediaTreeNode: MaxTreeNode
    {
        public MaxMediaTreeNode(string name): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.None;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxAudioTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>An audio file node in the explorer tree view</summary>
    public class MaxAudioTreeNode: MaxMediaTreeNode
    {
        public MaxAudioTreeNode(string name): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.Audio;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexAudio;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxLocaleTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>An locale folder node in the explorer tree view</summary>
    public class MaxLocaleAudioFolder : MaxMediaTreeNode
    {
        public MaxLocaleAudioFolder(string localeName)
            : base(localeName)
        {
            nodetype = MaxTreeNode.NodeTypes.AudioLocale;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexFolderEmpty;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxVrResxTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A voice rec resource file node in the explorer tree view</summary>
    public class MaxVrResxTreeNode: MaxMediaTreeNode
    {
        public MaxVrResxTreeNode(string name): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.VrResx;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexVrResource;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxTtsTextTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A TTS text file node in the explorer tree view</summary>
    public class MaxTtsTextTreeNode: MaxTreeNode
    {
        public MaxTtsTextTreeNode(string name): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.TtsText;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexTtsTextFiles;
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxReferenceTreeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>An audio file node in the explorer tree view</summary>
    public class MaxReferenceTreeNode: MaxMediaTreeNode
    {
        public MaxReferenceTreeNode(string name): base(name)
        {
            nodetype = MaxTreeNode.NodeTypes.Reference;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.framework16x16IndexReference;
        }
    }


} // namespace
