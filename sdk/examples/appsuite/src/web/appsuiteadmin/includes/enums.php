<?php

abstract class UserStatus
{
    const ACTIVE    = 1;
    const DISABLED  = 2;
    const LOCKED    = 4;
    const DELETED   = 8;

    public static $names =  array (
                            UserStatus::ACTIVE    => "Active",
                            UserStatus::DISABLED  => "Disabled",
                            UserStatus::LOCKED    => "Locked",
                            UserStatus::DELETED   => "Deleted",
                            );

}

abstract class EndReason
{
    const OK                = 1;
    const NOT_ANSWERED      = 2;
    const BUSY              = 4;
    const UNREACHABLE       = 8;
    const OTHER_OR_UNKNOWN  = 16;
    const SYSTEM_FAILURE    = 32768;
    const INVALID           = 65536;

    public static $names =  array(
                            EndReason::OK               => "Hang Up",
                            EndReason::NOT_ANSWERED     => "Not Answered",
                            EndReason::UNREACHABLE      => "Unreachable",
                            EndReason::BUSY             => "Busy",
                            EndReason::OTHER_OR_UNKNOWN => "Other/Unknown",
                            EndReason::SYSTEM_FAILURE   => "System Failure",
                            EndReason::INVALID          => "Invalid"
                            );
}


abstract class AuthenticationResult
{
    const OK                            = 1;
    const INVALID_ACCOUNT_CODE_OR_PIN   = 2;
    const NOT_ALLOWED_DUE_TO_DISABLED   = 4;
    const NOT_ALLOWED_DUE_LOCKED        = 8;
    const NOT_ALLOWED_DUE_TO_DELETED    = 16;
    const QUOTA_EXCEEDED                = 32;
    const FAILURE                       = 65536;

    public static $names =  array(
                            AuthenticationResult::OK                            => "Ok",
                            AuthenticationResult::INVALID_ACCOUNT_CODE_OR_PIN   => "Invalid Account Code/Pin",
                            AuthenticationResult::NOT_ALLOWED_DUE_TO_DISABLED   => "Disabled",
                            AuthenticationResult::NOT_ALLOWED_DUE_TO_LOCKED     => "Locked",
                            AuthenticationResult::QUOTA_EXCEEDED                => "Quota Exceeded",
                            AuthenticationResult::FAILURE                       => "Failure"
                            );

}

abstract class SortOrder
{
    const INCREASING    = 1;
    const DECREASING    = 2;
    
    public static $names =  array(
                            SortOrder::INCREASING   => "Increasing",
                            SortOrder::DECREASING   => "Decreasing",
                            );
}

abstract class VoicemailNotificationMethod
{
    const NONE              = 1;
    const PHONE             = 2;
    const PAGER             = 4;
    const EMAIL             = 8;
    const CISCO_IP_SCREEN   = 16;
    
    public static $names =  array(
                            VoicemailNotificationMethod::NONE            => "None",
                            VoicemailNotificationMethod::PHONE           => "Phone",
                            VoicemailNotificationMethod::PAGER           => "Pager",
                            VoicemailNotificationMethod::EMAIL           => "E-mail",
                            VoicemailNotificationMethod::CISCO_IP_SCREEN => "Cisco IP Screen",
                            );
}

abstract class BackupStatus
{
    const IN_PROGRESS       = 1;
    const DB_BACKUP         = 2;
    const DONE              = 3;
    const FAILED            = 4;

    public static $names =  array(
                            BackupStatus::IN_PROGRESS       => "In Progress",
                            BackupStatus::DB_BACKUP         => "Backing Up Database",
                            BackupStatus::DONE              => "Ready",
                            BackupStatus::FAILED            => "Failed",
                            );
}

abstract class MediaFileType
{
    const WAV   = 1;
    const VOX   = 2;
    const OTHER = 65536;

    public static $names =  array(
                            MediaFileType::WAV      => "WAV",
                            MediaFileType::VOX      => "VOX",
                            MediaFileType::OTHER    => "Other",
                            );
}

abstract class ConferenceStatus
{
    const ENABLED   = 1;
    const DISABLED  = 2;

    public static $names =  array(
                            ConferenceStatus::ENABLED       => "Enabled",
                            ConferenceStatus::DISABLED      => "Disabled",
                            );
}

abstract class RemoteAgentUserLevel
{
    const USER          = 1;
    const SUPERVISOR    = 5;

    public static $names =  array(
                            RemoteAgentUserLevel::USER          => "User",
                            RemoteAgentUserLevel::SUPERVISOR    => "Supervisor",
                            );
}

abstract class GroupUserLevel
{
    const MEMBER        = 1;
    const ADMINISTRATOR = 64;
    
    public static $names =  array(
                            GroupUserLevel::MEMBER          => "Member",
                            GroupUserLevel::ADMINISTRATOR   => "Administrator"
                            );
}

abstract class SimpleMatchType
{
    const BEGINS_WITH   = 0;
    const ENDS_WITH     = 1;
    const CONTAINS      = 2;
    const EQUALS        = 3;

    public static $names =  array(
                            SimpleMatchType::BEGINS_WITH    => "Begins with",
                            SimpleMatchType::ENDS_WITH      => "Ends with",
                            SimpleMatchType::CONTAINS       => "Contains",
                            SimpleMatchType::EQUALS         => "Equals",
                            );
}

abstract class FindMeCallRecordType
{
    const FIND_ME       = 1;
    const TRANSFER      = 2;
    const SWAP          = 4;
    
    public static $names =  array(
                            FindMeCallRecordType::FIND_ME   => "Find Me",
                            FindMeCallRecordType::TRANSFER  => "Transfer",
                            FindMeCallRecordType::SWAP      => "Swap",
                            );

}

abstract class ServiceStatus
{

    const UNKNOWN       = 0;
    const STOPPED       = 1;
    const START_PENDING = 2;
    const STOP_PENDING  = 3;
    const RUNNING       = 4;
    const PAUSED        = 5;

}

abstract class ReplicationRole
{

    const NONE          = 0;
    const MASTER        = 1;
    const SLAVE         = 2;

}

abstract class ApplicationId
{
    const SCHEDULED_CONFERENCING    = 1;
    const INTERCOM_TALKBACK         = 2;
    const RAPID_RECORD              = 3;
    const REMOTE_AGENT              = 4;
    const ACTIVE_RELAY              = 5;
    const VOICE_TUNNEL              = 6;
    const CLICK_TO_TALK             = 7;
    const VOICEMAIL                 = 8;
    const AUTO_ATTENDANT            = 9;
}

abstract class FilterNumberType
{
    const UNKNOWN   = 0;
    const ALLOW     = 1;
    const BLOCK     = 2;
}


?>