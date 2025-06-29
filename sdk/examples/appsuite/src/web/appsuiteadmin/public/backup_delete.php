<?php

require_once("init.php");
require_once("config.backup_restore.php");
require_once("Archive/Tar.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$id = $_REQUEST['id'];

$backups = $db->GetRow("SELECT * FROM as_backups WHERE as_backups_id = ?", array($id));
$backup_file = BackupRestoreConfig::ROUTINE_BACKUP_DIR . $backups['name'] . ".tar.gz";
$tar_file = new Archive_Tar($backup_file, TRUE);
$metadata = $tar_file->extractInString("metadata.properties");
if (empty($metadata))
    $metadata = "No metadata could be found.";

if ($_POST['delete_yes'])
{
    if (!empty($backups['name']) && file_exists($backup_file))
        unlink($backup_file);
    $db->Execute("DELETE FROM as_backups WHERE as_backups_id = ?", array($id));
    Utils::redirect('backup.php');
}

if ($_POST['delete_no'])
{
    Utils::redirect('backup.php');
}

$page = new Layout();
$page->SetPageTitle('Delete Backup');
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', '<a href="backup.php">Settings and Records Backup</a>', 'Delete Backup'));
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('metadata', $metadata);
$page->mTemplate->assign('backup_date', $backups['backup_date']);
$page->Display('backup_delete.tpl');

?>