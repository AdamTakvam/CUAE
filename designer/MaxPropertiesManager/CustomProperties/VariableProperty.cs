using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for ActionParameterProperty.
    /// </summary>
    [TypeConverter(typeof(VariablePropertyConverter))]
    public class VariableProperty : MaxProperty
    {     
        public string VariableDisplayName { get { return variableDisplayName; } set { variableDisplayName = value; } }
        public string VariableName { get { return base.value_ as string; } set { base.value_ = value; } }
        private string variableDisplayName;

        public VariableProperty(IMpmDelegates mpm, object subject,
            PropertyDescriptorCollection container) 
            : base(DataTypes.VARIABLE_TYPE, Defaults.TYPE, false, mpm, subject, container)
        {
            this.category = DataTypes.BASIC_PROPERTIES;
            this.description = Defaults.VARIABLE_TYPE_DESCRIPTION;
        }

        public override void AfterSetValue(object value)
        {
            // When the user selects from the dropdown of native type display names,
            // we save the real name of the type chosen.  Using this real name, we then 
            // serialize this real name to script XML. Conversely, when loading the application,
            // we check for a native type of this name,
            // and use the display name in the property grid.

            if(value.GetType() == typeof(string))
            {
                string displayName = value as string;

                nativeTypePackageType[] allNativeTypes =  this.Mpm.GetNativeTypesInfoDelegate();

                if(allNativeTypes == null || allNativeTypes.Length == 0)
                {
                    this.VariableDisplayName = displayName;
                    this.VariableName= displayName;
                }
                else
                {
                    string matchedType = null;
                    foreach(nativeTypePackageType nativeType in allNativeTypes)
                    {
                        if(nativeType.type == null || nativeType.type.Length == 0)
                            continue;

                        if(matchedType != null) break;

                        foreach(typeType type in nativeType.type)
                        {
                            if(String.Compare(displayName, type.displayName, true) == 0)
                            {
                                matchedType = Util.MakeFullyQualified(nativeType.name, type.name);
                                break;
                            }
                        }
                    }

                    if(matchedType != null)
                    {
                        this.VariableDisplayName = displayName;
                        this.VariableName= matchedType;
                    }
                    else
                    {
                        this.VariableDisplayName = displayName;
                        this.VariableName= displayName;
                    }
                }
            }
        }
    }  

    internal class VariablePropertyConverter : TypeConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(null);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
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

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
        {
            if(value is MaxProperty && destType == typeof(string))
            {
                MaxProperty property = (MaxProperty)value;

                return property.Value;
            }

            return base.ConvertTo(context,culture,value,destType);
        }  

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
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
            VariableProperty variableProperty = context.PropertyDescriptor as VariableProperty;
     
            return new StandardValuesCollection(variableProperty.Mpm.GetAllNativeTypesDelegate());
    
        }
    }
}
