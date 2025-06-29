<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();

$backups = $db->GetAll("SELECT * FROM as_backups ORDER BY backup_date DESC");
for ($x = 0; $x < sizeof($backups); $x++)
{
    $backups[$x]['status_display'] = BackupStatus::$names[$backups[$x]['status']];
}


$page = new Layout();
$page->SetPageTitle('All Backups');
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', 'Settings and Records Backup'));
$page->mTemplate->assign('backups', $backups);
$page->Display('backup.tpl');

?>