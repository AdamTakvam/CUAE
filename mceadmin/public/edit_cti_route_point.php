<?php

/* This page if for editing  CTI Route Points associated with a CallManager cluster.
 * This is a very simple page that simply relies on the ComponentPageHandler class
 * to update its configurations.
 */

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.DevicePool.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();

if (is_array($_REQUEST['id']))
    $id = Utils::get_first_array_key($_REQUEST['id']);
else
    $id = $_REQUEST['id'];


// ** Get device pool object and let the page handler do the dirty work **

$dp = new DevicePool();
$dp->SetId($id);
$dp->Build();

$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($dp);

$c_handler->HandleActions();

$c_handler->AddTemplateVar('type', $dp->GetType());
$c_handler->AddTemplateVar('type_display', ComponentType::describe($dp->GetType()));
$c_handler->Display("edit_cti_route_point.tpl");

?>