using System;
using System.Xml.Serialization;

[Serializable]
[System.Xml.Serialization.XmlRootAttribute("ReserveRequest", Namespace="", IsNullable=false)]
public class ReserveRequestType {
    
    public string CcmIP;

    public string CcmUser;

    public string DeviceName;

    public string DeviceProfile;

    public string First;

    public string Last;

    public string IntegrationUser;

    public string RecordId;

    public string SecurityToken;
    
    public string Timeout;
}
