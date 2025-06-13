<?php

require_once("common.php");


// I assure you not everything that needs to be here is here.


abstract class CallManagerUtils
{

    static function refresh_provider($provider, ErrorHandler $errors)
    {
        $asi = new AppServerInterface();
        if ($asi->Connected())
        {
            $params = array('ComponentType' => ComponentType::display(ComponentType::PROVIDER), 'ComponentName' => $provider);
            $asi->Send(MceUtils::generate_xml_command('RefreshConfiguration', $params));
        }
        if ($asi->Error())
        {
            $errors->Add($asi->Error());
            return FALSE;
        }
        return TRUE;
    }

}

?>