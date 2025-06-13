using System;
using System.ComponentModel;
using System.Collections;



namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for StandardProperty.
  /// </summary>
  [TypeConverter(typeof(RenamingPropertyConverter))]
  public class RenamingProperty : MaxProperty
  {       
    public MaxProperty parent;
        
    public RenamingProperty(
        string value_, 
        MaxProperty parent, 
        IMpmDelegates mpm, 
        object subject,
        PropertyDescriptorCollection container) 
        : base(DataTypes.NAME, value_, false, mpm, subject, container)
    {
      this.parent = parent;
    }

    public RenamingProperty(
        string value_, 
        MaxProperty parent, 
        string category, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(DataTypes.NAME, value_, false, mpm, subject, container)
    {
      this.parent = parent;
    }
    
      public override void AfterSetValue(object value)
      {
          this.parent.SetName = value as string;
      }
  }

  internal class RenamingPropertyConverter : TypeConverter
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
