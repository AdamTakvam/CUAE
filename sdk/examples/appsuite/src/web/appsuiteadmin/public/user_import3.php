<?php

require_once("init.php");
require_once("lib.UserUtils.php");
require_once("class.UserGroup.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$title = "Import Users";
$finished = FALSE;

// Retrieve the import data from the session if it is not posted.
// This information has to be stored in session, because it cannot be passed
// as a GET in IE.
if (empty($_POST['s_import']))
{
    $import = unserialize($_SESSION['s_import']);
    unset($_SESSION['s_import']);
}
else
    $import = unserialize($_POST['s_import']);


// ** Drop the import and go back to account management **

if ($_POST['cancel'])
    Utils::redirect('account_mgmt.php');

// ** Add user submitted account codes

if ($_POST['remove_import'])
{
    $index = Utils::get_first_array_key($_POST['remove_import']);
    unset($import[$index]);
}

if ($_POST['add_data'])
{
    if (!empty($_POST['new_account_codes']))
    foreach ($_POST['new_account_codes'] as $index => $code)
    {
        $import[$index]['account_code'][0] = $code;
    }

    if (!empty($_POST['new_usernames']))
    foreach ($_POST['new_usernames'] as $index => $username)
    {
        $import[$index]['uid'][0] = $username;
    }
}



// ** Check import data to make sure it is ready for import **

$taken_acs = array();
$taken_usernames = array();
foreach ($import as $index => $data)
{
    $account_code = $data['account_code'][0];
    if (empty($account_code) ||
        !is_numeric($account_code) ||
        array_key_exists($account_code, $taken_acs) ||
        UserUtils::find_by_account_code($account_code) > 0)
    {
        $need_user_input[$index] = $data;
        $need_user_input[$index]['need_ac'] = TRUE;
        if (isset($_POST['new_account_codes'][$index]))
            $need_user_input[$index]['account_code'][0] = $_POST['new_account_codes'][$index];
    }
    else
        $taken_acs[$account_code] = TRUE;

    $username = $data['uid'][0];
    if (empty($username) ||
        array_key_exists($username, $taken_usernames) ||
        UserUtils::find_by_username($username) > 0)
    {
        if (empty($need_user_input[$index]))
            $need_user_input[$index] = $data;
        $need_user_input[$index]['need_username'] = TRUE;
        if (isset($_POST['new_usernames'][$index]))
            $need_user_input[$index]['uid'][0] = $_POST['new_usernames'][$index];
    }
    else
        $taken_usernames[$username] = TRUE;
}


// ** If all is okay, import the users **

if (empty($need_user_input))
{
    $ldap_server_id = $_REQUEST['as_ldap_servers_id'];

    // Get default values from database
    $ld = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_LOCKOUT_DURATION);
    $d_hours = floor(intval($ld) / 60);
    $d_minutes = intval($ld) % 60;

    $def['gmt_offset'] = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_TIMEZONE);
    $def['lockout_threshold'] = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_LOCKOUT_THRESHOLD);
    $def['lockout_duration'] = $d_hours . ':' . $d_minutes . ':00';
    $def['max_concurrent_sessions'] = ConfigUtils::get_global_config(GlobalConfigNames::DEFAULT_MAX_CONCURRENT_S);

    $db->StartTrans();
    $ids = array();
    foreach ($import as $data)
    {
        // Get a username that is not already taken based on the profile's userID attribute
        $num = 0;
        $taken = 1;
        while ($taken > 0)
        {
            $username = $data['uid'][0];
            if ($num > 0)
                $username .= "-$num";
            $taken = UserUtils::find_by_username($username);
            ++$num;
        }

        // Prepare user profile
        $vaules = array();
        $values['as_ldap_servers_id'] = $ldap_server_id;
        $values['username'] = $username;
        $values['account_code'] = $data['account_code'][0];
        if ($data['givenname'][0])
            $values['first_name'] = $data['givenname'][0];
        if ($data['sn'][0])
            $values['last_name'] = $data['sn'][0];
        if ($data['mail'][0])
            $values['email'] = $data['mail'][0];
        $values['external_auth_dn'] = $data['dn'];
        $values['external_auth_enabled'] = TRUE;
        $values['ldap_synched'] = TRUE;
        $values['status'] = UserStatus::ACTIVE;
        $values['time_zone'] = $def['gmt_offset'];
        $values['lockout_threshold'] = $def['lockout_threshold'];
        $values['lockout_duration'] = $def['lockout_duration'];
        $values['max_concurrent_sessions'] = $def['max_concurrent_sessions'];

        $db->MakeAndExecuteInsert('as_users', $values);
        $ids[] = $db->Insert_ID();
    }
    $db->CompleteTrans();

    $db->StartTrans();
    if ($_REQUEST['import_group'] > 0)
    {
        $group = new UserGroup();
        $group->SetId($_REQUEST['import_group']);
        foreach ($ids as $id)
            $group->AddUser($id);
    }
    $db->CompleteTrans();
    $finished = TRUE;
}

if ($_POST['add_data'] && !$finished)
{
    $errors->Add('The submitted data does not resolve all conflicts.  Please resolve the remaining conflicts.');
}

// ** Render page **

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>','<a href="account_mgmt.php">Account Management</a>',$title));
$page->SetErrorMessage($errors->Dump());

$page->mTemplate->assign('as_ldap_servers_id', $_REQUEST['as_ldap_servers_id']);
$page->mTemplate->assign('import_group', $_REQUEST['import_group']);
$page->mTemplate->assign('users', $need_user_input);
$page->mTemplate->assign('s_import', serialize($import));
$page->mTemplate->assign('count', sizeof($import));
$page->mTemplate->assign('finished', $finished);

$page->Display('user_import3.tpl');

?>
