<?php

require_once("init.php");

// LAYOUT

$page = new Layout();
$page->TurnOffNavigation();
$page->Display($_SERVER['argv'][0] . ".tpl");

?>