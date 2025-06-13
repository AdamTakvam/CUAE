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


// If config is for a partition, get the partition information.
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


// Get some stats on the values of the table
if ($_POST['values'])
{
    $values = $_POST['values'];
    $rows = sizeof($values);
    $cols = sizeof($values[0]);
}


// Handle actions
if ($_POST['add_row'])
{
    $values[] = array_fill(0,$cols,'');
    $response = "Row added";
}
else if ($_POST['add_col'])
{
    for ($x = 0; $x < $rows; $x++)
    {
        $values[$x][] = '';
    }
    $response = "Column added";
}
else if ($_POST['delete_row'])
{
    $d_row = Utils::get_first_array_key($_POST['delete_row']);
    unset($values[$d_row]);
    $response = "Row deleted";
}
else if ($_POST['delete_col'])
{
    $d_col = Utils::get_first_array_key($_POST['delete_col']);
    for ($x = 0; $x < $rows; $x++)
    {
        unset($values[$x][$d_col]);
    }
    $response = "Column deleted";
}
else
{
    if (empty($_POST['values']))
        $_POST['values'] = array();

    if ($_POST['update'] && is_array($_POST['values']))
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


    // Construct Table
    $sizes = $db->GetRow("SELECT MAX(ordinal_row) AS max_row, MAX(key_column) AS max_column FROM mce_config_values WHERE mce_config_entries_id = ?", array($id));
    $raw = $db->GetAll("SELECT * FROM mce_config_values WHERE mce_config_entries_id = ? ORDER BY ordinal_row ASC, key_column ASC", array($id));
    $i = 0;
    for ($x = 0 ; $x <= $sizes['max_row']; ++$x)
    {
        for ($y = 0; $y <= $sizes['max_column']; ++$y)
        {
            $values[$x][$y] = $raw[$i]["value"];
            ++$i;
        }
    }
}


// Finally, display the table.
// If the table has nothing defined, display it as one empty cell.
if (empty($values))
    $values = array(array(''));

$template_vars = array( 'id'                => $id,
                        'part_id'           => $part_id,
                        'description'       => $c_handler->GetDescription(),
                        'meta_description'  => $c_handler->GetMetaDescription(),
                        'values'            => $values,
                      );

$title = $c_handler->GetDisplayName();

$page = new Layout();
$page->TurnOffNavigation();
$page->SetPageTitle($title);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($template_vars);
$page->Display('config_edit_table.tpl');

?>
