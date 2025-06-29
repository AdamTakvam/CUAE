<?php

require_once("init.php");
require_once("lib.UserUtils.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$id = $_REQUEST['id'];

$db = new MceDb();
$errors = new ErrorHandler();
$title = "Remove LDAP Server";


function users_assoc_with_ldap()
{
    global $db, $id;
    $count = $db->GetOne("SELECT COUNT(as_users_id) FROM as_users WHERE as_ldap_servers_id = ? AND status <> ?", array($id, UserStatus::DELETED));
    return $count;
}


// ** Handle user reponses **

if ($_POST['cancel'])
{
    Utils::redirect("ldap.php");
}


if ($_POST['delete_all'])
{
    $user_ids = $db->GetCol("SELECT as_users_id FROM as_users WHERE as_ldap_servers_id = ?", array($id));
    $db->StartTrans();
    foreach ($user_ids as $user_id)
    {
        UserUtils::delete_user($user_id);
    }
}

if ($_POST['change'] && $_POST['new_ldap_assoc_id'])
{
    $db->StartTrans();
    $db->Execute("UPDATE as_users SET as_ldap_servers_id = ? WHERE as_ldap_servers_id = ?", array($_POST['new_ldap_assoc_id'], $id));
}

$user_count = users_assoc_with_ldap();
if ($user_count == 0)
{
    $db->Execute("DELETE FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($id));
    $db->CompleteTrans();
    Utils::redirect('ldap.php?s_response=' . Utils::safe_serialize('LDAP server has been removed'));
}


// ** Render page **

$hostname = $db->GetOne("SELECT hostname FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($id));
$servers = $db->GetAll("SELECT * FROM as_ldap_servers WHERE as_ldap_servers_id <> ?", array($id));

$page = new Layout();
$page->SetPageTitle($title);
$page->TurnOffNavigation();
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('user_count', $user_count);
$page->mTemplate->assign('hostname', $hostname);
$page->mTemplate->assign('servers', $servers);
$page->Display('ldap_remove.tpl');

?>