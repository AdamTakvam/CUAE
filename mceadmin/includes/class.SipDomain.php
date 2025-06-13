<?php

require_once("common.php");
require_once("components/class.SipDomainMember.php");
require_once("class.NewSipConfigHandler.php");
require_once("class.ComponentGroup.php");
require_once("lib.CallManagerUtils.php");
require_once("lib.ComponentUtils.php");

define('__CLASS_SIPDOMAIN_TRUNK_INTERFACE_REGISTRAR_NAME__',    'MetreosReserved_RegistrarIpAddress');

class SipDomain
{

    // ** CLASS MEMBERS **

    private $mId;
    private $mData;
    private $mErrorHandler;
    
    
    // ** PUBLIC METHODS **
    
    public function __construct()
    {
        $this->mId = NULL;
        $this->mData = array();
        $this->mErrorHandler = NULL;
    }
    
    
    public function SetId($id)
    {
        if (empty($this->mId))
        {
            $this->mId = $id;
            return TRUE;
        }
        return FALSE;
    }
    
    
    public function SetErrorHandler(ErrorHandler $eh)
    {
        $this->mErrorHandler = $eh;
    }
    
    
    public function GetId($id)
    {
        return $this->mId;
    }
    
    public function Create($type, $domain, $primaryIp, $secondaryIp, $outboundProxy)
    {
        if ($this->mId > 0 || !empty($this->mData))
            throw new Exception("Cannot create SipDomain because object is already associated with a SipDomain");
        
        $values['type'] = $type;
        $values['domain_name'] = $domain;
        $values['primary_registrar'] = $primaryIp;
        $values['secondary_registrar'] = $secondaryIp;
        
        $db = new MceDb();
        $db->StartTrans();
        $db->MakeAndExecuteInsert('mce_sip_domains',$values);
        $this->mId = $db->Insert_ID();
        
        $proxy = array();
        if (!empty($outboundProxy))
        {
            $proxy['mce_sip_domains_id'] = $this->mId;
            $proxy['ip_address'] = $outboundProxy;
            $db->MakeAndExecuteInsert('mce_sip_outbound_proxies',$proxy);
        }
        
        EventLog::log(LogMessageType::AUDIT, 'SIP Domain Created', LogMessageId::SIP_DOMAIN_CREATED, print_r(array_merge($values,$proxy), TRUE));        
        $db->CompleteTrans();
        if ($type == SipDomainType::CISCO)
            $this->CreateTrunkInterface($domain, $primaryIp);
        $this->Refresh();
        return $this->mId;
    }
    
    
    public function GetData()
    {
        if (empty($this->mData))
            $this->Build();
                   
        return $this->mData;
    }
    
    public function GetType()
    {
        $data = $this->GetData();
        return $data['type'];
    }
    
    public function Update($data)
    {
        $db = new MceDb();
        $db->StartTrans();
        $update = Utils::extract_array_using_keys(array('domain_name','primary_registrar','secondary_registrar'),$data);
        $db->MakeAndExecuteUpdate('mce_sip_domains','mce_sip_domains_id = ' . $db->Quote($this->mId), $update);
        
        if ($data['outbound_proxy'])
        {
            $count = $db->GetOne("SELECT COUNT(*) FROM mce_sip_outbound_proxies WHERE mce_sip_domains_id = ?", array($this->mId));
            $proxy['mce_sip_domains_id'] = $this->mId;
            $proxy['ip_address'] = $data['outbound_proxy'];
            if ($count > 0)
                $db->MakeAndExecuteUpdate('mce_sip_outbound_proxies','mce_sip_domains_id = ' . $db->Quote($this->mId), $proxy);
            else
                $db->MakeAndExecuteInsert('mce_sip_outbound_proxies',$proxy);
        }
        else
            $db->Execute("DELETE FROM mce_sip_outbound_proxies WHERE mce_sip_domains_id = ?", array($this->mId));
            
        $type = $this->GetType();
        if ($type == SipDomainType::CISCO)
        {
            $trunk_id = $db->GetOne("SELECT mce_components.mce_components_id " .
                                    "FROM mce_sip_domain_members LEFT JOIN mce_components USING (mce_components_id) " .
                                    "WHERE type = ? AND mce_sip_domains_id = ?", array(ComponentType::SIP_TRUNK_INTERFACE,$this->mId));
            $trunk = new SipDomainMember();
            $trunk->SetId($trunk_id);
            $trunk->SetErrorHandler($this->mErrorHandler);
            $trunk->Build();
            $trunk_cfgs = $trunk->GetConfigs();
            $trunk_cfgs[__CLASS_SIPDOMAIN_TRUNK_INTERFACE_REGISTRAR_NAME__]->Update($data['primary_registrar']);
        }
        
        EventLog::log(LogMessageType::AUDIT, 'SIP Domain Updated', LogMessageId::SIP_DOMAIN_UPDATED, print_r($data,TRUE));
        $db->CompleteTrans();
        
        $this->Refresh();
        return TRUE;
    }
    
