using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Drawing;


namespace Metreos.Max.Framework.Satellite.Explorer
{  
    ///<summary>MaxExplorerTreeView context menu manager</summary>
    public class MaxExplorerMenu
    {
        private MaxExplorerWindow   explorer;
        private MaxExplorerTreeView tree;
                      
        private MenuCommand mcpBuild;
        private MenuCommand mcpAddScript;
        private MenuCommand mcpAddScriptNew;
        private MenuCommand mcpAddScriptExist;
        private MenuCommand mcpAddInstaller;
        private MenuCommand mcpAddInstalNew;
        private MenuCommand mcpAddInstalExist;
        private MenuCommand mcpAddLocales;
        private MenuCommand mcpAddLocalesNew;
        private MenuCommand mcpAddLocalesExist;
        private MenuCommand mcpAddResource;
        private MenuCommand mcpAddDbScript;
        private MenuCommand mcpAddDbScriptNew;
        private MenuCommand mcpAddDbScriptExist;
        private MenuCommand mcpAddMedia;
        private MenuCommand mcpAddMediaExist;
        private MenuCommand mcpAddVrResx;
        //ivate MenuCommand mcpAddTtsText;
        //ivate MenuCommand mcpAddTtsTextExist;
        //ivate MenuCommand mcpAddTtsTextNew;
        private MenuCommand mcpSepB;
        private MenuCommand mcpAddReference;
        private MenuCommand mcpAddWebService;
        private MenuCommand mcpRename;
        private MenuCommand mcpProps;

        public  MenuCommand mcsGoTo;
        private MenuCommand mcsDelete;
        private MenuCommand mcsBuild;
        private MenuCommand mcsRename;
        private MenuCommand mcsProps;

        private MenuCommand mccClose;
        private MenuCommand mccOpen;
        private MenuCommand mccDelete;
        private MenuCommand mccRename;
        public  MenuCommand mccGoTo;
        private MenuCommand mcnGoTo;
        private MenuCommand mccProps;

        public  MenuCommand mciGoTo;
        private MenuCommand mciSepA;
        private MenuCommand mciRemove;
        private MenuCommand mciProps;

        public  MenuCommand mclGoTo;
        private MenuCommand mclSepA;
        private MenuCommand mclRemove;
        private MenuCommand mclProps;

        public  MenuCommand mcdGoTo;
        private MenuCommand mcdSepA;
        private MenuCommand mcdRemove;
        private MenuCommand mcdProps;

        private MenuCommand mcmRemove;
        private MenuCommand mcmProps;

        private MenuCommand mcalAdd;
        private MenuCommand mcalProps;

        private MenuCommand mcvRemove;
        private MenuCommand mcvProps;
        private MenuCommand mcvSepA;
        public  MenuCommand mcvGoTo;

        private MenuCommand mctRemove;
        private MenuCommand mctProps;

        private MenuCommand mcrAddReference;
        private MenuCommand mcrAddWebService;
        private MenuCommand mcrRemove;
        private MenuCommand mcrProps;

        static MenuCommand mcnDelete = new MenuCommand(Const.menuGenericDelete,
                                       new EventHandler(MaxExplorerWindow.OnMenuNodeDelete));
        static MenuCommand mcnRename = new MenuCommand(Const.menuGenericRename,
                                       new EventHandler(MaxExplorerWindow.OnMenuNodeRename));
        static MenuCommand mcnProps  = new MenuCommand(Const.menuGenericProperties,
                                       new EventHandler(MaxExplorerWindow.OnMenuNodeProperties));
        static MenuCommand separator = new MenuCommand(Const.dash);


