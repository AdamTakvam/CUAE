using System;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;



namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for StandardProperty.
    /// </summary>
    [Editor(typeof(LogStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(OnLogEventConverter))]
    public class OnLogEventProperty : MaxProperty
    {        
        private ContextMenu contextMenu;
        private MenuItem literalItem;
        private MenuItem variableItem;
        private MenuItem csharpItem;

        public OnLogEventProperty(string name, string value_, IMpmDelegates mpm,
            object subject, PropertyDescriptorCollection container) 
            : base(name, value_, false, mpm, subject, container)
        {
            literalItem = new MenuItem();
            literalItem.Index = 0;
            literalItem.Text = "literal";
            literalItem.Click += new EventHandler(LiteralClick); 
            variableItem = new MenuItem();
            variableItem.Index = 1;
            variableItem.Text = "variable";
            variableItem.Click += new EventHandler(VariableClick);
            csharpItem = new MenuItem();
            csharpItem.Index = 0;
            csharpItem.Text = "csharp";
            csharpItem.Click += new EventHandler(CsharpClick);
            contextMenu = new ContextMenu(new MenuItem[] { literalItem, variableItem, csharpItem } );
        }

        public override void OnContextMenuRequest(Control control, System.Drawing.Point point)
        {
            contextMenu.Show( control, point );
        }

        private void LiteralClick(object sender, EventArgs e)
        {
            SetUserTypeValue( DataTypes.UserVariableType.literal );
        }

        private void VariableClick(object sender, EventArgs e)
        {
            SetUserTypeValue( DataTypes.UserVariableType.variable );
        }

        private void CsharpClick(object sender, EventArgs e)
        {
            SetUserTypeValue( DataTypes.UserVariableType.csharp );
        }

        public void SetUserTypeValue(DataTypes.UserVariableType userType)
        {
            foreach(MaxProperty property in this.ChildrenProperties)
            {
                if(property is UserTypeProperty)
                {
                    UserTypeProperty userTypeProperty = property as UserTypeProperty; 
                    userTypeProperty.SetValue(userType);
                    break;
                }
            }
        }
    }

    internal class OnLogEventConverter : ExpandableObjectConverter
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
            if(typeof(MaxProperty) == destinationType)
                return true;

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
            OnLogEventProperty logProperty = context.PropertyDescriptor as OnLogEventProperty;
            
            for(int i = 0; i < logProperty.ChildrenProperties.Count; i++)
            {
                if(logProperty.ChildrenProperties[i] is UserTypeProperty)
                {
                    UserTypeProperty userType = logProperty.ChildrenProperties[i] as UserTypeProperty;
                    
                    if(userType.Value == DataTypes.UserVariableType.variable)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            OnLogEventProperty logProperty = context.PropertyDescriptor as OnLogEventProperty;

            object[] globalVarsObj   = logProperty.Mpm.GetGlobalVarsDelegate();
            object[] functionVarsObj = logProperty.Mpm.GetFunctionVarsDelegate(logProperty.Subject);
            string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);
            string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);

            ArrayList allVars = MaxProperty.FormatVariablesList(globalVars, functionVars);

            return new StandardValuesCollection(allVars);    
        }
    }

    internal class LogStringEditor : System.Drawing.Design.UITypeEditor
    {
        public LogStringEditor() : base()
        {}

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            OnLogEventProperty logEventProperty = (OnLogEventProperty) context.PropertyDescriptor;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            UserTypeProperty type = null;
            for(int i = 0; i < logEventProperty.ChildrenProperties.Count; i++)
                if(logEventProperty.ChildrenProperties[i] is UserTypeProperty)
                    type = logEventProperty.ChildrenProperties[i] as UserTypeProperty;

            // Should never ever happen
            if(type == null)
                throw new Exception("Code is out of date.");

            if(type.Value == DataTypes.UserVariableType.literal)
            {
                MaxProperty prop = value as MaxProperty;      

                LiteralEditor editor = new LiteralEditor(prop.Value as string);
                edSvc.ShowDialog(editor);

                string value_ = editor.Ok ? editor.Value : prop.Value as string;
                editor.Dispose();
                editor = null;
                return value_;
            }
            else
            {
                CSharpEditor editor = 
                    new CSharpEditor(logEventProperty.Mpm.GetUsings(), (value as MaxProperty).Value.ToString());
                edSvc.ShowDialog(editor);

                // Update usings if user selected ok
                if(editor.Ok)
                    logEventProperty.Mpm.UpdateUsings(editor.Usings);

                object value_;
                value_ = editor.Ok ? editor.Value : logEventProperty.Value;
                editor.Dispose();
                editor = null;
                return value_; 
            }    
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            OnLogEventProperty onLogEventProperty = (OnLogEventProperty)e.Context.PropertyDescriptor;
          
            for(int i = 0; i < onLogEventProperty.ChildrenProperties.Count; i++)
                if(onLogEventProperty.ChildrenProperties[i].GetType() == typeof(OnOffProperty))
                {
                    OnOffProperty onOffProperty = (OnOffProperty) onLogEventProperty.ChildrenProperties[i];
                  
                    if(onOffProperty.Value == DataTypes.OnOff.On)
                        e.Graphics.DrawImage(PropertiesImageControl.on, e.Bounds);

                    if(onOffProperty.Value == DataTypes.OnOff.Off)
                        e.Graphics.DrawImage(PropertiesImageControl.off, e.Bounds);
                }

            base.PaintValue (e);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }
}
