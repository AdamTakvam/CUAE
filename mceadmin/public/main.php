<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();

if ($_REQUEST['reconnect'] > 0)
{
    // Remove this value from session so that it will actually
    // retry connecting to the server next time it needs it.
    unset($_SESSION['app_server_running']);
}

$alarm_count_query = "SELECT COUNT(*) FROM mce_event_log WHERE type = " . LogMessageType::ALARM . " AND status <> " . LogEventStatus::RESOLVED . " AND severity = ?";
$alarm_count_red = $db->GetOne($alarm_count_query, array(Severity::RED));
$alarm_count_yellow = $db->GetOne($alarm_count_query, array(Severity::YELLOW));


$template_vars = array( 'core' => ComponentType::CORE,
                        'app' => ComponentType::APPLICATION,
                        'provider' => ComponentType::PROVIDER,
                        'media_server' => ComponentType::MEDIA_SERVER,
                        'alarm_count_red' => $alarm_count_red,
                        'alarm_count_yellow' => $alarm_count_yellow,
                        'alarms_active' => $alarm_count_red + $alarm_count_yellow,
                        'admin_access' => $access->CheckAccess(AccessControl::ADMINISTRATOR));

$page = new Layout();
$page->SetPageTitle("Main Control Panel");
$page->SetBreadcrumbs(array("Main Control Panel"));
$page->mTemplate->assign($template_vars);
$page->Display("main.tpl");

?>