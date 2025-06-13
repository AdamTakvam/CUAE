<?php

require_once("init.php");
require_once("lib.CallManagerUtils.php");
require_once("components/class.DevicePool.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = new ErrorHandler();
Utils::trim_array($_POST);
$id = $_REQUEST['id'];


if ($_POST['delete_yes'])
{
	$db->StartTrans();
    // Delete subscrbiers & cti managers
    $db->Execute("DELETE FROM mce_call_manager_cluster_subscribers WHERE mce_call_manager_clusters_id = ?", 
                 array($id));
    $db->Execute("DELETE FROM mce_call_manager_cluster_cti_managers WHERE mce_call_manager_clusters_id = ?", 
                 array($id));
    
    // Delete device pools
    $dp_ids = $db->GetCol("SELECT mce_components_id FROM mce_call_manager_cluster_members " .
                          "WHERE mce_call_manager_clusters_id = ?",
                          array($id));
    foreach ($dp_ids as $dp_id)
    {
    	try
    	{
	        $dp = new DevicePool();
	        $dp->SetErrorHandler($errors);
	        $dp->SetId($dp_id);
	        $dp->Build();
	        $dp->Uninstall();
    	}
    	catch (Exception $ex)
    	{
    		// More than likely, a device pool is already uninstalled
    		// but the reference is still in the database.
    		ErrorLog::raw_log(print_r($ex, TRUE));
    	}
    }
    
    // Delete the cluster
    $db->Execute("DELETE FROM mce_call_manager_clusters WHERE mce_call_manager_clusters_id = ?", array($id));
    CallManagerUtils::refresh_provider(SCCP_PROVIDER, $errors);
    CallManagerUtils::refresh_provider(JTAPI_PROVIDER, $errors);    
    CallManagerUtils::refresh_provider(DEVICELISTX_PROVIDER, $errors);
    
    if (!$errors->IsEmpty())
        $add = "?s_errors=" . Utils::safe_serialize($errors);
    $add = "?s_response=" . Utils::safe_serialize("Unified Communications Manager removed");

    EventLog::log(LogMessageType::AUDIT, 'Unified Communications Manager Removed', LogMessageId::CALL_MANAGER_CLUSTER_REMOVED);
    $db->CompleteTrans();
    Utils::redirect("telephony.php" . $add);
}

if ($_POST['delete_no'])
    Utils::redirect("edit_call_manager.php?id=$id");

$call_manager = $db->GetRow("SELECT * FROM mce_call_manager_clusters WHERE mce_call_manager_clusters_id = ?", array($id));
    
$page = new Layout();
$page->SetPageTitle($call_manager['name']);
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            'Remove Unified Communications Manager: ' . htmlspecialchars($call_manager['name'])));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('call_manager', $call_manager);
$page->mTemplate->assign('id', $id);
$page->Display('delete_call_manager.tpl');

?>