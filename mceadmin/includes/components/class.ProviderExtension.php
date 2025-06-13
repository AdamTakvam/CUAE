<?php

require_once("class.ComponentExtension.php");


class ProviderExtension extends ComponentExtension
{

    public function InvokeExtension($params = array())
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $params['ExtensionName'] = $this->mData['name'];
            $socket->Send( MceUtils::generate_xml_command('InvokeExtension', $params));
            EventLog::log(LogMessageType::AUDIT, 'Provider Extension invoked', LogMessageId::PROVIDER_EXTENSION_INVOKED, $params);
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