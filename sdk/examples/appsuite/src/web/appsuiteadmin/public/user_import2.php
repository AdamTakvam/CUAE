<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$title = "Import Users";


// Retrieve the import data from the session if it is not posted.
// This information has to be stored in session, because it cannot be passed
// as a GET in IE.
if (empty($_POST['s_data']))
{
    $import_data = unserialize($_SESSION['s_data']);
    unset($_SESSION['s_data']);
}
else
    $import_data = unserialize($_POST['s_data']);
$selected = unserialize($_POST['s_selected']);


if ($_POST['cancel'])
    Utils::redirect('account_mgmt.php');

    
// ** Maintain the list of selected user profiles **

if ($_POST['next'] || $_POST['previous'] || $_POST['import'])
{
    for ($x = $_POST['old_first']; $x <= $_POST['old_last']; ++$x)
        unset($selected[$x]);
    if (is_array($_POST['iuser_index']))
        foreach ($_POST['iuser_index'] as $index)
            $selected[$index] = TRUE;
}


// ** Determine user profiles to import **

if ($_POST['import_all'])
{
    $_SESSION['s_import'] = serialize($import_data);
    $query['as_ldap_servers_id'] = $_REQUEST['as_ldap_servers_id'];
    $query['import_group'] = $_REQUEST['import_group'];
    Utils::redirect("user_import3.php" . Utils::make_query($query));    
}

if ($_POST['import'])
{
    if (is_array($selected))
    {
        foreach ($selected as $index => $cond)
        {
            if ($cond)
                $final_import[] = $import_data[$index];
        }
    }
    
    if (empty($final_import))
        $errors->Add('No users were selected for import.');

    if ($errors->IsEmpty())
    {
        $_SESSION['s_import'] = serialize($final_import);
        $query['as_ldap_servers_id'] = $_REQUEST['as_ldap_servers_id'];
        $query['import_group'] = $_REQUEST['import_group'];
        Utils::redirect("user_import3.php" . Utils::make_query($query));
    }
}

// ** Get the current page number **

$p = ($_POST['p'] < 1) ? 1 : $_POST['p'] ;
if ($_POST['next'])
    ++$p;
if ($_POST['previous'])
    --$p;


// ** Pick out range of imported data to display on the page

$count = sizeof($import_data);
$first = ($p - 1) * MceConfig::RECORDS_PER_PAGE;
$last = ($p * MceConfig::RECORDS_PER_PAGE) - 1;
if ($last + 1 > $count)
    $last = $count - 1;
    
$users = array_slice($import_data, $first, MceConfig::RECORDS_PER_PAGE, TRUE);


// ** Render the page **

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>','<a href="account_mgmt.php">Account Management</a>',$title));
$page->SetErrorMessage($errors->Dump());

$page->mTemplate->assign('as_ldap_servers_id', $_REQUEST['as_ldap_servers_id']);
$page->mTemplate->assign('count', $count);
$page->mTemplate->assign('first', $first);
$page->mTemplate->assign('last', $last);
$page->mTemplate->assign('users', $users);
$page->mTemplate->assign('groups', $db->GetAll("SELECT * FROM as_user_groups"));
$page->mTemplate->assign('selected', $selected);
$page->mTemplate->assign('uid_search',$_REQUEST['uid_search']);
$page->mTemplate->assign('sn_search',$_REQUEST['sn_search']);
$page->mTemplate->assign('gn_search',$_REQUEST['gn_search']);
$page->mTemplate->assign('import_group',$_REQUEST['import_group']);
$page->mTemplate->assign('s_selected', serialize($selected));
$page->mTemplate->assign('s_data', serialize($import_data));
$page->mTemplate->assign('p', $p);

$page->Display('user_import2.tpl');

?>

