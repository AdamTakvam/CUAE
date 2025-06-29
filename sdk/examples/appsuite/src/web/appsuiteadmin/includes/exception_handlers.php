<?php

require_once("class.Layout.php");

function _mce_exception_handler(Exception $ex)
{
    $page = new Layout();
    $page->TurnOffNavigation();
    $page->mTemplate->assign_by_ref('exception',$ex);
    $page->Display("uncaught_exception.tpl");
}

function _mce_db_connect_exception_handler(Exception $ex)
{
    $page = new Layout();
    $page->TurnOffNavigation();
    $page->Display("db_exception.tpl");
    restore_exception_handler();
}

// Default exception handler
set_exception_handler('_mce_exception_handler');

?>