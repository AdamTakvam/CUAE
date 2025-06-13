using System;
using System.ComponentModel;
using System.Collections;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for StandardProperty.
  /// </summary>
  [TypeConverter(typeof(ReferenceConverter))]
  public class ReferenceProperty : MaxProperty
  {       
    public new DataTypes.ReferenceType Value
    {
      get{ return (DataTypes.ReferenceType) Enum.Parse(typeof(DataTypes.ReferenceType), value_.ToString(), true); }
      set{ this.value_ = value; }
    }
        
    public ReferenceProperty(IMpmDelegates mpm,
      object subject, PropertyDescriptorCollection container) 
        : base(DataTypes.REFERENCE_TYPE_NAME, Defaults.REFERENCE_TYPE, false, mpm, subject, container)
    {
      this.category = DataTypes.BASIC_PROPERTIES;
    }

    public override void Initialize(SimplePropertyType info)
    {
      try
      {
        this.value_   = (DataTypes.ReferenceType) Enum.Parse(typeof(DataTypes.ReferenceType), info.Value, true);
        this.oldValue = (DataTypes.ReferenceType) Enum.Parse(typeof(DataTypes.ReferenceType), info.Value, true);
      }
      catch
      {
        // TODO: WARNING
        this.value_   = DataTypes.ReferenceType.reference;
        this.oldValue = DataTypes.ReferenceType.reference;
      }   
    }
  }

  internal class ReferenceConverter : ExpandableObjectConverter
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
        ReferenceProperty property = value as ReferenceProperty;

        if(property.Value == DataTypes.ReferenceType.reference)
        {
          property.SetDescription = Defaults.REFERENCE_TYPE_REFERENCE_DESC;
        }
        else if(property.Value == DataTypes.ReferenceType.value)
        {
          property.SetDescription = Defaults.REFERENCE_TYPE_VALUE_DESC;
        }

        return property.Value.ToString();
      }

      return base.ConvertTo(context,culture,value,destType);;
    }  

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      return new StandardValuesCollection( System.Enum.GetNames(typeof(DataTypes.ReferenceType)) );
    }
  }
}
