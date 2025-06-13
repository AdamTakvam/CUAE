using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using Northwoods.Go;
using Metreos.Max.Core;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Max.Core.NativeType;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.Framework.Satellite.Explorer;
using Metreos.AppArchiveCore.Xml;
using PropertyGrid.Core;



namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>Properties grid in docking satellite window</summary>
  
    public class MaxPropertyWindow: Form, MaxSatelliteWindow, IMaxPropManCallbacks
    {
        #region singleton
        private MaxPropertyWindow() {}
        private static MaxPropertyWindow instance;
        public  static MaxPropertyWindow Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new MaxPropertyWindow();
                    instance.Init();
                }
                return instance;
            }
        }
        #endregion

        private void Init()
        {
            // We've made this a singleton for easy access from Max.
            // Before we implement the Visual Studio version, we'll need to
            // come up with a means of separating properties display from
            // properties notification. The grid frame (in framework) must
            // currently be supplied to properties manager (in Max). The
            // MaxTool (Max) must create properties thru properties manager.
            // All this is fine for now, since all is managed, we merely need
            // to eventually come up with the split environment methodology.
      
            InitializeComponent();
            // Map props changed to handler
            this.maxproCallback 
                = new MaxPropertiesManager.GetChangedProperties(OnPropertiesChanged);
            this.maxproFunctionVarsRequest 
                = new MaxPropertiesManager.GetFunctionVars(OnMaxproFunctionVarsRequest);
            this.maxproGlobalVarsRequest   
                = new MaxPropertiesManager.GetGlobalVars  (OnMaxproGlobalVarsRequest);
            this.maxproNativeTypesRequest
                = new MaxPropertiesManager.GetAllNativeTypes(OnMaxproNativeTypesRequest);
            this.maxProNativeTypesFullRequest
                = new MaxPropertiesManager.GetNativeTypesInfo(OnMaxproNativeTypesFullRequest);
            this.maxproGetInitwithValuesRequest
                = new MaxPropertiesManager.GetValidInitWithValues(OnMaxproValidInitWithValuesRequest);
            this.maxproGetConfigParameters
                = new MaxPropertiesManager.GetInstallerConfigParameters(OnMaxProGetConfigParameters);
            this.maxproGetLocaleStrings
                = new MaxPropertiesManager.GetLocalizableStrings(OnMaxProGetLocaleParameters);
            this.maxproGetUsings 
                = new MaxPropertiesManager.GetUsingStatements(OnMaxProGetUsings);
            this.maxProUpdateUsings
                = new MaxPropertiesManager.UpdateUsingStatements(OnMaxProUpdateUsings);
            this.maxproGetAction
                = new MaxPropertiesManager.GetPackagedAction(OnMaxGetAction);
            this.maxproGetGrammarFiles
                = new MaxPropertiesManager.GetGrammarFilesDelegate(OnMaxproGetGrammarFiles);
            this.maxproGetVoiceRecFiles
                = new MaxPropertiesManager.GetVoiceRecFilesDelegate(OnMaxproGetVoiceRecFiles);
            this.maxproGetMediaFiles
                = new MaxPropertiesManager.GetMediaFilesDelegate(OnMaxproGetMediaFiles);
            this.maxproGetDefaultUserType
                = new MaxPropertiesManager.GetDefaultUserType(OnMaxproGetDefaultUserTypes);
            this.maxproGetEventParamByDisplay 
                = new MaxPropertiesManager.GetEventParameter(OnMaxProGetEventParamByDisplay);
            this.maxproGetEventParamByName
                = new MaxPropertiesManager.GetEventParameter(OnMaxProGetEventParamByName);
            // Instantiate properties manager
            this.maxpro = new MaxPropertiesManager(this.grid, this);
        }

        private MaxMain  main;
        private ComboBox combo;
        private MaxPropertyGrid grid; 
        public  MaxPropertyGrid Grid { get { return grid; } }   
        private System.ComponentModel.Container components = null;

        private MaxPropertiesManager                              maxpro;    
        private MaxPropertiesManager.GetChangedProperties         maxproCallback;
        private MaxPropertiesManager.GetFunctionVars              maxproFunctionVarsRequest;
        private MaxPropertiesManager.GetGlobalVars                maxproGlobalVarsRequest;
        private MaxPropertiesManager.GetAllNativeTypes            maxproNativeTypesRequest;
        private MaxPropertiesManager.GetNativeTypesInfo           maxProNativeTypesFullRequest;
        private MaxPropertiesManager.GetValidInitWithValues       maxproGetInitwithValuesRequest;
        private MaxPropertiesManager.GetInstallerConfigParameters maxproGetConfigParameters;
        private MaxPropertiesManager.GetLocalizableStrings        maxproGetLocaleStrings;
        private MaxPropertiesManager.GetUsingStatements           maxproGetUsings;
        private MaxPropertiesManager.UpdateUsingStatements        maxProUpdateUsings;
        private MaxPropertiesManager.GetPackagedAction            maxproGetAction;
        private MaxPropertiesManager.GetMediaFilesDelegate        maxproGetMediaFiles;
        private MaxPropertiesManager.GetGrammarFilesDelegate      maxproGetGrammarFiles;
        private MaxPropertiesManager.GetVoiceRecFilesDelegate     maxproGetVoiceRecFiles;
        private MaxPropertiesManager.GetDefaultUserType           maxproGetDefaultUserType;
        private MaxPropertiesManager.GetEventParameter            maxproGetEventParamByDisplay;
        private MaxPropertiesManager.GetEventParameter            maxproGetEventParamByName;
        public  MaxPropertiesManager PropertiesManager            { get { return maxpro; } } 
  
        private static XmlSerializer installerDeserializer  
          = new XmlSerializer(typeof(installType));
        private static XmlSerializer localesDeserializer
          = new XmlSerializer(typeof(LocaleTableType));

        public enum MediaFileType { audio, grammar, voicerec };


        public void Create(MaxMain main)
        {
            this.main = main;  
        }

    
        /// <summary> Force the property grid to dump any unregistered changes</summary>
        public void CheckPropertyGrid()
        {
            this.ForcePropertyUpdate();
        }


        /// <summary>Properties changed callback from properties manager</summary>
        private void OnPropertiesChanged(MaxProperty[] changedProperties)
        {  
            if (changedProperties == null)       
                main.SignalPropertiesChanged(null);
       
            else foreach(MaxProperty property in changedProperties)
            {      
                MaxSelectableObject maxobject = property.Subject as MaxSelectableObject;
                if (maxobject == null) continue;
                                    
                if (changedProperties != null)
                    maxobject.OnPropertiesChangeRaised(changedProperties);

                main.SignalPropertiesChanged(changedProperties);
                break;        
            }
        }


        /// <summary>Function variable attributes requested from properties manager</summary>
        private object[] OnMaxproFunctionVarsRequest(object propSubject)
        {  
            IMaxNode maxnode = propSubject as IMaxNode; if (maxnode == null) return null;
            MaxCanvas canvas = maxnode.Canvas; 
            IMaxNode[] nodes = canvas.GetFunctionVariables(false);
      
            object[] variableAttributes = GetVariablesAttributes(nodes);

            if (maxnode.Container == 0)  // Is not contained by loop
                return variableAttributes;
            else
            {
                // Special case variable name is loopIndex for actions contained by a loop
                string[] loopIndexAttr = new string[2];
                loopIndexAttr[0] = Metreos.Interfaces.IApp.NAME_LOOP_INDEX;
                loopIndexAttr[1] = "Int"; // TODO: alter to int when Metreos.Types prepend is removed

                // Grow the variableAttributes to one larger
                object[] variablesAttributesLarger = new object[
                    variableAttributes != null? 
                    variableAttributes.Length + 1: 1];

                variablesAttributesLarger[0] = loopIndexAttr;
                if (variableAttributes != null)
                    Array.Copy(variableAttributes, 0, variablesAttributesLarger, 1, variableAttributes.Length); 

                return variablesAttributesLarger;
            }
        }


        /// <summary>Global variables attributes requested from properties manager</summary>
        private object[] OnMaxproGlobalVarsRequest()
        {  
            IMaxNode[] nodes = MaxProject.Instance.GetGlobalVariables();
            return GetVariablesAttributes(nodes);
        }


        /// <summary>All the names of the loaded native types requested from properties manager </summary>
        private string[] OnMaxproNativeTypesRequest()
        {
            return MaxNativeTypes.Instance.AllNativeTypes();
        }


        /// <summary>All the loaded native types requested from properties manager</summary>
        private nativeTypePackageType[] OnMaxproNativeTypesFullRequest()
        {
            return MaxNativeTypes.Instance.AllNativeTypesFull();
        }
    

        /// <summary>Extract and return attributes from array of variable nodes</summary>
        private object[] GetVariablesAttributes(IMaxNode[] nodes)
        { 
            if (nodes == null) return null;
            object[] allVarAttrs = new object[nodes.Length];
            int i = 0;

            foreach(IMaxNode varnode in nodes)       
            { 
                string[] thisVarAttrs = new string[2]; 
                thisVarAttrs[0] = varnode.NodeName;

                foreach(MaxProperty property in varnode.MaxProperties)
                {
                    if (!(property is VariableProperty)) continue;
                    thisVarAttrs[1] = property.Value.ToString();
                    break;       
                }

                allVarAttrs[i++] = thisVarAttrs;
            }

            return allVarAttrs;
        }


        /// <summary> Searches the event package for the currently visible function 
        /// canvas and returns all valid event parameters for use with the property
        /// grid 'initWith' property.  The 'DisplayName' is returned for each Event
        /// Parameter.  Also, all events found that handle this function are returned
        /// as an out parameter</summary>  
        /// <returns>All event params for the current canvas, or null to indicate a 
        /// function created by a CallFunction, or an event with no params</returns>
        private string[] OnMaxproValidInitWithValuesRequest(out string[] events)
        {
            events = null;
            MaxFunctionCanvas currentCanvas = MaxProject.currentCanvas as MaxFunctionCanvas;
            if(currentCanvas == null) return null; // Probably showing appcanvas
            if(currentCanvas.AppTreeNodes == null) return null;

            ArrayList allEventParams = new ArrayList();
            string[]  eventParams = null;

            // This holds event names already accounted for, to avoid duplicates
            StringCollection handledEventNames = new StringCollection();
       
            // AppTreeNodes will return all events referring to this function
            // which we will iterate through, and accumulate eventParams from
            foreach(MaxAppTreeNodeFunc node in currentCanvas.AppTreeNodes)
            {
                if(node == null) continue;
                if(!(node is MaxAppTreeNodeEVxEH))   continue;
                MaxAppTreeNodeEVxEH treenode = node as MaxAppTreeNodeEVxEH;
       
                Max.Core.Tool.MaxEventTool eventTool  
                    = treenode.CanvasNodeEvent.Tool as Max.Core.Tool.MaxEventTool;

                if (eventTool == null || eventTool.PmEvent == null 
                 || eventTool.PmEvent.Parameters == null) 
                    continue;

                string fullyQualifiedEventName 
                    = eventTool.Package.Name + Const.dot + eventTool.PmEvent.Name;

                if (!handledEventNames.Contains(fullyQualifiedEventName))        
                {
                    handledEventNames.Add(fullyQualifiedEventName);

                    allEventParams.Add(Metreos.Interfaces.ICommands.Fields.ROUTING_GUID);

                    if (eventTool.PmEvent.Type == EventType.asyncCallback)
                        allEventParams.Add(Metreos.Interfaces.ICommands.Fields.USER_DATA);

                    for(int i = 0; i < eventTool.PmEvent.Parameters.Length; i++)
                        allEventParams.Add(eventTool.PmEvent.Parameters[i].DisplayName);
                }           
            }

            if (allEventParams.Count > 0)
            {
                eventParams = new string[allEventParams.Count];
                allEventParams.CopyTo(eventParams);
            }

            if (handledEventNames.Count > 0)
            {
                events = new string[handledEventNames.Count];
                handledEventNames.CopyTo(events, 0);
            }

            return eventParams;
        }


        /// <summary>Extracts and returns config values from app installer file</summary>
        private string[] OnMaxProGetConfigParameters()
        {
            string[] installersRel = MaxMainUtil.PeekProjectFileFiles
                (MaxProject.ProjectPath, Const.xmlValFileSubtypeInstall);

            // Only one installer supported currently.
            if (installersRel == null || installersRel.Length < 1) return null;
            string[] installers = Utl.MakeAbsolute(installersRel, MaxMain.ProjectFolder);      
            string   installer  = installers[0];
            if(installer == null || !System.IO.File.Exists(installer)) return null;
            string[] configValues = null;

            try
            {
                using(System.IO.FileStream stream = System.IO.File.Open(installer, System.IO.FileMode.Open))
                {
                    System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(stream);

                    if (installerDeserializer.CanDeserialize(reader))
                    {
                        installType installerData = installerDeserializer.Deserialize(reader) as installType;
            
                        if (installerData.configuration != null)  
                        {
                            ArrayList configParametersGrowable = new ArrayList();

                            foreach(configurationType configuration in installerData.configuration)
                                    ValidConfigValues(configuration, configParametersGrowable);

                            if (configParametersGrowable.Count != 0)  
                            {
                                configValues = new string[configParametersGrowable.Count];
                                configParametersGrowable.CopyTo(configValues);
                            }
                        }    // if (installerData.configuration 
                    }      // if (CanDeserialize
                }        // using
            }          // try
            catch
            { 
                return new string[] { Const.InvalidInstallerDropDown };
            }

            return configValues;
        }


        /// <summary>Extracts and returns localizable strings values from app locales file</summary>
        private string[] OnMaxProGetLocaleParameters()
        {
            string[] localesRel = MaxMainUtil.PeekProjectFileFiles
                (MaxProject.ProjectPath, Const.xmlValFileSubtypeLocales);

            // Only one locales supported currently.
            if (localesRel == null || localesRel.Length < 1) return null;
            string[] locales = Utl.MakeAbsolute(localesRel, MaxMain.ProjectFolder);
            string localePath = locales[0];
            if (localePath == null || !System.IO.File.Exists(localePath)) return null;
            string[] localeValues = null;

            try
            {
                using (System.IO.FileStream stream = System.IO.File.Open(localePath, System.IO.FileMode.Open))
                {
                    System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(stream);

                    if (localesDeserializer.CanDeserialize(reader))
                    {
                        LocaleTableType localeData = localesDeserializer.Deserialize(reader) as LocaleTableType;

                        if (localeData.Prompts != null && localeData.Prompts.Prompt != null)
                        {
                            List<string> localeParameters = new List<string>();

                            foreach (Prompt prompt in localeData.Prompts.Prompt)
                                ValidLocaleValues(prompt, localeParameters);

                            if (localeParameters.Count != 0)
                            {
                                localeValues = new string[localeParameters.Count];
                                localeParameters.CopyTo(localeValues);
                            }
                        }    // if (localeData.Prompts.Prompt
                    }      // if (CanDeserialize
                }        // using
            }          // try
            catch
            {
                return new string[] { Const.InvalidLocalesDropDown };
            }

            return localeValues;
        }

    
        /// <summary>Gets project properties, extracts the using properties</summary>
        /// <returns>A list of all currently configured usings</returns>
        private string[] OnMaxProGetUsings()
        {
            PropertyDescriptorCollection projectProps = MaxProject.Instance.MaxProperties; 
            return this.ExtractUsingStatements(projectProps);
        }


        /// <summary>Gets project properties, updates the using properties</summary>
        /// <param name="updateUsings">The total amount of new usings</param>
        private void OnMaxProUpdateUsings(string[] updateUsings)
        {
            PropertyDescriptorCollection projectProps = MaxProject.Instance.MaxProperties; 
            this.UpdateUsingStatements(projectProps, updateUsings);
        }



        private MaxPmAction OnMaxGetAction(string packagename, string actionname)
        {
            MaxActionTool actionTool = MaxPackages.Instance.FindByToolName
                (packagename + Const.dot + actionname, DataTypes.Type.Action) as MaxActionTool;

            return actionTool == null?  null: actionTool.PmAction;
        }

    
        /// <summary> Gets valid configValues from configuration </summary>
        private void ValidConfigValues(configurationType configuration, ArrayList configValuesGrowable)
        {
            if (configuration.configValue == null) return;

            foreach(configValueType configValue in configuration.configValue)       
                 if(configValue.name != null && configValue.name.Length > 0)   
                    configValuesGrowable.Add(configValue.name);
        }


        /// <summary> Gets valid configValues from configuration </summary>
        private void ValidLocaleValues(Prompt prompt, List<string> list)
        {
            if (prompt == null || prompt.name == null || prompt.name == String.Empty) return;

            list.Add(prompt.name);
        }


        /// <summary>Returns all using statements in project PropertyDescriptorCollection</summary>
        private string[] ExtractUsingStatements(PropertyDescriptorCollection projectProperties)
        {
            GenericProperty parentUsing = projectProperties[DataTypes.USINGS] as GenericProperty;

            if(parentUsing == null)                       return null;
            if(parentUsing.ChildrenProperties == null)    return null;
            if(parentUsing.ChildrenProperties.Count == 0) return null;

            string[] usings = new string[parentUsing.ChildrenProperties.Count];

            for(int i = 0; i < parentUsing.ChildrenProperties.Count; i++)
                usings[i] = (parentUsing.ChildrenProperties[i] as MaxProperty).Value.ToString();

            return usings;
        }


        /// <summary> Return all referenced media files </summary>
        private string[] OnMaxproGetMediaFiles()
        {
            return MaxproGetMediaFiles(MediaFileType.audio);
        }


        /// <summary> Return all referenced media files </summary>
        private string[] OnMaxproGetGrammarFiles()
        {
            return MaxproGetMediaFiles(MediaFileType.grammar);
        }


        /// <summary> Return all referenced media files </summary>
        private string[] OnMaxproGetVoiceRecFiles()
        {
            return MaxproGetMediaFiles(MediaFileType.voicerec);
        }


        /// <summary> Return all referenced media files </summary>
        private string[] MaxproGetMediaFiles(MediaFileType ft)
        {
            MaxFolderTreeNode treenode = null; 
            string path = null;

            switch(ft)
            {   case MediaFileType.audio:    treenode = main.Explorer.MediaFolderNode;  break;
                case MediaFileType.grammar:
                case MediaFileType.voicerec: treenode = main.Explorer.VrResxFolderNode; break;
            }

            if (treenode == null) return null;

            ArrayList filesList = new ArrayList();

            if (ft == MediaFileType.audio)
            {
                // Audio files are one-level deeper, and so have to be treated different
                // then grammar & voicerec files
                foreach (MaxLocaleAudioFolder localeAudioNode in treenode.Nodes)
                {
                    foreach (MaxMediaTreeNode node in localeAudioNode.Nodes)
                    {
                        path = node.Text;
                        if (path != null && path != String.Empty &&
                            !filesList.Contains(path))
                        {
                            if (Utl.IsAudioFile(path)) filesList.Add(path);
                        }
                    }
                }
            }
            else
            {
                foreach (MaxMediaTreeNode node in treenode.Nodes)
                {
                    path = null;

                    switch (ft)
                    {
                        case MediaFileType.grammar:
                            if (node is MaxVrResxTreeNode) path = (node as MaxVrResxTreeNode).Text;
                            if (Utl.IsGrammarFile(path)) filesList.Add(path);
                            break;

                        case MediaFileType.voicerec:
                            if (node is MaxVrResxTreeNode) path = (node as MaxVrResxTreeNode).Text;
                            if (Utl.IsVoiceRecFile(path)) filesList.Add(path);
                            break;
                    }
                }
            }

            if (filesList.Count == 0) return null;

            string[] mediaFileNames = new string[filesList.Count];
            filesList.CopyTo(mediaFileNames);

            return mediaFileNames;
        }


        private DataTypes.UserVariableType OnMaxproGetDefaultUserTypes()
        {
            return Config.DefPropertyType;
        }

  
        private void UpdateUsingStatements
            (PropertyDescriptorCollection projectProperties, string[] updateUsings)
        {
            GenericProperty parentUsing = projectProperties[DataTypes.USINGS] as GenericProperty;

            if (parentUsing == null) return;
            if (parentUsing.ChildrenProperties == null)
                parentUsing.ChildrenProperties = new PropertyDescriptorCollection(null);
      
            parentUsing.ChildrenProperties.Clear();

            if (updateUsings == null) return;

            foreach(string newUsing in updateUsings)
                parentUsing.ChildrenProperties.Add(
                    new GenericProperty(DataTypes.USING, newUsing, false, 
                        parentUsing.Mpm, null, projectProperties));
        }


        private EventParameter OnMaxProGetEventParamByDisplay(string fullyQualEventName, string eventParamDisplayName)
        {
            MaxEventTool tool = MaxPackages.Instance.FindEventByToolName(fullyQualEventName);

            if (tool == null || tool.PmEvent == null || tool.PmEvent.Parameters == null) return null;
          
            foreach(EventParameter eParam in tool.PmEvent.Parameters)
                if (eParam.DisplayName == eventParamDisplayName)
                    return eParam;

            // Happens with parameters such as RoutingGuid and UserData, which are auto-generated by Max
            return null;
        }


        private EventParameter OnMaxProGetEventParamByName(string fullyQualEventName, string eventParamName)
        {
            MaxEventTool tool = MaxPackages.Instance.FindEventByToolName(fullyQualEventName);

            if (tool == null || tool.PmEvent == null || tool.PmEvent.Parameters == null) return null;
        
            foreach(EventParameter eParam in tool.PmEvent.Parameters)
                 if(String.Compare(eParam.Name, eventParamName, true) == 0)
                    return eParam;

            // Happens with parameters such as RoutingGuid and UserData, which are auto-generated by Max
            return null;
        }

        /// <summary>Gets the main property window combo box</summary>
        public System.Windows.Forms.ComboBox Combo { get { return combo; } }


        /// <summary>Clears the grid and combobox and refreshes property window</summary>
        public void Clear(Object caller)
        {
            this.combo.Items.Clear();
            this.Combo.Items.Insert(0, String.Empty);
            this.combo.SelectedIndex = 0;

            this.grid.SelectedObject = null;
  
            this.grid.Show();

            if  (caller is Control)               // Restore focus
                ((Control)caller).Focus();
        }


        protected override void Dispose(bool disposing)
        {
            if  (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
    
        private void InitializeComponent()
        {
            this.grid = new MaxPropertyGrid(); 
            this.combo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.CommandsVisibleIfAvailable = true;
            this.grid.LargeButtons = false;
            this.grid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.grid.Location = new System.Drawing.Point(0, 21);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(292, 248);
            this.grid.TabIndex = 0;
            this.grid.Text = "grid";
            this.grid.ViewBackColor = System.Drawing.SystemColors.Window;
            this.grid.ViewForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // combo
            // 
            this.combo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.combo.Location = new System.Drawing.Point(0, 0);
            this.combo.Name = "combo";
            this.combo.Size = new System.Drawing.Size(292, 21);
            this.combo.TabIndex = 1;
            // 
            // MaxPropertyWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.combo);
            this.Controls.Add(this.grid);
            this.Name = "MaxPropertyWindow";
            this.Text = "Properties";
            this.ResumeLayout(false);
        }
        #endregion

        #region MaxSatelliteWindow Members

        public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
        {
            get { return SatelliteTypes.Properties; }
        }

        public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
        { 
            get { return MaxMenu.menuViewProperties; } 
        }

        #endregion  

        #region IMaxPropManCallbacks Members
        public event MaxPropertiesManager.VoidDelegate ForcePropertyUpdate;
        public MaxPropertiesManager.GetFunctionVars GetFunctionVars                 { get { return maxproFunctionVarsRequest; } }
        public MaxPropertiesManager.GetGlobalVars GetGlobalVars                     { get { return maxproGlobalVarsRequest; } }
        public MaxPropertiesManager.GetChangedProperties GetChangedProperties       { get { return maxproCallback; } }
        public MaxPropertiesManager.GetAllNativeTypes GetAllNativeTypes             { get { return maxproNativeTypesRequest; } }
        public MaxPropertiesManager.GetNativeTypesInfo GetNativeTypesInfo           { get { return maxProNativeTypesFullRequest; } }
        public MaxPropertiesManager.GetValidInitWithValues GetValidInitWithValues   { get { return maxproGetInitwithValuesRequest; } }
        public MaxPropertiesManager.GetInstallerConfigParameters GetConfigParameters{ get { return maxproGetConfigParameters; } }
        public MaxPropertiesManager.GetLocalizableStrings GetLocaleStrings          { get { return maxproGetLocaleStrings; } }
        public MaxPropertiesManager.GetUsingStatements       GetUsings              { get { return maxproGetUsings; } }
        public MaxPropertiesManager.GetMediaFilesDelegate    GetMediaFiles          { get { return maxproGetMediaFiles;   } }
        public MaxPropertiesManager.UpdateUsingStatements    UpdateUsings           { get { return maxProUpdateUsings;    } }
        public MaxPropertiesManager.GetGrammarFilesDelegate  GetGrammarFiles        { get { return maxproGetGrammarFiles; } }
        public MaxPropertiesManager.GetVoiceRecFilesDelegate GetVoiceRecFiles       { get { return maxproGetVoiceRecFiles;} }

        public MaxPropertiesManager.GetPackagedAction  GetAction                    { get { return maxproGetAction; } }
        public MaxPropertiesManager.GetDefaultUserType GetDefaultUserType           { get { return maxproGetDefaultUserType; } }
        public MaxPropertiesManager.GetEventParameter  GetEventParamByDisplay       { get { return maxproGetEventParamByDisplay; } }
        public MaxPropertiesManager.GetEventParameter  GetEventParamByName          { get { return maxproGetEventParamByName; } }
        #endregion

    } // class MaxPropertyWindow




    /// <summary>This gives us a global handle to properties manager. Properties manager
    /// should be a singleton, but while it is a dll, we'll use this to access it</summary>
    public class PmProxy
    {
        #region singleton
        private static readonly PmProxy instance = new PmProxy();
        public  static PmProxy Instance {get { return instance; }}
        private PmProxy(){} 
        #endregion
        private static MaxPropertyWindow    propertyWindow    = MaxPropertyWindow.Instance;
        private static MaxPropertiesManager propertiesManager = propertyWindow.PropertiesManager;
        public  static MaxPropertyWindow    PropertyWindow    { get { return propertyWindow;    } }
        public  static MaxPropertiesManager PropertiesManager { get { return propertiesManager; } }
        public  static MaxSelectableObject  PropertiesSubject;

        public  static void ShowProperties
        ( MaxSelectableObject x, Framework.Satellite.Property.DataTypes.Type type)
        {
            PropertiesSubject = x;
            propertyWindow.Combo.Items.Clear(); // for now
            propertyWindow.Combo.Items.Insert(0, x.ObjectDisplayName);
            propertyWindow.Combo.SelectedIndex = 0;
            
            PropertiesManager.Show(x.MaxProperties, type);
        }


        /// <summary>Ensure property window visible</summary>
        public static void ShowPropertiesWindow(bool focus)
        {
            MaxMain.DockMgr.ShowContent(MaxMain.PropertyWindow);
            MaxMain.DockMgr.BringAutoHideIntoView(MaxMain.PropertyWindow);  
            if (focus) MaxPropertyWindow.Instance.Grid.Focus();
        }
    } // class PmProxy

} // namespace



