<?php

require_once("init.php");
require_once("lib.LdapUtils.php");
require_once("lib.TimeZoneUtils.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

Utils::trim_array($_POST);

$db = new MceDb();
$errors = new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);

$admin_password = ConfigUtils::get_global_config(GlobalConfigNames::ADMIN_PASSWORD);
$raw_cfgs = $db->GetAll("SELECT * FROM as_configs WHERE as_applications_id IS NULL");
foreach ($raw_cfgs as $cfg)
{
    $configs[$cfg['name']] = $cfg['value'];
}


if ($_POST['done'])
{
    Utils::redirect('main.php');
}

if ($_POST['submit'])
{

    foreach($configs as $key => $value)
    {
        if (isset($_POST[$key]))
            $submitted[$key] = $_POST[$key];
    }
    
    // -- Validate submitted values
    // Validate change of password (if the user is trying to change it)
    if (!empty($_POST['new_password']))
    {
        if (Utils::encrypt_password($_POST['password']) != $admin_password) 
        {
            $errors->Add("Password is incorrect.");
        }
        if ($_POST['new_password'] != $_POST['new_password_verify'])
        {
            $errors->Add("New password is blank or could not be verified.");
        }    
    }

    if (!Utils::is_pos_int($submitted['default_lockout_threshold']))
        $errors->Add('Default Lockout Threshold is not a valid value');
    if (!Utils::is_pos_int($submitted['default_lockout_duration']))
        $errors->Add('Default Lockout Duration is not a valid value');
    if (!Utils::is_pos_int($submitted['default_max_concurrent_sessions']))
        $errors->Add('Default Max Concurrent Sessions is not a valid value');
    if (!Utils::is_pos_int($submitted['media_ports']))
        $errors->Add('Media Ports must be greater than 0');
    if (!Utils::is_pos_int($submitted['smtp_port'],TRUE))
        $errors->Add('SMTP Port is not a valid port number');
    if (ereg('^[0-9\.]+$',$submitted['smtp_server']) && !Utils::validate_ip($submitted['smtp_server']))
        $errors->Add('SMTP Server must be a recognizable hostname or valid IP address');
    if (!Utils::is_pos_int($submitted['recordings_expiration']))
        $errors->Add("Recordings Expiration must be 0 or greater");
    if (!Utils::is_pos_int($submitted['scheduled_conference_dn']))
        $errors->Add("Scheduled Confernce DN is not valid");
    
    // -- Update configurations
    if ($errors->IsEmpty())
    {
        $u_query = "UPDATE as_configs SET value = ? WHERE name = ?";
        if (!empty($_POST['new_password']))
        {
            $db->Execute($u_query, array(Utils::encrypt_password($_POST['new_password']), GlobalConfigNames::ADMIN_PASSWORD));
            $access->UpdatePassword($_POST['new_password']);
        }
        foreach ($submitted as $name => $data)
        {
            $db->Execute($u_query, array($submitted[$name], $name));
        }
        $response = "System configuration updated";
    }

    // Put submitted values in config data to display
    foreach ($configs as $name => $data)
    {
        $configs[$name] = $submitted[$name];
    }

}


// ** Render page **

// Retrieve LDAP servers
$ldap_servers = $db->GetAll("SELECT * FROM as_ldap_servers");

$page = new Layout();
$title = "System Management";
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->Assign($configs);
$page->mTemplate->assign('timezone_list', TimeZoneUtils::get_timezone_list());
$page->Display("system_mgmt.tpl");

?>