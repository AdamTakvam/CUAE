<?php

require_once("init.php");
require_once("lib.LicenseManagement.php");


// ** Set up **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$feedback = NULL;

$r_oid = $_REQUEST['oid'];
$r_interval = $_REQUEST['range'];

$oids[STATS_OID_APPSESSIONS] = array("name" => "Application Sessions");
$oids[STATS_OID_CALLS] = array("name" => "Calls");
$oids[STATS_OID_VOICE] = array("name" => "Voice Ports");
$oids[STATS_OID_RTP] = array("name" => "RTP Ports");
$oids[STATS_OID_ERTP] = array("name" => "Enhanced RTP Ports");
$oids[STATS_OID_CONFERENCE] = array("name" => "Conference Ports");
$oids[STATS_OID_CONFERENCE_USE] = array("name" => "Conferences");
$oids[STATS_OID_TTS] = array("name" => "Text To Speech Ports");
    

// ** Dingy util function **

function format_interval_name($interval)
{
    switch ($interval)
    {
        case STATS_INTERVAL_6HOURS:
            return "6 Hours";
        case STATS_INTERVAL_12HOURS:
            return "12 Hours";
        default:
            return $interval;
    }
}


// ** Determine request and generate graphs **

$graphs = array();
if ($r_oid > 0)
{
    $set_name = $oids[$r_oid]['name'];
    $files = LicenseManagement::generate_oid_graphs($r_oid);
    $oid_value = LicenseManagement::get_license_active($r_oid);
    foreach ($files as $interval => $data)
    {
        $graphs[$interval] = array("name" => "Last " . format_interval_name($interval), "filename" => basename($data));
    }
}
else if (!empty($r_interval))
{
    $set_name = "Last " . format_interval_name($r_interval);
    $files = LicenseManagement::generate_interval_graphs($r_interval, $oids);
    foreach ($files as $oid => $data)
    {
        $graphs[$oid] = array("name" => $oids[$oid]['name'], "filename" => basename($data), "value" => LicenseManagement::get_license_active($oid));
    }
}


// ** Render page **

$title = 'Statistics: ' . $set_name;
$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = '<a href="stats.php">Statistics</a>';
$breadcrumbs[] = $set_name;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);

$page->mTemplate->Assign('oid', $r_oid);
$page->mTemplate->Assign('oid_value', $oid_value);
$page->mTemplate->Assign('interval', $r_interval);
$page->mTemplate->Assign('set_name', $set_name);
$page->mTemplate->Assign('graphs', $graphs);

$page->Display('stats_view.tpl');

?>