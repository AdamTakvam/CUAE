<?php

require_once("init.php");
require_once("3rdparty/pclzip/pclzip.lib.php");
require_once("lib.SystemConfig.php");
require_once("lib.SystemUtils.php");
require_once("lib.DateUtils.php");
require_once("function.rmdirr.php");
require_once("class.PageLogic.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$pl = new PageLogic();
$page = $_REQUEST['p'];


function sort_by_name($a, $b)
{
    if ($a['name'] == $b['name'])
        return 0;
    else
        return ($a['name'] > $b['name']) ? 1 : -1;
}

function sort_by_time($a, $b)
{
    if ($a['timestamp'] == $b['timestamp'])
        return 0;
    else
        return ($a['timestamp'] > $b['timestamp']) ? -1 : 1;

}


// ** BASIC INPUT CHECKS **

if (!is_dir(LOGS_ROOT))
{
    die("There is no logs directory.  Please make sure that the directory " . SystemUtils::clean_path(METREOS_ROOT) . "/Logs exists.");
}

// Retrieve desired path for log files
$initial_log_path = LOGS_ROOT . '/';
$current_path = urldecode($_REQUEST['f']);
// Trying to reach below the root log path is uncool
if ($current_path == "./" || ereg("\.\.", $current_path))
    unset($current_path);
$real_path = $initial_log_path . $current_path;
$log_dir = dir($real_path);


// -- DELETE LOGS --

if ($_POST['submit_delete'])
{
    foreach ($_POST['logs'] as $log)
    {
        if (!@rmdirr($initial_log_path . $log))
            $no_del[] = $log;
    }
    if (empty($no_del))
    {
        $response = "Selected logs have been deleted.";
        EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::LOGS_DELETED, print_r($_POST['logs'],TRUE));  
    }
    else
        $errors->Add("Some logs could not be deleted:\n" . implode("\n",$no_del));
}


// -- CREATE ARCHIVE OF FILES --

if ($_POST['submit_archive'])
{
    $logfile = SystemUtils::get_hostname() . MceConfig::LOG_ARCHIVE_SUFFIX . date("-Ymd") . ".zip";
    
    @mkdir(MceConfig::LOG_TEMP, 0700, TRUE);
    @unlink(MceConfig::LOG_TEMP . $logfile);
    
    $archive = new PclZip(MceConfig::LOG_TEMP . $logfile);
    foreach ($_POST['logs'] as $archive_file)
    {
        if (!$archive->Add($initial_log_path . $archive_file, PCLZIP_OPT_REMOVE_PATH, $real_path))
        {
            $error[] = $archive_file . " could not be archived.";
            if (MceConfig::DEV_MODE)
            {
                $error[] = "PclZip Error: " . $archive->errorName();
                $error[] = "PclZip Error Code: " . $archive->errorCode();
                $error[] = "PclZip Error Info: " . $archive->errorInfo();
            }
            $errors->Add(implode("\n", $error));
        }
    }

    if ($errors->IsEmpty())
    {
        EventLog::log(LogMessageType::AUDIT, 'Logs have been archived', LogMessageId::LOGS_ARCHIVED, print_r($_POST['logs'],TRUE));     
        Utils::redirect("log_archive.php?f=$logfile");
    }
}


// -- CREATE FILE LIST --

$pl->SetItemCount(sizeof(scandir($real_path)) - 2);
$pl->SetCurrentPageNumber($page);
$pl->Calculate();

$files = array();
while ($file = $log_dir->read())
{
    if (!in_array($file, array('.','..')))
    {
        $file_handle = array();
        $file_handle['dir'] = is_dir($real_path . $file);
        if ($file_handle['dir'])
            $file .= "/";
        $file_handle['name'] = $file;
        $file_handle['file_path'] = $current_path . $file;
        $file_handle['timestamp'] = filemtime($real_path . $file);
        $file_handle['file_size'] = is_file($real_path . $file) ? number_format(filesize($real_path . $file)) : 0;

        $files[] = $file_handle;
    }
}
$log_dir->close();

if (empty($current_path))
    usort($files, 'sort_by_name');
else
    usort($files, 'sort_by_time');

$file_list = array_slice($files, $pl->sql_start, $pl->sql_limit);

if (!empty($current_path))
{
    $parent = array(    'dir'       => TRUE, 
                        'name'      => 'PARENT DIRECTORY', 
                        'file_path' => dirname($current_path) . '/');
    $file_list = array_merge(array($parent),$file_list);
}


// -- RENDER PAGE --

$tpl_vars = array(  'current_path'  =>      $current_path,
                    'files'         =>      $file_list );

$page = new Layout();
$page->SetPageTitle("Server Logs");
$page->SetBreadCrumbs(array('<a href="main.php">Main Control Panel</a>', 'Server Logs'));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign_by_ref('page_logic', $pl);
$page->mTemplate->assign($tpl_vars);
$page->Display('logs.tpl');

?>