<?php

require_once("lib.Utils.php");


abstract class SystemUtils
{

    public static function clean_path($path)
    {
        return str_replace(array('\\\\','\\','//'),'/',$path);
    }

    public static function get_hostname()
    {
        return !empty($_ENV['COMPUTERNAME']) ? $_ENV['COMPUTERNAME'] : $_SERVER['SERVER_NAME'];
    }

}

?>