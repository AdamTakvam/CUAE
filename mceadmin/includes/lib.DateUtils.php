<?php

require_once("class.MceDb.php");


define('__DATE_UTILS_DEFAULT_TIME_ZONE__', 'SYSTEM');


abstract class DateUtils
{

    public static function get_timezone_list()
    {
        $db = new MceDb();
        $tz_list = $db->GetCol("SELECT Name FROM mysql.time_zone_name ORDER BY Name ASC");
        return $tz_list;
    }

    public static function sec_to_min_sec_string($seconds)
    {
        $seconds = floatval($seconds);
        $secs = $seconds % 60;
        $mins = floor($seconds / 60);
        return "$mins min $secs sec";
    }

    public static function timezone_offset($timestamp, $offset)
    {
        throw new Exception("DateUtils::timezone_offset() deprecated.  Use DateUtils::timezone_adjust().");
    }


    public static function timezone_adjust($timestamp, $timezone)
    {
        $db = new MceDb();
        try
        {
            return $db->GetOne("SELECT CONVERT_TZ(FROM_UNIXTIME($timestamp),'".__DATE_UTILS_DEFAULT_TIME_ZONE__."','$timezone')");
        }
        catch (Exception $e)
        {
            return FALSE;
        }
    }

}

?>