<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
$user_id = $access->GetUserId();

$db = new MceDb();
$errors = new ErrorHandler();
$title = "Group Management";


// ** Perform user requested actions **

if ($_POST['create_group'])
{
    $group_name = trim($_POST['group_name']);
    if (!empty($group_name))
    {
        $g_exists = $db->GetOne("SELECT as_user_groups_id FROM as_user_groups WHERE name = ?", array($group_name));
        if (empty($g_exists))
        {
            $db->Execute("INSERT INTO as_user_groups SET name = ?", array($group_name));
            $response = "Group '$group_name' created";
        }
        else
            $errors->Add("Group '$group_name' already exists");
    }
}


// ** Retrieve the groups appropiate to this administrator **

if ($user_id > 0)
{
    $group_ids = GroupUtils::get_administrators_group_ids($user_id);
    $groups = $db->GetAll("SELECT * FROM as_user_groups WHERE as_user_groups_id IN (" . implode(',', $group_ids) . ") ORDER BY name ASC");
}
else
{
    $groups = $db->GetAll("SELECT * FROM as_user_groups ORDER BY name ASC");
}


// ** Render the page **

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', $title));
$page->SetResponseMessage($response);
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('admin_access', $admin_access);
$page->mTemplate->assign('groups', $groups);
$page->Display('group_mgmt.tpl');

?>