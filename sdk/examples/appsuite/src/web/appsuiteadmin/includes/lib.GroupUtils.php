<?php

require_once("common.php");
require_once("class.MceDb.php");


abstract class GroupUtils
{

    public static function get_administrators_group_ids($admin_id)
    {
        $db = new MceDb();
        $ids = $db->GetCol("SELECT as_user_groups_id FROM as_user_group_members WHERE as_users_id = ? AND user_level = ?",
                           array($admin_id, GroupUserLevel::ADMINISTRATOR));
        return $ids;
    }

    public static function get_group_administrator_ids($group_id)
    {
        $db = new MceDb();
        $ids = $db->GetCol("SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id = ? AND user_level = ?",
                           array($group_id, GroupUserLevel::ADMINISTRATOR));
        return $ids;
    }

}

?>