<?php

require_once("init.php");


// *** INITIALIZE OBJECTS AND VARIABLES ***

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$db = new MceDb();
$errors = $_REQUEST['s_errors'] ? Utils::safe_unserialize($_REQUEST['s_errors']) : new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);
$type = $_REQUEST['type'] ? $_REQUEST['type'] : $_SERVER[argv][0];


// *** SET PROPER TEMPLATES ***

switch ($type)
{
    case ComponentType::PROVIDER:
        $template = "provider_list.tpl";
        break;
    case ComponentType::APPLICATION:
        $template = "app_list.tpl";
        break;
    case ComponentType::CORE:
        $template = "core_list.tpl";
        break;
    default:
        throw new Exception('Component type improper for this page.');
}
$title = ComponentType::describe($type) . 's';


// *** RETRIEVE COMPONENT LISTING ***

$components = $db->GetAll("SELECT * FROM mce_components WHERE type=? ORDER BY name", array($type));

for ($i = 0; $i < sizeof($components); ++$i)
{
    $components[$i]['display_status'] = ComponentStatus::display($components[$i]['status']);
    if (Utils::is_blank($components[$i]['display_name']))
        $components[$i]['display_name'] = $components[$i]['name'];
}


// *** RENDER PAGE ***

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>', $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('type', $type);
$page->mTemplate->assign('components', $components);
$page->Display($template);

?>