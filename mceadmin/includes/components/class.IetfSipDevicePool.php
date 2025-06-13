<?php

require_once("class.IptServer.php");
require_once("class.SipConfigHandler.php");

class IetfSipDevicePool extends IptServer
{

    public function __construct()
    {
        parent::__construct();
        
    }

    public function SetId($id)
    {
        parent::SetId($id);
        $this->mSipDomainId = $this->mDb->GetOne("SELECT mce_sip_domains_id FROM mce_sip_domain_members WHERE mce_components_id = ?",
                                            array($this->mId));
        $this->mEditPage = $this->mListPage = "sip_domain_edit.php?id=" . $this->mSipDomainId;
    }

    public function GetConfigs()
    {
        if (array() == $this->mConfigs)
        {
            $query  = "SELECT * FROM mce_config_entries LEFT JOIN mce_config_entry_metas USING (mce_config_entry_metas_id) ";
            $query .= "WHERE mce_components_id = ? ORDER BY mce_config_entries_id ASC";

            $rs = $this->mDb->Execute($query, array($this->mData['mce_components_id']));
            while ($config_row = $rs->FetchRow())
            {
                $this->mConfigs[$config_row['name']] = new SipConfigHandler();
                $this->mConfigs[$config_row['name']]->BuildWithData($config_row);
                if (NULL != $this->mErrorHandler)
                {
                    $this->mConfigs[$config_row['name']]->SetErrorHandler($this->mErrorHandler);
                }
            }
        }
        return $this->mConfigs;
    }
    
    public function GetDomain()
    {
        $db = new MceDb();
        return $db->GetOne("SELECT domain_name FROM mce_sip_domains WHERE mce_sip_domains_id = ?", array($this->mSipDomainId));
    }
    
    public function AddDevice($username, $password)
    {
        $count = $this->mDb->GetOne("SELECT COUNT(*) FROM mce_ietf_sip_devices WHERE username = ? AND mce_components_id = ?", array($username, $this->mId));
        if ($count == 0)
        {
            $this->mDb->Execute("INSERT INTO mce_ietf_sip_devices (username, password, status, mce_components_id) VALUES (?,?,?,?)",
                         array($username, $password, DeviceStatus::ENABLED_STOPPED, $this->mId));
            $this->Refresh();
            EventLog::log(LogMessageType::AUDIT, 'SIP Device Pool contents changed', LogMessageId::IPT_DEVICE_POOL_CHANGED, print_r(MceUtils::remove_password_info($_POST), TRUE));
            return TRUE;
        }
        else
            $this->mErrorHandler->Add("This device already exists");            
    }
    
    public function RemoveDevices($devices)
    {
        if (!is_array($devices))
            $devices = array($devices);
        else
        {
            $sql  = "DELETE FROM mce_ietf_sip_devices WHERE mce_components_id = ? ";
            $sql .= "AND mce_ietf_sip_devices_id IN (" . implode(',', array_fill(0, count($devices), '?')) . ")";
            $vars = array_merge(array($this->mId), $devices);            
            $this->mDb->Execute($sql, $vars);
        }
        return TRUE;
    }
    
    public function Uninstall()
    {
        $this->mDb->StartTrans();
        if (parent::Uninstall())
        {
            $this->mDb->Execute("DELETE FROM mce_sip_domain_members WHERE mce_components_id = ?", array($this->mId));
            $this->mDb->Execute("DELETE FROM mce_ietf_sip_devices WHERE mce_components_id = ?", array($this->mId));
            $this->mDb->CompleteTrans();
            $this->Refresh();
            return TRUE;
        }
        $this->mDb->CompleteTrans();
        return FALSE;
    }
    
}

?>