<?php

require_once("init.php");
require_once("components/class.MediaServer.php");
require_once("class.NewConfigHandler.php");
require_once("lib.MediaServerUtils.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);


// *** RETURN TO MEDIA ENGINE LIST ***

if ($_POST['cancel'])
    Utils::redirect("media_server_list.php");


// *** RETRIEVE CONFIG ITEM DATA ***

$conf_metas = ComponentUtils::get_standard_config_metas(ComponentType::MEDIA_SERVER);
for ($i = 0; $i < sizeof($conf_metas); ++$i)
{
    $key = $conf_metas[$i]['name'];
    $new_configs[$key] = new NewConfigHandler($db);
    $new_configs[$key]->SetErrorHandler($errors);
    $new_configs[$key]->BuildWithMetaData($conf_metas[$i]);
}
unset($new_configs['HasMedia']);


// *** CREATE MEDIA ENGINE COMPONENT ***

if ($_POST['add'])
{

    if (empty($_POST['add_name']))
        $errors->Add("No name was specified.");
    foreach ($new_configs as $key => $obj)
    {
        $obj->Validate($_POST[$key]);
    }
    // Check IP addresses of other media servers
    $sql =  "SELECT COUNT(ce.mce_config_entries_id) " .
            "FROM mce_config_entries AS ce, mce_config_entry_metas AS cem, mce_config_values AS cv " .
            "WHERE ce.mce_config_entries_id = cv.mce_config_entries_id AND ce.mce_config_entry_metas_id = cem.mce_config_entry_metas_id " .
            "AND cem.component_type = ? AND cem.name = ? AND cv.value = ?";
    $count = $db->GetOne($sql, array(ComponentType::MEDIA_SERVER, 'MetreosReserved_Address', $_POST['MetreosReserved_Address']));
    if ($count > 0)
        $errors->Add("A media engine with that address already exists.");

    $socket = new AppServerInterface();
    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        $db->Execute("INSERT INTO mce_components (name, type, status) VALUES (?, ?, ?)",
                     array($_POST['add_name'], ComponentType::MEDIA_SERVER, ComponentStatus::ENABLED_STOPPED));
        $id = $db->Insert_ID();
        foreach ($new_configs as $key => $obj)
        {
            $obj->Create($id, $_POST[$key]);
        }

        $db->Execute("INSERT INTO mce_component_group_members (mce_component_groups_id, mce_components_id) VALUES (?,?)",
                     array($_POST['add_to_group'], $id));

        if (MceUtils::is_app_server_running())
            $socket->Send(MceUtils::generate_xml_command('AddMediaServer'));
        MediaServerUtils::clear_mrg_cache($errors);

        $response = "Media Engine $_POST[add_name] added";

        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::MEDIA_SERVER_ADDED, print_r($_POST,TRUE));
        $db->CompleteTrans();

        Utils::redirect("media_server_list.php?s_response=" . Utils::safe_serialize($response));
    }
}


// *** PREP DATA FOR DISPLAY ***

foreach ($new_configs as $key => $cfg_object)
{
    $configs[] = array( 'name'              => $key,
                        'display_name'      => $cfg_object->GetDisplayName(),
                        'fields'            => $errors->IsEmpty() ? $cfg_object->GetFields() : $cfg_object->GetFields($_POST[$key]),
                        'description'       => $cfg_object->GetDescription(),
                        'meta_description'  => $cfg_object->GetMetaDescription(),
                      );
}


// *** RENDER PAGE ***

$title = "Add Media Engine";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', '<a href="media_server_list.php">Media Engines</a>', htmlspecialchars($title)));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('add_name', $_POST['add_name']);
$page->mTemplate->assign('groups', ComponentUtils::get_groups_of_type(GroupType::MEDIA_RESOURCE_GROUP));
$page->mTemplate->assign('configs', $configs);
$page->Display('media_server_add.tpl');

?>