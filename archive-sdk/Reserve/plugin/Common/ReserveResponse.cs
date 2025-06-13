using System;
using System.Xml.Serialization;

[Serializable]
[System.Xml.Serialization.XmlRootAttribute("ReserveResponse", Namespace="", IsNullable=false)]
public class ReserveResponseType {
    
    public string ResultCode;
    
    public string ResultMessage;
    
    public string DiagnosticCode;
    
    public string DiagnosticMessage;

    public string LoggedInUser;
    
    public string LoggedInDevice;
}
