using System;
using System.Xml;
using System.Xml.Serialization;

namespace Metreos.Common.Reserve
{
	/// <summary>
	/// Summary description for ResponseStructs.
    /// 
    ///<SignOnAction>
    ///<Result>Success</Result>
    ///<User><Id>2579463</Id><CompanyId>208133</CompanyId><SecurityToken>1147874458477</SecurityToken><FirstName>Seth</FirstName><LastName>Call</LastName></User>
    ///</SignOnAction>     
	/// </summary>
	
    [Serializable()]
    [XmlRoot("SignOnAction")]
    public class SignOnResponse
    {
        public string Result;

        public User User;
    }

    [XmlRoot("User")]
    public class User
    {
        // A numeric ID needed to logout with
        public string Id;

        public string CompanyId;

        public string SecurityToken;

        public string FirstName;

        public string LastName;
    }

    /// <summary>
    /// <SignOutAction>
    /// <Result>Success</Result>
    /// </SignOutAction>"
    /// </summary>
    
    [Serializable()]
    [XmlRoot("SignOutAction")]
    public class SignOffResponse
    {
        public string Result;
    }

    [Serializable()]
    [XmlRoot("BoRecords")]
    public class SaveBoResponse
    {
        [XmlElement("BoRecord")]
        public BoRecordResponse[] BoRecord;
    }

    [Serializable()]
    [XmlRoot("BoRecord")]
    public class BoRecordResponse
    {
        [XmlElement("Message", typeof(XmlCDataSection))]
        public XmlCDataSection Message;
        
        public string RecordId;
    }

    [Serializable()]
    [XmlRoot("BoRecords")]
    public class SaveBoRequest
    {
        [XmlElement("BoRecord")]
        public BoRecordRequest[] BoRecord;
    }


    [Serializable()]
    [XmlRoot("BoRecord")]
    public class BoRecordRequest
    {
        [XmlAttribute("BoId")]
        public string BoId;

        [XmlAttribute("BoName")]
        public string BoName;

        [XmlAttribute("RecordId")]
        public string RecordId;
        
        [XmlAttribute("ModuleId")]
        public string ModuleId;
        
        [XmlAttribute("CompanyId")]
        public string CompanyId;
        
        [XmlAttribute("ParentName")]
        public string ParentName;

        [XmlAttribute("ActionName")]
        public string ActionName;

        [XmlAttribute("GUIId")]
        public string GUIId;

        [XmlAttribute("ProjectId")]
        public string ProjectId;

        public General General;

        public RecordInformation RecordInformation;
    }

    [Serializable()]
    [XmlRoot("General")]
    public class General
    {
        public string triRecordIdTX;
        public string enyCheckInTX;
    }

    [Serializable()]
    [XmlRoot("RecordInformation")]
    public class RecordInformation
    {
        [XmlElement("eyResultCodeTX", typeof(XmlCDataSection))]
        public XmlCDataSection eyResultCodeTX;

        [XmlElement("eyResultMessageTX", typeof(XmlCDataSection))]
        public XmlCDataSection eyResultMessageTX;

        [XmlElement("eyDiagnosticCodeTX", typeof(XmlCDataSection))]
        public XmlCDataSection eyDiagnosticCodeTX;

        [XmlElement("eyDiagnosticMessageTX", typeof(XmlCDataSection))]
        public XmlCDataSection eyDiagnosticMessageTX;

        [XmlElement("eyLoggedInUserTX", typeof(XmlCDataSection))]
        public XmlCDataSection eyLoggedInUserTX;
        
        [XmlElement("eyLoggedInDeviceTX", typeof(XmlCDataSection))]
        public XmlCDataSection eyLoggedInDeviceTX;
    }
}
