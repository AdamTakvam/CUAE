<?php


abstract class LogMessageType
{
    const   UNSPECIFIED = 0;
    const   ALARM       = 1;
    const   AUDIT       = 2;
    const   SECURITY    = 3;

    public static function display($type)
    {
        switch ($type)
        {
            case self::ALARM :      return "Alarm";
            case self::AUDIT :      return "Audit";
            case self::SECURITY :   return "Security";
            default :               return "Event";
        }
    }

}


abstract class Severity
{
    const   UNSPECIFIED     = 0;
    const   RED             = 1;
    const   YELLOW          = 2;
    const   GREEN           = 3;
    
    public static $msDisplayName = array(
            Severity::UNSPECIFIED   => "Unspecified",
            Severity::RED           => "Error",
            Severity::YELLOW        => "Warning",
            Severity::GREEN         => "OK",
    );
    
}


abstract class LogEventStatus
{
    const   UNSPECIFIED     = 0;
    const   OPEN            = 1;
    const   ACKNOWLEDGED    = 2;
    const   RESOLVED        = 3;

    public static $msDisplayName = array(
            LogEventStatus::UNSPECIFIED     => "Unspecified",
            LogEventStatus::OPEN            => "Open",
            LogEventStatus::ACKNOWLEDGED    => "Acknowledged",
            LogEventStatus::RESOLVED        => "Resolved",
    );
    
}


abstract class LogMessageId
{
    const   PARTITION_CONFIG_MODIFIED       = 1000;
    const   COMPONENT_CONFIG_MODIFIED       = 1001;
    const   GROUP_CONFIG_MODIFIED           = 1002;

    const   APPLICATION_INSTALLED           = 1010;
    const   APPLICATION_ENABLED             = 1011;
    const   APPLICATION_DISABLED            = 1012;
    const   APPLICATION_UNINSTALLED         = 1013;
    const   APPLICATION_INSTALL_FAILED      = 1014;
    const   APPLICATION_UPDATED             = 1015;
    const   APPLICATION_UPDATE_FAILED       = 1016;

    const   APPLICATION_PARTITION_CREATED   = 1020;
    const   APPLICATION_PARTITION_DELETED   = 1021;

    const   TRIGGER_PARAMETER_ADDED             = 1022;
    const   TRIGGER_PARAMETER_DELETED           = 1023;
    const   TRIGGER_PARAMETER_VALUE_ADDED       = 1024;
    const   TRIGGER_PARAMETER_VALUE_DELETED     = 1025;
    const   TRIGGER_PARAMETER_VALUES_UPDATED    = 1026;

    const   PROVIDER_INSTALLED              = 1030;
    const   PROVIDER_ENABLED                = 1031;
    const   PROVIDER_DISABLED               = 1032;
    const   PROVIDER_UNINSTALLED            = 1033;
    const   PROVIDER_EXTENSION_INVOKED      = 1034;
    const   PROVIDER_INSTALLED_FAILED       = 1035;

    const   MEDIA_SERVER_ADDED              = 1040;
    const   MEDIA_SERVER_DELETED            = 1041;
    const   MEDIA_SERVER_UPDATED            = 1042;

    const   IPT_SERVER_CREATED              = 1050;
    const   IPT_SERVER_REMOVED              = 1052;
    const   IPT_DEVICE_POOL_CHANGED         = 1503;

    const   CALL_MANAGER_CLUSTER_CREATED    = 1060;
    const   CALL_MANAGER_CLUSTER_MODIFIED   = 1061;
    const   CALL_MANAGER_CLUSTER_REMOVED    = 1062;

    const   CTI_MANAGER_CREATED             = 1063;
    const   CTI_MANAGER_MODIFIED            = 1064;
    const   CTI_MANAGER_REMOVED             = 1065;

    const   SUBSCRIBER_CREATED              = 1066;
    const   SUBSCRIBER_MODIFIED             = 1067;
    const   SUBSCRIBER_REMOVED              = 1068;

    const   ALARM_CREATED                   = 1070;
    const   ALARM_REMOVED                   = 1071;

    const   USER_ADDED                      = 1080;
    const   USER_DELETED                    = 1081;
    const   USER_MODIFIED                   = 1082;
    const   USER_LOGIN                      = 1083;
    const   USER_LOGOUT                     = 1084;
    const   USER_LOGIN_FAILED               = 1085;

    const   GROUP_ADDED                     = 1090;
    const   GROUP_DELETED                   = 1091;
    const   GROUP_MODIFIED                  = 1092;
    const   ADDED_TO_GROUP                  = 1093;
    const   REMOVED_FROM_GROUP              = 1094;

    const   NETWORK_CONFIGURED              = 1100;
    const   CONSOLE_CONFIGURED              = 1101;
    const   CONSOLE_REBOOT                  = 1102;

    const   SERVICE_ENABLED                 = 1110;
    const   SERVICE_DISABLED                = 1111;
    const   SERVICE_STARTED                 = 1112;
    const   SERVICE_STOPPED                 = 1113;
    const   SERVICE_RESTARTED               = 1114;

    const   BACKUP_STARTED                  = 1120;
    const   BACKUP_ERROR                    = 1121;
    const   BACKUP_COMPLETED                = 1122;
    const   BACKUP_DELETED                  = 1123;

    const   RESTORE_STARTED                 = 1130;
    const   RESTORE_ERROR                   = 1131;
    const   RESTORE_COMPLETED               = 1132;

    const   UPDATE_STARTED                  = 1140;
    const   UPDATE_ERROR                    = 1141;
    const   UPDATE_COMPLETED                = 1142;

    const   LOGS_DELETED                    = 1150;
    const   LOGS_ARCHIVED                   = 1151;

    const   HMP_ACTIVATION_ERROR            = 1160;
    const   HMP_ACTIVATION_COMPLETED        = 1161;

    const   DATE_TIME_ERROR                 = 1170;
    const   DATE_TIME_UPDATED               = 1171;

    const   TTS_LICENSE_UPLOAD_ERROR        = 1180;
    const   TTS_LICENSE_UPLOAD_COMPLETED    = 1181;

    const   MEDIA_SERVER_SERVICE_UPDATED    = 1190;

    const   CORE_EXTENSION_INVOKED          = 1200;
    
    const   SIP_DOMAIN_CREATED              = 1210;
    const   SIP_DOMAIN_UPDATED              = 1211;
    const   SIP_DOMAIN_REMOVED              = 1212;
    
    const   SSL_CERTIFICATE_GENERATED       = 1220;
    const   SSL_CERTIFICATE_UPLOADED        = 1221;
    const   SSL_KEY_UPLOADED                = 1222;
    const   SSL_ENABLED                     = 1223;
    const   SSL_DISABLED                    = 1224;
    
    const   VR_SERVER_ADD                   = 1300;
    const   VR_SERVER_REMOVE                = 1301;
    const   VR_LICENSE_SERVER_ADD           = 1302;
    const   VR_LICENSE_SERVER_REMOVE        = 1303;
    
    const   REDUNDANCY_STANDBY_ENABLED      = 1400;
    const   REDUNDANCY_STANDBY_DISABLED     = 1401;
    const   REDUNDANCY_MASTER_ENABLED       = 1402;
    const   REDUNDANCY_MASTER_DISABLED      = 1403;
}

?>