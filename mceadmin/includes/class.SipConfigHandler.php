<?php

/* SipConfigHandler is a child class of ConfigHandler which is specially made to handle 
   ...
 */
 
require_once "class.ConfigHandler.php";

class SipConfigHandler extends ConfigHandler
{

    public function BuildWithData($data)
    // Builds the configuration object.  If it is any of the special case configurations,
    // it pulls the proper options for that configuration from the database.
    {
        switch($data['name'])
        {
            case "MetreosReserved_OutboundProxyId" :
                $query = "SELECT * FROM mce_sip_domain_members JOIN mce_sip_outbound_proxies USING (mce_sip_domains_id) " .
                         "WHERE mce_components_id = ?";
                $proxies = $this->mDb->GetAll($query, array($data['mce_components_id']));
                $options['None'] = 0;
                foreach ($proxies as $proxy)
                {
                    $options[$proxy['ip_address']] = $proxy['mce_sip_outbound_proxies_id'];
                }
                $this->mType = FormatType::CONFIG_NUMBER;
                break;
                
            default :
                return parent::BuildWithData($data);
        }

        $this->mConfig = new CustomEnumConfig();
        $this->mConfig->SetEnum($options);
        $this->mConfig->BuildWithData($data);
        return $this->mConfig->FetchValues();
    }

    public function GetFields($values = NULL)
    {
        if ($this->GetType() == FormatType::CONFIG_PASSWORD)
        {
            return "(Not displayed)";
        }
        else
        {
            return $this->mConfig->AssembleValues($this->mConfig->FetchValues());
        }
    }

}

?>