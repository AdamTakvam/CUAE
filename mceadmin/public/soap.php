<?php 
// soap.php
//
require_once("SOAP/Server.php");
require_once("SOAP/Disco.php");
include_once("class.WebService.php");

$server = new SOAP_Server();
$webservice = new WebService();
$server->addObjectMap($webservice,'urn:MetreosMCEAdminService');

// start serve
if (isset($_SERVER['REQUEST_METHOD']) && $_SERVER['REQUEST_METHOD']=='POST') {
    $server->service($HTTP_RAW_POST_DATA);
} else {
    $disco = new SOAP_DISCO_Server($server, 'MetreosMCEAdminService');
    header("Content-type: text/xml");
    if (isset($_SERVER['QUERY_STRING']) && strcasecmp($_SERVER['QUERY_STRING'],'wsdl') == 0) {
        echo $disco->getWSDL();
    } else {
        echo $disco->getDISCO();
    }
}
exit;
?>

