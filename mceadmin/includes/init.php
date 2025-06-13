<?php

// Files commonly required for use for a typical page of the
// management console

require_once("common.php");
require_once("class.Layout.php");
require_once("class.AccessControl.php");
require_once("exception_handlers.php");


// In a production setting, PHP errors should be caught and then logged
// into the Management logs
if (!MceConfig::DEV_MODE)
    set_error_handler(array('ErrorLog','log'), (int) ini_get('error_reporting'));

// If the console and environment has never been configured (as indicated
// by a lack of a specific file), then send the user to the install wizard.

if (!INSTALL_PERFORMED && !defined('INSTALL_WIZARD_PAGE'))
{
    Utils::redirect(WEB_PATH . "/wizard/");
}

?>
