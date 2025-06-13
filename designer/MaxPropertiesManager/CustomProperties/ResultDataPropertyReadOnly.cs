using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;



namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for ActionParameterProperty.
  /// </summary>
  [TypeConverter(typeof(ResultDataReadOnlyConverter))]
  public class ResultDataPropertyReadOnly : MaxProperty
  {
    public ResultDataPropertyReadOnly(
        string name,
        string displayName,
        string value_, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, true, mpm, subject, container)
    {
            
    }
  }  

  internal class ResultDataReadOnlyConverter : ExpandableObjectConverter
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
      if(typeof(ResultDataPropertyReadOnly) == destinationType)
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
      if(value is ResultDataPropertyReadOnly && destType == typeof(string))
      {
        ResultDataPropertyReadOnly parameter = (ResultDataPropertyReadOnly)value;

        return parameter.Value;
      }

      return base.ConvertTo(context,culture,value,destType);
    }  
  }

}
