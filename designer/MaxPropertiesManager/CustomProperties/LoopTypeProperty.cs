using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;


namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for LoopTypeProperty.
  /// </summary>
  [TypeConverter(typeof(LoopTypeConverter))]
  public class LoopTypeProperty : MaxProperty
  {
    public new DataTypes.LoopType Value
    {
      get { return (DataTypes.LoopType) Enum.Parse(typeof(DataTypes.LoopType), this.value_.ToString(), true); } 
      set { this.value_ = value; }
    }

    public LoopTypeProperty(IMpmDelegates mpm, object subject,
        PropertyDescriptorCollection container)
        : base(DataTypes.LOOP_TYPE_NAME, DataTypes.LoopType.literal, false, mpm, subject, container)
    {
      this.description = Defaults.BASE_DESCRIPTION + Defaults.INTEGER_DESCRIPTION;
    }

    public LoopTypeProperty(string value_, string description,
        IMpmDelegates mpm, object subject, PropertyDescriptorCollection container)
        : base(DataTypes.LOOP_TYPE_NAME, value_, false, mpm, subject, container)
    {
      this.description = description;
    }

    public override void Initialize(SimplePropertyType info)
    {
      try
      {
        this.value_ = ( DataTypes.LoopType) Enum.Parse(typeof(DataTypes.LoopType), info.Value, true);
        this.oldValue = (DataTypes.LoopType) Enum.Parse(typeof(DataTypes.LoopType), info.Value, true);
      }
      catch
      {
        // TODO: WARNING
        this.value_ = DataTypes.LoopType.literal;
        this.oldValue = DataTypes.LoopType.literal;
      }  
    }
  }  

  internal class LoopTypeConverter : TypeConverter
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
      if(value is LoopTypeProperty && destType == typeof(string))
      {
        LoopTypeProperty property = (LoopTypeProperty)value;

        //Update Description
        if(property.Value.ToString() == DataTypes.LoopType.literal.ToString())
        {
          property.SetDescription = Defaults.BASE_DESCRIPTION + Defaults.INTEGER_DESCRIPTION;
        }
        else if(property.Value.ToString() == DataTypes.LoopType.csharp.ToString())
        {
          property.SetDescription = Defaults.BASE_DESCRIPTION + Defaults.CSHARP_DESCRIPTION;
        }
        else if(property.Value.ToString() == DataTypes.LoopType.variable.ToString())
        {
          property.SetDescription = Defaults.BASE_DESCRIPTION + Defaults.VARIABLE_DESCRIPTION;
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
      return new StandardValuesCollection(System.Enum.GetNames(typeof(DataTypes.LoopType)));
    }
  }

}
