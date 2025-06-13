using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using Metreos.Common.Reserve;

namespace Metreos.Native.Reserve
{
    /// <summary></summary>
    public class SignOut : INativeAction
    {
        private static XmlSerializer outRes = new XmlSerializer(typeof(SignOffResponse));

        [ActionParamField("Host", "The host of the Tririga Webservice" , true)]
        public string Host { set { host = value; } }
        private string host;

        [ActionParamField("Port", "The port of the Tririga Webservice", false)]
        public string Port { set { port = value; } }
        private string port; 

        [ActionParamField("Secure", "Connect securely to Tririga Webservice", true)]
        public bool Secure { set { secure = value; } }
        private bool secure;

        [ActionParamField("SecurityToken", "Used to access webservices, once logged in", true)]
        public string SecurityToken { set { securityToken = value; } }
        private string securityToken; 
        
        [ActionParamField("SignOn UserID", "Used to access webservices, once logged in", true)]
        public long SignOnUserId { set { signOnUserId = value; } }
        private long signOnUserId; 
       
        [ActionParamField("Proxy URL", "An HTTP proxy server", false)]
        public string ProxyURL { set { proxyUrl = value; } }
        private string proxyUrl;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            host = null;
            port = null;
            secure = false;
            securityToken = null;
            signOnUserId = -1;
            proxyUrl = null;
        }

        public SignOut()
        {
            Clear();    
        }
        
        public bool ValidateInput()
        {
            return true;
        }

        [Action("SignOut", false, "Sign Out", "Signs out to Tririga")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            SignOnInterfaceService service = new SignOnInterfaceService();
            service.SetUrl(host, port, proxyUrl, secure);
    
            bool communicationSuccess = true;
            string responseXml = null;

            try
            {
                responseXml = service.signOut(securityToken, signOnUserId);
            }
            catch(System.Web.Services.Protocols.SoapException se)
            {
                log.Write(TraceLevel.Error, "Soap error in the SignOut Web Service.\n" + 
                    "Detail: {0}\n{1}", se.Detail.InnerXml, se);
                communicationSuccess = false;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "General failure in the SignOut Web Service.\n" + e);
                communicationSuccess = false;
            }

            if(communicationSuccess)
            {
                bool foundResponse = true;
                if(responseXml == null || responseXml == String.Empty)
                {
                    log.Write(TraceLevel.Error, "The response XML from the SignOut Web Service method was empty");
                    foundResponse = false;
                }
                    
                if(foundResponse)
                {
                    bool validResponseXml = true;
                    SignOffResponse response = null;
                    StringReader reader = null;
                    try
                    {
                        reader = new StringReader(responseXml);
                        response = outRes.Deserialize(reader) as SignOffResponse;
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Error, "The response XML from the SignOff Web Service was not deserializable\n" + responseXml + "\n" + e);
                        validResponseXml = false;
                    }
                    finally
                    {
                        if(reader != null)
                        {
                            reader.Close();
                        }
                    }

                    if(validResponseXml)
                    {
                        bool isSuccess = true;
                        string responseString = response.Result == null ? String.Empty : response.Result.Trim().ToLower();
                        if(responseString != "success")
                        {
                            log.Write(TraceLevel.Error, "SignOff service returned failure");
                            isSuccess = false;
                        }

                        if(isSuccess)
                        {
                            return IApp.VALUE_SUCCESS;
                        }
                    }
                }
            }

            return IApp.VALUE_FAILURE;
        }
    }

}
