using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.WebServicesConsumerCore;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary> Represents an action parameter.  string editor, variable dropdown, and csharp editor </summary>
  [TypeConverter(typeof(ComplexTypeConverter))]
  public class ComplexTypeProperty : MaxProperty
  { 
    public static ComplexTypeProperty FindComplexParent(string name, ComplexTypeProperty[] properties)
    {
      if(properties == null || properties.Length == 0)  return null;

      int index = name.IndexOf(NativeActionAssembler.heirarchySeperator);
      if(index < 0) return null;

      string parentName = name.Substring(0,index);

      foreach(ComplexTypeProperty property in properties)
        if(property.Name == parentName)
          return property;

      return null;
    }

    public ComplexTypeProperty(string name, IMpmDelegates mpm, 
      object subject, PropertyDescriptorCollection container) 
      : base(name, String.Empty, true, mpm, subject, container)
    {
      this.description        = Defaults.COMPLEX_TYPE_DESCRIPTION;
      this.category           = DataTypes.ACTION_PARAMETERS_CATEGORY;
    }
  }  

  internal class ComplexTypeConverter : ExpandableObjectConverter
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
      if(typeof(ComplexTypeProperty) == destinationType)  return true;

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
      if(value is ComplexTypeProperty && destType == typeof(string))
      {
        return FormatCount(value as ComplexTypeProperty);
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

    public string FormatCount(ComplexTypeProperty property)
    {
      if(property.ChildrenProperties.Count == 1)
        return String.Format("({0} nested parameter)", property.ChildrenProperties.Count);
      else
        return String.Format("({0} nested parameters)", property.ChildrenProperties.Count);
    }
  } 
}
