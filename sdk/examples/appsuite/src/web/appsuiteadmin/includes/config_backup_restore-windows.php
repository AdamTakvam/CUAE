<?php

class BackupRestoreConfig
{
  
    const BACKUP_RESTORE_VERSION    	= "1.0";
    
    // These should stay the same
    const BACKUP_EXTENSION				= ".tar.gz";
    const METADATA_FILENAME         	= "metadata.properties";
    const DB_APPSUITE_BACKUP_FILENAME   = "appsuite.sql";  
    
    // Important paths & file locations
    const TEMP_DIR                  = "C:/temp/appbackup/";
    const ROUTINE_BACKUP_DIR        = "C:/Backup/";
    const MYSQLBIN_PATH             = "C:/Program Files/MySQL/MySQL Server 4.1/bin/";
    const LAST_KNOWN_GOOD_SUFFIX    = "-LKG";

}

?>