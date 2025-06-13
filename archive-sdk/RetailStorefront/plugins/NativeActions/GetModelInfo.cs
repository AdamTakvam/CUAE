using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;

namespace Metreos.Native.PhoneActions
{
	/// <summary>
	///     Given the IP address of a Cisco IP Phone, retrieves the DeviceInformation  XML.
	///     Something like this easily belongs is the framework, but anything of this nature
	///     should be tested with all Modes of all IP phones, as well as across all versions of CCM.
	/// </summary>
	public class GetModeInfo : INativeAction
	{
        private static XmlSerializer deseri = new XmlSerializer(typeof(CiscoIPPhoneModeInfo));

        [ActionParamField("Phone IP", true)]
        public string PhoneIP { set { phoneIp = value; } }
        private string phoneIp;

        [ActionParamField("Phone Username", true)]
        public string PhoneUser { set { phoneUser = value; } }
        private string phoneUser;

        [ActionParamField("Phone Password", true)]
        public string PhonePass { set { phonePass = value; } }
        private string phonePass;
       
        [ResultDataField("PhoneDN")]
        public string Title { get { return title; } }
        private string title;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            phoneIp = null;
            title = null;
            phoneUser = null;
            phonePass = null;
        }

		public GetModeInfo()
		{
		    Clear();    
		}

        
        public bool ValidateInput()
        {
            return true;
        }

        [ReturnValue(typeof(UrlStatus), "The result of HTTP GET to the phone")]
        [Action("GetModeInfo", false, "Get Mode Info", "Returns the Mode Information retrieved from a phone")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            object modeInfoObj = null;
            string url = String.Format("http://{0}/CGI/ModeInfo", phoneIp);

            // Download content
            MemoryStream data   = null;        
            UrlStatus status = UrlStatus.Success;
            HttpWebRequest request    = null;
            HttpWebResponse response  = null;
            Stream stream             = null;

            try 
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.PreAuthenticate = true;
                request.Credentials = new NetworkCredential(phoneUser, phonePass);
                request.ProtocolVersion = HttpVersion.Version11;
                request.KeepAlive = false;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                byte[] buffer = new byte[2048];
                data = new MemoryStream(2048);
                
                int n = 0;
                int bytesRead = 0;

                while (true) 
                {
                    n = stream.Read(buffer, 0, buffer.Length);
                    if (n == 0) break;
          
                    data.Write(buffer, 0, n);
                    bytesRead += n;
                }
            }
            catch(WebException e) 
            {
                if(e.Status == WebExceptionStatus.ConnectFailure || 
                    e.Status == WebExceptionStatus.ConnectionClosed ||
                    e.Status == WebExceptionStatus.Timeout ||
                    e.Status == WebExceptionStatus.NameResolutionFailure)
                    status = UrlStatus.Unreachable;
                else  status = UrlStatus.CommunicationError;
                if(data != null) data.Close();
            }
            catch 
            {
                status = UrlStatus.CommunicationError;    
                if(data != null) data.Close();
            }
            finally
            {
                if(stream != null) 
                {      
                    stream.Flush(); 
                    stream.Close(); 
                    if(response != null) response.Close();
                }
            }

            data.Seek(0, SeekOrigin.Begin);
            
            if(status != UrlStatus.Success) return status.ToString();

            // Deserialize content
            try
            {              
                modeInfoObj = deseri.Deserialize(data);
            }
            catch { status = UrlStatus.Invalid; }
            finally { if(data != null) data.Close(); }
            

            CiscoIPPhoneModeInfo info = modeInfoObj as CiscoIPPhoneModeInfo;
            if(info != null)
            {
                title = info.PlaneTitle;
            }
            return status.ToString();
        }
	}

    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class CiscoIPPhoneModeInfo 
    {
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PlaneTitle;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PlaneFieldCount;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PlaneSoftKeyIndex;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PlaneSoftKeyMask;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Prompt;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Notify;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Status;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CiscoIPPhoneFields", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CiscoIPPhoneModeInfoCiscoIPPhoneFields[] CiscoIPPhoneFields;
    }

    /// <remarks/>
    public class CiscoIPPhoneModeInfoCiscoIPPhoneFields 
    {
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FieldType;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FieldAttr;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fieldHelpIndex;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FieldName;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FieldValue;
    }
}
