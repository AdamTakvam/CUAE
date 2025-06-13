<?php

/* This pages allows the user to configure a new CTI Route Point for a CallManager 
 * cluster.  A route point cannot be created without an existing CTI manager in the 
 * associated CallManager cluster.  The route point is not an actual device pool, but
 * relates to a single device.
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
$cluster_id = $_REQUEST['cluster_id'];


// ** Retrieve the route point's config metadata **

$conf_metas = ComponentUtils::get_standard_config_metas(ComponentType::CTI_ROUTE_POINT);
foreach ($conf_metas as $meta)
{
    $key = $meta['name'];
    $new_configs[$key] = new NewDpConfigHandler();
    $new_configs[$key]->SetErrorHandler($errors);
    $new_configs[$key]->SetCallManagerId($cluster_id);
    $new_configs[$key]->BuildWithMetaData($meta);
}


// ** Attempt to create the route point if requested **

if ($_POST['submit'])
{
    // Validate fields

    if (empty($_POST['name']))
        $errors->Add("The name field is required.");
    if (!eregi('^[[:alnum:]_-]+$', $_POST['dn']))
        $errors->Add("The device name has invalid characters.  It can only contain letters, numbers, underscores, and dashes.");
    foreach ($new_configs as $key => $conf_obj)
    {
        $conf_obj->Validate($_POST[$key]);
    }

    // Create the route point

    if ($errors->IsEmpty())
    {
        $db->StartTrans();

        $db->Execute("INSERT INTO mce_components (name, type, status, version, description) " .
                     "VALUES (?,?,?,?,?)",
                     array($_POST['name'], ComponentType::CTI_ROUTE_POINT, ComponentStatus::ENABLED_RUNNING, '1.0', $_POST['description']));
        $rp_id = $db->Insert_ID();

        // Create the configs
        foreach ($new_configs as $key => $conf_obj)
        {
            $conf_obj->Create($rp_id, $_POST[$key]);
        }

        // Create device
        $db->Execute("INSERT INTO mce_call_manager_devices (mce_components_id, device_name, device_type, status) VALUES (?,?,?,?)",
                     array($rp_id, $_POST['dn'], DeviceType::CTI_ROUTE_POINT, DeviceStatus::ENABLED_STOPPED));

        // Create association with CallManager
        $db->Execute("INSERT INTO mce_call_manager_cluster_members (mce_call_manager_clusters_id, mce_components_id) VALUES (?,?)",
                     array($cluster_id, $rp_id));

        // Create association with group
        if ($_POST['add_to_group'] > 0)
        {
            $group = new ComponentGroup();
            $group->SetId($_POST['add_to_group']);
            $group->Build();
            $group->AddComponent($rp_id);
        }

        $log_values = MceUtils::remove_password_info($_POST);
        EventLog::log(LogMessageType::AUDIT, 'CTI Route Point Created', LogMessageId::IPT_SERVER_CREATED, print_r($log_values, TRUE));

        $db->CompleteTrans();

        // Tell the appserver to update the Tapi provider
        $dp = new DevicePool();
        $dp->SetErrorHandler($errors);
        $dp->SetId($rp_id);
        $dp->Build();
        $dp->Refresh();

        // Clear the call route group cache
        $tm_ex = new TelephonyManagerExtensions();
        $tm_ex->ClearCrgCache();

        Utils::redirect("edit_call_manager.php?id=$cluster_id");
    }
}


// ** Retrieve config information and groups **

if (!$errors->IsEmpty())
{
    unset($_POST['MetreosReserved_Password']);
}

foreach ($new_configs as $key => $config_obj)
{
    $configs[] = array( 'display_name' =>   $config_obj->GetDisplayName(),
                        'fields' =>         $errors->IsEmpty() ? $config_obj->GetFields() : $config_obj->GetFields($_POST[$key]),
                        'description' =>    $config_obj->GetDescription());
}

$groups = ComponentUtils::get_groups_of_type(GroupType::CTI_SERVER_GROUP);


// ** Render and display the page **

$template_vars = array( 'cluster_id'    => $cluster_id,
                        'configs'       => $configs,
                        'groups'        => $groups);

$page = new Layout();
$page->SetPageTitle("Create A CTI Route Point");
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            '<a href="edit_call_manager.php?id=' . $cluster_id . '">Unified Communications Manager</a>',
                            'Add CTI Route Point'));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign($template_vars);
$page->mTemplate->assign($_POST);
$page->Display('add_cti_route_point.tpl');

?>