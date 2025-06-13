<?php

require_once("init.php");
require_once("components/class.Application.php");


// ** Set Up Objects & Variables **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

Utils::trim_array($_POST);

$script_id = $_REQUEST['script_id'];
$part_id = $_REQUEST['part_id'];

$db = new MceDb();
$errors = new ErrorHandler();

$app_part = new ApplicationPartition();
$app_part->SetErrorHandler($errors);
$app_part->SetId($part_id);
$app_part->Build();
$app = $app_part->GetParentApplication();

$script = new ApplicationScript();
$script->SetId($script_id);
$script->SetPartitionId($part_id);
$script->Build();


// ** Supporting Functions **

function check_conflicting_values($event_type, $param_name, $value, $value_id = 0)
{
    global $errors;

    if (!$errors->IsEmpty())
        return FALSE;
        
    $db = new MceDb();
    $count = $db->GetOne("SELECT COUNT(*) FROM mce_trigger_parameter_values AS stpv, " .
                         "mce_application_script_trigger_parameters AS stp, " .
                         "mce_application_scripts AS s " .
                         "WHERE " .
                         "stpv.mce_application_script_trigger_parameters_id = stp.mce_application_script_trigger_parameters_id " .
                         "AND stp.mce_application_scripts_id = s.mce_application_scripts_id " .
                         "AND s.event_type = ? AND stp.name = ? AND stpv.value = ?" .
                         "AND stpv.mce_trigger_parameter_values_id <> ?",
                         array($event_type, $param_name, $value, $value_id));
    if ($count > 0)
    {
        $errors->Add('This value is already set for a similar parameter of a script of this event type, probably in another partition or application.');
        return FALSE;
    }
    return TRUE;
}

function check_conflicting_names($param_name)
{
    global $part_id, $script_id, $errors;
    
    if (!$errors->IsEmpty())
        return FALSE;
        
    $db = new MceDb();
    
    $count = $db->GetOne("SELECT COUNT(*) FROM mce_application_script_trigger_parameters " .
                         "WHERE name = ? AND mce_application_partitions_id = ? AND mce_application_scripts_id = ?",
                         array($param_name, $part_id, $script_id));
    if ($count > 0)
    {
        $errors->Add('This parameter is already defined for this script on this partition.');
        return FALSE;
    }
    return TRUE;
}


// ** Handle User Requests **

$log_info = array(	'app_id'		=> $app->GetId(),
					'app_part_id'	=> $part_id);

if ($_POST['done'])
{
    Utils::redirect("edit_app_partition.php?id=$part_id");
}

if ($_POST['update_parameter'])
{
    $param_id = Utils::get_first_array_key($_POST['update_parameter']);
    $param_name = $db->GetOne("SELECT name FROM mce_application_script_trigger_parameters " .
                              "WHERE mce_application_script_trigger_parameters_id = ?",
                              array($param_id));
    $values = $_POST['parameters'][$param_id];
    foreach ($values as $val_id => $val)
    {
        if (empty($val))
            $errors->Add('A parameter value cannot be empty');
        check_conflicting_values($script->GetEventType(), $param_name, $val, $val_id);
    }

    if ($errors->IsEmpty())
    {
    	$db->StartTrans();
        foreach ($values as $val_id => $val)
        {
            $db->Execute("UPDATE mce_trigger_parameter_values SET value = ? WHERE mce_trigger_parameter_values_id = ?",
                         array($val, $val_id));
        }
        $log_info['param_id'] = $param_id;
        $log_info['values'] = $values;
        $response = "Parameter values have been updated.";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::TRIGGER_PARAMETER_VALUES_UPDATED, $log_info);
        $db->CompleteTrans();
        $need_router_refresh = TRUE;
    }
}

