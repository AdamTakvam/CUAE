using System;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for StandardProperty.
    /// </summary>
    [Editor(typeof(GenericStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(GenericPropertyConverter))]
    public class GenericProperty : MaxProperty
    {       
        
        public GenericProperty(
            string name, 
            string value_, 
            bool isReadOnly, 
            IMpmDelegates mpm,
            object subject,
            PropertyDescriptorCollection container) 
            : base(name, value_, isReadOnly, mpm, subject, container)
        {
			
        }

        public GenericProperty(
            string name, 
            string value_, 
            bool isReadOnly, 
            string category, 
            IMpmDelegates mpm, 
            object subject,
            PropertyDescriptorCollection container) 
            : base(name, value_, isReadOnly, mpm, subject, container)
        {
            this.category = category;
        }	
    }

    internal class GenericStringEditor : System.Drawing.Design.UITypeEditor
    {
        public GenericStringEditor() : base()
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
            return false;
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }

    internal class GenericPropertyConverter : TypeConverter
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

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            if(context.PropertyDescriptor is MaxProperty)
            {
                MaxProperty property = (MaxProperty) context.PropertyDescriptor;

                if(property.ChildrenProperties.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            return false;
        }


        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if(typeof(MaxProperty) == destinationType)
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
                                                             
        }
            
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
        {
            if(value is MaxProperty && destType == typeof(string))
            {
                MaxProperty property = (MaxProperty)value;

                return property.Value;
            }

            return base.ConvertTo(context,culture,value,destType);
        }  

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
