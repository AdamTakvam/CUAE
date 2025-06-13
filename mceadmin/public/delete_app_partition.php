<?php

require_once("init.php");
require_once("components/class.Application.php");
require_once("lib.MediaServerUtils.php");


$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);

$id = $_REQUEST['id'];

$errors = new ErrorHandler();

$app_part = new ApplicationPartition();
$app_part->SetErrorHandler($errors);
$app_part->SetId($id);
$app_part->Build();
$app = $app_part->GetParentApplication();

if (DEFAULT_PARTITION_NAME == $app_part->GetName()) 
    Utils::redirect("edit_app.php?id=" . $app->GetId());


// Handle user submitted actions

if ($_POST['submit_yes'])
{
    $app_part->Delete();
    Utils::redirect("edit_app.php?id=" . $app->GetId());
}

if ($_POST['submit_no']) 
    { Utils::redirect("edit_app_partition.php?id=$id"); }


// -- RENDER PAGE --

$template_vars = array( 'id' => $id,
                        'app_id' => $app->GetId(),
                        'name' => $app_part->GetName());

$page = new Layout();
$page->SetPageTitle("Deleting " . $app->GetName() . " Partition " . $app_part->GetName());
$page->SetBreadcrumbs(array('<a href="main.php">Main Control Panel</a>',
                            '<a href="component_list.php?app">Applications</a>',
                            '<a href="edit_app.php?id=' . $app->GetId() . '">' . htmlspecialchars($app->GetName()) .'</a>',
                            'Partition: ' . htmlspecialchars($app_part->GetName()) ));
$page->mTemplate->assign($template_vars);
$page->Display("delete_app_partition.tpl");

?>