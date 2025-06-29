<?php

require_once("init.php");
require_once("class.ReplicationManagement.php");


function redirect_properly()
{
    global $is_slave;
    global $access;
    if ($is_slave)
    {
        if ($access->CheckAccess(AccessControl::ADMINISTRATOR))
        {
            Utils::redirect("replication.php");
        }
        else
        {
            $access->Destroy();
            return FALSE;
        }
    }
    Utils::redirect("main.php");
}


// ** SET UP **

$access = new AccessControl();

$rp = new ReplicationManagement();
$rp_settings = $rp->GetSettings();
$is_slave = $rp_settings['role'] == ReplicationRole::SLAVE;


// ** HANDLE ACTION **


// If a user has submitted a log in
if ($_POST['submit'])
{
    if ($access->LogIn($_POST['username'], $_POST['password']))
    {
        if (!redirect_properly())
            $error = "This appliance is a subscriber in a replication setup, and users are not allowed to log in.";        
    }
    else
        $error = "Incorrect username and/or password";
}
else if ($access->CheckLogin())
{
    redirect_properly("main.php");
}
// Destroy session data and present a login screen
else
{
    $access->Destroy();
}


// ** RENDER PAGE **

if ($_REQUEST['message'])
    $feedback = Utils::safe_unserialize($_REQUEST['message']);

$page = new Layout();
$page->TurnOffNavigation();
$page->SetErrorMessage($error);
$page->SetResponseMessage($feedback);
$page->mTemplate->Assign('is_slave_server', $rp_settings['role'] == ReplicationRole::SLAVE);
$page->mTemplate->Assign('master_server', $rp_settings['host']);
$page->Display("login.tpl");

?>