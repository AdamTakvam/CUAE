using System;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using Metreos.ApplicationSuite.Types;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// Retrieves voicemail settings specific to a user.
    /// </summary>
    public class GetVoiceMailSettings : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("userId of user whose VoiceMail settings we wish to retrieve", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;
        
        [ResultDataField("'true' if this is the first time the user has logged in")]
        public bool IsFirstLogin { get { return isFirstLogin; } }
        private bool isFirstLogin;

        [ResultDataField("id of the VoiceMail settings record")]
        public uint VoiceMailSettingsId{ get { return voiceMailSettingsId; } }
        private uint voiceMailSettingsId;

        [ResultDataField("Name of the file that gets played to callers when they reach the user's mailbox")]
        public string GreetingFilename { get { return greetingFilename; } }
        private string greetingFilename;

        [ResultDataField("Status of this VoiceMail account")]
        public string AccountStatus { get { return accountStatus; } }
        private string accountStatus;

        [ResultDataField("Order in which VoiceMail messages are sorted")]
        public string SortingOrder { get { return sortingOrder; } }
        private string sortingOrder;

        [ResultDataField("The method to be used for notifying the user of new VoiceMails")]
        public string NotifyMethod { get { return notifyMethod; } } 
        private string notifyMethod;

        [ResultDataField("Address to which the notification is to be sent")]
        public string NotificationAddress { get { return notificationAddress; } }
        private string notificationAddress;

        [ResultDataField("Length of messages left for this user may not exceed the specified number of seconds")]
        public int MaxMessageLength { get { return maxMessageLength; } }
        private int maxMessageLength;
        
        [ResultDataField("The maximum number of messages that a user's mailbox may contain")]
        public uint MaxNumberMessages { get { return maxNumberMessages; } }
        private uint maxNumberMessages;
        
        [ResultDataField("The longest number of days for which messages will be kept before being deleted")]
        public uint MaxStorageDays { get { return maxStorageDays; } }
        private uint maxStorageDays;

        [ResultDataField("If 'true', the VoiceMail system will announce information such as \"Date Recevied\" before playing each message")]
        public bool DescribeEachMessage { get { return describeEachMessage; } }
        private bool describeEachMessage;

        public GetVoiceMailSettings()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            userId = 0;
            voiceMailSettingsId = 0;
            isFirstLogin = true;
            describeEachMessage = false;
            greetingFilename = string.Empty;
            notificationAddress = string.Empty;
            accountStatus = string.Empty;
            sortingOrder = string.Empty;
            notifyMethod = string.Empty;
            maxMessageLength = 0;
            maxNumberMessages = maxStorageDays = 0;
        }

        [Action("GetVoiceMailSettings", false, "GetVoiceMailSettings", "Retrieves voicemail settings specific to a user.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(VoiceMailSettings vmDbAccess = new VoiceMailSettings(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success;
                VoiceMailSettings.VoiceMailSettingsRecord record;

                success = vmDbAccess.GetVoiceMailSettings(userId, out record);

                if(success)
                {
                    isFirstLogin = record.isFirstLogin;
                    accountStatus = Enum.GetName(typeof(UserStatus), record.accountStatus);
                    describeEachMessage = record.describeEachMessage;
                    greetingFilename = record.greetingFilename;
                    maxMessageLength = record.maxMessageLength * 1000;
                    maxNumberMessages = record.maxNumberMessages;
                    maxStorageDays = record.maxStorageDays;
                    notificationAddress = record.notificationAddress;
                    notifyMethod = Enum.GetName(typeof(NotificationMethod), record.notificationMethod);
                    sortingOrder = Enum.GetName(typeof(SortOrder), record.sortOrder);
                    voiceMailSettingsId = record.id;
                    return IApp.VALUE_SUCCESS;
                }
            
                return IApp.VALUE_FAILURE;
            }
        }
    }
}
