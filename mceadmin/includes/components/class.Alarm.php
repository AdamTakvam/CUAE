<?php

require_once("class.Component.php");
require_once("class.StatsServerInterface.php");

class Alarm extends Component
{

    function __construct()
    {
        parent::__construct();
        $this->mListPage = "alarm_list.php";
        $this->mEditPage = "edit_alarm.php";
    }
    
    function Refresh()
    {
        $socket = new StatsServerInterface();
        if ($socket->Connected())
        {
            $socket->Send(MceUtils::generate_xml_command('RefreshConfiguration'));
        }
        if ($socket->Error())
        {
            $this->mErrorHandler->Add($socket->Error() . " Please check to make sure that the Stats server is running.");
            return FALSE;
        }
        return TRUE;
    }

    function Uninstall()
    {
    	$db = new MceDb();
    	$db->StartTrans();
        parent::Uninstall();
        
        $message = ComponentType::describe($this->GetType()) . ' ' . $this->GetName() . ' has been removed.';
        EventLog::log(LogMessageType::AUDIT, $message, LogMessageId::ALARM_REMOVED);
        $db->CompleteTrans();
        $this->Refresh();
    }

}

?>