<?php

require_once("init.php");
require_once("class.Restore.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();

$backups = $db->GetAll("SELECT * FROM as_backups WHERE status = ? ORDER BY backup_date DESC", 
                       array(BackupStatus::DONE));


if ($_POST['select'] || $_POST['upload'])
{
    $restore = new Restore();
    
    if ($_POST['select'])
        $restore->GetFromBackup($_POST['backup_id']);
    else if ($_POST['upload'])
    {
        $source = $_FILES['restore_file']['tmp_name'];
        if (eregi("\.tar\.gz$", $_FILES['restore_file']['name']))
        {
        	if (!file_exists(BackupRestoreConfig::TEMP_DIR))
        		if (!mkdir(BackupRestoreConfig::TEMP_DIR,'0775'))
        			throw new Exception("Could not create temporary directory " . BackupRestoreConfig::TEMP_DIR);
        		
            $destination = BackupRestoreConfig::TEMP_DIR . $_FILES['restore_file']['name'];
            if (move_uploaded_file($source, $destination))
                $restore->GetFromFile($destination);
            else
                $errors->Add("Could not move the uploaded file to the temporary work directory.");
        }
        else
        {
            $errors->Add("Restore file format not valid.");
        }
    }
    
    if ($errors->IsEmpty())
    {
        if (!$restore->CheckFile())
            $errors->Add($restore->GetError());
        else
            Utils::redirect("restore_perform.php?step=1&s_restore=" . Utils::safe_serialize($restore));
    }
    
}


$page = new Layout();
$page->SetErrorMessage($errors->Dump());
$page->SetPageTitle('Restore');
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', 'Restore'));
$page->mTemplate->assign('backups', $backups);
$page->Display('restore.tpl')

?>