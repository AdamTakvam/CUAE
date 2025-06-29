<?php

require_once("init.php");

// Generic display page.  Simply pass in the name of the template (without the extension)
// immediately into the query.  i.e. page.php?templatename

$page = new Layout();
$page->TurnOffNavigation();
$page->Display($_SERVER[argv][0] . ".tpl");

?>