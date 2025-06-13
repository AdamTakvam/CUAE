using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary> Acts as the initWith property in local variables.
  /// Its behavior is marked by a dropdown with all applicable 
  /// event params for the function that the variable is contained in </summary>
  [TypeConverter(typeof(EventParameterInitWithPropertyConverter))]
  public class EventParameterInitWithProperty : MaxProperty
  {
    public EventParamTypeProperty Type { get { return type; } set { type = value; } }
    private EventParamTypeProperty type;

    public EventParameterInitWithProperty(EventParamTypeProperty type, IMpmDelegates mpm, object subject, PropertyDescriptorCollection container)
        : base(DataTypes.CONFIG_INIT_WITH, 
      String.Empty, false, mpm, subject, container)
    {
      this.type                 = type;
      this.category             = DataTypes.BASIC_PROPERTIES;
      this.description          = Defaults.CONFIG_INIT_WITH_DESC;
    }
  }  

  internal class EventParameterInitWithPropertyConverter : TypeConverter
  {
    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
      return new PropertyDescriptorCollection(null);
    }

    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
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
      EventParameterInitWithProperty eventInitWith = context.PropertyDescriptor as EventParameterInitWithProperty;

      return eventInitWith.Type.Value == DataTypes.EventParamType.variable;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return false;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      EventParameterInitWithProperty eventInitWith = context.PropertyDescriptor as EventParameterInitWithProperty;
     
      return new StandardValuesCollection(eventInitWith.Mpm.GetConfigParameters());
    }
  }
}
