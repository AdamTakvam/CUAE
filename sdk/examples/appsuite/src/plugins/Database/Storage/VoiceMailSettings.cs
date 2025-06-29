using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using UserTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Users;
using VmSettings = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.VoiceMailSettings;
using Storage = Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the VoiceMail settings table
    /// </summary>
    public class VoiceMailSettings : DbTable
    {
        public class VoiceMailSettingsRecord
        {
            public uint id = 0;
            public bool isFirstLogin = false;
            public string greetingFilename = string.Empty;
            public SortOrder sortOrder = SortOrder.Increasing;
            public NotificationMethod notificationMethod = NotificationMethod.None;
            public UserStatus accountStatus = UserStatus.Ok;
            public string notificationAddress = string.Empty;
            public int maxMessageLength = 0;
            public uint maxNumberMessages = 0;
            public uint maxStorageDays = 0;
            public bool describeEachMessage = false;
        }

        public VoiceMailSettings(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }
 
        #region GetVoiceMailSettings
        public bool GetVoiceMailSettings(uint userId, out VoiceMailSettingsRecord voiceMailSettings)
        {
            voiceMailSettings = null;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, VmSettings.TableName);
            builder.where[VmSettings.UserId] = userId;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;
                if (table == null || table.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Warning, "{0}{1}{2}", "Did not find any VoiceMail settings records for user Id: '", 
                        userId, "' !" );
                    return false;
                }
                else if (table.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Error, "{0}{1}{2}", "Found multiple VoiceMail settings records for user Id: '", 
                        userId, "' !" );
                }
                
                DataRow row = table.Rows[0];
                voiceMailSettings = new VoiceMailSettingsRecord();
                voiceMailSettings.id = Convert.ToUInt32(row[VmSettings.Id]);
                voiceMailSettings.accountStatus = (UserStatus) Convert.ToUInt32(row[VmSettings.AccountStatus]);
                voiceMailSettings.describeEachMessage = Convert.ToBoolean(row[VmSettings.DescribeEachMessage]);
                voiceMailSettings.greetingFilename = Convert.ToString(row[VmSettings.GreetingFilename]);
                voiceMailSettings.isFirstLogin = Convert.ToBoolean(row[VmSettings.IsFirstLogin]);
                voiceMailSettings.maxMessageLength = Convert.ToInt32(row[VmSettings.MaxMessageLength]);
                voiceMailSettings.maxNumberMessages = Convert.ToUInt32(row[VmSettings.MaxNumberMessages]);
                voiceMailSettings.maxStorageDays = Convert.ToUInt32(row[VmSettings.MaxStorageDays]);
                voiceMailSettings.notificationAddress = Convert.ToString(row[VmSettings.NotificationAddress]);
                voiceMailSettings.notificationMethod = (NotificationMethod) Convert.ToUInt32(row[VmSettings.NotificationMethod]);
                voiceMailSettings.sortOrder = (SortOrder) Convert.ToUInt32(row[VmSettings.SortOrder]);

                return (voiceMailSettings.id >= SqlConstants.StandardPrimaryKeySeed);
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetVoiceMailSettings method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                return false;
            }
        }
        #endregion

        #region UpdateVoiceMailGreeting
        public bool UpdateVoiceMailGreeting(uint voiceMailSettingsId, string greetingFilename)
        {
            SqlBuilder builder = new SqlBuilder(Method.UPDATE, VmSettings.TableName);
            builder.AddFieldValue(VmSettings.GreetingFilename, greetingFilename);
            builder.where[VmSettings.Id] = voiceMailSettingsId;

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
            {
                if(result.rowsAffected == 1)
                    return true;
                else 
                    return false;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the UpdateVoiceMailGreeting method, using voiceMailSettingsId: '{0}'\n" +
                    "Error message: {1}", voiceMailSettingsId, result.e.Message);
                return false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("UpdateVoiceMailGreeting"));
                return false;
            }

            return false;
        }
        #endregion

    }
}