<?php

require_once("init.php");
require_once("class.ComponentPageHandler.php");
require_once("components/class.IptServer.php");

define('IP_ADDRESS_NAME', 'MetreosReserved_IPAddress');

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();


function validate_properties(ErrorHandler $eh)
{
    global $_POST;
    if (Utils::is_blank($_POST['name']))
        $eh->Add("Name field is blank.");
}

function validate_ip_not_duplicated(ErrorHandler $eh)
{
    global $_POST, $ipt;
    if ($eh->IsEmpty())
    {
        $configs = $ipt->GetConfigs();
        $ip_meta_id = $configs[IP_ADDRESS_NAME]->GetMetaId();
        $ipt_id = $ipt->GetId();
        $ip = $_POST[IP_ADDRESS_NAME];

        $db = new MceDb();
        $conflicts = $db->GetOne("SELECT COUNT(*) FROM mce_config_entries as mce LEFT JOIN mce_config_values USING (mce_config_entries_id) " .
                                 "WHERE value = ? AND mce_config_entry_metas_id = ? AND mce_components_id <> ?", array($ip, $ip_meta_id, $ipt_id));
        if ($conflicts > 0)
        {
            $eh->Add("There is already a " . ComponentType::describe($type) . " with the address $ip .");
            return FALSE;
        }
    }
    return TRUE;
}

function update_properties($id)
{
    global $_POST;
    $db = new MceDb();
    $db->Execute("UPDATE mce_components SET name = ?, description = ? WHERE mce_components_id = ?",
                 array($_POST['name'],$_POST['description'],$id));
}


$ipt = new IptServer();
$ipt->SetId($_REQUEST['id']);
$ipt->Build();

// Hackety hack hack - we want a special warning to show on a config for H.323 Gateways, 
// so it gets special attention.
if ($ipt->GetType() == ComponentType::H323_GATEWAY)
    $template = "edit_h323.tpl";
else
    $template = "edit_ipt.tpl";

$c_handler = new ComponentPageHandler();
$c_handler->SetErrorHandler($errors);
$c_handler->SetComponent($ipt);

$c_handler->AddValidateFunction('validate_properties', array($errors));
$c_handler->AddValidateFunction('validate_ip_not_duplicated', array($errors));
$c_handler->AddUpdateFunction('update_properties', array($_REQUEST['id']));
$c_handler->HandleActions();

// A bit of a hack - if an update is successful, tell the component to rebuild itself
// to retrieve updated values for properties (NOT configs)
if ($_POST['update'] && $errors->IsEmpty())
{
    $ipt->Build();
}

$c_handler->AddTemplateVar('type', $ipt->GetType());
$c_handler->AddTemplateVar('type_display', ComponentType::describe($ipt->GetType()));
$c_handler->Display($template);

?>