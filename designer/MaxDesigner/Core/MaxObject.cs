using System;
using System.Xml;
using System.ComponentModel;


namespace Metreos.Max.Core
{
    /// <summary> High-level object type</summary>
    public enum ObjectTypes { Other, Project, App, Canvas, Tool, Node, Link, Label, Group, Collection }


    ///<summary> Interface implemented by all serializable objects</summary>
    public interface MaxObject
    {    
        ObjectTypes MaxObjectType { get; }  
        string ObjectDisplayName  { get; } 
        void MaxSerialize(XmlTextWriter writer); 
    }


    /// <summary> Interface implemented by objects which can assume property focus</summary>
    public interface MaxSelectableObject: MaxObject, ICustomTypeDescriptor
    {
        ///<summary>Interface implemented by all serializable objects</summary>
        PropertyDescriptorCollection MaxProperties { get; }      

        ///<summary>Create Max-specific properties for this object</summary>                              
        PropertyDescriptorCollection CreateProperties(PropertyGrid.Core.PackageElement pe);

        ///<summary>Get Properties Manager object type for this object</summary>
        Framework.Satellite.Property.DataTypes.Type PmObjectType { get; }  

        ///<summary>Handle change notification from Properties Manager</summary>
        void OnPropertiesChangeRaised(Framework.Satellite.Property.MaxProperty[] changedProperties);

    }

} // namespace
