<?php

require_once("common.php");

class UserGroup
{

    // ** PRIVATE MEMBERS **

    private $mId;
    private $mName;
    private $mErrorMessage;
    
    
    // ** PUBLIC METHODS **
    
    public function __construct()
    {
        $this->mId = NULL;
        $this->mName = NULL;
        $this->mErrorMessage = NULL;
    }
    
    public function SetId($id)
    {
        $db = new MceDb();
        $this->mId = $id;
        $this->mName = $db->GetOne("SELECT name FROM as_user_groups WHERE as_user_groups_id = ?", array($id));
        if (empty($this->mName))
        {
            $this->mId = NULL;
            throw new Exception("UserGroup object set to an id that does not exist");
        }
        return TRUE;
    }
    
    public function GetName()
    {
        $this->Validate();
        return $this->mName;
    }
    
    public function GetSize()
    {
        $db = new MceDb();
        $db->GetOne("SELECT COUNT(*) FROM as_user_group_members WHERE as_user_groups_id = ?", array($this->mId));
    }
    
    public function SetAdministrators($user_ids)
    {
        return $this->SetUsersLevel($user_ids, GroupUserLevel::ADMINISTRATOR);
    }
    
    public function UnsetAdministrators($user_ids)
    {
        return $this->SetUsersLevel($user_ids, GroupUserLevel::MEMBER);
    }
    
    public function GetUsers($start = NULL, $limit = NULL)
    {
        $db = new MceDb();
        $query  = "SELECT as_users.as_users_id AS as_users_id, first_name, last_name, username, user_level ";
        $query .= "FROM as_user_group_members JOIN as_users USING (as_users_id) ";
        $query .= "WHERE as_user_groups_id = $this->mId ORDER BY username ASC ";
        if (!empty($start) && !empty($limit))
        {
            $query .= "LIMIT $start, $limit";
        }
        return $db->GetAll($query);
    }
    
    public function AddUser($user_id)
    {
        $this->Validate();
        $db = new MceDb();
        $id = $db->GetOne("SELECT as_user_group_members_id FROM as_user_group_members WHERE as_users_id = ?", array($user_id));
        if (empty($id))
        {
            $db->Execute("INSERT INTO as_user_group_members SET as_users_id = ?, as_user_groups_id = ?, user_level = ?",
                         array($user_id, $this->mId, GroupUserLevel::MEMBER));
            return TRUE;
        }
        else
        {
            $this->mErrorMessage = "This user is already a member of this or another user group.";
            return FALSE;
        }
    }

    public function ForceAddUser($user_id)
    {
        $this->Validate();
        $db = new MceDb();
        $nochange = $db->GetOne("SELECT 1 FROM as_user_group_members WHERE as_users_id = ? AND as_user_groups_id = ?", array($user_id, $this->mId));
        if (!$nochange)
        {
            $db->StartTrans();
            $db->Execute("DELETE FROM as_user_group_members WHERE as_users_id = ?", array($user_id));
            $db->Execute("INSERT INTO as_user_group_members SET as_users_id = ?, as_user_groups_id = ?, user_level = ?",
                         array($user_id, $this->mId, GroupUserLevel::MEMBER));
            $db->CompleteTrans();
        }
        return TRUE;
    }
    
    public function RemoveUser($user_id)
    {
        $this->Validate();
        $db = new MceDb();
        $db->Execute("DELETE FROM as_user_group_members WHERE as_users_id = ? AND as_user_groups_id = ?",
                     array($user_id, $this->mId));
        return TRUE;
    }
    
    public function RemoveUsers($user_id_array)
    {
        $this->Validate();
        if (is_array($user_id_array))
        {
            $db = new MceDb();
            $db->Execute("DELETE FROM as_user_group_members WHERE as_user_groups_id = ? AND as_users_id IN (" . implode(',',$user_id_array) . ")",
                         array($this->mId));
            return TRUE;
        }
        else
            throw new Exception('RemoveUsers methods requires an array');
    }
    
    public function Delete()
    {
        $this->Validate();
        $db = new MceDb();
        $db->StartTrans();
        $db->Execute("DELETE FROM as_user_group_members WHERE as_user_groups_id = ?", array($this->mId));
        $db->Execute("DELETE FROM as_user_groups WHERE as_user_groups_id = ?", array($this->mId));
        $db->CompleteTrans();
        return TRUE;
    }
    
    public function GetError()
    {
        return $this->mErrorMessage;
    }
    
    
    // **  PRIVATE METHODS **
    
    private function Validate()
    {
        if (!($this->mId > 0))
            throw new Exception('UserGroup object not associated with a valid group');
        return true;
    }
    
    private function SetUserLevel($user_id, $level)
    {
        $this->Validate();
        $db = new MceDb();
        $db->Execute("UPDATE as_user_group_members SET user_level = ? WHERE as_users_id = ? AND as_user_groups_id = ?",
                     array($level, $user_id, $this->mId));
        return TRUE;    
    }

    private function SetUsersLevel($user_ids, $level)
    {
        $this->Validate();
        $db = new MceDb();
        if (is_array($user_ids))
        {
            $db->Execute("UPDATE as_user_group_members SET user_level = ? WHERE as_user_groups_id = ? " .
                         "AND as_users_id IN (" . implode(',',$user_ids) . ")",
                         array($level, $this->mId));
            return TRUE;
        }
        else
            throw new Exception('Call to method SetUsersLevel require the first argument to be an array.');
    }
    
}

?>