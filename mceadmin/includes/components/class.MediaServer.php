<?php

require_once("lib.CallManagerUtils.php");
require_once("class.Component.php");


class MediaServer extends Component
{

    public function __construct()
    {
        parent::__construct();
        $this->mListPage = "media_server_list.php";
        $this->mEditPage = "media_server_list.php";
    }

    public function GetIpAddressConfig()
    {
        $this->GetConfigs();
        return $this->mConfigs['MetreosReserved_Address'];
    }

    public function GetConfigs()
    {
        parent::GetConfigs();
        // Hide some of the configs, as these are (currently) not relevant 
        // as configurable settings
        unset($this->mConfigs['HasMedia']);
        unset($this->mConfigs['MetreosReserved_ConnectionType']);
        return $this->mConfigs;
    }

    public function GetStatusDisplay()
    {
        $status = $this->GetStatus();
        switch ($status)
        {
            case ComponentStatus::ENABLED_RUNNING :
                return "Connected";
            case ComponentStatus::ENABLED_STOPPED :
                return "Not Connected";
            default:
                return ComponentStatus::display($status);
        }
    }
    
    public function IsEnabled()
    {
        $status = $this->GetStatus();
        return ($status == ComponentStatus::ENABLED_RUNNING || $status == ComponentStatus::ENABLED_STOPPED);
    }
    
    public function Enable()
    {
        $this->mDb->Execute("UPDATE mce_components SET status = ? WHERE mce_components_id = ?", array(ComponentStatus::ENABLED_STOPPED, $this->mId));
        CallManagerUtils::refresh_provider(MEDIA_CONTROL_PROVIDER, $this->mErrorHandler);
        return TRUE;
    }
    
    public function Disable()
    {
        $this->mDb->Execute("UPDATE mce_components SET status = ? WHERE mce_components_id = ?", array(ComponentStatus::DISABLED, $this->mId));
        CallManagerUtils::refresh_provider(MEDIA_CONTROL_PROVIDER, $this->mErrorHandler);
        return TRUE;
    }
    
    public function Refresh()
    {
        return TRUE;
    }

    public function Uninstall()
    {
        if (parent::Uninstall())
        {
            $socket = new AppServerInterface();
            if ($socket->Connected())
            {
                $socket->Send( MceUtils::generate_xml_command('RemoveMediaServer') );
                if ($socket->Error())
                {
                    $this->mErrorHandler->Add('Uninstall failed.  Reason: ' . $socket->Error());
                    return FALSE;
                }
            }
            $message = "Media Engine " . $this->GetName() . " has been removed.";
            EventLog::log(LogMessageType::AUDIT, $message, LogMessageId::MEDIA_SERVER_DELETED);
            return TRUE;
        }
        return FALSE;
    }
    
}

?>