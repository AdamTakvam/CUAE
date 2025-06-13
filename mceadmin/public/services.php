<?php

require_once("init.php");
require_once("class.ServiceControl.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();

$title = "Service Control";
$errors = new ErrorHandler();


if ($_POST['restart_console'])
{
    $page = new Layout();
    $page->SetPageTitle('Console Restarting');
    $page->TurnOffNavigation();
    $page->Display('restart_message.tpl');

    SystemConfig::store_config(SystemConfig::APACHE_NEEDS_RESTART,'0');
    EventLog::log(LogMessageType::AUDIT, "Webserver restarted", LogMessageId::CONSOLE_REBOOT);
    Utils::background_execute(SCRIPTS_ROOT, "restart_apache.bat",'');
    exit();    
}

if ($_POST['start'] || $_POST['stop'] || $_POST['restart'] || $_POST['enable'] || $_POST['disable'] || $_POST['kill'])
{
    $id = Utils::get_first_array_key($_POST['start']);
    $id = $id ? $id : Utils::get_first_array_key($_POST['stop']);
    $id = $id ? $id : Utils::get_first_array_key($_POST['restart']);
    $id = $id ? $id : Utils::get_first_array_key($_POST['enable']);
    $id = $id ? $id : Utils::get_first_array_key($_POST['disable']);
    $id = $id ? $id : Utils::get_first_array_key($_POST['kill']);
    $service = new ServiceControl();
    $service->SetId($id);
    
    $db->StartTrans();
    try
    {
        if ($_POST['start'])
        {
            $service->Start();
            $response = "Sent start command for " . $service->GetName();
        }
        else if ($_POST['stop'])
        {
            $service->Stop();
            $response = "Sent stop command for " . $service->GetName();
        }
        else if ($_POST['restart'])
        {
            $service->Restart();
            $response = "Attempting to restart " . $service->GetName();
        }
        else if ($_POST['kill'])
        {
            $service->Kill();
            $response = "Killed process for " . $service->GetName();
        }
        else if ($_POST['disable'])
        {
        	$service->Disable();
            $response = $service->GetName() . " disabled";
        }
        else if ($_POST['enable'])
        {
        	$service->Enable();
            $response = $service->GetName() . " enabled";
        }
    }
    catch (Exception $e)
    {
        $message = "This action failed.";
        if (MceConfig::DEV_MODE)
            $message .= $e->GetMessage();
        $errors->Add($message);
    }
    $db->CompleteTrans();
}


// -- RETRIEVE SERVICES --

$s_ids = $db->GetCol("SELECT mce_services_id FROM mce_services ORDER BY mce_services_id ASC");
foreach ($s_ids as $id)
{
	$serv = new ServiceControl();
	$serv->SetId($id);
	
	try
	{
		$services[] = array(
						'id'			=> $serv->GetId(),
						'name'			=> $serv->GetName(),
						'description'	=> $serv->GetDescription(),
						'status'		=> ServiceStatus::display($serv->GetStatus()),
						'enabled'		=> $serv->IsEnabled(),
						);
	}
	catch (Exception $e)
	{
        $services[] = array(
						'id'			=> $serv->GetId(),
						'name'			=> $serv->GetName(),
						'description'	=> $serv->GetDescription(),
						'status'		=> ServiceStatus::display(ServiceStatus::UNKNOWN),
						'enabled'		=> FALSE,
						);                    
	}
}


// -- RENDER PAGE --

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', 'Service Control'));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('services', $services);
$page->Display('services.tpl');

?>