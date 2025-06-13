<?php

require_once("init.php");
require_once("components/class.MediaServer.php");
require_once("class.NewConfigHandler.php");
require_once("lib.MediaServerUtils.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);


// ** Remove a media server requested by the user **

if ($_POST['remove'])
{
    $r_id = ($_POST['$id'] > 0) ? $_POST['$id'] : Utils::get_first_array_key($_POST['remove']);
    $ms = new MediaServer();
    $ms->SetErrorHandler($errors);
    $ms->SetId($r_id);
    $ms->Uninstall();
    MediaServerUtils::clear_mrg_cache($errors);
    $response = "The media engine was removed";
}

if ($_POST['enable'])
{
    $e_id = Utils::get_first_array_key($_POST['enable']);
    $ms = new MediaServer();
    $ms->SetErrorHandler($errors);
    $ms->SetId($e_id);
    $ms->Enable();
    $response = "The media engine was enabled";
}

if ($_POST['disable'])
{
    $e_id = Utils::get_first_array_key($_POST['disable']);
    $ms = new MediaServer();
    $ms->SetErrorHandler($errors);
    $ms->SetId($e_id);
    $ms->Disable();
    $response = "The media engine was disabled";
}

// ** Redirect user based on other actions **

if ($_POST['edit'])
    Utils::redirect("media_server_edit.php?id=" . Utils::get_first_array_key($_POST['edit']));

if ($_POST['add'])
    Utils::redirect("media_server_add.php");

if ($_POST['edit_group'])
    Utils::redirect("edit_group.php?id=" . $_POST['group_id'] . "&type=" . GroupType::MEDIA_RESOURCE_GROUP);

if ($_POST['create_group'])
    Utils::redirect("edit_group.php?type=" . GroupType::MEDIA_RESOURCE_GROUP);


// ** Get a listing of media servers **

$ms_ids = $db->GetCol("SELECT mce_components_id FROM mce_components WHERE type=? ORDER BY name", array(ComponentType::MEDIA_SERVER));

foreach ($ms_ids as $ms_id)
{
    $ms = new MediaServer();
    $ms->SetErrorHandler($errors);
    $ms->SetId($ms_id);
    $ms->Build($data);
    $ip = $ms->GetIpAddressConfig();
    $mservers[] = array('id'            => $ms->GetId(),
                        'name'          => $ms->GetName(),
                        'ip_address'    => $ip->GetValues(),
                        'status'        => $ms->GetStatusdisplay(),
                        'enabled'       => $ms->IsEnabled());
}


// ** Render and display the page **

$title = "Media Engines";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('groups', ComponentUtils::get_groups_of_type(GroupType::MEDIA_RESOURCE_GROUP));
$page->mTemplate->assign('mservers', $mservers);
$page->Display('media_server_list.tpl');

?>