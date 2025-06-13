using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary> Acts as the initWith property in local variables.
  /// Its behavior is marked by a dropdown with all applicable 
  /// event params for the function that the variable is contained in </summary>
  [TypeConverter(typeof(GlobalVarInitWithPropertyConverter))]
  public class GlobalVarInitWithProperty : MaxProperty
  {      
    public GlobalVarInitWithProperty(string value_, IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) 
        : base(DataTypes.CONFIG_INIT_WITH, value_, false, mpm, subject, container)
    {
      this.displayName = DataTypes.LOCAL_VAR_INIT_WITH;
      this.category = DataTypes.BASIC_PROPERTIES;
      this.description = Defaults.CONFIG_INIT_WITH_DESC;
    }
  }  

  internal class GlobalVarInitWithPropertyConverter : TypeConverter
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
      return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return false;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
          GlobalVarInitWithProperty variableInitWithProperty = context.PropertyDescriptor as GlobalVarInitWithProperty;

          string[] configs = variableInitWithProperty.Mpm.GetConfigParameters();
          if (configs != null)
          {
              for(int i = 0; i < configs.Length; i++)
              {
                  configs[i] = Defaults.ConfigPrepend + Defaults.dot + configs[i];
              }
          }

          string[] locales = variableInitWithProperty.Mpm.GetLocaleStrings();
          if (locales != null)
          {
              for (int i = 0; i < locales.Length; i++)
              {
                  locales[i] = Defaults.LocalePrepend + Defaults.dot + locales[i];
              }
          }

          List<string> list = new List<string>();
          if (configs != null)
          {
              list.AddRange(configs);
          }
          if (locales != null)
          {
              list.AddRange(locales);
          }

          return new StandardValuesCollection(list);
    }
  }
}
