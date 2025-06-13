//using System;
//using System.IO;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Net;
//using System.Web;
//using System.Web.Services.Protocols;
//
//using System.Data;
//using System.Collections;
//using System.Diagnostics;
//
//using Metreos.Core;
//using Metreos.Interfaces;
//using Metreos.LoggingFramework;
//using Metreos.ApplicationFramework;
//using Metreos.PackageGeneratorCore.Attributes;
//using Metreos.ApplicationFramework.Collections;
//
//using Metreos.AxlSoap;
//using Metreos.AxlSoap601;
//using Metreos.Types.AxlSoap601;
//
//namespace Metreos.Native.AxlSoap601
//{
//    /// <summary> Wraps up the additional, non-serializable 'updateTranslationPattern' AXL SOAP fields for Cisco CallManager 6.0.1 </summary>
//    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.PACKAGE_DESCRIPTION)]
//    public class UpdateTranslationPatternSecondary : INativeAction
//    {     
//        [ActionParamField(Package.Params.NewRouteFilterName.DISPLAY, Package.Params.NewRouteFilterName.DESCRIPTION, false, Package.Params.NewRouteFilterName.DEFAULT)]
//        public string NewRouteFilterName { set { newRouteFilterName = value;} }
//
//        [ActionParamField(Package.Params.NewRouteFilterId.DISPLAY, Package.Params.NewRouteFilterId.DESCRIPTION, false, Package.Params.NewRouteFilterId.DEFAULT)]
//        public string NewRouteFilterId { set { newRouteFilterId = value;} }
//          
//        [ActionParamField(Package.Params.NewRoutePartitionName.DISPLAY, Package.Params.NewRoutePartitionName.DESCRIPTION, false, Package.Params.NewRoutePartitionName.DEFAULT)]
//        public string NewRoutePartitionName { set { newRoutePartitionName = value; } }
//
//        [ActionParamField(Package.Params.NewRoutePartitionId.DISPLAY, Package.Params.NewRoutePartitionId.DESCRIPTION, false, Package.Params.NewRoutePartitionId.DEFAULT)]
//        public string NewRoutePartitionId { set { newRoutePartitionId = value; } }
//
//        [ActionParamField(Package.Params.Uuid.DISPLAY, Package.Params.Uuid.DESCRIPTION, true, Package.Params.Uuid.DEFAULT)]
//        public string Uuid { set { uuid = value; } }
//
//        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
//        public string CallManagerIP { set { callManagerIP = value; } }
//
//        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
//        public string AdminUsername { set { username = value; } }
//
//        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
//        public string AdminPassword { set { password = value; } }
//
//        [ResultDataField(Package.Results.UpdateTransPatternResponse.DISPLAY, Package.Results.UpdateTransPatternResponse.DESCRIPTION)]
//        public updateTransPatternResponse UpdateTransPatternResponse { get { return response; } }
//
//        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
//        public string FaultMessage { get { return message; } }
//
//        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
//        public int FaultCode { get { return code; } }
//
//        public LogWriter Log { set { log = value; } }
//
//        private string newRouteFilterName;
//        private string newRouteFilterId;
//      
//        #region Shared with UpdateLine (Important for future action integration, for UpdateRoutePattern, for instance)
//        private string newRoutePartitionName;
//        private string newRoutePartitionId;
//        private string uuid;
//        private string username;
//        private string password;
//        private string message;
//        private string callManagerIP;
//        private int code;
//        private LogWriter log;
//        #endregion
//        
//        private updateTransPatternResponse response;
//
//        public UpdateTranslationPatternSecondary()
//        {
//            Clear();	
//        }
//
//        public void Clear()
//        {
//            this.newRouteFilterName                     = null;
//            this.newRouteFilterId                       = null;
//            this.newRoutePartitionId                    = null;
//            this.newRoutePartitionName                  = null;
//            this.uuid                                   = null;
//            this.username                               = IAxlSoap.DefaultCcmAdmin;
//            this.password                               = null;
//            this.callManagerIP                          = null;
//            this.response                               = new updateTransPatternResponse();
//            this.message                                = String.Empty;
//            this.code                                   = 0;
//        }
//
//        public bool ValidateInput()
//        {
//            return true;
//        } 
//
//        public enum Result
//        {
//            success,
//            failure,
//            fault,
//        }
//
//        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
//        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
//        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
//        {
//            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
//                         
//            updateTransPattern2 trans = new updateTransPattern2();
//            
//            trans.Item1 = IAxlSoap.DetermineChosenBetweenStrings(newRoutePartitionName, newRoutePartitionId);
//            trans.Item1ElementName = (Item1ChoiceType13) IAxlSoap.DetermineChosenBetweenStringsType(
//                newRoutePartitionName, newRoutePartitionId, Item1ChoiceType13.routePartitionName, Item1ChoiceType13.routePartitionId);
//            trans.Item4 = IAxlSoap.DetermineChosenBetweenStrings(newRouteFilterName, newRouteFilterId);
//            trans.Item4ElementName = (Item4ChoiceType8) IAxlSoap.DetermineChosenBetweenStringsType(
//                newRouteFilterName, newRouteFilterId, Item4ChoiceType8.routeFilterName, Item4ChoiceType8.routeFilterId);
//            trans.uuid = uuid;
//                
//            try
//            {
//                response = client.updateTransPattern2(trans);
//            }
//            catch(System.Web.Services.Protocols.SoapException e)
//            {
//                IAxlSoap.ReportSoapError(e, log, ref code, ref message);
//
//                return Result.fault.ToString();
//            }
//            catch(Exception e)
//            {
//                log.Write(TraceLevel.Error, Metreos.Utilities.Exceptions.FormatException(e));
//                return IApp.VALUE_FAILURE;
//            }
//
//            return IApp.VALUE_SUCCESS;
//        }    
//    }
//}
