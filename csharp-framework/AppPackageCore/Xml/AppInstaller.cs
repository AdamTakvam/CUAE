﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=1.1.4322.2032.
// 
namespace Metreos.AppArchiveCore.Xml {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
    [System.Xml.Serialization.XmlRootAttribute("install", Namespace="http://metreos.com/AppInstaller.xsd", IsNullable=false)]
    public class installType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("configuration")]
        public configurationType[] configuration;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
    public class configurationType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("configValue")]
        public configValueType[] configValue;
        
        /// <remarks/>
        public string unused;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metreos.com/AppInstaller.xsd")]
    public class configValueType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EnumItem")]
        public string[] EnumItem;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int minValue;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minValueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int maxValue;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxValueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultValue;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool readOnly;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool readOnlySpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool required;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool requiredSpecified;
    }
}
