<?php

require_once("class.DevicePool.php");

class CtiDevicePool extends DevicePool
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
                             array($deviceName, DeviceType::CTI_DEVICE, DeviceStatus::ENABLED_STOPPED, $this->mId));
                $this->Refresh();
                EventLog::log(LogMessageType::AUDIT, 'CTI Device Pool contents changed', LogMessageId::IPT_DEVICE_POOL_CHANGED, print_r($_POST, TRUE));
                return TRUE;
            }
            else
                $this->mErrorHandler->Add("This device already exists");
        }
        return FALSE;
    }

    public function ValidateBulkAddDevices($startName, $count)
    {
        if (is_numeric($count))
        {
            $no2register = intval($count);
            if ($no2register < 1 || $no2register > 9999)
                $this->mErrorHandler->Add("Number of devices to register is out of range.");
        }
        else
        {
            $this->mErrorHandler->Add("Number of devices to register is invalid.");
        }
        if (!eregi('^[[:alnum:]_-]+$', $startName))
            $this->mErrorHandler->Add("The device prefix has invalid characters.  It can only contain letters, numbers, underscores, and dashes.");

        return $this->mErrorHandler->IsEmpty();
    }

    public function GetBulkAddConflicts($startName, $count)
    {
        $count = intval($count);
        $old_devices = $this->mDb->GetCol("SELECT device_name FROM mce_call_manager_devices WHERE device_name LIKE ? AND device_type = ?",
                                          array($startName . '%', DeviceType::CTI_DEVICE));
        if (sizeof($old_devices) > 0)
        {
            $new_devices = array();
            for ($i = 1; $i <= $count; ++$i)
            {
                $new_devices[] = $startName . str_pad($i, 4, '0', STR_PAD_LEFT);
            }
            $conflicts = array_intersect($old_devices, $new_devices);
            return $conflicts;
        }
        return NULL;
    }

    public function GetHighestDeviceName($startName)
    {
        return $this->mDb->GetOne("SELECT device_name FROM mce_call_manager_devices WHERE device_name LIKE ? AND device_type = ? " .
                                  "ORDER BY device_name DESC LIMIT 0, 1",
                                  array($startName . '%', DeviceType::CTI_DEVICE));
    }

    public function BulkAddDevices($startName, $count, $exceptions = array())
    {
        $count = intval($count);
        if ($this->mErrorHandler->IsEmpty())
        {
            $new_devices = array();
            for ($i = 1; $i <= $count; ++$i)
            {
                $new_devices[] = $startName . str_pad($i, 4, '0', STR_PAD_LEFT);
            }
            $create_devices = array_diff($new_devices, $exceptions);

            $this->mDb->StartTrans();
            foreach ($create_devices as $device)
            {
                $this->mDb->Execute("INSERT INTO mce_call_manager_devices (device_name, device_type, status, mce_components_id) VALUES (?,?,?,?)",
                            array($device, DeviceType::CTI_DEVICE, DeviceStatus::ENABLED_STOPPED, $this->mId));
            }

            $log_data = $_POST;
            $log_data['created_devices'] = $create_devices;
            EventLog::log(LogMessageType::AUDIT, 'CTI Device Pool contents changed', LogMessageId::IPT_DEVICE_POOL_CHANGED, print_r($log_data, TRUE));
            $this->mDb->CompleteTrans();
            $this->Refresh();

            return TRUE;
        }
        return FALSE;
    }

}

?>