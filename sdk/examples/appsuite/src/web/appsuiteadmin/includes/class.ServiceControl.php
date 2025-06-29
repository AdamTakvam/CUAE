<?php

require_once("common.php");

class ServiceControl
{
    
    
    private $mId;
    private $mOsLayer;
    private $mInfo;
    private $mStatus;
    
    const SERVICE_TIMEOUT 	= 60;
    
    
    public function __construct()
    {
        $class = '_ServiceControl_' . MceConfig::OPERATING_SYSTEM;
        $this->mOsLayer = new $class;
    }
    
    
    public function SetId($id)
    {
        $this->mId = $id;
        $db = Resources::get_main_db();
        $this->mInfo = $db->GetRow("SELECT * FROM mce_services WHERE mce_services_id = ?", array($this->mId));
    }
    
    
    public function GetFromName($name)
    {
        $db = Resources::get_main_db();
        $id = $db->GetOne("SELECT mce_services_id FROM mce_services WHERE name = ?", array($name));
        if ($id > 0)
            $this->SetId($id);
        else
            return FALSE;
        return TRUE;
    }
    
    
    public function GetId()
    {
    	return $this->mInfo['mce_services_id'];
    }
    
        
    public function GetName()
    {
    	return $this->mInfo['display_name'];
    }
    

    public function GetDescription()
    {
    	return $this->mInfo['description'];
    }
    
    
    public function GetStatus()
    {
    	$this->mStatus = $this->mOsLayer->_GetStatus($this->mInfo['name']);
    	if ($this->mStatus == ServiceStatus::RUNNING)
    	{
			$db = Resources::get_main_db();
			$db->Execute("UPDATE mce_services SET user_stopped = 0 WHERE mce_services_id = ?", array($this->mId));
    	}
    	return $this->mStatus;
    }
    
    
    public function IsEnabled()
    {
    	return $this->mInfo['enabled'];
    }
    
    
    public function Enable()
    {
		$this->mOsLayer->_EnableService($this->mInfo['name']);
        $db = Resources::get_main_db();
		$db->Execute("UPDATE mce_services SET enabled = 1 WHERE mce_services_id = ?", array($this->mId));
		$this->mInfo['enabled'] = 1;
//        EventLog::log(LogMessageType::AUDIT, $this->mInfo['display_name'] . " service was enabled", LogMessageId::SERVICE_ENABLED);
    }


    public function Disable()
    {
    	$this->Stop();
		$this->mOsLayer->_DisableService($this->mInfo['name']);
        $db = Resources::get_main_db();
		$db->Execute("UPDATE mce_services SET enabled = 0 WHERE mce_services_id = ?", array($this->mId));
		$this->mInfo['enabled'] = 0;
//        EventLog::log(LogMessageType::AUDIT, $this->mInfo['display_name'] . " service was disabled", LogMessageId::SERVICE_DISABLED);
    }
    
    
    public function Start($wait = FALSE)
    {
    	$this->GetStatus();
        if ($this->mStatus == ServiceStatus::STOPPED)
        {
	        set_time_limit(self::SERVICE_TIMEOUT);
            $this->mOsLayer->_StartService($this->mInfo['name'], $wait);
			$db = Resources::get_main_db();
			$db->Execute("UPDATE mce_services SET user_stopped = 0 WHERE mce_services_id = ?", array($this->mId));
//            EventLog::log(LogMessageType::AUDIT, $this->mInfo['display_name'] . " service was started", LogMessageId::SERVICE_STARTED);
        }
    }
    
    
    public function Stop($wait = FALSE)
    {
    	$this->GetStatus();
        if ($this->mStatus == ServiceStatus::RUNNING)
        {
	        set_time_limit(self::SERVICE_TIMEOUT);
			$db = Resources::get_main_db();
			$db->Execute("UPDATE mce_services SET user_stopped = 1 WHERE mce_services_id = ?", array($this->mId));            
			$this->mOsLayer->_StopService($this->mInfo['name'], $wait);
//            EventLog::log(LogMessageType::AUDIT, $this->mInfo['display_name'] . " service was stopped", LogMessageId::SERVICE_STOPPED);
        }
    }


	public function Kill()
	{
		$this->GetStatus();
        set_time_limit(self::SERVICE_TIMEOUT);
        $db = Resources::get_main_db();
		$db->Execute("UPDATE mce_services SET user_stopped = 1 WHERE mce_services_id = ?", array($this->mId));            
		$this->mOsLayer->_KillService($this->mInfo['name']);
//        EventLog::log(LogMessageType::AUDIT, $this->mInfo['display_name'] . " service had its process killed", LogMessageId::SERVICE_STOPPED);
	}
    
    
}


class _ServiceControl_windows
{
    

    public function _StartService($service_name, $wait)
    {
    	if ($wait)
        	return Utils::execute("net start \"$service_name\"");
       	else
       		return Utils::execute("sc start \"$service_name\"");
    }
    
    public function _StopService($service_name, $wait)
    {
    	if ($wait)
        	return Utils::execute("net stop \"$service_name\" /y");
       	else
       		return Utils::execute("sc stop \"$service_name\"");
    }
    
    
    public function _KillService($service_name)
    {
    	$squery = Utils::execute("sc queryex \"$service_name\"");
    	if (!ereg("PID[[:space:]]+: ([0-9]+)", $squery, $regs))
    		throw new Exception("Could not retrieve the PID of the service");
    	else
    		return Utils::execute("taskkill /F /T /PID " . $regs[1]);
    }
    
    public function _EnableService($service_name)
    {
    	return Utils::execute("sc config \"$service_name\" start= auto");
    }
    
    public function _DisableService($service_name)
    {
    	return Utils::execute("sc config \"$service_name\" start= disabled");
    }
    
    public function _GetStatus($service_name)
    {
    	$output = Utils::execute("sc query \"$service_name\"");
    	if (!ereg("STATE[[:space:]]+: [0-9][[:space:]]+([[:alpha:]_]+)", $output, $regs))
    		throw new Exception("Could not parse the service status");
    	else
    	{
    		switch ($regs[1])
    		{
    			case "RUNNING" :
    				return ServiceStatus::RUNNING;
    			case "STOPPED" :
    				return ServiceStatus::STOPPED;
    			case "START_PENDING" :
    				return ServiceStatus::START_PENDING;
    			case "STOP_PENDING" :
    				return ServiceStatus::STOP_PENDING;
    			case "PAUSED" :
    				return ServiceStatus::PAUSED;
    			default :
    				return ServiceStatus::UNKNOWN;
    		}
    	}
    }
}


class _ServiceControl_linux
{
    
    public function _StartService($service_name, $wait)
    {
        // NULL function for now
    }
    
    public function _StopService($service_name, $wait)
    {
        // NULL function for now
    }

    public function _EnableService($service_name)
    {
        // NULL function for now
    }
    
    public function _DisableService($service_name)
    {
        // NULL function for now
    }
 
 	public function _GetStatus()
 	{
 		// NULL function for now
 	}
     
}


?>