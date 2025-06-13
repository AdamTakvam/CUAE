<?php

require_once("init.php");
require_once("class.NewConfigHandler.php");
require_once("class.StatsServerInterface.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

Utils::trim_array($_POST);
$type = $_REQUEST['type'];

$db = new MceDb();
$errors = new ErrorHandler();


// Get the standards configs

$conf_metas = ComponentUtils::get_standard_config_metas($type);

for ($i = 0; $i < sizeof($conf_metas); ++$i)
{
    $key = $conf_metas[$i]['name'];
    $new_configs[$key] = new NewConfigHandler($db);
    $new_configs[$key]->SetErrorHandler($errors);
    $new_configs[$key]->BuildWithMetaData($conf_metas[$i]);
}

if ($_POST['cancel'])
    Utils::redirect("alarm_list.php");


// Add telephony server if requested

if ($_POST['add'])
{
    foreach ($new_configs as $key => $obj)
    {
        $values = $_POST[$key];
        $obj->Validate($values);
    }
    if ($errors->IsEmpty())
    {
        // Enter in predetermined name depending on alarm type.
        switch ($type)
        {
            case ComponentType::SMTP_MANAGER:
                $name = SMTP_MANAGER_NAME;
                break;
            case ComponentType::SNMP_MANAGER:
                $name = SNMP_MANAGER_NAME;
                break;
            default :
                throw new Exception("The alarm type is invalid");
        }

        $db->StartTrans();
        $db->Execute("INSERT INTO mce_components (name, type) VALUES (?, ?)",
                     array($name, $type));
        $id = $db->Insert_ID();
        foreach ($new_configs as $key => $obj)
        {
            $values = $_POST[$key];
            $obj->Create($id, $values);
        }

        $response = ComponentType::describe($type) . ' Created';
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::ALARM_CREATED, print_r($_POST, TRUE));
        $db->CompleteTrans();

        $socket = new StatsServerInterface();
        if ($socket->Connected())
        {
            $socket->Send(MceUtils::generate_xml_command('RefreshConfiguration'));
        }
        if ($socket->Error())
        {
            $errors->Add($socket->Error());
        }
        
        Utils::redirect("alarm_list.php");
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
                            '<a href="alarm_list.php">Alarm Management</a>',
                            'Add ' . ComponentType::describe($type) . ' Alarm'));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('configs', $configs);
$page->mTemplate->assign('type', $type);
$page->mTemplate->assign('type_display', ComponentType::describe($type));
$page->mTemplate->assign($_POST);
$page->Display('alarm_add.tpl');

?>