<?php

/* This page manages the SCCP Subscribers for a CallManager Cluster.  This page
 * essentially has two modes: add and edit mode.  Which mode is used is
 * determined by which action is POSTed to it ('add' or 'edit').  Subscribers must
 * exist before any SCCP Device Pools are created.
 */

require_once("init.php");
require_once("lib.CallManagerUtils.php");


function refresh_sccp_provider(ErrorHandler $errors)
{
    return CallManagerUtils::refresh_provider(SCCP_PROVIDER, $errors);
}


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = new ErrorHandler();
Utils::trim_array($_POST);


// ** Send user back to CallMangager cluster page **

if ($_POST['done'])
{
    Utils::redirect("edit_call_manager.php?id=" . $_POST['cluster_id']);
}


// **  Verify POSTed data on add and updated actions **

if ($_POST['add'] || $_POST['update'])
{
        // Check if the requested IP is good
        if (!Utils::is_blank($_POST['ip_address']))
        {
            // If no name is speficied, make the IP the name
            if (Utils::is_blank($_POST['name']))
                $_POST['name'] = $_POST['ip_address'];

            // Check for conflicts
            $subscriber_id = isset($_POST['edit']) ? $_POST['edit'] : 0;
            $conflicts = $db->GetOne("SELECT COUNT(*) FROM mce_call_manager_cluster_subscribers WHERE ip_address = ? " .
                                     "AND mce_call_manager_cluster_subscribers_id <> ?",
                                     array($_POST['ip_address'], $subscriber_id));
            if ($conflicts > 0)
                $errors->Add("A subscriber at that address already exists.");
        }
        else
        {
            $errors->Add("The subscriber address is not valid");
        }
}


// ** Actions for the page in 'new' mode **

if ($_POST['new'])
{
    $page_title = "Create A Subscriber";
    $template = "add_subscriber.tpl";

    // Create a Subscriber if requested

    if ($_POST['add'] && $errors->IsEmpty())
    {
        $db->StartTrans();
        $db->Execute("INSERT INTO mce_call_manager_cluster_subscribers " .
                     "(name, ip_address, mce_call_manager_clusters_id) " .
                     " VALUES (?,?,?)",
                     array($_POST['name'], $_POST['ip_address'], $_POST['cluster_id']));
        $_POST['id'] = $db->Insert_ID();
        EventLog::log(LogMessageType::AUDIT, 'Sccp Subscriber created', LogMessageId::SUBSCRIBER_CREATED, $_POST);
        $db->CompleteTrans();
        refresh_sccp_provider($errors);
        $response = "Subscriber created";
        Utils::redirect("edit_call_manager.php?id=" . $_POST['cluster_id'] . "&s_response=" . Utils::safe_serialize($response));
    }
}


// ** Actions for the page in 'edit' mode **

if ($_POST['edit'])
{
    if (is_array($_POST['edit']))
        $id = Utils::get_first_array_key($_POST['edit']);
    else
        $id = $_POST['edit'];
    $page_title = "Edit Subscriber " . $subscriber['name'] ;
    $template = "edit_subscriber.tpl";
    $template_vars['id'] = $id;

    // Delete Subscriber

    if ($_POST['delete'])
    {
        $check_query = "SELECT name FROM mce_components WHERE mce_components_id IN
                        (SELECT mce_components_id
                        FROM mce_config_entry_metas
                        LEFT JOIN mce_config_entries USING (mce_config_entry_metas_id)
                        LEFT JOIN mce_config_values USING (mce_config_entries_id)
                        WHERE (name = 'MetreosReserved_PrimarySubscriberId' OR name = 'MetreosReserved_SecondarySubscriberId')
                        AND value = ?)";
        $still_using = $db->GetCol($check_query, $id);
        if (sizeof($still_using) > 0)
        {
            $message  = "You cannot delete this Subscriber because it is still associated with the following device pools:\n";
            $message .= implode(',',$still_using) . "\n";
            $message .= "Please disassociate the Subscriber from these devices before deleting it.";
            $errors->Add($message);
        }
      
        if ($errors->IsEmpty())
        {
            $db->StartTrans();
            $info['id'] = $id;
            $info['name'] = $db->GetOne("SELECT name FROM mce_call_manager_cluster_subscribers " .
                                        "WHERE mce_call_manager_cluster_subscribers_id = ?",
                                        array($id));
            $db->Execute("DELETE FROM mce_call_manager_cluster_subscribers " .
                         "WHERE mce_call_manager_cluster_subscribers_id = ?",
                         array($id));
            EventLog::log(LogMessageType::AUDIT, 'Sccp Subscriber removed', LogMessageId::SUBSCRIBER_REMOVED, $info);
            $db->CompleteTrans();
            refresh_sccp_provider($errors);
            $response = "Subscriber removed";
            Utils::redirect("edit_call_manager.php?id=" . $_POST['cluster_id'] . "&s_response=" . Utils::safe_serialize($response));
       }
    }

    // Update the Subscriber's properties

    if ($_POST['update'] && $errors->IsEmpty())
    {
        $db->StartTrans();
        $info = $_POST;
        $info['id'] = $id;
        $db->Execute("UPDATE mce_call_manager_cluster_subscribers " .
                     "SET name=?, ip_address=? " .
                     "WHERE mce_call_manager_cluster_subscribers_id = ?",
                     array($_POST['name'], $_POST['ip_address'], $id));
        EventLog::log(LogMessageType::AUDIT, 'Sccp Subscriber updated', LogMessageId::SUBSCRIBER_MODIFIED, $info);
        $db->CompleteTrans();
        refresh_sccp_provider($errors);
        $response = "Subscriber updated";
    }

    // Retrieve the Subscriber's properties

    if ($errors->IsEmpty())
    {
        $subscriber = $db->GetRow("SELECT * FROM mce_call_manager_cluster_subscribers " .
                                   "WHERE mce_call_manager_cluster_subscribers_id = ?",
                                   array($id));
        $template_vars += $subscriber;
    }
}


// ** Render and display page **

$page = new Layout();
$page->SetPageTitle($page_title);
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            '<a href="edit_call_manager.php?id=' . $_POST['cluster_id'] . '">Unified Communications Manager</a>',
                            'Subscriber'));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($_POST);
$page->mTemplate->assign($template_vars);
$page->Display($template);

?>