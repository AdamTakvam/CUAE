using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using Metreos.Max.Core;

namespace Metreos.Max.Debugging
{
    /// <summary> Reflects any object and formats for the property grid </summary>
    public class MaxWatchGridAdapter : ICustomTypeDescriptor
    {   
        private SortedList properties;

        /// <summary> Display these items to the property grid.  
        /// Key of list must be a string, must be a VariableData type </summary>
        public MaxWatchGridAdapter(SortedList properties)
        {
            this.properties = properties;    
        }
   
        #region ICustomTypeDescriptor Members

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
      
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
      
        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
      
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
      
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
      
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
      
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        { 
            return GetProperties();
        }
      

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection propColl = new PropertyDescriptorCollection(null);
                
            if(properties == null || properties.Count == 0) return propColl;

            IDictionaryEnumerator listEnum = properties.GetEnumerator();

            while(listEnum.MoveNext())
            {
                string key    = listEnum.Key as string;
                VariableData @value = listEnum.Value as VariableData;

                if(@value.VariableInstance is IDictionary)
                {
                    IDictionary dict = @value as IDictionary;
                    propColl.Add(new CollectionProperty
                        (key, dict.Keys, dict.Keys.GetType(), @value.Type));
                    //                    propColl.Add(new CollectionProperty
                    //                       (CollectionProperty.DictValueName, dict.Values, dict.Values.GetType(), 0));
                }
                else if(@value.VariableInstance is ICollection)
                {
                    propColl.Add(new CollectionProperty(key, 
                        @value.VariableInstance as ICollection, @value.VariableInstance.GetType(), @value.Type));
                }
                else
                {
                    propColl.Add(new FlexibleProperty(key, @value.VariableInstance, @value.Type));
                }
            }

