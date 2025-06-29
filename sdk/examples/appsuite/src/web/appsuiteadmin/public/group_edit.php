<?php

require_once("init.php");
require_once("lib.UserUtils.php");
require_once("lib.GroupUtils.php");
require_once("class.PageLogic.php");
require_once("class.UserGroup.php");


$id = $_REQUEST['id'];

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
if (!$admin_access)
{
    $admin_id = $access->GetUserId();
    $admins_groups = GroupUtils::get_administrators_group_ids($admin_id);
    $access->DenyPageAccess(!in_array($id, $admins_groups));
}

$db = new MceDb();
$errors = new ErrorHandler();
$page_logic = new PageLogic();

$group = new UserGroup();
$group->SetId($id);


// ** Add user **

if ($_POST['add_user'])
{
    $u_id = UserUtils::find_by_username($_POST['username']);
    if ($u_id > 0)
    {
        if ($group->AddUser($u_id))
            $response = $_POST['username'] . " was added to the group.";
        else
            $errors->Add($group->GetError());
    }
    else
        $errors->Add('No user could be found by the name of ' . $_POST['username']);
}


// ** Do bulk user actions **

if (!empty($_POST['user_ids']))
{
    if ($_POST['make_admin'])
    {
        $group->SetAdministrators($_POST['user_ids']);
        $response = "Users are new group administrators";
    }
    
    if ($_POST['make_user'])
    {
        $group->UnsetAdministrators($_POST['user_ids']);
        $response = "Users are now normal group members";
    }
    
    if ($_POST['remove_users'])
    {
        $group->RemoveUsers($_POST['user_ids']);
        $response = "Users have been removed from the group";
    }
}


// ** Retrieve users and handle paging**

$page_logic->SetCurrentPageNumber($_REQUEST['p']);
$page_logic->SetItemCount($group->GetSize());
$page_logic->Calculate();

$users = $group->GetUsers($page_logic->sql_start, $page_logic->sql_limit);
foreach ($users as $index => $user)
{
    $users[$index]['level_display'] = GroupUserLevel::$names[$user['user_level']];
}


// ** Render page **

$title = "Edit Group " .$group->GetName();

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>','<a href="group_mgmt.php">Group Management</a>', htmlspecialchars($title) ));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('page_logic', $page_logic);
$page->mTemplate->assign('users', $users);
$page->mTemplate->assign('admin_id', $admin_id);
$page->mTemplate->assign('id', $id);
$page->Display('group_edit.tpl');

?>