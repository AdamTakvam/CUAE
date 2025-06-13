<?php

/* 
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

$conf_metas = ComponentUtils::get_standard_config_metas(ComponentType::MONITORED_CTI_DEVICE_POOL);
foreach ($conf_metas as $meta)
{
    $key = $meta['name'];
    $new_configs[$key] = new NewDpConfigHandler();
    $new_configs[$key]->SetErrorHandler($errors);
    $new_configs[$key]->SetCallManagerId($id);
    $new_configs[$key]->BuildWithMetaData($meta);
}


// ** Create the device pool if requested **

if ($_POST['submit'])
{
    // Validate fields
    if (Utils::is_blank($_POST['name']))
        $errors->Add("The name field is required.");

    // Create the device pool and devices
    if ($errors->IsEmpty())
    {
        $db->StartTrans();

        $db->Execute("INSERT INTO mce_components (name, type, status, version, description) " .
                     "VALUES (?,?,?,?,?)",
                     array($_POST['name'], ComponentType::MONITORED_CTI_DEVICE_POOL, ComponentStatus::ENABLED_RUNNING, '1.0', $_POST['description']));
        $dp_id = $db->Insert_ID();

        // Create the configs
        foreach ($new_configs as $key => $conf_obj)
        {
            $conf_obj->Create($dp_id, $_POST[$key]);
        }

        // Create association with CallManager
        $db->Execute("INSERT INTO mce_call_manager_cluster_members (mce_call_manager_clusters_id, mce_components_id) VALUES (?,?)",
                     array($id, $dp_id));

        $log_values = MceUtils::remove_password_info($_POST);
        EventLog::log(LogMessageType::AUDIT, 'Monitored CTI Device Pool Created', LogMessageId::IPT_SERVER_CREATED, print_r($log_values, TRUE));

        $db->CompleteTrans();

        // Tell the appserver to update the TAPI provider
        $dp = new DevicePool();
        $dp->SetErrorHandler($errors);
        $dp->SetId($dp_id);
        $dp->Build();
        $dp->Refresh();
        
        Utils::redirect("edit_call_manager.php?id=$id");
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


// ** Render and display page **

$template_vars = array( 'id'            => $id,
                        'configs'       => $configs);

$page = new Layout();
$page->SetPageTitle("Create A Monitored CTI Device Pool");
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            '<a href="edit_call_manager.php?id=' . $id . '">Unified Communications Manager</a>',
                            'Add Monitored CTI Device Pool'));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign($template_vars);
$page->mTemplate->assign($_POST);
$page->Display('monitored_cti_device_pool_add.tpl');

?>