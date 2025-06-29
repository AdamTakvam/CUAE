<?php

require_once("remoteagent_init.php");
require_once("lib.DeviceUtils.php");

$access = new RemoteAgentAccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$id = $access->GetUserId();

$errors = new ErrorHandler();
$db = new MceDb();

Utils::trim_array($_POST);

if ($_POST['cancel'])
{
    Utils::redirect('account_mgmt.php');
}


// -- HANDLE ACTIONS --

// Device management
if ($_POST['add_device'])
{
    Utils::redirect("device.php");
}

if ($_POST['edit_device'])
{
    $d_id = Utils::get_first_array_key($_POST['edit_device']);
    Utils::redirect("device.php?id=$d_id");
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


// External number management
if ($_POST['add_number'])
{
    Utils::redirect("ext_number.php");
}

if ($_POST['edit_number'])
{
    $d_id = Utils::get_first_array_key($_POST['edit_number']);
    Utils::redirect("ext_number.php?id=$d_id");
}

if ($_POST['delete_number'])
{
    $n_id = Utils::get_first_array_key($_POST['delete_number']);
    $db->Execute("DELETE FROM as_external_numbers WHERE as_external_numbers_id = ?", array($n_id));
    $response = "Number deleted.";
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

$tpl_vars = array('s_options' => UserStatus::$names,
				  'ral_options' => RemoteAgentUserLevel::$names,
                  'admin_access' => $admin_access);


// Retrieve user
$user_info = $db->GetRow("SELECT * FROM as_users WHERE as_users_id = ?", array($id));
$full_name = $user_info['first_name'] . ' ' . $user_info['last_name'];

// Retrieve devices
$devices = $db->GetAll("SELECT * FROM as_phone_devices WHERE as_users_id = ?",
                       array($id));                          
$tpl_vars['devices'] = $devices;

// Retrieve external numbers
$numbers = $db->GetAll("SELECT * FROM as_external_numbers WHERE as_users_id = ?", array($id));
$tpl_vars['numbers'] = $numbers;

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



// -- RENDER PAGE --

$page = new RemoteAgentLayout();
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);

$title = "Remote Agent for $full_name";

$page->SetPageTitle($title);
$page->SetBreadcrumbs(array($title));

$page->mTemplate->assign($tpl_vars);
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign($user_info);
if (!$errors->IsEmpty())
{
    $page->mTemplate->assign($_POST);
}
$page->Display("remoteagent_edit_user.tpl");


?>