<?php

require_once("init.php");

$id = $_REQUEST['id'];
if (empty($id))
    throw new Exception('A user id is required');

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL, $id);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
if ($id > 0 && $group_admin_access)
    $group_admin_access = $access->CheckPageAccess(AccessControl::GROUP_ADMIN, $id);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();

$password = $db->GetOne("SELECT pin FROM as_users WHERE as_users_id = ?", array($id));


// ** Handle password change **

if ($_POST['update'])
{
    if (empty($_POST['pin']))
        $errors->Add('PIN cannot be blank.');
    if (!is_numeric($_POST['pin']))
        $errors->Add('PIN can only contain numbers.');
    if ($_POST['pin'] != $_POST['pin_verify'])
        $errors->Add('PIN could not be verified.');
        
    if ($errors->IsEmpty())
    {
        $db->Execute("UPDATE as_users SET pin = ? WHERE as_users_id = ?", array($_POST['pin'], $id));
        $response = "User PIN changed";
    }
}


// ** Render page **

$page = new Layout();
$page->SetPageTitle("Change User PIN");
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->TurnOffNavigation();
$page->mTemplate->assign('id', $id);
$page->Display('user_pin.tpl');

?>