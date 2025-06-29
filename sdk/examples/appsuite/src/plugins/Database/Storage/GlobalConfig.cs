using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using ConfigTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Configs;

namespace Metreos.ApplicationSuite.Storage
{
	/// <summary>
	///     Provides data access to the Configuration table.
	/// </summary>
	public class Config : DbTable
	{
		public Config(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        public string GetConfigValue(ConfigurationName configName)
        {
            string fieldName = SqlConstants.GetConfigName(configName);

            SqlBuilder builder = new SqlBuilder(Method.SELECT, ConfigTable.TableName);
            builder.fieldNames.Add(ConfigTable.Value);
            builder.where[ConfigTable.Name] = fieldName;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            string configValue = null;

            if(result.result == ReadResult.Success)
            {
                DataTable data = result.results;

                if(data == null || data.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Error, "The Configuration table does not have the " + 
                        "configuration value '{0}'", configName);
                    configValue = null;
                }
                else if(data.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Error, "The Configuration table has more than one " + 
                        "configuration entry for '{0}'", configName);
                }

                configValue = data.Rows[0][ConfigTable.Value].ToString();
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetConfigValue method in attempting to access the configuration table.\n" +
                    "Error message: {0}", result.e.Message);
                configValue = null;
            }

            return configValue;
        }
	}
}
