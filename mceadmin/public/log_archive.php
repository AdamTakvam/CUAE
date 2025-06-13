<?php

require_once("init.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$archive = $_REQUEST['f'];
$file = MceConfig::LOG_TEMP . $archive;

if ($_POST['download'])
{
	if (file_exists($file))
	{
		$length = filesize($file);
		
		header("Content-type: application/zip;\r\n");
		header("Content-Length: $length;\r\n");
		header("Content-Disposition: attachment; filename=\"$archive\";\r\n");
		readfile($file);
		exit();
	}
}

if ($_POST['done'])
{
	if (!is_dir($file) && file_exists($file))
		unlink($file);
	Utils::redirect("logs.php");
}


// -- PAGE RENDERING --

$page = new Layout();
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', 'Server Logs'));
$page->SetPageTitle('Download Logs');
$page->mTemplate->assign('archive', $archive);
$page->Display('log_archive.tpl');

?>