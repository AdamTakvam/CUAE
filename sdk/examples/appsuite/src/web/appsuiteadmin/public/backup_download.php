<?php
require_once("init.php");
require_once("config.backup_restore.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$id = $_REQUEST['id'];
$db = new MceDb();

$basename = $db->GetOne("SELECT name FROM as_backups WHERE as_backups_id = ?", array($id));

$filename = $basename . ".tar.gz";
$file = BackupRestoreConfig::ROUTINE_BACKUP_DIR . $filename;
$length = filesize($file);

header("Content-type: application/x-gzip;\r\n");
header("Content-Length: $length;\r\n");
header("Content-Disposition: attachment; filename=\"$filename\";\r\n");
readfile($file);
exit();
?>