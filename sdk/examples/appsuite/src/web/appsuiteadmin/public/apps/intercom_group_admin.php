<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);

$db = new MceDb();
$errors = new ErrorHandler();
$id = $_REQUEST['id'];
Utils::trim_array($_POST);

if ($_POST['cancel'])
{
    Utils::redirect('intercom_admin.php');
}

if ($_POST['delete'])
{
    $db->Execute("DELETE FROM as_intercom_group_members WHERE as_intercom_groups_id = ?", array($id));
    $db->Execute("DELETE FROM as_intercom_groups WHERE as_intercom_groups_id = ?", array($id));
    Utils::redirect('intercom_admin.php');
}

if ($_POST['delete_member'])
{
    $d_id = Utils::get_first_array_key($_POST['delete_member']);
    $db->Execute("DELETE FROM as_intercom_group_members " .
                 "WHERE as_intercom_groups_id = ? AND as_users_id = ?",
                 array($id, $d_id));
    $response = "Member removed.";
}

if ($_POST['add_user_submit'])
{
    $user_id = UserUtils::find_by_username($_POST['add_user']);
    if ($user_id > 0)
    {
        $count = $db->GetOne("SELECT COUNT(*) FROM as_intercom_group_members " .
                             "WHERE as_intercom_groups_id = ? AND as_users_id = ?",
                             array($id, $user_id));
        if ($count > 0)
        {
            $errors->Add("That user is already part of this group.");
        }
        else
        {
            $db->Execute("INSERT INTO as_intercom_group_members SET as_intercom_groups_id = ?, as_users_id = ?",
                         array($id, $user_id));
            $response = "User $_POST[add_user] added to group.";
        }
    }
    else
    {
        $errors->Add("No user was found by the username.");
    }
}

if ($_POST['submit'])
{
    // Validate submitted values
    if (empty($_POST['name']))
    {
        $errors->Add("Group needs to be given a name.");
    }
    else
    {
        $dup_id = $id ? $id : 0;
        $cnt = $db->GetOne("SELECT COUNT(*) FROM as_intercom_groups " .
                           "WHERE name = ? AND as_intercom_groups_id <> ?", array($_POST['name'], $dup_id));
        if ($cnt > 0)
            $errors->Add("Group name is already taken.  Please choose another one.");
    }
    
    // Collect values
    $values[] = $_POST['name'];
    $values[] = (int)(bool) $_POST['is_enabled'];
    $values[] = (int)(bool) $_POST['is_talkback_enabled'];    
    $values[] = (int)(bool) $_POST['is_private'];  
      
    if ($errors->IsEmpty())
    {
        if (empty($id))
        {
            $db->Execute("INSERT INTO as_intercom_groups (name, is_enabled, is_talkback_enabled, is_private) " .
                         "VALUES (?,?,?,?)",
                         $values);
            $id = $db->Insert_ID();
            Utils::redirect("intercom_group_admin.php?id=$id");
        }
        else
        {
            $values[] = $id;
            $db->Execute("UPDATE as_intercom_groups " .
                         "SET name = ?, is_enabled = ?, is_talkback_enabled = ?, is_private = ? " .
                         "WHERE as_intercom_groups_id = ?",
                         $values);
            $response = "Group configuration updated.";
        }
    
    }
}

if (!empty($id))
{
    $group = $db->GetRow("SELECT * FROM as_intercom_groups WHERE as_intercom_groups_id = ?", array($id));
    $members = $db->GetAll("SELECT * FROM as_intercom_group_members LEFT JOIN as_users USING (as_users_id) " .
                         "WHERE as_intercom_groups_id = ? " .
                         "ORDER BY username ASC",
                         array($id));
}


// -- PAGE RENDERING --

if (empty($id))
    $title = "Create Intercom Group";
else
    $title = "Edit Intercom Group"; 

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                            '<a href="/appsuiteadmin/apps/intercom_admin.php">Intercom</a>',
                            $title));
$page->SetResponseMessage($response);
$page->SetErrorMessage($errors->Dump());
if ($errors->IsEmpty())
    $page->mTemplate->assign($group);
else
    $page->mTemplate->assign($_POST);
$page->mTemplate->assign('members', $members);
$page->mTemplate->assign('id', $id);
$page->Display("apps_intercom_group_admin.tpl");

?>