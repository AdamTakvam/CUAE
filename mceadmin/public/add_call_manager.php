<?php

/* This page allows the user to create a CallManager cluster.  The CallManager cluster
 * is not a telephony server component, but contains references to those components.
 * However, it masquerades on the UI as a telephony server component.  Therefore,
 * there is a table and specialized code to handle this entity.
 */

require_once("init.php");
require_once("lib.CallManagerUtils.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
Utils::trim_array($_POST);

$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);


// ** Create the CallManager cluster **

if ($_POST['submit'])
{
    $db = new MceDb();

    if (empty($_POST['name']))
        $errors->Add('Name was not set');
    else
    {
        $cm_id = $db->GetOne("SELECT mce_call_manager_clusters_id FROM mce_call_manager_clusters WHERE name = ?", array($_POST['name']));
        if ($cm_id > 0)
            $errors->Add('A CallManager with this name already exists');
    }
    
    if (Utils::is_blank($_POST['publisher_ip_address']))
        $errors->Add('Publisher address is not valid');
    if (empty($_POST['publisher_username']))
        $errors->Add('Publisher administrator username is blank');
    if (empty($_POST['publisher_password']) || $_POST['publisher_password'] != $_POST['publisher_password_verify'])
        $errors->Add("Password is empty or failed to verify password");

    if ($errors->IsEmpty())
    {
        $conflicts = $db->GetOne("SELECT COUNT(*) FROM mce_call_manager_clusters WHERE publisher_ip_address = ?", array($_POST['publisher_ip_address']));
        if ($conflicts > 0)
            $errors->Add("A call manager cluster already exists with that address " . $_POST['publisher_ip_address']);
    }
        
    if ($errors->IsEmpty())
    {
        $db->StartTrans();
        
        // Create the Call Manager Cluster entity
        $db->Execute("INSERT INTO mce_call_manager_clusters " .
                     "(name, version, publisher_ip_address, publisher_username, publisher_password, snmp_community, description) " .
                     "VALUES (?, ?, ?, ?, ?, ?, ?)",
                     array($_POST['name'], $_POST['version'], $_POST['publisher_ip_address'], $_POST['publisher_username'], 
                           $_POST['publisher_password'], $_POST['snmp_community'], $_POST['description']));
        $id = $db->Insert_ID();
        
        $log_values = MceUtils::remove_password_info($_POST);
        $response = 'Unified Communications Manager Created';
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::CALL_MANAGER_CLUSTER_CREATED, print_r($log_values, TRUE));
        $db->CompleteTrans();
        CallManagerUtils::refresh_provider(SCCP_PROVIDER, $errors);
        CallManagerUtils::refresh_provider(JTAPI_PROVIDER, $errors);
        CallManagerUtils::refresh_provider(DEVICELISTX_PROVIDER, $errors);

        Utils::redirect("edit_call_manager.php?id=$id&s_response=" . Utils::safe_serialize($response));
    }
}


// ** Render and display the page **

$title = "Create Unified Communications Manager";
$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            $title));
$page->SetErrorMessage($errors->Dump());
if ($_POST['submit'] && !$errors->IsEmpty())
    $page->mTemplate->assign($_POST);
$page->mTemplate->assign('cm_versions', CallManagerVersion::get_versions());
$page->Display('add_call_manager.tpl');

?>