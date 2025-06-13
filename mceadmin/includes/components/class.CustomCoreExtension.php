<?php

require_once("class.CoreExtension.php");

class CustomCoreExtension extends CoreExtension
{

    private $mAddParameters;
    
    public function __construct()
    {
        parent::__construct();
        $this->mAddParameters = array();
    }
    
    public function AddParameter($name, $value)
    {
        $this->mAddParameters[$name] = $value;
    }
    
    public function InvokeExtension($params = array())
    {
        $socket = new AppServerInterface();
        
        if ($socket->Connected())
        {
            $params = array_merge($params, $this->mAddParameters);
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