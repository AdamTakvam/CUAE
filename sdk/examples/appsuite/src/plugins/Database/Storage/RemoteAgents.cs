using System;
using System.Data;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using RemoteAgentsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.RemoteAgents;
using DevicesTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Devices;
using ExtNumTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.ExternalNumbers;

namespace Metreos.ApplicationSuite.Storage
{
	/// <summary>
	/// This class contains various access methods that can be used to retrieve information from the as_remote_agents table
	/// </summary>
	public class RemoteAgents : DbTable
	{
        public RemoteAgents(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        public bool GetRemoteAgentByDevice(string deviceName, out uint remoteAgentId, out uint userId, out string externalNumber, out uint user_level)
        {
            remoteAgentId = userId = user_level = 0;
            externalNumber = string.Empty;

            if (deviceName == string.Empty || deviceName == null)
            {
                log.Write(TraceLevel.Warning, 
                            "Error encountered in the GetRemoteAgentByDevice method, the value of the DeviceName parameter is: " +
                            (deviceName == null ? "NULL" : "empty"));
                return false;
            }

            Devices devicesDbAccess = new Devices(this);
            uint deviceId;
            bool success = devicesDbAccess.GetDeviceIdByDeviceName(deviceName, out deviceId);
            if (!success)
                return false;
            
            SqlBuilder builder = new SqlBuilder(Method.SELECT, RemoteAgentsTable.TableName);
            builder.fieldNames.Add(RemoteAgentsTable.Id);
            builder.fieldNames.Add(RemoteAgentsTable.UserId);
            builder.fieldNames.Add(RemoteAgentsTable.ExternalNumber);
            builder.fieldNames.Add(RemoteAgentsTable.AgentUserLevel);
            builder.where[RemoteAgentsTable.DeviceId] = deviceId;
            
            uint externalNumbersId;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;
                
                if (table == null || table.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Info, 
                        "Error encountered in the GetRemoteAgentByDevice method, could not find a Remote Agent entry for device: " + 
                        deviceName);
                    return false;
                }
                else if (table.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Warning, 
                        "Error encountered in the GetRemoteAgentByDevice method, found multiple Remote Agent entries for device: " + 
                        deviceName);
                    return false;                    
                }

                DataRow row = table.Rows[0];
                userId              = Convert.ToUInt32(row[RemoteAgentsTable.UserId]);

                if (Convert.IsDBNull(row[RemoteAgentsTable.ExternalNumber]))
                {
                    log.Write(TraceLevel.Warning, "No external numbers defined for user with userId '" + userId + "' associated with device: " +
                        deviceName);
                    return false;
                }
                externalNumbersId   = Convert.ToUInt32(row[RemoteAgentsTable.ExternalNumber]);

                remoteAgentId       = Convert.ToUInt32(row[RemoteAgentsTable.Id]);
                user_level          = Convert.ToUInt32(row[RemoteAgentsTable.AgentUserLevel]);

                
                string message = string.Empty;
                if (userId == 0)
                {    
                    success = false;
                    message += "UserId field is '0'. ";
                }
                if ( ! Enum.IsDefined(typeof(Metreos.ApplicationSuite.Storage.RemoteAgentUserLevel), user_level))
                {
                    success = false;
                    message += " Remote Agent user level '" + user_level + "' is invalid. ";
                }
                if (externalNumbersId == 0)
                {
                    success = false;
                    message += " reference to the external numbers table is invalid. Does the user have an external number configured for use " +
                        "with Remote Agent?";
                }
                if ( ! success)
                {
                    log.Write(TraceLevel.Warning, "Error encountered in the GetRemoteAgentByDevice method, retrieved " + message);
                    return false; 
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "An exception was thrown in the GetRemoteAgentByDevice method while obtaining agent info. " +
                    "Exception message is: " + result.e.Message);
                return false;
            }

            builder = new SqlBuilder(Method.SELECT, ExtNumTable.TableName);
            builder.fieldNames.Add(ExtNumTable.PhoneNumber);
            builder.where[ExtNumTable.Id] = externalNumbersId;

            result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if (table == null || table.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Warning, "Error encountered in the GetRemoteAgentByDevice method, could not retrieve external " +
                        "number with primary key: " + externalNumbersId);
                    return false;
                }
                
                DataRow row = table.Rows[0];

                if (Convert.IsDBNull(row[ExtNumTable.PhoneNumber]))
                {
                    externalNumber = string.Empty;
                    return false;
                }

                externalNumber = Convert.ToString(row[ExtNumTable.PhoneNumber]);
                if (externalNumber == string.Empty)
                {
                    log.Write(TraceLevel.Warning, 
                        "Error encountered in the GetRemoteAgentByDevice method, the value of retrieved PhoneNumber parameter is: " +
                        (externalNumber == null ? "NULL" : "empty"));
                    externalNumber = string.Empty;
                    return false;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "An exception was thrown in the GetRemoteAgentByDevice method while trying to obtain number " +
                    "to dial. Exception message is: " + result.e.Message);
                externalNumber = string.Empty;
                return false;
            }

            return true;
        }

        public bool GetRemoteAgentLevel(uint userId, out uint userLevel)
        {
            userLevel = 0;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, RemoteAgentsTable.TableName);
            builder.fieldNames.Add(RemoteAgentsTable.AgentUserLevel);
            builder.where[RemoteAgentsTable.UserId] = userId;

            ReadResultContainer result = ExecuteScalar(builder);

            if(result.result == ReadResult.Success)
            {
                userLevel = Convert.ToUInt32(result.scalar);
                return true;
            }
            else
            {
                log.Write(TraceLevel.Error, "An exception was thrown in the GetRemoteAgentLevel method while trying to obtain Remote Agent user " +
                    "level for userId: " + userId + ". Exception message is: " + result.e.Message);
                return false;
            }
        }
	}
}
