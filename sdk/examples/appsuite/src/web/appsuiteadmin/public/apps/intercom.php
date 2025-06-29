<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$user_id = $access->GetUserId();
$user_data = UserUtils::get_user_data($user_id);

$db = new MceDb();
$errors = new ErrorHandler();

if ($_POST['leave'])
{
    $g_id = Utils::get_first_array_key($_POST['leave']);
    $db->Execute("DELETE FROM as_intercom_group_members WHERE as_users_id = ? AND as_intercom_groups_id = ?",
                 array($user_id, $g_id));
    $response = "You have left the group.";
}

if ($_POST['join'])
{
    if ($_POST['join_group_id'] > 0)
    {
        $db->Execute("INSERT INTO as_intercom_group_members (as_users_id, as_intercom_groups_id) VALUES (?,?)",
                     array($user_id, $_POST['join_group_id']));
        $response = "You are now a member of the group.";
    }
    else
    {
        $errors->Add("Please select a group to join.");
    }
}


$groups = $db->GetAll("SELECT * FROM as_intercom_group_members LEFT JOIN as_intercom_groups USING (as_intercom_groups_id) " .
                      "WHERE as_users_id = ?",
                      array($user_id));

$publics = $db->GetAll("SELECT * FROM as_intercom_groups " .
                       "WHERE as_intercom_groups_id NOT " .
                       "IN (SELECT as_intercom_groups_id FROM as_intercom_group_members WHERE as_users_id = ?) " .
                       "AND is_private = 0",
                       array($user_id));

// -- RENDER PAGE --

$title = "Intercom Group Memberships";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                            $title));
$page->SetResponseMessage($response);
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('groups', $groups);
$page->mTemplate->assign('publics', $publics);
$page->Display("apps_intercom.tpl");

?>