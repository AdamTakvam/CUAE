<?php

require_once('class.MceDb.php');

abstract class SystemConfig
{

    const FIRMWARE_VERSION          = 'firmware_version';
    const SOFTWARE_VERSION          = 'software_version';
    const RELEASE_TYPE              = 'release_type';

    const ROUTINE_BACKUP_DBS        = 'routine_backup_databases';
    const ROUTINE_BACKUP_DBS_DATA   = 'routine_backup_database_data';
    const ROUTINE_BACKUP_SCHEDULE   = 'routine_backup_schedule';

    const APACHE_NEEDS_RESTART      = 'apache_restart_needed';
    
    const MEDIA_PROVISION_PASSWORD  = 'media_provisioning_password';


    public static function get_config($config_name)
    {
        $db = new MceDb();
        return $db->GetOne("SELECT value FROM mce_system_configs WHERE name = ?", array($config_name));
    }


    public static function store_config($config_name, $value)
    {
        $db = new MceDb();
        return $db->GetOne("UPDATE mce_system_configs SET value = ? WHERE name = ?", array($value, $config_name));
    }

}

?>