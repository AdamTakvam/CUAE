<?php

require_once("init.php");
require_once("lib.LdapUtils.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

Utils::trim_array($_POST);

$db = new MceDb();
$errors = new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);

if ($_GET['test_ldap'])
{
    $id = $_GET['test_ldap'];
    $ldap = $db->GetRow("SELECT * FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($id));
    $conn = LdapUtils::make_connection($ldap['hostname'], $ldap['port'], $ldap['secure_connect'], $ldap['user_dn'], $ldap['password'], $errors);
    if ($errors->IsEmpty() && $conn)
        $response = "Connection and binding to the LDAP server " . $ldap['hostname'] . " tested okay.";
}

if ($_POST['apply'])
{
    ConfigUtils::set_global_config(GlobalConfigNames::LDAP_UID_ATTRIB, $_POST[GlobalConfigNames::LDAP_UID_ATTRIB]);
    ConfigUtils::set_global_config(GlobalConfigNames::LDAP_AC_ATTRIB, $_POST[GlobalConfigNames::LDAP_AC_ATTRIB]);
    $response = "Username and Account Code Attribute updated";
}

// Retrieve LDAP servers
$ldap_servers = $db->GetAll("SELECT * FROM as_ldap_servers");


$title = "LDAP Servers";
$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', '<a href="system_mgmt.php">System Management</a>', $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->Assign(GlobalConfigNames::LDAP_UID_ATTRIB, ConfigUtils::get_global_config(GlobalConfigNames::LDAP_UID_ATTRIB));
$page->mTemplate->Assign(GlobalConfigNames::LDAP_AC_ATTRIB, ConfigUtils::get_global_config(GlobalConfigNames::LDAP_AC_ATTRIB));
$page->mTemplate->assign('ldap_servers', $ldap_servers);
$page->Display("ldap.tpl");


?>