using System;
using System.ComponentModel;


namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for StyleProperty.
    /// </summary>
    [TypeConverter(typeof(StyleConverter))]
    public class StyleProperty : MaxProperty
    {
        public new DataTypes.LinkStyle Value
        {
            get{ return (DataTypes.LinkStyle) Enum.Parse(typeof(DataTypes.LinkStyle), this.value_.ToString(), true);}
            set{ this.value_ = value; }
        }

        public StyleProperty(DataTypes.LinkStyle value_, IMpmDelegates mpm, 
            object subject, PropertyDescriptorCollection container) 
            : base(DataTypes.LINKSTYLE, value_, false, mpm, subject, container)
        {
            this.description = DataTypes.STYLE_PROPERTY_DESCRIPTION;
        } 
   
        public StyleProperty(DataTypes.LinkStyle value_, string description, IMpmDelegates mpm, 
            object subject, PropertyDescriptorCollection container) 
            : base(DataTypes.LINKSTYLE, value_, false, mpm, subject, container)
        {
            this.description = description; 
        } 

        public override void Initialize(SimplePropertyType info)
        {
            try
            {
                this.value_ = (DataTypes.LinkStyle) Enum.Parse(typeof(DataTypes.LinkStyle), info.Value, true);
                this.oldValue = (DataTypes.LinkStyle) Enum.Parse(typeof(DataTypes.LinkStyle), info.Value, true);
            }
            catch
            {
                // TODO: WARNING
                this.value_ = DataTypes.LinkStyle.Bezier;
                this.oldValue = DataTypes.LinkStyle.Bezier;
            }  
        }
    }

    internal class StyleConverter : TypeConverter
    { 

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
        {
            if(value is MaxProperty && destType == typeof(string))
            {
                MaxProperty property = (MaxProperty)value;

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
            return new StandardValuesCollection(System.Enum.GetNames(typeof(DataTypes.LinkStyle)));
        }

    }
}
