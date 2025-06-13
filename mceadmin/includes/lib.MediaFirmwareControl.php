<?php

require_once("class.ServiceControl.php");
require_once("lib.Utils.php");


class MediaFirmwareControl
{
    function __construct()
    {
    }

    function IsEnabled()
    {
        $sc = new _ServiceControl_windows();
        if ($sc->_GetStatus(DIALOGIC_SERVICE_NAME) == ServiceStatus::RUNNING)
        {
            $output = Utils::execute('reg query "' . self::KEY . '" /v ' . self::VALUE);
            if (ereg("0x0", $output))
                return TRUE;
        }
        return FALSE;
    }

    function Start()
    {
        $sc = new _ServiceControl_windows();

        if ($sc->_GetStatus(DIALOGIC_SERVICE_NAME) == ServiceStatus::STOPPED)
            $sc->_StartService(DIALOGIC_SERVICE_NAME, SERVICE_WAIT);
    }
    
    function Stop()
    {
        $sc = new _ServiceControl_windows();

        if ($sc->_GetStatus(DIALOGIC_SERVICE_NAME) == ServiceStatus::RUNNING)
            $sc->_StopService(DIALOGIC_SERVICE_NAME, SERVICE_WAIT);
    }

}

?>