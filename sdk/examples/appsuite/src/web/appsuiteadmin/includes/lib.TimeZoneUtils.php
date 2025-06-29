<?php

require_once("class.MceDb.php");
require_once("class.AccessControl.php");
require_once("lib.UserUtils.php");

define('__TIME_ZONE_UTILS_DEFAULT_TIME_ZONE__', 'SYSTEM');


abstract class TimeZoneUtils
{

    public static function get_timezone_list()
    {
        $db = new MceDb();
        $tz_list = $db->GetCol("SELECT Name FROM mysql.time_zone_name ORDER BY Name ASC");
        return $tz_list;
    }

    public static function get_user_timezone(AccessControl $ac)
    {
        if ($ac->CheckAccess(AccessControl::ADMINISTRATOR))
            $tz = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_TIMEZONE);
        else
        {
            $user_id = $ac->GetUserId();
            $user_data = UserUtils::get_user_data($user_id);
            $tz = $user_data['time_zone'];
        }
        if (empty($tz))
            return __TIME_ZONE_UTILS_DEFAULT_TIME_ZONE__;
        else
            return $tz;
    }
    
    public static function apply_offset($timestamp, $offset)
    {
        return ($timestamp + intval(floatval($offset) * 3600));
    }
    

}

?>