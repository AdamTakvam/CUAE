<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.ConfigUtils.php");


function redirect_back()
{
    global $from, $user_id;
    if ($from == 'ar')
        Utils::redirect("/appsuiteadmin/apps/active_relay.php");
    else
        Utils::redirect("user.php?id=$user_id");
}


// ** SET UP **

$user_id = $_REQUEST['user_id'];
$id = $_REQUEST['id'];
$from = $_REQUEST['from'];

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL, $user_id);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
if ($id > 0 && $group_admin_access)
    $group_admin_access = $access->CheckPageAccess(AccessControl::GROUP_ADMIN, $user_id);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();
$ext_numbers_limit = ConfigUtils::get_global_config(GlobalConfigNames::MAX_EXT_NUMBERS_PER_USER);
$ext_numbers_count = $db->GetOne("SELECT COUNT(*) FROM as_external_numbers WHERE as_users_id = ?", array($user_id));

if ( ($ext_numbers_limit > 0) && ($ext_numbers_count >= $ext_numbers_limit) && empty($id))
{
    redirect_back();
}

if (!$admin_access)
{
    $user_id = $access->GetUserId();
}

Utils::trim_array($_POST);


// ** HANDLE ACTIONS **

if ($_POST['cancel'])
{
    redirect_back();
}

if ($_POST['submit'])
{
    $submitted = $_POST;
    $stored_vrs = unserialize(ConfigUtils::get_global_config(GlobalConfigNames::FIND_ME_VALIDATE_REGEXS));
    $stored_brs = unserialize(ConfigUtils::get_global_config(GlobalConfigNames::FIND_ME_BLACKLIST_REGEXS));
    $valid_regex = !empty($stored_vrs) ? implode('|',$stored_vrs) : ".+";
    $blacklist_regex = !empty($stored_brs) ? implode('|',$stored_brs) : "~(.+)";
    
    if (empty($submitted['phone_number']))
        $errors->Add('Phone Number Required');
    if (!eregi($valid_regex,$submitted['phone_number']))
        $errors->Add('Phone Number does not validate');
    if (eregi($blacklist_regex,$submitted['phone_number']))
        $errors->Add('This phone number is blacklisted by the administrator.  Try another number.');
    if (empty($submitted['delay_call_time']))
        $submitted['delay_call_time'] = 0;
    if (empty($submitted['call_attempt_timeout']))
        $submitted['call_attempt_timeout'] = 0;        
    $submitted['ar_enabled'] = (string)(bool) $submitted['ar_enabled'];

    if ($submitted['timeofday_enabled'])
        $submitted['timeofday_weekend'] = $submitted['sat_enabled'] + $submitted['sun_enabled'];
    else
    {
        $submitted['timeofday_enabled'] = 0;
        $submitted['timeofday_weekend'] = 0;       
    }
    
    $submitted['timeofday_start'] = strtotime($submitted['td_start']['Hour'] . ":" . $submitted['td_start']['Minute'] . " " . $submitted['td_start']['Meridian']);
    $submitted['timeofday_end'] = strtotime($submitted['td_end']['Hour'] . ":" . $submitted['td_end']['Minute'] . " " . $submitted['td_end']['Meridian']);

    
    if ($errors->IsEmpty())
    {
        $vars['name']                   = $submitted['name'];
        $vars['phone_number']           = $submitted['phone_number'];
        $vars['delay_call_time']        = $submitted['delay_call_time'];
        $vars['call_attempt_timeout']   = $submitted['call_attempt_timeout'];
        $vars['ar_enabled']             = $submitted['ar_enabled'];
        $vars['timeofday_enabled']      = $submitted['timeofday_enabled'];
        $vars['timeofday_weekend']      = $submitted['timeofday_weekend'];
        $vars['timeofday_start']        = date("H:i:s", $submitted['timeofday_start']);
        $vars['timeofday_end']          = date("H:i:s", $submitted['timeofday_end']);
        
        // Create Number
        if (empty($id))
        {
            $vars['as_users_id'] = $user_id;
            $db->MakeAndExecuteInsert('as_external_numbers', $vars);
            redirect_back();
        }

        // Update Number
        if ($id > 0)
        {
            $db->MakeAndExecuteUpdate('as_external_numbers', "as_external_numbers_id = $id", $vars);
            $response = "External number updated.";
        }

    }

}


// RETRIEVE VALUES

$full_name = $db->GetOne("SELECT CONCAT(first_name, ' ', last_name) FROM as_users WHERE as_users_id = ?", array($user_id));

$tpl_vars = array('id'                  => $id,
                  'user_id'             => $user_id,
                  'from'                => $from,
                  'number_description'  => ConfigUtils::get_global_config(GlobalConfigNames::FIND_ME_DESCRIPTION),
                  'ar_exposed'          => ConfigUtils::is_application_exposed(ApplicationId::ACTIVE_RELAY),
                  );

if ($id > 0)
{
    $number = $db->GetRow("SELECT * FROM as_external_numbers WHERE as_external_numbers_id = ?",
                          array($id));
    if ($number['timeofday_start'] == "00:00:00")
        $number['timeofday_start'] = "08:00:00";
    if ($number['timeofday_end'] == "00:00:00")
        $number['timeofday_end'] = "17:00:00";
    $title = "Edit Number ";
}
else
{
    $title = "Add Number";
}


// -- PAGE RENDER --

$breadcrumbs[] = '<a href="main.php">Home</a>';
if ($from == 'ar')
{
    $breadcrumbs[] = '<a href="./apps/active_relay.php">ActiveRelay</a>';
}
else
{
    if ($admin_access) $breadcrumbs[] = '<a href="account_mgmt.php">Account Management</a>';
    $breadcrumbs[] = '<a href="user.php?id=' . $user_id . '">Account for ' . htmlspecialchars($full_name) . '</a>';
}
$breadcrumbs[] = htmlspecialchars($title);

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($tpl_vars);

if (!$errors->IsEmpty())
    $page->mTemplate->assign($submitted);
else if ($id > 0)
    $page->mTemplate->assign($number);
else
{
    $page->mTemplate->assign('timeofday_start', "08:00:00");
    $page->mTemplate->assign('timeofday_end', "17:00:00");
}
    
$page->Display("ext_number.tpl");

?>