<?php

/* 
 */

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.DevicePool.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$db = new MceDb();

if (is_array($_REQUEST['id']))
    $id = Utils::get_first_array_key($_REQUEST['id']);
else
    $id = $_REQUEST['id'];

$dp = new DevicePool();
$dp->SetId($id);
$dp->Build();


// ** Use the component page handler for basic update functions **

$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($dp);

$c_handler->HandleActions();

// Additional variables outside of the handler needed to be added to the template
$c_handler->AddTemplateVar('type', $dp->GetType());
$c_handler->AddTemplateVar('type_display', ComponentType::describe($dp->GetType()));
$c_handler->SetResponseMessage($response);
$c_handler->Display("edit_device_pool.tpl");

?>