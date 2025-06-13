using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Manager;
using Metreos.Max.Core.Package;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using PropertyGrid.Core;



namespace Metreos.Max.Core.Tool
{
    public class MaxTool: MaxSelectableObject
    {           
        public enum ToolTypes 
        { 
            None, Action, Event, Code, Function, Variable, 
            Comment, Annotation, Label, Start, Loop, Null 
        }

        public MaxTool(ToolTypes type, MaxPackage pkg)
        {
            this.tooltype = type;
            this.package  = pkg;
            if (this.package == null)
                this.package = MaxManager.Instance.Packages.StubPackage;

            this.deletable = !package.Name.Equals(Const.stockPackageName) 
                          && !package.Name.Equals(Const.AppControlPackageName); 

            this.imagesSm = MaxImageIndex.Instance.StockToolImages16x16;
            this.imagesLg = MaxImageIndex.Instance.StockToolImages32x32;

            this.toolID = Const.Instance.NextNodeID;
            this.packageToolDefinition = null;
        }

        public string ObjectDisplayName { get { return this.DisplayName; } }

        protected ToolTypes  tooltype;
        protected MaxPackage package;
        protected string name;
        protected string description;
        protected string samoaType;
        protected object tag;
        protected long   toolID;
        protected bool   disabled;
        protected bool   displayed;          
        protected bool   deletable;
        protected bool   pathIsDefault;
        protected MaxToolGroup toolGroup;
        protected Framework.Satellite.Property.DataTypes.Type pmObjectType;
        
        protected MaxImageList imagesSm;
        protected MaxImageList imagesLg;
        protected int imageIndexSm;
        protected int imageIndexLg;
        protected PropertyDescriptorCollection properties;

        public    bool Deletable        { get { return deletable;    } }  
        public    long ToolID           { get { return toolID;       } }
        public    MaxPackage Package    { get { return package;      } }
        public    ToolTypes  ToolType   { get { return tooltype;     } }
        public    MaxImageList ImagesSm { get { return imagesSm;     } set { imagesSm = value;     } }
        public    MaxImageList ImagesLg { get { return imagesLg;     } set { imagesLg = value;     } }
        public    string Name           { get { return name;         } set { name = value;         } }
        public    object Tag            { get { return tag;          } set { tag  = value;         } }   
        public    bool   PathIsDefault  { get { return pathIsDefault;} set { pathIsDefault= value; } }   
        public    int    ImageIndexSm   { get { return imageIndexSm; } set { imageIndexSm = value; } }
        public    int    ImageIndexLg   { get { return imageIndexLg; } set { imageIndexLg = value; } }         
        public    bool   Disabled       { get { return disabled;     } set { disabled     = value; } }  
        public    bool   Displayed      { get { return displayed;    } set { displayed    = value; } }  
        public    string Description    { get { return description;  } set { description  = value; } } 
        public    MaxToolGroup ToolGroup{ get { return toolGroup;    } set { toolGroup    = value; } }   
        public    string FullQualName   { get { return package.Name + Const.dot + name; } }
        public    bool IsAction         { get { return tooltype == ToolTypes.Action || tooltype == ToolTypes.Code; } } 
        public    bool IsEvent          { get { return tooltype == ToolTypes.Event;  } } 
        public virtual string DisplayName { get { return name;  } set { name = value;  } }

        protected PackageElement packageToolDefinition;
        public    PackageElement PackageToolDefinition { get { return packageToolDefinition; } }  


        /// <summary>Insert supplied tool icon into package image list</summary>
        public void SetToolImages(MaxPackage.IconInfo iconInfo)
        {
            if (iconInfo == null || package.ImagesLocked) return;
          
            if (iconInfo.small != null) 
            {
                int newindex = package.ImagesSm.Add(iconInfo.small);
                if (newindex > 0)
                {                                   // temp hack to replace adam's icon
                    if  (this.Name == "Write")      // we need to remove this from pkg def
                         this.ImageIndexSm = MaxImageIndex.stockTool16x16IndexAction;
                    else
                    {
                         this.imagesSm = package.ImagesSm;
                         this.imageIndexSm = newindex;
                    }
                }
            } 

            if (iconInfo.large != null) 
            {
                int newindex = package.ImagesLg.Add(iconInfo.large);
                if (newindex > 0)
                {                                  // temp hack to replace adam's icon                                                    
                    if (this.Name == "Write")      // we need to remove this from pkg def
                        this.ImageIndexLg = MaxImageIndex.stockTool32x32IndexAction;            
                    else
                    {
                        this.imagesLg = package.ImagesLg;
                        this.imageIndexLg = newindex;
                    }
                }
            }  
        }   // setToolImages


