<?php

require_once("init.php");
require_once("class.UserGroup.php");


$id = $_REQUEST['id'];

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN, $id);

$db = new MceDb();

$group = new UserGroup();
$group->SetId($id);

$title = "Confirm Delete Group";


// ** Act on confirmed or cancelled delete

if ($_POST['delete_cancel'])
    Utils::redirect("group_mgmt.php");

if ($_POST['delete_confirm'])
{
    if ($_POST['my_action'] == "move" && $_POST['new_group'])
    {
        $db->Execute("UPDATE as_user_group_members SET as_user_groups_id = ? WHERE as_user_groups_id = ?", array($_POST['new_group'], $id));
    }
    $group->Delete();
    Utils::redirect("group_mgmt.php");
}

$user_count = $db->GetOne("SELECT COUNT(*) FROM as_user_group_members WHERE as_user_groups_id = ?", array($id)); 

$other_groups = $db->GetAll("SELECT * FROM as_user_groups WHERE as_user_groups_id <> ?", array($id));

// ** Render page **

$page = new Layout();
$page->SetPageTitle($title);
$page->TurnOffNavigation();
$page->mTemplate->assign('name',$group->GetName());
$page->mTemplate->assign('user_count', $user_count);
$page->mTemplate->assign('other_groups', $other_groups);
$page->mTemplate->assign('id',$id);
$page->Display('group_delete.tpl');

?>