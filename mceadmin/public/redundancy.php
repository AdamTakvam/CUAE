<?php

require_once("init.php");
require_once("lib.SystemConfig.php");
require_once("class.ReplicationManagement.php");


// ** Set up objects and variables **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$rep_mgmt = new ReplicationManagement();

Utils::trim_array($_POST);

$keys = array('redundancy_standby_ip','redundancy_standby_startup_sync_time',
              'redundancy_master_ip','redundancy_master_max_missed_heartbeat',
              'redundancy_master_heartbeat');

$get_post_vals = array( 'server_id','redundancy_standby_username',
                        'redundancy_standby_password','verify_redundancy_standby_password',
                        'redundancy_master_username','redundancy_master_password',
                        'verify_redundancy_master_password');

if ($_POST['done'])
    Utils::redirect('main.php');


// ** Validate and update values **

if ($_POST['update'])
{
    $prev_slave_settings = $rep_mgmt->GetSlaveSettings();
    $prev_master_settings = $rep_mgmt->GetMasterSettings();
    $values = Utils::extract_array_using_keys(array_merge($keys,$get_post_vals), $_POST);

    $values['server_id'] = intval($values['server_id']);
    $values['redundancy_standby_startup_sync_time'] = intval($values['redundancy_standby_startup_sync_time']);
    $values['redundancy_master_max_missed_heartbeat'] = intval($values['redundancy_master_max_missed_heartbeat']);
    $values['redundancy_master_heartbeat'] = intval($values['redundancy_master_heartbeat']);

    if ($values['server_id'] < 1)
        $errors->Add("Server ID must be a positive integer");
    if ($values['redundancy_standby_startup_sync_time'] <= 0)
        $errors->Add("Startup Syncrhonization Timeout must be a positive number");
    if ($values['redundancy_master_max_missed_heartbeat'] <= 0)
        $errors->Add("Max Missed Heartbeats must be a positive number");
    if ($values['redundancy_master_heartbeat'] <= 0)
        $errors->Add("Heartbeat Interval must be a positive number");

    if (!empty($values['redundancy_standby_ip']))
    {
        if (empty($values['redundancy_standby_username']))
            $errors->Add("Username for stand-by access is required");
        if (in_array(strtolower($values['redundancy_standby_username']), array('root',MceConfig::DB_USER)))
            $errors->Add($values['redundancy_standby_username'] . " is not an acceptable username.  Try something else.");
        if (empty($values['redundancy_standby_password']) && empty($prev_master_settings['redundancy_standby_password']))
            $errors->Add("Password for stand-by access is required");
        if (!empty($values['redundancy_standby_password']) && ($values['redundancy_standby_password'] <> $values['verify_redundancy_standby_password']))
            $errors->Add("Password for stand-by access could not be verfied");
    }

    if (!empty($values['redundancy_master_ip']))
    {
        if (empty($values['redundancy_master_username']))
            $errors->Add("Username to access master database is required");
        if (empty($values['redundancy_master_password']) && empty($prev_slave_settings['redundancy_master_password']))
            $errors->Add("Password to access master database is required");
        if (!empty($values['redundancy_master_password']) && ($values['redundancy_master_password'] <> $values['verify_redundancy_master_password']))
            $errors->Add("Password to access master database could not be verified");
    }

    if ($errors->IsEmpty())
    {
        $standby_ip = SystemConfig::get_config('redundancy_standby_ip');
        $master_ip = SystemConfig::get_config('redundancy_master_ip');
        $server_id = $rep_mgmt->GetServerId();

        // Determine if services need to be shut down
        $restart_services = (empty($values['redundancy_standby_ip']) <> empty($standby_ip) ||
                             $master_ip <> $values['redundancy_master_ip'] ||
                             $server_id <> $values['server_id']);

        if ($errors->IsEmpty())
        {
            if ($server_id <> $values['server_id'])
                $rep_mgmt->SetServerId($values['server_id']);

            // Toggle redundancy as a master
            if (Utils::is_blank($values['redundancy_standby_ip']))
            {
                $values['redundancy_standby_ip'] = NULL;
                $rep_mgmt->DisableMaster();
                EventLog::log(LogMessageType::AUDIT, 'Redundancy as a master disabled', LogMessageId::REDUNDANCY_MASTER_DISABLED);
            }
            else
            {
                $rep_mgmt->DisableMaster();
                $password = empty($values['redundancy_standby_password']) ? $prev_master_settings['redundancy_standby_password'] : $values['redundancy_standby_password'];
                try
                {
                    $rep_mgmt->EnableReplication(ReplicationRole::MASTER, $values['server_id'], $values['redundancy_standby_username'], $password, $values['redundancy_standby_ip']);
                    EventLog::log(LogMessageType::AUDIT, 'Redundancy as a master enabled', LogMessageId::REDUNDANCY_MASTER_ENABLED, print_r(MceUtils::remove_password_info($values),TRUE));
                }
                catch (Exception $e)
                {
                    $rep_mgmt->DisableMaster();
                    $errors->Add('There was a problem enabling redundancy as a master: ' . $e->GetMessage());
                }
            }

            // Toggle redundancy as a stand-by
            if (Utils::is_blank($values['redundancy_master_ip']))
            {
                $values['redundancy_master_ip'] = NULL;
                $rep_mgmt->DisableSlave();
                EventLog::log(LogMessageType::AUDIT, 'Redundancy as a stand-by disabled', LogMessageId::REDUNDANCY_STANDBY_DISABLED);
            }
            else
            {
                $host = $values['redundancy_master_ip'];
                $password = empty($values['redundancy_master_password']) ? $prev_slave_settings['redundancy_master_password'] : $values['redundancy_master_password'];
                try
                {
                    $rep_mgmt->EnableReplication(ReplicationRole::SLAVE, $values['server_id'], $values['redundancy_master_username'], $password, $host);
                    EventLog::log(LogMessageType::AUDIT, 'Redundancy as a stand-by enabled', LogMessageId::REDUNDANCY_STANDBY_ENABLED, print_r(MceUtils::remove_password_info($values),TRUE));
                }
                catch (Exception $e)
                {
                    $rep_mgmt->DisableSlave();
                    $errors->Add('There was a problem enabling redundancy as a slave: ' . $e->GetMessage());
                }
            }

            if ($errors->IsEmpty())
            {
                // Update confg values in the database
                $db->StartTrans();
                foreach ($values as $name => $val)
                {
                    $db->Execute("UPDATE mce_system_configs SET value = ? WHERE name = ?", array($val, $name));
                }
                $db->CompleteTrans();
            }
        }

        if ($errors->IsEmpty())
        {
            // Refresh the ClusterInterface
            $asi = new AppServerInterface();
            if ($asi->Connected())
            {
                $command = 'RefreshConfiguration';
                $params['ComponentType'] = 'Core';
                $params['ComponentName'] = 'ClusterInterface';
                $asi->Send(MceUtils::generate_xml_command($command,$params));
                if ($asi->Error())
                    $errors->Add("ClusterInterface refresh failed.\nReason: " . $asi->Error());
            }

            if ($restart_services)
            {
                // Restart MySQL and all dependent services if necessary
                $rep_mgmt->RestartMySql();
                $db = new MceDb();
            }

            $feedback = "Redundancy configuration has been updated";
        }
    }

}

// ** Retrieve configurations **

foreach ($keys as $key)
{
    $names[] = "`name` = " . $db->Quote($key);
}
$rs = $db->GetAll("SELECT name, value FROM mce_system_configs WHERE " . implode(' OR ', $names));
foreach ($rs as $res)
{
    $configs[$res['name']] = $res['value'];
}


// ** Check to see if master has severed connection with us as a slave **

if (empty($configs['redundancy_master_ip']) && $rep_mgmt->IsSlaveEnabled())
{
    $rep_mgmt->DisableSlave();
    $feedback = "This appliance is no longer a redundant stand-by.  It is likely that the master has no longer authorized this appliance for it.";
}


// ** Render page **

$title = "Redundancy Setup";

$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);
if ($errors->IsEmpty())
{
    $page->mTemplate->Assign($configs);
    $page->mTemplate->Assign('server_id', $rep_mgmt->GetServerId());
    $page->mTemplate->Assign($rep_mgmt->GetMasterSettings());
    $page->mTemplate->Assign($rep_mgmt->GetSlaveSettings());
}
else
    $page->mTemplate->Assign($values);
$page->mTemplate->Assign('slave_enabled', $rep_mgmt->IsSlaveEnabled());
$page->Display('redundancy.tpl');

?>