        public MaxExplorerMenu(MaxExplorerTreeView tree)
        {
            this.tree = tree;
            this.explorer = tree.parent;

            mccClose  = new MenuCommand(Const.menuGenericClose,    
                        new EventHandler(explorer.OnMenuCanvasOpenClose));
            mccOpen   = new MenuCommand(Const.menuGenericOpen,      
                        new EventHandler(explorer.OnMenuCanvasOpenClose));
            mccDelete = new MenuCommand(Const.menuGenericDelete,    
                        new EventHandler(explorer.OnMenuCanvasDelete));
            mccRename = new MenuCommand(Const.menuGenericRename,    
                        new EventHandler(explorer.OnMenuCanvasRename));
            mccGoTo   = new MenuCommand(Const.menuGenericGoTo,     
                        new EventHandler(explorer.OnMenuCanvasGoTo));
            mcnGoTo   = new MenuCommand(Const.menuGenericGoTo,     
                        new EventHandler(explorer.OnMenuNodeGoTo));
            mccProps  = new MenuCommand(Const.menuGenericProperties,
                        new EventHandler(explorer.OnMenuCanvasProperties));

            mcsGoTo   = new MenuCommand(Const.menuGenericGoTo,     
                        new EventHandler(explorer.OnMenuAppGoTo));
            mcsDelete = new MenuCommand(Const.menuGenericRenameA,     
                        new EventHandler(explorer.OnMenuAppDelete));
            mcsBuild  = new MenuCommand(Const.menuBuild,    
                        new EventHandler(explorer.OnMenuAppBuild));
            mcsRename = new MenuCommand(Const.menuGenericRename,    
                        new EventHandler(explorer.OnMenuAppRename));
            mcsProps  = new MenuCommand(Const.menuGenericProperties,
                        new EventHandler(explorer.OnMenuAppProperties)); 

            mcpBuild  = new MenuCommand(Const.menuBuild,
                        new EventHandler(explorer.OnMenuProjectBuild));

            mcpAddScript        = new MenuCommand(Const.menuFileAddScript);
            mcpAddScriptNew     = new MenuCommand(Const.menuFileAddScriptNew, 
                                  new EventHandler(explorer.OnMenuAddScriptNew));
            mcpAddScriptExist   = new MenuCommand(Const.menuFileAddScriptExist, 
                                  new EventHandler(explorer.OnMenuAddScriptExisting));       
            mcpAddScript.MenuCommands.AddRange
                (new MenuCommand[] {mcpAddScriptNew, mcpAddScriptExist });  
    
            mcpAddInstaller     = new MenuCommand(Const.menuFileAddInstal);
            mcpAddInstalNew     = new MenuCommand(Const.menuFileAddInstalNew, 
                                  new EventHandler(explorer.OnMenuAddInstallerNew));
            mcpAddInstalExist   = new MenuCommand(Const.menuFileAddInstalExist, 
                                  new EventHandler(explorer.OnMenuAddInstallerExisting));  
            mcpAddInstaller.MenuCommands.AddRange
                (new MenuCommand[] {mcpAddInstalNew, mcpAddInstalExist });

            mcpAddLocales       = new MenuCommand(Const.menuFileAddLocales);
            mcpAddLocalesNew    = new MenuCommand(Const.menuFileAddLocalesNew,
                                  new EventHandler(explorer.OnMenuAddLocalesNew));
            mcpAddLocalesExist  = new MenuCommand(Const.menuFileAddLocalesExist,
                                  new EventHandler(explorer.OnMenuAddLocalesExisting));
            mcpAddLocales.MenuCommands.AddRange
                (new MenuCommand[] { mcpAddLocalesNew, mcpAddLocalesExist }); 

            mcpAddResource      = new MenuCommand(Const.menuFileAddResource);

            mcpAddDbScript      = new MenuCommand(Const.menuFileAddDbScript);
            mcpAddDbScriptNew   = new MenuCommand(Const.menuFileAddDbScriptNew, 
                                  new EventHandler(explorer.OnMenuAddDbScriptNew));
            mcpAddDbScriptExist = new MenuCommand(Const.menuFileAddDbScriptExist, 
                                  new EventHandler(explorer.OnMenuAddDbScriptExisting));  
            mcpAddDbScript.MenuCommands.AddRange
                (new MenuCommand[] {mcpAddDbScriptNew, mcpAddDbScriptExist });

            #if(false)
            mcpAddTtsText       = new MenuCommand(Const.menuFileAddTtsText);
            mcpAddTtsTextNew    = new MenuCommand(Const.menuFileAddTtsTextNew, 
                                  new EventHandler(explorer.OnMenuAddTtsTextNew));
            mcpAddTtsTextExist  = new MenuCommand(Const.menuFileAddTtsTextExist, 
                                  new EventHandler(explorer.OnMenuAddTtsTextExisting));  
            mcpAddDbScript.MenuCommands.AddRange
                (new MenuCommand[] {mcpAddTtsTextNew, mcpAddTtsTextExist });
            #endif

            mcpAddMedia         = new MenuCommand(Const.menuFileAddMedia);    
            mcpAddMediaExist    = new MenuCommand(Const.menuFileAddMediaExist, 
                                  new EventHandler(explorer.OnMenuAddMediaExisting));  
            mcpAddMedia.MenuCommands.Add(mcpAddMediaExist); 
            mcpAddVrResx        = new MenuCommand(Const.menuFileAddVrResx, 
                                  new EventHandler(explorer.OnMenuAddVrResx));

            mcpAddResource.MenuCommands.AddRange (new MenuCommand[] 
             {mcpAddDbScript, mcpAddMedia, mcpAddVrResx 
              //, mcpAddTtsText // remove comment to add TTS text to context menu
             }); 

            mcpSepB             = new MenuCommand(Const.dash);
            mcpAddReference     = new MenuCommand(Const.menuProjectAddReference,
                                  new EventHandler(explorer.OnMenuProjectAddReference));
            mcpAddWebService    = new MenuCommand(Const.menuProjectAddWebService,
                                  new EventHandler(explorer.OnMenuProjectAddWebService));
  
            mcpRename = new MenuCommand(Const.menuGenericRename,    
                        new EventHandler(explorer.OnMenuProjectRename)); 
            mcpProps  = new MenuCommand(Const.menuGenericProperties,    
                        new EventHandler(explorer.OnMenuProjectProperties)); 

            mciSepA   = new MenuCommand(Const.dash);
            mciGoTo   = new MenuCommand(Const.menuGenericGoTo, 
                        new EventHandler(explorer.OnMenuInstallerGoTo)); 
            mciRemove = new MenuCommand(Const.menuGenericRenameA, 
                        new EventHandler(explorer.OnMenuInstallerRemove)); 
            mciProps  = new MenuCommand(Const.menuGenericProperties,    
                        new EventHandler(explorer.OnMenuInstallerProperties));

            mclSepA = new MenuCommand(Const.dash);
            mclGoTo = new MenuCommand(Const.menuGenericGoTo,
                        new EventHandler(explorer.OnMenuLocalesGoTo));
            mclRemove = new MenuCommand(Const.menuGenericRenameA,
                        new EventHandler(explorer.OnMenuLocalesRemove));
            mclProps = new MenuCommand(Const.menuGenericProperties,
                        new EventHandler(explorer.OnMenuMediaProperties)); 

            mcdSepA   = new MenuCommand(Const.dash);
            mcdGoTo   = new MenuCommand(Const.menuGenericGoTo, 
                        new EventHandler(explorer.OnMenuDatabaseGoTo)); 
            mcdRemove = new MenuCommand(Const.menuGenericRenameA, 
                        new EventHandler(explorer.OnMenuDatabaseRemove)); 
            mcdProps  = new MenuCommand(Const.menuGenericProperties,    
                        new EventHandler(explorer.OnMenuDatabaseProperties)); 

            mcmRemove = new MenuCommand(Const.menuGenericRenameA, 
                        new EventHandler(explorer.OnMenuMediaRemove)); 
            mcmProps  = new MenuCommand(Const.menuGenericProperties,    
                        new EventHandler(explorer.OnMenuMediaProperties));

            mcalAdd = new MenuCommand(Const.menuGenericAddNewItem,
                        new EventHandler(explorer.OnMenuMediaAdd));
            mcalProps = new MenuCommand(Const.menuGenericProperties,
                        new EventHandler(explorer.OnMenuMediaProperties));  

            mcvRemove = new MenuCommand(Const.menuGenericRenameA, 
                        new EventHandler(explorer.OnMenuVrResxRemove)); 
            mcvProps  = new MenuCommand(Const.menuGenericProperties,    
                        new EventHandler(explorer.OnMenuVrResxProperties)); 
            mcvGoTo   = new MenuCommand(Const.menuGenericGoTo, 
                        new EventHandler(explorer.OnMenuVrResxGoTo)); 
            mcvSepA   = new MenuCommand(Const.dash);
            
            mctRemove = new MenuCommand(Const.menuGenericRenameA, 
                        new EventHandler(explorer.OnMenuTtsTextRemove)); 
            mctProps  = new MenuCommand(Const.menuGenericProperties,    
                        new EventHandler(explorer.OnMenuTtsTextProperties));              

            mcrAddReference  = new MenuCommand(Const.menuProjectAddReference,    
                               new EventHandler(explorer.OnMenuProjectAddReference));  
            mcrAddWebService = new MenuCommand(Const.menuProjectAddWebService,    
                               new EventHandler(explorer.OnMenuProjectAddWebService));  
            mcrRemove = new MenuCommand(Const.menuGenericRenameA, 
                        new EventHandler(explorer.OnMenuReferenceRemove));
            mcrProps  = new MenuCommand(Const.menuGenericProperties,    
                        new EventHandler(explorer.OnMenuReferenceProperties));   
        }                   


