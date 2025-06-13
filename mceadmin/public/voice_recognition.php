<?php

require_once("init.php");
require_once("class.VoiceRecognitionManager.php");


// *** INITIALIZE OBJECTS AND VARIABLES ***

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();

$title = "Speech Recognition Configuration";


// *** HANDLE CONFIGURATION CHANGES ***

if ($_POST['vr_add'])
{
    if (!Utils::is_pos_int($_POST['vr_port_add'],TRUE))
        $errors->Add('Invalid port number for new speech recognition server');
    if (Utils::is_blank($_POST['vr_host_add']))
        $errors->Add('No host defined for new speech recognition server');
    
    if ($errors->IsEmpty())
    {
        $list = VoiceRecognitionManager::read_server_list();
        $list[] = $data = array('host' => $_POST['vr_host_add'], 'port' => $_POST['vr_port_add']);
        VoiceRecognitionManager::store_server_list($list);
        $response = "Speech recognition server added.";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::VR_SERVER_ADD, print_r($data,TRUE));
    }
}

if ($_POST['vr_delete'])
{
    $index = Utils::get_first_array_key($_POST['vr_delete']);
    $list = VoiceRecognitionManager::read_server_list();
    $data = $list[$index];
    unset($list[$index]);
    VoiceRecognitionManager::store_server_list($list);
    $response = "Speech recognition server removed.";
    EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::VR_SERVER_REMOVE, print_r($data,TRUE));    
}

if ($_POST['vrl_add'])
{
    if (!Utils::is_pos_int($_POST['vrl_port_add'],TRUE))
        $errors->Add('Invalid port number for new speech recognition license server');
    if (Utils::is_blank($_POST['vrl_host_add']))
        $errors->Add('No host defined for new speech recognition license server');
    
    if ($errors->IsEmpty())
    {
        $list = VoiceRecognitionManager::read_license_server_list();
        $list[] = $data = array('host' => $_POST['vrl_host_add'], 'port' => $_POST['vrl_port_add']);
        VoiceRecognitionManager::store_license_server_list($list);
        $response = "Speech recognition license server added.";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::VR_LICENSE_SERVER_ADD, print_r($data,TRUE));
    }
}

if ($_POST['vrl_delete'])
{
    $index = Utils::get_first_array_key($_POST['vrl_delete']);
    $list = VoiceRecognitionManager::read_license_server_list();
    $data = $list[$index];
    unset($list[$index]);
    VoiceRecognitionManager::store_license_server_list($list);
    $response = "Speech recognition license server removed.";
    EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::VR_LICENSE_SERVER_REMOVE, print_r($data,TRUE));
}


// *** RENDER PAGE ***

$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = $title;

if (!Utils::is_blank($response))
    $response .= " Please restart the media engine for this change to take effect.";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
try
{
    $page->mTemplate->Assign('vrservers', VoiceRecognitionManager::read_server_list());
    $page->mTemplate->Assign('vrlservers', VoiceRecognitionManager::read_license_server_list());
    if (!$errors->IsEmpty())
        $page->mTemplate->Assign($_POST);
    $page->Display('voice_recognition.tpl');
}
catch (Exception $e)
{
    $page->Display('voice_recognition_error.tpl');
}

?>