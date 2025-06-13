<?php

require_once("constants.php");

define('__CONFIG_PHP_LOG_TEMP',     TEMP_PATH . '/logs/');

abstract class MceConfig
{

    // Runing environment config
    const OPERATING_SYSTEM          = "windows";
    const DEV_MODE                  = DEVMODE_FILE_FOUND;
    const APP_SERVER_INSTALLED      = TRUE;
    const MEDIA_SERVER_INSTALLED    = TRUE;

    // Debugging-type configs
    const SHOW_SQL                  = FALSE;
    const SHOW_SQL_RS               = FALSE;
    const SHOW_SMARTY               = FALSE;

    // Database configs
    const DB_TYPE                   = "mysqli";
    const DB_SERVER                 = "localhost";
    const DB_USER                   = "root";
    const DB_PASSWORD               = "metreos";
    const DB_DATABASE               = "mce";
    const DB_STANDBY_DATABASE       = "mce_standby";
    const DB_CHARSET                = "utf8";

    // Application installation configs
    const APP_INSTALL_DIRECTORY     = "/Deploy";
    const APP_INSTALL_WAIT          = 2;
    const APP_DIRECTORY             = "/Applications";

    // Provider installation configs
    const PROVIDER_INSTALLER        = "/instprov.exe";

    // Server log configs
    const CONSOLE_LOG_DIR           = "/Management";
    const LOG_TEMP                  = __CONFIG_PHP_LOG_TEMP;
    const LOG_ARCHIVE_SUFFIX        = "-logs";

    // Application server interface configs
    const APP_SERVER_HOST           = '127.0.0.1';
    const APP_SERVER_PORT           = 8120;
    const APP_SERVER_OPEN_WAIT      = 1;
    const APP_SERVER_WAIT           = 5;
    const APP_SERVER_INSTALL_WAIT   = 300;
    const APP_SERVER_PROVIDER_WAIT  = 20;
    const APP_SERVER_RCVLEN         = 4096;
    const APP_MESSAGE_LENGTH_SIZE   = 4;

    // Statistic server interface configs
    const STATS_SERVER_HOST         = '127.0.0.1';
    const STATS_SERVER_PORT         = 9200;
    const STATS_SERVER_OPEN_WAIT    = 1;
    
    // Miscellaneous configs
    const USERS_PER_PAGE            = 50;
    const RECORDS_PER_PAGE          = 50;
    const SESSION_TIMEOUT           = 3600;             // One hour in seconds
    const SESSION_LIFESPAN          = 86400;            // One day in seconds

    // Static Layout config
    const LAYOUT_TEMPLATE           = "main_layout.tpl";
}

?>