<?php

require_once("init.php");
require_once("class.Backup.php");
require_once("lib.SystemConfig.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$step = $_POST['step'] ? $_POST['step'] : 0;
$backup = $_POST['backup'] ? Utils::safe_unserialize($_POST['backup']) : new Backup(TRUE);

if ($_POST['done'])
    Utils::redirect('backup.php');



// -- RENDER PAGE --

$page = new Layout();
$page->TurnOffNavigation();

if ($step == 1)
{
    $backup->Start();
    $backup->BackupDatabase();
    if ($backup->CheckFreeSpace())
        $backup->Create();
}

$page->SetPageTitle('Performing a Backup');
$page->mTemplate->assign('error', $backup->GetError());
$page->mTemplate->assign('step', $step);
$page->mTemplate->assign('backup_object', Utils::safe_serialize($backup));
$page->Display('backup_perform.tpl');

?>