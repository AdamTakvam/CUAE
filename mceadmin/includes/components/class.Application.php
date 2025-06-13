<?php

require_once("common.php");

require_once("class.Component.php");
require_once("class.ApplicationPartition.php");


class Application extends Component
{

    private $mPartitions;
    private $mScripts;
    private $mLocaleList;

    function __construct()
    {
        parent::__construct();
        $this->mPartitions = array();
        $this->mScripts = array();
        $this->mLocaleList = array();
        $this->mListPage = "component_list.php?type=" . ComponentType::APPLICATION;
        $this->mEditPage = "edit_app.php";
    }

    function GetConfigs()
    {
        if (empty($this->mConfigs))
        {
	        parent::GetConfigs();
	        
            if (array_key_exists('LocaleList', $this->mConfigs))
            {
    	        // Hide the locale list from the app configuration page, but store it for later...
    	        $list = $this->mConfigs['LocaleList']->GetValues();
    	        unset($this->mConfigs['LocaleList']);
                if (empty($this->mLocaleList))
                {
        	        foreach ($list as $item)
        	        {
        	            $this->mLocaleList[] = $item['value'];
        	        }
                    sort($this->mLocaleList);
                }
            }
            
            if (array_key_exists('StringTable', $this->mConfigs))
            {
    	        // Hide the string table for now (?)
    	        unset($this->mConfigs['StringTable']);
            }
        }
        
        return $this->mConfigs;
    }
    
    function GetPartitions()
    {
        if (array() == $this->mPartitions)
        {
            $rs = $this->mDb->Execute("SELECT * FROM mce_application_partitions WHERE mce_components_id = ? ORDER BY mce_application_partitions_id ASC", array($this->mId));
            while ($partition_data = $rs->FetchRow())
            {
                $id = $partition_data['mce_application_partitions_id'];
                $this->mPartitions[$id] = new ApplicationPartition();
                $this->mPartitions[$id]->SetParentApp($this);
                if (NULL != $this->mErrorHandler) { $this->mPartitions[$id]->SetErrorHandler($this->mErrorHandler); }
                $this->mPartitions[$id]->BuildWithData($partition_data);
            }
        }
        return $this->mPartitions;
    }

    function GetScriptInfo()
    {
        if (array() == $this->mScripts)
        {
            $this->mScripts = $this->mDb->GetAll("SELECT * FROM mce_application_scripts WHERE mce_components_id = ?", array($this->mId));
        }
        return $this->mScripts;
    }

    function GetLocaleList()
    {
        if (empty($this->mConfigs))
        {
            $this->GetConfigs();
        }
        return $this->mLocaleList;
    }
    
    function GetDefaultPartition()
    {
        $this->GetPartitions();
        foreach ($this->mPartitions as $part)
        {
            if (DEFAULT_PARTITION_NAME == $part->GetName()) return $part;
        }
        throw new Exception("Default partition for application " . $this->GetName() . " not found.");
    }

    function Enable()
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $socket->Send( MceUtils::generate_xml_command('EnableApplication', array('ComponentName' => $this->mData['name'])) );
        }
        if ($socket->Error())
        {
            $this->mErrorHandler->Add($socket->Error());
            return FALSE;
        }
        $this->Build();
        return TRUE;
    }

    function Disable()
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $socket->Send( MceUtils::generate_xml_command('DisableApplication', array('ComponentName' => $this->mData['name'])) );
        }
        if ($socket->Error())
        {
            $this->mErrorHandler->Add($socket->Error());
            return FALSE;
        }
        $this->Build();
        return TRUE;
    }

    function Uninstall()
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $socket->Send( MceUtils::generate_xml_command('UninstallApplication', array('ComponentName' => $this->mData['name'])) );
        }
        if ($socket->Error())
        {
            $this->mErrorHandler->Add($socket->Error());
            return FALSE;
        }
        EventLog::log(LogMessageType::AUDIT, "Application " . $this->GetName() . " uninstalled.", LogMessageId::APPLICATION_UNINSTALLED);
        return TRUE;
    }

}

?>