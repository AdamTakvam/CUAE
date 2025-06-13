<?php

require_once("init.php");
require_once("class.ComponentGroup.php");
require_once("lib.MediaServerUtils.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$id = $_REQUEST['id'];
$type = $_REQUEST['type'];

switch ($type)
{
    case GroupType::SCCP_GROUP :
    case GroupType::H323_GATEWAY_GROUP :
    case GroupType::SIP_GROUP :
    case GroupType::CTI_SERVER_GROUP :
    case GroupType::TEST_IPT_GROUP :  
        $parent_page = "telephony.php";
        $parent_page_name = "Telephony Servers";
        break;
    case GroupType::ALARM_GROUP:
        $parent_page = "alarm_list.php";
        $parent_page_name = "Alarms";
        break;
    case GroupType::MEDIA_RESOURCE_GROUP :
        $parent_page = "media_server_list.php";
        $parent_page_name = "Media Engines";
        break;  
    default:
        throw new Exception('Group type not defined');
        break;
}


$group = new ComponentGroup();
$group->SetId($id);
$group->Build();
if ($group->IsDefault())
    Utils::redirect("edit_group.php?id=$id&type=$type");

// Handle user requested actions

if ($_POST['submit_yes'])
{
    $group->Delete();
    if ($type == GroupType::MEDIA_RESOURCE_GROUP)
        MediaServerUtils::clear_mrg_cache(new ErrorHandler());
    Utils::redirect($parent_page);
}
else if ($_POST['submit_no'])
{
    Utils::redirect("edit_group.php?id=$id&type=$type");
}


// -- RENDER PAGE --

$template_vars = array( 'id'    => $id,
                        'type'  => $_REQUEST['type'],
                        'name'  => $group->GetName());

$page = new Layout();
$page->SetPageTitle("Delete Group: " . $group->GetName());
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>',
                            '<a href="' . htmlspecialchars($parent_page) . '">' . htmlspecialchars($parent_page_name) . '</a>',
                            'Delete Group: ' . htmlspecialchars($group->GetName())));
$page->mTemplate->assign($template_vars);
$page->Display("delete_group.tpl");

?>