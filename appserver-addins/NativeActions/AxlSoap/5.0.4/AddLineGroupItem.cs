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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504.Actions.AddLineGroupItem;

namespace Metreos.Native.AxlSoap504
{
    /// <summary> Creates a line item for the 'updateLineGroup' AXL SOAP method for Cisco CallManager 5.0.4 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.PACKAGE_DESCRIPTION)]
    public class AddLineGroupItem : INativeAction
    {    

        [ActionParamField(Package.Params.Pattern.DISPLAY, Package.Params.Pattern.DESCRIPTION, false, Package.Params.Pattern.DEFAULT)]
        public string Pattern { set { pattern = value; } }

		[ActionParamField(Package.Params.RoutePartitionName.DISPLAY, Package.Params.RoutePartitionName.DESCRIPTION, false, Package.Params.RoutePartitionName.DEFAULT)]
		public string RoutePartitionName { set { routePartitionName = value; } }

		[ActionParamField(Package.Params.RoutePartitionId.DISPLAY, Package.Params.RoutePartitionId.DESCRIPTION, false, Package.Params.RoutePartitionId.DEFAULT)]
		public string RoutePartitionId { set { routePartitionId = value; } }

		[ActionParamField(Package.Params.DirectoryNumberId.DISPLAY, Package.Params.DirectoryNumberId.DESCRIPTION, false, Package.Params.DirectoryNumberId.DEFAULT)]
		public string DirectoryNumberId { set { directoryNumberId = value; } }

		[ActionParamField(Package.Params.LineSelectionOrder.DISPLAY, Package.Params.LineSelectionOrder.DESCRIPTION, true, Package.Params.LineSelectionOrder.DEFAULT)]
		public string LineSelectionOrder { set { lineSelectionOrder = value; } }

        [ResultDataField(Package.Results.LineGroupItem.DISPLAY, Package.Results.LineGroupItem.DESCRIPTION)]
        public XLineGroupMember LineGroupItem { get { return member; } }

        public LogWriter Log { set { log = value; } }

		private string pattern;
		private string routePartitionName;
		private string routePartitionId;
		private string directoryNumberId;
		private string lineSelectionOrder;
		private XLineGroupMember member;
        private LogWriter log;
        
        public AddLineGroupItem()
        {
            Clear();	
        }

        public void Clear()
        {
           
			this.pattern					= null;
			this.routePartitionName			= null;
			this.routePartitionId			= null;
			this.directoryNumberId			= null;
			this.lineSelectionOrder			= null;
			this.directoryNumberId          = null;
            this.member                     = new XLineGroupMember();
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
			member = new XLineGroupMember();
            
			if(directoryNumberId != null)
			{
				XNPDirectoryNumber dn = new XNPDirectoryNumber();
				dn.uuid = directoryNumberId;
				member.Item = dn;
			}
			else
			{
				XLineGroupMemberDnPatternAndPartition pattPart = new XLineGroupMemberDnPatternAndPartition();
				pattPart.dnPattern = pattern;
				pattPart.Item = IAxlSoap.DetermineChosenBetweenStrings(routePartitionName, routePartitionId); 
				pattPart.ItemElementName = (ItemChoiceType4) IAxlSoap.DetermineChosenBetweenStringsType(
					routePartitionName, routePartitionId, ItemChoiceType4.routePartitionName, ItemChoiceType4.routePartitionId);
				member.Item = pattPart;

			}

            member.lineSelectionOrder = lineSelectionOrder;
            
            return IApp.VALUE_SUCCESS;
        }   
    }
}
