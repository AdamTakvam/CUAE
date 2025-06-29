using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    ///     Returns a system configuration value(s) based on requested system configuration name(s)
    /// </summary>
    [PackageDecl("Metreos.ApplicationSuite.Actions")]
    public class GetSystemConfigValue : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("FieldName", false)]
        public SystemConfigurationName FieldName { set { fieldName = value; } }
        private SystemConfigurationName fieldName;

        [ActionParamField("FieldName", false)]
        public SystemConfigurationName FieldName2 { set { fieldName2 = value; } }
        private SystemConfigurationName fieldName2;

        [ActionParamField("FieldName", false)]
        public SystemConfigurationName FieldName3 { set { fieldName3 = value; } }
        private SystemConfigurationName fieldName3;

        [ActionParamField("FieldName", false)]
        public SystemConfigurationName FieldName4 { set { fieldName4 = value; } }
        private SystemConfigurationName fieldName4;

        [ResultDataField("FieldValue")]
        public  string FieldValue { get { return fieldValue; } }
        private string fieldValue;

        [ResultDataField("FieldValue")]
        public  string FieldValue2 { get { return fieldValue2; } }
        private string fieldValue2;

        [ResultDataField("FieldValue")]
        public  string FieldValue3 { get { return fieldValue3; } }
        private string fieldValue3;

        [ResultDataField("FieldValue")]
        public  string FieldValue4 { get { return fieldValue4; } }
        private string fieldValue4;

        public GetSystemConfigValue() 
        {
            Clear();
        }

        public bool ValidateInput()
        {   
            return true;
        }

        public void Clear()
        {
            fieldName       = SystemConfigurationName.Unspecified;
            fieldName2      = SystemConfigurationName.Unspecified;
            fieldName3      = SystemConfigurationName.Unspecified;
            fieldName4      = SystemConfigurationName.Unspecified;
            fieldValue      = null;
            fieldValue2     = null;
            fieldValue3     = null;
            fieldValue4     = null;
        }

        [Action("GetSystemConfigValue", false, "Get System Config Value", "Returns a system configuration value based on requested system configuration name")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            GlobalConfig global = new GlobalConfig(
                sessionData.DbConnections[SqlConstants.DbConnectionName],
                log,
                sessionData.AppName,
                sessionData.PartitionName);
    
            bool success = true;

            if(fieldName != SystemConfigurationName.Unspecified)
            {
                fieldValue   = global.GetGlobalValue(fieldName);
                success     &= fieldValue != null;
            }
            if(fieldName2 != SystemConfigurationName.Unspecified)
            {
                fieldValue2  = global.GetGlobalValue(fieldName2);
                success     &= fieldValue2 != null;
            }
            if(fieldName3 != SystemConfigurationName.Unspecified)
            {
                fieldValue3  = global.GetGlobalValue(fieldName3);
                success     &= fieldValue3 != null;
            }
            if(fieldName4 != SystemConfigurationName.Unspecified)
            {
                fieldValue4  = global.GetGlobalValue(fieldName4);
                success     &= fieldValue4 != null;
            }

            if(success) return IApp.VALUE_SUCCESS;
            else        return IApp.VALUE_FAILURE;
        }
    }
}
