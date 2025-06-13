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
  /// <summary>User code property</summary>
  [Editor(typeof(CodeEditor), typeof(System.Drawing.Design.UITypeEditor))]
  [TypeConverter(typeof(CodeConverter))]
  public class CodeProperty : MaxProperty
  {          
    public CodeProperty(IMpmDelegates mpm,
      object subject, PropertyDescriptorCollection container) 
      : base(DataTypes.USER_CODE, Defaults.INITIAL_USER_CODE, false, mpm, subject, container)
    {
      this.description        = Defaults.USER_CODE_DESCRIPTION;
      this.category           = DataTypes.BASIC_PROPERTIES;
    }
  }  

  internal class CodeConverter : ExpandableObjectConverter
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
      if(typeof(CodeProperty) == destinationType)  return true;

      return base.CanConvertTo(context, destinationType);                                                       
    }
            
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return false;
    }

    public override object ConvertFrom(ITypeDescriptorContext context, 
      System.Globalization.CultureInfo culture, object value)
    {
      return value;
    }

    public override object ConvertTo(ITypeDescriptorContext context, 
      System.Globalization.CultureInfo culture, object value, Type destType )
    {
      if(value is CodeProperty && destType == typeof(string))
      {
        return Defaults.CUSTOM_CODE_PROPERTY_FILLER;
      }

      return base.ConvertTo(context,culture,value,destType);
    }  

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return false;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return false;
    } 
  }

  internal class CodeEditor : System.Drawing.Design.UITypeEditor
  {
    public CodeEditor() : base()
    {}

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      CodeProperty CodeProperty = context.PropertyDescriptor as CodeProperty;

      IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
      
      CSharpEditor editor =
        new CSharpEditor(CodeProperty.Mpm.GetUsings(), (value as MaxProperty).Value.ToString());

      edSvc.ShowDialog(editor);

      // Update usings if user selected ok
      if(editor.Ok)
        CodeProperty.Mpm.UpdateUsings(editor.Usings);
      
      // Update text value if user selected ok to exit
      object value_ = editor.Ok ? editor.Value : CodeProperty.Value;
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
}
