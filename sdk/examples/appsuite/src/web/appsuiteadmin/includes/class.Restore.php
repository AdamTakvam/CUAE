<?php

require_once("common.php");
require_once("config.backup_restore.php");
require_once("Archive/Tar.php");
require_once("class.ServiceControl.php");
require_once("function.copyr.php");
require_once("function.rmdirr.php");

class Restore
{
    
    
    // -- DATA MEMBERS --
    
    private $mTimeStamp;
    private $mError;
    private $mArchiveFile;
    private $mTempDir;
    private $mBackup;
    
    
    // -- PUBLIC METHODS --
    
    public function __construct()
    {
        $this->mTimeStamp = date("Ymd-His");
        $this->mBackup = FALSE;
    }
    
    
    public function GetFromBackup($id)
    {
        $db = new MceDb();
        $b_info = $db->GetRow("SELECT * FROM as_backups WHERE as_backups_id = ?", array($id));
        if ($b_info['status'] != BackupStatus::DONE)
        {
            $this->LogError("Backup with id $id is not in a ready state for restore.");
            return FALSE;
        }
        $this->mBackup = TRUE;
        return $this->GetFromFile(BackupRestoreConfig::ROUTINE_BACKUP_DIR . $b_info['name'] . BackupRestoreConfig::BACKUP_EXTENSION);
    }
    
    
    public function GetFromFile($file)
    {
        if (empty($this->mArchiveFile))
        {
            if (!file_exists($file))
            {
                $this->LogError("No backup file could be found.");
                return FALSE;
            }
            $this->mArchiveFile = $file;
            return TRUE;
        }
        return FALSE;
    }
    
    
    public function GetMetadata()
    {
        $tar_file = new Archive_Tar($this->mArchiveFile, TRUE);
        return $tar_file->extractInString(BackupRestoreConfig::METADATA_FILENAME);
    }
    
    
    public function CheckFile()
    {
        $tar_file = new Archive_Tar($this->mArchiveFile, TRUE);
        $list = $tar_file->listContent();
        $file_list = array();
        // Extract the filenames from the list array
        foreach ($list as $file)
        {
            $file_list[] = $file['filename'];
        }

        // Check to see if all necessary files are there
        $valid = TRUE;
        if (!in_array(BackupRestoreConfig::METADATA_FILENAME, $file_list))
        	$valid = FALSE;
        if (!in_array(BackupRestoreConfig::DB_APPSUITE_BACKUP_FILENAME, $file_list))
        	$valid = FALSE;

        
        // Not all necessary files are found.                
        if (!$valid) 
        	$this->LogError("Restore file has invalid content.  File list: " . implode(', ',$file_list));
        return $valid;
    }
    
    
    public function DoRestore()
    {   
		$this->StopServices();
        $this->mTempDir = BackupRestoreConfig::TEMP_DIR . "restore-app-" . $this->mTimeStamp . "/";
        $success = TRUE;
        
        try
        {
            if (!mkdir($this->mTempDir, '0775'))
                throw new Exception("Could not create temporary directory $this->mTempDir");
            $tar_file = new Archive_Tar($this->mArchiveFile, TRUE);
            if (!$tar_file->extract($this->mTempDir))
                throw new Exception("Could not temporarily extract restore TAR into $this->mTempDir");
            $this->RestoreDatabase($this->mTempDir . BackupRestoreConfig::DB_APPSUITE_BACKUP_FILENAME, $this->mTempDir);
        	rmdirr($this->mTempDir);
        }
        catch (Exception $e)
        {
            $this->LogError($e->GetMessage());
            if (is_dir($this->mTempDir))
            {
                rmdirr($this->mTempDir);
            }
            $success = FALSE;
        }
        $this->DeleteFile();
        $this->StartServices();
        return $success;  
    }

    
    public function DeleteFile()
    {
        if (!$this->mBackup)
            if (file_exists($this->mArchiveFile))
                unlink($this->mArchiveFile);
    }
    
    
    public function GetError()
    {
        return $this->mError;
    }
    
    
    // -- PRIVATE METHODS --
        
    private function RestoreDatabase($db_dump, $temp_dir)
    {
        if (!file_exists($db_dump))
            throw new Exception("Could not find database file at $db_dump");

        $parameters  = " -u " . MceConfig::DB_USER . " ";
        $parameters .= "-p" . MceConfig::DB_PASSWORD;
        $command = '"' . BackupRestoreConfig::MYSQLBIN_PATH . 'mysql"';
        
        Utils::execute($command . $parameters . ' ' . MceConfig::DB_DATABASE . ' < ' . $db_dump);
    }


    private function StopServices()
    {
        $db = Resources::get_main_db();
        $services = $db->GetCol("SELECT mce_services_id FROM mce_services WHERE enabled = 1");
        foreach ($services as $s_id)
        {
            $sv = new ServiceControl();
            $sv->SetId($s_id);
            if ($status <> ServiceStatus::STOPPED)
                $sv->Stop(TRUE);
        }
    }
   
   
    private function StartServices()
    {
        $db = Resources::get_main_db();
        $services = $db->GetCol("SELECT mce_services_id FROM mce_services WHERE enabled = 1");
        foreach ($services as $s_id)
        {
            $sv = new ServiceControl();
            $sv->SetId($s_id);
            $sv->Start();
        }
    }
            
    private function LogError($error)
    {
        $this->mError = $error;
    }
    
}


?>