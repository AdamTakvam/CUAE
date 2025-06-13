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
    public class ReleaseResponse : INativeAction
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

        [ActionParamField("SecurityToken", "Used to access webservices, once logged in", true)]
        public string SecurityToken { set { securityToken = value; } }
        private string securityToken; 
        
        [ActionParamField("SignOn UserID", "Used to access webservices, once logged in", true)]
        public long SignOnUserId { set { signOnUserId = value; } }
        private long signOnUserId; 
       
        [ActionParamField("Proxy URL", "An HTTP proxy server", false)]
        public string ProxyURL { set { proxyUrl = value; } }
        private string proxyUrl;

        [ActionParamField("CompanyID", "Used to access webservices, once logged in", true)]
        public long CompanyId { set { companyId = value; } }
        private long companyId; 
        
        [ActionParamField("RecordID", "Used to indicate which record to update to Tririga", true)]
        public string RecordId { set { recordId = value; } }
        private string recordId; 

        [ActionParamField("ResultCode", "The Metreos result code of a failure.  Defaults to nothing (null)", false)]
        public string ResultCode { set { resultCode = value; } }
        private string resultCode; 

        [ActionParamField("ResultMessage", "The message complimenting a Metreos result code.  Defaults to nothing (null)", false)]
        public string ResultMessage { set { resultMessage = value; } }
        private string resultMessage; 

        [ActionParamField("DiagnosticCode", "The CCM result code of a failure.  Defaults to nothing (null)", false)]
        public string DiagnosticCode { set { diagnosticCode = value; } }
        private string diagnosticCode; 

        [ActionParamField("DiagnosticCode", "The message complimenting a CCM result code.  Defaults to nothing (null)", false)]
        public string DiagnosticMessage { set { diagnosticMessage = value; } }
        private string diagnosticMessage; 

        [ActionParamField("LoggedInUser", "The logged in user on EM code XX.  Defaults to nothing (null)", false)]
        public string LoggedInUser { set { loggedInUser  = value; } }
        private string loggedInUser; 

        [ActionParamField("LoggedInDevice", "The logged in device on EM code XX.  Defaults to nothing (null)", false)]
        public string LoggedInDevice { set { loggedInDevice  = value; } }
        private string loggedInDevice; 

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            companyId = -1;
            signOnUserId = -1;
            host = null;
            port = null;
            recordId = null;
            securityToken = null;
            secure = false;
            resultCode = null;
            resultMessage = null;
            diagnosticCode = null;
            diagnosticMessage = null;
            loggedInUser = null;
            loggedInDevice = null;
            proxyUrl = null;
        }

        public ReleaseResponse()
        {
            Clear();    
        }

        
        public bool ValidateInput()
        {
            return true;
        }

        [Action("ReleaseResponse", false, "Release Response", "Saves a BO Record on Tririga for a Release Request")]
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

            if(companyId == -1)
            {
                log.Write(TraceLevel.Error, "Invalid CompanyID specified (empty)");
                invalidParams = true;
            }

            if(recordId == null)
            {
                log.Write(TraceLevel.Error, "Invalid RecordID specified (empty)");
                invalidParams = true;
            }

            if(invalidParams)
            {
                return IApp.VALUE_FAILURE;
            }
            #endregion

            BoRecordActionsInterfaceService saveService = new BoRecordActionsInterfaceService();
            saveService.SetUrl(host, port, proxyUrl, secure);
            
            // Formulate SaveBo Request
            SaveBoRequest request = new SaveBoRequest();
            request.BoRecord = new BoRecordRequest[1];
            BoRecordRequest boRecord = new BoRecordRequest();
            boRecord.CompanyId = companyId.ToString();
            boRecord.RecordId = recordId;
            RecordInformation recInfo = new RecordInformation();
            recInfo.eyDiagnosticCodeTX = diagnosticCode == null ? null : doc.CreateCDataSection(diagnosticCode);
            recInfo.eyDiagnosticMessageTX = diagnosticMessage == null ? null : doc.CreateCDataSection(diagnosticMessage);
            recInfo.eyLoggedInDeviceTX = loggedInDevice == null ? null : doc.CreateCDataSection(loggedInDevice);
            recInfo.eyLoggedInUserTX = loggedInUser == null ? null : doc.CreateCDataSection(loggedInUser);
            recInfo.eyResultCodeTX = resultCode == null ? null : doc.CreateCDataSection(resultCode);
            recInfo.eyResultMessageTX = resultMessage == null ? null : doc.CreateCDataSection(resultMessage);
            boRecord.RecordInformation = recInfo;
            request.BoRecord[0] = boRecord;
            
            bool formedRequest = true;
            string requestXml = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                TextWriter writer = new StringWriter(sb);
                saveReq.Serialize(writer, request);
                writer.Close();
            
                requestXml = sb.ToString();
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to serialize the request XML.  \n" + 
                    "CompanyID: {0}, RecordID: {1}, DiagnosticCode: {2}, DiagnosticError: {3}, ResultCode: {4}, ResultMessage: {5}, LoggedInDevice: {6}, LoggedInUser: {7}\n{8}", companyId, recordId, diagnosticCode, diagnosticMessage, resultCode, resultMessage, loggedInDevice, loggedInUser, e);
                formedRequest = false;
            }

            if(formedRequest)
            {
                bool communicationSuccess = true;
                string responseXml = null;
                try
                {
                    responseXml = saveService.saveBoRecord(securityToken, signOnUserId, companyId, requestXml);
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
