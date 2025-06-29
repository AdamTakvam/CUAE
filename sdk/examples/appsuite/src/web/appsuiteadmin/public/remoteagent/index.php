<?php

require_once("remoteagent_init.php");

$access = new RemoteAgentAccessControl();

// If a user has submitted a log in
if ($_POST['submit'])
{
    if ($access->LogIn($_POST['username'], $_POST['password']))
    {
    	Utils::redirect("user.php");
    }
    else
    {
        $error = "Incorrect username and/or password";
    }
}
else if ($access->CheckLogin())
{
    Utils::redirect("user.php");
}
// Destroy session data and present a login screen
else
{
    $access->Destroy();
}


$page = new RemoteAgentLayout();
$page->TurnOffNavigation();
$page->SetErrorMessage($error);
$page->Display("login.tpl");

?>