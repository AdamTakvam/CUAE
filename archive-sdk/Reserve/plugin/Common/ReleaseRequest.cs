using System;
using System.Xml.Serialization;

[Serializable]
[System.Xml.Serialization.XmlRootAttribute("ReleaseRequest", Namespace="", IsNullable=false)]
public class ReleaseRequestType {
  
    public string CcmIP;

    public string CcmUser;

    public string DeviceName;

    public string IntegrationUser;

    public string RecordId;

    public string SecurityToken;
}
