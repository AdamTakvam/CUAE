<?php

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.Application.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);

$app = new Application();
$app->SetId($_REQUEST['id']);
$app->Build();


$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($app);

$scripts = $app->GetScriptInfo();
$partitions = $app->GetPartitions();

foreach ($partitions as $part)
{
    $parts[] = array(   'id'            => $part->GetId(), 
                        'name'          => $part->GetName(), 
                        'description'   => $part->GetDescription());
}

$c_handler->HandleActions();

if ($_POST['disable'])
{
    if ($app->Disable())
    {
        $response = "The application " . $app->GetName() . " was disabled.";
        $c_handler->SetResponseMessage($response);
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::APPLICATION_DISABLED);
    }
}

if ($_POST['enable'])
{
    if ($app->Enable())
    {
        $response = "The application " . $app->GetName() . " was enabled.";
        $c_handler->SetResponseMessage($response);
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::APPLICATION_ENABLED);
    }
}

$c_handler->AddTemplateVar('enabled', ($app->GetStatus() == ComponentStatus::ENABLED_RUNNING || $app->GetStatus() == ComponentStatus::ENABLED_STOPPED));
$c_handler->AddTemplateVar('scripts', $scripts);
$c_handler->AddTemplateVar('partitions', $parts);
$c_handler->SetResponseMessage($response);
$c_handler->Display("edit_app.tpl");

?>