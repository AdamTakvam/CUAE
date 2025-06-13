using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary> Represents an action parameter.  string editor, variable dropdown, and csharp editor </summary>
  [Editor(typeof(LoopCountEditor), typeof(System.Drawing.Design.UITypeEditor))]
  [TypeConverter(typeof(LoopCountConverter))]
  public class LoopCountProperty : MaxProperty
  {  
    #region Properties
    public new string Category { get { return this.category; } set { this.category = value; } }
    #endregion Properties
        
    public LoopCountProperty(IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) 
      : base(DataTypes.COUNT, Defaults.LOOP_COUNT, false, mpm, subject, container)
    {
      this.description        = Defaults.LOOP_COUNT_DESCRIPTION;
      this.category           = DataTypes.BASIC_PROPERTIES;
    }
  }  

  internal class LoopCountConverter : ExpandableObjectConverter
  {
    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
      object value, Attribute[] attributes)
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
      if(typeof(LoopCountProperty) == destinationType)  return true;

      return base.CanConvertTo(context, destinationType);                                                       
    }

            
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return true;
    }

    public override object ConvertFrom(ITypeDescriptorContext context, 
      System.Globalization.CultureInfo culture, object value)
    {
      return value;
    }

    public override object ConvertTo(ITypeDescriptorContext context, 
      System.Globalization.CultureInfo culture, object value, Type destType )
    {
      if(value is LoopCountProperty && destType == typeof(string))
      {
        LoopCountProperty parameter = (LoopCountProperty)value;
        return parameter.Value;
      }

      return base.ConvertTo(context,culture,value,destType);
    }  

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      LoopCountProperty loopCount = context.PropertyDescriptor as LoopCountProperty;
            
      for(int i = 0; i < loopCount.ChildrenProperties.Count; i++)
        if(loopCount.ChildrenProperties[i] is LoopTypeProperty)
        {
          LoopTypeProperty loopType = loopCount.ChildrenProperties[i] as LoopTypeProperty; 
          if(loopType.Value == DataTypes.LoopType.variable)   return true;
        }

      return false;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return false;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      LoopCountProperty loopCount = context.PropertyDescriptor as LoopCountProperty;

      object[] globalVarsObj    = loopCount.Mpm.GetGlobalVarsDelegate();
      object[] functionVarsObj  = loopCount.Mpm.GetFunctionVarsDelegate(loopCount.Subject);

      string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);
      string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);

      ArrayList allVars = MaxProperty.FormatVariablesList(globalVars, functionVars);

      return new StandardValuesCollection(allVars);
    }
  }

  internal class LoopCountEditor : System.Drawing.Design.UITypeEditor
  {
    public LoopCountEditor() : base()
    {}

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      LoopCountProperty loopCount = context.PropertyDescriptor as LoopCountProperty;

      IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
      
      LoopTypeProperty type = null;
      for(int i = 0; i < loopCount.ChildrenProperties.Count; i++)
        if(loopCount.ChildrenProperties[i] is LoopTypeProperty)
          type = loopCount.ChildrenProperties[i] as LoopTypeProperty;

      // Should never ever happen
      System.Diagnostics.Debug.Assert(type != null, "Code is out of date.");

      if(type.Value == DataTypes.LoopType.literal)
      {
          MaxProperty prop = value as MaxProperty;      

          LiteralEditor editor = new LiteralEditor(prop.Value as string);
          edSvc.ShowDialog(editor);

          string value_ = editor.Ok ? editor.Value : prop.Value as string;
          editor.Dispose();
          editor = null;
          return value_;
      }
      else
      {
        CSharpEditor editor =
          new CSharpEditor(loopCount.Mpm.GetUsings(), (value as MaxProperty).Value.ToString());

        edSvc.ShowDialog(editor);

        // Update usings if user selected ok
        if(editor.Ok)
          loopCount.Mpm.UpdateUsings(editor.Usings);
        
        // Update text value if user selected ok to exit
        object value_ = editor.Ok ? editor.Value : loopCount.Value;
        editor.Dispose();
        editor = null;
        return value_;
      }
    }

    public override bool GetPaintValueSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
    {
      LoopCountProperty loopCount = e.Context.PropertyDescriptor as LoopCountProperty;

      // Indicates that we have the drop down list to draw
      if(e.Value is string)
      {
        object[] functionVarsObj = loopCount.Mpm.GetFunctionVarsDelegate(loopCount.Subject);
        object[] globalVarsObj   = loopCount.Mpm.GetGlobalVarsDelegate();

        string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);
        string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);

        bool isLocal = false;
        if(functionVars != null)
          isLocal = 0 <= Array.IndexOf(functionVars, e.Value);

        bool isGlobal = false;
        if(globalVars != null)
          isGlobal = 0 <= Array.IndexOf(globalVars, e.Value);

        // Strictly a global var
        if(isGlobal && !isLocal)
          e.Graphics.DrawImage(PropertiesImageControl.glo, e.Bounds);

        // Special icon showing naming clash (which is allowed, but the local wins the scope contest
        if(isGlobal && isLocal)     
          e.Graphics.DrawImage(PropertiesImageControl.lg, e.Bounds);

        // Is only a local var
        if(!isGlobal && isLocal)
          e.Graphics.DrawImage(PropertiesImageControl.loc, e.Bounds);
      }
      else
      {     
        for(int i = 0; i < loopCount.ChildrenProperties.Count; i++)
          if(loopCount.ChildrenProperties[i] is LoopTypeProperty)
          {
            LoopTypeProperty userType = loopCount.ChildrenProperties[i] as LoopTypeProperty;
                    
            paramType type = Util.FromMaxToMetreosLoopType(userType.Value);

            if(type == paramType.variable)
              e.Graphics.DrawImage(PropertiesImageControl.var, e.Bounds);

            else if(type == paramType.literal)
              e.Graphics.DrawImage(PropertiesImageControl.str, e.Bounds);

            else if(type == paramType.csharp)
              e.Graphics.DrawImage(PropertiesImageControl.csp, e.Bounds);

            break;
          }
      }

      base.PaintValue (e);
    }

    public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return System.Drawing.Design.UITypeEditorEditStyle.Modal;
    }
  }
}
