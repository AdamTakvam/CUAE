<?php

require_once("init.php");
require_once("Mail\RFC822.php");       // PEAR Library
require_once("lib.DeviceUtils.php");
require_once("lib.UserUtils.php");
require_once("lib.ConfigUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.UserGroup.php");

define('FIRST_ACCOUNT_CODE', 1000);


// ** Set Up **

$id = $_REQUEST['id'];

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL, $id);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
if ($id > 0 && $group_admin_access)
    $group_admin_access = $access->CheckPageAccess(AccessControl::GROUP_ADMIN, $id);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();
$ext_numbers_limit = ConfigUtils::get_global_config(GlobalConfigNames::MAX_EXT_NUMBERS_PER_USER);
$response = Utils::safe_unserialize($_REQUEST['s_response']);
Utils::trim_array($_POST);


// ** Handle User Requests **

if ($_POST['cancel'])
{
    if ($group_admin_access)
        Utils::redirect('account_mgmt.php');
    else
        Utils::redirect('main.php');
}

if ($id > 0) {


    // Delete user
    if ($_POST['delete'])
        Utils::redirect("user_confirm_delete.php?id=$id");
        
    // Edit voicemail settings
    if (MceConfig::SHOW_USER_VOICEMAIL_SETTINGS && $_POST['edit_voicemail'])
        Utils::redirect("user_voicemail_settings.php?id=$id");

    // ActiveRelay Filters
    if ($_POST['whitelist'])
        Utils::redirect("user_ar_filter_manage.php?id=$id&type=" . FilterNumberType::ALLOW);

    if ($_POST['blacklist'])
        Utils::redirect("user_ar_filter_manage.php?id=$id&type=" . FilterNumberType::BLOCK);


    // Device management
    if ($_POST['add_device'])
    {
        Utils::redirect("device.php?user_id=$id");
    }

    if ($_POST['edit_device'])
    {
        $d_id = Utils::get_first_array_key($_POST['edit_device']);
        Utils::redirect("device.php?user_id=$id&id=$d_id");
    }

    if ($_POST['delete_device'])
    {
        $d_id = Utils::get_first_array_key($_POST['delete_device']);
        $db->Execute("DELETE FROM as_directory_numbers WHERE as_phone_devices_id = ?", array($d_id));
        $db->Execute("DELETE FROM as_phone_devices WHERE as_phone_devices_id = ?", array($d_id));
        DeviceUtils::auto_set_primary_device($id);
        $response = "Device deleted.";
    }

    if ($_POST['set_primary_device'])
    {
        $d_id = Utils::get_first_array_key($_POST['set_primary_device']);
        $db->Execute("UPDATE as_phone_devices SET is_primary_device = 0 WHERE as_users_id = ?", array($id));
        $db->Execute("UPDATE as_phone_devices SET is_primary_device = 1 WHERE as_phone_devices_id = ?", array($d_id));
        $response = "Primary device set";
    }
    
    if ($_POST['set_corporate'])
    {
        $db->Execute("UPDATE as_external_numbers SET is_corporate = 0 WHERE as_users_id = ?", array($id));
        if ($_POST['corporate_number_id'] > 0)
            $db->Execute("UPDATE as_external_numbers SET is_corporate = 1, timeofday_enabled = 0, timeofday_weekend = 0 WHERE as_external_numbers_id = ?", array($_POST['corporate_number_id']));
        $response = "Voice mail box number set";
    }


    // External number management
    if ($_POST['add_number'])
    {
        Utils::redirect("ext_number.php?user_id=$id");
    }

    if ($_POST['edit_number'])
    {
        $d_id = Utils::get_first_array_key($_POST['edit_number']);
        Utils::redirect("ext_number.php?user_id=$id&id=$d_id");
    }

    if ($_POST['delete_number'])
    {
        $n_id = Utils::get_first_array_key($_POST['delete_number']);
        $db->Execute("DELETE FROM as_external_numbers WHERE as_external_numbers_id = ?", array($n_id));
        $response = "Number deleted.";
    }


    // Single Reach Number Management
    if ($_POST['add_sr'])
    {
        if (Utils::is_blank($_POST['new_sr']))
        {
            $errors->Add("The new Single Reach number is blank");
        }
        else
        {
            $db->Execute("INSERT INTO as_single_reach_numbers SET number = ?, as_users_id = ?", array($_POST['new_sr'],$id));
            $response = "A new Single Reach number has been added";
        }
    }
    
    if ($_POST['delete_sr'])
    {
        $d_id = Utils::get_first_array_key($_POST['delete_sr']);
        $db->Execute("DELETE FROM as_single_reach_numbers WHERE as_single_reach_numbers_id = ?", array($d_id));
        $response = "The Single Reach number has been deleted";
    }
    
    // Reset actions
    if ($_POST['reset_failed_logins'])
    {
        $db->Execute("UPDATE as_users SET failed_logins = 0 WHERE as_users_id = ?", array($id));
        Utils::redirect($_SERVER['PHP_SELF'] . "?id=$id");
    }

    if ($_POST['reset_concurrent_logins'])
    {
        $db->Execute("UPDATE as_users SET current_active_sessions = 0 WHERE as_users_id = ?", array($id));
        Utils::redirect($_SERVER['PHP_SELF'] . "?id=$id");
    }
    
}


