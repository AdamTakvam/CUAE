using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>Enumerates allowable languages to write custom code in</summary>
  [TypeConverter(typeof(LanguageConverter))]
  public class LanguageProperty : MaxProperty
  {
    public new DataTypes.AllowableLanguages Value
    { 
        get { return (DataTypes.AllowableLanguages) Enum.Parse(typeof(DataTypes.AllowableLanguages), this.value_.ToString() , true); }
        set { this.value_ = value; }
    }
    public LanguageProperty(
        IMpmDelegates mpm,
        object subject, 
        PropertyDescriptorCollection container)
        : base(DataTypes.LANGUAGE, DataTypes.AllowableLanguages.csharp, false, mpm, subject, container)
    {
      this.description = Defaults.BASE_DESCRIPTION + Defaults.STRING_DESCRIPTION;
    }

    public override void Initialize(SimplePropertyType info)
    {
      try
      {
        this.value_ = (DataTypes.AllowableLanguages) Enum.Parse(typeof(DataTypes.AllowableLanguages), info.Value, true);
        this.oldValue = (DataTypes.AllowableLanguages) Enum.Parse(typeof(DataTypes.AllowableLanguages), info.Value, true);
      }
      catch
      {
        // TODO: WARNING
        this.value_ = DataTypes.AllowableLanguages.csharp;
        this.oldValue = DataTypes.AllowableLanguages.csharp;
      }  
    }
  }  

  internal class LanguageConverter : TypeConverter
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
      if(value is LanguageProperty && destType == typeof(string))
      {
        LanguageProperty property = (LanguageProperty)value;

        //Update Description
        if(property.Value == DataTypes.AllowableLanguages.csharp)
        {
          property.SetDescription = Defaults.BASE_LANGUAGE_DESCRIPTION + Defaults.CSHARP_LANGUAGE_DESCRIPTION;
        }
                
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
      return new StandardValuesCollection(System.Enum.GetNames(typeof(DataTypes.AllowableLanguages)));
    }
  }

}
