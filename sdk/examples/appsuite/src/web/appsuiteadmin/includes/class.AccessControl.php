<?php

require_once("common.php");
require_once("class.ErrorHandler.php");
require_once("lib.GroupUtils.php");
require_once("lib.LdapUtils.php");


class AccessControl
{

    protected $mDb;
    protected $mAccessLevel;


    const UNKNOWN =         0;
    const NORMAL =          1;
    const GROUP_ADMIN =     32;
    const ADMINISTRATOR =   42;


    function __construct()
    {
        $this->mDb = new MceDb();
        session_name("MCEAppSuite");
        session_start();
        $_SESSION['page_session_checked'] = FALSE;
    }

    function Login($username, $password)
    {
        $_SESSION['username'] = $username;
        if (strtolower($username) == strtolower(MceConfig::ADMIN_USERNAME))
        {
            $_SESSION['password'] = Utils::encrypt_password($password);
            $_SESSION['access_level'] = self::ADMINISTRATOR;
        }
        else
        {
            $_SESSION['password'] = $password;
            $_SESSION['access_level'] = self::NORMAL;
        }
        $_SESSION['ip'] =  $_SERVER['REMOTE_ADDR'];
        $_SESSION['last_accessed'] = time();
        if (!$this->CheckLogin())
        {
            $this->Destroy();
            return FALSE;
        }
        return TRUE;
    }

    function CheckPageAccess($accessLevel, $id = NULL)
    {
        $this->DenyPageAccess(!$this->CheckAccess($accessLevel, $id));
        return TRUE;
    }

    function CheckAccess($accessLevel, $id = NULL)
    {
        if ($this->CheckLogin())
        {
            if ($_SESSION['access_level'] > $accessLevel) 
                return TRUE;
                
            if ($accessLevel == AccessControl::GROUP_ADMIN && !empty($id))
            {
                $user_groups = $this->mDb->GetCol("SELECT as_user_groups_id FROM as_user_group_members WHERE as_users_id = ?", array($id));
                $admin_groups = GroupUtils::get_administrators_group_ids($_SESSION['user_id']);
                return (sizeof(array_intersect($user_groups, $admin_groups)) > 0);
            }
            
            if ($accessLevel == AccessControl::NORMAL && !empty($id))
                return ($_SESSION['access_level'] == AccessControl::NORMAL && $_SESSION['user_id'] == $id);
 
            return ($_SESSION['access_level'] == $accessLevel);
        }
        else
        {
            $this->Destroy();
            $message = "We're sorry, but your session has expired.  Please log in again to start a new session.";
            Utils::redirect('/appsuiteadmin/index.php?message=' . Utils::safe_serialize($message));
        }
        return FALSE;
    }

    function CheckLogin()
    {
        if (time() > ($_SESSION['last_accessed'] + MceConfig::SESSION_TIMEOUT)) { return FALSE; }
        if ($_SESSION['username'] && $_SESSION['password'])
        {
            // Check login credentials for this page if it has not already happened
            if (!$_SESSION['page_session_checked'])
            {
                
                // Check for administrator login
                if (strtolower($_SESSION['username']) == strtolower(Mceconfig::ADMIN_USERNAME))
                {
                    $admin_pass = $this->mDb->GetOne("SELECT value FROM as_configs WHERE name = ?",
                                                     array(GlobalConfigNames::ADMIN_PASSWORD));
                    if ($admin_pass == $_SESSION['password']) 
                        $_SESSION['page_session_checked'] = TRUE;
                    $_SESSION['user_id'] = 0;
                }
                else
                {
                    $data = $this->mDb->GetRow("SELECT as_users_id, external_auth_enabled FROM as_users WHERE username = ? AND status <> ?",
                                               array($_SESSION['username'], UserStatus::DELETED));
                    if ($data['external_auth_enabled'] && $data['as_users_id'])
                    {
                        $errors = new ErrorHandler();
                        // Authenticate user with the LDAP server
                        $ext_data = $this->mDb->GetRow("SELECT as_ldap_servers_id, external_auth_dn FROM as_users WHERE as_users_id = ?",
                                                       array($data['as_users_id']));
                        $ldap = $this->mDb->GetRow("SELECT * FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($ext_data['as_ldap_servers_id']));
                        
                        $conn = LdapUtils::make_connection($ldap['hostname'], $ldap['port'], $ldap['secure_connect'], $ldap['user_dn'], $ldap['password'], $errors);
                        if ($errors->IsEmpty())
                        {
                            $result = ldap_search($conn, $ext_data['external_auth_dn'], "(cn=*)", array('userpassword'));
                            if (ldap_count_entries($conn, $result) > 0)
                            {
                                $info = ldap_get_entries($conn, $result);
                                if ($info[0]['userpassword'][0] == "{md5}".md5($_SESSION['password'],TRUE))
                                {
                                    $user_id = $data['as_users_id'];
                                    $_SESSION['page_session_checked'] = TRUE;
                                }
                            }
                            @ldap_close($conn);
                        }
                    }
                    else
                    {
                        // Check for local user login
                        $user_id = $this->mDb->GetOne("SELECT as_users_id FROM as_users WHERE username = ? AND password = ? AND status <> ?",
                                                        array($_SESSION['username'], $_SESSION['password'], UserStatus::DELETED));
                        if ($user_id > 0) 
                            $_SESSION['page_session_checked'] = TRUE;
                    }
                    $_SESSION['user_id'] = $user_id;
                }

                // Determine if the user is a group administrator
                if ($_SESSION['user_id'] > 0)
                {
                    $group_ids = GroupUtils::get_administrators_group_ids($_SESSION['user_id']);
                    if (sizeof($group_ids) > 0)
                    {
                        $_SESSION['access_level'] = AccessControl::GROUP_ADMIN;
                    }
                }
            }

            if ($_SESSION['page_session_checked'])
            {
                $_SESSION['last_accessed'] = time();
                return TRUE;
            }
        }
        return FALSE;
    }

    function DenyPageAccess($condition = TRUE)
    {
        if ($condition)
            Utils::redirect("/appsuiteadmin/page.php?no_access");
    }
    
    function UpdatePassword($password)
    {
        if (AccessControl::ADMINISTRATOR == $_SESSION['access_level'])
        {
            $_SESSION['password'] = Utils::encrypt_password($password);
        }
        else
        {
            $_SESSION['password'] = $password;
        }
    }

    function GetUserId()
    {
        return $_SESSION['user_id'];
    }

    function Destroy()
    {
        session_unset();
        session_destroy();
    }

}

?>
