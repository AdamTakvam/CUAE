<?php


require_once("remoteagent_init.php");

$id = $_REQUEST['id'];

$access = new RemoteAgentAccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$user_id = $access->GetUserId();

$errors = new ErrorHandler();
$db = new MceDb();

Utils::trim_array($_POST);

if ($_POST['cancel'])
{
    Utils::redirect("user.php?id=$user_id");
}


// HANDLE ACTIONS

if ($_POST['submit'])
{
    if (empty($_POST['phone_number'])) { $errors->Add('Phone Number Required'); }
    $_POST['ar_enabled'] = (string)(bool) $_POST['ar_enabled'];

    if ($errors->IsEmpty())
    {

        // Create Number
        if (empty($id))
        {
            $db->Execute("INSERT INTO as_external_numbers (as_users_id, name, phone_number, ar_enabled) " .
                         "VALUES (?,?,?,?)",
                         array($user_id, $_POST['name'], $_POST['phone_number'], $_POST['ar_enabled']));
             Utils::redirect("user.php?id=$user_id");
        }

        // Update Number
        if ($id > 0)
        {
            $db->Execute("UPDATE as_external_numbers SET name = ?, phone_number = ?, ar_enabled = ? " .
                         "WHERE as_external_numbers_id = ?",
                         array($_POST['name'], $_POST['phone_number'], $_POST['ar_enabled'], $id));
             $response = "External number updated.";
        }

    }

}


// RETRIEVE VALUES

$full_name = $db->GetOne("SELECT CONCAT(first_name, ' ', last_name) FROM as_users WHERE as_users_id = ?", array($user_id));

$tpl_vars = array('id'          => $id,
                  'user_id'     => $user_id);

if ($id > 0)
{
    $number = $db->GetRow("SELECT * FROM as_external_numbers WHERE as_external_numbers_id = ?",
                          array($id));
    $title = "Edit Number ";
}
else
{
    $title = "Add Number";
}



// -- PAGE RENDER --

$breadcrumbs[] = "<a href=\"user.php\">Remote Agent for $full_name</a>";
$breadcrumbs[] = $title;

$page = new RemoteAgentLayout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($tpl_vars);
if (!$errors->IsEmpty())
{
    $page->mTemplate->assign($_POST);
}
else if ($id > 0)
{
    $page->mTemplate->assign($number);
}
$page->Display("ext_number.tpl");

?>