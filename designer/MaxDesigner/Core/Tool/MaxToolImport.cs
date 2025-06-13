using System;
using System.ComponentModel;
using Metreos.Max.Core.Package;
using Metreos.Max.Framework.Satellite.Property;
using PropertyGrid.Core;


namespace Metreos.Max.Core.Tool
{
    /// <summary>Samoa importer</summary>
    public class MaxToolImport
    {
        #region singleton
        private static readonly MaxToolImport instance = new MaxToolImport();
        public  static MaxToolImport Instance { get { return instance; }}
        private MaxToolImport() {} 
        #endregion

        public static PropertyGrid.Core.MaxPmAction ImportAction
        ( Metreos.PackageGeneratorCore.PackageXml.actionType xmlAction)
        {
            MaxPmAction pmAction;

            Iconinfo.Reset();
            int iconCount = xmlAction.icon == null? 0: xmlAction.icon.Length;

            for(int i = 0; i < iconCount; i++)
                package.GetCurrentIcon(xmlAction.icon[i]);                                                

            // Construct ActionParameters
            int paramCount = xmlAction.actionParam == null? 0: xmlAction.actionParam.Length;
            
            ActionParameter[] parameters = new ActionParameter[paramCount];

            for(int i = 0; i < paramCount; i++)
            {
                PackageGeneratorCore.PackageXml.actionParamType xmlaction = xmlAction.actionParam[i];
                string name = xmlaction.name;
                string displayName = xmlaction.displayName != null ? xmlaction.displayName : name;

                string description = xmlaction.description != null ? 
                    xmlaction.description : xmlaction.name;

                string type = xmlaction.type;
                
                Use use = xmlaction.use == Metreos.PackageGeneratorCore.PackageXml.useType.required ?
                    Use.required : Use.optional;

                parameters[i] = new ActionParameter(name, displayName, description, xmlaction.EnumItem, type, xmlaction.allowMultiple, use);
            }
     
            // Construct ResultData
            int resultDatumCount = xmlAction.resultData == null? 0: xmlAction.resultData.Length;
            
            ResultDatum[] resultData = new ResultDatum[resultDatumCount];

            for(int i = 0; i < resultDatumCount; i++)
            {
                PackageGeneratorCore.PackageXml.resultDataType xmldata = xmlAction.resultData[i];
                string name = xmldata.Value;
                string displayName = xmldata.displayName != null? xmldata.displayName : name;

                string description = xmldata.description != null ? 
                    xmldata.description : xmldata.Value;

                string type = xmldata.type;
                   
                resultData[i] = new ResultDatum(name, displayName, description, type);
            }   
        
            ReturnValue returnValue = xmlAction.returnValue == null? new ReturnValue():                  
                new ReturnValue(xmlAction.returnValue.description, xmlAction.returnValue.EnumItem);   

            // Construct the actionType

            ActionType actionType;

            switch(xmlAction.type)
            {
                case Metreos.PackageGeneratorCore.PackageXml.actionTypeType.appControl:
                    actionType = ActionType.appControl;
                    break;

                case Metreos.PackageGeneratorCore.PackageXml.actionTypeType.native:
                    actionType = ActionType.native;
                    break;

                case Metreos.PackageGeneratorCore.PackageXml.actionTypeType.provider:
                    actionType = ActionType.provider;
                    break;

                default:
                    actionType = ActionType.native;
                    break;
            }

            // Construct the simple type properties
            string actionName = xmlAction.name;
            string actionDisplayName = xmlAction.displayName;
            string packageName = package.Name;
            string actionDescription = xmlAction.description  != null ? xmlAction.description : actionName;
            string[] asyncCallbacks = xmlAction.asyncCallback != null ? xmlAction.asyncCallback : null;
            bool allowCustomParams = xmlAction.allowCustomParams;
            bool final = xmlAction.final;
            
            pmAction = new MaxPmAction(actionName, actionDisplayName, packageName, actionDescription,
                asyncCallbacks, allowCustomParams, final, parameters,
                resultData, returnValue, actionType);

            pmAction.Tag = Iconinfo; 
            
            return pmAction;
        }


        public static PropertyGrid.Core.MaxPmEvent ImportEvent
        (Metreos.PackageGeneratorCore.PackageXml.eventType xmlEvent)
        {
            MaxPmEvent pmEvent;

            Iconinfo.Reset();
            int iconCount = xmlEvent.icon == null? 0: xmlEvent.icon.Length;

            for(int i = 0; i < iconCount; i++)
                package.GetCurrentIcon(xmlEvent.icon[i]); 


            // Construct EventParameters
            int paramCount = xmlEvent.eventParam == null? 0: xmlEvent.eventParam.Length;

            EventParameter[] parameters = new EventParameter[paramCount];

            for(int i = 0; i < paramCount; i++)
            { 
                PackageGeneratorCore.PackageXml.eventParamType eventParam = xmlEvent.eventParam[i];
                string name = eventParam.name;
                string displayName = eventParam.displayName != null ? eventParam.displayName: name;
                string description = eventParam.description != null ? eventParam.description : name;
                string type = eventParam.type != null ? eventParam.type : Defaults.TYPE;
                bool guaranteed = eventParam.guaranteed;

                parameters[i] = new EventParameter(name, displayName, description, eventParam.EnumItem, type, guaranteed);
            }
            
            // Construct type

            EventType eventType;

            switch(xmlEvent.type)
            {
                case Metreos.PackageGeneratorCore.PackageXml.eventTypeType.triggering:
                    eventType = EventType.triggering;
                    break;

                case Metreos.PackageGeneratorCore.PackageXml.eventTypeType.nontriggering:
                    eventType = EventType.nonTriggering;
                    break;

                case Metreos.PackageGeneratorCore.PackageXml.eventTypeType.hybrid:
                    eventType = EventType.hybrid;
                    break;

                case Metreos.PackageGeneratorCore.PackageXml.eventTypeType.asyncCallback:
                    eventType = EventType.asyncCallback;
                    break;

                default:
                    eventType = EventType.hybrid;
                    break;
            }
            
            string eventName = xmlEvent.name;
            string eventDescription = xmlEvent.description != null ? xmlEvent.description : eventName;
            string expects = xmlEvent.expects != null ? xmlEvent.expects : null;

            pmEvent = new MaxPmEvent(eventName, eventDescription, expects, eventType, parameters);
            pmEvent.Tag = Iconinfo;

            return pmEvent;
        }

        private static MaxPackage package;
        public  static MaxPackage Package { get { return package; } set { package = value; } }
        public  static MaxPackage.IconInfo Iconinfo { get { return package.Iconinfo; } }

    } // class MaxToolImport

}   // namespace