            return propColl;        
        }
      

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
      
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }
      
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
      
        public string GetClassName()
        {
            TypeDescriptor.GetClassName(this, true);
            return null;
        }

        #endregion
    }

    public class VariableData
    {
        public Variable Type            { get { return variableType; } }
        public Object VariableInstance  { get { return variableInstance; } }
        private Variable variableType;
        private Object variableInstance;

        public VariableData(Variable variableType, object variableInstance)
        {
            this.variableType = variableType;
            this.variableInstance = variableInstance;
        }
    }

    public enum Variable
    {
        Local,
        Global
    }

    #region CollectionProperty

    [TypeConverter(typeof(CollectionProperty.CollectionConverter))]
    public class CollectionProperty : PropertyDescriptor
    {
        public const string DictKeyName = "Key";
        public const string DictValueName = "Value";
        public const string DefaultName = "List";

        public override string Description  { get { return description; } }
        public override string Category     { get { return category; } }
        public PropertyDescriptorCollection ChildrenProperties { get { return childrenProperties; } }

        protected const string InnerName = "Values";
        protected const string IndexFormatter = "[{0}]";
        protected static string CountFormatter(int arg) { return "{Length=" + arg + "}"; }
        protected Type type;
        protected string description;
        protected string category;
        protected PropertyDescriptorCollection childrenProperties;
        protected ICollection collection;
        protected bool expanded;

        public CollectionProperty(string name, ICollection collection, Type type, Variable variableType): 
            base(name, null)
        {
            this.expanded       = false;
            this.type           = type;
            if(type != null)    this.description    = type.FullName;
            this.category       = variableType.ToString();
            this.collection     = collection;
        }


        public CollectionProperty(string name, ICollection collection, Type type) :
            this(name, collection, type, Variable.Local) {}


        private void ConstructChildren()
        {
            childrenProperties = new PropertyDescriptorCollection(null);

            if(collection == null || collection.Count <= 0) return;

            try
            {
                int i = 0;
                foreach(object obj in collection)
                {
                    if(obj is IDictionary)
                    {
                        IDictionary dict = obj as IDictionary;
                        childrenProperties.Add(new CollectionProperty
                            (FormatIndex(i), dict.Values, dict.Values.GetType()));
                    }
                    else if(obj is ICollection)
                    {
                        childrenProperties.Add(new CollectionProperty
                            (FormatIndex(i), obj as ICollection, type));
                    }
                    else
                    {
                        childrenProperties.Add(new FlexibleProperty(FormatIndex(i), obj));
                    }
                    i++;
                }
            }
            catch { }
     
            childrenProperties.Add(new TypeProperty(collection.GetType()));
        }


        private static string FormatIndex(int i)
        {
            return String.Format(IndexFormatter, i);
        }
          
  
        public static string FormatCount(int count)
        {
            if(count != 1)  
                return CountFormatter(count >= 1 ? count - 1 : 0);
            else return CountFormatter(count >= 1 ? count - 1 : 0);            
        }


        internal class CollectionConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
                object value, Attribute[] attributes)
            {
                if(value is CollectionProperty)
                {
                    CollectionProperty property = value as CollectionProperty;
                    return property.ChildrenProperties;
                }

                return base.GetProperties (context, value, attributes);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                if(context.PropertyDescriptor is CollectionProperty)
                {
                    CollectionProperty property = context.PropertyDescriptor as CollectionProperty;

                    if(property.expanded == false)
                    {
                        property.expanded = true;
                        property.ConstructChildren();
                    }

                    return property.ChildrenProperties.Count != 0;
                }
                    
                return false;
            }


            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if(typeof(CollectionProperty).IsAssignableFrom(destinationType))  return true;
                return base.CanConvertTo(context, destinationType);                                                       
            }

            
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                // Not editable at the moment
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
                if(value is CollectionProperty && typeof(string).IsAssignableFrom(destType))
                {
                    CollectionProperty property = value as CollectionProperty;
                    return CollectionProperty.FormatCount(property.ChildrenProperties.Count);
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


        #region PropertyDescriptor Members

        public override bool IsReadOnly { get { return true; } }

        public override void SetValue(object component, object value)
        {
            // Not implemented
        }

        public override object GetValue(object component)
        {
            return this;
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return this.GetType();
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.GetType();
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            if(this.childrenProperties.Count != 0)
            {
                return childrenProperties;   
            }

            return base.GetChildProperties (instance, filter);
        }


        public override void AddValueChanged(object component, EventHandler handler)
        {
            base.AddValueChanged (component, handler);
        }

        public override void ResetValue(object component)
        {
      
        }

        protected override void OnValueChanged(object component, EventArgs e)
        {
            base.OnValueChanged (component, e);
        }

            

        #endregion PropertyDescriptor Members
    }

    #endregion

    #region FlexibleProperty

    [TypeConverter(typeof(FlexibleProperty.FlexibleConverter))]
    public class FlexibleProperty : PropertyDescriptor
    {
        public static string FormatString(string arg) { return "\"" + arg + "\""; }
        public string Value { get { return arg == null ? Const.debugWatchNullValue : arg.ToString(); } }
        public string TypeName { get { return type.FullName; } }
        public bool IsSingle { get { return isSingle; } }
        public bool IsString { get { return isString; } }
        public PropertyDescriptorCollection ChildrenProperties { get { return childrenProperties; } }
        public override bool IsReadOnly { get { return false; } }
        public override string Description { get { return description; } }
        public override string Category { get { return category; } }

        protected object arg; 
        protected Type type;
        protected bool isSingle;
        protected PropertyDescriptorCollection childrenProperties;
        protected string description;
        protected string category;
        protected bool isString;
        protected bool expanded;

        public FlexibleProperty(string name, object arg, Variable variableType): base(name, null)
        {
            this.expanded       = false;
            this.arg            = arg;
            if(arg != null)     
            {
                this.type = arg.GetType();
                this.isSingle       = type.IsPrimitive || type == typeof(string);
                this.isString       = type == typeof(string);
                this.description    = type.FullName;
            }
            else     
            {
                this.type = null;
                this.isSingle = true;
                this.isString = false;
                this.description =  Const.debugWatchNullValue;
            }

            this.category       = variableType.ToString();
            
        }
        
        public FlexibleProperty(string name, object arg) : this(name, arg, Variable.Local){ }

        private void ConstructChildren()
        {
            childrenProperties = new PropertyDescriptorCollection(null);

            try
            {
                if(isSingle)
                {
                    AddTypeProperty(arg, childrenProperties);
                }
                else
                {
                    AddFields(arg, childrenProperties);
                    AddProperties(arg, childrenProperties);
                    AddTypeProperty(arg, childrenProperties);
                }
            }
            catch {}
        }


        private void AddFields(object arg, PropertyDescriptorCollection properties)
        {           
            if(type == null)                            return;

            FieldInfo[] fields = type.GetFields();

            if(fields == null || fields.Length == 0)    return;

            foreach(FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(arg); 

                if(fieldValue is IDictionary)
                {
                    IDictionary dict = fieldValue as IDictionary;
                    properties.Add(new CollectionProperty
                        (field.Name, dict.Values, dict.Values.GetType()));
                }
                else if(fieldValue is ICollection)
                {
                    properties.Add(new CollectionProperty
                        (field.Name, fieldValue as ICollection, type));
                }
                else
                {
                    properties.Add(new FlexibleProperty(field.Name, fieldValue));
                }
            }
        }


        private void AddProperties(object arg, PropertyDescriptorCollection properties)
        {
            if(type == null) return;

            PropertyInfo[] propertyInfos = type.GetProperties();

            if(propertyInfos == null || propertyInfos.Length == 0) return;

            foreach(PropertyInfo propertyInfo in propertyInfos)
            {       
                ParameterInfo[] parameters = propertyInfo.GetIndexParameters();

                if(parameters != null && parameters.Length > 0) continue; // Indexer property

                object propertyValue = propertyInfo.GetValue(arg, null);

                if(propertyValue is IDictionary)
                {
                    IDictionary dict = propertyValue as IDictionary;
                    properties.Add(new CollectionProperty
                        (propertyInfo.Name, dict.Values, dict.Values.GetType()));
                }
                else if(propertyValue is ICollection)
                {
                    properties.Add(new CollectionProperty
                        (propertyInfo.Name, propertyValue as ICollection, type));
                }
                else
                {
                    properties.Add(new FlexibleProperty(propertyInfo.Name, propertyValue));
                }
            }            
        }  


        private void AddTypeProperty(object arg, PropertyDescriptorCollection properties)
        {   
            if(type == null) return;
            properties.Add(new TypeProperty(type));
        }


        internal class FlexibleConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
                object value, Attribute[] attributes)
            {
                if(value is FlexibleProperty)
                {
                    FlexibleProperty property = value as FlexibleProperty;
                    return property.ChildrenProperties;
                }

                return base.GetProperties (context, value, attributes);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                if(context.PropertyDescriptor is FlexibleProperty)
                {
                    FlexibleProperty property = context.PropertyDescriptor as FlexibleProperty;

                    if(property.expanded == false)
                    {
                        property.expanded = true;
                        property.ConstructChildren();
                    }

                    return property.ChildrenProperties.Count != 0;
                }
 
                return false;
            }


            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if(typeof(FlexibleProperty).IsAssignableFrom(destinationType))  return true;
                return base.CanConvertTo(context, destinationType);                                                       
            }

            
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                // Not editable at the moment
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
                if(value is FlexibleProperty && typeof(string).IsAssignableFrom(destType))
                {
                    FlexibleProperty property = value as FlexibleProperty;

                    if(property.IsSingle)   return property.IsString ? FlexibleProperty.FormatString(property.Value) :  
                                                property.Value;
                    else                    return property.TypeName;
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
   
        #region PropertyDescriptor Members

        /// <summary>
        /// Requires correct implementation of a custom TypeConverter, which should convert a 
        /// string (which is entered into the PropertyGrid) to a object of type MaxProperty.  
        /// </summary>
        /// <param name="component">The component of this property</param>
        /// <param name="value">Should be of type MaxProperty, and the Value property found in data 
        /// should be set to what the user entered into the PropertyGrid</param>
        public override void SetValue(object component, object value)
        {
            // Not implemented
        }

        public override object GetValue(object component)
        {
            return this;
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return this.GetType();
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.GetType();
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            if(this.childrenProperties.Count != 0)
            {
                return childrenProperties;
            }

            return base.GetChildProperties (instance, filter);
        }


        public override void AddValueChanged(object component, EventHandler handler)
        {
            base.AddValueChanged (component, handler);
        }

        public override void ResetValue(object component)
        {
      
        }

        protected override void OnValueChanged(object component, EventArgs e)
        {
            base.OnValueChanged (component, e);
        }

        #endregion PropertyDescriptor Members
    }

    #endregion  
    
    #region TypeProperty

    [TypeConverter(typeof(TypeProperty.TypeConverter))]
    public class TypeProperty : PropertyDescriptor
    {
        private const string TypeName = "System.Type";
        private const string InterfaceName = "Interface";
        private const string BaseName = "Base";

        public string Value { get { return type.FullName; } }
        public PropertyDescriptorCollection ChildrenProperties { get { return childrenProperties; } }
        public override bool IsReadOnly { get { return true; } }
        public override string Description { get { return description; } }

        protected Type type;
        protected bool isSingle;
        protected PropertyDescriptorCollection childrenProperties;
        protected string description;
        protected bool expanded;

        public TypeProperty(Type type): base(TypeName, null)
        {
            this.expanded       = false;
            this.type           = type;
            if(type != null)    this.description = type.FullName;
        }


        public TypeProperty(string name, Type type) : base(name, null) 
        {
            this.expanded       = false;
            this.type           = type;
            if(type != null)    this.description = type.FullName;
        }


        private void ConstructChildren()
        {
            childrenProperties = new PropertyDescriptorCollection(null);

            try
            {
                AddBaseProperty(type, childrenProperties);
            }
            catch {}
        }


        private void AddBaseProperty(Type type, PropertyDescriptorCollection properties)
        {
            if(type.BaseType == typeof(Object) || type.BaseType == null) return;
            properties.Add(new TypeProperty(BaseName, type.BaseType));
        }


        internal class TypeConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
                object value, Attribute[] attributes)
            {
                if(value is TypeProperty)
                {
                    TypeProperty property = value as TypeProperty;
                    return property.ChildrenProperties;
                }

                return base.GetProperties (context, value, attributes);
            }


            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                if(context.PropertyDescriptor is TypeProperty)
                {
                    TypeProperty property = context.PropertyDescriptor as TypeProperty;
                    
                    if(property.expanded == false)
                    {
                        property.expanded = true;
                        property.ConstructChildren();
                    }

                    return property.ChildrenProperties.Count != 0;
                }
                    
                return false;
            }


            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if(typeof(TypeProperty).IsAssignableFrom(destinationType))  return true;
                return base.CanConvertTo(context, destinationType);                                                       
            }

            
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                // Not editable at the moment
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
                if(value is TypeProperty && typeof(string).IsAssignableFrom(destType))
                {
                    TypeProperty property = value as TypeProperty;
                    return property.Value;
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
    
        #region PropertyDescriptor Members

        /// <summary>
        /// Requires correct implementation of a custom TypeConverter, which should convert a 
        /// string (which is entered into the PropertyGrid) to a object of type MaxProperty.  
        /// </summary>
        /// <param name="component">The component of this property</param>
        /// <param name="value">Should be of type MaxProperty, and the Value property found in data 
        /// should be set to what the user entered into the PropertyGrid</param>
        public override void SetValue(object component, object value)
        {
            // Not implemented
        }

        public override object GetValue(object component)
        {
            return this;
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return this.GetType();
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.GetType();
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            if(this.childrenProperties.Count != 0)
            {
                return childrenProperties;
            }

            return base.GetChildProperties (instance, filter);
        }


        public override void AddValueChanged(object component, EventHandler handler)
        {
            base.AddValueChanged (component, handler);
        }

        public override void ResetValue(object component)
        {
        
        }

        protected override void OnValueChanged(object component, EventArgs e)
        {
            base.OnValueChanged (component, e);
        }

        #endregion PropertyDescriptor Members
    }

    #endregion      
}
