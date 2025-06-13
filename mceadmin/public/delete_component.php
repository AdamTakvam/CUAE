<?php

require_once("init.php");
Utils::require_directory("components");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$errors = new ErrorHandler();
$type = $_REQUEST['type'];
$id = $_REQUEST['id'];

$class = ComponentType::get_class_name($type);
$component = new $class;
$component->SetErrorHandler($errors);
$component->SetId($id);
$component->Build();

$name = $component->GetName();
$edit_page = $component->mEditPage;
$list_page = $component->mListPage;

if ($_POST['submit_no'])
{
    Utils::redirect("$edit_page?id=$id");
}
else if ($_POST['submit_yes'])
{
    $component->Uninstall();
    
    $db = new MceDb();
    $test = $db->GetOne("SELECT COUNT(*) FROM mce_components WHERE mce_components_id = ?", $id);
    if (empty($test))
    {
        $response = ComponentType::describe($type) . " $name has been deleted.";
        $q_char = ereg("\?", $list_page) ? '&' : '?'; 
        Utils::redirect("$list_page" . $q_char . "s_response=" . Utils::safe_serialize($response));
    }
    else
    {
        Utils::redirect("$edit_page?id=$id&s_errors=" . Utils::safe_serialize($errors));
    }
}


// -- RENDER PAGE --

$page = new Layout();
$page->SetPageTitle("Uninstall " . $component->GetName());
$page->TurnOffNavigation();
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('component_name', $component->GetName());
$page->mTemplate->assign($_REQUEST);
$page->Display('delete_component.tpl');

?>