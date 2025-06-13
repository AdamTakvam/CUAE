<?php

require_once("init.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);
$db = new MceDb();
$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);


// ** Handle User Actions **

if ($_POST['add_server'])
{
    switch ($_POST['type'])
    {
        case 'cm' :
            Utils::redirect("add_call_manager.php");
        case 'sip' :
            Utils::redirect("sip_domain_add.php?type=" . SipDomainType::CISCO);
        case 'ietf_sip' :
            Utils::redirect("sip_domain_add.php?type=" . SipDomainType::IETF);            
        default:
            Utils::redirect('add_ipt.php?type=' . $_POST['type']);
    }
}
if ($_POST['edit_group'])
    Utils::redirect("edit_group.php?id=" . $_POST['group_id']);
if ($_POST['create_group'])
    Utils::redirect("edit_group.php?type=" . $_POST['group_type']);


// ** Retrieve Data **

// Retrieve special telephony servers
$call_managers = $db->GetAll("SELECT * FROM mce_call_manager_clusters ORDER BY name ASC");
$sip_domains = $db->GetAll("SELECT * FROM mce_sip_domains ORDER BY domain_name ASC");
for ($i = 0; $i < sizeof($sip_domains); ++$i)
{
    $sip_domains[$i]['display_type'] = SipDomainType::display($sip_domains[$i]['type']);
}

// Retrieve other Telephony Servers
$exclude_types[] = ComponentType::SCCP_DEVICE_POOL;
$exclude_types[] = ComponentType::CTI_DEVICE_POOL;
$exclude_types[] = ComponentType::MONITORED_CTI_DEVICE_POOL;
$exclude_types[] = ComponentType::CTI_ROUTE_POINT;
$exclude_types[] = ComponentType::SIP_DEVICE_POOL;
$exclude_types[] = ComponentType::SIP_TRUNK_INTERFACE;
$exclude_types[] = ComponentType::IETF_SIP_DEVICE_POOL;
$components = $db->GetAll("SELECT * FROM mce_components WHERE type >= ? AND type <= ? AND type NOT IN (".implode(',',$exclude_types).") ORDER BY name", 
                          array(IPT_TYPE_ENUM_START, IPT_TYPE_ENUM_END));
for ($i = 0; $i < sizeof($components); ++$i)
{
    $components[$i]['display_type'] = ComponentType::describe($components[$i]['type']);
}

$telephony_server_types['cm'] = "Unified Communications Manager";
$telephony_server_types['sip'] = "Cisco SIP Domain";
$telephony_server_types['ietf_sip'] = "IETF SIP Domain";
$telephony_server_types[ComponentType::H323_GATEWAY] = ComponentType::describe(ComponentType::H323_GATEWAY);


$cr_group_types[GroupType::SCCP_GROUP] = GroupType::display(GroupType::SCCP_GROUP);
$cr_group_types[GroupType::CTI_SERVER_GROUP] = GroupType::display(GroupType::CTI_SERVER_GROUP);
$cr_group_types[GroupType::H323_GATEWAY_GROUP] = GroupType::display(GroupType::H323_GATEWAY_GROUP);
$cr_group_types[GroupType::SIP_GROUP] = GroupType::display(GroupType::SIP_GROUP);


// ** Render Page **

$template_vars = array( 'call_managers'             => $call_managers,
                        'sip_domains'               => $sip_domains,
                        'components'                => $components,
                        '_component_edit_page'      => 'edit_ipt.php',
                        'telephony_server_types'    => $telephony_server_types,
                        'cr_group_types'            => $cr_group_types,
                        'groups'                    => ComponentUtils::get_call_route_groups());
                        
$page = new Layout();
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->SetPageTitle("Telephony Servers");
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            'Telephony Servers'));
$page->mTemplate->assign($template_vars);
$page->Display('telephony.tpl');

?>