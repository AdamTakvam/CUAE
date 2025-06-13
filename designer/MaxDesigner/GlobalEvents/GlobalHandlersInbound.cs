using System;
using Metreos.Max.Core;
using Metreos.Max.Manager;



namespace Metreos.Max.GlobalEvents
{

    /// <summary>Handlers for events inbound from IDE framework to Max</summary>
    public class InboundHandlers
    {                    
        #region singleton     
        private InboundHandlers() {}
        private static InboundHandlers instance;
        public  static InboundHandlers Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new InboundHandlers();
                    instance.Init();
                }
                return instance;
            }
        }
        #endregion  

        private void Init()
        {
            propertiesActivityCallback 
                = new MaxPropertiesActivityHandler(Instance.OnPropertiesActivity);
            toolboxActivityCallback    
                = new MaxToolboxActivityHandler(Instance.OnToolboxActivity);
            explorerActivityCallback   
                = new MaxExplorerActivityHandler(Instance.OnExplorerActivity);
            menuActivityCallback       
                = new MaxMenuActivityHandler(Instance.OnMenuActivity);
            userInputCallback          
                = new MaxUserInputHandler(Instance.OnUserInput);
            tabRequestCallback         
                = new MaxTabActivityHandler (Instance.OnTabChangeRequest);
            frameworkRequestCallback   
                = new MaxFrameworkActivityHandler (Instance.OnFrameworkRequest);
        }
 
        public  static MaxPropertiesActivityHandler PropertiesActivityCallback
        { 
            get { return propertiesActivityCallback; }
        }

        public  static MaxToolboxActivityHandler    ToolboxActivityCallback
        { 
            get { return toolboxActivityCallback; }
        }

        public  static MaxExplorerActivityHandler   ExplorerActivityCallback
        {
            get { return explorerActivityCallback; }
        }

        public  static MaxMenuActivityHandler  MenuActivityCallback
        {
            get { return menuActivityCallback; }
        }

        public  static MaxUserInputHandler     UserInputCallback
        {
            get { return userInputCallback; }
        }

        public  static MaxTabActivityHandler  TabRequestCallback
        {
            get { return tabRequestCallback; }
        }

        public  static MaxFrameworkActivityHandler  FrameworkRequestCallback
        {
            get { return frameworkRequestCallback; }
        }


        /// <summary>Notify Max of framework properties window state change</summary>
        public void OnPropertiesActivity(object sender, MaxPropertiesEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxPropertiesEventArgs.MaxEventTypes.Changed:

                    MaxProject.Instance.MarkViewDirty();
                    break;
            }
        }


        /// <summary>Pass a request for toolbox content down to Max</summary>
        public void OnToolboxActivity(object sender, MaxToolboxEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxToolboxEventArgs.MaxEventTypes.ToolGroupRequest:
             
                    MaxManager.Instance.OnToolGroupsRequest(e);
                    break;
            }
        }


        /// <summary>Notify Max of explorer window state change</summary>
        public void OnExplorerActivity(object sender, MaxExplorerEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxExplorerEventArgs.MaxEventTypes.ToBeDefined:
                    break;
            }
        }


        /// <summary>Notify Max of a main menu selection</summary>
        public void OnMenuActivity(object sender, MaxMenuEventArgs e)
        {
            MaxProject project = MaxProject.Instance;

            switch(e.MaxEventType)
            {
               case MaxMenuEventArgs.MaxEventTypes.EditCopy:
                    project.OnEditCopy();
                    break;
               case MaxMenuEventArgs.MaxEventTypes.EditCut:
                    project.OnEditCut();
                    break;
               case MaxMenuEventArgs.MaxEventTypes.EditDelete:
                    project.OnEditDelete();
                    break;
               case MaxMenuEventArgs.MaxEventTypes.EditPaste:
                    project.OnEditPaste();
                    break;
               case MaxMenuEventArgs.MaxEventTypes.EditRedo:
                    project.OnEditRedo();
                    break;            
               case MaxMenuEventArgs.MaxEventTypes.EditSelectAll:
                    project.OnEditSelectAll();
                    break;
               case MaxMenuEventArgs.MaxEventTypes.EditUndo:
                    project.OnEditUndo();
                    break;
               case MaxMenuEventArgs.MaxEventTypes.OptWaitMotion: // 511
                    project.OnOptionWaitMotionChanged(e);
                    break;
               case MaxMenuEventArgs.MaxEventTypes.OptLargePorts:  
                    project.OnOptionLargePortsChanged(e);
                    break;
               case MaxMenuEventArgs.MaxEventTypes.OptVisiblePorts:  
                    project.OnOptionVisiblePortsChanged(e);
                    break;
            }       
        }

  
        ///<summary>Notify Max of user input</summary>
        public void OnUserInput(object sender, MaxUserInputEventArgs e)
        {
            MaxManager manager = MaxManager.Instance;
            MaxProject project = MaxProject.Instance;    

            switch(e.MaxEventType)
            {
               case MaxUserInputEventArgs.MaxEventTypes.NewProject:
                    project.New(e.UserInput1, e.UserInput2);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.OpenProject:
                    project.Open(e.UserInput1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.SaveProject:
                    if  (e.UserInt1 != 0)
                          MaxManager.ShuttingDown = true;

                    if (e.UserInput1 == null)
                         project.Save();
                    else project.SaveAs(e.UserInput1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.CloseProject:
                    project.Close(e.UserInt1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.SaveFile:  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RenameCanvas:
                    project.OnRenameCanvas(e.UserInput1, e.UserInput2);    
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RenameNode:
                    project.OnRenameNode(e.UserInput1, e.UserLong1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RemoveCanvas:
                    project.OnRemoveCanvas(e.UserInput1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RemoveNode:
                    project.OnRemoveNode(e.UserInput1, e.UserLong1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.AddNewScript:     
                    project.OnAddNewScript(e.UserInput1, e.UserInput2);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.AddExistingScript:  
                    project.OnAddExistingScript(e.UserInput1, e.UserInput2);
                    break;
       
               case MaxUserInputEventArgs.MaxEventTypes.OpenScript:
                    project.OnOpenScript(e.UserInput1, e.UserInput2);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RemoveScript:
                    project.OnRemoveScript(e.UserInput1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.AddNewInstaller:   
                    project.OnAddNewInstaller(e.UserInput1); 
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.AddExistingInstaller:  
                    project.OnAddExistingInstaller(e.UserInput1);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.OpenInstaller:  
                    project.OnOpenInstaller(e.UserInput1);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RemoveInstaller:  
                    project.OnRemoveInstaller();  
                    break;

                case MaxUserInputEventArgs.MaxEventTypes.AddNewLocales:
                    project.OnAddNewLocales(e.UserInput1);
                    break;

                case MaxUserInputEventArgs.MaxEventTypes.AddExistingLocales:
                    project.OnAddExistingLocales(e.UserInput1);
                    break;

                case MaxUserInputEventArgs.MaxEventTypes.OpenLocales:
                    project.OnOpenLocales(e.UserInput1);
                    break;

                case MaxUserInputEventArgs.MaxEventTypes.RemoveLocales:
                    project.OnRemoveLocales();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.AddNewDatabase:
                    project.OnAddNewDatabase(e.UserInput1); 
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.AddExistingDatabase:  
                    project.OnAddExistingDatabase(e.UserInput1);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.OpenDatabase:  
                    project.OnOpenDatabase(e.UserInput1);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.OpenVrResx:
                    project.OnOpenVoiceRecResource(e.UserInput1);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RemoveDatabase:  
                    project.OnRemoveDatabase(e.UserInput1);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.AddNewMediaFile:
               case MaxUserInputEventArgs.MaxEventTypes.AddExistingMediaFile:
               case MaxUserInputEventArgs.MaxEventTypes.AddExistingVrResxFile:
                    project.OnAddMediaFile(e.UserInput1, e.UserInput2);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.RemoveMediaFile:
               case MaxUserInputEventArgs.MaxEventTypes.RemoveVrResxFile:
                    project.OnRemoveMediaFile(e.UserInput1, e.UserInput2);  
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.CloseFile:   
                    project.OnCloseView();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Build:   
                    project.Build();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.GoToNode: 
                    manager.NavigateToNode(e.UserInput1, e.UserLong1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Zoom:
                    project.OnZoomRequest(e.UserInt1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Grid:
                    project.OnGridRequest(e.UserInt1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Tray:
                    project.OnViewTray(e.UserInt1);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.FocusCanvas:
                    manager.SetFocusCanvas();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.FocusTray:
                    manager.SetFocusTray();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Test:
                    project.OnTestRequest();
                    break;

                case MaxUserInputEventArgs.MaxEventTypes.PageSet:
                    project.OnPageSetupRequest();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Print:
                    project.OnPrintRequest();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.PrintPreview:
                    project.OnPrintPreviewRequest();
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.ShowProperties:
                    project.OnFrameworkPropertiesRequest(e);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Shutdown:
                    project.OnFrameworkShutdownNotification(e);
                    break;
            }
        }


        public void OnTabChangeRequest(object sender, MaxCanvasTabEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxCanvasTabEventArgs.MaxEventTypes.GoTo:
                    MaxManager.Instance.GoToTab(e.TabName);
                    break;

               case MaxCanvasTabEventArgs.MaxEventTypes.Toggle:
                    MaxManager.Instance.GoToPriorTab();
                    break;

               case MaxCanvasTabEventArgs.MaxEventTypes.Close:
                    MaxManager.Instance.RemoveTab(e.TabName);
                    break;
            }      
        }


        public void OnFrameworkRequest(object sender, MaxFrameworkEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxFrameworkEventArgs.MaxEventTypes.Start:
                    MaxManager.Instance.Start(e);
                    break;

               case MaxFrameworkEventArgs.MaxEventTypes.IdeModified:
                    MaxManager.Instance.OnSaveIdeRequest();
                    break;     
            }      
        }

        private static MaxPropertiesActivityHandler propertiesActivityCallback;
        private static MaxToolboxActivityHandler    toolboxActivityCallback;
        private static MaxExplorerActivityHandler   explorerActivityCallback;
        private static MaxFrameworkActivityHandler  frameworkRequestCallback;
        private static MaxMenuActivityHandler       menuActivityCallback;
        private static MaxUserInputHandler          userInputCallback;
        private static MaxTabActivityHandler        tabRequestCallback;

    } // class InboundHandlers

}   // namespace
