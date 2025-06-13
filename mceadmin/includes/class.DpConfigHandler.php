<?php

/* DpConfigHandler is a child class of ConfigHandler which is specially made to handle 
   DevicePool type component configs.  This sepcial case is needed because the options for
   certain configs (Primary/Secondary Subscrber & Primary/Secondary CTI Manager) has to be
   pulled from a special place in the database.  If it is not any of these special configs,
   it behaves like its parent class.
 */
 
require_once "class.ConfigHandler.php";

class DpConfigHandler extends ConfigHandler
{

    public function BuildWithData($data)
    // Builds the configuration object.  If it is any of the special case configurations,
    // it pulls the proper options for that configuration from the database.
    {
        switch($data['name'])
        {
            case "MetreosReserved_SecondarySubscriberId" :
            case "MetreosReserved_TertiarySubscriberId" :
            case "MetreosReserved_QuaternarySubscriberId" :
            case "MetreosReserved_SRST" :
                $options["None"] = "0";
            case "MetreosReserved_PrimarySubscriberId" :
                $query = "SELECT * FROM mce_call_manager_cluster_members " .
                         "LEFT JOIN mce_call_manager_cluster_subscribers USING (mce_call_manager_clusters_id) " .
                         "WHERE mce_components_id = ?";
                $subs = $this->mDb->GetAll($query, array($data['mce_components_id']));
                foreach ($subs as $sub)
                {
                    $options[$sub['name']] = $sub['mce_call_manager_cluster_subscribers_id'];
                }
                $this->mType = FormatType::CONFIG_NUMBER;
                break;

            case "MetreosReserved_SecondaryCtiManagerId" :
                $options["None"] = "0";
            case "MetreosReserved_PrimaryCtiManagerId" :
                $query = "SELECT * FROM mce_call_manager_cluster_members " .
                         "LEFT JOIN mce_call_manager_cluster_cti_managers USING (mce_call_manager_clusters_id) " .
                         "WHERE mce_components_id = ?";
                $subs = $this->mDb->GetAll($query, array($data['mce_components_id']));
                foreach ($subs as $sub)
                {
                    $options[$sub['name']] = $sub['mce_call_manager_cluster_cti_managers_id'];
                }
                $this->mType = FormatType::CONFIG_NUMBER;
                break;

                
            case "MetreosReserved_OutboundProxyId" :
                $query = "SELECT * FROM mce_sip_domain_members LEFT JOIN mce_sip_outbound_proxies USING (mce_sip_domains_id) " .
                         "WHERE mce_components_id = ?";
                $proxies = $this->mDb->GetAll($query, array($data['mce_components_id']));
                foreach ($proxies as $proxy)
                {
                    $options[$proxy['address']] = $proxy['mce_sip_outbound_proxies_id'];
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