<?php

require_once("init.php");
require_once("class.PageLogic.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);

$page_logic = new PageLogic();

if ($_POST['edit'])
{
    $g_id = Utils::get_first_array_key($_POST['edit']);
    Utils::redirect("intercom_group_admin.php?id=$g_id");
}

if ($_POST['create'])
{
    Utils::redirect("intercom_group_admin.php");
}

$db = new MceDb();

$count = $db->GetOne("SELECT COUNT(*) FROM as_intercom_groups");
$page_logic->SetCurrentPageNumber($_REQUEST['p']);
$page_logic->SetItemCount($count);
$page_logic->Calculate();

$groups = $db->GetAll("SELECT * FROM as_intercom_groups ORDER BY name ASC " . $page_logic->GetSqlLimit());


// -- PAGE RENDERING --

$title = "Intercom";

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', $title));
$page->SetResponseMessage($response);
$page->mTemplate->assign('groups', $groups);
$page->mTemplate->assign('page_logic', $page_logic);
$page->Display("apps_intercom_admin.tpl");

?>