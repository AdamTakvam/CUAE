using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;


namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary> Output parameters of actions </summary>
  [Editor(typeof(ResultDataEditor), typeof(System.Drawing.Design.UITypeEditor))]
  [TypeConverter(typeof(ResultDataConverter))]
  public class ResultDataProperty : MaxProperty
  {
    public ResultDataProperty(
        string name, 
        string displayName,
        string value_, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, displayName, value_, false, mpm, subject, container)
    {
      this.category = DataTypes.RESULT_DATA_CATEGORY;
      this.description = DataTypes.RESULT_DATA_DESCRIPTION;
    }

    public ResultDataProperty(
        string name, 
        string displayName,
        string value_,  
        string description, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, displayName, value_, false, mpm, subject, container)
    {
      this.category = DataTypes.RESULT_DATA;
      this.description = description;
    }
  }  

  internal class ResultDataConverter : ExpandableObjectConverter
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
      if(typeof(ResultDataProperty) == destinationType)
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
      if(value is ResultDataProperty && destType == typeof(string))
      {
        ResultDataProperty parameter = (ResultDataProperty)value;

        return parameter.Value;
      }

      return base.ConvertTo(context,culture,value,destType);
    }  

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      ResultDataProperty returnValueProperty = (ResultDataProperty) context.PropertyDescriptor;

      object[] globalVarsObj    = returnValueProperty.Mpm.GetGlobalVarsDelegate();
      object[] functionVarsObj  = returnValueProperty.Mpm.GetFunctionVarsDelegate(returnValueProperty.Subject);
      string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);
      string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);

      ArrayList allVars = MaxProperty.FormatVariablesList(globalVars, functionVars);

      return new StandardValuesCollection(allVars);
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return false;
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }
  }

  internal class ResultDataEditor : System.Drawing.Design.UITypeEditor
  {
    public ResultDataEditor() : base()
    {}
   
    public override bool GetPaintValueSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
    {
      ResultDataProperty returnValueProperty = (ResultDataProperty) e.Context.PropertyDescriptor;

      string @value;
      if(e.Value is String)  @value = e.Value as string;
      else                   @value = returnValueProperty.Value as string;

      object[] functionVarsObj = returnValueProperty.Mpm.GetFunctionVarsDelegate(returnValueProperty.Subject);
      object[] globalVarsObj   = returnValueProperty.Mpm.GetGlobalVarsDelegate();

      string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);
      string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);

      bool isLocal = false;
      if(functionVars != null)
        isLocal = 0 <= Array.IndexOf(functionVars, @value);

      bool isGlobal = false;
      if(globalVars != null)
        isGlobal = 0 <= Array.IndexOf(globalVars, @value);

      // Strictly a global var
      if(isGlobal && !isLocal)
        e.Graphics.DrawImage(PropertiesImageControl.glo, e.Bounds);

      // Special icon showing naming clash (which is allowed, but the local wins the scope contest
      if(isGlobal && isLocal)     
        e.Graphics.DrawImage(PropertiesImageControl.lg, e.Bounds);

      // Is only a local var
      if(!isGlobal && isLocal)
        e.Graphics.DrawImage(PropertiesImageControl.loc, e.Bounds); 

      base.PaintValue (e);
    }

    public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return System.Drawing.Design.UITypeEditorEditStyle.Modal;
    }
  }

}
