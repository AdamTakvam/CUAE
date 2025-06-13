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
    public class SignOn : INativeAction
    {
        private static XmlSerializer onRes = new XmlSerializer(typeof(SignOnResponse));

        [ActionParamField("Host", "The host of the Tririga Web Service" , true)]
        public string Host { set { host = value; } }
        private string host;

        [ActionParamField("Port", "The port of the Tririga Web Service", false)]
        public string Port { set { port = value; } }
        private string port; 

        [ActionParamField("Secure", "Connect securely to Tririga Webservice", true)]
        public bool Secure { set { secure = value; } }
        private bool secure; 

        [ActionParamField("UserID", "Used to login to Tririga Web Service", true)]
        public string UserID { set { userId = value; } }
        private string userId; 
       
        [ActionParamField("Password", "Used to login to Tririga Web Service", true)]
        public string Password { set { password = value; } }
        private string password; 
        
        [ActionParamField("Proxy URL", "An HTTP proxy server", false)]
        public string ProxyURL { set { proxyUrl = value; } }
        private string proxyUrl;

        [ResultDataField("Company ID")]
        public long CompanyId { get { return companyId; } }
        private long companyId;

        [ResultDataField("SignOn UserID")]
        public long SignOnUserId { get { return signOnUserId; } }
        private long signOnUserId;
        
        [ResultDataField("Security Token")]
        public string SecurityToken { get { return securityToken; } }
        private string securityToken;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            host = null;
            port = null;
            secure = false;
            userId = null;
            password = null;
            proxyUrl = null;
            companyId = -1;
            signOnUserId = -1;
            securityToken = null;
        }

        public SignOn()
        {
            Clear();    
        }

        
        public bool ValidateInput()
        {
            return true;
        }

        [Action("SignOn", false, "Sign On", "Signs on to Tririga")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            SignOnInterfaceService service = new SignOnInterfaceService();
            service.SetUrl(host, port, proxyUrl, secure);
    
            bool communicationSuccess = true;
            string responseXml = null;

            try
            {
                responseXml = service.signOn(userId, password);
            }
            catch(System.Web.Services.Protocols.SoapException se)
            {
                log.Write(TraceLevel.Error, "Soap error in the SignOn Web Service.\n" + 
                    "Detail: {0}\n{1}", se.Detail.InnerXml, se);
                communicationSuccess = false;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "General failure in the SignOn Web Service.\n" + e);
                communicationSuccess = false;
            }

            if(communicationSuccess)
            {
                bool foundResponse = true;
                if(responseXml == null || responseXml == String.Empty)
                {
                    log.Write(TraceLevel.Error, "The response XML from the SignOn Web Service method was empty");
                    foundResponse = false;
                }
                    
                if(foundResponse)
                {
                    bool validResponseXml = true;
                    SignOnResponse response = null;
                    StringReader reader = null;
                    try
                    {
                        reader = new StringReader(responseXml);
                        response = onRes.Deserialize(reader) as SignOnResponse;
                    }
                    catch(Exception e)
                    {
                        // Purposely verbose--this should only log in debug mode.  This log statement
                        // can potentially log sensitive information
                        log.Write(TraceLevel.Verbose, "The response XML from the SignOn Web Service was not deserializable\n" + responseXml + "\n" + e);
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
                            log.Write(TraceLevel.Error, "SignOn service returned failure");
                            isSuccess = false;
                        }

                        if(isSuccess)
                        {
                            // Parse results
                            bool userStructDefined = true;
                            if(response.User == null)
                            {
                                log.Write(TraceLevel.Error, "The User element in the SignOn response was empty");
                                userStructDefined = false;
                            }

                            if(userStructDefined)
                            {
                                bool requiredFieldMissing = false; 
                                if(response.User.CompanyId == null)
                                {
                                    log.Write(TraceLevel.Error, "CompanyID in successful SignOn response was empty");
                                    requiredFieldMissing = true;
                                }
                                try
                                {
                                    companyId = long.Parse(response.User.CompanyId);
                                }
                                catch
                                {
                                    log.Write(TraceLevel.Error, "CompanyID was not a 64-bit integer");
                                    requiredFieldMissing = true;
                                }

                                if(response.User.Id == null)
                                {
                                    log.Write(TraceLevel.Error, "UserID in successful SignOn response was empty");
                                    requiredFieldMissing = true;
                                }
                                try
                                {
                                    signOnUserId = long.Parse(response.User.Id);
                                }
                                catch
                                {
                                    log.Write(TraceLevel.Error, "UserID was not a 64-bit integer");
                                    requiredFieldMissing = true;
                                }

                                securityToken = response.User.SecurityToken;

                                if(securityToken == null)
                                {
                                    log.Write(TraceLevel.Error, "SecurityToken in successful SignOn response was empty");
                                    requiredFieldMissing = true;
                                }

                                if(!requiredFieldMissing)
                                {
                                    return IApp.VALUE_SUCCESS;
                                }
                            }   
                        }
                    }
                }
            }

            return IApp.VALUE_FAILURE;
        }
    }

}
