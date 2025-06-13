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
    [Editor(typeof(MediaFileEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(MediaFileConverter))]
    public class MediaFileChooserProperty : ActionParameterProperty
    {  
        public enum MediaFileType { audio, grammar, voicerec };
        protected MediaFileType subtype;
        public    MediaFileType Subtype { get { return subtype; } }

        public MediaFileChooserProperty
        (   MediaFileType subtype, string name, string displayName, string value_, 
            bool isRequired, bool allowMultiple, string descr, IMpmDelegates mpm,
            object subj, PropertyDescriptorCollection container) 

          : base(name, displayName, value_, isRequired, allowMultiple, descr, 
                 null, mpm, subj, container
        )
        {
            this.subtype = subtype;
        }  
    }

  internal class MediaFileConverter : ExpandableObjectConverter
  {

    public override PropertyDescriptorCollection GetProperties
    ( ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
        if (value is MaxProperty)
        {
            MaxProperty property = (MaxProperty) value;
            return property.ChildrenProperties;
        }

        return base.GetProperties (context, value, attributes);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if(typeof(MediaFileChooserProperty) == destinationType)  return true;

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
      if(value is MediaFileChooserProperty && destType == typeof(string))
      {
        MediaFileChooserProperty parameter = (MediaFileChooserProperty)value;
        return parameter.Value;
      }

      return base.ConvertTo(context,culture,value,destType);
    }  

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      MediaFileChooserProperty actionProperty = context.PropertyDescriptor as MediaFileChooserProperty;
            
      foreach(MaxProperty property in actionProperty.ChildrenProperties)
      {
        if(!(property is UserTypeProperty))  continue;
        UserTypeProperty userType = property as UserTypeProperty;
        if(userType.Value == DataTypes.UserVariableType.variable)   return true;
        if(userType.Value == DataTypes.UserVariableType.literal)    return true;  
      }

      return false;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return false;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      DataTypes.UserVariableType type = DataTypes.UserVariableType.literal;

      MediaFileChooserProperty actionProperty = context.PropertyDescriptor as MediaFileChooserProperty;

      foreach(MaxProperty property in actionProperty.ChildrenProperties)
      {
        if(!(property is UserTypeProperty))  continue;
        UserTypeProperty userType = property as UserTypeProperty;
        type = userType.Value;
      }

      if(type == DataTypes.UserVariableType.literal)
      {
        string[] mediaFiles = null;

        switch(actionProperty.Subtype)   // 0120
        {
            case MediaFileChooserProperty.MediaFileType.audio:
                 mediaFiles = actionProperty.Mpm.GetMediaFiles();
                 break;
            case MediaFileChooserProperty.MediaFileType.grammar:
                 mediaFiles = actionProperty.Mpm.GetGrammarFiles();
                 break;           
            case MediaFileChooserProperty.MediaFileType.voicerec:
                 mediaFiles = actionProperty.Mpm.GetVoiceRecFiles();
                 break;
        }

        return mediaFiles == null? null: new StandardValuesCollection(mediaFiles);
      }
      else if(type == DataTypes.UserVariableType.variable)
      {
        object[] globalVarsObj   = actionProperty.Mpm.GetGlobalVarsDelegate();
        object[] functionVarsObj = actionProperty.Mpm.GetFunctionVarsDelegate(actionProperty.Subject);

        string[] globalVars   = MaxProperty.ParseVarsFromFramework(globalVarsObj);
        string[] functionVars = MaxProperty.ParseVarsFromFramework(functionVarsObj);

        ArrayList allVars = MaxProperty.FormatVariablesList(globalVars, functionVars);

        return new StandardValuesCollection(allVars);
      }
      else
      {
        return null;
      }
    }
  }

  internal class MediaFileEditor : StringEditorWithIcon 
  {
    public MediaFileEditor() : base()
    {}

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
      ActionParameterProperty actionProperty = context.PropertyDescriptor as ActionParameterProperty;

      IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
      
      UserTypeProperty type = null;
      for(int i = 0; i < actionProperty.ChildrenProperties.Count; i++)
        if(actionProperty.ChildrenProperties[i] is UserTypeProperty)
          type = actionProperty.ChildrenProperties[i] as UserTypeProperty;

      // Should never ever happen
      if(type == null)
        throw new Exception("Code is out of date");

      if(type.Value == DataTypes.UserVariableType.literal)
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
          new CSharpEditor(actionProperty.Mpm.GetUsings(), (value as MaxProperty).Value.ToString());

        edSvc.ShowDialog(editor);

        // Update usings if user selected ok
        if(editor.Ok)
          actionProperty.Mpm.UpdateUsings(editor.Usings);
        
        // Update text value if user selected ok to exit
        object value_ = editor.Ok ? editor.Value : actionProperty.Value;
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
           
      ActionParameterProperty actionProperty = (ActionParameterProperty)e.Context.PropertyDescriptor;
            
      for(int i = 0; i < actionProperty.ChildrenProperties.Count; i++)
        if(actionProperty.ChildrenProperties[i].GetType() == typeof(UserTypeProperty))
        {
          UserTypeProperty userType = (UserTypeProperty) actionProperty.ChildrenProperties[i];
                    
          paramType type = Util.FromMaxToMetreosActPar(userType.Value);

          if(type == paramType.variable)
            e.Graphics.DrawImage(PropertiesImageControl.var, e.Bounds);

          else if(type == paramType.literal)
            e.Graphics.DrawImage(PropertiesImageControl.str, e.Bounds);

          else if(type == paramType.csharp)
            e.Graphics.DrawImage(PropertiesImageControl.csp, e.Bounds);
        }

      base.PaintValue (e);
    }

    public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return System.Drawing.Design.UITypeEditorEditStyle.Modal;
    }
  }
}
