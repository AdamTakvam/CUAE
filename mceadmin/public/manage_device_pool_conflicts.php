<?php

require_once("init.php");
require_once("components/class.SccpDevicePool.php");
require_once("components/class.CtiDevicePool.php");
require_once("components/class.MonitoredCtiDevicePool.php");
require_once("components/class.SipDevicePool.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

Utils::require_req_var('id');
Utils::require_req_var('type');
Utils::require_req_var('start');
Utils::require_req_var('count');

$id = $_REQUEST['id'];
$type = $_REQUEST['type'];
$start = $_REQUEST['start'];
$count = $_REQUEST['count'];

$db = new MceDb();
$errors = new ErrorHandler();

$class_name = ComponentType::get_class_name($type);
$dp = new $class_name();
$dp->SetId($id);
$dp->SetErrorHandler($errors);
$dp->Build();


// *** RETRIEVE & CALCULATE CONFLICT DATA ***

$conflicts = $dp->GetBulkAddConflicts($start, $count);
$highest_device = $dp->GetHighestDeviceName($start);

switch ($type)
{
    case ComponentType::SCCP_DEVICE_POOL :
    case ComponentType::SIP_DEVICE_POOL :
        $start_number = hexdec($highest_device);
        $new_highest_device = strtoupper(Utils::big_dechex($start_number + $count));
        break;
    case ComponentType::CTI_DEVICE_POOL :
        $highnum = intval(substr($highest_device, -4, 4));
        $new_highest_device = $start . str_pad($highnum + $count, 4, '0', STR_PAD_LEFT);
        break;
    default :
        throw new Exception('Bulk add devices is not supported for this component type');
}

$return_query['id'] = $id;
$return_query['type'] = $type;
$return_url = 'manage_device_pool.php' . Utils::make_query($return_query);


// *** USER CANCELS ***

if ($_POST['cancel'])
{
    Utils::redirect($return_url);
}


// *** USER MAKES CHOICE ON ADDING DEVICES ***

if ($_POST['submit'])
{
    if ($_POST['resolve_method'] == 'overlap')
    {
        $dp->BulkAddDevices($start, $count, $conflicts);
    }

    if ($_POST['resolve_method'] == 'append')
    {
        switch ($type)
        {
            case ComponentType::SCCP_DEVICE_POOL :
            case ComponentType::SIP_DEVICE_POOL :
                $dp->BulkAddDevices(Utils::big_dechex($start_number + 1), $count);
                break;
            case ComponentType::CTI_DEVICE_POOL :
                $dp->BulkAddDevices($start, $highnum + $count, $conflicts);
                break;
            default :
                throw new Exception('Bulk add devices is not supported for this component type');
        }
    }
    
    Utils::redirect($return_url);
}


// *** RENDER PAGE ***

$title = 'Resolve Add Multiple Devices Conflict';

$page = new Layout();
$page->SetPageTitle($title);
$page->TurnOffNavigation();
$page->SetErrorMessage($errors->Dump());

$page->mTemplate->Assign('id', $id);
$page->mTemplate->Assign('type', $type);
$page->mTemplate->Assign('count', $count);
$page->mTemplate->Assign('start', $start);
$page->mTemplate->Assign('conflict_count', sizeof($conflicts));
$page->mTemplate->Assign('highest_device', $highest_device);
$page->mTemplate->Assign('new_highest_device', $new_highest_device);
$page->mTemplate->Assign('type_display', ComponentType::describe($type));

$page->Display('manage_device_pool_conflicts.tpl')

?>