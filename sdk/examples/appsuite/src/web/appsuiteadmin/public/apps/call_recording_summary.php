<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.PageLogic.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
if (!$admin_access)
    $user_id = $access->GetUserId();
    
$db = new MceDb();
$errors = new ErrorHandler();
$time_zone = TimeZoneUtils::get_user_timezone($access);


// -- DATA RETRIEVAL --

$db->SetSessionTimezone($time_zone);
// Setup user filter
if ($admin_access)
{
    $user_filter = "1=1";
}
else if ($group_admin_access)
{
    $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
    $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
    $user_filter = "as_users_id IN ($sq)";
}
else 
{
    $user_filter = "as_users_id = $user_id";
}

// Set up generic query
$present_subquery = "SELECT DISTINCT as_call_records_id FROM as_recordings WHERE $user_filter";
$count_subquery = "SELECT COUNT(*) FROM as_recordings WHERE as_call_records_id = cr.as_call_records_id AND end <> '0000-00-00 00:00:00' AND $user_filter";

$clause[] = "FROM as_call_records AS cr, as_users AS u";
$clause[] = "WHERE cr.as_call_records_id IN ($present_subquery)";
$clause[] = "AND cr.as_users_id = u.as_users_id";
$clause[] = "ORDER BY cr.start DESC";

// Set up page logic
$page_logic = new PageLogic();
$count = $db->GetOne("SELECT COUNT(cr.as_call_records_id) " . implode(' ', $clause), $vals);
$page_logic->SetItemCount($count);
$page_logic->SetCurrentPageNumber($_REQUEST['p']);
$page_logic->Calculate();

// Retrieve call records with recordings
$query = "SELECT cr.*, CONCAT(u.first_name, ' ', u.last_name) AS name, " .
         "IF(cr.end-cr.start >= 0,cr.end-cr.start,NULL) AS duration_seconds, " .
         "($count_subquery) AS recording_count " . 
         implode(' ', $clause) .
         $page_logic->GetSqlLimit();
$calls = $db->GetAll($query, $vals);
$db->ResetSessionTimezone();

for ($x = 0; $x < sizeof($calls); $x++)
{
    $calls[$x]['duration'] = DateUtils::sec_to_min_sec_string($calls[$x]['duration_seconds']);
}


// -- RENDER PAGE --

$title = "RapidRecord";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                            $title));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('calls', $calls);
$page->mTemplate->assign('group_admin_access', $group_admin_access);
$page->mTemplate->assign('admin_access', $admin_access);
$page->mTemplate->assign('page_logic', $page_logic);
$page->Display("apps_call_recording_summary.tpl");

?>