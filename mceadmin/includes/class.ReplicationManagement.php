<?php

require_once("common.php");
require_once("config.replication.php");
require_once("class.ReplicationSettingsManagement.php");
require_once("class.ServiceControl.php");


class ReplicationManagement
{

	private $mOsLayer;
    private $mSettingsManager;
    private $mMainDb;
    

	// -- PUBLIC METHODS --

	public function __construct()
	{
		$osLayerName = "ReplicationManagement_" . MceConfig::OPERATING_SYSTEM;
		$this->mOsLayer = new $osLayerName;
        $this->mSettingsManager = new ReplicationSettingsManagement();
        
        $this->mMainDb = &ADONewConnection(MceConfig::DB_TYPE);
        $this->mMainDb->Connect(MceConfig::DB_SERVER, MceConfig::DB_USER, MceConfig::DB_PASSWORD, "mysql");
        $this->mMainDb->SetFetchMode(ADODB_FETCH_ASSOC);
        $this->mMainDb->SetCharSet(MceConfig::DB_CHARSET);        
	}


	public function GetMasterSettings()
	{
		$raw_settings = $this->mSettingsManager->GetMasterSettings();
        if (!empty($raw_settings))
        {
    		$settings['redundancy_standby_username'] 	= $raw_settings['USER'];		
    		$settings['redundancy_standby_password'] 	= $raw_settings['PASSWORD'];
        }
		return $settings;					
	}
	    
    public function GetSlaveSettings()
    {
        $raw_settings = $this->mSettingsManager->GetSlaveSettings(TRUE);
        if (!empty($raw_settings))
        {
            $settings['config_file_host']               = $raw_settings['HOST'];
    		$settings['redundancy_master_username'] 	= $raw_settings['USER'];		
    		$settings['redundancy_master_password'] 	= $raw_settings['PASSWORD'];
        }
		return $settings;					
    }
    
    public function GetServerId()
    {
        return $this->mSettingsManager->GetServerId();
    }
    
    public function SetServerId($id)
    {
        return $this->mSettingsManager->SetServerId($id);
    }

    public function IsSlaveEnabled()
    {
        $settings = $this->mSettingsManager->GetSlaveSettings();
        return !empty($settings);
    }
	
	public function EnableReplication($role, $server_id, $user, $password, $host = NULL)
	{
			
		if ($role == ReplicationRole::MASTER)
		{
			$this->EnableMaster($server_id, $user, $password, $host);
		}
		else if ($role == ReplicationRole::SLAVE)
		{
			$this->EnableSlave($server_id, $user, $password, $host);
		}
		else
		{
			throw new Exception("Not a recognizable replication server role");
		}

	}
	
	public function DisableMaster()
	{
		$info = $this->GetMasterSettings();
		if (!empty($info['redundancy_standby_username']))
		{
		    $this->mMainDb->Execute("DELETE FROM user WHERE User = ? AND Host <> '%'", array($info['redundancy_standby_username']));
		}
        $this->mSettingsManager->DisableMaster();
        $this->mSettingsManager->ApplySettings();
		return TRUE;
	}

	public function DisableSlave()
	{
        $this->mMainDb->Execute("STOP SLAVE");			
        $this->mSettingsManager->DisableSlave();
        $this->mSettingsManager->ApplySettings();
        if (!$this->mSettingsManager->IsMasterHostSet())
            $this->mMainDb->Execute("CHANGE MASTER TO MASTER_HOST = \"\"");;
		return TRUE;
	}
    
	public function RestartMySQL()
	{
	    // Restarting MySQL now involves stopping (almost) all of the services because
	    // several of them depend on MySQL as a depedency.  The media engine doesn't
	    // depend on the database, though, and is a pain to restart.
        $db = new MceDb();
        $services = $db->GetCol("SELECT mce_services_id FROM mce_services WHERE enabled = 1 AND name <> ?", array(MEDIA_SERVER_SERVICE_NAME));
        foreach ($services as $id)
        {
            $service = new ServiceControl();
            $service->SetId($id);
            $service->Stop(TRUE);
        }
        $db->Close();
        
		$this->mOsLayer->RestartMySQL();
		
		$db = new MceDb();
	    foreach ($services as $id)
        {
            $service = new ServiceControl();
            $service->SetId($id);
            $service->Start();
        }		
	}


