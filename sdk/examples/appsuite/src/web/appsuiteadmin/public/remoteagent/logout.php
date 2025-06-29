<?php

require_once("remoteagent_init.php");

// Destroy the session

$access = new RemoteAgentAccessControl();
$access->Destroy();
        
$page = new RemoteAgentLayout();
$page->SetPageTitle("Logged Out");
$page->TurnOffNavigation();
$page->Display("logout.tpl");

?>