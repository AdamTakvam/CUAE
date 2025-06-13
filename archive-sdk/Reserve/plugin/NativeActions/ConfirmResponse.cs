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
    public class ConfirmResponse : INativeAction
    {
        private static XmlSerializer saveReq = new XmlSerializer(typeof(SaveBoRequest));
        private static XmlSerializer saveRes = new XmlSerializer(typeof(SaveBoResponse));
        private static XmlDocument doc = new XmlDocument();

        [ActionParamField("Host", "The host of the Tririga Webservice" , true)]
        public string Host { set { host = value; } }
        private string host;

        [ActionParamField("Port", "The port of the Tririga Webservice", false)]
        public string Port { set { port = value; } }
        private string port; 

        [ActionParamField("Secure", "Connect securely to Tririga Webservice", true)]
        public bool Secure { set { secure = value; } }
        private bool secure;

        [ActionParamField("SignOn UserID", "Used to access webservices, once logged in", true)]
        public long SignOnUserId { set { signOnUserId = value; } }
        private long signOnUserId; 

        [ActionParamField("SecurityToken", "Used to access webservices, once logged in", true)]
        public string SecurityToken { set { securityToken = value; } }
        private string securityToken; 
       
        [ActionParamField("Proxy URL", "An HTTP proxy server", false)]
        public string ProxyURL { set { proxyUrl = value; } }
        private string proxyUrl;

        [ActionParamField("ModuleID", "The static Module ID", true)]
        public string ModuleId { set { moduleId = value; } }
        private string moduleId; 

        [ActionParamField("CompanyID", "The Company ID (legacy)", true)]
        public long CompanyId { set { companyId = value; } }
        private long companyId; 
        
        [ActionParamField("NewRecordID", "Used to indicate that a new record is being created", true)]
        public string NewRecordId { set { newRecordId = value; } }
        private string newRecordId; 

        [ActionParamField("RecordID", "Used to indicate the ID of the record", true)]
        public string RecordId { set { recordId = value; } }
        private string recordId; 

        [ActionParamField("BoId", "BoId", true)]
        public string BoId { set { boId = value; } }
        private string boId; 

        [ActionParamField("BoName", "BoName", true)]
        public string BoName { set { boName = value; } }
        private string boName; 

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            signOnUserId = -1;
            companyId = -1;
            newRecordId = null;
            moduleId = null;
            recordId = null;
            boName = null;
            boId = null;
            securityToken = null;
            host = null;
            port = null;
            secure = false;
            proxyUrl = null;
        }

        public ConfirmResponse()
        {
            Clear();    
        }

        public class StringWriterPlus : StringWriter
        {
            public StringWriterPlus(StringBuilder sb) : base(sb) { }
            public override Encoding Encoding
            {
                get
                {
                    return System.Text.Encoding.GetEncoding("ISO-8859-1");
                }
            }

        }

        public bool ValidateInput()
        {
            return true;
        }

        [Action("ConfirmResponse", false, "Confirm Response", "Saves a BO Record on Tririga for a User Confirmation")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            #region Check Parameters for Correctness
            bool invalidParams = false;
            if(securityToken == null || securityToken == String.Empty )
            {
                log.Write(TraceLevel.Error, "Invalid security token specified (empty)");
                invalidParams = true;
            }

            if(signOnUserId == -1)
            {
                log.Write(TraceLevel.Error, "Invalid SignOnUserId specified (empty)");
                invalidParams = true;
            }

            if(invalidParams)
            {
                return IApp.VALUE_FAILURE;
            }
            #endregion

            BoRecordActionsInterfaceService saveService = new BoRecordActionsInterfaceService();
            saveService.RequestEncoding = new System.Text.UTF8Encoding(false);
            saveService.SetUrl(host, port, proxyUrl, secure);
            log.Write(TraceLevel.Info, "URL of SaveBo: " + saveService.Url);
            
            // Formulate SaveBo Request
            SaveBoRequest request = new SaveBoRequest();
            request.BoRecord = new BoRecordRequest[1];
            BoRecordRequest boRecord = new BoRecordRequest();
            request.BoRecord[0] = boRecord;
            boRecord.CompanyId = companyId.ToString();
            boRecord.RecordId = newRecordId;
            boRecord.ModuleId = moduleId;
            boRecord.BoName = boName;
            boRecord.BoId = boId;
            General general = new General();
            general.triRecordIdTX = recordId;
            general.enyCheckInTX = "Yes";
            boRecord.General = general;

            bool formedRequest = true;
            string requestXml = null;
            try
            {
                using(MemoryStream stream = new MemoryStream())
                {
                    XmlTextWriter writer = new XmlTextWriter(stream, new System.Text.UTF8Encoding(false));
                    saveReq.Serialize(writer, request);
                    stream.Position = 0;
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        requestXml = reader.ReadToEnd();
                    }
                }
                
                log.Write(TraceLevel.Verbose, requestXml);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to serialize the request XML.  \n" + 
                    "CompanyID: {0}, RecordID: {1}, ModuleId: {2}, BoName: {3}, BoId: {4}, TriRecordIdTX {5}, EnyCheckInTx: {6}\n{7}", companyId, newRecordId, moduleId, boName, boId, recordId, "Yes", e);
                formedRequest = false;
            }

            if(formedRequest)
            {
                bool communicationSuccess = true;
                string responseXml = null;
                try
                {
                    responseXml = saveService.saveBoRecordIntegration(securityToken, signOnUserId, companyId, requestXml);
                }
                catch(System.Web.Services.Protocols.SoapException se)
                {
                    log.Write(TraceLevel.Error, "Soap error in the SaveBoRecord Web Service.\n" + 
                        "Detail: {0}\n{1}", se.Detail.InnerXml, se);
                    communicationSuccess = false;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "General failure in the SaveBoRecord Web Service.\n" + e);
                    communicationSuccess = false;
                }

                if(communicationSuccess)
                {
                    bool foundResponse = true;
                    if(responseXml == null || responseXml == String.Empty)
                    {
                        log.Write(TraceLevel.Error, "The response XML from the SaveBoRecord Web Service method was empty");
                        foundResponse = false;
                    }
                    
                    if(foundResponse)
                    {
                        bool validResponseXml = true;
                        SaveBoResponse response = null;
                        StringReader reader = null;
                        try
                        {
                            reader = new StringReader(responseXml);
                            response = saveRes.Deserialize(reader) as SaveBoResponse;
                        }
                        catch(Exception e)
                        {
                            log.Write(TraceLevel.Error, "The response XML from the SaveBoRecord Web Service was not deserializable\n" + responseXml + "\n" + e);
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
                            bool foundMessage = true;
                            if(response == null || response.BoRecord == null || response.BoRecord.Length != 1 || response.BoRecord[0].Message == null || response.BoRecord[0].Message.InnerText == null)
                            {
                                log.Write(TraceLevel.Error, "The response XML from the SaveBoRecord Web Service was malformed\n" + responseXml);
                                foundMessage = false;
                            }

                            if(foundMessage)
                            {
                                string responseString = response.BoRecord[0].Message.InnerText.ToLower();

                                bool foundSuccess = false;
                                bool foundFailure = false;
                                if(responseString.IndexOf("successfully saved") > -1)
                                {
                                    foundSuccess = true;
                                }
                                if(responseString.IndexOf("failed") > -1)
                                {
                                    foundFailure = true;
                                }

                                bool nonAmbigousResponse = true;
                                if(foundSuccess && foundFailure) 
                                {
                                    log.Write(TraceLevel.Error, "Ambiguous response from Tririga in SaveBoRecord Web Service: " + responseString);
                                    nonAmbigousResponse = false;
                                }

                                if(nonAmbigousResponse)
                                {
                                    bool deterministicResponse = true;
                                    if(!foundSuccess && !foundFailure)
                                    {
                                        log.Write(TraceLevel.Error, "Non-deterministic response from Tririga in SaveBoRecord Web Service: " + responseString);
                                        deterministicResponse = false;
                                    }

                                    if(deterministicResponse)
                                    {
                                        if(foundFailure)
                                        {
                                            log.Write(TraceLevel.Error, "SaveBoRecord Web Service responded with failure: " + responseString);
                                        }
                                        else
                                        {
                                            return IApp.VALUE_SUCCESS;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
                
            log.Write(TraceLevel.Error, "Unable to save BO with RecordID " + recordId);
            return IApp.VALUE_FAILURE;
        }
    }
}