	// -- PRIVATE METHODS --

	private function EnableMaster($server_id, $user, $password, $host)
	{		
		$this->mMainDb->Execute("GRANT REPLICATION SLAVE ON *.* TO ?@? IDENTIFIED BY ?", array($user, $host, $password));
		$this->mMainDb->Execute("GRANT RELOAD ON *.* TO ?@?", array($user, $host));
		$this->mMainDb->Execute("GRANT SUPER ON *.* TO ?@?", array($user, $host));
		$this->mMainDb->Execute("GRANT SELECT, INSERT, UPDATE, DELETE ON " . MceConfig::DB_DATABASE . ".* TO ?@?", array($user, $host));
		$this->mMainDb->Execute("FLUSH PRIVILEGES");

        $this->mSettingsManager->SetServerId($server_id);
        $this->mSettingsManager->SetMasterSettings($user, $password);
        $this->mSettingsManager->ApplySettings();        
    }
	
	
	private function EnableSlave($server_id, $user, $password, $host = NULL)
	{		
        $slave_status = $this->mMainDb->GetRow("SHOW SLAVE STATUS");
        $this->mMainDb->Execute("STOP SLAVE");
		$this->mMainDb->Execute("CHANGE MASTER TO MASTER_USER = ?, MASTER_PASSWORD = ?", array($user, $password));
		if ($host <> $slave_status['Master_Host'])
		{
			$this->mMainDb->Execute("CHANGE MASTER TO MASTER_HOST = ?", array($host));
		}
        $this->LoadMasterSnapshot($host, $user, $password);

        $this->mSettingsManager->SetServerId($server_id);
        $this->mSettingsManager->SetSlaveSettings($user, $password, $host);
        $this->mSettingsManager->ApplySettings();
        
        try
        {
            $this->mMainDb->Execute("START SLAVE");
        }
        catch (Exception $e)
        {
            // Probably complains that the server is not configured as a slave.
            // MySQL needs to be rebooted for this, which must be explicitly done elsewhere.
        }
	}

	
	private function LoadMasterSnapshot($host, $username, $password)
	{
		try
		{
			$command = "\"" . ReplicationConfig::MYSQL_DUMP . "\" -h $host -u $username --password=$password " .
					   "--master-data --result-file=\"" . ReplicationConfig::MYSQL_MASTER_FILE_TEMP . "\" " . MceConfig::DB_DATABASE;
			Utils::execute_with_cmd($command);
			
			$command = "\"" . ReplicationConfig::MYSQL_BIN . "\" -u " . MceConfig::DB_USER . " -p" . MceConfig::DB_PASSWORD .
					   " " . MceConfig::DB_STANDBY_DATABASE . " < \"" . ReplicationConfig::MYSQL_MASTER_FILE_TEMP . "\"";
			Utils::execute_with_cmd($command);
			@unlink(ReplicationConfig::MYSQL_MASTER_FILE_TEMP);
		}
		catch (Exception $e)
		{
			if (file_exists(ReplicationConfig::MYSQL_MASTER_FILE_TEMP))
			    unlink(ReplicationConfig::MYSQL_MASTER_FILE_TEMP);
		    $message  = "Could not create a snapshot of the master database.  Either the master database is not available or the credentials to access it are incorrect.\n";
			if (MceConfig::DEV_MODE)
				$message .= $e->GetMessage();
			throw new Exception($message);
		}
	}


}


// -- OS LEVEL AGGREGATES --

class ReplicationManagement_windows
{
	
	public function RestartMySQL()
	{
        try
        {
            return Utils::execute("\"" . SCRIPTS_ROOT. "/restart_service.vbs\" mysql");
        }
        catch (Exception $ex)
        {
            return FALSE;
        }
	}
	
}


?>