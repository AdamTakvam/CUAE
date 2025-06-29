<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.GroupUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.PageLogic.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
if (!$admin_access)
    $user_id = $access->GetUserId();

$db = new MceDb();
$page_logic = new PageLogic();
$time_zone = TimeZoneUtils::get_user_timezone($access);


// HANDLE ACTIONS

if ($_POST['edit'])
{
    $c_id = Utils::get_first_array_key($_POST['edit']);
    Utils::redirect("scheduled_conference_detail.php?id=$c_id");
}

if ($_POST['add'])
{
    Utils::redirect('scheduled_conference_detail.php');
}



// Prepare general SQL
$db->SetSessionTimezone($time_zone);
$clause[] = "FROM as_scheduled_conferences AS sc, as_users AS u ";
$clause[] = "WHERE sc.as_users_id = u.as_users_id ";
if (!$admin_access)
{
    if ($group_admin_access)
    {
        $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
        $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
        $clause[] ="AND sc.as_users_id IN ($sq)";    
    }
    else
    {
        $clause[] = "AND sc.as_users_id = $user_id ";
        $clause[] = "AND sc.status = " . ConferenceStatus::ENABLED;
    }
}
$clause[] = "AND (sc.scheduled_timestamp + INTERVAL sc.duration_minutes MINUTE) >= NOW()";
$clause[] = "ORDER BY scheduled_timestamp ASC";

// Do page logic
$count = $db->GetOne("SELECT COUNT(sc.as_scheduled_conferences_id) " . implode(' ', $clause));
$page_logic->SetCurrentPageNumber($_REQUEST['p']);
$page_logic->SetItemCount($count);
$page_logic->Calculate();

// Retrieve values
$conferences = $db->GetAll("SELECT sc.*, u.username " .
                           implode(' ', $clause) .
                           $page_logic->GetSqlLimit());

for ($x = 0; $x < sizeof($conferences); $x++)
{
    // Convert minutes to hour/minutes
    $hours = floor(floatval($conferences[$x]['duration_minutes']) / 60);
    $mins = floatval($conferences[$x]['duration_minutes']) % 60;
    $conferences[$x]['duration'] = "$hours hr(s) $mins min(s)";

    // Check for status
    $conferences[$x]['status_display'] = ConferenceStatus::$names[$conferences[$x]['status']];
    $conferences[$x]['disabled'] = ($conferences[$x]['status'] == ConferenceStatus::DISABLED);
}
$db->ResetSessionTimezone();


// -- RENDER PAGE --

$title = 'ScheduledConference';

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', $title));
$page->SetResponseMessage($response);
$page->mTemplate->assign('conferences', $conferences);
$page->mTemplate->assign('group_admin_access', $group_admin_access);
$page->mTemplate->assign('admin_access', $admin_access);
$page->mTemplate->assign('page_logic', $page_logic);
$page->Display("apps_scheduled_conference.tpl");

?>