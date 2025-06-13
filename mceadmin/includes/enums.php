<?php


/* ----------- COMPONENTS & GROUPS ------------ */

abstract class ComponentType
{
    const UNSPECIFIED =     0;
    const CORE =            1;
    const APPLICATION =     2;
    const PROVIDER =        3;
    const MEDIA_SERVER =    4;
    const LOG_SERVER =      5;
    const RTP_RELAY =       6;

    // 50-99 Reserved for Alarm Servers
    const SMTP_MANAGER =    50;
    const SNMP_MANAGER =    51;

    // 100-149 Reserved for Telephony Servers
    const SCCP_DEVICE_POOL          = 100;
    const H323_GATEWAY              = 101;
    const SIP_DEVICE_POOL           = 102;
    const CTI_DEVICE_POOL           = 103;
    const CTI_ROUTE_POINT           = 104;
    const SIP_TRUNK_INTERFACE       = 105;
    const IETF_SIP_DEVICE_POOL      = 106;
    const MONITORED_CTI_DEVICE_POOL = 107;
    const TEST_IPT                  = 149;

    public static function display($type)
    {
        switch ($type)
        {
            case self::CORE :
                return "Core";
            case self::APPLICATION :
                return "Application";
            case self::PROVIDER :
                return "Provider";
            case self::MEDIA_SERVER :
                return "MediaServer";
            case self::RTP_RELAY :
                return "RTPRelay";
            case self::SMTP_MANAGER :
                return "SMTP_Manager";
            case self::SNMP_MANAGER :
                return "SNMP_Manager";
            case self::SCCP_DEVICE_POOL :
                return "SCCP_DevicePool";
            case self::H323_GATEWAY :
                return "H323_Gateway";
            case self::SIP_DEVICE_POOL :
                return "SIP_DevicePool";
            case self::SIP_TRUNK_INTERFACE:
                return "SIP_Trunk_Interface";
            case self::IETF_SIP_DEVICE_POOL :
                return "IETF_Sip_DevicePool";
            case self::CTI_DEVICE_POOL :
                return "CTI_DevicePool";
            case self::MONITORED_CTI_DEVICE_POOL :
                return "MonitoredCTI_DevicePool";
            case self::CTI_ROUTE_POINT :
                return "CTI_RoutePoint";
            case self::TEST_IPT :
                return "Test_CC";
            default :
                return "Unspecified";
        }
    }

    public static function describe($type)
    {
        switch ($type)
        {
            case self::CORE :
                return "Core Component";
            case self::MEDIA_SERVER :
                return "Media Engine";
            case self::RTP_RELAY :
                return "RTP Relay";
            case self::SMTP_MANAGER :
                return "SMTP Manager";
            case self::SNMP_MANAGER :
                return "SNMP Manager";
            case self::SCCP_DEVICE_POOL :
                return "SCCP Device Pool";
            case self::H323_GATEWAY :
                return "H.323 Gateway";
            case self::SIP_DEVICE_POOL :
                return "Cisco SIP Device Pool";
            case self::SIP_TRUNK_INTERFACE:
                return "Cisco SIP Trunk Interface";
            case self::IETF_SIP_DEVICE_POOL :
                return "IETF SIP Device Pool";
            case self::CTI_DEVICE_POOL :
                return "CTI Device Pool";
            case self::MONITORED_CTI_DEVICE_POOL :
                return "Monitored CTI Device Pool";
            case self::CTI_ROUTE_POINT :
                return "CTI Route Point";
            case self::TEST_IPT :
                return "Test CC";
            default:
                return self::display($type);
        }
    }

    public static function get_class_name($type)
    {
        switch ($type)
        {
            case self::CORE :
                return "Core";
            case self::APPLICATION :
                return "Application";
            case self::PROVIDER :
                return "Provider";
            case self::MEDIA_SERVER :
                return "MediaServer";
            case self::SMTP_MANAGER :
            case self::SNMP_MANAGER :
                return "Alarm";
            case self::SCCP_DEVICE_POOL :
                return "SccpDevicePool";
            case self::CTI_DEVICE_POOL :
                return "CtiDevicePool";
            case self::MONITORED_CTI_DEVICE_POOL :
                return "MonitoredCtiDevicePool";
            case self::CTI_ROUTE_POINT :
                return "DevicePool";
            case self::SIP_DEVICE_POOL :
                return "SipDevicePool";
            case self::IETF_SIP_DEVICE_POOL :
                return "IetfSipDevicePool";
            case self::SIP_TRUNK_INTERFACE :
            case self::H323_GATEWAY :
            case self::TEST_IPT :
                return "IptServer";
            default :
                return "Component";
        }
    }

}


abstract class ComponentStatus
{

    const UNSPECIFIED =     0;
    const DISABLED =        1;
    const DISABLED_ERROR =  2;
    const ENABLED_RUNNING = 3;
    const ENABLED_STOPPED = 4;

