<?php

require_once('init.php');
require_once('lib.LicenseManagement.php');
require_once('class.StatsServerInterface.php');


// ** Set up **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$feedback = NULL;

$oids[STATS_OID_APPSESSIONS] = array("name" => "Application Sessions");
$oids[STATS_OID_CALLS] = array("name" => "Calls");
$oids[STATS_OID_VOICE] = array("name" => "Voice Ports");
$oids[STATS_OID_RTP] = array("name" => "RTP Ports");
$oids[STATS_OID_ERTP] = array("name" => "Enhanced RTP Ports");
$oids[STATS_OID_CONFERENCE] = array("name" => "Conference Ports");
$oids[STATS_OID_CONFERENCE_USE] = array("name" => "Conferences");
$oids[STATS_OID_TTS] = array("name" => "Text To Speech Ports");


// ** Retrieve statistical data **

$stats_if = new StatsServerInterface();
if (!$stats_if->Connected())
{
    $errors->Add("No active statistics were retrieved because a connection to the stats server could not be established.");
}

foreach ($oids as $oid => $data)
{
    $oids[$oid]['active'] = LicenseManagement::get_license_active($oid);
}


// ** Render page **

$title = 'Statistics';
$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);

$page->mTemplate->Assign('oids', $oids);
$page->Display('stats.tpl');

?>