        ///<summary>Pop node context menu</summary>
        public void PopContextMenu()
        {
            MaxTreeNode node = tree.GetMaxTreeNodeAt(MaxExplorerTreeView.mouseXY);
            if (node == null) return;

            PopupMenu contextmenu = new PopupMenu();

            switch(node.NodeType)
            {
               case MaxTreeNode.NodeTypes.Node:
                    this.MakeNodeContextMenu(contextmenu, node); 
                    break;

               case MaxTreeNode.NodeTypes.Canvas:
                    this.MakeCanvasContextMenu(contextmenu, node);                       
                    break;

               case MaxTreeNode.NodeTypes.App:
                    this.MakeAppContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.Project:
                    this.MakeProjectContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.Installer:
                    this.MakeInstallerContextMenu(contextmenu, node);
                    break;

                case MaxTreeNode.NodeTypes.Locales:
                    this.MakeLocalesContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.DbScript:
                    this.MakeDatabaseContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.Audio:
                    this.MakeMediaContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.AudioLocale:
                    this.MakeAudioLocaleContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.VrResx: // 0120
                    this.MakeVoiceRecResourceContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.TtsText:
                    this.MakeTtsTextContextMenu(contextmenu, node);
                    break;

               case MaxTreeNode.NodeTypes.Folder:
                    switch(((MaxFolderTreeNode)node).FolderType) 
                    {
                       case MaxFolderTreeNode.FolderTypes.References:
                            this.MakeReferencesContextMenu(contextmenu, node);
                            break;
                    }
                    break;

               case MaxTreeNode.NodeTypes.Reference:
                    this.MakeReferenceContextMenu(contextmenu, node);
                    break;

               default: return;
            }
     
            MenuCommand selection = contextmenu.TrackPopup
                (tree.PointToScreen(MaxExplorerTreeView.mouseXY));
        }


