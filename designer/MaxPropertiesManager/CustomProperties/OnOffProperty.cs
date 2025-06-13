using System;
using System.ComponentModel;
using System.Collections;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary></summary>
  [TypeConverter(typeof(OnOffConverter))]
  public class OnOffProperty : MaxProperty
  {       
    public new DataTypes.OnOff Value
    {
      get{ return (DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), this.value_.ToString(), true); }
      set{ this.value_ = value; }
    }
        
    public OnOffProperty(
        DataTypes.OnOff value_, 
        IMpmDelegates mpm,
        object subject, 
        PropertyDescriptorCollection container) 
        : base(DataTypes.LOGGING, value_, false, mpm, subject, container)
    {
      this.category = DataTypes.LOGGING;
    }

    public OnOffProperty(
        string name, 
        DataTypes.OnOff value_, 
        bool isReadOnly, 
        string category, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, isReadOnly, mpm, subject, container)
    {
      this.category = category;
    }

    public override void Initialize(SimplePropertyType info)
    {
      try
      {
        this.value_ = (DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), info.Value, true);
        this.oldValue = (DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), info.Value, true);
      }
      catch
      {
        // TODO: WARNING
        this.value_ = DataTypes.OnOff.Off;
        this.oldValue = DataTypes.OnOff.Off;
      }   
    }
  }

  internal class OnOffConverter : ExpandableObjectConverter
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

        if(property.Value.ToString() == DataTypes.OnOff.On.ToString())
        {
          property.SetDescription = Defaults.ON_OFF_DESCRIPTION_BASE + Defaults.ON_OFF_IS_ON;
        }
        else if(property.Value.ToString() == DataTypes.OnOff.Off.ToString())
        {
          property.SetDescription = Defaults.ON_OFF_DESCRIPTION_BASE + Defaults.ON_OFF_IS_OFF;
        }

        return property.Value.ToString();
      }

      return base.ConvertTo(context,culture,value,destType);
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
      return new StandardValuesCollection( System.Enum.GetNames(typeof(DataTypes.OnOff)) );
    }
  }
}
