using System;
using System.ComponentModel;
using System.Collections;


namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for StandardProperty.
  /// </summary>
  [Editor(typeof(LoggingPropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
  [TypeConverter(typeof(LoggingConverter))]
  public class LoggingProperty : MaxProperty
  {       
    public new string Category { get { return category; } set { category = value; } }
    public new DataTypes.OnOff Value
    {
      get{ return (DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), this.value_.ToString(), true); }
      
      set{ this.value_ = value; }
    }

    public LoggingProperty(
        DataTypes.OnOff value_, 
        IMpmDelegates mpm,
        object subject, PropertyDescriptorCollection container) 
        : base(DataTypes.LOGGING, value_, false, mpm, subject, container)
    {
      this.category = DataTypes.LOGGING;
    }

    public LoggingProperty(
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

  internal class LoggingConverter : ExpandableObjectConverter
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
      if(value is LoggingProperty && destType == typeof(string))
      {
        LoggingProperty property = (LoggingProperty)value;
         
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
      return new StandardValuesCollection( System.Enum.GetNames( typeof( DataTypes.OnOff ) ));
    }
  }

  internal class LoggingPropertyEditor : System.Drawing.Design.UITypeEditor
  {
    public LoggingPropertyEditor() : base()
    {}

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      return base.EditValue(context, provider, value);    
    }

    public override bool GetPaintValueSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
    {
      LoggingProperty loggingProperty = (LoggingProperty)e.Context.PropertyDescriptor;	
	
      if(loggingProperty.Value.ToString() == DataTypes.OnOff.On.ToString())
        e.Graphics.DrawImage(PropertiesImageControl.on, e.Bounds);

      else if(loggingProperty.Value.ToString() == DataTypes.OnOff.Off.ToString())
        e.Graphics.DrawImage(PropertiesImageControl.off, e.Bounds);

      base.PaintValue (e);
    }

    public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return System.Drawing.Design.UITypeEditorEditStyle.None;
    }
  }
}
