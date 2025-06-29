<?php

require_once("config.php");
require_once("3rdparty/adodb/adodb.inc.php");
require_once("3rdparty/adodb/adodb-exceptions.inc.php");
require_once("3rdparty/adodb/tohtml.inc.php");


class Resources
{
    // TODO: Properly implement singleton for this
    public static function get_main_db()
    {
		static $db = FALSE;
		if (!$db)
		{
			$db = &ADONewConnection(MceConfig::DB_TYPE);
			@$db->Connect(MceConfig::DB_SERVER, MceConfig::DB_USER, MceConfig::DB_PASSWORD, MceConfig::DB_MAIN_DATABASE);
			$db->SetFetchMode(ADODB_FETCH_ASSOC);
			if (MceConfig::SHOW_SQL)
			{
				$db->debug = TRUE;
			}
		}
		return $db; 	
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
