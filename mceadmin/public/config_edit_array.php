<?php

require_once("init.php");
require_once("class.ConfigHandler.php");
require_once("lib.ComponentUtils.php");
Utils::require_directory("components");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$id = $_REQUEST['id'];
$part_id = $_REQUEST['part_id'] ? $_REQUEST['part_id'] : NULL;

$db = new MceDb();
$errors = new ErrorHandler();

Utils::trim_array($_POST);


if (!empty($part_id))
{
    // Check to see if this is the config for the partition
    // Sometimes the edit button will call the app level config when
    // a partition level one exists - this gets around that.
    $ep_id = ComponentUtils::get_existing_partition_config($id, $part_id);
    if (!empty($ep_id))
        $id = $ep_id;
}

$c_handler = new ConfigHandler();
$c_handler->SetId($id);
$c_handler->SetErrorHandler($errors);
$c_handler->Build();

if (!empty($part_id))
{
    // Check to see if the partition is a default partition.
    // If it is, then no need to specify it.
    $app_part = new ApplicationPartition();
    $app_part->SetErrorHandler($errors);
    $app_part->SetId($part_id);
    $app_part->Build();
    if (DEFAULT_PARTITION_NAME == $app_part->GetName())
        unset($part_id);
}


if ($_POST['add'])
{
    $values = $_POST['values'];
    if (!empty($_POST['add_value']))
    {
        $values[]['value'] = $_POST['add_value'];
        $response = "Value added";
    }
    else
    {
        $errors->Add("Blank values are not allowed.");
    }
}
else if ($_POST['delete'])
{
    $d_count = Utils::get_first_array_key($_POST['delete']);
    $values = $_POST['values'];
    unset($values[$d_count]);
    $response = "Value deleted";
}
else
{
    if (empty($_POST['values']))
        $_POST['values'] = array();
    if ($_POST['done'] && is_array($_POST['values']))
    {
        $db->StartTrans();
        $c_handler->Update($_POST['values'], $part_id);
        $id = $c_handler->GetId();
        $response = "Update successful";

        $log_message_id = $part_id ? LogMessageId::PARTITION_CONFIG_MODIFIED : LogMessageId::COMPONENT_CONFIG_MODIFIED;
        EventLog::log(LogMessageType::AUDIT, $c_handler->GetName() . " config was updated.",
                      $log_message_id, $_POST);
        $db->CompleteTrans();

        // Retrieve component and refresh
        $c_id = $c_handler->GetComponentId();
        $c_type = $db->GetOne("SELECT type FROM mce_components WHERE mce_components_id = ?", array($c_id));
        $c_class = ComponentType::get_class_name($c_type);
        $comp = new $c_class;
        $comp->SetErrorHandler($errors);
        $comp->SetId($c_id);
        $comp->Build();
        $comp->Refresh();
    }
    $values = ComponentUtils::get_config_values($id);
}

$title = $c_handler->GetDisplayName();

$template_vars = array( 'id'                => $id,
                        'part_id'           => $part_id,
                        'description'       => $c_handler->GetDescription(),
                        'meta_description'  => $c_handler->GetMetaDescription(),
                        'values'            => $values,
                      );

$page = new Layout();
$page->TurnOffNavigation();
$page->SetPageTitle($title);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($template_vars);
$page->Display('config_edit_array.tpl');

?>
