<?php

/* This page allows the user to create SCCP Device Pools for a CallManager cluster.  
 * A SCCP Device Pool cannot be created unless a SCCP subscriver exists for the 
 * associated CallManager cluster.  A SCCP Device Pool creates references to a series
 * of devices based on their MAC address.  The first 6 digits of the MAC Address
 * are chosen by the user, and the last 6 digits comes from the hex value of 0 to
 * the number of devices the user specifies.
 */

require_once("init.php");
require_once("class.NewDpConfigHandler.php");
require_once("class.ComponentGroup.php");
require_once("class.TelephonyManagerExtensions.php");
require_once("components/class.DevicePool.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = new ErrorHandler();
Utils::trim_array($_POST);
$id = $_REQUEST['id'];


// ** Retrieve the device pool's config metadata **

$conf_metas = ComponentUtils::get_standard_config_metas(ComponentType::SCCP_DEVICE_POOL);
foreach ($conf_metas as $meta)
{
    $key = $meta['name'];
    $new_configs[$key] = new NewDpConfigHandler();
    $new_configs[$key]->SetErrorHandler($errors);
    $new_configs[$key]->SetCallManagerId($id);
    $new_configs[$key]->BuildWithMetaData($meta);
}


// ** Attempt to create the device pool if requested **

if ($_POST['submit'])
{
    // Validate fields
    if (empty($_POST['name'])) { $errors->Add("The name field is required."); }
    foreach ($new_configs as $key => $conf_obj)
    {
        $conf_obj->Validate($_POST[$key]);
    }

    // Create the device pool and devices
    if ($errors->IsEmpty())
    {
        $db->StartTrans();

        $db->Execute("INSERT INTO mce_components (name, type, status, version, description) " .
                     "VALUES (?,?,?,?,?)",
                     array($_POST['name'], ComponentType::SCCP_DEVICE_POOL, ComponentStatus::ENABLED_RUNNING, '1.0', $_POST['description']));
        $dp_id = $db->Insert_ID();

        // Create the configs
        foreach ($new_configs as $key => $conf_obj)
        {
            $conf_obj->Create($dp_id, $_POST[$key]);
        }

        // Create association with CallManager
        $db->Execute("INSERT INTO mce_call_manager_cluster_members (mce_call_manager_clusters_id, mce_components_id) VALUES (?,?)",
                     array($id, $dp_id));

        // Create association with group
        if ($_POST['add_to_group'] > 0)
        {
            $group = new ComponentGroup();
            $group->SetId($_POST['add_to_group']);
            $group->Build();
            $group->AddComponent($dp_id);
        }

        EventLog::log(LogMessageType::AUDIT, 'Device Pool Created', LogMessageId::IPT_SERVER_CREATED, print_r($_POST, TRUE));

        $db->CompleteTrans();

        // Tell the appserver to update the H.323 provider
        $dp = new DevicePool();
        $dp->SetErrorHandler($errors);
        $dp->SetId($dp_id);
        $dp->Build();
        $dp->Refresh();

        // Clear the call route group cache
        $tm_ex = new TelephonyManagerExtensions();
        $tm_ex->ClearCrgCache();

        Utils::redirect("manage_device_pool.php?id=$dp_id&type=" . ComponentType::SCCP_DEVICE_POOL);
    }
}


// ** Retrieve config information and groups **

foreach ($new_configs as $key => $config_obj)
{
    $configs[] = array( 'display_name' =>   $config_obj->GetDisplayName(),
                        'fields' =>         $errors->IsEmpty() ? $config_obj->GetFields() : $config_obj->GetFields($_POST[$key]),
                        'description' =>    $config_obj->GetDescription());
}

$groups = ComponentUtils::get_groups_of_type(ComponentType::SCCP_DEVICE_POOL);


// ** Render and display the page **

$template_vars = array( 'id'            => $id,
                        'configs'       => $configs,
                        'groups'        => $groups);

$page = new Layout();
$page->SetPageTitle("Create A Device Pool");
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            '<a href="edit_call_manager.php?id=' . $id . '">Unified Communications Manager</a>',
                            'Add SCCP Device Pool'));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign($template_vars);
$page->mTemplate->assign($_POST);
$page->Display('add_sccp_device_pool.tpl');

?>