<?php

require_once("init.php");
require_once("lib.LdapUtils.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

Utils::trim_array($_POST);

$db = new MceDb();
$errors = new ErrorHandler();
$title = "Add LDAP Server";


// ** Handle actions **

if ($_POST['goback'])
    Utils::redirect('ldap.php');
    
if ($_POST['submit'] || $_POST['test'])
{
    // Validate submissions
    $server = Utils::extract_array_using_keys(array('hostname','port','secure_connect','base_dn','user_dn','password','password_verify'), $_POST);
    if (empty($server['hostname']))
        $errors->Add('Hostname is a required field');
    if (empty($server['port']) || !is_numeric($server['port']))
        $errors->Add('Port field is not a valid number');
    if (!empty($server['password']))
        if ($server['password'] <> $server['password_verify'])
            $errors->Add('The new password could not be verified');
    if (empty($server['secure_connect']))
        $server['secure_connect'] = 0;
}

if ($_POST['test'] && $errors->IsEmpty())
{
    $conn = LdapUtils::make_connection($server['hostname'], $server['port'], $server['secure_connect'], $server['user_dn'], $server['password'], $errors);
    if ($errors->IsEmpty() && $conn)
        $response = "Connection and binding to the LDAP server tested okay.";
}

if ($_POST['submit'] && $errors->IsEmpty())
{
    // Insert the server entry
    if ($errors->IsEmpty())
    {
        $db->Execute("INSERT INTO as_ldap_servers SET hostname = ?, port = ?, secure_connect = ?, base_dn = ?, user_dn = ?, password = ?",
                     array($server['hostname'], $server['port'], $server['secure_connect'], $server['base_dn'], 
                           $server['user_dn'], $server['password']));
        Utils::redirect('ldap.php?s_response=' . Utils::safe_serialize('LDAP server added'));
        exit();
    }
}


// ** Render page **

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>','<a href="system_mgmt.php">System Management</a>','<a href="ldap.php">LDAP Servers</a>',$title));
$page->SetResponseMessage($response);
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('mode','add');
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('server', $server);
$page->Display('ldap_form.tpl');

?>