<?php

require_once("init.php");
require_once("lib.SimpleMatch.php");

function validate_number($number)
{
    return ereg('^[\*\+#0-9]+$',$number);
}


// ** Intialize objects and variables **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();

Utils::trim_array($_POST);


// ** Handle user requests **

if ($_POST['add'])
{
    if (validate_number($_POST['add_number']))
    {
        $db->Execute("INSERT INTO as_activerelay_filter_numbers SET as_users_id = ?, type = ?, number = ?", array($id, FilterNumberType::BLOCK, SimpleMatch::form_regex($_POST['add_type'],$_POST['add_number']) ));
        $feedback = "The number has been added to the blacklist";
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
        $feedback = "The number has been removed from the blacklist";
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
        $feedback = "Blacklist updated";
    }
}    


// ** Render and display **

$results = $db->GetAll("SELECT * FROM as_activerelay_filter_numbers WHERE as_users_id IS NULL AND type = ?", array(FilterNumberType::BLOCK));
$numbers = SimpleMatch::translate_from_regex($results);

$title = "ActiveRelay Blacklist";

$breadcrumbs[] = '<a href="main.php">Home</a>';
$breadcrumbs[] = '<a href="system_mgmt.php">System Management</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);
$page->mTemplate->Assign('numbers', $numbers);
$page->mTemplate->Assign('match_types',SimpleMatchType::$names);
$page->mTemplate->Assign('id', $id);
$page->mTemplate->Assign('type', $type);
$page->Display('ar_blocklist.tpl');
?>