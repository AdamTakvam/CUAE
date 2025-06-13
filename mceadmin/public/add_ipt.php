<?php

require_once("init.php");
require_once("class.NewConfigHandler.php");
require_once("class.ComponentGroup.php");
require_once("class.TelephonyManagerExtensions.php");
require_once("components/class.IptServer.php");

define('IP_ADDRESS_NAME', 'MetreosReserved_IPAddress');


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$db = new MceDb();
$errors = new ErrorHandler();


Utils::trim_array($_POST);
$type = $_REQUEST['type'];


// Get the standards configs
$new_configs = array();
$conf_metas = ComponentUtils::get_standard_config_metas($type);
$groups = ComponentUtils::get_groups_of_type($type);

for ($i = 0; $i < sizeof($conf_metas); ++$i)
{
    $key = $conf_metas[$i]['name'];
    $new_configs[$key] = new NewConfigHandler($db);
    $new_configs[$key]->SetErrorHandler($errors);
    $new_configs[$key]->BuildWithMetaData($conf_metas[$i]);
}


// ** Handle user requests **

if ($_POST['cancel'])
    Utils::redirect("telephony.php");

if ($_POST['add'])
{
    // Validate settings
    if (empty($_POST['name']))
        $errors->Add("Name field is blank.");
    $c_id = $db->GetOne("SELECT mce_components_id FROM mce_components WHERE type = ? AND name = ?",
                        array($type, $_POST['name']));
    if ($c_id > 0)
        $errors->Add("There is already a " . ComponentType::describe($type) . " with the name " . $_POST['name'] . ".");
    
    foreach ($new_configs as $key => $obj)
    {
        $values = $_POST[$key];
        $obj->Validate($values);
    }
    
    if ($errors->IsEmpty())
    {
        $ip = $_POST[IP_ADDRESS_NAME];
        $ip_meta_id = $new_configs[IP_ADDRESS_NAME]->GetMetaId();
        $conflicts = $db->GetOne("SELECT COUNT(*) FROM mce_config_entries as mce LEFT JOIN mce_config_values USING (mce_config_entries_id) " .
                                 "WHERE value = ? AND mce_config_entry_metas_id = ?", array($ip, $ip_meta_id));
        if ($conflicts > 0)
            $errors->Add("There is already a " . ComponentType::describe($type) . " with the IP $ip .");
    }
    
    // Create IPT server component
    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        $db->Execute("INSERT INTO mce_components (name, type, description) VALUES (?, ?, ?)",
                     array($_POST['name'], $type, $_POST['description']));
        $id = $db->Insert_ID();
        foreach ($new_configs as $key => $obj)
        {
            $values = $_POST[$key];
            $obj->Create($id, $values);
        }

         // Create association with group
        if ($_POST['add_to_group'] > 0)
        {
            $group = new ComponentGroup();
            $group->SetId($_POST['add_to_group']);
            $group->Build();
            $group->AddComponent($id);
        }
        $response = ComponentType::describe($type) . ' Created';

        EventLog::log(LogMessageType::AUDIT, 'Telephony Server ' . $response,
                      LogMessageId::IPT_SERVER_CREATED, print_r($_POST, TRUE));
        $db->CompleteTrans();

        // Update the proper provider for the IPT server
        $ipt = new IptServer();
        $ipt->SetErrorHandler($errors);
        $ipt->SetId($id);
        $ipt->Build();
        $ipt->Refresh();

        // Clear the call route group cache
        $tm_ex = new TelephonyManagerExtensions();
        $tm_ex->ClearCrgCache();

        Utils::redirect("telephony.php?s_response=" . Utils::safe_serialize($response));
    }
}


// Ready config information for display
foreach ($new_configs as $key => $cfg_object)
{
    $configs[] = array( 'display_name' =>   $cfg_object->GetDisplayName(),
                        'fields' =>         $errors->IsEmpty() ? $cfg_object->GetFields() : $cfg_object->GetFields($_POST[$key]),
                        'description' =>    $cfg_object->GetDescription());
}


// -- RENDER PAGE --

$page = new Layout();
$page->SetPageTitle("Add " . ComponentType::describe($type));
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            'Add ' . ComponentType::describe($type) . ' Server'));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('configs', $configs);
$page->mTemplate->assign('groups', $groups);
$page->mTemplate->assign('type', $type);
$page->mTemplate->assign('type_display', ComponentType::describe($type));
$page->mTemplate->assign($_POST);
$page->Display('add_ipt.tpl');

?>