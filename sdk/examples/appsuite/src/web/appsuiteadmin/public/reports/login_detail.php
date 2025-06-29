<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.PageLogic.php");


$id = $_REQUEST['id'];

// Retrieve Authentication record data
$db = new MceDb();
$auth = $db->GetRow("SELECT * FROM as_auth_records WHERE as_auth_records_id = ?", array($id));

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN, $auth['as_users_id']);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$page_logic = new PageLogic();
$time_zone = TimeZoneUtils::get_user_timezone($access);
$db->SetSessionTimezone($time_zone);

// Retrieve Auth data
$auth = $db->GetRow("SELECT * FROM as_auth_records WHERE as_auth_records_id = ?", array($id));

// Retrieve User data
$user = $db->GetRow("SELECT * FROM as_users WHERE as_users_id = ?", array($auth['as_users_id']));
$user_real_name = $user['first_name'] . ' ' . $user['last_name'];

// Prepare general SQL
$clause[] = "FROM as_call_records WHERE as_auth_records_id = $id ";
$clause[] = "ORDER BY start DESC";

// Do paging logic
$count = $db->GetOne("SELECT COUNT(*) " . implode(' ', $clause));
$page_logic->SetCurrentPageNumber($_REQUEST['p']);
$page_logic->SetItemCount($count);
$page_logic->Calculate();

// Retrieve call records
$query  = "SELECT *, IF(end-start >= 0, end-start, NULL) AS duration_seconds " .
          implode(' ', $clause) .
          $page_logic->GetSqlLimit();

$calls = $db->GetAll($query);
$db->ResetSessionTimezone();

for ($x = 0; $x < sizeof($calls); $x++)
{
    $calls[$x]['duration'] = DateUtils::sec_to_min_sec_string($calls[$x]['duration_seconds']);
    $calls[$x]['end_reason_display'] = EndReason::$names[ $calls[$x]['end_reason'] ];
}


// -- RENDER PAGE --

$title = "Login Detail for $user_real_name";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                            '<a href="/appsuiteadmin/reports/account_summary.php">Account Summary</a>',
                            '<a href="/appsuiteadmin/reports/account_detail.php?id=' . $auth['as_users_id'] . '">Account Detail</a>',
                            htmlspecialchars($title)));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('user', $user);
$page->mTemplate->assign('auth_timestamp', $auth['auth_timestamp']);
$page->mTemplate->assign('calls', $calls);
$page->mTemplate->assign('page_logic', $page_logic);
$page->Display("reports_login_detail.tpl");

?>