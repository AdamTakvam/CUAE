<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.GroupUtils.php");
require_once("lib.TimeZoneUtils.php");


// :TODO: Better security so that only users with recordings to this call can see this
// page.  This is too much to think about hours before shipping the box.

$id = $_REQUEST['id'];

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
if (!$admin_access)
    $user_id = $access->GetUserId();

$db = new MceDb();
$errors = new ErrorHandler();
$time_zone = TimeZoneUtils::get_user_timezone($access);


$db->SetSessionTimezone($time_zone);
// Retrieve call record
$query = "SELECT *, IF(end-start >= 0, end-start, NULL) AS duration_seconds " .
         "FROM as_call_records WHERE as_call_records_id = ?";
$record = $db->GetRow($query, $id);
$record['duration'] = DateUtils::sec_to_min_sec_string($record['duration_seconds']);

// Retrieve recordings
$clause[] = "FROM as_recordings AS r, as_users AS u";
$clause[] = "WHERE r.as_call_records_id = $id ";
if (!$admin_access)
{
    if ($group_admin_access)
    {
        $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
        $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
        $clause[] ="AND r.as_users_id IN ($sq)";
    }
    else
        $clause[] = "AND r.as_users_id = $user_id";
}
$clause[] = "AND r.as_users_id = u.as_users_id";
$clause[] = "ORDER BY r.start ASC";

$query = "SELECT r.*, " .
         "IF(r.end-r.start >= 0, r.end-r.start, NULL) AS duration_seconds, " .
         "CONCAT(u.first_name, ' ', u.last_name) AS name " .
         implode(' ',$clause);
$recordings = $db->GetAll($query);
for ($x = 0; $x < sizeof($recordings); $x++)
{
    $recordings[$x]['duration'] = DateUtils::sec_to_min_sec_string($recordings[$x]['duration_seconds']);
    $recordings[$x]['type_display'] = MediaFileType::$names[ $recordings[$x]['type'] ];
}
$db->ResetSessionTimezone();


// -- RENDER PAGE --

$title = "Recordings";
$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                            '<a href="/appsuiteadmin/apps/call_recording_summary.php">RapidRecord</a>',
                            $title));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('group_admin_access', $group_admin_access);
$page->mTemplate->assign('admin_access', $admin_access);
$page->mTemplate->assign('global_webhost', $_SERVER['HTTP_HOST']);
$page->mTemplate->assign('global_recording_path', ConfigUtils::get_global_config(GlobalConfigNames::RECORDINGS_PATH));
$page->mTemplate->assign('record', $record);
$page->mTemplate->assign('recordings', $recordings);
$page->Display("apps_call_recordings.tpl");

?>