<?php

// General Detected Settings

define('MCE_APPSUITE_ROOT',                 dirname(dirname(__FILE__)));
define('DEVMODE_FILE_FOUND',                file_exists(MCE_APPSUITE_ROOT . "/devmode"));
define('MCE_APPSUITE_IMAGES_DIR',           MCE_APPSUITE_ROOT . "/public/images");

// Calculated Smarty Template Paths

define('SMARTY_DIR',                        MCE_APPSUITE_ROOT . "/includes/3rdparty/smarty/libs/");
define('SMARTY_TEMPLATE_DIR',               MCE_APPSUITE_ROOT . "/templates");
define('SMARTY_TEMPLATE_C_DIR',             MCE_APPSUITE_ROOT . "/templates_c");
define('SMARTY_CONFIG_DIR',                 SMARTY_DIR . "configs/");
define('SMARTY_CACHE_DIR',                  SMARTY_DIR . "cache/");

// Updater file constants

define('UPDATE_IN_PROGRESS_FILE',			"update_in_progress");
define('UPDATE_SUCCESSFUL_FILE',			"update_success");
define('UPDATE_FAILED_FILE',				"update_failed");

?>