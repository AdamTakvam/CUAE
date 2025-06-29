<?php

require_once("init.php");
require_once("lib.DeviceUtils.php");
require_once("lib.ConfigUtils.php");


// ** SET UP **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$id = $access->GetUserId();

$errors = new ErrorHandler();
$db = new MceDb();
$ext_numbers_limit = ConfigUtils::get_global_config(GlobalConfigNames::MAX_EXT_NUMBERS_PER_USER);
$response = Utils::safe_unserialize($_REQUEST['s_response']);
Utils::trim_array($_POST);


// ** HANDLE ACTIONS **

// ActiveRelay Filters
if ($_POST['whitelist'])
    Utils::redirect("../user_ar_filter_manage.php?id=$id&from=arpage&type=" . FilterNumberType::ALLOW);

if ($_POST['blacklist'])
    Utils::redirect("../user_ar_filter_manage.php?id=$id&from=arpage&type=" . FilterNumberType::BLOCK);

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
        
// Find Me Number Management
if ($_POST['add_number'])
{
    Utils::redirect("../ext_number.php?user_id=$id&from=ar");
}

if ($_POST['edit_number'])
{
    $d_id = Utils::get_first_array_key($_POST['edit_number']);
    Utils::redirect("../ext_number.php?user_id=$id&id=$d_id&from=ar");
}

if ($_POST['delete_number'])
{
    $n_id = Utils::get_first_array_key($_POST['delete_number']);
    $db->Execute("DELETE FROM as_external_numbers WHERE as_external_numbers_id = ?", array($n_id));
    $response = "Number deleted.";
}

if ($_POST['update'])
{
    $db->Execute("UPDATE as_external_numbers SET is_corporate = 0 WHERE as_users_id = ?", array($id));
    if ($_POST['corporate_number_id'] > 0)
        $db->Execute("UPDATE as_external_numbers SET is_corporate = 1, timeofday_enabled = 0, timeofday_weekend = 0 WHERE as_external_numbers_id = ?", array($_POST['corporate_number_id']));
    $db->Execute("UPDATE as_users SET ar_transfer_number = ? WHERE as_users_id = ?", array($_POST['ar_transfer_number'],$id));
    $response = "ActiveRelay settings updated";
}

    

// ** RETRIEVE DATA **

$numbers = $db->GetAll("SELECT * FROM as_external_numbers WHERE as_users_id = ?", array($id));
if ($ext_numbers_limit > 0)
    $tpl_vars['reached_numbers_limit'] = $ext_numbers_limit <= sizeof($numbers);
$tpl_vars['numbers'] = $numbers;
$tpl_vars['corporate_number_id'] = $db->GetOne("SELECT as_external_numbers_id FROM as_external_numbers WHERE as_users_id = ? AND is_corporate = 1", array($id));
$tpl_vars['ar_transfer_number'] = $db->GetOne("SELECT ar_transfer_number FROM as_users WHERE as_users_id = ?", array($id));

// Retrieve single-reach numbers
$tpl_vars['sr_numbers'] = $db->GetAll("SELECT * FROM as_single_reach_numbers WHERE as_users_id = ?", array($id));


// ** RENDER PAGE **

$title = "ActiveRelay";
$breadcrumbs[] = '<a href="/appsuiteadmin/main.php">Home</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetResponseMessage($response);

$page->mTemplate->assign($tpl_vars);
if (!$errors->IsEmpty())
    $page->mTemplate->assign($_POST);
$page->Display("apps_active_relay.tpl");
        
?>