// Verify any submitted data
if ($_POST['submit'])
{
    if (empty($_POST['first_name']))
        $errors->Add('First name is required.');
    if (empty($_POST['last_name']))
        $errors->Add('Last name field is required.');
    if (!empty($_POST['email']) && !Mail_RFC822::isValidInetAddress($_POST['email']))
        $errors->Add('E-mail address is not valid');
        
    if (empty($id))
    {
        if (empty($_POST['password']))
            $errors->Add('Password is required.');
        if ($_POST['password'] != $_POST['password_verify'])
            $errors->Add('Password could not be verified.');
            
        if (!$_POST['generate_account_code'])
        {
            if (!is_numeric($_POST['pin']))
                $errors->Add('PIN is not a number');
            if ($_POST['pin'] != $_POST['pin_verify'])
                $errors->Add('PIN could not be verified.');
        }
    }
        
    if ($group_admin_access || $admin_access)
    {
        // Check username field
        if (isset($_POST['username']) && empty($_POST['username']))
        {
            $errors->Add('Username is required');
        }
        else if (strtolower($_POST['username']) == 'administrator')
        {
            $errors->Add('That username already in use.  Please choose another one.');
        }
        else
        {
            $dup_id = $id ? $id : '0';
            $dup_count = $db->GetOne("SELECT COUNT(*) FROM as_users WHERE username=? AND status <> ? AND as_users_id <> ?",
                                     array($_POST['username'], UserStatus::DELETED, $dup_id));
            if ($dup_count > 0)
                $errors->Add('That username already in use.  Please choose another one.');
        }
        
        // Check account code field
        if (!$_POST['generate_account_code'])
        {
            if (!is_numeric($_POST['account_code']))
            {
                $errors->Add('Account code should be a number');
            }
            else
            {
                $dup_id = $id ? $id : '0';
                $dup_count = $db->GetOne("SELECT COUNT(*) FROM as_users WHERE account_code=? AND status <> ? AND as_users_id <> ?",
                                         array($_POST['account_code'], UserStatus::DELETED, $dup_id));
                if ($dup_count > 0)
                    $errors->Add('Account code already in use.  Please choose another one.');
            }
        }
 
        if (!Utils::is_pos_int($_POST['lockout_threshold']))
            $errors->Add('Lockout threshold is not a positive number');
        if (!Utils::is_pos_int($_POST['lockout_duration']['hours']) || !Utils::is_pos_int($_POST['lockout_duration']['minutes']))
            $errors->Add('Lockout duration fields need to be positive numbers');
        if (intval($_POST['lockout_duration']['minutes']) > 59)
            $errors->Add('Lockout duration minutes field is too large.');
        if (!Utils::is_pos_int($_POST['max_concurrent_sessions']))
            $errors->Add('Max concurrent sessions is not a positive number');
    }

    // Gather submitted information
    if ($errors->IsEmpty())
    {
        $values['first_name'] = $_POST['first_name'];
        $values['last_name'] = $_POST['last_name'];
        $values['email'] = $_POST['email'];
        $values['time_zone'] = $_POST['time_zone'];
        if (ConfigUtils::is_application_exposed(ApplicationId::RAPID_RECORD))
        {
            $values['record'] = (int)(bool) $_POST['record'];
            $values['recording_visible'] = (int)(bool) $_POST['recording_visible'];
        }
        if (isset($_POST['ar_transfer_number']))
            $values['ar_transfer_number'] = $_POST['ar_transfer_number'];
        
        if (!empty($_POST['password'])) 
            $values['password'] = $_POST['password'];
        if (!empty($_POST['pin'])) 
            $values['pin'] = $_POST['pin'];

        if ($group_admin_access || $admin_access)
        {
            if (!empty($_POST['username']))
                $values['username'] = $_POST['username'];
            $values['account_code'] = $_POST['account_code'];
            $values['status'] = $_POST['status'];            
            $values['status'] = $_POST['status'];            
            $values['lockout_threshold'] = $_POST['lockout_threshold'];
            $values['lockout_duration'] = $_POST['lockout_duration']['hours'] . ':' . $_POST['lockout_duration']['minutes'] . ':00';
            $values['max_concurrent_sessions'] = $_POST['max_concurrent_sessions'];
            $values['pin_change_required'] = $_POST['pin_change_required'];
            if (isset($_POST['external_auth_enabled']))
            {
                $values['external_auth_enabled'] = $_POST['external_auth_enabled'];
                if (isset($_POST['external_auth_dn']))
                    $values['external_auth_dn'] = $_POST['external_auth_dn'];
            }
            
            if ($_POST['generate_account_code'])
            {   
                $n = $db->GetOne("SELECT MAX(account_code)+1 FROM as_users WHERE status <> ?", UserStatus::DELETED);
                if (empty($n))
                    $n = FIRST_ACCOUNT_CODE;
                $values['pin'] = $values['account_code'] = $n;
            }
        }

        // Create User (if create page)
        if (empty($id))
        {
            $db->MakeAndExecuteInsert('as_users', $values);
            $id = $db->Insert_ID();

            if (!empty($_POST['as_user_groups_id']))
            {
                $group = new UserGroup();
                $group->SetId($_POST['as_user_groups_id']);
                $group->AddUser($id);
            }
            $response = "The user has been created. ";
            if ($_POST['generate_account_code'])
                $response .= "Please note that the generated PIN is the same as the generated account code.";
            Utils::redirect("user.php?action=edit&id=$id&s_response=" . Utils::safe_serialize($response));
        }

        // Update User (if edit page)
        if ($id > 0)
        {
            $db->MakeAndExecuteUpdate('as_users',"as_users_id = $id", $values);
            if (isset($_POST['as_user_groups_id']))
            {
                if (!empty($_POST['as_user_groups_id']))
                {
                    $group = new UserGroup();
                    $group->SetId($_POST['as_user_groups_id']);
                    $group->ForceAddUser($id);
                }
                else
                {
                    $db->Execute("DELETE FROM as_user_group_members WHERE as_users_id = ?", array($id));
                }
            }
            $response = "User account has been updated.";
        }
    }

}


