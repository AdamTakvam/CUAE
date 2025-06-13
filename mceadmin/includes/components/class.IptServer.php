<?php

require_once("class.Component.php");
require_once("class.TelephonyManagerExtensions.php");


class IptServer extends Component
{

    public function __construct()
    {
        parent::__construct();
        $this->mListPage = "telephony.php";
        $this->mEditPage = "edit_ipt.php";
    }

    public function Uninstall()
    {
        $this->mDb->StartTrans();
        if (parent::Uninstall())
        {
            $message = ComponentType::describe($this->GetType()) . ' ' . $this->GetName() . ' has been removed.';
            EventLog::log(LogMessageType::AUDIT, $message, LogMessageId::IPT_SERVER_REMOVED);
            $this->mDb->CompleteTrans();
            $this->Refresh();
            $tm_ex = new TelephonyManagerExtensions();
            $tm_ex->ClearCrgCache();            
            return TRUE;
        }
        $this->mDb->CompleteTrans();
        return FALSE;
    }

    public function Refresh()
    {
        $asi = new AppServerInterface();
        if ($asi->Connected())
        {
            $type = $this->GetType();
            switch ($type)
            {
                case ComponentType::CTI_DEVICE_POOL :
                case ComponentType::MONITORED_CTI_DEVICE_POOL:
                case ComponentType::CTI_ROUTE_POINT :
                    $provider = JTAPI_PROVIDER;
                    break;
                case ComponentType::H323_GATEWAY :
                    $provider = H323_PROVIDER;
                    break;
                case ComponentType::SCCP_DEVICE_POOL :
                    $provider = SCCP_PROVIDER;
                    break;
                case ComponentType::SIP_DEVICE_POOL :
                case ComponentType::SIP_TRUNK_INTERFACE :
                case ComponentTYpe::IETF_SIP_DEVICE_POOL :
                    $provider = SIP_PROVIDER;
                    break;
                default:
                    throw new Exception ('This telephony server is unknown or not yet supported.');
            }

            $command = array('ComponentType'    => ComponentType::display(ComponentType::PROVIDER),
                             'ComponentName'    => $provider);
            $asi->Send(MceUtils::generate_xml_command('RefreshConfiguration', $command));
        }
        if ($asi->Error())
        {
            $this->mErrorHandler->Add($asi->Error());
            return FALSE;
        }
        return TRUE;
    }
    
}

?>