<?php

require_once("init.php");
require_once("class.SipDomain.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = new ErrorHandler();
Utils::trim_array($_POST);
$id = $_REQUEST['id'];

$sip_domain = new SipDomain();
$sip_domain->SetId($id);
$sip_domain->SetErrorHandler($errors);


// ** Handle User Response **

if ($_POST['delete_yes'])
{
    $sip_domain->Delete();
    
    if (!$errors->IsEmpty())
        $add = "?s_errors=" . Utils::safe_serialize($errors);
    $add = "?s_response=" . Utils::safe_serialize("SIP Domain removed");
    Utils::redirect("telephony.php" . $add);
}

if ($_POST['delete_no'])
    Utils::redirect("sip_domain_edit.php?id=$id");

    
// ** Render page **

$sd_data = $sip_domain->GetData();
    
$page = new Layout();
$page->SetPageTitle("Remove " . $sd_data['domain_name']);
$page->SetBreadcrumbs(array('<a href="index.php">Main Control Panel</a>',
                            '<a href="telephony.php">Telephony Servers</a>',
                            'Remove SIP Domain: ' . $sd_data['domain_name']));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('sd_data', $sd_data);
$page->mTemplate->assign('id', $id);
$page->Display('sip_domain_delete.tpl');

?>