// -- REMOTE AGENT ACTIONS --

if ($_POST['enable_remote_agent'])
{
    $db->Execute("INSERT INTO as_remote_agents (as_users_id, user_level) VALUES (?,?)", array($id, RemoteAgentUserLevel::USER));
    $response = "Remote Agent has been enabled for the user";
}

if ($_POST['disable_remote_agent'])
{
    $db->Execute("DELETE FROM as_remote_agents WHERE as_users_id = ?", array($id));
    $response = "Remote Agent has been deactivated for the user";
}

if ($_POST['update_remote_agent'])
{
    if (empty($_POST['ra_as_phone_devices_id']))
        $_POST['ra_as_phone_devices_id'] = NULL;
    if (empty($_POST['ra_as_external_numbers_id']))
        $_POST['ra_as_external_numbers_id'] = NULL;
    $db->Execute("UPDATE as_remote_agents SET user_level = ?, as_phone_devices_id = ?, as_external_numbers_id = ? " .
                 "WHERE as_users_id = ?",
                 array($_POST['ra_user_level'],$_POST['ra_as_phone_devices_id'],$_POST['ra_as_external_numbers_id'], $id));
    $response = "Remote Agent settings updated";
}


// -- RETRIEVE VALUES --

$tpl_vars = array('s_options'           => UserStatus::$names,
                  'ral_options'         => RemoteAgentUserLevel::$names,
                  'group_admin_access'  => $group_admin_access,
                  'admin_access'        => $admin_access,
                  'timezone_list'       => TimeZoneUtils::get_timezone_list(),
                 );

if ($admin_access)
    $tpl_vars['user_groups'] = $db->GetAll("SELECT * FROM as_user_groups");
else
{
    $admin_group_id = UserUtils::get_associated_group_id($access->GetUserId());
    $tpl_vars['user_groups'] = $db->GetAll("SELECT * FROM as_user_groups WHERE as_user_groups_id = ?", array($admin_group_id));
}

