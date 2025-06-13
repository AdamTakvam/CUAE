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

$licensed_stats[STATS_OID_APPSESSIONS] = array("name" => "Application Sessions");
$licensed_stats[STATS_OID_VOICE] = array("name" => "Voice Ports");
$licensed_stats[STATS_OID_RTP] = array("name" => "RTP Ports");
$licensed_stats[STATS_OID_ERTP] = array("name" => "Enhanced RTP Ports");
$licensed_stats[STATS_OID_CONFERENCE] = array("name" => "Conference Ports");
$licensed_stats[STATS_OID_SPEECHINTEG] = array("name" => "Speech Integration Ports");
$licensed_stats[STATS_OID_TTS] = array("name" => "Text To Speech Ports");


// ** Handle uploaded licenses **

if ($_POST['upload'])
{
    if (0 == $_FILES['upload_license']['error'])
    {
        if (LicenseManagement::verify_license_file($_FILES['upload_license']))
        {
	        LicenseManagement::place_license_file($_FILES['upload_license']);
	        LicenseManagement::refresh_license_manager();
	        $feedback = "The license file uploaded successfully.";
        }
        else
        {
            $errors->Add("This is not a valid license file.  Please try another file.");
        }
    }
    else
    {
        $errors->Add("The license file upload failed.");
    }
}


// ** Retrieve license file list **

$license_file_list = array();
if (is_dir(LICENSES_PATH))
{
	$dirh = opendir(LICENSES_PATH);
	while (false !== ($file = readdir($dirh)))
	{
	    // Don't show "default" license file.  They don't need to be messing with that.
	    if (!is_dir($file) && !ereg("^\.",$file))
	    {
	        $license_file_list[] = $file;
	    }
	}
	closedir($dirh);
}


// ** Handle file deletion **
// Done after retrieving the file list to verify that there is such a file

if ($_REQUEST['del'])
{
    $file_name = $_REQUEST['del'];
    $found_index = array_search($file_name, $license_file_list);
    if ((false !== $found_index) && unlink(LICENSES_PATH . '/' . $file_name))
    {
        unset($license_file_list[$found_index]);
	    LicenseManagement::refresh_license_manager();
        $feedback = "The license file $file_name was deleted.";
    }
}


// ** Retrieve statistical data **

$stats_if = new StatsServerInterface();
if (!$stats_if->Connected())
{
    $errors->Add("No active statistics were retrieved because a connection to the stats server could not be established.");
}

foreach ($licensed_stats as $oid => $data)
{
    $licensed_stats[$oid]['max'] = LicenseManagement::get_license_max($oid);
    $licensed_stats[$oid]['total'] = LicenseManagement::get_license_total($oid);
    $licensed_stats[$oid]['active'] = LicenseManagement::get_license_active($oid);
}


// ** Render page **

$title = 'License Management';
$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);

$page->mTemplate->Assign('license_file_list', $license_file_list);
$page->mTemplate->Assign('cuae_license_mode', LicenseManagement::get_cuae_license_mode());
$page->mTemplate->Assign('cume_license_mode', LicenseManagement::get_cume_license_mode());
$page->mTemplate->Assign('license_values', $licensed_stats);
$page->Display('license_mgmt.tpl');

?>