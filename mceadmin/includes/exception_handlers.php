<?php

require_once("common.php");


function _mce_exception_handler(Exception $ex)
{
	ErrorLog::raw_log(print_r($ex, TRUE));
    $page = new Template();
	if (MceConfig::DEV_MODE)
	{
	    $page->assign_by_ref('exception',$ex);
	    $page->Display("uncaught_exception.tpl");
	}
	else
	{
		$page->Display("uncaught_exception_live.tpl");
	}
}


function _mce_db_connect_exception_handler(Exception $ex)
{
    $page = new Template();
    $page->assign_by_ref("exception", $ex);
    $page->assign("_dev_mode", MceConfig::DEV_MODE);
    $page->Display("db_exception.tpl");
    restore_exception_handler();
}

set_exception_handler('_mce_exception_handler');

?>