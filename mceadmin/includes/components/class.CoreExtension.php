<?php

require_once("class.ComponentExtension.php");


class CoreExtension extends ComponentExtension
{
    
    public function InvokeExtension($params = array())
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $socket->Send(MceUtils::generate_xml_command($this->mData['name'], $params));
            $log_vals = $params;
            $log_vals['extension_name'] = $this->mData['name'];
            EventLog::log(LogMessageType::AUDIT, 'Core Extension invoked', LogMessageId::CORE_EXTENSION_INVOKED, $log_vals);
        }
        if ($socket->Error())
        {
            $this->mErrorHandler->Add('Invoking extension failed: ' . $socket->Error());
            return FALSE;
        }
        return TRUE;
    }
    
}

?>