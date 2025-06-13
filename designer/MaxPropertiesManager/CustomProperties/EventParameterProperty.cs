using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using PropertyGrid.Core;

using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for EventParametersProperty.
    /// </summary>
    [Editor(typeof(EventParameterEditorAttribute), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(EventParameterConverter))]
    public class EventParameterProperty : MaxProperty
    {
        public bool IsGuaranteed { get { return this.isGuaranteed; } set { isGuaranteed = value; } }    
        private bool isGuaranteed;

        public EventParameterProperty(string name, string displayName, bool isGuaranteed,
            IMpmDelegates mpm, object subject, 
            PropertyDescriptorCollection container)
            : base(name, displayName, String.Empty, false, mpm, subject, container)
        {
            this.description      = Defaults.GUARANTEED_PARAM_APPEND;
            this.isGuaranteed     = isGuaranteed;
            this.category         = DataTypes.EVENT_CATEGORY;
        }

        public EventParameterProperty( string name, string displayName, bool isGuaranteed, string description, 
            IMpmDelegates mpm, object subject, 
            PropertyDescriptorCollection container)
            : base(name, displayName, String.Empty, false, mpm, subject, container)
        {
            this.isGuaranteed     = isGuaranteed;
            this.category         = DataTypes.EVENT_CATEGORY;

            if(isGuaranteed)
            {
                string descrip = description != null ? description : String.Empty; 
                this.description = descrip + Defaults.blank + Defaults.GUARANTEED_PARAM_APPEND;
            }
            else
            {
                this.description = description != null ? description : String.Empty; 
            }
        }	
    }

    internal class EventParameterConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            if(value is MaxProperty)
            {
                MaxProperty property = (MaxProperty) value;

                return property.ChildrenProperties;
            }
            return base.GetProperties (context, value, attributes);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if(typeof(EventParameterProperty) == destinationType)
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
                                                             
        }
            
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, 
            System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            EventParameterProperty eventParamProp = context.PropertyDescriptor as EventParameterProperty;
            EventParamTypeProperty typeProp = eventParamProp.ChildrenProperties[DataTypes.EVENT_PARAM_TYPE] as EventParamTypeProperty;
            return typeProp.Value == DataTypes.EventParamType.variable;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            EventParameterProperty eventProperty = context.PropertyDescriptor as EventParameterProperty;
            object[] globalVarsObj    = eventProperty.Mpm.GetGlobalVarsDelegate();
            string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);
            return new StandardValuesCollection(globalVars);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
            System.Globalization.CultureInfo culture, object value, Type destType )
        {
			
            if(value is EventParameterProperty && destType == typeof(string))
            {
                EventParameterProperty parameter = value as EventParameterProperty;
                return parameter.Value;
            }

            return base.ConvertTo(context,culture,value,destType);
        }  
    }

    internal class EventParameterEditorAttribute : System.Drawing.Design.UITypeEditor
    {
        public EventParameterEditorAttribute() : base()
        {}

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            MaxProperty prop = value as MaxProperty;      

            LiteralEditor editor = new LiteralEditor(prop.Value as string);
            edSvc.ShowDialog(editor);

            string value_ = editor.Ok ? editor.Value : prop.Value as string;
            editor.Dispose();
            editor = null;
            return value_;            
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {       
            EventParameterProperty eventProperty = e.Context.PropertyDescriptor as EventParameterProperty;
            
            EventParamTypeProperty typeProperty 
                = eventProperty.ChildrenProperties[DataTypes.EVENT_PARAM_TYPE] as EventParamTypeProperty;

            eventParamTypeType eventType = Util.FromMaxToMetreosEventParamType(typeProperty.Value);
          
            if(eventType == eventParamTypeType.literal)
            {
                e.Graphics.DrawImage(PropertiesImageControl.str, e.Bounds);
            }

            if(eventType == eventParamTypeType.variable)
            {
                e.Graphics.DrawImage(PropertiesImageControl.var, e.Bounds);
            }
    
            base.PaintValue (e);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            EventParameterProperty eventParamProp = context.PropertyDescriptor as EventParameterProperty;
            EventParamTypeProperty typeProp = eventParamProp.ChildrenProperties[DataTypes.EVENT_PARAM_TYPE] as EventParamTypeProperty;

            return typeProp.Value == DataTypes.EventParamType.variable ?
                System.Drawing.Design.UITypeEditorEditStyle.None : System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }
    }
}
