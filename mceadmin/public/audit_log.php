<?php

require_once("init.php");
require_once("lib.SystemConfig.php");
require_once("lib.DateUtils.php");
require_once("class.PageLogic.php");


// -- SET UP PAGE PROPERTIES --

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$pl = new PageLogic();
$page = $_REQUEST['p'];
if ($_REQUEST['t'])
{
    $type = $_REQUEST['t'];
    $title = LogMessageType::display($type) . " Log";
}
else
{
    $type = array(LogMessageType::ALARM, LogMessageType::SECURITY);
    $title = "Event Log";
}


// -- CALCULATE PAGINATED LOGS --

$pl->SetItemCount(EventLog::get_log_size($type));
$pl->SetCurrentPageNumber($page);
$pl->AddQueryVar('t', $_REQUEST['t']);
$pl->Calculate();


// -- RETRIEVE LOG PAGE --

$short_log = EventLog::get_log($type, $pl->sql_limit, $pl->sql_start);
for ($x = 0; $x < sizeof($short_log); ++$x)
{
    $short_log[$x]['type_display'] = LogMessageType::display($short_log[$x]['type']);
    $short_log[$x]['display_status']   = LogEventStatus::$msDisplayName[$short_log[$x]['status']];
}


// -- RENDER PAGE --

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadCrumbs(array('<a href="main.php">Main Control Panel</a>', $title));
$page->mTemplate->assign('_audit_log', $short_log);
$page->mTemplate->assign('type', $type);
$page->mTemplate->assign_by_ref('page_logic', $pl);
$page->Display('audit_log.tpl');

?>