<?php

require_once("init.php");
require_once("lib.DeviceUtils.php");

$user_id = $_REQUEST['user_id'];
$id = $_REQUEST['id'];

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL, $user_id);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
if ($id > 0 && $group_admin_access)
    $group_admin_access = $access->CheckPageAccess(AccessControl::GROUP_ADMIN, $user_id);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();

Utils::trim_array($_POST);

if ($_POST['cancel'])
{
    Utils::redirect("user.php?id=$user_id");
}


// HANDLE DEVICE ACTIONS

if ($_POST['delete_device'])
{
    $db->Execute("DELETE FROM as_directory_numbers WHERE as_phone_devices_id = ?", array($id));
    $db->Execute("DELETE FROM as_phone_devices WHERE as_phone_devices_id = ?", array($id));
    DeviceUtils::auto_set_primary_device($user_id);
    Utils::redirect("user.php?id=$user_id");
}

if ($_POST['submit'])
{
    if (!empty($_POST['is_ip_phone']))
    {
        if (empty($_POST['mac_address'])) 
            $errors->Add('MAC Address Required');
        // Verify format and manipulate the MAC Address if necessary
        if (!eregi("^(SEP)?[a-f0-9]+$",$_POST['mac_address'],$regs))
        {
            $errors->Add('The device name needs to be a hexadecimal number optionally prefixed with "SEP"');
        }
        else
        {
            if (empty($regs[1])) 
                $_POST['mac_address'] = 'SEP' . $_POST['mac_address'];
        }
    }
    else
    {
        $_POST['is_ip_phone'] = 0;
        $_POST['mac_address'] = NULL;
    }
        
    $q_id = $id ? $id : 0;
    $found = $db->GetOne("SELECT COUNT(*) FROM as_phone_devices WHERE mac_address = ? AND as_phone_devices_id <> ?", array($_POST['mac_address'], $q_id));
    if ($found > 0)
        $errors->Add("A device already exists with that device name.");

    if ($errors->IsEmpty())
    {

        // Create Device
        if (empty($id))
        {
            $db->Execute("INSERT INTO as_phone_devices (as_users_id, name, is_ip_phone, mac_address) " .
                         "VALUES (?,?,?,?)",
                         array($user_id, $_POST['name'], $_POST['is_ip_phone'], $_POST['mac_address']));
            $id = $db->Insert_ID();
            DeviceUtils::auto_set_primary_device($user_id);
            Utils::redirect("device.php?id=$id&user_id=$user_id");
        }

        // Update Device
        if ($id > 0)
        {
            $db->Execute("UPDATE as_phone_devices SET name = ?, is_ip_phone = ?, mac_address = ? WHERE as_phone_devices_id = ?",
                         array($_POST['name'], $_POST['is_ip_phone'], $_POST['mac_address'], $id));
            $response = "Device updated.";
        }

    }

}


// HANDLE NUMBER ACTIONS

if ($_POST['delete_number'])
{
    $n_id = Utils::get_first_array_key($_POST['delete_number']);
    $db->Execute("DELETE FROM as_directory_numbers WHERE as_directory_numbers_id = ?", array($n_id));
    DeviceUtils::auto_set_primary_number($id);
    $response = "Line number has been deleted from the device.";
}

if ($_POST['add_number'])
{
    if (empty($_POST['number']))
    {
        $errors->Add("No line number was entered");
    }
    else
    {
        $found = $db->GetOne("SELECT COUNT(*) FROM as_directory_numbers WHERE directory_number = ?", array($_POST['number']));
        if (empty($found))
        {
            $db->Execute("INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number) VALUES (?,?)",
                         array($id, $_POST['number']));
            DeviceUtils::auto_set_primary_number($id);
            $response = "Line number " . $_POST['number'] . " has been added to this device.";
        }
        else
        {
            $errors->Add("That line number is already assigned to a device.");
        }
    }
}

if ($_POST['set_primary_number'])
{
     $n_id = Utils::get_first_array_key($_POST['set_primary_number']);
     $db->Execute("UPDATE as_directory_numbers SET is_primary_number = 0 WHERE as_phone_devices_id = ?", array($id));
     $db->Execute("UPDATE as_directory_numbers SET is_primary_number = 1 WHERE as_directory_numbers_id = ?", array($n_id));
     $response = "Primary number set";
}


// RETRIEVE VALUES

$full_name = $db->GetOne("SELECT CONCAT(first_name, ' ', last_name) FROM as_users WHERE as_users_id = ?", array($user_id));

$tpl_vars = array('id'          => $id,
                  'user_id'     => $user_id);

if ($id > 0)
{
    $device = $db->GetRow("SELECT * FROM as_phone_devices WHERE as_phone_devices_id = ?", array($id));
    $numbers = $db->GetAll("SELECT * FROM as_directory_numbers WHERE as_phone_devices_id = ?", array($id));
    $title = "Edit Device ";
}
else
{
    $title = "Add Device";
}



// -- PAGE RENDER --

$breadcrumbs[] = '<a href="main.php">Home</a>';
if ($admin_access) $breadcrumbs[] = '<a href="account_mgmt.php">Account Management</a>';
$breadcrumbs[] = '<a href="user.php?id=' . $user_id . '">Account for ' . htmlspecialchars($full_name) . '</a>';
$breadcrumbs[] = htmlspecialchars($title);

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($tpl_vars);
if ($_POST['submit'] && !$errors->IsEmpty())
{
    $page->mTemplate->assign($_POST);
}
else
{
    $page->mTemplate->assign($device);
}
$page->mTemplate->assign('numbers', $numbers);
$page->Display("device.tpl");

?>