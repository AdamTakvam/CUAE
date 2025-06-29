<?php

require_once("init.php");
require_once("lib.UserUtils.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);

$db = new MceDb();

$id = $_REQUEST['id'];

if ($id <= 0)
    throw new Exception('No user id is defined');

if ($_POST['confirm_no'])
    Utils::redirect("user.php?id=$id");

if ($_POST['confirm_yes'])
{
    UserUtils::delete_user($id);
    Utils::redirect("account_mgmt.php");
}

$user = UserUtils::get_user_data($id);


// ** Display the page **

$title = "Delete User " . $user['username'];
$breadcrumbs[] = '<a href="main.php">Home</a>';
$breadcrumbs[] = '<a href="account_mgmt.php">Account Management</a>';
$breadcrumbs[] = htmlspecialchars($title);

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->mTemplate->assign('id',$id);
$page->mTemplate->assign('user',$user);
$page->Display('user_confirm_delete.tpl');

?>