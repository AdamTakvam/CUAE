using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;   



namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Summary description for StandardProperty.
  /// </summary>
  [Editor(typeof(GenericIconEditor), typeof(System.Drawing.Design.UITypeEditor))]
  [TypeConverter(typeof(GenericPropertyConverter))]
  public class GenericPropertyWithIcon : MaxProperty
  {       
    Bitmap icon;

    public Bitmap Icon
    {
      get
      {
        return icon;
      }
      set
      {
        icon = value;
      }
    }
        
    public GenericPropertyWithIcon(
        string name, 
        string value_, 
        bool isReadOnly, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, isReadOnly, mpm, subject, container)
    {
			
    }

    public GenericPropertyWithIcon(
        string name, 
        string value_, 
        bool isReadOnly, 
        Bitmap icon, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, isReadOnly, mpm, subject, container)
    {
      this.icon = icon;
    }
  }


  internal class GenericIconEditor : System.Drawing.Design.UITypeEditor
  {
    public GenericIconEditor() : base()
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
      e.Graphics.DrawImage(((GenericPropertyWithIcon) e.Context.PropertyDescriptor).Icon, e.Bounds);

      base.PaintValue (e);
    }

    public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return System.Drawing.Design.UITypeEditorEditStyle.None;
    }
  }
}
