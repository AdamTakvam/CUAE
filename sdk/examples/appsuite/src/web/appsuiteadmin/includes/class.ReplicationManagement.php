<?php

require_once("common.php");
require_once("config.replication.php");
require_once("class.ReplicationSettingsManagement.php");
require_once("class.ServiceControl.php");


class ReplicationManagement
{

	private $mOsLayer;
    private $mSettingsManager;
    

	// -- PUBLIC METHODS --

	public function __construct()
	{
		$osLayerName = "ReplicationManagement_" . MceConfig::OPERATING_SYSTEM;
		$this->mOsLayer = new $osLayerName;
        $this->mSettingsManager = new ReplicationSettingsManagement();
	}


	public function GetSettings()
	{
		$raw_settings = $this->mSettingsManager->GetMasterSettings();
        if (!empty($raw_settings))
            $settings['role'] = ReplicationRole::MASTER;
        else
        {
            $raw_settings = $this->mSettingsManager->GetSlaveSettings();
            if (!empty($raw_settings))
                $settings['role'] = ReplicationRole::SLAVE;
        }

        if (!empty($raw_settings))
        {
    		$settings['server_id'] 	= $this->mSettingsManager->GetServerId();
    		$settings['host'] 		= $raw_settings['HOST'];
    		$settings['user'] 		= $raw_settings['USER'];		
    		$settings['password'] 	= $raw_settings['PASSWORD'];
        }
		return $settings;					
	}
	
    public function GetGlobalSlaveSettings()
    {
        $raw_settings = $this->mSettingsManager->GetSlaveSettings(TRUE);
        if (!empty($raw_settings))
        {
            $settings['role']       = ReplicationRole::SLAVE;
    		$settings['server_id'] 	= $this->mSettingsManager->GetServerId();
    		$settings['host'] 		= $raw_settings['HOST'];
    		$settings['user'] 		= $raw_settings['USER'];		
    		$settings['password'] 	= $raw_settings['PASSWORD'];
        }
		return $settings;					
    }
    
    public function GetServerId()
    {
        return $this->mSettingsManager->GetServerId();
    }
	
	public function EnableReplication($role, $server_id, $user, $password, $host = NULL)
	{
			
		if ($role == ReplicationRole::MASTER)
		{
			$this->EnableMaster($server_id, $user, $password);
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
	
	
	public function DisableReplication()
	{
		$db = new MceDb();
		$info = $this->GetSettings();
		
		if ($info['role'] == ReplicationRole::MASTER)
		{
			$db->Execute("DELETE FROM mysql.user WHERE User=? AND Host=?", array($info['user'],'%'));
            $this->mSettingsManager->DisableMaster();
		}
		else if ($info['role'] == ReplicationRole::SLAVE)
		{
			$db->Execute("STOP SLAVE");			
            $this->mSettingsManager->DisableSlave();
            if (!$this->mSettingsManager->IsMasterHostSet())
                $db->Execute("CHANGE MASTER TO MASTER_HOST = \"\"");;
        }
        $this->mSettingsManager->ApplySettings();
		return TRUE;
	}


	public function RestartMySQL()
	{
        $db = new MceDb();
        $db->Close();
		$this->mOsLayer->RestartMySQL();
	}


	// -- PRIVATE METHODS --

	private function EnableMaster($server_id, $user, $password)
	{		
		$db = new MceDb();
		$db->Execute("GRANT REPLICATION SLAVE ON *.* TO '$user'@'%' IDENTIFIED BY '$password'");
		$db->Execute("GRANT RELOAD ON *.* TO '$user'@'%'");
		$db->Execute("GRANT SUPER ON *.* TO '$user'@'%'");
		$db->Execute("GRANT SELECT, INSERT, UPDATE, DELETE ON " . MceConfig::DB_DATABASE . ".* TO '$user'@'%'");

        $this->mSettingsManager->SetServerId($server_id);
        $this->mSettingsManager->SetMasterSettings($user, $password);
        $this->mSettingsManager->ApplySettings();

		$this->RestartMySQL();
	}
	
	
	private function EnableSlave($server_id, $user, $password, $host = NULL)
	{		
		$db = new MceDb();
		$slave_status = $db->GetRow("SHOW SLAVE STATUS");
        $db->Execute("STOP SLAVE");
		$db->Execute("CHANGE MASTER TO MASTER_USER = ?, MASTER_PASSWORD = ?", array($user, $password));
		if ($host <> $slave_status['Master_Host'])
		{
			$db->Execute("CHANGE MASTER TO MASTER_HOST = ?", array($host));
		}
        $this->LoadMasterSnapshot($host, $user, $password);

        $this->mSettingsManager->SetServerId($server_id);
        $this->mSettingsManager->SetSlaveSettings($user, $password, $host);
        $this->mSettingsManager->ApplySettings();

		$this->RestartMySQL();
	}

	
	private function LoadMasterSnapshot($host, $username, $password)
	{
		try
		{
			$command = "\"" . ReplicationConfig::MYSQL_DUMP . "\" -h $host -u $username --password=$password " .
					   "--master-data --result-file=" . ReplicationConfig::MYSQL_MASTER_FILE_TEMP . " " .
					   "--databases " . MceConfig::DB_DATABASE;
			Utils::execute($command);
			
			$command = "\"" . ReplicationConfig::MYSQL_BIN . "\" -u " . MceConfig::DB_USER . " -p" . MceConfig::DB_PASSWORD .
					   " " . MceConfig::DB_DATABASE . " < " . ReplicationConfig::MYSQL_MASTER_FILE_TEMP;
			Utils::execute($command);
		}
		catch (Exception $e)
		{
			$message = "Could not create snapshot of publisher database to initialize subscriber.\n";
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
            return Utils::execute("net stop mysql && net start mysql");
        }
        catch (Exception $ex)
        {
            return FALSE;
        }
	}
	
}


?>