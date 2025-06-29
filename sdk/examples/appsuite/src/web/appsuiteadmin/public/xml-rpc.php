<?php
// xml-rpc.php
//
//  MCE XML-RPC interface
//
include_once("lib.xmlrpc.php");
include_once("class.WebService.php");

$dao = new WebService();
XMLRPCService($HTTP_RAW_POST_DATA, $dao);
?>
