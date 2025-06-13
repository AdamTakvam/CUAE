using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for ActionParameterProperty.
    /// </summary>
    [Editor(typeof(ActionParameterGrowableEditorAttribute), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(ActionParameterGrowable2Converter))]
    public class ActionParameterPropertyGrowable : MaxProperty
    {
        public bool Growable { get { return growable; } }
        public bool Replicatable { get { return replicatable; } }

        private bool growable;
        private bool replicatable;

        public ActionParameterPropertyGrowable(string name, bool growable, bool replicatable, string category, bool isReadOnly, 
             IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) 
            : base(name, String.Empty, isReadOnly, mpm, subject, container)
        {
            this.growable           = growable;
            this.replicatable       = replicatable;
            this.category           = category;
        }
    }  

    internal class ActionParameterGrowable2Converter : ExpandableObjectConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            MaxProperty property = context.PropertyDescriptor as MaxProperty;
            return property.ChildrenProperties.Count != 0;
        }

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
            return true;
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
            MaxProperty property = (MaxProperty) context.PropertyDescriptor;

            return property.ChildrenProperties.Count + " defined";
        }  
    }

    internal class ActionParameterGrowableEditorAttribute : System.Drawing.Design.UITypeEditor
    {
        public ActionParameterGrowableEditorAttribute() : base()
        {}

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ActionParameterPropertyGrowable actionProperty = context.PropertyDescriptor as ActionParameterPropertyGrowable;

            // get the editor service.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc == null) 
            {
                return value;
            }

            if(actionProperty.Growable && actionProperty.Replicatable)
            {
                ActionRepComboEditor editor = new ActionRepComboEditor(context, edSvc, value, actionProperty.Mpm.RemovePropertyDelegate);
				
                // instruct the editor service to display the control as a dropdown.
                edSvc.DropDownControl(editor);
		
                MaxProperty property = (MaxProperty) value;

                property.ValueChanged(false);
                property.Mpm.FocusPropertyGrid();
                editor.Dispose();
                editor = null;

                // return the updated value;
                return value;
            }
            else if(actionProperty.Growable && !actionProperty.Replicatable)
            {
                ActionParameterGrowableEditor editor = new ActionParameterGrowableEditor(context, edSvc, value, actionProperty.Mpm.RemovePropertyDelegate);
				
                // instruct the editor service to display the control as a dropdown.
                edSvc.DropDownControl(editor);
		
                MaxProperty property = (MaxProperty) value;

                property.ValueChanged(false);
                property.Mpm.FocusPropertyGrid();

                editor.Dispose();
                editor = null;

                // return the updated value;
                return value;
            }
            else if(!actionProperty.Growable && actionProperty.Replicatable)
            {
                ReplicateParameterGrowableEditor editor = new ReplicateParameterGrowableEditor(
                    context, 
                    edSvc,
                    value, 
                    actionProperty.Mpm.RemovePropertyDelegate);
				
                // instruct the editor service to display the control as a dropdown.
                edSvc.DropDownControl(editor);
		
                MaxProperty property = (MaxProperty) value;

                property.ValueChanged(false);
                property.Mpm.FocusPropertyGrid();

                editor.Dispose();
                editor = null;

                // return the updated value;
                return value;
            }
            else
            {
                return value;
            }
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            base.PaintValue (e);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }
    }

}
