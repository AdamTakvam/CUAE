<?php

// General Paths

define('MCE_CONSOLE_ROOT',                  dirname(dirname(__FILE__)));
define('IMAGES_ROOT',                       MCE_CONSOLE_ROOT . "/public/images");
define('DEVMODE_FILE_FOUND',                file_exists(MCE_CONSOLE_ROOT . "/devmode"));
define('INSTALL_DONE_FILE',                 MCE_CONSOLE_ROOT . "/installed");
define('INSTALL_PERFORMED',                 file_exists(INSTALL_DONE_FILE));

define('TEMP_PATH',							MCE_CONSOLE_ROOT . "/temp");
define('SESSIONS_PATH',						TEMP_PATH . "/sessions");

if (is_dir(dirname(MCE_CONSOLE_ROOT) . "/Build"))
    define('METREOS_ROOT',                  dirname(MCE_CONSOLE_ROOT) . "/Build");
else
    define('METREOS_ROOT',                  dirname(MCE_CONSOLE_ROOT));

define('APP_SERVER_ROOT',                   METREOS_ROOT . "/AppServer");
define('MEDIA_SERVER_ROOT',                 METREOS_ROOT . "/MediaServer");
define('FRAMEWORK_ROOT',                    METREOS_ROOT . "/Framework/1.0");
define('LICENSE_MANAGER_ROOT',              METREOS_ROOT . "/LicenseServer");
define('LICENSES_PATH',                     LICENSE_MANAGER_ROOT . "/Licenses");
define('SCRIPTS_ROOT',                      MCE_CONSOLE_ROOT . "/scripts");
define('LOGS_ROOT',                         is_dir(METREOS_ROOT . "/logs") ? METREOS_ROOT . "/logs" : "C:/Metreos/logs");
define('WEB_PATH',                          '/mceadmin');
define('STATS_WEB_PATH',                    '/stats');


// Smarty Template Paths

define('SMARTY_DIR',                        MCE_CONSOLE_ROOT . "/includes/3rdparty/smarty/libs/");
define('SMARTY_TEMPLATE_DIR',               MCE_CONSOLE_ROOT . "/templates");
define('SMARTY_TEMPLATE_C_DIR',             MCE_CONSOLE_ROOT . "/templates_c");
define('SMARTY_CONFIG_DIR',                 SMARTY_DIR . "configs/");
define('SMARTY_CACHE_DIR',                  SMARTY_DIR . "cache/");


// Enumeration ranges (extend later)

define('CONFIG_META_DATA_START',            101);
define('ALARM_TYPE_ENUM_START',             50);
define('ALARM_TYPE_ENUM_END',               99);
define('IPT_TYPE_ENUM_START',               100);
define('IPT_TYPE_ENUM_END',                 149);


// Service Key Names

define('WEB_SERVER_SERVICE_NAME',           'Apache');
define('MYSQL_SERVICE_NAME',                'MySQL');
define('DIALOGIC_SERVICE_NAME',             'Dialogic');
define('APP_SERVER_SERVICE_NAME',           'MetreosAppServerService');
define('MEDIA_SERVER_SERVICE_NAME',         'MediaServerService');
define('LICENSE_MANAGER_SERVICE_NAME',      'CUAE License Server');


// Provider names

define('MEDIA_SERVER_PROVIDER',             'MediaServerProvider');
define('JTAPI_PROVIDER',                    'JTapiProvider');
define('H323_PROVIDER',                     'H323Provider');
define('SCCP_PROVIDER',                     'SccpProvider');
define('SIP_PROVIDER',                      'SipProvider');
define('MEDIA_CONTROL_PROVIDER',            'MediaControlProvider');
define('DEVICELISTX_PROVIDER',              'CiscoDeviceListX');
define('CLEAR_MRG_CACHE',                   'Metreos.MediaControl.ClearMRGCache');


// Default Names and IDs for Components/Partitions/etc.

define('MAIN_ADMIN_ACCOUNT_ID',             1);
define('DEFAULT_ALARM_GROUP_ID',            5);

define('SNMP_MANAGER_NAME',                 'SnmpManager');
define('SMTP_MANAGER_NAME',                 'SmtpManager');

define('DEFAULT_PARTITION_NAME',            'Default');
define('PRINT_DIAG_EXTENSION_NAME',         'PrintDiags');


// Miscellaneous

define('MIN_SYSTEM_USER_PASSWORD_LENGTH',   7);
define('SERVICE_WAIT',                      TRUE);
define('SERVICE_DO_NOT_WAIT',               FALSE);

?>