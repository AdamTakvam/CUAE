<?php

require_once("init.php");
require_once("class.SipDomain.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = new ErrorHandler();
Utils::trim_array($_POST);

$type = isset($_REQUEST['type']) ? $_REQUEST['type'] : SipDomainType::CISCO;


// ** Create SIP Domain **

if ($_POST['submit'])
{
    if (!eregi("^[a-z0-9-]+(\.[a-z0-9-]+)+$", $_POST['domain_name']))
        $errors->Add("Domain name is not valid");
        
    if ($errors->IsEmpty())
    {
	    $db = new MceDb();
	    $count = $db->GetOne("SELECT COUNT(domain_name) FROM mce_sip_domains WHERE domain_name = ?", array($_POST['domain_name']));
	    if ($count > 0)
	        $errors->Add("A SIP domain with the name '" . $_POST['domain_name'] . "' already exists");
    }
    
    if ($errors->IsEmpty())
    {
        $sip_domain = new SipDomain();
        $sip_domain->SetErrorHandler($errors);
        $id = $sip_domain->Create($type, $_POST['domain_name'],$_POST['primary_registrar'],$_POST['secondary_registrar'],$_POST['outbound_proxy']);
        
        if ($errors->IsEmpty())
        {
            $response = "SIP Domain created for " . $_POST['domain_name'];
            Utils::redirect('sip_domain_edit.php?id=' . $id . '&s_response=' . Utils::safe_serialize($response));
        }
    }
}


// ** Render Page **

$page = new Layout();
$title = "Add " . SipDomainType::display($type);
$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = '<a href="telephony.php">Telephony Servers</a>';
$breadcrumbs[] = $title;

$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
if (!$errors->IsEmpty())
    $page->mTemplate->Assign($_POST);
$page->mTemplate->Assign('type',$type);
$page->Display('sip_domain_add.tpl');

?>