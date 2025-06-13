using System;
using System.ComponentModel;



namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Essential data needed to populate a row on the Property Grid
  /// </summary>
  public interface IMaxProperty
  {      
    bool IsRelease{get;}

    bool IsChanged{get;set;}

    bool IsDeleted{get;set;}

    bool IsNew{get;set;}

    bool IsValid{get;}

    object Value{get;set;}

    object OldValue{get;set;}

    object Subject{get;set;}

    PropertyDescriptorCollection ChildrenProperties{get;set;}
  }
}
