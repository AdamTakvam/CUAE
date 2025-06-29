<?php

require_once("init.php");
require_once("lib.SimpleMatch.php");

$id = Utils::require_req_var('id');
$type = Utils::require_req_var('type');
$from = $_REQUEST['from'];

function validate_number($number)
{
    return ereg('^[\*\+#0-9]+$',$number);
}


// ** Intialize objects and variables **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL, $id);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
if ($id > 0 && $group_admin_access)
    $group_admin_access = $access->CheckPageAccess(AccessControl::GROUP_ADMIN, $id);

$errors = new ErrorHandler();
$db = new MceDb();

Utils::trim_array($_POST);


// ** Handle requests

if ($_POST['go_back'])
{
    if ($from == "arpage")
        Utils::redirect("apps/active_relay.php");
    else
        Utils::redirect("user.php?id=$id");
}

if ($_POST['add'])
{
    if (validate_number($_POST['add_number']))
    {
        $db->Execute("INSERT INTO as_activerelay_filter_numbers SET as_users_id = ?, type = ?, number = ?", array($id, $type, SimpleMatch::form_regex($_POST['add_type'],$_POST['add_number']) ));
        $feedback = "The number has been added to the list";
    }
    else
        $errors->Add($_POST['add_number'] . " is not a valid number format");
}

if ($_POST['delete'])
{
    $del_id = Utils::get_first_array_key($_POST['delete']);
    if (!empty($del_id))
    {
        $db->Execute("DELETE FROM as_activerelay_filter_numbers WHERE as_activerelay_filter_numbers_id = ?", array($del_id));
        $feedback = "The number has been removed from the list";
    }
    else
        $errors->Add("That number could not be found.");
}
    
if ($_POST['update'] && !empty($_POST['numbers']))
{
    foreach ($_POST['numbers'] as $key => $values)
    {
        if (!validate_number($values['number']))
            $errors->Add($values['number'] . " is not a valid number format");
    }
    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        foreach ($_POST['numbers'] as $key => $values)
        {
            $db->Execute("UPDATE as_activerelay_filter_numbers SET number = ? WHERE as_activerelay_filter_numbers_id = ?", array(SimpleMatch::form_regex($values['type'],$values['number']),$key));
        }
        $db->CompleteTrans();
        $feedback = "Number list updated";
    }
}    
    

// ** Render page **

$name = $db->GetOne("SELECT CONCAT(first_name,' ',last_name) FROM as_users WHERE as_users_id = ?", array($id));
$results = $db->GetAll("SELECT * FROM as_activerelay_filter_numbers WHERE as_users_id = ? AND type = ?", array($id,$type));
$numbers = SimpleMatch::translate_from_regex($results);

switch ($type)
{
    case FilterNumberType::ALLOW :
        $title = "ActiveRelay Whitelist";   break;
    case FilterNumberType::BLOCK :
        $title = "ActiveRelay Blacklist";   break;
    default :
        break;
}

$breadcrumbs[] = '<a href="main.php">Home</a>';
if ($from == "arpage")
{
    $breadcrumbs[] = '<a href="apps/active_relay.php">Active Relay</a>';
}
else
{
    $breadcrumbs[] = '<a href="account_mgmt.php">Account Management</a>';
    $breadcrumbs[] = '<a href="user.php?id='.$id.'">Edit Account for '.htmlspecialchars($name).'</a>';
}
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);
$page->mTemplate->Assign('numbers', $numbers);
$page->mTemplate->Assign('id', $id);
$page->mTemplate->Assign('from', $from);
$page->mTemplate->Assign('type', $type);
$page->mTemplate->Assign('match_types',SimpleMatchType::$names);
$page->Display('user_ar_filter_manage.tpl');

?>