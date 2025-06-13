using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using PropertyGrid.Core;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary> Acts as the initWith property in local variables.
    /// Its behavior is marked by a dropdown with all applicable 
    /// event params for the function that the variable is contained in </summary>
    [TypeConverter(typeof(LocalVarInitWithPropertyConverter))]
    public class LocalVarInitWithProperty : MaxProperty
    {
        public string[] Events { set { events = value; } }
        private string[] events;

        public string InnerName { get { return innerName; }  set { innerName = value; } }
        private string innerName;
        
        public string InnerEventName { get { return innerEventName; }  set { innerEventName = value; } }
        private string innerEventName;

        public LocalVarInitWithProperty(IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) 
            : base(DataTypes.LOCAL_VAR_INIT_WITH, String.Empty, false, mpm, subject, container)
        {
            this.category = DataTypes.BASIC_PROPERTIES;
        }

        public override void AfterSetValue(object value)
        {
            // When the user selects the dropdown of event parameters ('GetStandardValues'), 
            // we save the event(s) associated with those parameters.  Using this information, 
            // we then query what is the true name of the event parameter using a callback,
            // and populate the InnerValue property on the LocalVarInitWithProperty.  
            // This is later used for serialization of the application XML

            if(value.GetType() == typeof(string))
            {
                string displayName = value as string;
                if((events != null && events.Length != 0))
                {
                    
                    if(events.Length > 1) MessageBox.Show("Multiple Events share this parameter.\nThe application may not compile correctly.\nThis situation is currently unresolvable.");
                    EventParameter eventParam = this.Mpm.GetEventParameterByDisplayName(events[0], displayName);
                    if(eventParam == null)
                    {
                        this.InnerName = displayName;
                        this.InnerEventName = null;
                    }
                    else
                    {
                        this.InnerName = eventParam.Name;
                        this.InnerEventName = events[0];
                    }
                }
                else
                {
                    this.InnerName = displayName;
                    this.InnerEventName = null;
                }
            }
        }
    }

    public class EventParameterInfo
    {
        public string name;
        public string displayName;

        public EventParameterInfo(string name, string displayName)
        {
            this.name = name;
            this.displayName = displayName;
        }
    }

    internal class LocalVarInitWithPropertyConverter : TypeConverter
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

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
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

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            LocalVarInitWithProperty variableInitWithProperty = context.PropertyDescriptor as LocalVarInitWithProperty;
    
            string[] events;
            string[] eventParams = variableInitWithProperty.Mpm.GetInitWithValues(out events);
            variableInitWithProperty.Events = events;
            
            return new StandardValuesCollection(eventParams);
        }
    }
}
