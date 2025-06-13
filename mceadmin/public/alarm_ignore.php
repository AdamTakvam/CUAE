<?php

require_once("init.php");

// ** SET UP **

$access = new AccessControl();
$admin_access = $access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$feedback = NULL;
$title = "Ignored Alarms";


// ** REDIRECT FINISH **

if ($_POST['done'])
{
    Utils::redirect('alarm_list.php');
}


// ** HANDLE UPDATES **

if ($_POST['submit'])
{
    $db->Execute("UPDATE mce_snmp_mib_defs SET `ignore`=0 WHERE type=1");
    if (isset($_POST['ignore']))
    {
        $marks = implode(',',array_fill(0, sizeof($_POST['ignore']),'?'));
        $sqlt = "UPDATE mce_snmp_mib_defs SET `ignore`=1 WHERE type=1 AND oid IN ($marks)";
        $db->Execute($sqlt, $_POST['ignore']);
    }
    $feedback = "Ignored alarm list has been updated.";
}


// ** RETRIEVE ALARMS **

$sql = "SELECT * FROM mce_snmp_mib_defs WHERE type=1 AND oid BETWEEN ? AND ? ORDER BY oid ASC";

// General Alarms
$alarms_gen = $db->GetAll($sql, array(100,199));
// Media Server Alarms
$alarms_media = $db->GetAll($sql, array(200,299));
// Application Server Alarms
$alarms_as = $db->GetAll($sql, array(300,399));
// Licensing Alarms
$alarms_lic = $db->GetAll($sql, array(400,499));


// ** RENDER PAGE **

$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = '<a href="alarm_list.php">Alarm Management</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetResponseMessage($feedback);

$page->mTemplate->Assign("alarms_gen", $alarms_gen);
$page->mTemplate->Assign("alarms_media", $alarms_media);
$page->mTemplate->Assign("alarms_as", $alarms_as);
$page->mTemplate->Assign("alarms_lic", $alarms_lic);
$page->Display("alarm_ignore.tpl");

?>