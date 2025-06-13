using System;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;



namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for StandardProperty.
  /// </summary>
  [Editor(typeof(GrowableLogEditor), typeof(System.Drawing.Design.UITypeEditor))]
  [TypeConverter(typeof(OnLogEventGrowableConverter))]
  public class OnLogEventPropertyGrowable : MaxProperty
  {    
    public OnLogEventPropertyGrowable(IMpmDelegates mpm,
      object subject, PropertyDescriptorCollection container)
        : base(DataTypes.LOGGING_GROWABLE, String.Empty, false, mpm, subject, container)
    {
      this.category           = DataTypes.LOGGING;
    }
  }

  internal class OnLogEventGrowableConverter : ExpandableObjectConverter
  {
    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
      MaxProperty property = context.PropertyDescriptor as MaxProperty;
      return property.ChildrenProperties.Count != 0;
    }

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
      MaxProperty property = (MaxProperty) context.PropertyDescriptor;

      return property.ChildrenProperties.Count + " defined";
    }  

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return false;
    }
  }

  internal class GrowableLogEditor : System.Drawing.Design.UITypeEditor
  {
    public GrowableLogEditor() : base()
    {}

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      OnLogEventGrowableEditor editor = null;
      // get the editor service.
      IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

      if (edSvc == null) 
      {
        return value;
      }

      if (editor == null) 
      {
        OnLogEventPropertyGrowable logProperty = context.PropertyDescriptor as OnLogEventPropertyGrowable;
        editor = new OnLogEventGrowableEditor(context, edSvc, value, logProperty.Mpm.RemovePropertyDelegate);
      }
				
      // instruct the editor service to display the control as a dropdown.
      edSvc.DropDownControl(editor);
		
      MaxProperty property = (MaxProperty) value;

      property.ValueChanged(false);

      editor.Dispose();
      editor = null;

      // return the updated value;
      return value;
    }

    public override bool GetPaintValueSupported(ITypeDescriptorContext context)
    {
      return false;
    }

    public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
    {
      base.PaintValue (e);
    }

    public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
    }
  }
}
