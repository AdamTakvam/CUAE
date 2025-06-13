<?php

/* This page is for managing device in an IETF SIP Device Pool.  IETF SIP Devices have 
   a set of properties different from regular Call Manager devices, so this contains
   specialized handling for those devices.
 */

require_once("init.php");
require_once("components/class.IetfSipDevicePool.php");
require_once("class.PageLogic.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

Utils::require_req_var('id');

$id = $_REQUEST['id'];
$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$p = $_REQUEST['p'];
$search = $_REQUEST['s_search'] ? Utils::safe_unserialize($_REQUEST['s_search']) : $_REQUEST['search'];

$db = new MceDb();
$pl = new PageLogic();

$dp = new IetfSipDevicePool();
$dp->SetId($id);
$dp->SetErrorHandler($errors);
$dp->Build();


$title = $dp->GetName();
$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = '<a href="telephony.php">Telephony Servers</a>';
$breadcrumbs[] = '<a href="' .  $dp->mListPage . '">IETF SIP Domain: ' . $dp->GetDomain() . '</a>';
$breadcrumbs[] = ComponentType::describe(ComponentType::IETF_SIP_DEVICE_POOL) . ': ' . htmlspecialchars($title);


// ** User submits a change in the device pool contents **

if ($_POST['add_device'])
{
    if (Utils::is_blank($_POST['add_username']))
        $errors->Add('Username is required for a new device');
    if (Utils::is_blank($_POST['add_password']))
        $errors->Add('Password is required for a new device');
    if ($_POST['add_password'] <> $_POST['add_password_verify'])
        $errors->Add('Password for the new device could not be verified');
    
    if ($errors->IsEmpty())
    {
        if ($dp->AddDevice($_POST['add_username'], $_POST['add_password']))
            $response = "The device has been added to the device pool.";
    }
}

if ($_POST['delete'])
{
    $dp->RemoveDevices($_POST['select']);
    $dp->Refresh();
    $response = "Devices were removed from the device pool.";
}

if ($_POST['sub_search'])
{
    $search['mode'] = 1;
}


// ** Retrieve devices in the pool **

$clause[] = "mce_components_id = $id";
$device_count = $db->GetOne("SELECT COUNT(*) FROM mce_ietf_sip_devices WHERE " . implode(' AND ', $clause));
if ($search['mode'])
{
    $clause[] = "username LIKE " . $db->Quote('%' . $search['text'] . '%'); 
    if ($search['status'] > 0)
        $clause[] = "status = " . intval($search['status']);
}
$found_count = $db->GetOne("SELECT COUNT(*) FROM mce_ietf_sip_devices WHERE " . implode(' AND ', $clause));
$pl->SetItemCount($found_count);
$pl->SetCurrentPageNumber($p);
$pl->Calculate();
$pl->AddQueryVar('id', $id);
$pl->AddQueryVar('s_search', Utils::safe_serialize($search));

$devices = $db->GetAll("SELECT * FROM mce_ietf_sip_devices WHERE " . implode(' AND ', $clause) . " " .
                       "ORDER BY mce_ietf_sip_devices_id ASC " . $pl->GetSqlLimit());
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
$page->mTemplate->assign('page_logic', $pl);
$page->mTemplate->assign('search', $search);
$page->mTemplate->assign('statuses', $statuses);
if (!$errors->IsEmpty())
{
    $page->mTemplate->assign('add_device_name', $_POST['add_device_name']);
    $page->mTemplate->assign('device_count', $_POST['device_count']);
}
$page->mTemplate->assign('current_count', $device_count);
$page->mTemplate->assign('devices', $devices);
$page->mTemplate->assign('p', $p);

$page->Display("ietf_sip_device_pool_manage.tpl");
?>