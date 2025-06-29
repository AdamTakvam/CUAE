<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.PageLogic.php");


$id = $_REQUEST['id'];

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN, $id);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$page_logic = new PageLogic();
$time_zone = TimeZoneUtils::get_user_timezone($access);


if ($_POST['view_detail'])
{
    $a_id = Utils::get_first_array_key($_POST['view_detail']);
    Utils::redirect("login_detail.php?id=$a_id");
}


// Retrieve User data
$db->SetSessionTimezone($time_zone);
$user = $db->GetRow("SELECT * FROM as_users WHERE as_users_id = ?", array($id));
$user_real_name = $user['first_name'] . ' ' . $user['last_name'];

// Prepare general SQL
$clause[] = "FROM as_auth_records ";
$clause[] = "WHERE as_users_id = $id ";
$clause[] = "AND originating_number IS NOT NULL ";
$clause[] = "ORDER BY auth_timestamp DESC";

// Do page logic
$count = $db->GetOne("SELECT COUNT(*) " . implode(' ', $clause));
$page_logic->SetItemCount($count);
$page_logic->SetCurrentPageNumber($_REQUEST['p']);
$page_logic->Calculate();

// Retrieve authorization records
$subquery = "SELECT COUNT(*) FROM as_call_records WHERE as_auth_records_id = as_auth_records.as_auth_records_id";
$query = "SELECT *, ($subquery) AS call_count " .
         implode(' ', $clause) .
         $page_logic->GetSqlLimit();
$auths = $db->GetAll($query);

for ($x = 0; $x < sizeof($auths); $x++)
{
    $auths[$x]['status_display'] = AuthenticationResult::$names[ $auths[$x]['status'] ];
    $auths[$x]['invalid_pin'] = ($auths[$x]['status'] == AuthenticationResult::INVALID_ACCOUNT_CODE_OR_PIN);
}
$db->ResetSessionTimezone();


// -- RENDER PAGE --

$title = "Authentication Detail for $user_real_name";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>',
                            '<a href="/appsuiteadmin/reports/account_summary.php">Account Summary</a>',
                            htmlspecialchars($title)));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('user', $user);
$page->mTemplate->assign('display_status', UserStatus::$names[$user['status']]);
$page->mTemplate->assign('auths', $auths);
$page->mTemplate->assign('page_logic', $page_logic);
$page->Display("reports_account_detail.tpl");

?>