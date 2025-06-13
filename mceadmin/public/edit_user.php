<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();

$id = $_REQUEST['id'];
$user_id = $access->GetData('mce_users_id');
$user_data = $db->GetRow("SELECT * FROM mce_users WHERE mce_users_id = ?", array($id));
$can_modify = ($user_id == MAIN_ADMIN_ACCOUNT_ID) || ($id != MAIN_ADMIN_ACCOUNT_ID);
if (empty($user_data))
    { Utils::redirect("users.php"); }

Utils::trim_array($_POST);


// Handle user requests

if ($_POST['done'])
    Utils::redirect("users.php");

if ($_POST['delete'])
{
    if ($id == MAIN_ADMIN_ACCOUNT_ID)
        $errors->Add("The administrator account cannot be deleted.");
    else
        Utils::redirect("delete_user.php?id=$id");
}

if ($_POST['update'] && $can_modify)
{
    $db->StartTrans();
    // Verify password if the user wants to change it
    if (trim($_POST['password']) || trim($_POST['password2']))
    {
        if ($_POST['password'] == $_POST['password2'])
        {
            $db->Execute("UPDATE mce_users SET password = ? WHERE mce_users_id = ?",
                         array(Utils::encrypt_password($_POST['password']), $id));
        } else {
            $errors->Add("Passwords entered are not the same.  Try again.");
        }
    }

    if ($errors->IsEmpty())
    {

        // Update access level
        if ($_POST['access_level'] > 0 && $_POST['current_access_level'] != $_POST['access_level'])
        {
            $db->Execute("UPDATE mce_users SET access_level = ? WHERE mce_users_id = ?",
                         array($_POST['access_level'], $id));
        }

        // Done
        EventLog::log(LogMessageType::AUDIT,
                      "User $user_data[username] has been modified",
                      LogMessageId::USER_MODIFIED,
                      print_r(Utils::extract_array_using_keys(array('id','access_level'),$_POST),TRUE));
        $user_data = $db->GetRow("SELECT * FROM mce_users WHERE mce_users_id = ?", array($id));
        $response = "User settings updated.";
    }
    $db->CompleteTrans();
}


// -- RENDER PAGE --

$accesslevels[AccessControl::NORMAL] = "Normal User";
// $accesslevels[AccessControl::RESTRICTED] = "Restricted User";
$accesslevels[AccessControl::ADMINISTRATOR] = "Administrator";

$template_vars = array( 'accesslevels'  => $accesslevels,
                        'id'            => $id,
                        'user_id'       => $user_id,
                        'can_modify'    => $can_modify);

$page = new Layout();
$page->SetPageTitle("Edit User");
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', '<a href="users.php">Users</a>', 'Edit User'));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($template_vars);
$page->mTemplate->assign($user_data);
$page->Display("edit_user.tpl");

?>