using System;
using System.ComponentModel;
using System.Collections;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary> </summary>
  [TypeConverter(typeof(TraceLevelConverter))]
  public class TraceLevelProperty : MaxProperty
  {       
    public new System.Diagnostics.TraceLevel Value
    {
      get { return (System.Diagnostics.TraceLevel)Enum.Parse(typeof(System.Diagnostics.TraceLevel), this.value_.ToString(), true); }
      set { this.value_ = value; }
    }

    public TraceLevelProperty(
        System.Diagnostics.TraceLevel value_, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(DataTypes.TRACELEVEL, value_, false, mpm, subject, container)
    {
      this.category = DataTypes.ACTION_PARAMETERS_CATEGORY;
    }

    public TraceLevelProperty(
        string name, 
        System.Diagnostics.TraceLevel value_, 
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
            this.value_ = (System.Diagnostics.TraceLevel) Enum.Parse(typeof(System.Diagnostics.TraceLevel), info.Value, true);
            this.oldValue = (System.Diagnostics.TraceLevel) Enum.Parse(typeof(System.Diagnostics.TraceLevel), info.Value, true);
        }
        catch
        {
            // TODO: WARNING
            this.value_ = System.Diagnostics.TraceLevel.Info;
            this.oldValue = System.Diagnostics.TraceLevel.Info;
        }
    }
  }

  internal class TraceLevelConverter : ExpandableObjectConverter
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
        TraceLevelProperty property = (TraceLevelProperty)value;

        if(property.Value == System.Diagnostics.TraceLevel.Error)
        {
          property.SetDescription = Defaults.TRACE_LEVEL_DESCRIPTION_BASE + Defaults.TRACE_LEVEL_ERROR;
        }
        else if(property.Value == System.Diagnostics.TraceLevel.Warning)
        {
          property.SetDescription = Defaults.TRACE_LEVEL_DESCRIPTION_BASE + Defaults.TRACE_LEVEL_WARNING;
        }
        else if(property.Value == System.Diagnostics.TraceLevel.Info)
        {
          property.SetDescription = Defaults.TRACE_LEVEL_DESCRIPTION_BASE + Defaults.TRACE_LEVEL_INFO;
        }
        else if(property.Value == System.Diagnostics.TraceLevel.Verbose)
        {
          property.SetDescription = Defaults.TRACE_LEVEL_DESCRIPTION_BASE + Defaults.TRACE_LEVEL_VERBOSE;
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
      return new StandardValuesCollection( System.Enum.GetNames(typeof(System.Diagnostics.TraceLevel)) );
    }
  }
}
