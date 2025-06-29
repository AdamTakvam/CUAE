<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("lib.GroupUtils.php");

Utils::require_req_var('id');


// ** Initialize objects & setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$time_zone = TimeZoneUtils::get_user_timezone($access);

$id = $_REQUEST['id'];


// ** Retrieve data **

$db->SetSessionTimezone($time_zone);
$call_record = $db->GetRow("SELECT *, IF(end-start >= 0,end-start,NULL) AS duration_seconds FROM as_call_records WHERE as_call_records_id = ?", array($id));
$name = $db->GetOne("SELECT CONCAT(first_name, ' ', last_name) FROM as_users WHERE as_users_id = ?", array($call_record['as_users_id']));
$findme_records = $db->GetAll("SELECT * FROM as_findme_call_records WHERE as_call_records_id = ? ", array($id));
$db->ResetSessionTimezone();

$call_record['duration'] = DateUtils::sec_to_min_sec_string($call_record['duration_seconds']);
$call_record['end_reason_display'] = EndReason::$names[ $call_record['end_reason'] ];

for ($x = 0; $x < sizeof($findme_records); $x++)
{
    $findme_records[$x]['type_display'] = FindMeCallRecordType::$names[ $call_record['call_type'] ];
    $findme_records[$x]['end_reason_display'] = EndReason::$names[ $call_record['end_reason'] ];
}


// ** Render page **

$title = "FindMe Call Records";

$breadcrumbs[] = '<a href="/appsuiteadmin/main.php">Home</a>';
$breadcrumbs[] = '<a href="/appsuiteadmin/reports/call_stats.php">Call Statistics</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->mTemplate->Assign('call_record', $call_record);
$page->mTemplate->Assign('name', $name);
$page->mTemplate->Assign('findme_records', $findme_records);
$page->Display('reports_findme_records.tpl');


?>