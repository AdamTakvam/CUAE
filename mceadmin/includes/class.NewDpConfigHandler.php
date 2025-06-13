<?php

/* NewDpConfigHandler -> NewConfigHandler as DpConfigHandler ->ConfigHandler
 * It needs to pull a list of options from subscribers/CTI managers associated 
 * with a CallManager, so it needs to know which CallManager that is.
 */
 
require_once("class.NewConfigHandler.php");

class NewDpConfigHandler extends NewConfigHandler
{

/* PRIVATE MEMBERS */

    private $mCallManagerId;        // Call manager which is assocaited with the config (via the component)


/* PUBLIC MEMBERS */

    public function SetCallManagerId($id)
    // Set the associated CallManager
    {
        $this->mCallManagerId = $id;
    }

    public function BuildWithMetaData($data)
    // Builds the configuration object.  If it is any of the special case configurations,
    // it pulls the proper options for that configuration from the database.
    {
        switch($data['name'])
        {
            case "MetreosReserved_SecondarySubscriberId" :
            case "MetreosReserved_TertiarySubscriberId" :
            case "MetreosReserved_QuaternarySubscriberId" :
            case "MetreosReserved_SRST" :
                $options["None"] = 0;
            case "MetreosReserved_PrimarySubscriberId" :
                $query = "SELECT * FROM mce_call_manager_cluster_subscribers WHERE mce_call_manager_clusters_id = ?";
                $subs = $this->mDb->GetAll($query, array($this->mCallManagerId));
                foreach ($subs as $sub)
                {
                    $options[$sub['name']] = $sub['mce_call_manager_cluster_subscribers_id'];
                }
                $this->mType = FormatType::CONFIG_NUMBER;
                break;

            case "MetreosReserved_SecondaryCtiManagerId" :
                $options["None"] = 0;
            case "MetreosReserved_PrimaryCtiManagerId" :
                $query = "SELECT * FROM mce_call_manager_cluster_cti_managers WHERE mce_call_manager_clusters_id = ?";
                $subs = $this->mDb->GetAll($query, array($this->mCallManagerId));
                foreach ($subs as $sub)
                {
                    $options[$sub['name']] = $sub['mce_call_manager_cluster_cti_managers_id'];
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