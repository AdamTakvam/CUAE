<?php

require_once("init.php");

$id = $_REQUEST['id'];
if (empty($id))
    throw new Exception('An LDAP server id is required');

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();


// ** Handle password change **

if ($_POST['update'] && !ereg('^\*+$',$_POST['password']))
{
    if ($_POST['password'] != $_POST['password_verify'])
        $errors->Add('Password could not be verified.');
        
    if ($errors->IsEmpty())
    {
        $db->Execute("UPDATE as_ldap_servers SET password = ? WHERE as_ldap_servers_id = ?", array($_POST['password'], $id));
        $response = "LDAP server password changed";
    }
}

$password = $db->GetOne("SELECT password FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($id));

// ** Render page **

$page = new Layout();
$page->SetPageTitle("Change LDAP Password");
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->TurnOffNavigation();
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('password', str_repeat('*',strlen($password)));
$page->Display('user_password.tpl');

?>