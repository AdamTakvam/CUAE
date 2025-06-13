using System;
using System.Xml.Serialization;

[Serializable]
[System.Xml.Serialization.XmlRootAttribute("ReleaseResponse", Namespace="", IsNullable=false)]
public class ReleaseResponseType {
    
    public string ResultCode;
    
    public string ResultMessage;
    
    public string DiagnosticCode;
    
    public string DiagnosticMessage;
}
