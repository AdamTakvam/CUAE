<?php

require_once("common.php");
require_once("class.ApplicationScript.php");
require_once("class.TelephonyManagerExtensions.php");
require_once("lib.MediaServerUtils.php");
require_once("lib.ApplicationConfigUtils.php");


class ApplicationPartition
{

    private $mDb;
    private $mId;
    private $mData;
    private $mConfigs;
    private $mScripts;
    private $mParentApp;

    protected $mErrorHandler;

    function __construct()
    {
        $this->mDb = new MceDb();
        $this->mId = NULL;
        $this->mData = array();
        $this->mConfigs = array();
        $this->mScripts = array();
        $this->mParentApp = NULL;
        $this->mErrorHandler = NULL;
    }

    function SetId($id)
    {
        if (NULL == $this->mId)
        {
            $this->mId = $id;
            return TRUE;
        }
        return FALSE;
    }

    function SetErrorHandler(ErrorHandler $eh)
    {
        $this->mErrorHandler = $eh;
        if (NULL != $this->mParentApp) { $this->mParentApp->SetErrorHandler($this->mErrorHandler); }
    }

    function SetParentApp(Application $app)
    {
        $this->mParentApp = $app;
    }

    function Build()
    {
        if (NULL != $this->mId)
        {
            $data = $this->mDb->GetRow("SELECT * FROM mce_application_partitions WHERE mce_application_partitions_id = ?", array($this->mId));
            return $this->BuildWithData($data);
        }
        return FALSE;
    }

    function BuildWithData($data)
    {
        $this->mId = $data['mce_application_partitions_id'];
        $this->mData = $data;
        if (NULL == $this->mParentApp)
        {
            $this->mParentApp = new Application();
            if (NULL != $this->mErrorHandler) { $this->mParentApp->SetErrorHandler($this->mErrorHandler); }
            $this->mParentApp->SetId($this->mData['mce_components_id']);
            $this->mParentApp->Build();
        }
        return TRUE;
    }

    function GetConfigs()
    {
        if (array() == $this->mConfigs)
        {
            // Get configurations from parent Application
            $app_configs = $this->mParentApp->GetConfigs();

            $query  = "SELECT * FROM mce_config_entries ";
            $query .= "LEFT JOIN mce_config_entry_metas USING (mce_config_entry_metas_id) ";
            $query .= "WHERE mce_application_partitions_id = ?";

            $rs = $this->mDb->Execute($query, array($this->mId));
            $my_configs = array();
            while ($config_row = $rs->FetchRow())
            {
                $my_configs[$config_row['name']] = new ConfigHandler();
                $my_configs[$config_row['name']]->BuildWithData($config_row);
                if (NULL != $this->mErrorHandler)
                {
                    $my_configs[$config_row['name']]->SetErrorHandler($this->mErrorHandler);
                }
            }

            // Overwrite Application configs with Partition configs
            $this->mConfigs = array_merge($app_configs, $my_configs);
        }
        return $this->mConfigs;
    }

    function GetScripts()
    {
        $scripts = $this->mParentApp->GetScriptInfo();
        for ($i = 0; $i < sizeof($scripts); ++$i)
        {
            $script = new ApplicationScript();
            $script->SetPartitionId($this->mId);
            $script->BuildWithData($scripts[$i]);
            $this->mScripts[$script->GetId()] = clone $script;
        }
        return $this->mScripts;
    }

    function GetParentApplication()
    {
        return $this->mParentApp;
    }

    function GetId()
    {
        return $this->mId;
    }

    function GetName()
    {
        return $this->mData['name'];
    }

    function GetDescription()
    {
        return $this->mData['description'];
    }

    function IsEnabled()
    {
        return $this->mData['enabled'];
    }

    function GetCallRouteGroupId()
    {
        return $this->mData['mce_call_route_group_id'];
    }

    function GetAlarmGroupId()
    {
        return $this->mData['mce_alarm_group_id'];
    }

    function GetMediaResourceGroupId()
    {
        return $this->mData['mce_media_resource_group_id'];
    }
    
    function GetPreferredCodec()
    {
        return $this->mData['preferred_codec'];
    }

    function GetUseEarlyMedia()
    {
        return $this->mData['use_early_media'];
    }
    
    function GetLocale()
    {
        return $this->mData['locale'];
    }
        
    function Delete()
    {
        if (NULL != $this->mId)
        {
        	$this->mDb->StartTrans();
            // Delete the trigger parameter values
            $param_ids = $this->mDb->GetCol("SELECT mce_application_script_trigger_parameters_id FROM mce_application_script_trigger_parameters " .
                                            "WHERE mce_application_partitions_id = ?", array($this->mId));
            foreach ($param_ids as $param_id)
            {
                $this->mDb->Execute("DELETE FROM mce_trigger_parameter_values WHERE mce_application_script_trigger_parameters_id = ?", array($param_id));
            }
            $this->mDb->Execute("DELETE FROM mce_application_script_trigger_parameters WHERE mce_application_partitions_id = ?", array($this->mId));

            // Delete the configuration entries
            $entry_ids = $this->mDb->GetCol("SELECT mce_config_entries_id FROM mce_config_entries WHERE mce_application_partitions_id = ?", array($this->mId));
            foreach ($entry_ids as $entry_id)
            {
                $this->mDb->Execute("DELETE FROM mce_config_values WHERE mce_config_entries_id = ?", array($entry_id));
            }
            $this->mDb->Execute("DELETE FROM mce_config_entries WHERE mce_application_partitions_id = ?", array($this->mId));

            // Finally, delete the parition itself
            $this->mDb->Execute("DELETE FROM mce_application_partitions WHERE mce_application_partitions_id = ?", array($this->mId));

            // Log it
            EventLog::log(LogMessageType::AUDIT,
                          $this->GetName() . " partition of " . $this->mParentApp->GetName() . " deleted",
                          LogMessageId::APPLICATION_PARTITION_DELETED);

			$this->mDb->CompleteTrans();

            // Notify App Server
            $asi = new AppServerInterface();
            if ($asi->Connected())
            {
                $params = array('ComponentType' => ComponentType::display(ComponentType::CORE),
                                'ComponentName' => 'Router',
                                'ApplicationName' => $this->mParentApp->GetName(FALSE));
                $asi->Send( MceUtils::generate_xml_command('RefreshConfiguration', $params));
            }
            if ($asi->Error()) { $this->mErrorHandler->Add($asi->Error()); }
            MediaServerUtils::clear_mrg_cache($this->mErrorHandler);
            $tm_ex = new TelephonyManagerExtensions();
            if (!$tm_ex->ClearCrgCache())
                $this->ErrorHandler->Add($tm_ex->GetError());
            
            return TRUE;
        }
        return FALSE;
    }

}

?>