        ///<summary>Construct the context menu for a node node</summary>
        ///<remarks>We no longer show node detail in explorer view</remarks>
        public void MakeNodeContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            #if(false)
            MaxExplorerWindow.ToolInfo info = node.Tag as MaxExplorerWindow.ToolInfo; 

            MaxTreeNode canvasNode = info.toolType == Max.Drawing.NodeTypes.Function?
                explorer.FindByCanvasName(info.appName, node.NodeName): null;    

            MaxExplorerWindow.CanvasInfo canvasInfo = canvasNode == null? null: 
                canvasNode.Tag as MaxExplorerWindow.CanvasInfo;

            bool isNodeOnActiveCanvas = canvasInfo != null && canvasInfo.isActive;
               
            mcnRename.Enabled = mcnDelete.Enabled = false;  
            mcnGoTo.Visible   = info.toolType == Max.Drawing.NodeTypes.Function && !isNodeOnActiveCanvas;
            mcnGoTo.Tag       = info;  // Pass node info in tag for now

            contextmenu.MenuCommands.AddRange
                (new MenuCommand[] {mcnGoTo, mcnRename, mcnDelete, separator, mcnProps});
            #endif
        }


        ///<summary>Construct the context menu for a canvas treenode</summary>
        public void MakeCanvasContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            MaxExplorerWindow.CanvasInfo info = explorer.GetCanvasInfoAtMouse();
            if  (info == null) return;

            bool isCurrentView = info.appName == explorer.Maxmain.CurrentViewName; 

            mccOpen.Visible    = false; // !info.isShown; Go To does double duty
            mccClose.Visible   = isCurrentView && info.isShown;
            mccGoTo.Visible    = isCurrentView && !info.isActive;  
            mccDelete.Visible  = isCurrentView && !node.isPrimaryCanvas();  

            mccDelete.Enabled  = mccRename.Enabled = isCurrentView && !node.isPrimaryCanvas();

            if (mccDelete.Enabled)                // Can only delete called functions
            {                                     // and unsolicited events from Explorer
                MaxAppTree appTree = Manager.MaxManager.Instance.AppTree();
                MaxAppTreeNode treenode = appTree.GetFirstEntryFor(info.canvasName);
                mccDelete.Enabled  = MaxAppTreeMenu.IsCalledFunction(treenode) 
                    || appTree.Tree.Menu.IsUnsolicitedEvent(treenode as MaxAppTreeNodeEVxEH);     
            }
 
            // For now we only have properties for the application canvas
            mccProps.Enabled  = isCurrentView && node.isPrimaryCanvas(); 

