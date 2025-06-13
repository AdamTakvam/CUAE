<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$id = $_REQUEST['id'];
$db = new MceDb();
$errors = new ErrorHandler();
    
if ($id == $access->GetData('mce_users_id'))
{
    $errors->add("You cannot delete your own account.");
    Utils::redirect("users.php?s_errors=" . Utils::safe_serialize($errors));
}
else if ($id == MAIN_ADMIN_ACCOUNT_ID)
{
    $errors->add("You cannot delete the main administrator's account.");
    Utils::redirect("users.php?s_errors=" . Utils::safe_serialize($errors));	
}

$username = $db->GetOne("SELECT username FROM mce_users WHERE mce_users_id = ?", array($id));

if ($_POST['submit_yes'])
{
	$db->StartTrans();
    $db->Execute("DELETE FROM mce_users WHERE mce_users_id = ?", array($id));
    EventLog::log(LogMessageType::AUDIT, "User $username has been deleted.", LogMessageId::USER_DELETED);
    $db->CompleteTrans();
    Utils::redirect("users.php");
}

if ($_POST['submit_no'])
{
    if ($_POST['list'])
        Utils::redirect("users.php");
    else
        Utils::redirect("edit_user.php?id=$id");
}

$page = new Layout();
$page->SetPageTitle("Delete User");
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', 
                            '<a href="users.php">Users</a>', 
                            'Delete User'));
$page->mTemplate->assign(array( 'id'        => $id,
                                'username'  => $username,
                                'list'      => $_REQUEST['list']));
$page->Display('delete_user.tpl');

?>