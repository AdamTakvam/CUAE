<?php

/* The page displays the contents of a CallManager cluster and allows the editing
 * of the cluster's properties.  From this page, you can also create/edit SCCP Subscribers,
 * CTI Managers, CTI Device Pools and Route Points, and SCCP Device Pools.  A manager
 * must be created before a CTI Device Pool or Route Point.  A subscriber must be created
 * before a SCCP Device Pool.
 */

require_once("init.php");
require_once("lib.CallManagerUtils.php");


function get_cluster_members_of_type($type, $id)
// Retrieves all the members of a certain type for the CallManager Cluster
{
    $db = new MceDb();
    $members = $db->GetAll("SELECT * FROM mce_call_manager_cluster_members " .
                            "JOIN mce_components USING (mce_components_id) " .
                            "WHERE mce_call_manager_clusters_id = ? AND type = ?",
                            array($id, $type));
    return $members;
}


// ** Setup page **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = new ErrorHandler();
Utils::trim_array($_POST);
$id = $_REQUEST['id'];

$response = Utils::safe_unserialize($_REQUEST['s_response']);


// ** Redirect to the proper page based on the posted actions **

if ($_POST['view'])
{
    $view_id = Utils::get_first_array_key($_POST['view']);
    Utils::redirect("view_device_pool.php?id=$view_id");
}

if ($_POST['edit_device_pool'])
{
    $edit_id = Utils::get_first_array_key($_POST['edit_device_pool']);
    Utils::redirect("edit_device_pool.php?id=$edit_id");
}

if ($_POST['manage_device_pool'])
{
    $edit_id = Utils::get_first_array_key($_POST['manage_device_pool']);
    $type = $db->GetOne("SELECT type FROM mce_components WHERE mce_components_id = ?", array($edit_id));
    Utils::redirect("manage_device_pool.php?id=$edit_id&type=$type");
}

if ($_POST['manage_monitored_devices'])
{
    $mdp_id = $db->GetOne("SELECT c.mce_components_id " .
                          "FROM mce_call_manager_cluster_members AS cmcm LEFT JOIN mce_components AS c USING (mce_components_id) " .
                          "WHERE c.type = ? AND cmcm.mce_call_manager_clusters_id = ?",
                          array(ComponentType::MONITORED_CTI_DEVICE_POOL, $id));
    Utils::redirect("manage_device_pool.php?id=$mdp_id&type=" . ComponentType::MONITORED_CTI_DEVICE_POOL);
}

if ($_POST['uninstall_cluster'])
{
    Utils::redirect("delete_call_manager.php?id=$id");
}


// ** Update CallManager cluster configurations **

if ($_POST['update_cluster'])
{
    $call_manager = $_POST['call_manager'];
    if (empty($call_manager['name']))
        $errors->Add("Name field is blank");
    if (Utils::is_blank($call_manager['publisher_ip_address']))
        $errors->Add("Publisher address is not valid");
    if (empty($call_manager['publisher_username']))
        $errors->Add("Publisher administrator username is blank");
    if (!empty($call_manager['new_password']))
    {
        if ($call_manager['new_password'] != $call_manager['new_password_verify'])
            $errors->Add("Failed to verify password");
    }

    if ($errors->IsEmpty())
    {
        $conflicts = $db->GetOne("SELECT COUNT(*) FROM mce_call_manager_clusters WHERE publisher_ip_address = ? AND mce_call_manager_clusters_id <> ?", 
                                 array($_POST['publisher_ip_address'], $id));
        if ($conflicts > 0)
            $errors->Add("A call manager cluster already exists with that address " . $_POST['publisher_ip_address']);
    }
    
    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        $db->Execute("UPDATE mce_call_manager_clusters SET name = ?, publisher_ip_address = ?, publisher_username = ?, version = ?, snmp_community = ?, description = ? " .
                     "WHERE mce_call_manager_clusters_id = ?",
                     array($call_manager['name'], $call_manager['publisher_ip_address'], $call_manager['publisher_username'], $call_manager['version'], 
                           $call_manager['snmp_community'], $call_manager['description'], $id));
        if (!empty($call_manager['new_password']))
        {
            $db->Execute("UPDATE mce_call_manager_clusters SET publisher_password=? WHERE mce_call_manager_clusters_id=?",
                         array($call_manager['new_password'], $id));
        }
        $response = "Unified Communications Manager configuration updated";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::CALL_MANAGER_CLUSTER_MODIFIED, $call_manager);
        $db->CompleteTrans();

        CallManagerUtils::refresh_provider(SCCP_PROVIDER, $errors);
        CallManagerUtils::refresh_provider(JTAPI_PROVIDER, $errors);
        CallManagerUtils::refresh_provider(DEVICELISTX_PROVIDER, $errors);
    }
}
else
{
    $call_manager = $db->GetRow("SELECT * FROM mce_call_manager_clusters WHERE mce_call_manager_clusters_id = ?", array($id));
}


// ** Retrieve contents of the CallManager cluster **

// Subscribers
$subscribers = $db->GetAll("SELECT * FROM mce_call_manager_cluster_subscribers " .
                           "WHERE mce_call_manager_clusters_id = ?",
                           array($id));

// CTI Managers
$cti_managers = $db->GetAll("SELECT * FROM mce_call_manager_cluster_cti_managers " .
                           "WHERE mce_call_manager_clusters_id = ?",
                           array($id));

// Related components
$device_pools = get_cluster_members_of_type(ComponentType::SCCP_DEVICE_POOL, $id);
$cti_dps = get_cluster_members_of_type(ComponentType::CTI_DEVICE_POOL, $id);
$cti_rps = get_cluster_members_of_type(ComponentType::CTI_ROUTE_POINT, $id);
$mcti_dps = get_cluster_members_of_type(ComponentType::MONITORED_CTI_DEVICE_POOL, $id);
for ($x = 0; $x < sizeof($cti_rps); ++$x)
{
    $data = $db->GetRow("SELECT device_name, directory_number, status FROM mce_call_manager_devices WHERE mce_components_id = ?",
                        array($cti_rps[$x]['mce_components_id']));
    $cti_rps[$x]['device_name'] = $data['device_name'];
    $cti_rps[$x]['directory_number'] = $data['directory_number'];
    $cti_rps[$x]['status_display'] = DeviceStatus::display($data['status']);
}


// ** Render and display the page **

$template_vars = array( 'id'                    => $id,
                        'call_manager'          => $call_manager,
                        'add_subscriber_name'   => $add_subscriber_name,
                        'add_subscriber_ip'     => $add_subscriber_ip,
                        'subscribers'           => $subscribers,
                        'cti_managers'          => $cti_managers,
                        'subscriber_count'      => sizeof($subscribers),
                        'cti_manager_count'     => sizeof($cti_managers),
                        'sccp_device_pools'     => $device_pools,
                        'cti_device_pools'      => $cti_dps,
                        'cti_route_points'      => $cti_rps,
                        'monitored_device_pools'=> $mcti_dps,
                        'cm_versions'           => CallManagerVersion::get_versions());

$page = new Layout();
$page->SetPageTitle($call_manager['name']);
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            'Unified Communications Manager: ' . htmlspecialchars($call_manager['name']) ));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($template_vars);
$page->Display('edit_call_manager.tpl');

?>