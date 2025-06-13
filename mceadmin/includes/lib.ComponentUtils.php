<?php

require_once("common.php");

abstract class ComponentUtils
{


    public static function cleanup_config_metas()
    {
        $db = new MceDb();
        $metas = $db->GetCol(   "SELECT mce_config_entry_metas.mce_config_entry_metas_id" .
                                "FROM mce_config_entry_metas " .
                                "LEFT JOIN mce_config_entries USING (mce_config_entry_metas_id) " .
                                "WHERE mce_config_entries_id IS NULL");
        foreach ($metas as $meta_id)
        {
            $db->Execute("DELETE FROM mce_config_entry_metas WHERE mce_config_entry_metas_id = ?", array($meta_id));
        }
        return TRUE;
    }


    public static function get_call_route_groups($exclude_id = 0)
    {
        if (empty($exclude_id))
            $exclude_id = 0;
        $groups = array(GroupType::SCCP_GROUP,
                        GroupType::H323_GATEWAY_GROUP,
                        GroupType::SIP_GROUP,
                        GroupType::CTI_SERVER_GROUP,
                        GroupType::TEST_IPT_GROUP);
        $db = new MceDb();
        return $db->GetAll("SELECT * FROM mce_component_groups WHERE component_type IN (" . implode(',',$groups) . ") AND mce_component_groups_id <> ?", 
                            array($exclude_id));
    }
    
    public static function get_default_group_id_of_type($type)
    {
        $db = new MceDb();
        return $db->GetOne("SELECT mce_component_groups_id FROM mce_component_groups WHERE component_type = ? AND default_group = ?",
                           array($type,1));
    }
    
    public static function get_groups_of_type($type, $exclude_id = 0)
    {
        if (empty($exclude_id))
            $exclude_id = 0;
        $db = new MceDb();
        return $db->GetAll("SELECT * FROM mce_component_groups WHERE component_type = ? AND mce_component_groups_id <> ?", 
                            array($type, $exclude_id));
    }


    public static function get_standard_config_metas($type)
    {
        $db = new MceDb();
        return $db->GetAll("SELECT * FROM mce_config_entry_metas WHERE component_type = ?", array($type));
    }


    public static function determine_group_type($type)
    {
        if ($type == ComponentType::MEDIA_SERVER)
        {
            return GroupType::MEDIA_RESOURCE_GROUP;
        }
        else if ($type >= ALARM_TYPE_ENUM_START && $type <= ALARM_TYPE_ENUM_END)
        {
            return GroupType::ALARM_GROUP;
        }
        else if ($type >= IPT_TYPE_ENUM_START && $type <= IPT_TYPE_ENUM_END)
        {
            switch ($type)
            {
                case ComponentType::CTI_DEVICE_POOL:
                case ComponentType::CTI_ROUTE_POINT:
                    return GroupType::CTI_SERVER_GROUP;
                case ComponentType::SIP_DEVICE_POOL:
                case ComponentType::SIP_TRUNK_INTERFACE:
                case ComponentType::IETF_SIP_DEVICE_POOL:
                    return GroupType::SIP_GROUP;
                default:
                    return $type;
            }
        }
        else
        {
            return GroupType::UNSPECIFIED;
        }
    }


    public static function get_config($id)
    {
        $db = new MceDb();
        return $db->GetRow("SELECT * FROM mce_config_entries LEFT JOIN mce_config_entry_metas USING (mce_config_entry_metas_id) " .
                           "WHERE mce_config_entries_id = ? ", array($id));
    }


    public static function get_config_values($id)
    {
        $db = new MceDb();
        return $db->GetAll("SELECT * FROM mce_config_values WHERE mce_config_entries_id = ?", array($id));
    }

    public static function get_existing_partition_config($app_config_id, $part_id)
    {
        $db = new MceDb();
        $meta_conf = $db->GetOne("SELECT mce_config_entry_metas_id FROM mce_config_entries WHERE mce_config_entries_id = ?", array($app_config_id));
        $id = $db->GetOne("SELECT mce_config_entries_id FROM mce_config_entries WHERE mce_config_entry_metas_id = ? AND mce_application_partitions_id = ?",
                          array($meta_conf, $part_id));
        return $id;
    }

    public static function find_component_id($component_name)
    {
        $db = new MceDb();
        $id = $db->GetOne("SELECT mce_components_id FROM mce_components WHERE name = ?", array($component_name));
        return $id;
    }
    
}

?>