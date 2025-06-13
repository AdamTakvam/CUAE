<?php

require_once("init.php");
require_once("lib.SystemConfig.php");
require_once("lib.RegistryUtils.php");


// ** Setup **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();

define('BASE_KEYNAME',  'HKLM\SOFTWARE\Dialogic\Installed Boards\DM3\HMP_Software #0 in slot 0/65535');
define('IP_VALUENAME',  'DefaultIPAddress');
define('MAC_VALUENAME', 'DefaultMACAddress');


// ** Handle User Requests **

// Change password
if ($_POST['submit'])
{
    if ($errors->IsEmpty())
    {
        if (strlen($_POST['new_password']) < MIN_SYSTEM_USER_PASSWORD_LENGTH)
            $errors->Add('The new password must be seven characters or longer');
        if ($_POST['new_password'] != $_POST['verify_new_password'])
            $errors->Add('The new password could not be verified');
    }
    
    if ($errors->IsEmpty())
    {
        try
        {
            SystemConfig::store_config(SystemConfig::MEDIA_PROVISION_PASSWORD, Utils::encrypt_password($_POST['new_password']));
            $response = "The media engine password was changed";
            EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::MEDIA_SERVER_SERVICE_UPDATED);
        }
        catch (Exception $e)
        {
            $errors->Add("There was a failure updating the password on the system.");
            if (MceConfig::DEV_MODE)
                $errors->Add($e->GetMessage());
            else
                ErrorLog::raw_log(print_r($e,TRUE));
        }
    }
}

// Change default addresses
if ($_POST['address_submit'])
{
    if (Utils::is_blank($_POST['default_ip']))
        $errors->Add("A default IP address is required");
    if (Utils::is_blank($_POST['default_mac']))
        $errors->Add("A default MAC address is required");
    if (!Utils::validate_ip($_POST['default_ip']))
        $errors->Add("The default IP address is not valid");
    if (!Utils::validate_mac_address($_POST['default_mac']))
        $errors->Add("The default MAC address is not valid");
        
    if ($errors->IsEmpty())
    {
        try
        {
            RegistryUtils::set_registry_value(BASE_KEYNAME,IP_VALUENAME,$_POST['default_ip']);
            RegistryUtils::set_registry_value(BASE_KEYNAME,MAC_VALUENAME,$_POST['default_mac']);          
            $response = "Default addresses have been updated.  The media engine must be restarted for these changes to take effect.";
            EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::MEDIA_SERVER_SERVICE_UPDATED);
        }
        catch (Exception $e)
        {
            $errors->Add("There was a failure updating the default addresses on the system.");
            if (MceConfig::DEV_MODE)
                $errors->Add($e->GetMessage());
            else
                ErrorLog::raw_log(print_r($e,TRUE));
        }
    }
}

// ** Render page **

$page = new Layout();
$page->SetPageTitle('Media Engine Configuration');
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>','Local Media Engine Configuration'));
if ($_POST['address_submit'] && !$errors->IsEmpty())
{
    $page->mTemplate->Assign($_POST);
}
else
{
    try
    {
        $page->mTemplate->Assign('default_ip', RegistryUtils::read_registry_value(BASE_KEYNAME,IP_VALUENAME));
        $page->mTemplate->Assign('default_mac', RegistryUtils::read_registry_value(BASE_KEYNAME,MAC_VALUENAME));
    }
    catch (Exception $e)
    {
        // Perhaps it could not find the registry values.  Output to the template that there are currently no
        // settings for these values.
    }
}
$page->Display('media_server_config.tpl');

?>