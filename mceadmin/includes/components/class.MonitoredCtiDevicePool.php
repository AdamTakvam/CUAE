<?php

require_once("class.DevicePool.php");

class MonitoredCtiDevicePool extends DevicePool
{

    public function __construct()
    {
        parent::__construct();
    }
    
    public function AddDevice($deviceName)
    {
        if (!eregi('^[[:alnum:]_-]+$', $deviceName))
            $this->mErrorHandler->Add("The device prefix has invalid characters.  It can only contain letters, numbers, underscores, and dashes.");
        if ($this->mErrorHandler->IsEmpty())
        {
            $count = $this->mDb->GetOne("SELECT COUNT(*) FROM mce_call_manager_devices WHERE device_name = ?",$deviceName);
            if ($count == 0)
            {
                $this->mDb->Execute("INSERT INTO mce_call_manager_devices (device_name, device_type, status, mce_components_id) VALUES (?,?,?,?)",
                             array($deviceName, DeviceType::MONITORED_CTI_DEVICE, DeviceStatus::ENABLED_STOPPED, $this->mId));
                $this->Refresh();
                EventLog::log(LogMessageType::AUDIT, 'Monitored CTI Device added', LogMessageId::IPT_DEVICE_POOL_CHANGED, print_r($_POST, TRUE));
                return TRUE;
            }
            else
                $this->mErrorHandler->Add("This device already exists");            
        }
        return FALSE;
    }
    
}

?>