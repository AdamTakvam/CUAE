<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();

$db = new MceDb();
$clause = array();
$clause_data = array();


// *** COMPOSE FILTERED USER LIST ***

$this_page = ($_REQUEST['p'] > 0) ? $_REQUEST['p'] : 0;
// Get users whose names start with a specific letter
if (isset($_REQUEST['a']))
{
    $clause[] = "WHERE username LIKE ?";
    $clause_data[] = $_REQUEST['a'] . '%';
}

// Calculate number of pages needed for
$user_count = $db->GetOne("SELECT COUNT(mce_users_id) FROM mce_users " . implode(' ', $clause), $clause_data);
$last_page = ceil($user_count / MceConfig::USERS_PER_PAGE) - 1;
$last_page = $last_page > 0 ? $last_page : 0;
$this_page = $this_page > $last_page ? $last_page : $this_page;

// Retrieve user list
$clause[] = "ORDER BY username ASC";
$clause[] = "LIMIT ?,?";
$clause_data[] = $this_page * MceConfig::USERS_PER_PAGE;
$clause_data[] = MceConfig::USERS_PER_PAGE;
$users = $db->GetAll("SELECT * FROM mce_users " . implode(' ', $clause), $clause_data);


// *** RENDER PAGE ***

$title = "Users";
if ($_REQUEST['a'])
{
    $title .= " Starting with " . strtoupper($_REQUEST['a']);
}

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', $title));
$page->SetErrorMessage($errors->Dump());

$template_vars = array('users'      => $users,
                       'alphabet'   => range('a','z'),
                       'pages'      => range(0,$last_page),
                       'this_letter'=> $_REQUEST['a'],
                       'this_page'  => $this_page,
                       'last_page'  => $last_page);
$page->mTemplate->assign($template_vars);
$page->Display("users.tpl");

?>