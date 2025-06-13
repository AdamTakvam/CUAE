<?php

require_once("init.php");
require_once("class.ConfigHandler.php");

define('MAX_LOG_FILE_LENGTH_ID',    26);
define('MAX_LOG_FILES_ID',          27);

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();

$cfg = new ConfigHandler();
$cfg->SetId(MAX_LOG_FILE_LENGTH_ID);
$cfg->SetErrorHandler($errors);
$cfg->Build();
$configs[$cfg->GetName()] = $cfg;

$cfg2 = new ConfigHandler();
$cfg2->SetId(MAX_LOG_FILES_ID);
$cfg2->SetErrorHandler($errors);
$cfg2->Build();
$configs[$cfg2->GetName()] = $cfg2;

$title = "Log Server Configuration";



if ($_POST['cancel'])
    Utils::redirect('main.php');

if ($_POST['update'])
{

    foreach ($configs as $key => $config)
    {
        $config->Validate($_POST[$key]);
    }

    if ($errors->IsEmpty())
    {
        foreach ($configs as $key => $config)
        {
            $config->Update($_POST[$key]);
        }
        $response = "The log server configurations were updated.";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::COMPONENT_CONFIG_MODIFIED, print_r($_POST, TRUE));
    }

}

// Go through the component configurations and put the info in an array
foreach ($configs as $key => $config_obj)
{
    $config_data[] = array( 'display_name' =>   $config_obj->GetDisplayName(),
                            'fields' =>         $errors->IsEmpty() ? $config_obj->GetFields() : $config_obj->GetFields($_POST[$key]),
                            'description' =>    $config_obj->GetDescription());
}


$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array("<a href='main.php'>Main Control Panel</a>",$title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('_configs', $config_data);
$page->Display('logserver.tpl');

?>