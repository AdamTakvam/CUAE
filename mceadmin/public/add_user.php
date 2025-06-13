<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();

Utils::trim_array($_POST);

if ($_POST['done'])
    Utils::redirect("users.php");

if ($_POST['add'])
{
    $db = new MceDb();

    if (Utils::is_blank($_POST['username']))
        $errors->Add("Please enter a username");
    else if (!eregi("^[a-z][a-z0-9@_/.-]*$",$_POST['username']))
        $errors->Add("The username is not valid");
    else
    {
        $user_id = $db->GetOne("SELECT mce_users_id FROM mce_users WHERE username=?",array($_POST['username']));
        if ($user_id > 0)
            $errors->Add("User already exists with this username");
    }


    if (Utils::is_blank($_POST['password']))
        $errors->Add("Please enter a password for the user");

    if ($_POST['password'] != $_POST['password2'])
        $errors->Add("The entered passwords do not match.  Please try again.");

    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        $db->Execute("INSERT INTO mce_users (username, password, creator_mce_users_id, created_timestamp, updated_timestamp, access_level) " .
                     "VALUES (?, ?, ?, NOW(), NOW(), ?)",
                     array($_POST['username'], Utils::encrypt_password($_POST['password']), $access->GetData('mce_users_id'), $_POST['access_level']));
        $response = "User $_POST[username] has been added.";
        $user_data = Utils::extract_array_using_keys(array('username','access_level'), $_POST);
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::USER_ADDED, print_r($user_data, TRUE));
        $db->CompleteTrans();
    }
}

$page = new Layout();
$page->SetPageTitle("Add User");
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', '<a href="users.php">Users</a>', 'Add User'));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);

$accesslevels[AccessControl::NORMAL] = "Normal User";
// $accesslevels[AccessControl::RESTRICTED] = "Restricted User";
$accesslevels[AccessControl::ADMINISTRATOR] = "Administrator";

$template_vars = array('accesslevels' => $accesslevels);

if (!$errors->IsEmpty()) { $page->mTemplate->assign($_POST); }
$page->mTemplate->assign($template_vars);
$page->Display("add_user.tpl");

?>