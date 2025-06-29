<?php

class DeviceUtils
{
    
    
    public static function auto_set_primary_device($user_id)
    {
        $db = new MceDb();
        $already_set = $db->GetOne("SELECT COUNT(*) FROM as_phone_devices WHERE as_users_id = ? AND is_primary_device = 1",
                              array($user_id));
        if (!$already_set)
        {
            $id = $db->GetOne("SELECT MIN(as_phone_devices_id) FROM as_phone_devices WHERE as_users_id = ? AND as_phone_devices_id", array($user_id));
            $db->Execute("UPDATE as_phone_devices SET is_primary_device = 1 WHERE as_phone_devices_id = ?", array($id));
        }
    }
    
    
    public static function auto_set_primary_number($device_id)
    {
        $db = new MceDb();       
        $already_set = $db->GetOne("SELECT COUNT(*) FROM as_directory_numbers WHERE as_phone_devices_id = ? AND is_primary_number = 1",
                              array($device_id));
        if (!$already_set)
        {
            $id = $db->GetOne("SELECT MIN(as_directory_numbers_id) FROM as_directory_numbers WHERE as_phone_devices_id = ?", 
                              array($device_id));
            $db->Execute("UPDATE as_directory_numbers SET is_primary_number = 1 WHERE as_directory_numbers_id = ?", array($id));
        }
    }
 
    
}

?>