        #region MaxSelectableObject Members

        public PropertyDescriptorCollection MaxProperties { get{ return this.properties; }}

        /// <summary>Ask properties manager to create this object's properties</summary>                
        public PropertyDescriptorCollection CreateProperties(PackageElement pe) 
        {
            MaxPropertiesManager propertiesManager = PmProxy.PropertiesManager;

            CreatePropertiesArgs args = new
                CreatePropertiesArgs(this, pe, this.PmObjectType);

            args.iconSm = this.ImagesSm[this.ImageIndexSm];

            this.properties = propertiesManager.ConstructProperties(args);  // 319

            return this.properties;
        }   

        public Framework.Satellite.Property.DataTypes.Type PmObjectType { get { return pmObjectType; } }  

        public void OnPropertiesChangeRaised(MaxProperty[] properties) { }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Tool; } }

        public void MaxSerialize(System.Xml.XmlTextWriter writer)
        {
        }

        #endregion

        #region ICustomTypeDescriptor Members
      
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
      
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
      
        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
      
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
      
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
      
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
      
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        { 
            return GetProperties();
        }
      
        public PropertyDescriptorCollection GetProperties()
        {
            return this.MaxProperties;         
        }
      
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
      
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }
      
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
      
        public string GetClassName()
        {
            TypeDescriptor.GetClassName(this, true);
            return null;
        }
      
        #endregion

    }   // class MaxTool



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxActionTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxActionTool: MaxTool
    {           
        /// <summary>Normal constructor</summary>        
        public MaxActionTool(MaxPackage pkg, MaxPmAction pmaction): base(MaxTool.ToolTypes.Action, pkg)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexAction;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexAction;

            this.packageToolDefinition = pmaction;
            this.Name         = pmaction.Name;
            this.DisplayName  = Config.UseDisplayNames && pmaction.DisplayName != null ? pmaction.DisplayName : pmaction.Name;
            this.Description  = pmaction.Description;
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Action;
            this.SetToolImages(pmaction.Tag as MaxPackage.IconInfo);
            pmaction.Tag = null;

            this.CreateProperties(pmaction);
        }


        /// <summary>Stub action tool constructor</summary>  
        public MaxActionTool(string name, MaxPackage pkg): base(MaxTool.ToolTypes.Action, pkg)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexAction;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexAction;
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Action;
            this.Name         = name;
            this.DisplayName  = name;    // No package so no display name
            this.CreateProperties(null); // Null indicates no package present  
        }

        public MaxPmAction PmAction { get { return this.packageToolDefinition as MaxPmAction; } }
        public override string DisplayName { get { return displayName; } set { displayName = value; } }
        protected string displayName;

    }   // class MaxActionTool



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxEventTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxEventTool: MaxTool
    {    
        /// <summary>Normal constructor</summary>        
        public MaxEventTool(MaxPackage pkg, MaxPmEvent pmevent): base(MaxTool.ToolTypes.Event, pkg)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexEvent;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexEvent;

            this.packageToolDefinition = pmevent;
            this.Name         = pmevent.Name;
            this.Description  = pmevent.Description;
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Event;
            this.SetToolImages(pmevent.Tag as MaxPackage.IconInfo);
            pmevent.Tag = null;

            this.CreateProperties(pmevent);
        }


        /// <summary>Stub event tool constructor</summary> 
        public MaxEventTool(string name, MaxPackage pkg): base(MaxTool.ToolTypes.Event, pkg)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexEvent;
            this.imageIndexLg = MaxImageIndex.stockTool16x16IndexEvent;
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Event;
            this.Name         = name;
            this.DisplayName  = name;
            this.CreateProperties(null);   // Null indicates no package present 316
        }


        public MaxPmEvent PmEvent { get { return this.packageToolDefinition as MaxPmEvent; } }
       
        public bool IsTriggeringEvent()
        {
            if (this.MaxProperties == null) return false;
            string propName = Framework.Satellite.Property.DataTypes.TRIGGER_NAME;
            TriggerProperty prop = this.MaxProperties[propName] as TriggerProperty;              
            return (prop != null && 
               (prop.Value == PropertyGrid.Core.EventType.triggering || 
                prop.Value == PropertyGrid.Core.EventType.hybrid));
        }

        public bool IsUnsolicitedEvent()
        {
            if (this.MaxProperties == null) return false;
            string propName = Framework.Satellite.Property.DataTypes.TRIGGER_NAME;
            TriggerProperty prop = this.MaxProperties[propName] as TriggerProperty;
            return (prop != null &&
               (prop.Value == PropertyGrid.Core.EventType.nonTriggering ||
                prop.Value == PropertyGrid.Core.EventType.hybrid));
        }

    }   // class MaxEventTool



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxFunctionTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxFunctionTool: MaxTool
    {           
        public MaxFunctionTool(): base(MaxTool.ToolTypes.Function, null)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexFunction;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexFunction;

            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Function;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultFunctionToolName; 
            this.Description= Const.defaultFunctionToolDesc; 
        }
    } 



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxCodeTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxCodeTool: MaxTool
    {           
        public MaxCodeTool(): base(MaxTool.ToolTypes.Code, null)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexAction;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexAction;

            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Code;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultCodeToolName; 
            this.Description= Const.defaultCodeToolDesc; 
        }
    }    



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxVariableTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxVariableTool: MaxTool
    {           
        public MaxVariableTool(): base(MaxTool.ToolTypes.Variable, null)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexVariable;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexVariable;

            // This is set prematurely, because the tool box does not distinguish between
            // global and local variables.  The scope of a variable is decided at 
            // the moment the user drags the variable to a canvas, whether it be AppCanvas 
            // or FunctionCanvas. Is a slight problem though.
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.LocalVariable;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultVariableToolName; 
            this.Description= Const.defaultVariableToolDesc; 
        }
    }    


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxLabelTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxLabelTool: MaxTool
    {           
        public MaxLabelTool(): base(MaxTool.ToolTypes.Label, null)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexLabel;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexToolGroup;

            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Label;

            this.Name       = Const.defaultLabelToolName; 
            this.Description= Const.defaultLabelToolDesc; 

            this.CreateProperties(this.PackageToolDefinition); 
        }
    }    


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxCommentTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxCommentTool: MaxTool
    {           
        public MaxCommentTool(): base(MaxTool.ToolTypes.Comment, null)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexComment;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexToolGroup;

            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Comment;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultCommentToolName; 
            this.Description= Const.defaultCommentToolDesc; 
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxAnnotationTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxAnnotationTool: MaxTool
    {           
        public MaxAnnotationTool(): base(MaxTool.ToolTypes.Annotation, null)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexComment;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexToolGroup;

            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Comment;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultAnnotToolName; 
            this.Description= Const.defaultAnnotToolDesc; 
        }
    }      


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxStartTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxStartTool: MaxTool
    {           
        public MaxStartTool(): base(MaxTool.ToolTypes.Start, null)
        {
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexStart;

            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.StartNode;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultStartToolName; 
            this.Description= Const.defaultStartToolDesc;  
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxLoopTool  
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxLoopTool: MaxTool
    {           
        public MaxLoopTool(): base(MaxTool.ToolTypes.Loop, null)
        {
            this.imageIndexSm = MaxImageIndex.stockTool16x16IndexLoop;
            this.imageIndexLg = MaxImageIndex.stockTool32x32IndexToolGroup;

            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Loop;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultLoopToolName; 
            this.Description= Const.defaultLoopToolDesc; 
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxNullTool
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxNullTool: MaxTool
    {           
        public MaxNullTool(): base(MaxTool.ToolTypes.Null, null)
        {
            this.imageIndexSm = 0;
            this.imageIndexLg = 0;
                                             
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.Nothing;
            this.CreateProperties(this.PackageToolDefinition);

            this.Name       = Const.defaultNullToolName; 
            this.Description= Const.defaultNullToolDesc;  
        }
    }    

}   // namespace
