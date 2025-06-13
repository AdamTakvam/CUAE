<?php

require_once("class.DevicePool.php");

class SccpDevicePool extends DevicePool
{

    public function __construct()
    {
        parent::__construct();
    }

    public function AddDevice($deviceName)
    {
        if (strlen($deviceName) != 12 || !Utils::is_hex($deviceName))
            $this->mErrorHandler->Add("MAC address needs to be a 12-digit hexadecimal");
        if ($this->mErrorHandler->IsEmpty())
        {
            $count = $this->mDb->GetOne("SELECT COUNT(*) FROM mce_call_manager_devices WHERE device_name = ?",$deviceName);
            if ($count == 0)
            {
                $this->mDb->Execute("INSERT INTO mce_call_manager_devices (device_name, device_type, status, mce_components_id) VALUES (?,?,?,?)",
                             array($deviceName, DeviceType::SCCP, DeviceStatus::ENABLED_STOPPED, $this->mId));
                $this->Refresh();
                EventLog::log(LogMessageType::AUDIT, 'SCCP Device Pool contents changed', LogMessageId::IPT_DEVICE_POOL_CHANGED, print_r($_POST, TRUE));
                return TRUE;
            }
            else
                $this->mErrorHandler->Add("This device already exists");
        }
        return FALSE;
    }

    public function ValidateBulkAddDevices($startMac, $count)
    {
        if (is_numeric($count))
        {
            $no2register = intval($count);
            if ($no2register < 1 || $no2register > pow(16,6))
                $errors->Add("Number of devices to register is out of range.");
        }
        else
        {
            $this->mErrorHandler->Add("Number of devices to register is invalid.");
        }
        if (strlen($startMac) != 12 || !Utils::is_hex($startMac))
            $this->mErrorHandler->Add("MAC address prefix needs to a be 12-digit hexadecimal");

        return $this->mErrorHandler->IsEmpty();
    }

    public function GetBulkAddConflicts($startMac, $count)
    {
        $count = intval($count);
        $old_devices = $this->mDb->GetCol("SELECT device_name FROM mce_call_manager_devices WHERE device_type = ?",
                                          array(DeviceType::SCCP));
        if (sizeof($old_devices) > 0)
        {
            $new_devices = array();
            $startNum = hexdec($startMac);
            for ($i = 0; $i < $count; ++$i)
            {
                $new_devices[] = strtoupper(str_pad(Utils::big_dechex($startNum + $i), 12, '0', STR_PAD_LEFT));
            }
            $conflicts = array_intersect($old_devices, $new_devices);
            return $conflicts;
        }
        return NULL;
    }

    public function GetHighestDeviceName($startMac)
    {
        return $this->mDb->GetOne("SELECT device_name FROM mce_call_manager_devices WHERE device_type = ? ORDER BY device_name DESC LIMIT 0, 1",
                                  array(DeviceType::SCCP));
    }

    public function BulkAddDevices($startMac, $count, $exceptions = array())
    {
        if ($this->mErrorHandler->IsEmpty())
        {
            $new_devices = array();
            $startNum = hexdec($startMac);
            for ($i = 0; $i < $count; ++$i)
            {
                $new_devices[] = strtoupper(str_pad(Utils::big_dechex($startNum + $i), 12, '0', STR_PAD_LEFT));
            }
            $create_devices = array_diff($new_devices, $exceptions);

            $this->mDb->StartTrans();
            foreach ($create_devices as $device)
            {
                $this->mDb->Execute("INSERT INTO mce_call_manager_devices (device_name, device_type, status, mce_components_id) VALUES (?,?,?,?)",
                             array($device, DeviceType::SCCP, DeviceStatus::ENABLED_STOPPED, $this->mId));
            }

            $log_data = $_POST;
            $log_data['created_devices'] = $create_devices;
            EventLog::log(LogMessageType::AUDIT, 'SCCP Device Pool contents changed', LogMessageId::IPT_DEVICE_POOL_CHANGED, print_r($log_data, TRUE));
            $this->mDb->CompleteTrans();

            $this->Refresh();
            return TRUE;
        }
        return FALSE;
    }

}

?>