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
    public class GetConfigValue : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("FieldName", false)]
        public ConfigurationName FieldName { set { fieldName = value; } }
        private ConfigurationName fieldName;

        [ActionParamField("FieldName", false)]
        public ConfigurationName FieldName2 { set { fieldName2 = value; } }
        private ConfigurationName fieldName2;

        [ActionParamField("FieldName", false)]
        public ConfigurationName FieldName3 { set { fieldName3 = value; } }
        private ConfigurationName fieldName3;

        [ActionParamField("FieldName", false)]
        public ConfigurationName FieldName4 { set { fieldName4 = value; } }
        private ConfigurationName fieldName4;

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

        public GetConfigValue() 
        {
            Clear();
        }

        public bool ValidateInput()
        {   
            return true;
        }

        public void Clear()
        {
            fieldName       = ConfigurationName.Unspecified;
            fieldName2      = ConfigurationName.Unspecified;
            fieldName3      = ConfigurationName.Unspecified;
            fieldName4      = ConfigurationName.Unspecified;
            fieldValue      = null;
            fieldValue2     = null;
            fieldValue3     = null;
            fieldValue4     = null;
        }

        [Action("GetConfigValue", false, "Get Config Value", "Returns a system configuration value based on requested system configuration name")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            using(Config global = new Config(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = true;

                if(fieldName != ConfigurationName.Unspecified)
                {
                    fieldValue   = global.GetConfigValue(fieldName);
                    success     &= fieldValue != null;
                }
                if(fieldName2 != ConfigurationName.Unspecified)
                {
                    fieldValue2  = global.GetConfigValue(fieldName2);
                    success     &= fieldValue2 != null;
                }
                if(fieldName3 != ConfigurationName.Unspecified)
                {
                    fieldValue3  = global.GetConfigValue(fieldName3);
                    success     &= fieldValue3 != null;
                }
                if(fieldName4 != ConfigurationName.Unspecified)
                {
                    fieldValue4  = global.GetConfigValue(fieldName4);
                    success     &= fieldValue4 != null;
                }

                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }
    }
}
