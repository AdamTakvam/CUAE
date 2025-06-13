<?php

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.Alarm.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$alarm = new Alarm();
$alarm->SetId($_REQUEST['id']);
$alarm->Build();

$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($alarm);

$c_handler->AddTemplateVar('describe_type', ComponentType::describe($alarm->GetType()));
$c_handler->HandleActions();

if ($_POST['update'] && !MceUtils::is_app_server_running())
{
	if ($alarm->Refresh())
	    $c_handler->SetResponseMessage("The configuration has been updated");
}

$c_handler->Display("edit_alarm.tpl");

?>