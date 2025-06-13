<?php

define('INSTALL_WIZARD_PAGE',	TRUE);
define('ADMINISTRATOR_ID',      1);

require_once("init.php");
require_once("lib.SystemConfig.php");
require_once("lib.RegistryUtils.php");


$access = new AccessControl();
$access->CheckWizardAccess();

$errors = new ErrorHandler();

if ($_POST['confirm'])
{	
    if (Utils::is_blank($_POST['media_password']))
        $errors->Add("The media engine password cannot be blank.");
    if ($_POST['media_password'] <> $_POST['media_password_verify'])
        $errors->Add("The media engine password could not be verified.  Please try again.");

    if (Utils::is_blank($_POST['administrator_password']))
        $errors->Add("The administrator password cannot be blank.");
    if ($_POST['administrator_password'] <> $_POST['administrator_password_verify'])
        $errors->Add("The administrator password could not be verified.  Please try again.");
        
	if ($errors->IsEmpty())
	{
        $db = new MceDb();
        $db->Execute("UPDATE mce_users SET password=? WHERE mce_users_id=?", array(Utils::encrypt_password($_POST['administrator_password']), ADMINISTRATOR_ID));

        SystemConfig::store_config(SystemConfig::MEDIA_PROVISION_PASSWORD, Utils::encrypt_password($_POST['media_password']));
        
        touch(INSTALL_DONE_FILE);
		$access->Destroy();
		Utils::redirect('/mceadmin/page.php?wizard/finished');
	}
}


$page = new Layout();
$page->SetPageTitle('Install Wizard: Set Passwords');
$page->TurnOffNavigation();
$page->SetErrorMessage($errors->Dump());
$page->Display('wizard/set_password.tpl');

?>