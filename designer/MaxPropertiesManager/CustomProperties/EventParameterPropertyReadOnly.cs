using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for EventParametersProperty.
  /// </summary>
  [TypeConverter(typeof(EventParameterReadOnlyConverter))]
  public class EventParameterPropertyReadOnly : MaxProperty
  {
    private bool isGuaranteed;

    #region Properties
        
    public bool IsGuaranteed
    {
      get
      {
        return this.isGuaranteed;
      }
      set
      {
        isGuaranteed = value;
      }
    }

    #endregion Properties

    public EventParameterPropertyReadOnly(string name, string displayName, string value_, bool isGuaranteed, 
      IMpmDelegates mpm, object subject, 
        PropertyDescriptorCollection container) 
      : base(name, displayName, value_, true, mpm, subject, container)
    {
      this.description = Defaults.GUARANTEED_PARAM_APPEND;
      this.isGuaranteed = isGuaranteed;
      this.category = DataTypes.EVENT_CATEGORY;
    }

    public EventParameterPropertyReadOnly(string name, string displayName, string value_, bool isGuaranteed, 
      string description, IMpmDelegates mpm, object subject, 
        PropertyDescriptorCollection container) 
      : base(name, displayName, value_, true, mpm, subject, container)
    {
      this.isGuaranteed = isGuaranteed;
      
      if(isGuaranteed)
      {
        string descrip = description != null ? description : String.Empty; 
        this.description = descrip + Defaults.blank + Defaults.GUARANTEED_PARAM_APPEND;
      }
      else
      {
        this.description = description != null ? description : String.Empty; 
      }
    }
  }

  internal class EventParameterReadOnlyConverter : TypeConverter
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

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if(typeof(EventParameterPropertyReadOnly) == destinationType)
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
			
      if(value is EventParameterPropertyReadOnly && destType == typeof(string))
      {
        EventParameterPropertyReadOnly parameter = (EventParameterPropertyReadOnly)value;

        return parameter.Value;
      }

      return base.ConvertTo(context,culture,value,destType);
    }  
  }
}
