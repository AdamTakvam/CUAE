using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for ActionParameterProperty.
    /// </summary>
    [TypeConverter(typeof(ActionParameterReadOnlyConverter))]
    public class ActionParameterPropertyReadOnly : MaxProperty
    {
        private bool isRequired;

        #region Properties

        public bool IsRequired
        {
            get
            {
                return isRequired;
            }
        }

        #endregion Properties

        public ActionParameterPropertyReadOnly(string name, string displayName, string value_, bool isRequired, 
            IMpmDelegates mpm, object subject, 
            PropertyDescriptorCollection container)
            : base(name, value_, true, mpm, subject, container)
        {
            this.description = Defaults.REQUIRED_PARAM_APPEND;
            this.isRequired = isRequired;
            this.category = DataTypes.ACTION_PARAMETERS_CATEGORY;
            this.displayName = displayName;
        }

        public ActionParameterPropertyReadOnly(string name, string displayName, string value_, bool isRequired, 
            string description, IMpmDelegates mpm, 
            object subject, PropertyDescriptorCollection container) 
            : base(name, value_, true, mpm, subject, container)
        {
            this.isRequired = isRequired;
            this.category = DataTypes.ACTION_PARAMETERS_CATEGORY;
            this.displayName = displayName; 

            if(isRequired)
            {
                string descrip = description != null ? description : String.Empty;
                this.description = descrip + Defaults.blank + Defaults.REQUIRED_PARAM_APPEND;
            }
            else
            {
                this.description = description != null ? description : String.Empty;
            }
        }
    }  

    internal class ActionParameterReadOnlyConverter : TypeConverter
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
            if(typeof(ActionParameterPropertyReadOnly) == destinationType)
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
            if(value is ActionParameterPropertyReadOnly && destType == typeof(string))
            {
                ActionParameterPropertyReadOnly parameter = (ActionParameterPropertyReadOnly)value;

                return parameter.Value;
            }

            return base.ConvertTo(context,culture,value,destType);
        }  
    }

}
