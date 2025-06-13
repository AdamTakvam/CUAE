<?php

/* 
 */

require_once("init.php");
require_once("class.SipDomain.php");


function get_domain_members_of_type($type, $id)
// Retrieves all the members of a certain type for the domain
{
    $db = new MceDb();
    $members = $db->GetAll("SELECT * FROM mce_sip_domain_members JOIN mce_components USING (mce_components_id) " .
                           "WHERE mce_sip_domains_id = ? AND type = ?", array($id, $type));
    return $members;
}


// ** Setup page **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = new ErrorHandler();
Utils::trim_array($_POST);
$id = $_REQUEST['id'];

$type = $db->GetOne("SELECT type FROM mce_sip_domains WHERE mce_sip_domains_id = ?", array($id));

if ($type == SipDomainType::CISCO)
    $dp_type = ComponentType::SIP_DEVICE_POOL;
else if ($type == SipDomainType::IETF)
    $dp_type = ComponentType::IETF_SIP_DEVICE_POOL;

$sip_domain = new SipDomain();
$sip_domain->SetId($id);
$sip_domain->SetErrorHandler($errors);

$response = Utils::safe_unserialize($_REQUEST['s_response']);


// ** Redirect to the proper page based on the posted actions **

if ($_POST['edit_device_pool'])
{
    $edit_id = Utils::get_first_array_key($_POST['edit_device_pool']);
    if ($dp_type == ComponentType::SIP_DEVICE_POOL)
        Utils::redirect("sip_device_pool_edit.php?id=$edit_id");
    else if ($dp_type == ComponentType::IETF_SIP_DEVICE_POOL)
        Utils::redirect("ietf_sip_device_pool_edit.php?id=$edit_id");
}

if ($_POST['manage_device_pool'])
{
    $edit_id = Utils::get_first_array_key($_POST['manage_device_pool']);
    if ($dp_type == ComponentType::SIP_DEVICE_POOL)
        Utils::redirect("manage_device_pool.php?id=$edit_id&type=$dp_type");
    else if ($dp_type == ComponentType::IETF_SIP_DEVICE_POOL)
        Utils::redirect("ietf_sip_device_pool_manage.php?id=$edit_id");
}

if ($_POST['uninstall'])
{
    Utils::redirect("sip_domain_delete.php?id=$id");
}


// ** Update CallManager cluster configurations **

if ($_POST['update'])
{
    $sd_data = $_POST['sip_domain_data'];
    if (!eregi("^[a-z0-9-]+(\.[a-z0-9-]+)+$", $sd_data['domain_name']))
        $errors->Add("Domain name is not valid");

    if ($errors->IsEmpty())
    {
        $sip_domain->Update($_POST['sip_domain_data']);
        $response = "The " . SipDomainType::display($type) . " has been updated.";
    }
}
else
{
    $sd_data = $sip_domain->GetData();
}


// ** Retrieve SIP Domain members**

$device_pools = get_domain_members_of_type($dp_type, $id);


// ** Render and display the page **

$template_vars = array( 'id'                    => $id,
                        'sip_device_pools'      => $device_pools,
                        'sip_domain_data'       => $errors->IsEmpty() ? $sd_data : $_POST['sip_domain_data'],
                        'type'                  => $type,
                        'type_display'          => SipDomainType::display($type),
                        'dp_type'               => $dp_type,
                        'dp_type_display'       => ComponentType::describe($dp_type),
                        );

$page = new Layout();
$page->SetPageTitle($sd_data['domain_name']);
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            SipDomainType::display($type) . ': ' . $sd_data['domain_name']));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($template_vars);
$page->Display('sip_domain_edit.tpl');

?>