    public function Delete()
    {
        if (empty($this->mId))
            throw new Exception("Cannot delete nonexistent SIP Domain");
            
        $db = new MceDb();
        $db->StartTrans();
        // Delete outbound proxies
        $db->Execute("DELETE FROM mce_sip_outbound_proxies WHERE mce_sip_domains_id = ?", 
                     array($this->mId));
       
        // Delete members
        $members = $db->GetCol("SELECT mce_components_id FROM mce_sip_domain_members WHERE mce_sip_domains_id = ?",
                                array($this->mId));
        foreach ($members as $member_id)
        {
            try
            {
                $sm = new SipDomainMember();
                $sm->SetErrorHandler($this->mErrorHandler);
                $sm->SetId($member_id);
                $sm->Build();
                $sm->Uninstall();
            }
            catch (Exception $ex)
            {
                // More than likely, a member is already uninstalled
                // but the reference is still in the database.
                ErrorLog::raw_log(print_r($ex, TRUE));
            }
        }
        
        // Delete the domain
        $db->Execute("DELETE FROM mce_sip_domains WHERE mce_sip_domains_id = ?", array($this->mId));
        $db->CompleteTrans();
        
        if (!$this->mErrorHandler->IsEmpty())
        {
            EventLog::log(LogMessageType::AUDIT, 'SIP Domain Removed', LogMessageId::SIP_DOMAIN_REMOVED, "id = " . $this->mId);
            $this->Refresh();
            return TRUE;
        }
        else
        {
            return FALSE;
        }
    }
    
    
    public function Refresh()
    {
        if (MceUtils::is_app_server_running())
            CallManagerUtils::refresh_provider(SIP_PROVIDER, $this->mErrorHandler);
    }
    
    
    // ** PRIVATE METHODS **
    
    private function Build()
    {
        if (empty($this->mId))
            throw new Exception("Cannot build SipDomain object because there is no ID number");
        
        $db = new MceDb();
        $this->mData = $db->GetRow("SELECT * FROM mce_sip_domains WHERE mce_sip_domains_id = ?", array($this->mId));
        $this->mData['outbound_proxy'] = $db->GetOne("SELECT ip_address FROM mce_sip_outbound_proxies WHERE mce_sip_domains_id = ?", array($this->mId));
        return TRUE;
    }
    
    
    private function CreateTrunkInterface($dn, $ip)
    {
        if (empty($this->mId))
            throw new Exception("Cannot create trunk interface for nonexistent SIP Domain");
        
        $db = new MceDb();
        $this->Build();
        
        // Make trunk interface
        $db->StartTrans();
        $trunk['name'] = $this->mData['domain_name'];
        $trunk['display_name'] = $this->mData['domain_name'] . " Trunk Interface";
        $trunk['type'] = ComponentType::SIP_TRUNK_INTERFACE;
        $trunk['status'] = ComponentStatus::ENABLED_RUNNING;
        $log_values = $trunk;
        $db->MakeAndExecuteInsert('mce_components',$trunk);
        $log_values['trunk_id'] = $trunk_id = $db->Insert_ID();
        $log_values['device_name'] = $dn;

        // Make trunk interface configs
        $conf_metas = ComponentUtils::get_standard_config_metas(ComponentType::SIP_TRUNK_INTERFACE);
        foreach ($conf_metas as $meta)
        {
            $key = $meta['name'];
            $new_configs[$key] = new NewSipConfigHandler();
            $new_configs[$key]->SetSipDomainId($this->mId);
            $new_configs[$key]->SetErrorHandler($this->mErrorHandler);
            $new_configs[$key]->BuildWithMetaData($meta);
        }
        $new_configs[__CLASS_SIPDOMAIN_TRUNK_INTERFACE_REGISTRAR_NAME__]->Create($trunk_id, $ip);
        $log_values[__CLASS_SIPDOMAIN_TRUNK_INTERFACE_REGISTRAR_NAME__] = $ip;
        
        // Make trunk interface device
        $db->Execute("INSERT INTO mce_call_manager_devices (device_name, device_type, status, mce_components_id) VALUES (?,?,?,?)",
                     array($dn, DeviceType::SIP_TRUNK, DeviceStatus::ENABLED_STOPPED, $trunk_id));

        // Create association with domain
        $db->Execute("INSERT INTO mce_sip_domain_members (mce_sip_domains_id, mce_components_id) VALUES (?,?)",
                     array($this->mId, $trunk_id));

        // Create association with group
        $log_values['group_id'] = $group_id = ComponentUtils::get_default_group_id_of_type(GroupType::SIP_GROUP);
        $group = new ComponentGroup();
        $group->SetId($group_id);
        $group->Build();
        $group->AddComponent($trunk_id);

        EventLog::log(LogMessageType::AUDIT, 'SIP Trunk Interface Created', LogMessageId::IPT_SERVER_CREATED, print_r($log_values, TRUE));
        
        $db->CompleteTrans();
        return TRUE;
    }

}

?>