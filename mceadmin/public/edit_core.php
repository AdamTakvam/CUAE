<?php

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.Core.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();

$core = new Core();
$core->SetId($_REQUEST['id']);
$core->SetErrorHandler($errors);
$core->Build();

$extensions = $core->GetExtensions();

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
                            'params' => $param_fields);
}


// ** Use ComponentHandler **

$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($core);
$c_handler->IgnoreConfig('LicenseOverageTable');    // LicenseManager config


// ** Handle User Actions **

$c_handler->HandleActions();

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


// ** Render Page **

$c_handler->AddTemplateVar('extensions', $ext_array);
$c_handler->Display("edit_core.tpl");

?>