<?php

require_once("common.php");

/* This is similar in functionality to CoreExtension, except this strips
 * away the complications of interacting with the database and user input 
 * and allows the code to easily invoke an extension at its own will.   
 */
 
class TelephonyManagerExtensions
{

    private $mError;


    public function __construct()
    {
        $this->mError = NULL;
    }
    
    public function GetError()
    {
        return $this->mError;
    }

    public function ClearCrgCache()
    {
        return $this->Invoke('ClearCrgCache');
    }
    
    private function Invoke($name, $params = array())
    {
        if (!is_array($params))
        {
            return FALSE;
            $this->SetError('Parameter set not valid');
        }
        
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $socket->Send(MceUtils::generate_xml_command($name, $params));
            $log_values = $params;
            $log_values['extension_name'] = $name;
            EventLog::log(LogMessageType::AUDIT, 'Telephony Manager Extension invoked', LogMessageId::CORE_EXTENSION_INVOKED, $log_values);
        }
        if ($socket->Error())
        {
            $this->SetError('Invoking telephony manager extension failed: ' . $socket->Error());
            return FALSE;
        }
        return TRUE;
    }
    
    private function SetError($error)
    {
        $this->mError = $error;
    }
    
}

?>