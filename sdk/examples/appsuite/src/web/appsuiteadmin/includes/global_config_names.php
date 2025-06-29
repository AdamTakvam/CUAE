<?php

class GlobalConfigNames
{

    const ADMIN_PASSWORD            = "admin_password";
    const RECORDINGS_PATH           = "record_rel_path";
    const MEDIA_PATH                = "media_rel_path";
    const DEFAULT_TIMEZONE          = "default_timezone_offset";
    const DEFAULT_LOCKOUT_THRESHOLD = "default_lockout_threshold";
    const DEFAULT_LOCKOUT_DURATION  = "default_lockout_duration";
    const DEFAULT_MAX_CONCURRENT_S  = "default_max_concurrent_sessions";
    const REPORTS_TIMEZONE          = "report_timezone_offset";
    const SCHEDCONFERNCE_DN         = "scheduled_conference_dn";
    const SMTP_SERVER               = "smtp_server";
    const SMTP_PORT                 = "smtp_port";
    const SMTP_USER                 = "smtp_user";
    const SMTP_PASSWORD             = "smtp_password";
    const MEDIA_PORTS               = "media_ports";
    const LDAP_UID_ATTRIB           = "ldap_username_attribute";
    const LDAP_AC_ATTRIB            = "ldap_account_code_attribute";
    const HIDE_DEVICES_FROM_USERS   = "hide_devices_from_users";
    const MAX_EXT_NUMBERS_PER_USER  = "max_find_me_numbers_per_user";
    const FIND_ME_VALIDATE_REGEXS   = "find_me_numbers_validate_regexes";
    const FIND_ME_BLACKLIST_REGEXS  = "find_me_numbers_blacklist_regexes";
    const FIND_ME_DESCRIPTION       = "find_me_numbers_description";
    const CUSTOM_LOGO_FILE          = "custom_logo_file";
    
    const MCE_FIRMWARE_VERSION      = "firmware_version";
    const MCE_SOFTWARE_VERSION      = "software_version";    
    const MCE_RELEASE_TYPE          = 'release_type';
    
}

?>