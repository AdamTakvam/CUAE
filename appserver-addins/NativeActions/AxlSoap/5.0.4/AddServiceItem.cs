using System;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;

using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Metreos.AxlSoap;
using Metreos.AxlSoap504;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504.Actions.AddServiceItem;

namespace Metreos.Native.AxlSoap504
{
    /// <summary> Creates a service item for the 'updatePhone' AXL SOAP method for Cisco CallManager 5.0.4 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.PACKAGE_DESCRIPTION)]
    public class AddServiceItem : INativeAction
    {    
        [ActionParamField(Package.Params.Name.DISPLAY, Package.Params.Name.DESCRIPTION, false, Package.Params.Name.DEFAULT)]
        public string Name { set { name = value; } }

        [ActionParamField(Package.Params.Url.DISPLAY, Package.Params.Url.DESCRIPTION, false, Package.Params.Url.DEFAULT)]
        public string Url { set { url = value; } }

        [ActionParamField(Package.Params.UrlButtonIndex.DISPLAY, Package.Params.UrlButtonIndex.DESCRIPTION, false, Package.Params.UrlButtonIndex.DEFAULT)]
        public string UrlButtonIndex { set { urlButtonIndex = value; } }

        [ActionParamField(Package.Params.UrlLabel.DISPLAY, Package.Params.UrlLabel.DESCRIPTION, false, Package.Params.UrlLabel.DEFAULT)]
        public string UrlLabel { set { urlLabel = value; } }

        [ActionParamField(Package.Params.Uuid.DISPLAY, Package.Params.Uuid.DESCRIPTION, false, Package.Params.Uuid.DEFAULT)]
        public string Uuid { set { uuid = value; } }

        [ActionParamField(Package.Params.TelecasterServiceName.DISPLAY, Package.Params.TelecasterServiceName.DESCRIPTION, false, Package.Params.TelecasterServiceName.DEFAULT)]
        public string TelecasterServiceName { set { telecasterServiceName = value; } }

        [ActionParamField(Package.Params.TelecasterServiceId.DISPLAY, Package.Params.TelecasterServiceId.DESCRIPTION, false, Package.Params.TelecasterServiceId.DEFAULT)]
        public string TelecasterServiceId { set { telecasterServiceId = value; } }

        [ResultDataField(Package.Results.Service.DISPLAY, Package.Results.Service.DESCRIPTION)]
        public XSubscribedService Service { get { return service; } }

        public LogWriter Log { set { log = value; } }

        private string telecasterServiceName;
        private string telecasterServiceId;
        private string name;
        private string url;
        private string urlButtonIndex;
        private string urlLabel;
        private string uuid;
 
        private XSubscribedService service;
        private LogWriter log;
        
        public AddServiceItem()
        {
            Clear();	
        }

        public void Clear()
        {
            this.name = null;
            this.url = String.Empty;
            this.urlButtonIndex = null;
            this.urlLabel = null;
            this.uuid = null;
            this.telecasterServiceId = null;
            this.telecasterServiceName = null;
            this.service = new XSubscribedService();
        }

        public bool ValidateInput()
        {
            return true;
        } 

        public enum Result
        {
            success
        }

        [ReturnValue(typeof(Result), "Only success")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {  
            service = new XSubscribedService();
            service.name = name;
            service.url = url;
            service.urlButtonIndex = urlButtonIndex;
            service.urlLabel = urlLabel;
            service.uuid = uuid;
            
            if(telecasterServiceId != null)
            {
                XTelecasterService serviceId = new XTelecasterService();
                serviceId.uuid = telecasterServiceId;
                service.Item = serviceId;
            }
            else
            {
                service.Item = telecasterServiceName;
            }

            return IApp.VALUE_SUCCESS;
        }   
    }
}