            mccClose.Tag = mccDelete.Tag = mccGoTo.Tag = mccRename.Tag = mccProps.Tag = node;

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mccOpen, mccGoTo, mccClose, separator, mccRename, mccDelete, separator, mccProps
            });
        }


        ///<summary>Construct the context menu for an app script treenode</summary>
        public void MakeAppContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            MaxExplorerWindow.AppInfo info = explorer.GetAppInfoAtMouse();
            if  (info == null) return;

            mcsGoTo.Visible   = info.appName != explorer.Maxmain.CurrentViewName; 

            mcsRename.Enabled = false; 

            mcsDelete.Tag = mcsGoTo.Tag = mcsRename.Tag = mcsProps.Tag = mcsBuild.Tag = node;

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcsGoTo, mcsRename, mcsDelete, separator, mcsBuild, separator, mcsProps
            });
        }


        ///<summary>Construct the context menu for the project treenode</summary>
        public void MakeProjectContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            mcpRename.Enabled = false;

            mcpAddInstaller.Enabled = !explorer.Maxmain.InstallerPresent;
            
            mcpAddLocales.Enabled = !explorer.Maxmain.LocalesPresent;

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcpBuild, separator, 
                mcpAddScript, mcpAddInstaller, mcpAddLocales, mcpAddResource, 
                mcpSepB,   mcpAddReference, mcpAddWebService,
                separator, mcpRename, separator, mcpProps
            });
        }


        ///<summary>Construct the context menu for a installer node</summary>
        public void MakeInstallerContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {      
            bool isAlreadyOpen = MaxMain.View.IsInstaller;
       
            mciGoTo.Visible = mciSepA.Visible = !isAlreadyOpen;

            mciGoTo.Tag = mciRemove.Tag = mciProps.Tag = node;

            mciProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mciGoTo, mciSepA, mciRemove, separator, mciProps
            });
        }


        ///<summary>Construct the context menu for a locales node</summary>
        public void MakeLocalesContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            bool isAlreadyOpen = MaxMain.View.IsLocales;

            mclGoTo.Visible = mclSepA.Visible = !isAlreadyOpen;

            mclGoTo.Tag = mclRemove.Tag = mclProps.Tag = node;

            mclProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mclGoTo, mclSepA, mclRemove, separator, mclProps
            });
        }


        ///<summary>Construct the context menu for a database node</summary>
        public void MakeDatabaseContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            MaxExplorerWindow.MediaInfo info = explorer.GetMediaInfoAtMouse();
            if  (info == null) return;

            bool isAlreadyOpen 
                = MaxMain.View.IsDatabase && MaxMain.View.Name == info.name;

            mcdGoTo.Visible = mcdSepA.Visible = !isAlreadyOpen;

            mcdGoTo.Tag = mcdRemove.Tag = mcdProps.Tag = node;

            mcdProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcdGoTo, mcdSepA, mcdRemove, separator, mcdProps
            });
        }


        ///<summary>Construct the context menu for a media node</summary>
        public void MakeAudioLocaleContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            mcalAdd.Tag = mcalProps.Tag = node;

            mcalProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcalAdd, separator, mcalProps
            });
        } 


        ///<summary>Construct the context menu for a media node</summary>
        public void MakeMediaContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            mcmRemove.Tag = mcmProps.Tag = node;

            mcmProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcmRemove, separator, mcmProps
            });
        } 



        ///<summary>Construct the context menu for a voice rec resx node</summary>
        public void MakeVoiceRecResourceContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            MaxExplorerWindow.MediaInfo info = explorer.GetMediaInfoAtMouse();
            if  (info == null) return;

            bool isAlreadyOpen 
                = MaxMain.View.IsVrResx && MaxMain.View.Name == info.name;

            mcvGoTo.Visible = mcvSepA.Visible = !isAlreadyOpen;

            mcvGoTo.Tag = mcdRemove.Tag = mcdProps.Tag = node;

            mcvProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcvGoTo, mcvSepA, mcvRemove, separator, mcvProps
            });
        }



        ///<summary>Construct the context menu for a media node</summary>
        public void MakeTtsTextContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            mctRemove.Tag = mctProps.Tag = node;

            mctProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mctRemove, separator, mctProps
            });
        } 


        ///<summary>Construct the context menu for the references node</summary>
        public void MakeReferencesContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            mcrAddReference.Tag = node;
            contextmenu.MenuCommands.Add(mcrAddReference);
            contextmenu.MenuCommands.Add(mcrAddWebService);
        } 


        ///<summary>Construct the context menu for a media node</summary>
        public void MakeReferenceContextMenu(PopupMenu contextmenu, MaxTreeNode node)
        {
            mcrRemove.Tag = mcrProps.Tag = node;
            mcrProps.Enabled = false; // for now

            contextmenu.MenuCommands.AddRange(new MenuCommand[] 
            {
                mcrRemove, separator, mcrProps
            });
        } 

    } // class MaxExplorerMenu
}   // namespace
