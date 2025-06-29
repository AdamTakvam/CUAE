<?php

require_once("class.MceDb.php");

class UserUtils
{


    public static function get_user_data($id)
    {
        $db = new MceDb();
        $data = $db->GetRow("SELECT * FROM as_users WHERE as_users_id = ?", array($id));
        if (is_array($data))
            return $data;
        else
            return array();
    }

    public static function get_associated_group_id($id)
    {
        $db = new MceDb();
        return $db->GetOne("SELECT as_user_groups_id FROM as_user_group_members WHERE as_users_id = ?", array($id));
    }

    public static function delete_user($id)
    {
        $db = new MceDb();
        $db->StartTrans();
        // Delete user associations with applications & groups
        $db->Execute("DELETE FROM as_remote_agents WHERE as_users_id = ?", array($id));
        $db->Execute("DELETE FROM as_intercom_group_members WHERE as_users_id = ?", array($id));
        $db->Execute("DELETE FROM as_user_group_members WHERE as_users_id = ?", array($id));
		$db->Execute("DELETE FROM as_activerelay_filter_numbers WHERE as_users_id = ?", array($id));
        $db->Execute("DELETE FROM as_single_reach_numbers WHERE as_users_id = ?", array($id));		
       
        // Delete user devices and numbers
        $d_ids = $db->GetCol("DELETE FROM as_directory_numbers " .
                             "WHERE as_phone_devices_id IN (SELECT as_phone_devices_id FROM as_phone_devices WHERE as_users_id = ?)", array($id));
        $db->Execute("DELETE FROM as_phone_devices WHERE as_users_id = ?", array($id));
        $db->Execute("DELETE FROM as_external_numbers WHERE as_users_id = ?", array($id));

        // Set user to delete status
        $db->Execute("UPDATE as_users SET status = ?, as_ldap_servers_id = NULL WHERE as_users_id = ?", array(UserStatus::DELETED, $id));
        $db->CompleteTrans();
        return TRUE;
    }

    public static function find_by_username($username, $group_ids = NULL)
    {
        $db = new MceDb();
        $user_id = $db->GetOne("SELECT as_users_id FROM as_users WHERE username = ? AND status <> ?", array($username, UserStatus::DELETED));
        if (is_array($group_ids) && $user_id)
        {
            $user_group_id = UserUtils::get_associated_group_id($user_id);
            if (in_array($user_group_id, $group_ids))
                return $user_id;
            else
                return FALSE;
        }
        return $user_id;
    }

    public static function find_by_name($firstname, $lastname, $group_ids = NULL)
    {
        $db = new MceDb();
        $user_id = $db->GetOne("SELECT as_users_id FROM as_users WHERE first_name = ? AND last_name = ? AND status <> ?",
                               array($firstname, $lastname, UserStatus::DELETED));
        if (is_array($group_ids)  && $user_id)
        {
            $user_group_id = UserUtils::get_associated_group_id($user_id);
            if (in_array($user_group_id, $group_ids))
                return $user_id;
            else
                return FALSE;
        }
        return $user_id;
    }
    
    public static function find_by_account_code($account_code, $group_ids = NULL)
    {
        $db = new MceDb();
        $user_id = $db->GetOne("SELECT as_users_id FROM as_users WHERE account_code = ? AND status <> ?",
                               array($account_code, UserStatus::DELETED));
        if (is_array($group_ids)  && $user_id)
        {
            $user_group_id = UserUtils::get_associated_group_id($user_id);
            if (in_array($user_group_id, $group_ids))
                return $user_id;
            else
                return FALSE;
        }
        return $user_id;
    }
    
    public static function unlock_expired_lockouts($id = 0)
    {
        $db = new MceDb();
        $query = "UPDATE as_users SET status = ? WHERE status = ? AND lockout_duration <> '00:00:00' AND NOW() > ADDTIME(last_lockout,lockout_duration)";
        if ($id > 0)
            $query .= " AND as_users_id = $id";
        $db->Execute($query,array(UserStatus::ACTIVE, UserStatus::LOCKED));
        return TRUE;
    }

}

?>