<?php

class DateUtils
{


    public static function sec_to_min_sec_string($seconds)
    {
        $seconds = floatval($seconds);
        $secs = $seconds % 60;
        $mins = floor($seconds / 60);
        return "$mins min $secs sec";
    }


    public static function timezone_offset($timestamp, $offset)
    {
        return ($timestamp + intval(floatval($offset) * 3600));
    }


}

?>