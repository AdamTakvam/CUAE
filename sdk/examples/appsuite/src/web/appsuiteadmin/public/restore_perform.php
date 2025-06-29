<?php

require_once("init.php");
require_once("class.Restore.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$restore = Utils::safe_unserialize($_REQUEST['s_restore']);
$step = $_REQUEST['step'];


$page = new Layout();
$page->SetPageTitle('Perform a Restore');
$page->TurnOffNavigation();
$page->mTemplate->assign('metadata', $restore->GetMetaData());
$page->mTemplate->assign('s_restore', $_REQUEST['s_restore']);
$page->mTemplate->assign('step', $step);


switch ($step)
{
    case 1:
        break;
    case 2:
        if ($_POST['restore_yes'])
            $page->AddHeadInfo('<meta http-equiv="Refresh" content="0; url=' . $_SERVER['PHP_SELF'] . '?step=3&amp;s_restore=' . $_REQUEST['s_restore'] . '">');
        if ($_POST['restore_no'])
            Utils::redirect('restore.php');
        break;
    case 3:
        $restore_good = $restore->DoRestore();
        if (!$restore_good)
            $page->SetErrorMessage($restore->GetError());
        $page->mTemplate->assign('restore_good', $restore_good);
        break;
    default:
        break;
}

$page->Display('restore_perform.tpl');

?>