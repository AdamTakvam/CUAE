using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using PropertyGrid.Core;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for ActionParameterProperty.
  /// </summary>
  [TypeConverter(typeof(TriggerConverter))]
  public class TriggerProperty : MaxProperty
  {
    public new EventType Value  { 
        get { return (EventType) Enum.Parse(typeof(EventType), this.value_.ToString(), true); }
        set { this.value_ = value; } }

    public TriggerProperty(
        EventType value_, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(DataTypes.TRIGGER_NAME, value_, true, mpm, subject, container)
    {
      this.description = Defaults.TRIGGERING_PROPERTY_BASE_DESCRIPTION;
      this.category = DataTypes.BASIC_PROPERTIES;
    } 

    public override void Initialize(SimplePropertyType info)
    {
      try
      {
        this.value_ = (EventType) Enum.Parse(typeof(EventType), info.Value, true);
        this.oldValue = (EventType) Enum.Parse(typeof(EventType), info.Value, true);
      }
      catch
      {
        // TODO: WARNING
        this.value_ = EventType.nonTriggering;
        this.oldValue = EventType.nonTriggering;
      } 
    }
  }  

  internal class TriggerConverter : TypeConverter
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
      if(value is TriggerProperty && destType == typeof(string))
      {
        TriggerProperty property = (TriggerProperty)value;

        //Update Description
        if(property.Value == EventType.triggering)
        {
          property.SetDescription = Defaults.TRIGGERING_DESCRIPTION;
        }
        else if(property.Value == EventType.nonTriggering)
        {
          property.SetDescription = Defaults.NONTRIGGERING_DESCRIPTION;
        }
        else if(property.Value == EventType.hybrid)
        {
          property.SetDescription = Defaults.HYBRID_DESCRIPTION;
        }
                
        return property.Value.ToString();
      }

      return base.ConvertTo(context,culture,value,destType);
    }  

    // ReadOnly
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return false;
    }

    // Not a combo box
    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      return new StandardValuesCollection(System.Enum.GetNames(typeof(EventType)));
    }
  }

}
