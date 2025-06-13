<?php

require_once("init.php");
require_once("lib.SystemConfig.php");
require_once("lib.DateUtils.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);
$type = $_REQUEST['t'];

$title = LogMessageType::display($type) . ' Detail';

$id = $_REQUEST['id'];


$db = new MceDb();
$entry = $db->GetRow("SELECT * FROM mce_event_log WHERE mce_event_log_id = ?", array($id));
$entry['type_display'] = LogMessageType::display($entry['type']); 

// -- RENDER PAGE --

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadCrumbs(array('<a href="main.php">Main Control Panel</a>', 
							'<a href="audit_log.php?t=' . $type . '">' .  LogMessageType::display($type) . ' Log</a>',
							$title));
$page->mTemplate->assign('entry', $entry);
$page->Display('audit_detail.tpl');

?>