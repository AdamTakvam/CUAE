<?php

require_once("init.php");
require_once("class.Template.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$initial_log_path = LOGS_ROOT;
$log_file_path = urldecode($_SERVER['argv'][0]);
if (ereg("\.\.", $log_file_path))
	die('Illegal log file path');
$log_file = $initial_log_path . '/' . $log_file_path;

$page = new Template();
$page->assign('filename', $log_file_path);
$page->assign('log', file_get_contents($log_file));	
$page->display('view_log.tpl');

?>