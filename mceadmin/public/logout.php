<?php

require_once("init.php");

// Destroy the session

$access = new AccessControl();
$username = $access->GetData('username');
$access->Destroy();
EventLog::log(LogMessageType::AUDIT, "$username logged out", LogMessageId::USER_LOGOUT);
Utils::redirect('index.php?logout=1');

?>