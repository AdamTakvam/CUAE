using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;


namespace Metreos.Max.Framework.Satellite.Property
{
  [TypeConverter(typeof(EventParamTypeConverter))]
  public class EventParamTypeProperty : MaxProperty
  {
    public new DataTypes.EventParamType Value
    {
      get{ return (DataTypes.EventParamType) Enum.Parse(typeof(DataTypes.EventParamType), this.value_.ToString(), true); }
      set{ this.value_ = value; }
    }

    public EventParamTypeProperty(IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) 
      : base(DataTypes.EVENT_PARAM_TYPE, Defaults.EVENT_PARAM_TYPE, false, mpm, subject, container)
    {
      this.description = Defaults.EVENT_PARAM_TYPE_DESC;
    }

    public override void Initialize(SimplePropertyType info)
    {
      try
      {
        this.value_   = (DataTypes.EventParamType) Enum.Parse(typeof(DataTypes.EventParamType), info.Value, true);
        this.oldValue = (DataTypes.EventParamType) Enum.Parse(typeof(DataTypes.EventParamType), info.Value, true);
      }
      catch
      {
        // TODO: WARNING
        this.value_   = DataTypes.EventParamType.literal;
        this.oldValue = DataTypes.EventParamType.literal;
      }  
    }
  }  

  internal class EventParamTypeConverter : TypeConverter
  { 

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
      if(value is EventParamTypeProperty && destType == typeof(string))
      {
        EventParamTypeProperty property = value as EventParamTypeProperty;

        return property.Value.ToString();
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
      return new StandardValuesCollection(System.Enum.GetNames(typeof(DataTypes.EventParamType)));
    }

  }

}