if ($id > 0)
{
    UserUtils::unlock_expired_lockouts($id);
    // Retrieve user & group data
    $user_info = $db->GetRow("SELECT * FROM as_users WHERE as_users_id = ?", array($id));
    list($hours, $minutes, $seconds) = explode(':', $user_info['lockout_duration'], 3);
    $user_info['lockout_duration'] = array('hours' => $hours, 'minutes' => $minutes);
    $tpl_vars['as_user_groups_id'] = UserUtils::get_associated_group_id($id);
    
    $full_name = $user_info['first_name'] . ' ' . $user_info['last_name'];

    // Retrieve devices
    $devices = $db->GetAll("SELECT * FROM as_phone_devices WHERE as_users_id = ?",
                           array($id));
    $tpl_vars['devices'] = $devices;

    // Retrieve external numbers
    $numbers = $db->GetAll("SELECT * FROM as_external_numbers WHERE as_users_id = ?", array($id));
    if ($ext_numbers_limit > 0)
        $tpl_vars['reached_numbers_limit'] = $ext_numbers_limit <= sizeof($numbers);
    $tpl_vars['numbers'] = $numbers;
    $tpl_vars['corporate_number_id'] = $db->GetOne("SELECT as_external_numbers_id FROM as_external_numbers WHERE as_users_id = ? AND is_corporate = 1", array($id));
    
    // Retrieve single-reach numbers
    $tpl_vars['sr_numbers'] = $db->GetAll("SELECT * FROM as_single_reach_numbers WHERE as_users_id = ?", array($id));
    
    // Retrieve remote agent data
    $ra_id = $db->GetOne("SELECT as_remote_agents_id FROM as_remote_agents WHERE as_users_id = ?", array($id));
    if ($ra_id > 0)
    {
        $ragent_data = $db->GetRow("SELECT * FROM as_remote_agents WHERE as_users_id = ?", array($id));

        $ragent_devices = $db->GetAll("SELECT * " .
                                      "FROM as_phone_devices LEFT JOIN as_directory_numbers USING (as_phone_devices_id) " .
                                      "WHERE is_primary_number = 1 AND as_users_id = ?",
                                      array($id));

        $ragent_numbers = $db->GetAll("SELECT as_external_numbers_id, name FROM as_external_numbers WHERE as_users_id = ?",
                                      array($id));

        $tpl_vars['remote_agent_enabled'] = TRUE;
        $tpl_vars['ra_user_level'] = $ragent_data['user_level'];
        $tpl_vars['ra_as_phone_devices_id'] = $ragent_data['as_phone_devices_id'];
        $tpl_vars['ra_as_external_numbers_id'] = $ragent_data['as_external_numbers_id'];
        $tpl_vars['ra_devices'] = $ragent_devices;
        $tpl_vars['ra_numbers'] = $ragent_numbers;
    }
}
else
{
    // Get default values from database
    $ld = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_LOCKOUT_DURATION);
    $d_hours = floor(intval($ld) / 60);
    $d_minutes = intval($ld) % 60;
    $tpl_vars['time_zone'] = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_TIMEZONE);
    $tpl_vars['lockout_threshold'] = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_LOCKOUT_THRESHOLD);
    $tpl_vars['lockout_duration'] = array("hours" => $d_hours, "minutes" => $d_minutes);
    $tpl_vars['max_concurrent_sessions'] = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_MAX_CONCURRENT_S);
}


// -- RENDER PAGE --

$page = new Layout();
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);

if (empty($id))
{
    $page->SetPageTitle("Create User");
    $page->SetBreadcrumbs(array('<a href="main.php">Home</a>',
                                '<a href="account_mgmt.php">Account Management</a>',
                                'Create User'));

    $page->mTemplate->assign($tpl_vars);
    $page->mTemplate->assign($_POST);
    $page->Display("create_user.tpl");
}

if ($id > 0)
{
    $title = ($user_info['status'] <> UserStatus::DELETED) ? "Edit Account for $full_name" : "Account for $full_name";
    $breadcrumbs[] = '<a href="main.php">Home</a>';
    if ($group_admin_access || $admin_access) $breadcrumbs[] = '<a href="account_mgmt.php">Account Management</a>';
    $breadcrumbs[] = htmlspecialchars($title);
    $hide_devices_from_users = ConfigUtils::get_global_config(GlobalConfigNames::HIDE_DEVICES_FROM_USERS);
    
    $page->SetPageTitle($title);
    $page->SetBreadcrumbs($breadcrumbs);

    $page->mTemplate->assign($tpl_vars);
    $page->mTemplate->assign('id', $id);
    $page->mTemplate->assign($user_info);
    $page->mTemplate->assign('hide_devices', !($group_admin_access || $admin_access) && $hide_devices_from_users);
    $page->mTemplate->assign('own_profile', $id == $access->GetUserId());
    $page->mTemplate->assign('voicemail_settings_exposed', MceConfig::SHOW_USER_VOICEMAIL_SETTINGS);
    $page->mTemplate->assign('app_ra_exposed', ConfigUtils::is_application_exposed(ApplicationId::REMOTE_AGENT));
    $page->mTemplate->assign('app_ar_exposed', ConfigUtils::is_application_exposed(ApplicationId::ACTIVE_RELAY));
    $page->mTemplate->assign('app_rr_exposed', ConfigUtils::is_application_exposed(ApplicationId::RAPID_RECORD));
    if (!$errors->IsEmpty())
        $page->mTemplate->assign($_POST);
    if ($user_info['status'] <> UserStatus::DELETED)
        $page->Display("edit_user.tpl");
    else
        $page->Display("deleted_user.tpl");
}


?>
