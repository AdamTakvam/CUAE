<?php

require_once("init.php");
require_once('lib.LicenseManagement.php');

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$file = urldecode($_SERVER['argv'][0]);
if (ereg("\.\.", $file))
	die('Illegal license file');
$filename = LICENSES_PATH . '/' . $file;

if (is_file($filename))
{
	$page = new Template();
	$resources = array();    
	
    $fp = fopen($filename, 'r');
    while (!feof($fp))
    {
        $line = fgets($fp);
        
        if (ereg('^SERVER ([^[:space:]]+) ([[:alnum:]]+)', $line, $regs))
        {
            $page->Assign('hostname', $regs[1]);
            $page->Assign('mac', $regs[2]);
            continue;
        }
        
        if (ereg('^FEATURE ([^[:space:]]+) [^[:space:]]+ ([[:alnum:]\.]+) ([^[:space:]]+)', $line, $regs))
        {
            $page->Assign('feature', $regs[1]);
            $page->assign('version', $regs[2]);
            $page->assign('expires', $regs[3]);
            continue;
        }
        
        if (ereg('^INCREMENT ([^[:space:]]+) [^[:space:]]+ ([[:alnum:]\.]+) ([^[:space:]]+) ([0-9]+)', $line, $regs))
        {
            $data = array();
            $data['name'] = $regs[1];
            $data['version'] = $regs[2];
            $data['expires'] = $regs[3];
            $data['value'] = $regs[4];
            $resources[] = $data;
            continue;
        }
    }
    fclose($fp);

    $page->Assign('resources', $resources);
    $page->Assign('file', $file);
    $page->display('license_details.tpl');    
}
else
{
    die('No such file is found');
}
?>