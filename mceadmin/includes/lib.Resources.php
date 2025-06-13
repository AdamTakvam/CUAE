<?php

require_once("config.php");
require_once("3rdparty/adodb/adodb.inc.php");
require_once("3rdparty/adodb/adodb-exceptions.inc.php");
require_once("3rdparty/adodb/tohtml.inc.php");


abstract class Resources
{
    
    public static function get_socket()
    {
        static $sSocket = FALSE;
        if (!$sSocket)
        {
            $sSocket = @fsockopen(MceConfig::APP_SERVER_HOST, MceConfig::APP_SERVER_PORT, $errno, $errstring, MceConfig::APP_SERVER_OPEN_WAIT);
            if (!$sSocket)
            {
                throw new Exception($errstring, $errno);
            }
        }
        return $sSocket;
    }


    public static function get_stats_socket()
    {
        static $ssSocket = FALSE;
        if (!$ssSocket)
        {
            $ssSocket = @fsockopen(MceConfig::STATS_SERVER_HOST, MceConfig::STATS_SERVER_PORT, $errno, $errstring, MceConfig::STATS_SERVER_OPEN_WAIT);
            if (!$ssSocket)
            {
                throw new Exception($errstring, $errno);
            }
        }
        return $ssSocket;
    }
        
}

class DbResource
{
    private static $mDbInstance;
    
    private function __construct()
    {
    }
    
    public static function retrieve()
    {
        if (!isset(self::$mDbInstance))
        {
            self::$mDbInstance = &ADONewConnection(MceConfig::DB_TYPE);
            @self::$mDbInstance->Connect(MceConfig::DB_SERVER, MceConfig::DB_USER, MceConfig::DB_PASSWORD, MceConfig::DB_DATABASE);
            self::$mDbInstance->SetFetchMode(ADODB_FETCH_ASSOC);
            self::$mDbInstance->SetCharSet(MceConfig::DB_CHARSET);
            if (MceConfig::SHOW_SQL)
                self::$mDbInstance->debug = TRUE;
        }
        
        return self::$mDbInstance;
    }
    
    public static function close()
    {
        if (isset(self::$mDbInstance))
        {
            self::$mDbInstance->Close();
            self::$mDbInstance = NULL;
        }
        return TRUE;
    }    
}

?>
