<?php

require_once("init.php");

// Destroy the session

$access = new AccessControl();
$access->Destroy();

$message = "You have successfully logged out.";
Utils::redirect('index.php?message=' . Utils::safe_serialize($message));

?>