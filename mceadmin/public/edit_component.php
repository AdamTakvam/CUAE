<?php
/* A sort of a univeral page to use that will redirect you to 
 * the right page to edit a component of any type. */
require_once("init.php");
Utils::require_directory("components");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::NORMAL);


$id = $_REQUEST['id'];
$type = $_REQUEST['type'];

if (!$type && $id)
{
    $db = new MceDb();
    $type = $db->GetOne("SELECT type FROM mce_components WHERE mce_components_id = ?", array($id));
}

if ($type && $id)
{
    $class = ComponentType::get_class_name($type);
    $component = new $class;
    $editpage = $component->mEditPage;

    if ($_SERVER['QUERY_STRING'])
        Utils::redirect($editpage . "?" . $_SERVER['QUERY_STRING']);
    else
        Utils::redirect($editpage . "?id=" . $id);
}
else
{
    Utils::redirect("main.php");
}

?>