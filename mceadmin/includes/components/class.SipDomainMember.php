<?php

require_once("class.IptServer.php");
require_once("class.SipConfigHandler.php");

class SipDomainMember extends IptServer
{

    private $mSipDomainId;


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

    public function Build()
    {
        parent::Build();
        switch ($this->GetType())
        {
            case ComponentType::SIP_DEVICE_POOL :
                $this->mEditPage = "edit_device_pool.php";
                break;
            case ComponentType::SIP_TRUNK_INTERFACE :
                break;
            default :
                throw new Exception('This component type is not recognized as a Cisco SIP device pool or trunk interface');
        }
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

    public function RemoveDevices($devices)
    {
        if (!is_array($devices))
            $devices = array($devices);
        else
        {
            $sql  = "DELETE FROM mce_call_manager_devices WHERE mce_components_id = ? ";
            $sql .= "AND mce_call_manager_devices_id IN (" . implode(',', array_fill(0, count($devices), '?')) . ")";
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
            $this->mDb->Execute("DELETE FROM mce_call_manager_devices WHERE mce_components_id = ?", array($this->mId));
            $this->mDb->CompleteTrans();
            $this->Refresh();
            return TRUE;
        }
        $this->mDb->CompleteTrans();
        return FALSE;
    }


}

?>