if ($_POST['add_parameter_submit'])
{
    if (empty($_POST['add_parameter']['name']))
        $errors->Add("Parameter name is empty");
    if (empty($_POST['add_parameter']['value']))
        $errors->Add("Parameter value is empty");

    check_conflicting_names($_POST['add_parameter']['name']);
    check_conflicting_values($script->GetEventType(), $_POST['add_parameter']['name'], $_POST['add_parameter']['value']);

    if ($errors->IsEmpty())
    {
    	$db->StartTrans();
        $db->Execute("INSERT INTO mce_application_script_trigger_parameters " .
                     "(name, mce_application_scripts_id, mce_application_partitions_id) " .
                     "VALUES (?,?,?)",
                     array($_POST['add_parameter']['name'], $script_id, $part_id));
        $param_id = $db->Insert_ID();
        $db->Execute("INSERT INTO mce_trigger_parameter_values " .
                     "(mce_application_script_trigger_parameters_id, value) " .
                     "VALUES (?,?)",
                     array($param_id, $_POST['add_parameter']['value']));
        $log_info['add_parameter'] = $_POST['add_parameter'];
        $log_info['param_id'] = $param_id;
        $response = "Trigger parameter added";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::TRIGGER_PARAMETER_ADDED, $log_info);
        $db->CompleteTrans();
        $need_router_refresh = TRUE;
    }
}

if ($_POST['add_value_submit'])
{
    $param_id = Utils::get_first_array_key($_POST['add_value_submit']);
    $param_name = $db->GetOne("SELECT name FROM mce_application_script_trigger_parameters " .
                              "WHERE mce_application_script_trigger_parameters_id = ?",
                              array($param_id));
    if (empty($_POST['add_value'][$param_id]))
        $errors->Add("Parameter value is empty");

    check_conflicting_values($script->GetEventType(), $param_name, $_POST['add_value'][$param_id]);

    if ($errors->IsEmpty())
    {
    	$db->StartTrans();
        $db->Execute("INSERT INTO mce_trigger_parameter_values " .
                     "(mce_application_script_trigger_parameters_id, value) " .
                     "VALUES (?,?) ",
                     array($param_id, $_POST['add_value'][$param_id]));

        $log_info['param_id'] = $param_id;
        $log_info['value'] = $_POST['add_value'][$param_id];
        $response = "Trigger parameter value added";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::TRIGGER_PARAMETER_VALUE_ADDED, $log_info);
        $db->CompleteTrans();
        $need_router_refresh = TRUE;        
    }
}

if ($_POST['delete_parameter'])
{
    $p_id = Utils::get_first_array_key($_POST['delete_parameter']);
    $db->StartTrans();
    $db->Execute("DELETE FROM mce_application_script_trigger_parameters " .
                 "WHERE mce_application_script_trigger_parameters_id = ?",
                 array($p_id));
	$log_info['param_id'] = $p_id;
    $response = "Trigger parameter deleted";
    EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::TRIGGER_PARAMETER_DELETED, $log_info);
    $db->CompleteTrans();
    $need_router_refresh = TRUE;
}

if ($_POST['delete_value'])
{
    $v_id = Utils::get_first_array_key($_POST['delete_value']);
    $db->StartTrans();
    $v_info = $db->GetRow("SELECT * FROM mce_trigger_parameter_values WHERE mce_trigger_parameter_values_id = ?",
                 array($v_id));
    $db->Execute("DELETE FROM mce_trigger_parameter_values WHERE mce_trigger_parameter_values_id = ?",
                 array($v_id));
    $response = "Trigger parameter value deleted";
    EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::TRIGGER_PARAMETER_VALUE_DELETED, array_merge($log_info, $v_info));
    $db->CompleteTrans();
    $need_router_refresh = TRUE;
}

if ($need_router_refresh)
{
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
}


// ** Retreive Data **

$triggers = $script->GetTriggerParameters();


// ** Render Page **

$template_vars = array( 'script_id'             => $script_id,
                        'part_id'               => $part_id,
                        'event_type'            => $script->GetEventType(),
                        'triggers'              => $triggers);

$page = new Layout();
$page->SetPageTitle('Script Trigger Parameters for ' . $script->GetName());
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>',
                            '<a href="component_list.php?type=' . ComponentType::APPLICATION . '">Applications</a>',
                            '<a href="edit_app.php?id=' . $app->GetId() . '">' . htmlspecialchars($app->GetName()) .'</a>',
                            '<a href="edit_app_partition.php?id=' . $part_id . '">Partition: ' . htmlspecialchars($app_part->GetName()) . '</a>',
                            'Script: ' . htmlspecialchars($script->GetName()) ));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($template_vars);
$page->Display('script_trigger_edit.tpl');

?>