using System;
using System.Collections;
using System.ComponentModel;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    ///   Proxy object to host property collection for display in grid.
    ///   The .NET property grid can host any object.  Regardless of what it is,
    ///   it will use a number of rules to reflect the information found out about
    ///   that type.
    ///   
    ///   However, one can control the behavior of this inspection process by 
    ///   passing the .NET property grid an ICustomTypeDescriptor.  
    ///   
    ///   The only method we much care for in the ICustomTypeDescriptor interface
    ///   is the get properties method.  From this we can define exactly which
    ///   properties show up in the Visual Designer Property Grid
    /// </summary>
    public  class MaxProxyObject: ICustomTypeDescriptor
    {
        public  PropertyDescriptorCollection CustomProperties { get { return customProperties; } set { customProperties = value; } }
        public	DataTypes.Type NodeType {get { return nodeType; } }
        private PropertyDescriptorCollection customProperties;
        private DataTypes.Type nodeType;

        private static PropertyDescriptorCollection emptyCollection = new PropertyDescriptorCollection(null);

		
        public MaxProxyObject()
        {
            this.Reset();
            this.nodeType = DataTypes.Type.Nothing;
        }

        public void Reset() { this.customProperties = emptyCollection; }

        public void Reset(DataTypes.Type nodeType) 
        { 
            this.customProperties = emptyCollection;
            this.nodeType = nodeType;
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

        public PropertyDescriptorCollection  GetProperties()
        {
            return this.customProperties;
            
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
}