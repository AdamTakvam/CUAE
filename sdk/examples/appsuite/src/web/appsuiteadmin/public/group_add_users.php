<?php

require_once("init.php");
require_once("lib.UserUtils.php");
require_once("class.UserGroup.php");


// ** Set up page **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);
$id = $_REQUEST['id'];

$db = new MceDb();
$errors = new ErrorHandler();
$group = new UserGroup();
$group->SetId($id);

if ($_POST['add'])
{
    if (!empty($_POST['user_ids']))
    {
        foreach ($_POST['user_ids'] as $user_id)
            $group->AddUser($user_id);
        $response = "Users were added to " . $group->GetName();
        $done = TRUE;
    }
    else
    {
        $errors->Add("No users were selected");
    }
}

$search = $_POST['search'];

if ($search['mode'])
{
    $clause[] = $search['field'] . " LIKE '%" . $search['text'] . "%'";
    $clause[] = "as_users_id NOT IN (SELECT as_users_id FROM as_user_group_members)";
    $clause[] = "status <> " . UserStatus::DELETED;
    $users = $db->GetAll("SELECT * FROM as_users WHERE " . implode(' AND ', $clause) . " ORDER BY last_name ASC");
    
    if (sizeof($users) == 0)
        $response = "No users were found.";
}


$page = new Layout();
$page->SetPageTitle("Add Users to " . $group->GetName());
$page->TurnOffNavigation();
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('users', $users);
$page->mTemplate->assign('search', $search);
$page->mTemplate->assign('done', $done);
$page->Display("group_add_users.tpl");

?>