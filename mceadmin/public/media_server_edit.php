<?php

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.MediaServer.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();


function validate_properties($id, ErrorHandler $eh)
{
    global $_POST;

    if (empty($_POST['name']))
        $eh->Add("No name was specified.");

    // Check IP addresses of other media servers
    $db = new MceDb();
    $sql =  "SELECT COUNT(ce.mce_config_entries_id) " .
            "FROM mce_config_entries AS ce, mce_config_entry_metas AS cem, mce_config_values AS cv " .
            "WHERE ce.mce_config_entries_id = cv.mce_config_entries_id AND ce.mce_config_entry_metas_id = cem.mce_config_entry_metas_id " .
            "AND cem.component_type = ? AND cem.name = ? AND cv.value = ? AND ce.mce_components_id <> ?";
    $count = $db->GetOne($sql, array(ComponentType::MEDIA_SERVER, 'MetreosReserved_Address', $_POST['MetreosReserved_Address'], $id));
    if ($count > 0)
        $eh->Add("A media engine with that address already exists.");
}

function update_properties($id)
{
    global $_POST;
    $db = new MceDb();
    $db->Execute("UPDATE mce_components SET name = ? WHERE mce_components_id = ?",
                 array($_POST['name'],$id));
}


$ms = new MediaServer();
$ms->SetId($_REQUEST['id']);
$ms->Build();

$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($ms);

$c_handler->AddValidateFunction('validate_properties', array($_REQUEST['id'], $errors));
$c_handler->AddUpdateFunction('update_properties', array($_REQUEST['id']));
$c_handler->HandleActions();

// A bit of a hack - if an update is successful, tell the component to rebuild itself
// to retrieve updated values for properties (NOT configs)
if ($_POST['update'] && $errors->IsEmpty())
    $ms->Build();

$c_handler->Display("media_server_edit.tpl");

?>