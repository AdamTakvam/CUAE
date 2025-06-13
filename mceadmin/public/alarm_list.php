<?php

require_once("init.php");
require_once("lib.SystemConfig.php");
require_once("lib.DateUtils.php");


// ** SET UP **

$access = new AccessControl();
$admin_access = $access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$response = $_REQUEST['s_response'] ? Utils::safe_unserialize($_REQUEST['s_response']) : NULL;
$title = "Alarm Management";


// ** HANDLE ACTIONS **

if ($_POST['add_alarm'])
    Utils::redirect("alarm_add.php?type=" . $_POST['type']);

if ($_POST['ack'] && !empty($_POST['selected']))
{
    $log_ids = array_map('intval', $_POST['selected']);
    $db->Execute("UPDATE mce_event_log SET status = ? WHERE mce_event_log_id IN (" . implode(',', $log_ids). ")", 
                 array(LogEventStatus::ACKNOWLEDGED));
    $response = "The selected alarms have been acknowledged.";
}

if ($_POST['resolve'] && !empty($_POST['selected']))
{
    $log_ids = array_map('intval', $_POST['selected']);    
    $db->Execute("UPDATE mce_event_log SET status = ? WHERE mce_event_log_id IN (" . implode(',', $log_ids). ")", 
                 array(LogEventStatus::RESOLVED));
    $response = "The selected alarms have been resolved.";
}


// ** HANDLE DOWNLOAD MIB REQUEST **

if ($_REQUEST['download_mib'])
{
    $file = FRAMEWORK_ROOT . "/MIB/metreos-mce.mib";
	if (file_exists($file))
	{
		$length = filesize($file);
		
		header("Content-type: text/plain;\r\n");
		header("Content-Length: $length;\r\n");
		header("Content-Disposition: attachment; filename=\"metreos-mce.mib\";\r\n");
		readfile($file);
		exit();
	}
	else
	{
	    throw new Exception('Could not find the MIB file located at ' . $file);
	}
}


// ** RETRIEVE ALARM COMPONENTS **

$alarms = $db->GetAll("SELECT * FROM mce_components WHERE type >= " . ALARM_TYPE_ENUM_START . " AND type <= " . ALARM_TYPE_ENUM_END . " ORDER BY name");

for ($i = 0; $i < sizeof($alarms); ++$i)
{
    $alarms[$i]['display_type'] = ComponentType::describe($alarms[$i]['type']);
}

$smtp_count = $db->GetOne("SELECT COUNT(*) FROM mce_components WHERE type = ?", array(ComponentType::SMTP_MANAGER));
$snmp_count = $db->GetOne("SELECT COUNT(*) FROM mce_components WHERE type = ?", array(ComponentType::SNMP_MANAGER));

$alarm_types = array();
if ($smtp_count == 0)
	$alarm_types[ComponentType::SMTP_MANAGER] = ComponentType::describe(ComponentType::SMTP_MANAGER);
if ($snmp_count == 0)
	$alarm_types[ComponentType::SNMP_MANAGER] = ComponentType::describe(ComponentType::SNMP_MANAGER);


// ** RETRIEVE ACTIVE ALARMS **

$active_alarms = $db->GetAll("SELECT * FROM mce_event_log WHERE type = ? AND status <> ? ORDER BY created_timestamp DESC", 
                      array(LogMessageType::ALARM, LogEventStatus::RESOLVED));
foreach ($active_alarms as $i => $alarm)
{
    $active_alarms[$i]['display_severity'] = Severity::$msDisplayName[$alarm['severity']];
    $active_alarms[$i]['display_status']   = LogEventStatus::$msDisplayName[$alarm['status']];
    $active_alarms[$i]['is_open'] = $alarm['status'] == LogEventStatus::OPEN;
    $active_alarms[$i]['is_acknowledged'] = $alarm['status'] == LogEventStatus::ACKNOWLEDGED;
}


// ** RENDER PAGE **

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('admin_access', $admin_access);
$page->mTemplate->assign('alarms', $alarms);
$page->mTemplate->assign('alarm_types', $alarm_types);
$page->mTemplate->assign('active_alarms', $active_alarms);
$page->Display("alarm_list.tpl");

?>