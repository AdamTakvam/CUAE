<?php

require_once("init.php");
require_once("lib.LdapUtils.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

Utils::trim_array($_POST);

$id = $_REQUEST['id'];
if (empty($id))
    Utils::redirect('ldap.php');

$db = new MceDb();
$errors = new ErrorHandler();
$title = "Edit LDAP Server";


// ** Handle actions **

if ($_POST['goback'])
    Utils::redirect('ldap.php');

if ($_POST['submit'] || $_POST['test'])
{
    // Validate submissions
    $changes = Utils::extract_array_using_keys(array('hostname','port','secure_connect','base_dn','user_dn','password','password_verify'), $_POST);
    if (empty($changes['hostname']))
        $errors->Add('Hostname is a required field');
    if (empty($changes['port']) || !is_numeric($changes['port']))
        $errors->Add('Port field is not a valid number');    
}

if ($_POST['test'] && $errors->IsEmpty())
{
    $password = $db->GetOne('SELECT password FROM as_ldap_servers WHERE as_ldap_servers_id = ?', array($id));
    $conn = LdapUtils::make_connection($changes['hostname'], $changes['port'], $changes['secure_connect'], $changes['user_dn'], $password, $errors);
    if ($errors->IsEmpty() && $conn)
        $response = "Connection and binding to the LDAP server tested okay.";
}

if ($_POST['submit'] && $errors->IsEmpty())
{
    // Update the server entry
    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        $db->Execute("UPDATE as_ldap_servers SET hostname = ?, port = ?, secure_connect = ?, base_dn = ?, user_dn = ? WHERE as_ldap_servers_id = ?",
                     array($changes['hostname'], $changes['port'], $changes['secure_connect'], $changes['base_dn'], 
                           $changes['user_dn'], $id));
        $db->CompleteTrans();
        $response = "LDAP server settings updated";
    }
}
    

// ** Render page **

if (!$_POST['test'] && $errors->IsEmpty())
    $server = $db->GetRow("SELECT * FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($id));
else
    $server = $changes;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>','<a href="system_mgmt.php">System Management</a>','<a href="ldap.php">LDAP Servers</a>',$title));
$page->SetResponseMessage($response);
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('mode','edit');
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('server', $server);
$page->Display('ldap_form.tpl');

?>