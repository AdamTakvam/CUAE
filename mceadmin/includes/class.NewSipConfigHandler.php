<?php

/* NewSipConfigHandler -> NewConfigHandler as SipConfigHandler ->ConfigHandler
 */
 
require_once("class.NewConfigHandler.php");

class NewSipConfigHandler extends NewConfigHandler
{

/* PRIVATE MEMBERS */

    private $mSipDomainId;        // SIP Domain which is assocaited with the config (via the component)


/* PUBLIC MEMBERS */

    public function SetSipDomainId($id)
    // Set the associated CallManager
    {
        $this->mSipDomainId = $id;
    }

    public function BuildWithMetaData($data)
    // Builds the configuration object.  If it is any of the special case configurations,
    // it pulls the proper options for that configuration from the database.
    {
        switch($data['name'])
        {
            case "MetreosReserved_OutboundProxyId" :
                $query = "SELECT * FROM mce_sip_outbound_proxies WHERE mce_sip_domains_id = ?";
                $proxies = $this->mDb->GetAll($query, $this->mSipDomainId);
                $options['None'] = 0;
                foreach ($proxies as $proxy)
                {
                    $options[$proxy['ip_address']] = $proxy['mce_sip_outbound_proxies_id'];
                }
                $this->mType = FormatType::CONFIG_NUMBER;
                break;
                
            default :
                return parent::BuildWithMetaData($data);
        }
        $this->mConfig = new CustomEnumConfig();
        $this->mConfig->SetEnum($options);
        $this->mMetaId = $data["mce_config_entry_metas_id"];
        return $this->mConfig->BuildWithData($data);
    }

}

?>