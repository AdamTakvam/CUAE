using System;
using System.ComponentModel;
using System.Collections;



namespace Metreos.Max.Framework.Satellite.Property
{
 
  [Browsable(false)]
  [TypeConverter(typeof(GenericPropertyConverter))]
  public class HiddenGenericProperty : GenericProperty
  {       
    private static AttributeCollection hiddenAttributes 
        = new AttributeCollection(new Attribute[] { new BrowsableAttribute(false) } );
        

    public HiddenGenericProperty(
        string name, 
        string value_, 
        bool isReadOnly, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, isReadOnly, mpm, subject, container)
    {
			
    }


    public HiddenGenericProperty(
        string name, 
        string value_, 
        bool isReadOnly, 
        string category, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, isReadOnly, mpm, subject, container)
    {
        this.category = category;
    }	  

    public override AttributeCollection Attributes { get { return hiddenAttributes; } }

  }
    
}
