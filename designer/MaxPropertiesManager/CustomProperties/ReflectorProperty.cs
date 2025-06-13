using System;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;


namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary> Reflects a type</summary>
    [TypeConverter(typeof(ReflectorProperty.ReflectorConverter))]
    public class ReflectorProperty : MaxProperty
    {
        public override string Description { get { return description; } }
        public override string Category { get { return category; } }

        protected bool reflected;
        protected Type type;
        protected Type parentType;
        protected ArrayList reflectedTypes;

        public ReflectorProperty(string name, Type type, Type parentType,
            IMpmDelegates mpm, object subject, PropertyDescriptorCollection container) : 
            this(name, type, parentType, mpm, subject, new ArrayList(), container)
        {
      
        }

        public ReflectorProperty(string name, Type type, Type parentType,
            IMpmDelegates mpm, object subject, ArrayList reflectedTypes, PropertyDescriptorCollection container) : 
            base(name, type.FullName, false, mpm, subject, container)
        {
            this.type = type;
            this.parentType = parentType;
            this.reflectedTypes = reflectedTypes;
            this.description = null;
            this.category = null;
            this.reflected = false;

            if(!reflectedTypes.Contains(type))
                reflectedTypes.Add(type);
        }

        protected void ConstructChildren()
        {
            childrenProperties = new PropertyDescriptorCollection(null);

            try
            {
                AddMethods();
                AddProperties();
                AddFields();
                AddItem();
            }
            catch { }
        }

        protected void AddFields()
        {
            FieldInfo[] fields = type.GetFields();

            if(fields == null || fields.Length == 0)    return;

            foreach(FieldInfo field in fields)
            {
                if(field.FieldType != typeof(System.Object) &&   
                    !(field.FieldType == typeof(System.Int32) &&  // Filter out the value__ field on enums
                    field.Name == "value__"))
                {
                    reflectedTypes.Add(field.FieldType);

                    Type[] types = reflectedTypes.ToArray(typeof(Type)) as Type[];
                    ArrayList copy = new ArrayList();
                    copy.AddRange(types);

                    childrenProperties.Add(new ReflectorProperty(field.Name, field.FieldType, type,
                        this.mpm, subject, copy, this.container));
                }
            }
        }

        protected void AddProperties()
        {
            PropertyInfo[] propertyInfos = type.GetProperties();

            if(propertyInfos == null || propertyInfos.Length == 0) return;

            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                if(propertyInfo.PropertyType != typeof(System.Object))
                {
                    Type[] types = reflectedTypes.ToArray(typeof(Type)) as Type[];
                    ArrayList copy = new ArrayList();
                    copy.AddRange(types);

                    childrenProperties.Add(new ReflectorProperty(propertyInfo.Name, propertyInfo.PropertyType, 
                        type, this.mpm, subject, copy, this.container));     
                }
            }            
        }  

        protected void AddMethods()
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            if(methods == null || methods.Length == 0)  return;

            GenericProperty methodPlaceholder = 
                new GenericProperty("[Methods]", FormatMethodCount(methods.Length), false, this.mpm, this.subject, this.container);

            childrenProperties.Add(methodPlaceholder);
            foreach(MethodInfo method in methods)
            {
                methodPlaceholder.ChildrenProperties.Add(new MethodProperty(method, this.mpm, this.subject, this.container));
            }
        }

        protected string FormatMethodCount(int length)
        {
            if(length == 1)   return "1 method";
            else              return length + " methods";
        }

        /// <summary> Tests for IEnumerable, and if it is, returns the Item Type</summary>
        protected void AddItem()
        {
            if(type.IsArray)
            {
                childrenProperties.Add(new ReflectorProperty("[Item]", type.GetElementType(), 
                    parentType, this.mpm, subject, this.container));
            }
        }      

        internal class ReflectorConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
                object value, Attribute[] attributes)
            {
                if(value is ReflectorProperty)
                {
                    ReflectorProperty property = value as ReflectorProperty;
                    return property.ChildrenProperties;
                }

                return base.GetProperties (context, value, attributes);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                if(context.PropertyDescriptor is ReflectorProperty)
                {
                    ReflectorProperty property = context.PropertyDescriptor as ReflectorProperty;
                    if(property.reflected == false)
                    {
                        property.reflected = true;
                        property.ConstructChildren();
                    }
                    return property.ChildrenProperties.Count != 0;
                }
                    
                return false;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if(typeof(ReflectorProperty).IsAssignableFrom(destinationType))  return true;
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
                if(value is ReflectorProperty && typeof(string).IsAssignableFrom(destType))
                {
                    ReflectorProperty property = value as ReflectorProperty;
                    return property.type.FullName;
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
