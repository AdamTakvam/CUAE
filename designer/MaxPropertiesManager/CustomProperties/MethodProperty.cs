using System;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
    [TypeConverter(typeof(MethodProperty.MethodConverter))]
    public class MethodProperty : MaxProperty
    {
        public override string Description { get { return description; } }
        public override string Category { get { return category; } }

        protected int depth;
        protected MethodInfo method;

        public MethodProperty(MethodInfo method, 
            IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) : 
            base(method.Name, FormatParamCount(method.GetParameters() != null ? method.GetParameters().Length : 0), 
            false, mpm, subject, container)
        {
            this.method = method;
            this.description = null;
            this.category = null;

            ConstructChildren();   
        }

        protected void ConstructChildren()
        {
            childrenProperties = new PropertyDescriptorCollection(null);

            try
            {
                AddArguments();
                AddReturnType();
            }
            catch { }
        }

        protected void AddArguments()
        {
            ParameterInfo[] parameters = method.GetParameters();

            if(parameters == null || parameters.Length == 0)  return;

            foreach(ParameterInfo parameter in parameters)
            {
                childrenProperties.Add(new ReflectorProperty(parameter.Name, parameter.ParameterType, null,
                    this.mpm, subject, this.container));
            }
        }

        protected static string FormatParamCount(int length)
        {
            if(length == 1)                 return "1 argument";
            else                            return length + " arguments";
        }

        protected void AddReturnType()
        {
            if(method.ReturnType != null)
            {
                childrenProperties.Add(new ReflectorProperty("[ReturnType]", method.ReturnType, null,
                    this.mpm, subject, this.container));
            }
        }

        internal class MethodConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
                object value, Attribute[] attributes)
            {
                if(value is MethodProperty)
                {
                    MethodProperty property = value as MethodProperty;
                    return property.ChildrenProperties;            
                }

                return base.GetProperties (context, value, attributes);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                if(context.PropertyDescriptor is MethodProperty)
                {
                    MethodProperty property = context.PropertyDescriptor as MethodProperty;
                    return property.ChildrenProperties.Count != 0;
                }
                    
                return false;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if(typeof(MethodProperty).IsAssignableFrom(destinationType))  return true;
                return base.CanConvertTo(context, destinationType);                                                       
            }

            
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return false;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, 
                System.Globalization.CultureInfo culture, object value)
            {
                return value;
            }

            public override object ConvertTo(ITypeDescriptorContext context, 
                System.Globalization.CultureInfo culture, object value, Type destType )
            {
                if(value is MethodProperty && typeof(string).IsAssignableFrom(destType))
                {
                    MethodProperty property = value as MethodProperty;
                    ParameterInfo[] parameters = property.method.GetParameters();

                    return parameters != null ? FormatParamCount(parameters.Length) : FormatParamCount(0);
                }

                return base.ConvertTo(context,culture,value,destType);
            }  

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                // No drop down editor
                return false;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }
        }
    }
}
