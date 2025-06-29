<?php

class MceConfig
{
    // Debug Flags
    const SHOW_SQL                      = FALSE;
    const SHOW_SQL_RS                   = FALSE;
    const SHOW_SMARTY                   = FALSE;
    
    // Mode Flags
    const DEV_MODE                      = DEVMODE_FILE_FOUND;
    const SHOW_USER_VOICEMAIL_SETTINGS  = FALSE;
    
    // Database Connect Configuration
    const DB_TYPE                       = "mysqli";
    const DB_SERVER                     = "localhost";
    const DB_USER                       = "root";
    const DB_PASSWORD                   = "metreos";
    const DB_DATABASE                   = "application_suite";
    const DB_MAIN_DATABASE              = "mce";

    // Configurable constants
    const ADMIN_USERNAME                = "Administrator";
    const OPERATING_SYSTEM              = "windows";
    const SESSION_TIMEOUT               = 3600;
    const RECORDS_PER_PAGE              = 50;
    const SC_PIN_START_RANGE            = 10000;
    const SC_PIN_END_RANGE              = 99999;
    const MAX_RECORDS_BEFORE_WARN_USER  = 500;
    const SMTP_MAIL_TIMEOUT             = 10;

    // Main Templates
    const LAYOUT_TEMPLATE               = "main_layout.tpl";
    const REMOTE_AGENT_TEMPLATE         = "remoteagent_layout.tpl";
}

?>