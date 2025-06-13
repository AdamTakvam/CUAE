<?php

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.Provider.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();

$provider = new Provider();
$provider->SetId($_REQUEST['id']);
$provider->Build();

$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($provider);

$extensions = $provider->GetExtensions();

foreach ($extensions as $ext)
{
    $params = $ext->GetParameters();
    $param_fields = array();
    foreach ($params as $param)
    {
        $param_fields[] = array('name' => $param->GetName(),
                                'description' => $param->GetDescription(),
                                'field' => $param->GetFields());
    }
    $ext_array[] = array(   'id' => $ext->GetId(),
                            'name' => $ext->GetName(),
                            'description' => $ext->GetDescription(),
                            'wait_for_completion' => $ext->GetCompletionStatus(),
                            'params' => $param_fields);
}

$c_handler->HandleActions();

if ($_POST['disable'])
{
    if ($provider->Disable())
    {
        $provider->Build();
        $c_handler->SetResponseMessage("This provider has been disabled.");
    }
}

if ($_POST['enable'])
{
    if ($provider->Enable())
    {
        $provider->Build();
        $c_handler->SetResponseMessage("This provider has been enabled.");
    }
}

if ($_POST['invoke_ext'])
{
    $user_params = array();
    $i = 0;
    while ($extensions[$i]->GetId() != $_POST['extension_id'])
    {
        ++$i;
    }
    
    $i_params = $extensions[$i]->GetParameters();
    foreach ($i_params as $i_param)
    {
        $param_name = $i_param->GetName();
        if ($i_param->Validate($_POST[$param_name]))
        {
            $user_params[$param_name] = $i_param->Assemble($_POST[$param_name]);
        }
    }
    
    if ($errors->IsEmpty() && $extensions[$i]->InvokeExtension($user_params))
    {
        $c_handler->SetResponseMessage("The " . $extensions[$i]->GetName() . " extension has been invoked.");
    }
}

$c_handler->AddTemplateVar('enabled', ($provider->GetStatus() == ComponentStatus::ENABLED_RUNNING || $provider->GetStatus() == ComponentStatus::ENABLED_STOPPED));
$c_handler->AddTemplateVar('extensions', $ext_array);
$c_handler->Display("edit_provider.tpl");

?>