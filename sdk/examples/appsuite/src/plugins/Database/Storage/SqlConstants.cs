using System;
using System.Diagnostics;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary> Constants </summary>
    public class SqlConstants
    {
        /// <summary>
        ///     All tables in Application Suite use primary keys seeded at 1
        /// </summary>
        public const int StandardPrimaryKeySeed = 1;
        /// <summary>
        ///     Most Database access methods default integers to -1 to indicate error
        /// </summary>
        public const int DefaultOutIntegerValue = -1;

        public const string ConnectionString =
            "Server=localhost;User ID=root;Password=metreos;Database=application_suite";

        public const string GetLastInsertId =
            "SELECT LAST_INSERT_ID()";

        /// <summary>
        ///     Used in the DbConnections hash on SessionData
        /// </summary>
        public const string DbConnectionName = "ApplicationSuite";
        
        public const string AllowDBWriteName = "AllowDBWrite";

        public const string MecConnectionName = "mec";

        public static string GetConfigName(ConfigurationName configName)
        {
            switch(configName)
            {
                case ConfigurationName.AdminPassword:
                    return "admin_password";

                case ConfigurationName.WebServerFilepath:
                    return "webserver_root_filepath";

                case ConfigurationName.WebServerHost:
                    return "webserver_host";

                case ConfigurationName.RecordingRelPath:
                    return "record_rel_path";

                case ConfigurationName.RecordingsExpiration:
                    return "recordings_expiration";

                case ConfigurationName.MediaServerRelPath:
                    return "media_rel_path";

                case ConfigurationName.SmtpServer:
                    return "smtp_server";

                case ConfigurationName.SmtpUsername:
                    return "smtp_username"; 

                case ConfigurationName.SmtpPassword:
                    return "smtp_password"; 

                case ConfigurationName.SmtpAuthMethod:
                    return "smtp_auth_method";

                default:
                    Debug.Assert(false, "Unable to find field name that corresponds to " + 
                        "the specified ConfigurationName value of " + configName.ToString());
                    break;
            }

            return null;
        }

        public class Tables
        {
            /// <summary>
            /// Table used for SELECTs on SQL functions
            /// </summary>
            public class Dual
            {
                public const string TableName           = "dual";
            }

            public class ActiveRelayFilterNumbers
            {
                public const string TableName           = "as_activerelay_filter_numbers";
                public const string Id                  = "as_activerelay_filter_numbers_id";
                public const string UserId				= "as_users_id";
                public const string Type                = "type";
                public const string Number              = "number";
            }

            public class Users
            {
                public const string TableName           = "as_users";
                public const string Id                  = "as_users_id";
                public const string LdapServersId       = "as_ldap_servers_id";
                public const string Username            = "username";
                public const string Password            = "password";
                public const string AccountCode         = "account_code";
                public const string Pin                 = "pin";
                public const string FirstName           = "first_name";
                public const string LastName            = "last_name";
                public const string Email               = "email";
                public const string Status              = "status";
                public const string Created             = "created";
                public const string LastUsed            = "last_used";
                public const string LockoutThreshold    = "lockout_threshold";
                public const string LockoutDuration     = "lockout_duration";
                public const string LastLockout         = "last_lockout";
                public const string NumFailedLogins     = "failed_logins";
                public const string NumConcurrentSessions = "current_active_sessions";
                public const string MaxConcurrentSessions = "max_concurrent_sessions";
                public const string PinChangeRequired   = "pin_change_required";
                public const string ExternalAuthEnabled = "external_auth_enabled";
                public const string ExternalAuthDn      = "external_auth_dn";
                public const string PlacedCalls         = "placed_calls";
                public const string SuccessfulCalls     = "successful_calls";
                public const string TotalCallTime       = "total_call_time";
                
                // deprecated in favor of time_zone field
                public const string GmtOffset           = "gmt_offset";
                
                public const string TimeZone            = "time_zone";
                public const string Record              = "record";
                public const string RecordingVisible    = "recording_visible";
                public const string LdapSynched         = "ldap_synched";
                public const string ArTransferNumber    = "ar_transfer_number";
            }

            public class Devices
            {
                public const string TableName           = "as_phone_devices";
                public const string Id                  = "as_phone_devices_id";
                public const string UserId              = "as_users_id";
                public const string IsPrimaryDevice     = "is_primary_device";
                public const string MacAddress          = "mac_address";
            }

            public class AuthenticationRecords
            {
                public const string TableName           = "as_auth_records";
                public const string Id                  = "as_auth_records_id";
                public const string UserId              = "as_users_id";
                public const string AuthTimestamp       = "auth_timestamp";
                public const string Status              = "status";
                public const string DirectoryNumber     = "originating_number";
                public const string IpAddress           = "source_ip_address";
                public const string Username            = "username";
                public const string Pin                 = "pin";
                public const string ApplicationName     = "application_name";
                public const string PartitionName       = "partition_name";
            }

            public class SessionRecords
            {
                public const string TableName           = "as_session_records";
                public const string Id                  = "as_session_records_id";
                public const string Start               = "start";
                public const string End                 = "end";
                public const string AuthenticationRecordId = "as_auth_records_id";
            }

            public class DirectoryNumbers
            {
                public const string TableName           = "as_directory_numbers";
                public const string Id                  = "as_directory_numbers_id";
                public const string DeviceId            = "as_phone_devices_id";
                public const string DirectoryNumber     = "directory_number";
                public const string IsPrimaryNumber     = "is_primary_number";
            }

            public class Recordings
            {
                public const string TableName           = "as_recordings";
                public const string Id                  = "as_recordings_id";
                public const string UserId              = "as_users_id"; 
                public const string CallRecordId        = "as_call_records_id";
                public const string Start               = "start";
                public const string End                 = "end";
                public const string Type                = "type";
                public const string Url                 = "url";
            }

            public class CallRecords
            {
                public const string TableName           = "as_call_records";
                public const string Id                  = "as_call_records_id";
                public const string UserId              = "as_users_id";
                public const string SessionRecordsId    = "as_session_records_id";
                public const string RecordingsId        = "as_recordings_id";
                public const string ScheduledConfId     = "as_scheduled_conferences_id";
                public const string OriginNumber        = "origin_number";
                public const string DestinationNumber   = "destination_number";
                public const string ApplicationName     = "application_name";
                public const string Start               = "start";
                public const string End                 = "end";
                public const string EndReason           = "end_reason";
                public const string PartitionName       = "partition_name";
                public const string AuthRecordsId       = "as_auth_records_id";
                public const string ScriptName          = "script_name";
            }

			public class FindMeCallRecords
			{
				public const string TableName			= "as_findme_call_records";
				public const string Id					= "as_findme_call_records_id";
				public const string CallRecordsId		= "as_call_records_id";
				public const string Type				= "call_type";
				public const string From				= "calling_number";
				public const string To					= "called_number";
				public const string EndReason			= "end_reason";
			}

            public class ExternalNumbers
            {
                public const string TableName           = "as_external_numbers";
                public const string Id                  = "as_external_numbers_id";
                public const string UserId              = "as_users_id";
                public const string Name                = "name";
                public const string PhoneNumber         = "phone_number";
                public const string DelayCallTime       = "delay_call_time";
                public const string CallAttemptTimeout  = "call_attempt_timeout";
                public const string IsCorporate         = "is_corporate";
                public const string ArEnabled           = "ar_enabled";
                public const string IsBlacklisted       = "is_blacklisted";
                public const string TimeOfDayEnabled    = "timeofday_enabled";
                public const string TimeOfDayWeekend    = "timeofday_weekend";
                public const string TimeOfDayStart      = "timeofday_start";
                public const string TimeOfDayEnd        = "timeofday_end";
            }

            public class ScheduledConferences
            {
                public const string TableName           = "as_scheduled_conferences";
                public const string Id                  = "as_scheduled_conferences_id";
                public const string HostConfId          = "host_conf_id";
                public const string ParticipantConfId   = "participant_conf_id";
                public const string MmsId               = "mms_id";
                public const string MmsConfId           = "mms_conf_id";
                public const string ScheduledTimestamp  = "scheduled_timestamp";
                public const string DurationMinutes     = "duration_minutes";
                public const string NumParticipants     = "num_participants";
            }

            public class ScheduledConferencesFiles    
            {
                public const string TableName           = "as_scheduled_conferences_files";
                public const string Id                  = "as_scheduled_conferences_files_id";
                public const string ScheduledConferenceId = "as_scheduled_conferences_id";
                public const string Time                = "time";
                public const string Type                = "type";
                public const string Filename            = "filename";
            }

            public class IntercomGroups
            {
                public const string TableName           = "as_intercom_groups";
                public const string Id                  = "as_intercom_groups_id";
                public const string Name                = "name";
                public const string IsEnabled           = "is_enabled";
                public const string IsTalkbackEnabled   = "is_talkback_enabled";
                public const string IsPrivate           = "is_private";
            }

            public class IntercomGroupMembers
            {
                public const string TableName           = "as_intercom_group_members";
                public const string IntercomGroupId     = "as_intercom_groups_id";
                public const string UserId              = "as_users_id";
            }

            public class Applications
            {
                public const string TableName           = "as_applications";
                public const string Id                  = "as_applications_id";
                public const string Name                = "name";
                public const string Installed           = "installed";
            }

            public class Configs
            {
                public const string TableName           = "as_configs";
                public const string Id                  = "as_configs_id";
                public const string ApplicationId       = "as_applications_id";
                public const string Name                = "name";
                public const string Value               = "value";
                public const string Description         = "description";
                public const string GroupName           = "groupname";
                public const string Required            = "required";
            }

            public class VoiceMailSettings
            {
                public const string TableName           = "as_voicemail_settings";
                public const string Id                  = "as_voicemail_settings_id";
                public const string UserId              = "as_users_id";
                public const string IsFirstLogin        = "is_first_login";
                public const string AccountStatus       = "account_status";
                public const string SortOrder           = "sort_order";
                public const string GreetingFilename    = "greeting_filename";
                public const string NotificationMethod  = "notification_method";
                public const string NotificationAddress = "notification_address";
                public const string MaxMessageLength    = "max_message_length";
                public const string MaxNumberMessages   = "max_number_messages";
                public const string MaxStorageDays      = "max_storage_days";
                public const string DescribeEachMessage = "describe_each_message";
            }

            public class VoiceMails
            {
                public const string TableName           = "as_voicemails";
                public const string Id                  = "as_voicemails_id";
                public const string UserId              = "as_users_id";
                public const string Status              = "vm_status";
                public const string TimeStamp           = "vm_timestamp";
                public const string Filename            = "vm_filename";
                public const string Length              = "vm_length";
            }

            public class RemoteAgents
            {
                public const string TableName           = "as_remote_agents";
                public const string Id                  = "as_remote_agents_id";
                public const string UserId              = "as_users_id";
                public const string DeviceId            = "as_phone_devices_id";
                public const string ExternalNumber      = "as_external_numbers_id";
                public const string AgentUserLevel      = "user_level";
            }

            public class Registrations
            {
                public const string TableName           = "registrations";
                public const string Id                  = "id";
                public const string Sid                 = "sid";
                public const string CcmAddress          = "ccmaddress";
                public const string StartTime           = "starttime";
                public const string EndTime             = "endtime";
                public const string NumRingIn           = "num_ring_in";
                public const string NumRingOut          = "num_ring_out";
                public const string NumBusy             = "num_busy";
                public const string NumConnected        = "num_connected";
            }

            public class MecParticipants
            {
                public const string TableName           = "participants";
                public const string Id                  = "id";
                public const string ConferenceId        = "conferences_id";
                public const string MmsConnectionId     = "mms_connection_id";
                public const string CallId              = "callid";
                public const string PhoneNumber         = "phone_number";
                public const string Description         = "description";
                public const string IsHost              = "ishost";
                public const string Status              = "status";
                public const string FirstConnected      = "first_connected";
                public const string LastConnected       = "last_connected";
                public const string Disconnected        = "disconnected";
            }

            public class MecConferences
            {
                public const string TableName           = "conferences";
                public const string Id                  = "id";
                public const string MmsConferenceId     = "mms_conf_id";
                public const string ConfSessionId       = "conf_session_id";
                public const string IsPublic            = "is_public";
                public const string IsScheduled         = "is_scheduled";
                public const string IsRecorded          = "is_recorded";
                public const string Start               = "start";
                public const string End                 = "end";
            }

            public class LdapServers
            {
                public const string TableName           = "as_ldap_servers";
                public const string Id                  = "as_ldap_servers_id";
                public const string Hostname            = "hostname";
                public const string Port                = "port";
                public const string SecureConnect       = "secure_connect";
                public const string BaseDn              = "base_dn";
                public const string UserDn              = "user_dn";
                public const string Password            = "password";

            }
        }
    }

    /* YOU CAN NOT CHANGE THESE INT VALUES OR THEIR MEANINGS FOR A DATABASE IN DEPLOYMENT (i.e., ever)
     * YOU CAN ONLY ADD MORE ENUM VALUES
     *
     * The only option is to also package a SQL clean up script which will manually check all tables for
     * the old value and change it to the new one */
    #region Database-Linked Enumerations

    /// <summary>
    ///     The possible status of a user.
    ///     Corresponds to the 'status' field in the 'users' table
    ///
    ///     REFACTOR NOTICE:
    ///     If you change the string name of these, remember to change accompanying
    ///     ReturnValueUserStatus enum accordingly
    ///
    /// </summary>
    public enum UserStatus
    {
        Ok              = 1,
        Disabled        = 2,
        Locked          = 4,
        Deleted         = 8
    }

    /// <summary>
    ///     Used in the ResultValue attribute for NativeActions
    /// </summary>
    public enum ReturnValueUserStatus
    {
        Success,
        Disabled,
        Locked,
        Deleted
    }

    /// <summary>
    ///     The possible reasons a call can end.
    ///     Corresponds to the 'end_reason' field in the 'as_call_records' table
    /// </summary>
    /// <remarks>
    ///     This enum is the same as ICallControl.EndReason but with specific
    ///     values added to the enum items.
    /// </remarks>
    public enum EndReason
    {
        Normal          = 0x01,
        Ringout         = 0x02,
        Busy            = 0x04,
        Unreachable     = 0x08,
        Unknown         = 0x10,
        InternalError   = 0x8000,
        Invalid         = 0x10000
    }

	public enum ActiveRelayCallTypes : uint
	{
		FindMe			= 1,
		Transfer		= 2,
		Swap			= 4
	}

    public enum FilterType : uint
    {
        Allow           = 1,
        Block           = 2
    }

    public enum old
    {
        NormalCallClearing  = 1,
        NoAnswer            = 2,
        RemoteBusy          = 4,
        Unreachable         = 8,
        OtherOrUnknown      = 16,
        SystemFailure       = 32768,
        Invalid             = 65536
    }

    /// <summary>
    ///     The possible reasons a user can fail to authenticate.
    ///     Corresponds to the 'status' field in the 'as_auth_records' table
    ///
    ///     REFACTOR NOTICE:
    ///     If you change the string name of these, remember to change accompanying
    ///     ReturnValueAuthenticationResult enum accordingly
    /// </summary>
    public enum AuthenticationResult
    {
        success                 = 1,
        InvalidAccountCodeOrPin = 2,
        NotAllowedDueToDisabled = 4,
        NotAllowedDueLocked     = 8,
        NotAllowedDueToDeleted  = 16,
        QuotaExceeded           = 32,
        failure                 = 65536
    }

    /// <summary>
    /// These values are used in the as_external_numbers table to indicate when an 
    /// </summary>
    public enum TimeOfDayWeekendValues: uint
    {
        None                    = 0,
        Saturday                = 1,
        Sunday                  = 2,
        SaturdayAndSunday       = 3
    }

    /// <summary>
    ///     Used only in the ResultValue attribute for NativeActions
    /// </summary>
    public enum ReturnValueAuthenticationResult
    {
        success,
        InvalidAccountCodeOrPin,
        NotAllowedDueToDisabled,
        NotAllowedDueLocked,
        NotAllowedDueToDeleted,
        failure
    }

    /// <summary>
    ///     Used in the recordings table
    /// </summary>
    public enum MediaFileType
    {
        Wav             = 1,
        Vox             = 2,
        Other           = 65536
    }

    public enum ConfigurationName
    {
        Unspecified             = 0,
        AdminPassword           = 1,
        WebServerFilepath       = 2,
        WebServerHost           = 4, 
        RecordingRelPath        = 8,
        RecordingsExpiration    = 16,
        MediaServerRelPath      = 32,
        SmtpServer              = 64,
        SmtpUsername            = 128,
        SmtpPassword            = 256,
        SmtpAuthMethod          = 512
    }

    public enum MessageStatus
    {
        All                 = 1,
        New                 = 2,
        Old                 = 4,
        Saved               = 8,
        Deleted             = 16,
        Flagged             = 32,
        Other               = 65536
    }

    public enum SortOrder
    {
        Increasing          = 1,
        Decreasing          = 2
    }

    public enum NotificationMethod
    {
        None                = 1,
        Phone               = 2,
        Pager               = 4,
        Email               = 8,
        CiscoIpScreen       = 16
    }

    /// <summary>
    /// Access level of the remote agent user, either "User" or "Supervisor"
    /// </summary>
    public enum RemoteAgentUserLevel : uint
    {
        User                = 1,
        Supervisor          = 5
    }
    
    public enum MecParticipantStatus
    {
        None                = 0, // 0
        Connecting          = 1, // 1
        Online              = 2, // 2
        Conferenced         = 4, // 3
        Mute                = 8, // 4
        Disconnected        = 16 // 5
    }

    #endregion
}
