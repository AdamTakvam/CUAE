<?php

require_once("lib.Resources.php");

abstract class MceUtils
{


    public static function generate_xml_command($name, $params = array())
    {
        $xml[] = '<?xml version="1.0" encoding="utf-8" ?>';
        if (array() != $params)
        {
            $xml[] = '<command name="' . $name . '">';
            foreach ($params as $key => $value)
            {
                $xml[] = '<param name="' . $key . '">' . $value . '</param>';
            }
            $xml[] = '</command>';
        }
        else
        {
            $xml[] = '<command name="' . $name . '" />';
        }
        return implode("\n", $xml);
    }
    
    
    public static function is_app_server_running()
    {
        if ($_SESSION['app_server_running'] || !isset($_SESSION['app_server_running']))
        {
            try
            {
                $connect = Resources::get_socket();
                $_SESSION['app_server_running'] = TRUE;
            }
            catch (Exception $e)
            {
                $_SESSION['app_server_running'] = FALSE;               
            }
        }
        return $_SESSION['app_server_running'];
    }
    
    
    public static function remove_password_info($array)
    {
    	foreach ($array as $key => $val)
    	{
    		if (eregi('password', $key))
    			$array[$key] = "**********";
    	}
    	return $array;
    }
    
}

?>