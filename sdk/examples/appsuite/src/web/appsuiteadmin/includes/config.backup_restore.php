<?php

// Merely grab the right function library appropiate to the operating system.

require_once("config.php");
require_once("config_backup_restore-" . MceConfig::OPERATING_SYSTEM . ".php");

?>