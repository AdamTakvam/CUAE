<?php

require_once("class.MceDb.php");

class ConfigUtils
{
    
    static public function get_global_config($name)
    {
        $db = new MceDb();
        $value = $db->GetOne("SELECT value FROM as_configs WHERE name = ? AND as_applications_id IS NULL", array($name));
        return $value;
    }

    static public function get_mce_global_config($name)
    {
        $db = new MceDb();
        $value = $db->GetOne("SELECT value FROM mce.mce_system_configs WHERE name = ?", array($name));
        return $value;
    }
    
    static public function set_global_config($name, $value)
    {
        $db = new MceDb();
        $db->Execute("UPDATE as_configs SET value = ? WHERE name = ?", array($value, $name));
    }

    static public function is_application_exposed($application_id)
    {
        $db = new MceDb();
        $value = $db->GetOne("SELECT installed FROM as_applications WHERE as_applications_id = ?", array($application_id));
        return $value;
    }
    
}

?>