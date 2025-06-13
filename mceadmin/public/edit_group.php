<?php

require_once("init.php");
require_once("class.ComponentGroup.php");
require_once("class.TelephonyManagerExtensions.php");
require_once("lib.MediaServerUtils.php");


function clear_related_caches(ErrorHandler $errors)
{
    global $type, $call_route_group_types;

    if ($type == GroupType::MEDIA_RESOURCE_GROUP)
        MediaServerUtils::clear_mrg_cache($errors);
    else if (in_array($type, $call_route_group_types))
    {
        $tm_ex = new TelephonyManagerExtensions();
        if (!$tm_ex->ClearCrgCache())
            $errors->Add($tm_ex->GetError());
    }
}


// ** SET UP **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$id = $_REQUEST['id'];
$type = $_REQUEST['type'];
$errors = new ErrorHandler();

$call_route_group_types[] = GroupType::CTI_SERVER_GROUP;
$call_route_group_types[] = GroupType::SCCP_GROUP; 
$call_route_group_types[] = GroupType::H323_GATEWAY_GROUP; 
$call_route_group_types[] = GroupType::SIP_GROUP;
$call_route_group_types[] = GroupType::TEST_IPT_GROUP;

Utils::trim_array($_POST);


// ** RETRIEVE & SET CONTEXT DATA **

if ($id > 0)
{
    $group = new ComponentGroup();
    $group->SetId($id);
    $group->Build();
    if (empty($type))
        $type = $group->GetType();
}

switch ($type)
{
    case GroupType::SCCP_GROUP :
    case GroupType::H323_GATEWAY_GROUP :
    case GroupType::TEST_IPT_GROUP :
        $parent_page = "telephony.php";
        $parent_page_name = "Telephony Servers";
        $retrieve_clauses[] = "type = " . intval($type);
        break;
    case GroupType::CTI_SERVER_GROUP :
        $parent_page = "telephony.php";
        $parent_page_name = "Telephony Servers";
        $retrieve_clauses[] = "type IN (" . ComponentType::CTI_DEVICE_POOL . "," . ComponentType::CTI_ROUTE_POINT . ")";
        break;
    case GroupType::SIP_GROUP :
        $parent_page = "telephony.php";
        $parent_page_name = "Telephony Servers";
        $retrieve_clauses[] = "type = " . ComponentType::SIP_TRUNK_INTERFACE;
        break;
    case GroupType::ALARM_GROUP:
        $parent_page = "alarm_list.php";
        $parent_page_name = "Alarms";
        $retrieve_clauses[] = "type >= " . ALARM_TYPE_ENUM_START;
        $retrieve_clauses[] = "type <= " . ALARM_TYPE_ENUM_END;
        break;
    case GroupType::MEDIA_RESOURCE_GROUP :
        $parent_page = "media_server_list.php";
        $parent_page_name = "Media Engines";
        $retrieve_clauses[] = "type = " . ComponentType::MEDIA_SERVER;
        break;
    default:
        throw new Exception('Group type not defined');
        break;
}


// ** HANDLE USER ACTIONS **

if ($_POST['done'])
    Utils::redirect($parent_page);

