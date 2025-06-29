<?php

require_once("common.php");
require_once("config.backup_restore.php");
require_once("Archive/Tar.php");
require_once("lib.SystemConfig.php");
require_once("function.copyr.php");

class Backup
{
 
 
    // -- DATA MEMBERS --

    private $mId;
    private $mInteractive;
    private $mFiles;
    private $mDestDirectory;
    private $mError;
    
    // Tables from the main database that should not be backed up
    private $mFilterTables = array('as_backups');
    
    
    // -- PUBLIC METHODS --
    
    public function __construct($interactive = FALSE)
    {   
        $this->mInteractive = $interactive;
    }
    
    
    public function Start()
    {  
        @mkdir(BackupRestoreConfig::TEMP_DIR, 0700, TRUE);
        $db = new MceDb();
        $db->Execute("INSERT INTO as_backups (status) VALUES (?)", array(BackupStatus::IN_PROGRESS));
        $this->mId = $db->Insert_ID();
        
        // Write the metadata file
        $this->mFiles['metadata'] = BackupRestoreConfig::TEMP_DIR . BackupRestoreConfig::METADATA_FILENAME;
        $metadata[] = "date = " . date("M d, Y");
        $metadata[] = "backup_restore_version = " . BackupRestoreConfig::BACKUP_RESTORE_VERSION;
        $bytes = file_put_contents($this->mFiles['metadata'], implode("\n", $metadata));
        if (empty($bytes))
        {
            $this->LogFailure("Could not write temporary metadata file.");
            return FALSE;
        }
        
        if (!$this->mInteractive)
            $this->BackupDatabase();
            
        return TRUE;
    }
    
    
    public function BackupDatabase()
    {
        $this->SetStatus(BackupStatus::DB_BACKUP);

        // Get main database tables to backup (filter out ones we don't need)
        $db = new MceDb();

        // Setup mysqldump parameters
        $parameters  = " -u " . MceConfig::DB_USER . " ";
        $parameters .= "-p" . MceConfig::DB_PASSWORD . " ";
        $parameters .= "--add-drop-table ";
        $parameters .= "--single-transaction ";
        
        // Do the dump
        $command = '"' . BackupRestoreConfig::MYSQLBIN_PATH . 'mysqldump"';
        try
        {
	        $tables = $db->GetCol("SHOW TABLES");
	        foreach ($this->mFilterTables as $del_table)
	        {
	            unset($tables[array_search($del_table, $tables)]);
	        }
            $add_params = MceConfig::DB_DATABASE . ' ' . implode(' ', $tables);
            $db_dump = Utils::execute($command . $parameters . $add_params);
        }
        catch (Exception $e)
        {
            $this->LogFailure($e->GetMessage());
            return FALSE; 
        }
        
        $this->mFiles['database'] = BackupRestoreConfig::TEMP_DIR . BackupRestoreConfig::DB_APPSUITE_BACKUP_FILENAME;
        file_put_contents($this->mFiles['database'],$db_dump);
                
        $this->SetStatus(BackupStatus::IN_PROGRESS);
        return TRUE;
    }
    
    
    public function CheckFreeSpace()
    {
        // For linux, disk_free_space() is happy to take any path and find the free space.
        // For windows, it's not as kind - it only wants the directory letter.
        if (ereg("^/", BackupRestoreConfig::ROUTINE_BACKUP_DIR))
            $check_dir = BackupRestoreConfig::ROUTINE_BACKUP_DIR;
        else
            $check_dir = substr(BackupRestoreConfig::ROUTINE_BACKUP_DIR,0,2);
            
        $free_space = disk_free_space($check_dir);
        $required_space = 0;
        foreach ($this->mFiles as $file)
        {
            $required_space += Utils::file_size($file);
        }
        if ($free_space > $required_space)
        {
            return TRUE;
        }
        else
        {
            $this->LogFailure("Not enough free space on backup partition for a new backup. Required $required_space bytes.");
            return FALSE;
        }
    }


    public function Create()
    {
        $db = new MceDb();
        $hostname = str_replace(' ', '_', SystemConfig::get_config(SYSTEM_FRIENDLY_NAME));
        $basename = $hostname . date("-MdY-His") . "-app";

        $file = BackupRestoreConfig::ROUTINE_BACKUP_DIR . $basename . BackupRestoreConfig::BACKUP_EXTENSION;
        $tarfile = new Archive_Tar($file, TRUE);
        foreach ($this->mFiles as $a_file)
        {
            if (!file_exists($a_file))
            {
                $this->LogFailure("Could not find file " . basename($a_file) . " to add to the backup archive.");
                unlink($file);
                return FALSE;                
            }
            // The third parameter is needed so that it archives just the file, and
            // not the folders that make up the path to file as well.
            if (!$tarfile->AddModify($a_file, '', dirname($a_file)))
            {
                $this->LogFailure("Could not add file " . basename($a_file) . " to the backup archive.");
                unlink($file);
                return FALSE;
            }
        }
        $db->StartTrans();
        $db->Execute("UPDATE as_backups SET name = ? WHERE as_backups_id = ?", array($basename, $this->mId));
		$db->Execute("UPDATE as_backups SET status = ? WHERE as_backups_id = ?", array(BackupStatus::DONE,$this->mId));
        $db->CompleteTrans();  
        return TRUE;   
    }

    public function GetError()
    {
        return $this->mError;
    }    
    
    
    // -- PRIVATE METHODS --  
    
    private function LogFailure($reason)
    {
        $this->mError = $reason;
        $this->SetStatus(BackupStatus::FAILED);
        if (!$this->mInteractive)
            echo "Failure: $reason";
    }
    
    private function SetStatus($status)
    {
        $db = new MceDb();
        if (!empty($this->mId))
        {
            $db->Execute("UPDATE as_backups SET status = ? WHERE as_backups_id = ?", array($status,$this->mId));
        }
        else
            throw new Exception("No backup for which to set a status.");
    }
}

?>