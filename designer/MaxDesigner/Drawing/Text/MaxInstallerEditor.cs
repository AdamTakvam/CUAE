using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Resources.Images;


 
namespace Metreos.Max.Drawing
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxInstallerEditorTab 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A datagrid editor canvas to be hosted in a tab frame</summary>
    public class MaxInstallerEditorTab: MaxTabContent, IMaxViewType 
    {
        // Construction

        public MaxInstallerEditorTab(string name, string path, bool isnew): base(name) 
        {
            this.canvasType = CanvasTypes.Text;

            this.filename = name;   
            
            this.ed = new MaxInstallerEditor(this);
            this.ed.ContextMenu = new ContextMenu( new MenuItem[] { add, remove, manageConfig } );
            
            this.addHandler = new EventHandler(AddClick);
            add.Click += addHandler;

            this.removeHandler = new EventHandler(RequestRemoveItems);
            this.ed.ContextMenu.Popup += removeHandler;

            this.manageHandler = new EventHandler(ManageConfigClick);
            manageConfig.Click += manageHandler;

            this.filepath = path;
            this.Init();  
            // pmObjectType = Framework.Satellite.Property.DataTypes.Type.Installer;

            if (!isnew) return;

            ed.Save(this.filepath);           
        }

        public void Init()
        {                                                       
            this.Controls.Add(ed); 
      
            RaiseFrameworkActivity += OutboundHandlers.FrameworkActivityCallback;      
            RaiseCanvasActivity    += OutboundHandlers.CanvasActivityProxy;

            // this.CreateProperties(null);
        }

        // Properties

        protected MaxInstallerEditor ed; 
        protected EventHandler addHandler;
        protected EventHandler removeHandler;
        protected EventHandler manageHandler;
        protected Crownwood.Magic.Controls.TabPage tab; 
        protected string filename;     
        public    string FileName   { get { return filename; } } 
        protected string filepath;     
        public    string FilePath   { get { return filepath; } } 
        protected int    saveCount;
        public    int    SaveCount  { get { return saveCount;} } 
        protected bool   isNewFile;
        public    bool   IsNew      { get { return isNewFile; } set { isNewFile = value; } }
        public    const  string CurrentItem = "-Current Item-";

        public int ImageIndex()
        {
            return MaxImageIndex.framework16x16IndexInstaller;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsAddType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.AddInstaller;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsOpenType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.OpenInstaller;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsCloseType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.CloseInstaller;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsRenameType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.RenameInstaller;
        }
        // Events

        public event GlobalEvents.MaxFrameworkActivityHandler RaiseFrameworkActivity;
        public event GlobalEvents.MaxCanvasActivityHandler    RaiseCanvasActivity;

        // Right-click menu items
        public static MenuItem add = new MenuItem("Add");
        public static MenuItem remove = new MenuItem("Remove");
        public static MenuItem manageConfig = new MenuItem("Manage Custom Config Items");

        // Commands
  
        public bool New(string path, bool exists)
        {
            this.tab = MaxManager.Instance.AddTab(filename, this, this.ImageIndex());
            MaxProject.View.CurrentTab = tab;
            this.filepath = path;
 
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsAddType();
       
            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType, filename, filepath);
            args.Active = true;

            if (exists)                            
            {                                     
                ed.NoNotify = true;               // Adding existing file to project

                ed.Open(path);
      
                ed.NoNotify = false;
            }
            else 
            {
                ed.Save(path);
        
                // Load a default configuration template into the installer editor
                ed.InitializeConfig();
            }
            // We register with the file system initially, but this does not count  
            // as a user save when it comes time to determine if file was abandoned. 
      
            SignalProjectActivity(this, args); 
 
            this.isNewFile = true;
            return true;
        }


        public bool Open(string name, string path)   
        {                               
            this.filepath = path;
            this.filename = name;
            RaiseFrameworkActivity(this, MaxProject.SuspendLayoutEventArgs);

            Crownwood.Magic.Controls.TabPage tab 
                = MaxManager.Instance.AddTab(name, this, this.ImageIndex());
            MaxProject.View.CurrentTab = tab;
      
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsOpenType();
           
            MaxProjectEventArgs eventArgs = new MaxProjectEventArgs(eventType, name, path);

            ed.NoNotify = true;                   // Suppress text change event

            bool result = ed.Open(path);  

            ed.NoNotify = false;    
           
            eventArgs.Result = result? 
                MaxProjectEventArgs.MaxResults.OK: MaxProjectEventArgs.MaxResults.Error;
          
            SignalProjectActivity(this, eventArgs);

            RaiseFrameworkActivity(this, MaxProject.ResumeLayoutEventArgs);

            SignalProjectActivity (this, MaxProject.ViewNotDirtyEventArgs);

            this.Dirty = false;
            return result;
        }


        public bool CloseEditor(bool notify)
        {
            // remove handlers, so that events don't duplicate
            add.Click -= addHandler;
            remove.Click -= removeHandler;
            manageConfig.Click -= manageHandler;

            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsCloseType();      

            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType, this.filename);

            args.Result = this.CanClose()?   
                MaxProjectEventArgs.MaxResults.OK: MaxProjectEventArgs.MaxResults.Error;         

            if (notify) SignalProjectActivity(this, args);

            return args.Result == MaxProjectEventArgs.MaxResults.OK;
        }


        public bool SaveAs(string path)
        {
            bool result = ed.Save(path);

            if (result)
            {   
                SignalProjectActivity(this, MaxProject.ViewNotDirtyEventArgs);
                this.saveCount++;
            }

            return result;
        } 


        public bool Save()
        {
            return this.SaveAs(filepath);    
        } 


        /// <summary>Rename the object and its file</summary>
        public bool Rename(string oldname, string newname)
        {
            bool   result  = false;               // First rename source file
            string oldpath = filepath; 
            string ext     = Path.GetExtension(filepath);   
            string folder  = Utl.StripPathFilespec(oldpath);
            string newpath = folder + Const.bslash + newname + ext;
            try  { File.Move(oldpath, newpath); result = true; } 
            catch{ }
            if   (!result) return false; 

            this.filename = newname;
            this.filepath = newpath; 
            // Notify framework
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsRenameType();                                                     
            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType);
            args.NewName = newname;
            args.OldName = oldname;
            args.ScriptPath = newpath;

            SignalProjectActivity(this, args);
     
            return true;
        } 


        // Support methods

        /// <summary>If dirty, prompt to save and save if confirmed</summary>
        public bool CanClose()
        {
            if (this.Dirty)
                switch(Utl.PromptSaveChangesTo(this.filename, Const.FileSaveDlgTitle))
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Yes:    this.Save(); break;
                }
       
            return true;
        }


        /// <summary>Initailize editor in preparation for opening file</summary>
        public bool OnOpening(string path)
        {
            this.filepath = path;
            // this.CreateProperties(null);           
            return true;
        }

        private void AddClick(object sender, EventArgs e)
        {
            ed.AddItem();
            if (!this.Dirty && !ed.NoNotify) 
                this.SignalDirty();
        }

        private void RemoveClick(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            ed.RemoveItem(menuItem.Text);
            if (!this.Dirty && !ed.NoNotify) 
                this.SignalDirty();
        }

        private void RequestRemoveItems(object sender, EventArgs e)
        {
            remove.MenuItems.Clear();
            string[] configItemNames = ed.GetConfigNames();

            if(configItemNames == null || configItemNames.Length == 0)
            {
                remove.Enabled = false;
            }
            else
            {
                remove.Enabled = true;
                MenuItem currentMenuItem = new MenuItem(CurrentItem, new EventHandler(RemoveClick));
                remove.MenuItems.Add(currentMenuItem);
                foreach(string configName in configItemNames)
                {
                    remove.MenuItems.Add(configName, new EventHandler(RemoveClick));
                }
            }
        }

        /// <summary>
        /// Launch custom type editor
        /// </summary>
        private void ManageConfigClick(object sender, EventArgs e)
        {
            CustomConfigManager manager = new CustomConfigManager(MaxProject.Instance.CustomConfigItems);
            DialogResult result = manager.ShowDialog();

            if(result == DialogResult.OK)
            {
                MaxProject.Instance.CustomConfigItems = manager.ConfigItems;
            }
            manager.Dispose();
        }
    } // class MaxInstallerEditorTab 


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxInstallerEditor 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>
    ///     Overrides the .NET Datagrid, so that it is well behaved with the overriden
    /// </summary>
    public class MaxInstallerEditor : DataGrid
    {
        private static XmlSerializer seri = new XmlSerializer(typeof(Metreos.AppArchiveCore.Xml.installType));
        private MaxTabContent host;
        private bool noNotify;
        public  bool NoNotify { set { noNotify = value; } get { return noNotify; } }

        public MaxInstallerEditor(MaxTabContent tab)
        {
            this.host = tab;
            this.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Dock = DockStyle.Fill;
            this.BackgroundColor = Color.White;
        }

        #region Max Editor Save/Open/Load

        /// <summary>Open file and load into editor</summary>
        public bool Open(string path)
        {
            bool result = false;
            try   { result = this.Load(path);}
            catch { }
      
            if (!noNotify) host.SignalNotDirty();
            return result;
        }

        /// <summary>Save editor file</summary>
        public bool Save(string path)
        {
            bool result = false;
            try   { result = this.SaveFile(path); }
            catch { }

            host.SignalNotDirty();
            return result;
        }

        public bool SaveFile(string path)
        {
            bool success = false;
            ArrayList configItems = this.DataSource as ArrayList;

            // LRM: Fix for MAX-142, MAX-174.
            this.Select(this.CurrentRowIndex);
            this.UnSelect(this.CurrentRowIndex);
            
            TextWriter writer = null;
            try
            {
                ArrayList innerConfigItems = new ArrayList();
                if(configItems != null)
                {
                    foreach(ConfigWrapper config in configItems)
                    {
                        if(config.Config != null)
                        {
                            innerConfigItems.Add(config.Config);
                        }
                    }
                }

                Metreos.AppArchiveCore.Xml.configValueType[] configValues = null;
                if(innerConfigItems.Count > 0)
                {
                    configValues = new Metreos.AppArchiveCore.Xml.configValueType[innerConfigItems.Count];
                    innerConfigItems.CopyTo(configValues);
                }

                Metreos.AppArchiveCore.Xml.installType installer = CreateDefaultInstaller();
                installer.configuration[0].configValue = configValues;
                
                writer = new StreamWriter(path);
                seri.Serialize(writer, installer);

                success = true;
            }
            catch{}                
            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }
            }

            return success;
        }

        #endregion

        #region Add/Remove/Request Items

        public void AddItem()
        {
            ArrayList configs = this.DataSource as ArrayList;

            ConfigWrapper newConfig = new ConfigWrapper();
            newConfig.Name = MakeUniqueConfigName();
            configs.Add(newConfig);

            CurrencyManager manager = (CurrencyManager) this.BindingContext[configs];
            if (manager != null)
                manager.Refresh();
        }

        public void RemoveItem(string itemToRemove)
        {
            if(itemToRemove == MaxInstallerEditorTab.CurrentItem)
            {
                ArrayList configs = this.DataSource as ArrayList;
                if(configs == null) return;

                CurrencyManager manager = (CurrencyManager) this.BindingContext[configs];

                if(manager.Count <= 1) return;

                int removePosition = manager.Position;
                if(removePosition > 0)
                {
                    manager.Position = removePosition - 1;
                }

                configs.RemoveAt(removePosition);

                if (manager != null)
                    manager.Refresh();
            }
            else
            {
                ArrayList configs = this.DataSource as ArrayList;
                if(configs == null) return;

                CurrencyManager manager = (CurrencyManager) this.BindingContext[configs];

                if(manager.Count <= 1) return;

                bool foundConfigItem = false;;
                int location = 0;
                foreach(ConfigWrapper config in configs)
                {
                    if(config.Config.name == itemToRemove || (config.Config.name == null && itemToRemove == String.Empty))
                    {
                        foundConfigItem = true;
                        break;
                    }
                    location++;
                }

                if(foundConfigItem)
                {
                    configs.RemoveAt(location);

                    if (manager != null)
                        manager.Refresh();
                }
            }
        }

        public string[] GetConfigNames()
        {
            ArrayList configs = this.DataSource as ArrayList;
            ArrayList configNames = new ArrayList();
            foreach(ConfigWrapper config in configs)
            {
                configNames.Add(config.Config.name);
            }
            string[] configNamesArray = new string[configNames.Count];
            configNames.CopyTo(configNamesArray);
            return configNamesArray;
        }

        protected string MakeUniqueConfigName()
        {
            return MakeUniqueConfigName(-1);
        }

        protected string MakeUniqueConfigName(int rowNum)
        {
            string name = null;
            while(true)
            {       
                name = MakeNewConfigName();
                if  (this.CanNameConfig(name, rowNum)) break;
            }

            return name;
        }


        public static string MakeNewConfigName()
        {
            return Const.defaultConfigName + MaxProject.Instance.ConfigSequence; 
        }

        public bool CanNameConfig(string name, int currentRow)
        {
            if(name == null || name == String.Empty) return false;
            
            ArrayList configs = this.DataSource as ArrayList;
            if(configs == null) return true;
            int i = 0;

            foreach(ConfigWrapper config in configs)
            {
                if(i++ == currentRow)   continue; // do not validate against self

                if(config.Name == name)
                {
                    return false;
                }
            }

            return true;
        }
        
        public bool IsGreaterThanMax(string name, int currentRow)
        {
            if(name == null || name == String.Empty)    return false;

            bool isGreater = false;
            int testValue;

            try
            {
                testValue = int.Parse(name);

                ArrayList configs = this.DataSource as ArrayList;
                if(configs != null) 
                {
                    int i = 0;
                    foreach(ConfigWrapper config in configs)
                    {
                        if(i++ == currentRow)   
                        {
                            if(!config.MaxValue.IsNill)
                            {
                                isGreater = config.MaxValue.Value < testValue;
                            }

                            continue;
                        }
                    }
                }
            }
            catch{}

            return isGreater;
        }

        public bool IsLessThanMin(string name, int currentRow)
        {
            if(name == null || name == String.Empty)    return false;

            bool isLess = false;
            int testValue;

            try
            {
                testValue = int.Parse(name);

                ArrayList configs = this.DataSource as ArrayList;
                if(configs != null) 
                {
                    int i = 0;
                    foreach(ConfigWrapper config in configs)
                    {
                        if(i++ == currentRow)   
                        {
                            if(!config.MinValue.IsNill)
                            {
                                isLess = config.MinValue.Value > testValue;
                            }

                            continue;
                        }
                    }
                }
            }
            catch{}

            return isLess;
        }
        #endregion
    
        #region DataGrid Overrides

        protected override void ColumnStartedEditing(Control editingControl)
        {
            if (!host.Dirty && !noNotify) 
                host.SignalDirty();

            base.ColumnStartedEditing (editingControl);
        }

        protected override void ColumnStartedEditing(Rectangle bounds)
        {
            if (!host.Dirty && !noNotify) 
                host.SignalDirty();

            base.ColumnStartedEditing (bounds);
        }
    
    
        protected override void OnResize(EventArgs e)
        {
            if(this.TableStyles.Count > 0 && this.TableStyles[0].GridColumnStyles.Count > 0)
            {
                DataGridTableStyle style = this.TableStyles[0];
      
                int totalWidth = TotalWidth(style.GridColumnStyles);
                DataGridColumnStyle lastColumn = style.GridColumnStyles[style.GridColumnStyles.Count - 1] as DataGridColumnStyle; 

                int extraWidth = this.host.Size.Width - totalWidth - 40;

                if(extraWidth > 0)
                    lastColumn.Width = lastColumn.Width + extraWidth;
                else if (extraWidth < 0)
                {
                    if(lastColumn.Width - -extraWidth > 100)
                    {
                        lastColumn.Width = lastColumn.Width - -extraWidth;
                    }
                    else
                    {
                        lastColumn.Width = 100;
                    }
                }
            }

            base.OnResize (e);
        }

        private int TotalWidth(GridColumnStylesCollection columns)
        {
            int width = 0;
            foreach(DataGridColumnStyle column in columns)
            {
                width += column.Width;
            }
            return width;
        }

        #endregion

        #region X/Y Helpers
        //    public int ColumnHeight()
        //    {
        //      int totalHeight = 0;
        //      ArrayList configs = this.DataSource as ArrayList;
        //      if(configs != null && this.TableStyles.Count > 0 && this.TableStyles[0].GridColumnStyles.Count > 0)
        //      {
        //        DataGridTableStyle style = this.TableStyles[0];
        //        StringColumn lastColumn = style.GridColumnStyles[style.GridColumnStyles.Count - 1] as SimpleTextColum; 
        //      
        //        totalHeight = TotalHeight(configs.Count, lastColumn.Height);
        //      }
        //      return totalHeight; 
        //    }
        //
        //    private int TotalHeight(int configCount, int configHeight)
        //    {
        //      int height = 44;
        //      height += (configHeight - 2) * configCount;
        //      return height;
        //    }

        #endregion

        #region DataGrid Load

        public static string CreateDefaultInstallerText()
        {
            Metreos.AppArchiveCore.Xml.installType installer = CreateDefaultInstaller();
            System.Text.StringBuilder installerText = new System.Text.StringBuilder();
            StringWriter writer = new StringWriter(installerText);
            seri.Serialize(writer, installer);
            return installerText.ToString();
        }

        private static Metreos.AppArchiveCore.Xml.installType CreateDefaultInstaller()
        {
            Metreos.AppArchiveCore.Xml.installType installer = new Metreos.AppArchiveCore.Xml.installType();
            // Only one configuration section supported
            installer.configuration = new Metreos.AppArchiveCore.Xml.configurationType[1];
            installer.configuration[0] = new Metreos.AppArchiveCore.Xml.configurationType();
            return installer;
        }

        public bool InitializeConfig()
        {
            this.DataSource = LoadNewConfig();
            this.TableStyles.Clear();
            this.TableStyles.Add(StyleConfigurationItems());

            return true;
        }
      
        private bool Load(string filePath)
        {
            this.DataSource = LoadConfigFromFile(filePath);
            this.TableStyles.Clear();
            this.TableStyles.Add(StyleConfigurationItems());
            //this.TableStyles.Add(StyleEnumItems());

            return true;
        }
      
        private ArrayList LoadNewConfig()
        {   
            ConfigWrapper oneItem = new ConfigWrapper();
            oneItem.Name = MakeUniqueConfigName();
            return new ArrayList(new ConfigWrapper[] { oneItem });
        }

        private ArrayList LoadConfigFromFile(string filePath)
        {
            ArrayList wrappedConfigs = new ArrayList();
            FileStream stream = null;
            try
            {
                stream = new FileStream(filePath, FileMode.Open);
                Metreos.AppArchiveCore.Xml.installType type = seri.Deserialize(stream) as Metreos.AppArchiveCore.Xml.installType;
            
                if(type.configuration != null && type.configuration.Length > 0)
                {
                    // Multiple configuration sections were at one time supported,
                    // but they have never been used and are hereby not supported
                    // via MAX
                    if(type.configuration[0].configValue != null)
                    {
                        foreach(Metreos.AppArchiveCore.Xml.configValueType config in type.configuration[0].configValue)
                        {
                            // Config can't be null due to serialization
                            wrappedConfigs.Add(new ConfigWrapper(config));
                        }
                    }
                }
            }
            catch{}
            finally
            {
                if(stream != null)
                {
                    stream.Close();
                }
            }
          
            return wrappedConfigs;
        }

        private void PostLoad()
        {
            // Sync up remove context menu with current rows

        }

        private DataGridTableStyle StyleConfigurationItems()
        {
            DataGridTableStyle style = new DataGridTableStyle();
            style.MappingName = "ArrayList";
            style.AlternatingBackColor = Color.FromArgb(220, 215, 195);
            
            StringColumn nameColumn = new StringColumn(this);
            nameColumn.MappingName = ConfigWrapper.NameMapping;
            nameColumn.HeaderText = ConfigWrapper.NameHeader;
            style.GridColumnStyles.Add(nameColumn);

            StringColumn displayNameColumn = new StringColumn(this);
            displayNameColumn.MappingName = ConfigWrapper.DisplayNameMapping;
            displayNameColumn.HeaderText = ConfigWrapper.DisplayNameHeader;
            style.GridColumnStyles.Add(displayNameColumn);

            StringColumn defaultValueColumn = new StringColumn(this);
            defaultValueColumn.MappingName = ConfigWrapper.DefaultValueMapping;
            defaultValueColumn.HeaderText = ConfigWrapper.DefaultValueHeader;
            style.GridColumnStyles.Add(defaultValueColumn);
           
            StringColumn descriptionColumn = new StringColumn(this);
            descriptionColumn.MappingName = ConfigWrapper.DescriptionMapping;
            descriptionColumn.HeaderText = ConfigWrapper.DescriptionHeader;
            style.GridColumnStyles.Add(descriptionColumn);
           
            FormatColumn formatColumn = new FormatColumn(this);
            formatColumn.MappingName = ConfigWrapper.FormatMapping;
            formatColumn.HeaderText = ConfigWrapper.FormatHeader;
            style.GridColumnStyles.Add(formatColumn);
           
            IntColumn maxValueColumn = new IntColumn(this);
            maxValueColumn.MappingName = ConfigWrapper.MaxValueMapping;
            maxValueColumn.HeaderText = ConfigWrapper.MaxValueHeader;
            style.GridColumnStyles.Add(maxValueColumn);

            IntColumn minValueColumn = new IntColumn(this);
            minValueColumn.MappingName = ConfigWrapper.MinValueMapping;
            minValueColumn.HeaderText = ConfigWrapper.MinValueHeader;
            style.GridColumnStyles.Add(minValueColumn);

            DataGridBoolColumn requiredColumn = new DataGridBoolColumn();
            requiredColumn.AllowNull = false;
            requiredColumn.TrueValue = true;
            requiredColumn.FalseValue = false;
            requiredColumn.NullValue = false;
            requiredColumn.MappingName = ConfigWrapper.RequiredMapping;
            requiredColumn.HeaderText = ConfigWrapper.RequiredHeader;
            style.GridColumnStyles.Add(requiredColumn);

            DataGridBoolColumn readOnlyColumn = new DataGridBoolColumn();
            readOnlyColumn.AllowNull = false;
            readOnlyColumn.TrueValue = true;
            readOnlyColumn.FalseValue = false;
            readOnlyColumn.NullValue = false;
            readOnlyColumn.MappingName = ConfigWrapper.ReadOnlyMapping;
            readOnlyColumn.HeaderText = ConfigWrapper.ReadOnlyHeader;
            style.GridColumnStyles.Add(readOnlyColumn);

            return style;
        }
        #endregion

        #region Configuration Wrapper

        /// <summary>
        ///     Configuration Wrapper is necessary because the datagrid will only display Properties.
        /// </summary>
        public class ConfigWrapper 
        {
            public Metreos.AppArchiveCore.Xml.configValueType Config { get { return config; } set { config = value; } }
            private Metreos.AppArchiveCore.Xml.configValueType config;

            public const string NameMapping = "Name";
            public const string NameHeader = "Name";
            public string Name { get { return config.name; } set { config.name = value; } } 
            
            public const string DisplayNameMapping = "DisplayName";
            public const string DisplayNameHeader = "Display Name";
            public string DisplayName { get { return config.displayName; } set { config.displayName = value; } }

            public const string DefaultValueMapping = "DefaultValue";
            public const string DefaultValueHeader = "Default Value";
            public string DefaultValue { get { return config.defaultValue; } set { config.defaultValue = value; } }

            public const string DescriptionMapping = "Description";
            public const string DescriptionHeader = "Description";
            public string Description { get { return config.description; } set { config.description = value; } }

            public const string FormatMapping = "Format";
            public const string FormatHeader = "Format";
            public FormatTypes Format { get { return PrepareFormatTypes(config.format); } set { AddToTypes(ref config.format, value); } }

            public const string ReadOnlyMapping = "ReadOnly";
            public const string ReadOnlyHeader = "Read-only";
            public bool ReadOnly { get { return CreateNillableBoolean(config.readOnlySpecified, config.readOnly); } set { SetBooleanField(ref config.readOnlySpecified, ref config.readOnly, value); } }

            public const string RequiredMapping = "Required";
            public const string RequiredHeader = "Required";
            public bool Required { get { return  CreateNillableBoolean(config.requiredSpecified, config.required); } set { SetBooleanField(ref config.requiredSpecified, ref config.required, value); } }

            public const string MaxValueMapping = "MaxValue";
            public const string MaxValueHeader = "Max Value";
            public NillableInteger MaxValue { get { return CreateNillableInteger(config.maxValueSpecified, config.maxValue); } set { SetIntegerField(ref config.maxValueSpecified, ref config.maxValue, value); } }

            public const string MinValueMapping = "MinValue";
            public const string MinValueHeader = "Min Value";
            public NillableInteger MinValue { get { return CreateNillableInteger(config.minValueSpecified, config.minValue); } set { SetIntegerField(ref config.minValueSpecified, ref config.minValue, value); } }

            private ArrayList enums;

            public ConfigWrapper()
            {  
                this.config = new Metreos.AppArchiveCore.Xml.configValueType();
                this.config.format = Metreos.Interfaces.IConfig.StandardFormat.String.ToString();
                this.enums = new ArrayList();
            }

            public ConfigWrapper(Metreos.AppArchiveCore.Xml.configValueType config)
            {
                this.config = config;

                if(this.config != null && this.config.EnumItem != null)
                {
                    enums = new ArrayList(config.EnumItem);
                }
                else
                {
                    enums = new ArrayList();
                }
            }

            #region Format Utility
            private string ParseFormat(string toParse)
            {
                string defaultValue = 
                    Metreos.Interfaces.IConfig.StandardFormat.String.ToString();

                if(toParse == null || toParse == String.Empty) return defaultValue;

                try
                {
                    // If this parse fails, then it is a user-defined type
                    defaultValue = Enum.Parse(typeof(Metreos.Interfaces.IConfig.StandardFormat), toParse, true).ToString();
                    FormatTypes.AddType(defaultValue);
                }
                catch 
                {
                    // Last ditch efforts for some laxness we have shown in the past with regard to
                    // bool and string
                    if(typeof(bool) == System.Type.GetType("System." + toParse, false, true))
                    {
                        defaultValue = Metreos.Interfaces.IConfig.StandardFormat.Bool.ToString();
                    }
                }

                return defaultValue;
            }
            #endregion

            #region Nillable Utilities
            //      private NillableBoolean CreateNillableBoolean(bool boolSpecified, bool boolField)
            //      {
            //        return NillableBoolean.CreateNonNullBoolean(boolField);
            //      }

            private NillableInteger CreateNillableInteger(bool intSpecified, int intField)
            {
                if(!intSpecified) return NillableInteger.NullInteger;
                else return NillableInteger.CreateNonNullInteger(intField);
            }

            private bool CreateNillableBoolean(bool specified, bool boolField)
            {
                return specified ? boolField : false;
            }

            //      private void SetBooleanField(ref bool serialize, ref bool currentValue, bool newValue)
            //      {
            //        serialize = true;
            //        currentValue = newValue;
            //      }
            //        private void SetBooleanField(ref bool serialize, ref bool value, NillableBoolean fieldValue)
            //        {
            //            serialize = !fieldValue.IsNill;
            //
            //            if(!fieldValue.IsNill)
            //            {
            //                value = fieldValue.Value;
            //            }
            //        }
            //
            //
            private void SetIntegerField(ref bool serialize, ref int value, NillableInteger fieldValue)
            {
                serialize = !fieldValue.IsNill;

                if(!fieldValue.IsNill)
                {
                    value = fieldValue.Value;
                }
            }
            private void SetBooleanField(ref bool serialize, ref bool value, bool fieldValue)
            {
                serialize = true;
                value = fieldValue;
            }


            #endregion

            #region Format Types Utilities

            public FormatTypes PrepareFormatTypes(string current)
            {
                FormatTypes.CurrentType = current;
                return FormatTypes.Instance;
            }

            public void AddToTypes( ref string updateFormat, FormatTypes types)
            {
                updateFormat = FormatTypes.CurrentType;
                
                // The <EnumItem> array embedded in a config item is now auto-populated by the 
                // Values for a given custom user type, if the user chooses that type as the 
                // 'Format' field value.  

                // So, if we found they choose a custom type, we will kindly inject those values
                // as the EnumItem array for this config item, so that the Application Server
                // is aware of those values

                ArrayList customConfigs = MaxProject.Instance.CustomConfigItems;

                foreach(UserConfigType customConfig in customConfigs)
                {
                    if(customConfig.IsSameName(updateFormat))
                    {
                        // so, the user is using a custom installer type.  Let's autopopulate
                        // the enum items

                        string[] enumItems = new string[customConfig.Values.Count];
                        customConfig.Values.CopyTo(enumItems, 0);
                        config.EnumItem = enumItems;
                        break;
                    }
                }
                
            }

            public class FormatTypes
            {
                #region Singleton
                public static FormatTypes Instance 
                {
                    get 
                    {
                        if(instance == null)
                        {
                            instance = new FormatTypes();
                            Clear();
                        }

                        return instance;
                    }
                }
                private static FormatTypes instance;
                #endregion

                public static string CurrentType { get { return current; } set { current = value; } }
                public static string[] AllTypes
                { 
                    get 
                    { 
                        ArrayList userDefinedTypes = MaxProject.Instance.CustomConfigItems;
                        string[] allTypes = new string[knownTypes.Count + userDefinedTypes.Count]; 
                        knownTypes.CopyTo(allTypes, 0);
                        int i = knownTypes.Count;
                        foreach(UserConfigType config in userDefinedTypes)
                        {
                            allTypes[i++] = config.Name;
                        }

                        return allTypes;
                    }
                }

                private static ArrayList knownTypes = new ArrayList();
                private static string current = Metreos.Interfaces.IConfig.StandardFormat.String.ToString();

                public static void AddType(string newType)
                {
                    if(newType == null) return;
                    if(!knownTypes.Contains(newType)) knownTypes.Add(newType);
                }

                public static void Clear() 
                { 
                    knownTypes.Clear();
                    string[] coreNames = Enum.GetNames(typeof(Metreos.Interfaces.IConfig.StandardFormat));
                    knownTypes.AddRange(coreNames);
                }

                private FormatTypes() {}
            }

            #endregion
            #region Nillable Types
            public class NillableBoolean
            {
                public bool IsNill { get { return nill; } }
                public bool nill;

                public bool Value { get { return innerValue; } set { innerValue = value; } }
                private bool innerValue;
                private NillableBoolean()
                {
                    nill = true;
                }

                private NillableBoolean(bool @value)
                {
                    nill = false;
                    Value = @value;
                }


                public static NillableBoolean NullBoolean { get { return nullBoolean; } }
                public static NillableBoolean CreateNonNullBoolean(bool @value) { return new NillableBoolean(@value); }
                private static NillableBoolean nullBoolean = new NillableBoolean();
            }

            public class NillableInteger
            {
                public bool IsNill { get { return nill; } }
                private bool nill;

                public int Value { get { return innerValue; } set { innerValue = value; } }
                private int innerValue;

                private NillableInteger()
                {
                    nill = true;
                }

                private NillableInteger(int @value)
                {
                    nill = false;
                    Value = @value;
                }

                public static NillableInteger NullInteger { get { return nullInteger; } }
                public static NillableInteger CreateNonNullInteger(int @value) { return new NillableInteger(@value); }
                private static NillableInteger nullInteger = new NillableInteger(); 
            }
            #endregion

            #region String Comparison Utilities
        
            public static bool IsNameColumn(string name)
            {
                return name == ConfigWrapper.NameMapping;
            }

            public static bool IsMinColumn(string name)
            {
                return name == ConfigWrapper.MinValueMapping;
            }

            public static bool IsMaxColumn(string name)
            {
                return name == ConfigWrapper.MaxValueMapping;
            }

            #endregion
        }
        #endregion

        #region GridColumn Utilites For Use From Within GridColumn classes

        public bool IsPassword(int rowNum)
        {
            ArrayList configs = this.DataSource as ArrayList;

            for(int i = 0; i < configs.Count; i++)
            {
                if(rowNum == i)
                {
                    ConfigWrapper config = configs[i] as ConfigWrapper;
                    if(0 == String.Compare(config.Config.format, Metreos.Interfaces.IConfig.StandardFormat.Password.ToString(), true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public void MarkDirty()
        {
            if (!noNotify) host.SignalDirty();
        }

        #endregion
    } // MaxInstallerEditor

}   // namespace