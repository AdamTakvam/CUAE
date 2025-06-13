<?php

/*  */

require_once("init.php");
require_once("class.NewSipConfigHandler.php");
require_once("class.ComponentGroup.php");
require_once("class.TelephonyManagerExtensions.php");
require_once("components/class.SipDevicePool.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = new ErrorHandler();
Utils::trim_array($_POST);
$id = $_REQUEST['id'];


// ** Retrieve the device pool's config metadata **

$conf_metas = ComponentUtils::get_standard_config_metas(ComponentType::SIP_DEVICE_POOL);
foreach ($conf_metas as $meta)
{
    $key = $meta['name'];
    $new_configs[$key] = new NewSipConfigHandler();
    $new_configs[$key]->SetErrorHandler($errors);
    $new_configs[$key]->SetSipDomainId($id);
    $new_configs[$key]->BuildWithMetaData($meta);
}


// ** Create the device pool if requested **

if ($_POST['submit'])
{
    // Validate fields
    if (empty($_POST['name']))
        $errors->Add("The name field is required.");
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
                     array($_POST['name'], ComponentType::SIP_DEVICE_POOL, ComponentStatus::ENABLED_RUNNING, '1.0', $_POST['description']));
        $dp_id = $db->Insert_ID();

        // Create the configs
        foreach ($new_configs as $key => $conf_obj)
        {
            $conf_obj->Create($dp_id, $_POST[$key]);
        }

        // Create association with CallManager
        $db->Execute("INSERT INTO mce_sip_domain_members (mce_sip_domains_id, mce_components_id) VALUES (?,?)",
                     array($id, $dp_id));

        $log_values = MceUtils::remove_password_info($_POST);
        EventLog::log(LogMessageType::AUDIT, 'SIP Device Pool Created', LogMessageId::IPT_SERVER_CREATED, print_r($log_values, TRUE));

        $db->CompleteTrans();

        // Tell the appserver to update the SIP provider
        $dp = new SipDevicePool();
        $dp->SetErrorHandler($errors);
        $dp->SetId($dp_id);
        $dp->Build();
        $dp->Refresh();

        // Clear the call route group cache
        $tm_ex = new TelephonyManagerExtensions();
        $tm_ex->ClearCrgCache();
        
        Utils::redirect("manage_device_pool.php?id=$dp_id&type=" . ComponentType::SIP_DEVICE_POOL);
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

$template_vars['id']         = $id;
$template_vars['configs']    = $configs;

$page = new Layout();
$page->SetPageTitle("Create A Cisco SIP Device Pool");
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            '<a href="sip_domain_edit.php?id=' . $id . '">Cisco SIP Domain</a>',
                            'Add Cisco SIP Device Pool'));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign($template_vars);
$page->mTemplate->assign($_POST);
$page->Display('sip_device_pool_add.tpl');

?>