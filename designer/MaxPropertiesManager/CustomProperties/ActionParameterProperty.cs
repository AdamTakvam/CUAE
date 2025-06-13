using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Metreos.ApplicationFramework.ScriptXml;



namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary> Represents an action parameter.  string editor, variable dropdown, and csharp editor </summary>
    [Editor(typeof(StringEditorWithIcon), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(ActionParameterConverter))]
    public class ActionParameterProperty : MaxProperty
    {  
        #region Properties
        public bool Required  { get { return isRequired; } set { isRequired = value; } }
        public new string Category  { get { return this.category; }  set { this.category = value; } }
        public bool AllowMultiple   { get { return allowMultiple; }  set { allowMultiple = value; } }
        public bool IsHiddenProperty{ get { return isHidden;   } set { isHidden   = value; } }
        public string[] EnumValues  { get { return enumValues; } set { enumValues = value; } }
        #endregion Properties

        private bool isRequired;
        private bool isHidden;   // 20060907 
        private bool allowMultiple;
        private string[] enumValues;

        private ContextMenu contextMenu;
        private MenuItem literalItem;
        private MenuItem variableItem;
        private MenuItem csharpItem;

        
        public ActionParameterProperty(string name, string displayName, string value_, 
            bool isRequired, bool allowMultiple, string description, string[] enumValues,
            IMpmDelegates mpm, object subject, PropertyDescriptorCollection container)
            : base(name, displayName, value_, false, mpm, subject, container)
        {
            this.isRequired = isRequired;
            this.allowMultiple = allowMultiple;
            this.enumValues = enumValues;
            this.category = DataTypes.ACTION_PARAMETERS_CATEGORY;
            string descrip = description == null? String.Empty: description;

            this.description = isRequired?
                 descrip + Defaults.blank + Defaults.REQUIRED_PARAM_APPEND:             
                 descrip;
             
            literalItem = new MenuItem();
            literalItem.Index = 0;
            literalItem.Text = _lit;
            literalItem.Click += new EventHandler(LiteralClick); 

            variableItem = new MenuItem();
            variableItem.Index = 1;
            variableItem.Text = _var;
            variableItem.Click += new EventHandler(VariableClick);

            csharpItem = new MenuItem();
            csharpItem.Index = 0;
            csharpItem.Text = _csh;
            csharpItem.Click += new EventHandler(CsharpClick);

            contextMenu = new ContextMenu(new MenuItem[] { literalItem, variableItem, csharpItem } );
        }


        public override AttributeCollection Attributes   
        { 
            get // BrowsableAttribute(false) specifies that a property should                   
            {   // not be displayed in the property grid -- 20060907 
                return this.isHidden?       
                    new AttributeCollection(new Attribute[] { new BrowsableAttribute(false) } ):  
                    base.Attributes; 
            } 
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

        private const string _lit = "literal", _var = "variable", _csh = "csharp";

    }   // class ActionParameterProperty



    internal class ActionParameterConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
            object value, Attribute[] attributes)
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
            if(typeof(ActionParameterProperty) == destinationType)  return true;

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

        public override object ConvertTo(ITypeDescriptorContext context, 
            System.Globalization.CultureInfo culture, object value, Type destType )
        {
            if(value is ActionParameterProperty && destType == typeof(string))
            {
                ActionParameterProperty parameter = (ActionParameterProperty)value;
                return parameter.Value;
            }

            return base.ConvertTo(context,culture,value,destType);
        }  

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            ActionParameterProperty actionProperty = context.PropertyDescriptor as ActionParameterProperty;
            
            for(int i = 0; i < actionProperty.ChildrenProperties.Count; i++)
                if(actionProperty.ChildrenProperties[i] is UserTypeProperty)
                {
                    UserTypeProperty userType = actionProperty.ChildrenProperties[i] as UserTypeProperty; 
                    if(userType.Value == DataTypes.UserVariableType.variable)   return true;
                    if(userType.Value == DataTypes.UserVariableType.literal && 
                        actionProperty.EnumValues != null && 
                        actionProperty.EnumValues.Length != 0) return true;
                }
        
            return false;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ActionParameterProperty actionProperty = context.PropertyDescriptor as ActionParameterProperty;
            
            for(int i = 0; i < actionProperty.ChildrenProperties.Count; i++)
                if(actionProperty.ChildrenProperties[i] is UserTypeProperty)
                {
                    UserTypeProperty userType = actionProperty.ChildrenProperties[i] as UserTypeProperty; 
                    if(userType.Value == DataTypes.UserVariableType.variable)   
                    {                     
                        object[] globalVarsObj    = actionProperty.Mpm.GetGlobalVarsDelegate();
                        object[] functionVarsObj  = actionProperty.Mpm.GetFunctionVarsDelegate(actionProperty.Subject);

                        string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);
                        string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);

                        ArrayList allVars = MaxProperty.FormatVariablesList(globalVars, functionVars);

                        return new StandardValuesCollection(allVars);
                    }

                    if(userType.Value == DataTypes.UserVariableType.literal) 
                    {
                        return new StandardValuesCollection(actionProperty.EnumValues);
                    }
                }  

            return new StandardValuesCollection(null);
        }

        public static readonly string codeOutOfDate = "code is out of date";

    }   // class ActionParameterConverter 



    internal class StringEditorWithIcon : System.Drawing.Design.UITypeEditor
    {
        public StringEditorWithIcon() : base()
        {}                                              

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ActionParameterProperty actionProperty = context.PropertyDescriptor as ActionParameterProperty;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
      
            UserTypeProperty type = null;
            for(int i = 0; i < actionProperty.ChildrenProperties.Count; i++)
                if(actionProperty.ChildrenProperties[i] is UserTypeProperty)
                    type = actionProperty.ChildrenProperties[i] as UserTypeProperty;
            
            if(type == null) // Should never ever happen
                throw new Exception(ActionParameterConverter.codeOutOfDate);

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
                    new CSharpEditor(actionProperty.Mpm.GetUsings(), (value as MaxProperty).Value.ToString());

                edSvc.ShowDialog(editor);

                // Update usings if user selected ok
                if(editor.Ok)
                    actionProperty.Mpm.UpdateUsings(editor.Usings);
        
                actionProperty.Mpm.FocusPropertyGrid();

                // Update text value if user selected ok to exit
                object value_ = editor.Ok ? editor.Value : actionProperty.Value;
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
            ActionParameterProperty actionProperty = (ActionParameterProperty)e.Context.PropertyDescriptor;

            // Indicates that we have the drop down list to draw
            if(e.Value is string)
            {
                object[] functionVarsObj = actionProperty.Mpm.GetFunctionVarsDelegate(actionProperty.Subject);
                object[] globalVarsObj   = actionProperty.Mpm.GetGlobalVarsDelegate();

                string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);
                string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);

                bool isLocal = false;
                if(functionVars != null)
                    isLocal = 0 <= Array.IndexOf(functionVars, e.Value);

                bool isGlobal = false;
                if(globalVars != null)
                    isGlobal = 0 <= Array.IndexOf(globalVars, e.Value);

                // Strictly a global var
                if(isGlobal && !isLocal)
                    e.Graphics.DrawImage(PropertiesImageControl.glo, e.Bounds);

                // Special icon showing naming clash (which is allowed, but the local wins the scope contest
                if(isGlobal && isLocal)     
                    e.Graphics.DrawImage(PropertiesImageControl.lg, e.Bounds);

                // Is only a local var
                if(!isGlobal && isLocal)
                    e.Graphics.DrawImage(PropertiesImageControl.loc, e.Bounds);
            }
            else
            {     
                for(int i = 0; i < actionProperty.ChildrenProperties.Count; i++)
                    if(actionProperty.ChildrenProperties[i].GetType() == typeof(UserTypeProperty))
                    {
                        UserTypeProperty userType = (UserTypeProperty) actionProperty.ChildrenProperties[i];
                    
                        paramType type = Util.FromMaxToMetreosActPar(userType.Value);

                        if(type == paramType.variable)
                            e.Graphics.DrawImage(PropertiesImageControl.var, e.Bounds);

                        else if(type == paramType.literal)
                            e.Graphics.DrawImage(PropertiesImageControl.str, e.Bounds);

                        else if(type == paramType.csharp)
                            e.Graphics.DrawImage(PropertiesImageControl.csp, e.Bounds);

                        break;
                    }
            }

            base.PaintValue (e);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }

    }   // class StringEditorWithIcon



    internal class StringEditor : System.Drawing.Design.UITypeEditor
    {
        public StringEditor() : base()
        {}

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ActionParameterProperty actionProperty = context.PropertyDescriptor as ActionParameterProperty;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            
            UserTypeProperty type = null;
            for(int i = 0; i < actionProperty.ChildrenProperties.Count; i++)
                if(actionProperty.ChildrenProperties[i] is UserTypeProperty)
                    type = actionProperty.ChildrenProperties[i] as UserTypeProperty;
            
            if(type == null) // Should never ever happen
                throw new Exception(ActionParameterConverter.codeOutOfDate);

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
                CSharpEditor editor = new CSharpEditor
                    (actionProperty.Mpm.GetUsings(), (value as MaxProperty).Value.ToString());

                edSvc.ShowDialog(editor);

                // Update usings if user selected ok
                if(editor.Ok)
                    actionProperty.Mpm.UpdateUsings(editor.Usings);
        
                // Update text value if user selected ok to exit
                object value_ = editor.Ok ? editor.Value : actionProperty.Value;
                editor.Dispose();
                editor = null;
                return value_; 
            }
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }

    }   // class StringEditor

}   // namespace
