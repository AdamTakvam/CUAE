<?php

require_once("init.php");
require_once("lib.ConfigUtils.php");
require_once("class.ReplicationManagement.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$rp = new ReplicationManagement();
$rp_settings = $rp->GetSettings();


$page = new Layout();
$page->SetPageTitle("Main Page");
$page->SetBreadcrumbs(array('Home'));
$page->mTemplate->Assign('admin_access', $access->CheckAccess(AccessControl::ADMINISTRATOR));
$page->mTemplate->Assign('group_admin_access', $access->CheckAccess(AccessControl::GROUP_ADMIN));
$page->mTemplate->Assign('is_slave_server', $rp_settings['role'] == ReplicationRole::SLAVE);
$page->mTemplate->Assign('master_server', $rp_settings['host']);
$page->mTemplate->Assign('user_id', $access->GetUserId());
$page->mTemplate->Assign('app_sc_exposed', ConfigUtils::is_application_exposed(ApplicationID::SCHEDULED_CONFERENCING));
$page->mTemplate->Assign('app_i_exposed', ConfigUtils::is_application_exposed(ApplicationID::INTERCOM_TALKBACK));
$page->mTemplate->Assign('app_ar_exposed', ConfigUtils::is_application_exposed(ApplicationID::ACTIVE_RELAY));
$page->mTemplate->Assign('app_rr_exposed', ConfigUtils::is_application_exposed(ApplicationID::RAPID_RECORD));
$page->Display("main.tpl");

?>