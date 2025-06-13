<?php

define('INSTALL_WIZARD_PAGE',	TRUE);
require_once("init.php");
require_once("lib.RegistryUtils.php");

$access = new AccessControl();
$access->CheckWizardAccess();


$page = new Layout();
$page->TurnOffNavigation();

if ($_POST['agree'])
{
    // User agrees to the EULA, take him to the next step
	Utils::redirect('set_password.php');
} 
else if ($_POST['disagree'])
{
    // User does not agree with the EULA, take him to a page with information
	$page->SetPageTitle("Cisco End User License Agreement");
	$page->Display('wizard/reject_eula.tpl');
}
else
{
    // Determine if the user should see the SDK EULA or the regular one
    $sdk_mode = TRUE;
    try
    {
        $os_ver = RegistryUtils::read_registry_value("HKLM\SOFTWARE\Cisco Systems, Inc.\System Info\OS Image","Version");
        if (strcmp($os_ver,"2003.1.1") >= 0)
            $sdk_mode = FALSE;
    }
    catch (Exception $ex)
    {
        // NULL ACTION
    }
    
    if ($sdk_mode)
        $page->Display('wizard/sdk_eula.tpl');
    else
        $page->Display('wizard/eula.tpl');	
}

?>