if ($id > 0)
{

    if ($_POST['delete'])
        Utils::redirect("delete_group.php?id=$id&type=$type");

    if ($_POST['update'])
    {
        if (empty($_POST['name']))
            $errors->Add("Name field is empty.");
        if ($errors->IsEmpty())
        {
            $group->SetName($_POST['name']);
            $group->SetDescription($_POST['description']);
            $group->SetAlarmId($_POST['alarm_group_id']);
            $group->SetFailoverId($_POST['failover_group_id']);
            $group->Update();
            $response = "Group properties have been updated.";
        }
    }

    if ($_POST['add'] && $_POST['add_id'] > 0)
    {
        $group->AddComponent($_POST['add_id']);
        clear_related_caches($errors);
        $response = "The component has been added to group.";
    }

    if ($_POST['remove'] && !empty($_POST['member_ids']))
    {
        foreach ($_POST['member_ids'] as $mem_id)
            $group->RemoveComponent($mem_id);
        clear_related_caches($errors);
        $response = "The component(s) has been removed from group.";
    }

    if ($_POST['reorder'] && !empty($_POST['member_ids']))
    {
        $group->OrderComponents($_POST['member_ids']);
        clear_related_caches($errors);
        $response =  "The component(s) have been reordered.";
    }
    
}
else
{
    if ($_POST['create'])
    {
        if (!empty($_POST['name']))
        {
            if (in_array($type, $call_route_group_types))
            {
                $sql = "SELECT mce_component_groups_id FROM mce_component_groups WHERE name = ? AND component_type IN (" . implode(',',array_fill(0,count($call_route_group_types),'?')) . ")";
                $params = array_merge(array($_POST['name']),$call_route_group_types);
                $gid = $db->GetOne($sql, $params);
            }
            else
                $gid = $db->GetOne("SELECT mce_component_groups_id FROM mce_component_groups WHERE name = ? AND component_type = ?", array($_POST['name'], $type));
            
            if ($gid > 0)
            {
                $errors->Add("Another group of this type uses the name '" . $_POST['name'] . "'");
            }
            else
            {
                if (empty($_POST['failover_group_id']))
                    $_POST['failover_group_id'] = 0;
                $db->StartTrans();
                $db->Execute("INSERT INTO mce_component_groups (name, component_type, description, mce_failover_group_id, mce_alarm_group_id) " .
                             "VALUES (?,?,?,?,?)"
                             , array($_POST['name'], $_POST['type'], $_POST['description'], $_POST['failover_group_id'], $_POST['alarm_group_id']));
                $id = $db->Insert_ID();
                $response = "Group $_POST[name] was created.";
                EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::GROUP_ADDED, print_r($_POST,TRUE));
                $db->CompleteTrans();
    
                $group = new ComponentGroup();
                $group->SetId($id);
                $group->Build();
            }
        }
        else
        {
            $errors->Add("A group name was not defined.");
        }
    }
}


// ** RETRIVE GROUP DATA **

if ($id > 0)
{
    // Retrieve members of this group
    $members = $group->GetComponents();
    for ($i = 0; $i < sizeof($members); ++$i)
    {
        $members[$i]['display_type'] = ComponentType::describe($members[$i]['type']);
        $comp_ids[] = $members[$i]['mce_components_id'];
    }

    // Retrieve the non members
    if (!empty($comp_ids))
    {
        $comp_ids = array_map('intval', $comp_ids);
        $retrieve_clauses[] = "mce_components_id NOT IN (" . implode(',', $comp_ids) . ")";
    }
    $nonmembers = $db->GetAll("SELECT * FROM mce_components WHERE " . implode(" AND ", $retrieve_clauses));

    $title = "Edit Group: " . $group->GetName();
    $group_vars = array('name'              => $group->GetName(),
                        'description'       => $group->GetDescription(),
                        'alarm_group_id'    => $group->GetAlarmId(),
                        'failover_group_id' => $group->GetFailoverId());
}
else
{
    $title = "Create Group";
}


// ** RENDER PAGE **

$template_vars = array( 'id'                    => $id,
                        'type'                  => $type,
                        'type_display'          => GroupType::display($type),
                        'members'               => $members,
                        'nonmembers'            => $nonmembers,
                        'failovers'             => ComponentUtils::get_groups_of_type($type, $id),
                        'default_alarm_group'   => DEFAULT_ALARM_GROUP_ID,
                        'alarms'                => ComponentUtils::get_groups_of_type(GroupType::ALARM_GROUP, $id),
                        'is_call_route_group'   => in_array($type, $call_route_group_types),
                        'is_media_resource_group' => $type == GroupType::MEDIA_RESOURCE_GROUP ,
                       );
if (is_object($group))
    $template_vars['default'] = $group->IsDefault();


$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>',
                            '<a href="' . htmlspecialchars($parent_page) . '">' . htmlspecialchars($parent_page_name) . '</a>',
                            htmlspecialchars($title)));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
if (!empty($group_vars))
    $page->mTemplate->assign($group_vars);
$page->mTemplate->assign($template_vars);
$page->Display('edit_group.tpl');

?>