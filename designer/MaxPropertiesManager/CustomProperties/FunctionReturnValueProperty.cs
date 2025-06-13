using System;
using System.ComponentModel;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for StyleProperty.
  /// </summary>
  [TypeConverter(typeof(FunctionReturnValueConverter))]
  public class FunctionReturnValueProperty : MaxProperty
  {
    public new string Category
    {
      get
      {
        return this.category;
      }
      set
      {
        this.category = value;
      }
    }

    public FunctionReturnValueProperty(
        string name, 
        string value_, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container)
        : base(name, value_, false, mpm, subject, container)
    {
      this.description = DataTypes.RETURN_VALUE_DESCRIPTION;
    } 
   
    public FunctionReturnValueProperty(
        string name, 
        string value_,  
        string description, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, false, mpm, subject, container)
    {
      this.description = description; 
    } 
  }

  internal class FunctionReturnValueConverter : ExpandableObjectConverter
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


    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return true;
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
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



    // We want a drop down box
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    // Not a combo box
    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      return new StandardValuesCollection(System.Enum.GetNames(typeof(DataTypes.ReturnValues)));
    }

  }
}
