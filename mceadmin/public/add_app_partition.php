<?php

require_once("init.php");
require_once("components/class.Application.php");
require_once("lib.MediaServerUtils.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);


// *** INITIALIZE VALUES AND OBJECTS ***

$id = $_REQUEST['id'];

$nonfinal_submit = FALSE;
$errors = new ErrorHandler();
$app = new Application();

$app->SetErrorHandler($errors);
$app->SetId($id);
$app->Build();

$default_part = $app->GetDefaultPartition();
$scripts = $default_part->GetScripts();
$configs = $default_part->GetConfigs();
$default_locale = $default_part->GetLocale();
foreach ($configs as $conf)
{
    $conf->AddContextParam('partition_id', 0);
}


// *** CREATE PARTITION ***

if ($_POST['create'])
{
    $db = new MceDb();

    // Input validation
    if (empty($_POST['name']))
        $errors->Add("Please enter a name.");
    else if ($_POST['name'] == DEFAULT_PARTITION_NAME)
        $errors->Add("Cannot name the partition '" . DEFAULT_PARTITION_NAME . "'");

    if (is_null($_POST['enabled']))
        $errors->Add("Please select whether this partition will be enabled or disabled.");

    foreach ($configs as $config_obj)
    {
        $config_obj->Validate($_POST[$config_obj->GetName()]);
    }

    // If there are no errors in the user input
    if ($errors->IsEmpty())
    {
        $db->StartTrans();

        // Create a new application partition
        $db->Execute("INSERT INTO mce_application_partitions " .
                     "(mce_components_id, name, description, enabled, use_early_media, created_timestamp, preferred_codec, " .
                     "locale, mce_call_route_group_id, mce_media_resource_group_id) " .
                     "VALUES (?, ?, ?, ?, ?, NOW(), ?, ?, ?, ?)",
                     array($_POST['id'], $_POST['name'], $_POST['description'], $_POST['enabled'], $_POST['use_early_media'],
                           $_POST['preferred_codec'], $_POST['locale'], $_POST['call_route_group'], $_POST['media_resource_group']));
        $part_id = $db->Insert_ID();

        // Update/create config entries
        foreach ($configs as $config_obj)
        {
            $key = $config_obj->GetName();
            if (isset($_POST[$key]))
                $config_obj->Update($_POST[$key], $part_id);
        }

        // Create script trigger parameters
        foreach ($scripts as $script)
        {
            $params = $script->GetTriggerParameters();
            foreach ($params as $param)
            {
                $db->Execute("INSERT INTO mce_application_script_trigger_parameters " .
                             "(name, mce_application_scripts_id, mce_application_partitions_id) " .
                             "VALUES (?, ?, ?)",
                             array($param['name'], $script->GetId(), $part_id));
            }
        }

        // Log action
        EventLog::log(LogMessageType::AUDIT,
              $_POST['name'] . " partition created for " . $app->GetName(),
              LogMessageId::APPLICATION_PARTITION_CREATED,
              print_r($_POST, TRUE));

        $db->CompleteTrans();

        // Notify application server
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

        // Tell application server to clear various route group caches
        MediaServerUtils::clear_mrg_cache($errors);
        $tm_ex = new TelephonyManagerExtensions();
        if (!$tm_ex->ClearCrgCache())
            $errors->Add($tm_ex->GetError());

        // Done
        $response = "A new partition has been created.  Please edit the trigger parameters for each script in the new partition.";
        Utils::redirect("edit_app_partition.php?id=$part_id&s_response=" . Utils::safe_serialize($response));
    }
    else
    {
        $nonfinal_submit = TRUE;
    }
}


// *** RETRIEVE PARTITION SETTINGS ***

foreach ($configs as $key => $config_obj)
{
    $config_data[] = array( 'display_name'      => $config_obj->GetDisplayName(),
                            'fields'            => $nonfinal_submit ? $config_obj->GetFields($_POST[$key]) : $config_obj->GetFields(),
                            'description'       => $config_obj->GetDescription(),
                            'meta_description'  => $config_obj->GetMetaDescription(),
                          );
}

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

$page = new Layout();
$page->SetPageTitle("Creating Partition for " . $app->GetName());
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>',
                            '<a href="component_list.php?service=app">Applications</a>',
                            '<a href="edit_app.php?id=' . $id . '">' . htmlspecialchars($app->GetName()) . '</a>',
                            'Create Partition'));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('id', $id);
$page->mTemplate->assign('configs', $config_data);
$page->mTemplate->assign('scripts', $script_data);
$page->mTemplate->assign('sip_groups', $sip_groups);
$page->mTemplate->assign('call_route_groups', $crgroups);
$page->mTemplate->assign('media_resource_groups', $mrgroups);
$page->mTemplate->assign('codec_list', ApplicationConfigUtils::GetCodecList());
$page->mTemplate->assign('locale_list', $app->GetLocaleList());
$page->mTemplate->assign('locale', $default_locale);
if (isset($_POST))
{
	$page->mTemplate->assign('set_enabled', isset($_POST['enabled']));
	$page->mTemplate->assign('set_uem', isset($_POST['use_early_media']));
	$page->mTemplate->assign($_POST);
}
$page->Display("add_app_partition.tpl")

?>