    public static function display($status)
    {
        switch ($status)
        {
            case self::DISABLED :
                return "Disabled";
            case self::DISABLED_ERROR :
                return "Disabled Error";
            case self::ENABLED_RUNNING :
                return "Enabled Running";
            case self::ENABLED_STOPPED :
                return "Enabled Stopped";
            default :
                return "Unspecified";
        }
    }

}


abstract class GroupType
{

    const UNSPECIFIED =             0;
    const MEDIA_RESOURCE_GROUP =    4;
    const ALARM_GROUP =             50;
    const SCCP_GROUP =              100;
    const H323_GATEWAY_GROUP =      101;
    const SIP_GROUP =               102;
    const CTI_SERVER_GROUP =        103;
    const TEST_IPT_GROUP =          149;

    public static function display($type)
    {
        switch ($type)
        {
            case self::MEDIA_RESOURCE_GROUP :
                return "Media Resource Group";
            case self::ALARM_GROUP :
                return "Alarm Group";
            case self::SCCP_GROUP :
                return "SCCP Device Pool Group";
            case self::H323_GATEWAY_GROUP :
                return "H.323 Group";
            case self::SIP_GROUP :
                return "SIP Group";
            case self::CTI_SERVER_GROUP :
                return "CTI Server Group";
            case self::TEST_IPT_GROUP :
                return "Test CC Group";
            default :
                return "Unspecified";
        }
    }

}


/* ----------- SIP DOMAINS ------------- */

abstract class SipDomainType
{
    const UNKNOWN   = 0;
    const CISCO     = 1;
    const IETF      = 2;

    public static function display($type)
    {
        switch ($type)
        {
            case self::CISCO :
                return "Cisco SIP Domain";
            case self::IETF :
                return "IETF SIP Domain";
            default:
                return "Unspecified";
        }
    }
}


/* ----------- CONFIG FORMATS ------------ */

abstract class FormatType
{
    const UNSPECIFIED =         0;
    const CONFIG_STRING =       1;
    const CONFIG_BOOL =         2;
    const CONFIG_NUMBER =       3;
    const CONFIG_DATETIME =     4;
    const CONFIG_IP_ADDRESS =   5;
    const CONFIG_ARRAY =        6;
    const CONFIG_HASH =         7;
    const CONFIG_TABLE =        8;
    const CONFIG_PASSWORD =     9;
    const CONFIG_TRACELEVEL =   100;
}


/* ----------- SERVICES ------------ */

abstract class ServiceStatus
{

    const UNKNOWN       = 0;
    const STOPPED       = 1;
    const START_PENDING = 2;
    const STOP_PENDING  = 3;
    const RUNNING       = 4;
    const PAUSED        = 5;

    public static function display($status)
    {
        switch ($status)
        {
            case self::RUNNING :
                return "Running";
            case self::PAUSED :
                return "Paused";
            case self::STOPPED :
                return "Stopped";
            case self::STOP_PENDING :
                return "Stop Pending";
            case self::START_PENDING :
                return "Start Pending";
            default :
                return "Unknown";
        }
    }

}


/* ----------- DEVICES ------------ */

abstract class DeviceType
{
    const SCCP                  = 1;
    const CTI_DEVICE            = 2;
    const CTI_ROUTE_POINT       = 3;
    const SIP_DEVICE            = 4;
    const SIP_TRUNK             = 5;
    const MONITORED_CTI_DEVICE  = 6;

    public static function display($type)
    {
        switch ($type)
        {
            case self::SCCP :
                return "SCCP";
            case self::CTI_DEVICE :
                return "CTI Device";
            case self::CTI_ROUTE_POINT :
                return "CTI Route Point";
            case self::SIP_DEVICE :
                return "Cisco SIP Device";
            case self::SIP_TRUNK :
                return "Cisco SIP Trunk";
            default:
                return "Unknown";
        }
    }

}


// Adam:    Do you use the same enumeration for device status and component status?
// Hung:    No.  Actually . . . the device status enumeration has not been established to my knowledge.
// Adam:    Well, we need to decide now. So let's just use the same enumeration.
//          That way, we'll have additional flexibility than I originally envisioned for that field,
//          plus it will be uniform.


abstract class DeviceStatus
{

    const UNSPECIFIED =     0;
    const DISABLED =        1;
    const DISABLED_ERROR =  2;
    const ENABLED_RUNNING = 3;
    const ENABLED_STOPPED = 4;

    public static function display($status)
    {
        switch ($status)
        {
            case self::DISABLED :
                return "Disabled";
            case self::DISABLED_ERROR :
                return "Disabled Error";
            case self::ENABLED_RUNNING :
                return "Enabled Running";
            case self::ENABLED_STOPPED :
                return "Enabled Stopped";
            default :
                return "Unspecified";
        }
    }

}


/* -------------- REPLICATION ROLES ------------- */

abstract class ReplicationRole
{

    const NONE          = 0;
    const MASTER        = 1;
    const SLAVE         = 2;

}

/* ----------- CALL MANAGER VERSIONS ------------ */

abstract class CallManagerVersion
{

    public static function get_versions()
    {
        return array('3.3','4.0','4.1','4.2','5.0','5.1','6.0');
    }

}

?>