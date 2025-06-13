<?php

/* The script allows the editing of an existing Device Pool associated with a CallManager cluster.
 */

require_once("init.php");
require_once("components/class.SccpDevicePool.php");
require_once("components/class.CtiDevicePool.php");
require_once("components/class.MonitoredCtiDevicePool.php");
require_once("components/class.SipDevicePool.php");
require_once("class.PageLogic.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

Utils::require_req_var('id');
Utils::require_req_var('type');

$id = $_REQUEST['id'];
$type = $_REQUEST['type'];
$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$p = $_REQUEST['p'];
$search = $_REQUEST['s_search'] ? Utils::safe_unserialize($_REQUEST['s_search']) : $_REQUEST['search'];

$db = new MceDb();
$pl = new PageLogic();

$class_name = ComponentType::get_class_name($type);
$dp = new $class_name();
$dp->SetId($id);
$dp->SetErrorHandler($errors);
$dp->Build();

switch ($type)
{
    case ComponentType::SCCP_DEVICE_POOL :
    case ComponentType::CTI_DEVICE_POOL :
    case ComponentType::MONITORED_CTI_DEVICE_POOL :
        $list_page_name = "Unified Communications Manager";
        break;
    case ComponentType::SIP_DEVICE_POOL :
        $list_page_name = "SIP Domain";
        break;
    default :
        throw new Exception('This component is not a device pool');
}

$title = $dp->GetName();
$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = '<a href="telephony.php">Telephony Servers</a>';
$breadcrumbs[] = '<a href="' .  $dp->mListPage . '">'.$list_page_name.'</a>';
$breadcrumbs[] = ComponentType::describe($type) . ': ' . htmlspecialchars($title);


// ** User submits a change in the device pool contents **

if ($type <> ComponentType::MONITORED_CTI_DEVICE_POOL && $_POST['add_devices'])
{
    if ($dp->ValidateBulkAddDevices($_POST['device_prefix'], $_POST['device_count']))
    {
        $conflicts = $dp->GetBulkAddConflicts($_POST['device_prefix'], $_POST['device_count']);
        if (empty($conflicts))
        {
            $dp->BulkAddDevices($_POST['device_prefix'], $_POST['device_count']);
        }
        else
        {
            $query['id']        = $id;
            $query['type']      = $type;
            $query['start']     = $_POST['device_prefix'];
            $query['count']     = $_POST['device_count'];
            Utils::redirect('manage_device_pool_conflicts.php' . Utils::make_query($query));
        }
        $response = $_POST['device_count'] . " devices have been added to the device pool";
    }
}

if ($_POST['add_one_device'])
{
    if ($dp->AddDevice($_POST['add_device_name']))
    {
        $response = "The device has been added";
        if ($type <> ComponentType::MONITORED_CTI_DEVICE_POOL)
            $response .= " to the device pool.";
    }
}

if ($_POST['delete'])
{
    $dp->RemoveDevices($_POST['select']);
    $dp->Refresh();
    $response = "The devices were removed";
    if ($type <> ComponentType::MONITORED_CTI_DEVICE_POOL)
        $response .= " from the device pool.";
}

if ($_POST['sub_search'])
{
    $search['mode'] = 1;
}


// ** Retrieve devices in the pool **

$clause[] = "mce_components_id = " . intval($id);
$device_count = $db->GetOne("SELECT COUNT(*) FROM mce_call_manager_devices WHERE " . implode(' AND ', $clause));
if ($search['mode'])
{
    $clause[] = "`" . $search['field'] . "` LIKE " . $db->Quote('%' . $search['text'] . '%'); 
    if ($search['status'] > 0)
        $clause[] = "status = " . intval($search['status']);
}
$found_count = $db->GetOne("SELECT COUNT(*) FROM mce_call_manager_devices WHERE " . implode(' AND ', $clause));
$pl->SetItemCount($found_count);
$pl->SetCurrentPageNumber($p);
$pl->Calculate();
$pl->AddQueryVar('id', $id);
$pl->AddQueryVar('type', $type);
$pl->AddQueryVar('s_search', Utils::safe_serialize($search));

$devices = $db->GetAll("SELECT * FROM mce_call_manager_devices WHERE " . implode(' AND ', $clause) . " " .
                       "ORDER BY device_name ASC " . $pl->GetSqlLimit());
for ($x = 0; $x < sizeof($devices); ++$x)
{
    $devices[$x]['status_display'] = DeviceStatus::display($devices[$x]['status']);
}


// ** Use the component page handler for basic update functions **

$statuses[0] = "(Any Status)";
$statuses[DeviceStatus::ENABLED_RUNNING] = DeviceStatus::display(DeviceStatus::ENABLED_RUNNING);
$statuses[DeviceStatus::ENABLED_STOPPED] = DeviceStatus::display(DeviceStatus::ENABLED_STOPPED);
$statuses[DeviceStatus::DISABLED_ERROR] = DeviceStatus::display(DeviceStatus::DISABLED_ERROR);
$statuses[DeviceStatus::DISABLED] = DeviceStatus::display(DeviceStatus::DISABLED);

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetResponseMessage($response);
$page->SetErrorMessage($errors->Dump());

$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('type', $type);
$page->mTemplate->assign('type_display', ComponentType::describe($type));
$page->mTemplate->assign('page_logic', $pl);
$page->mTemplate->assign('search', $search);
$page->mTemplate->assign('statuses', $statuses);
if (!$errors->IsEmpty())
{
    $page->mTemplate->assign('add_device_name', $_POST['add_device_name']);
    $page->mTemplate->assign('device_count', $_POST['device_count']);
    $page->mTemplate->assign('device_prefix', $_POST['device_prefix']);
}
$page->mTemplate->assign('current_count', $device_count);
$page->mTemplate->assign('devices', $devices);
$page->mTemplate->assign('p', $p);

switch ($type)
{
    case ComponentType::SCCP_DEVICE_POOL :
    case ComponentType::SIP_DEVICE_POOL :
        $page->Display("edit_sccp_device_pool.tpl");
        break;
    case ComponentType::CTI_DEVICE_POOL :
        $page->Display("edit_cti_device_pool.tpl");
        break;
    case ComponentType::MONITORED_CTI_DEVICE_POOL :
        $page->Display("monitored_cti_devices_manage.tpl");
        break;
    default :
        throw new Exception('This component is not a device pool');
}
?>