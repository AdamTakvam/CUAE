using System;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxRecumbentEventNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A small-icon event node in sideways orientation</summary>
    public class MaxRecumbentEventNode: MaxConfigurableIconicNode
    {
        public MaxRecumbentEventNode(MaxCanvas canvas, MaxTool tool): 
        base (NodeTypes.Event, canvas, tool, configInfo) 
        {
            InitEvent();   
        }

        public MaxRecumbentEventNode(MaxCanvas canvas, MaxTool tool, long id):
        base (NodeTypes.Event, canvas, tool, id, configInfo)  
        {       
            InitEvent(); 
        } 

        private void InitEvent()
        {
            this.Label.Editable = false;   
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.EventInstance;
            this.CreateProperties(this.Tool.PackageToolDefinition);
            this.canLinkIn = this.canLinkOut = true;
        }

        public override bool CanLinkTo()    { return canLinkIn;  }
        public override bool CanLinkFrom()  { return canLinkOut; }  

        protected bool canLinkOut; 
        protected bool canLinkIn;  
        public    bool CanLinkOut { get { return canLinkOut; } set { canLinkOut = value; } }
        public    bool CanLinkIn  { get { return canLinkIn;  } set { canLinkIn  = value; } }

        protected static MaxEventListItemNodeConfigInfo configInfo  
            = new MaxEventListItemNodeConfigInfo();
    }



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxRecumbentFunctionNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A small-icon function node in sideways orientation</summary>
    public class MaxRecumbentFunctionNode: MaxConfigurableIconicNode
    {
        public MaxRecumbentFunctionNode(MaxCanvas canvas, MaxTool tool): 
        base (NodeTypes.Function, canvas, tool, configInfo) 
        {
            InitFunction();   
        }

        public MaxRecumbentFunctionNode(MaxCanvas canvas, MaxTool tool, long id):
        base (NodeTypes.Function, canvas, tool, id, configInfo)  
        {       
            InitFunction(); 
        } 

        private void InitFunction()
        {
            this.Label.Editable = false;   
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.EventInstance;
            this.CreateProperties(this.Tool.PackageToolDefinition);
        }

        public override bool CanLinkTo()    { return this.Port.LinksCount == 0; }
        public override bool CanLinkFrom()  { return false; }  

        private static MaxFunctionListItemNodeConfigInfo configInfo  
            = new MaxFunctionListItemNodeConfigInfo();
    }



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxRecumbentVariableNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A small-icon variable node in sideways orientation</summary>
    public class MaxRecumbentVariableNode: MaxConfigurableIconicNode
    {
        public MaxRecumbentVariableNode(MaxCanvas canvas, MaxTool tool, DataTypes.Type scope): 
        base (NodeTypes.Variable, canvas, tool, configInfo) 
        {
            InitVariable(scope);   
        }

        public MaxRecumbentVariableNode(MaxCanvas canvas, MaxTool tool, DataTypes.Type scope, long id):
        base (NodeTypes.Variable, canvas, tool, id, configInfo)  
        {       
            InitVariable(scope); 
        } 

        private void InitVariable(DataTypes.Type scope)
        {
            this.NodeName = this.Label.Text = this.MakeUniqueNodeName();

            this.Label.AddObserver(this);         // Monitor label changes  

            this.pmObjectType = scope;

            this.CreateProperties(this.Tool.PackageToolDefinition);
            Utl.SetProperty(this.MaxProperties, Const.PmVariableName, this.NodeName); 
        }


        /// <summary>Catch node label text change</summary>
        protected override void OnObservedChanged(GoObject observed, int subhint, 
            int oI, object oldVal, RectangleF oR, int nI, object newVal, RectangleF nR)
        {
            if  (observed is GoText && newVal is string) 
            {
                this.OnNodeNameChanged(MaxCanvas.CanvasTypes.None, NodeTypes.Variable, 
                    observed as GoText, newVal as string, oldVal as string);

                Utl.SetProperty(this.MaxProperties, Const.PmVariableName, newVal as string);  
            }

            base.OnObservedChanged(observed, subhint, oI, oldVal, oR, nI, newVal, nR);
        }


        /// <summary>Notify framework of node name change</summary>
        public override void FireNodeNameChangeEvent
        ( MaxCanvas.CanvasTypes canvasType, string oldname, string newname)
        {
            this.Canvas.FireNodeEvent(new MaxLocalNodeEventArgs(oldname, newname, this));        
        }


        /// <summary>Handle property change notificatiopn from grid</summary>
        protected override void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty property in properties)
            {
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                   case Const.PmVariableName:
                        this.OnVariableNamePropertyChanged(property);
                        break;
                }  
            }      
        }


        /// <summary>Handle variable name change via property grid</summary>
        protected void OnVariableNamePropertyChanged(MaxProperty property)
        {      
            string newname  = property.Value    as string;
            string oldname  = property.OldValue as string;
            if  (oldname == newname) return;     

            this.Label.Text = newname;  // Fires synchronous name change event

            if (this.Label.Text != newname) 
                property.Value   = oldname;  
            else                        // Change name in variable tray also
            if (this.UserObject is ListViewItem)
               (this.UserObject as ListViewItem).Text = newname;   
        } 


        public string MakeUniqueNodeName()
        {
            string name = null;
            while(true)
            {       
                name = MakeNodeName();
                if  (this.Canvas.CanNameNode(NodeTypes.Variable, name)) break;
            }

            return name;
        }


        public static string MakeNodeName()
        {
            return Const.defaultVariableName + MaxProject.CurrentApp.VariableSequence; 
        }


        public override bool CanLinkTo()    { return false; }
        public override bool CanLinkFrom()  { return false; }  

        private static MaxVariableNodeConfigInfo configInfo  
            = new MaxVariableNodeConfigInfo();
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxNodeConfigInfo
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Iconic node configuration parameters</summary>
    public class MaxNodeConfigInfo
    {
        public enum Orientations { None, Vertical, Horizontal, ListItem }
        public enum ImageTypes   { None, Small, Large, Custom }
        public Orientations orientation;
        public ImageTypes imageType;
        public ImageList  customImage;          // If ImageType is Custom
        public int    customImageIndex;         // ditto
        public Size   iconSize;                 // ditto
        public int    labelFontDelta;           // +/- font size
        public PointF labelOffset;              // icon to label offset deltas
        public int    relativeIconSpot;         // GoObject spot to measure from
        public int    labelSpot;                // GoObject spot to place label
        public bool   labelEditable;            
        public bool   labelItalic;
    }


    /// <summary>Configuration information for a sideways small iconic node</summary>
    public class MaxIconicListItemNodeConfigInfo: MaxNodeConfigInfo
    {
        public MaxIconicListItemNodeConfigInfo(bool editable, bool ital)
        {
            this.orientation = MaxNodeConfigInfo.Orientations.ListItem;
            this.imageType   = MaxNodeConfigInfo.ImageTypes.Small;
            this.labelOffset = new PointF(1F,0F);
            this.relativeIconSpot = GoObject.MiddleRight;
            this.labelSpot        = GoObject.MiddleLeft;
            this.labelEditable = editable;
            this.labelItalic   = ital;
        }
    }


    /// <summary>Configuration information for a sideways small event node</summary>
    public class MaxEventListItemNodeConfigInfo: MaxIconicListItemNodeConfigInfo
    {
        public MaxEventListItemNodeConfigInfo(): base(false, true) 
        { 
            // this.labelFontDelta = -1; 
        }
    }

    /// <summary>Configuration information for a sideways small function node</summary>
    public class MaxFunctionListItemNodeConfigInfo: MaxIconicListItemNodeConfigInfo
    {
        public MaxFunctionListItemNodeConfigInfo(): base(false, false) 
        { 
            this.customImageIndex = MaxImageIndex.stockTool16x16IndexFunction;
        }   
    }


    /// <summary>Configuration information for a sideways small variable node</summary>
    public class MaxVariableNodeConfigInfo: MaxIconicListItemNodeConfigInfo
    {
        public MaxVariableNodeConfigInfo(): base(true, false)  // param 2 = italic
        { 
            this.customImageIndex = MaxImageIndex.stockTool16x16IndexVariable;
        }   
    }

} // namespace
 
