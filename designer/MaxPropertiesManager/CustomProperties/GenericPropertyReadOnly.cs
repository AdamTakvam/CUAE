using System;
using System.ComponentModel;
using System.Collections;



namespace Metreos.Max.Framework.Satellite.Property
{
  [TypeConverter(typeof(GenericPropertyConverter))]
  public class GenericPropertyReadOnly : MaxProperty
  {       
        
    public GenericPropertyReadOnly(
        string name, 
        string value_, 
        IMpmDelegates mpm, 
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, true, mpm, subject, container)
    {
			
    }

    public GenericPropertyReadOnly(
        string name, 
        string value_, 
        string category, 
        IMpmDelegates mpm,
        object subject,
        PropertyDescriptorCollection container) 
        : base(name, value_, true, mpm, subject, container)
    {
      this.category = category;
    }
  }
}
