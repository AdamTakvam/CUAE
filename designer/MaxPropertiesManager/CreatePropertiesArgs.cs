using System;
using System.Drawing;

namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>Passed in when creating the PropertyDescriptorCollecion for the first time.</summary>
  public class CreatePropertiesArgs
  {
    public object           value_;  
    public object           subject;
    public DataTypes.Type   type;   // jldnote this is a local class
    public Bitmap           iconSm;                   
    // For now, not having a spot in those samoa classes to hang 
    // some extra data on, we're going to put this bitmap here.
    // A better solution will pop up when/if we lose the samoa classes. 
                                            
    public CreatePropertiesArgs(object value_, DataTypes.Type type)
    {   
      this.type   = type;           
      this.value_ = value_;           
    }

    public CreatePropertiesArgs(object subject, object value_, DataTypes.Type type)
    {   
      this.subject = subject;
      this.type   = type;           
      this.value_ = value_;           
    }
  }
}
