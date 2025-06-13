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
  /// <summary>
  /// Summary description for ActionParameterProperty.
  /// </summary>
  [Editor(typeof(ResultDataPropertyAttribute), typeof(System.Drawing.Design.UITypeEditor))]
  [TypeConverter(typeof(ActionParameterGrowable2Converter))]
  public class ResultDataPropertyGrowable : MaxProperty
  {
    public ResultDataPropertyGrowable(
      IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) : 
      base(DataTypes.RESULT_DATA_GROWABLE_NAME, String.Empty, false, mpm, subject, container)
    {
      this.category = DataTypes.RESULT_DATA_CATEGORY;
    }
  }  


   
  internal class ResultDataPropertyAttribute : System.Drawing.Design.UITypeEditor
  {
    public ResultDataPropertyAttribute() : base()
    {}

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      ResultDataPropertyGrowable actionProperty = context.PropertyDescriptor as ResultDataPropertyGrowable;
      ResultDataGrowableEditor editor = null;
      // get the editor service.
      IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

      if (edSvc == null) 
      {
        return value;
      }

      if (editor == null) 
      {
        editor = new ResultDataGrowableEditor(context, edSvc, value, actionProperty.Mpm.RemovePropertyDelegate);
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
