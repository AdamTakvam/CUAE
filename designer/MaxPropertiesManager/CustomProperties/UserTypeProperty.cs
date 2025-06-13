using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary> UserType Property, restricted to specified values </summary>
    [TypeConverter(typeof(UserTypeConverter))]
    public class UserTypeProperty : MaxProperty
    {
        [Flags()]
        public enum AllowableTypes
        {
            literal  = 1,
            variable = 2,
            csharp   = 4
        }

        public new DataTypes.UserVariableType Value
        {
            get { return (DataTypes.UserVariableType) Enum.Parse(typeof(DataTypes.UserVariableType), this.value_.ToString() , true); }
            set { this.value_ = value; } 
        }

        public AllowableTypes Allow { get { return allow; } set { allow = value; } }
        private AllowableTypes allow;

        public UserTypeProperty(DataTypes.UserVariableType value_,
            IMpmDelegates mpm, object subject,
            PropertyDescriptorCollection container)
            : base(DataTypes.USERTYPE, value_, false, mpm, subject, container)
        {
            allow             = AllowableTypes.csharp | AllowableTypes.literal | AllowableTypes.variable;
            this.Value        = value_;
            this.description  = Defaults.BASE_DESCRIPTION + Defaults.STRING_DESCRIPTION;
        }

        public UserTypeProperty(AllowableTypes allow, DataTypes.UserVariableType value_,
            IMpmDelegates mpm, object subject,
            PropertyDescriptorCollection container)
            : base(DataTypes.USERTYPE, value_, false, mpm, subject, container)
        {
            this.allow        = allow;
            this.Value        = value_;
            this.description  = Defaults.BASE_DESCRIPTION + Defaults.STRING_DESCRIPTION;
        }

        public override void Initialize(SimplePropertyType info)
        {
            try
            {
                this.value_ = (DataTypes.UserVariableType) Enum.Parse(typeof(DataTypes.UserVariableType), info.Value, true);
                this.oldValue = (DataTypes.UserVariableType) Enum.Parse(typeof(DataTypes.UserVariableType), info.Value, true);
            }
            catch
            {
                // TODO: WARNING
                this.value_ = DataTypes.UserVariableType.literal;
                this.oldValue = DataTypes.UserVariableType.literal;
            }  
        }
    }  

    internal class UserTypeConverter : TypeConverter
    { 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if(typeof(UserTypeProperty) == destinationType)
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
        {
            if(value is UserTypeProperty && destType == typeof(string))
            {
                UserTypeProperty property = (UserTypeProperty)value;

                //Update Description
                if(property.Value == DataTypes.UserVariableType.literal)
                {
                    property.SetDescription = Defaults.BASE_DESCRIPTION + Defaults.STRING_DESCRIPTION;
                }
                else if(property.Value == DataTypes.UserVariableType.csharp)
                {
                    property.SetDescription = Defaults.BASE_DESCRIPTION + Defaults.CSHARP_DESCRIPTION;
                }
                else if(property.Value == DataTypes.UserVariableType.variable)
                {
                    property.SetDescription = Defaults.BASE_DESCRIPTION + Defaults.VARIABLE_DESCRIPTION;
                }
                
                return property.Value.ToString();
            }

            return base.ConvertTo(context,culture,value,destType);
        }  

        // We want a drop down box
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        // Not a combo box
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            UserTypeProperty userType = context.PropertyDescriptor as UserTypeProperty;

            StringCollection allowableTypes = new StringCollection();
            if((UserTypeProperty.AllowableTypes.literal & userType.Allow) != 0)
            {
                allowableTypes.Add(DataTypes.UserVariableType.literal.ToString());
            }
            if((UserTypeProperty.AllowableTypes.variable & userType.Allow) != 0)
            {
                allowableTypes.Add(DataTypes.UserVariableType.variable.ToString());
            }
            if((UserTypeProperty.AllowableTypes.csharp & userType.Allow) != 0)
            {
                allowableTypes.Add(DataTypes.UserVariableType.csharp.ToString());
            }
            return new StandardValuesCollection(allowableTypes);
        }
    }
}
