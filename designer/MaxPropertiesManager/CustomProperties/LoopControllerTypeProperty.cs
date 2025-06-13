using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;


namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary> Enumerates for user different types of loop iteration control </summary>
  [TypeConverter(typeof(LoopControllerTypeConverter))]
  public class LoopControllerTypeProperty : MaxProperty
  {
    public new DataTypes.LoopIterateType Value
    {
      get { return (DataTypes.LoopIterateType) Enum.Parse(typeof(DataTypes.LoopIterateType), this.value_.ToString(), true); }
      set { this.value_ = value; }
    }

    public LoopControllerTypeProperty(IMpmDelegates mpm, object subject,
        PropertyDescriptorCollection container)
        : base(DataTypes.LOOP_ITERATE_TYPE_NAME, Defaults.LOOP_ITERATE_TYPE, false, mpm, subject, container)
    {
      this.description = Defaults.ITERATE_BASE_DESCRIPTION + Defaults.INT_DESCRIPTION;
    }

    public override void Initialize(SimplePropertyType info)
    {
      try
      {
        this.value_   = (DataTypes.LoopIterateType) Enum.Parse(typeof(DataTypes.LoopIterateType), info.Value, true);
        this.oldValue = (DataTypes.LoopIterateType) Enum.Parse(typeof(DataTypes.LoopIterateType), info.Value, true);
      }
      catch
      {
        // TODO: WARNING
        this.value_ = DataTypes.LoopIterateType.@int;
        this.oldValue = DataTypes.LoopIterateType.@int;
      }  
    }
  }  

  internal class LoopControllerTypeConverter : TypeConverter
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
      if(value is LoopControllerTypeProperty && destType == typeof(string))
      {
        LoopControllerTypeProperty property = (LoopControllerTypeProperty)value;

        //Update Description
        if(property.Value.ToString() == DataTypes.LoopIterateType.@int.ToString())
        {
          property.SetDescription = Defaults.ITERATE_BASE_DESCRIPTION + Defaults.INT_DESCRIPTION;
        }
        else if(property.Value.ToString() == DataTypes.LoopIterateType.@enum.ToString())
        {
          property.SetDescription = Defaults.ITERATE_BASE_DESCRIPTION + Defaults.ENUM_DESCRIPTION;
        }
        else if(property.Value.ToString() == DataTypes.LoopIterateType.dictEnum.ToString())
        {
          property.SetDescription = Defaults.ITERATE_BASE_DESCRIPTION + Defaults.DICT_ENUM_DESCRIPTION;
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
      return new StandardValuesCollection(System.Enum.GetNames(typeof(DataTypes.LoopIterateType)));
    }
  }

}
