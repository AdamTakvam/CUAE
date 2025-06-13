<?php

require_once("init.php");
require_once("components/class.Application.php");
require_once("lib.MediaServerUtils.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);


// *** INITIALIZE VARIABLES AND OBJECTS ***

$id = $_REQUEST['id'];
$response = Utils::safe_unserialize($_REQUEST['s_response']);

$nonfinal_submit = FALSE;
$need_router_refresh = FALSE;

$db = new MceDb();
$errors = new ErrorHandler();

$app_part = new ApplicationPartition();
$app_part->SetErrorHandler($errors);
$app_part->SetId($id);
$app_part->Build();
$app = $app_part->GetParentApplication();


// *** RETURN USER TO APPLICATION PAGE ***

if ($_POST['cancel'])
{
    Utils::redirect("edit_app.php?id=" . $app->GetId());
}


// *** UPDATE APPLICATION PARTITION ***

if ($_POST['update'])
{
    // Validate submission
    $configs = $app_part->GetConfigs();
    foreach ($configs as $key => $config_obj)
    {
        $config_obj->Validate($_POST[$key]);
    }

    // Update configs
    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        $updated = array();
        foreach ($configs as $key => $config_obj)
        {
            if (isset($_POST[$key]))
            {
                // Updating the default partition should update configs on the application
                // and not on the partition level.
                $values = $_POST[$key];
                $updated[$key] = $values;
                if (DEFAULT_PARTITION_NAME == $app_part->GetName())
                    $config_obj->Update($values);
                else
                    $config_obj->Update($values, $id);
            }
        }

        // Update partition specfic attributes
        $db->Execute("UPDATE mce_application_partitions SET mce_call_route_group_id = ?, preferred_codec = ?, locale = ?, " .
                     "mce_media_resource_group_id = ?, enabled = ?, use_early_media = ? " .
                     "WHERE mce_application_partitions_id = ?",
                     array($_POST['call_route_group'], $_POST['preferred_codec'], $_POST['locale'], $_POST['media_resource_group'], $_POST['enabled'], $_POST['use_early_media'], $id));
        $updated2 = Utils::extract_array_using_keys(array('call_route_group','media_resource_group','enabled','use_early_media','preferred_codec','locale'), $_POST);
        $updated = $updated + $updated2;

        EventLog::log(LogMessageType::AUDIT,
                      $app_part->GetName() . " partition of " . $app->GetName() . " was updated",
                      LogMessageId::PARTITION_CONFIG_MODIFIED,
                      print_r($updated, TRUE));
        $db->CompleteTrans();


        // Notify the application server
        $asi = new AppServerInterface();
        if ($asi->Connected())
        {
            $params = array('ComponentType' => ComponentType::display(ComponentType::CORE),
                            'ComponentName' => 'Router',
                            'ApplicationName' => $app->GetName(FALSE));
            $asi->Send(MceUtils::generate_xml_command('RefreshConfiguration', $params));
        }
        if ($asi->Error())
            $errors->Add($asi->Error());
        MediaServerUtils::clear_mrg_cache($errors);
        $tm_ex = new TelephonyManagerExtensions();
        if (!$tm_ex->ClearCrgCache())
            $errors->Add($tm_ex->GetError());


        // Update object with latest values
        $app_part->Build();

        $response = "Configuration has been updated.";
    }
}

// There were errors, so the submitted data is not final.
if (!$errors->IsEmpty())
    $nonfinal_submit = TRUE;


// *** RETRIEVE PARTITION SETTINGS ***

$configs = $app_part->GetConfigs();
foreach ($configs as $conf)
{
    $conf->AddContextParam('partition_id', $id);
}

// Extract data into arrays for the templates
foreach ($configs as $key => $config_obj)
{
    $config_data[] = array( 'display_name'      => $config_obj->GetDisplayName(),
                            'fields'            => $nonfinal_submit ? $config_obj->GetFields($_POST[$key]) : $config_obj->GetFields(),
                            'description'       => $config_obj->GetDescription(),
                            'meta_description'  => $config_obj->GetMetaDescription(),
                          );
}


// *** RETRIEVE SELECTABLE GROUP OPTIONS ***

$crgroups = ComponentUtils::get_call_route_groups();
$mrgroups = ComponentUtils::get_groups_of_type(GroupType::MEDIA_RESOURCE_GROUP);
$sip_groups = array();

foreach ($crgroups as $crg)
{
    if (ComponentUtils::determine_group_type($crg['component_type']) == GroupType::SIP_GROUP)
    {
        $sip_groups[] = $crg['mce_component_groups_id'];
    }
}


// *** RENDER PAGE ***

$template_vars = array( 'name'                      => $app_part->GetName(),
                        'id'                        => $id,
                        'description'               => $app_part->GetDescription(),
                        'enabled'                   => $app_part->IsEnabled(),
                        'crg_id'                    => $app_part->GetCallRouteGroupId(),
                        'mrg_id'                    => $app_part->GetMediaResourceGroupId(),
                        'use_early_media'           => $app_part->GetUseEarlyMedia(),
                        'preferred_codec'           => $app_part->GetPreferredCodec(),
                        'locale'                    => $app_part->GetLocale(),
                        'call_route_groups'         => $crgroups,
                        'media_resource_groups'     => $mrgroups,
                        'sip_groups'                => $sip_groups,
                        'codec_list'                => ApplicationConfigUtils::GetCodecList(),
                        'locale_list'               => $app->GetLocaleList(),
                        'configs'                   => $config_data,
                        'scripts'                   => $app_part->GetScripts(),
                        'is_default_partition'      => $app_part->GetName() == DEFAULT_PARTITION_NAME,
                       );

$page = new Layout();
$page->SetPageTitle($app->GetName() . " Partition: " . $app_part->GetName());
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>',
                            '<a href="component_list.php?type=' . ComponentType::APPLICATION . '">Applications</a>',
                            '<a href="edit_app.php?id=' . $app->GetId() . '">' . htmlspecialchars($app->GetName()) .'</a>',
                            'Partition: ' . htmlspecialchars($app_part->GetName())
                           ));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($template_vars);
$page->Display('edit_app_partition.tpl');

?>