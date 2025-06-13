<?php

require_once("class.Component.php");
require_once("class.ProviderExtension.php");


class Provider extends Component
{

    private $mExtensions;

    function __construct()
    {
        parent::__construct();
        $this->mExtensions = array();
        $this->mListPage = "component_list.php?type=" . ComponentType::PROVIDER;
        $this->mEditPage = "edit_provider.php";
    }

    function GetExtensions()
    {
        if (array() == $this->mExtensions)
        {
            $rs = $this->mDb->Execute("SELECT * FROM mce_provider_extensions WHERE mce_components_id = ?", array($this->mId));
            $i = 0;
            while ($extension_data = $rs->FetchRow())
            {
                $this->mExtensions[$i] = new ProviderExtension();
                $this->mExtensions[$i]->BuildWithData($extension_data);
                $this->mExtensions[$i]->SetErrorHandler($this->mErrorHandler);
                ++$i;
            }
        }
        return $this->mExtensions;
    }

    function Enable()
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $socket->SetTimeout(MceConfig::APP_SERVER_PROVIDER_WAIT);
            $socket->Send( MceUtils::generate_xml_command('EnableProvider', array('ComponentName' => $this->mData['name'])) );
            if ($socket->Error())
            {
                $this->mErrorHandler->Add($socket->Error());
                return FALSE;
            }
        }
        else
        {
            $this->mDb->Execute("UPDATE mce_components SET status = ? WHERE mce_components_id = ?", array(ComponentStatus::ENABLED_STOPPED, $this->mId));
        }
        EventLog::log(LogMessageType::AUDIT, 'Provider Enabled', LogMessageId::PROVIDER_ENABLED, $this->GetName());
        return TRUE;
    }

    function Disable()
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $socket->SetTimeout(MceConfig::APP_SERVER_PROVIDER_WAIT); 
            $socket->Send(MceUtils::generate_xml_command('DisableProvider', array('ComponentName' => $this->mData['name'])) );
            if ($socket->Error())
            {
                $this->mErrorHandler->Add($socket->Error());
                return FALSE;
            }
        }
        else
        {
            $this->mDb->Execute("UPDATE mce_components SET status = ? WHERE mce_components_id = ?", array(ComponentStatus::DISABLED, $this->mId));
        }
        EventLog::log(LogMessageType::AUDIT, 'Provider Disabled', LogMessageId::PROVIDER_DISABLED, $this->GetName());            
        return TRUE;
    }

    function Uninstall()
    {
        $socket = new AppServerInterface();
        $socket->SetTimeout(MceConfig::APP_SERVER_PROVIDER_WAIT);
        if ($socket->Connected())
        {
            $socket->Send( MceUtils::generate_xml_command('UninstallProvider', array('ComponentName' => $this->mData['name'])) );
        }
        
        if ($socket->Error())
        {
            $this->mErrorHandler->Add($socket->Error());
            return FALSE;
        }
        else
        {
        	try
        	{
        		parent::Uninstall();
        	}
        	catch (Exception $e)
        	{
        		$this->mErrorHandler->Add("A database error occurred.  Provider could not be uninstalled.");
				ErrorLog::raw_log(print_r($e, TRUE));
        		return FALSE;
        	}	
        }
        
        EventLog::log(LogMessageType::AUDIT, "Provider " . $this->GetName() . " was uninstalled.", LogMessageId::PROVIDER_UNINSTALLED);
        return TRUE;
    }

}

?>