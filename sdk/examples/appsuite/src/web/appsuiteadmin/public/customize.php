<?php

require_once("init.php");
require_once("lib.ConfigUtils.php");


// ** SET UP **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

Utils::trim_array($_POST);

$db = new MceDb();
$errors = new ErrorHandler();


// ** HANDLE ACTIONS **

if ($_POST['logo_remove'])
{
    $custom_logo = ConfigUtils::get_global_config(GlobalConfigNames::CUSTOM_LOGO_FILE);
    ConfigUtils::set_global_config(GlobalConfigNames::CUSTOM_LOGO_FILE, NULL);
    @unlink(MCE_APPSUITE_IMAGES_DIR . "/$custom_logo");
    $response = "Custom logo removed.";
}

if ($_POST['logo_submit'])
{
    $acceptable_types = array('image/gif','image/x-bmp','image/jpeg','image/x-png');
    if (is_uploaded_file($_FILES['new_logo']['tmp_name']) && in_array(mime_content_type($_FILES['new_logo']['tmp_name']), $acceptable_types))
    {
        $custom_logo_file = "c_" . time() . "_" . $_FILES['new_logo']['name'];
        move_uploaded_file($_FILES['new_logo']['tmp_name'], MCE_APPSUITE_IMAGES_DIR . DIRECTORY_SEPARATOR . $custom_logo_file);
        @unlink(MCE_APPSUITE_IMAGES_DIR . DIRECTORY_SEPARATOR . ConfigUtils::get_global_config(GlobalConfigNames::CUSTOM_LOGO_FILE));
        ConfigUtils::set_global_config(GlobalConfigNames::CUSTOM_LOGO_FILE, $custom_logo_file);
        $response = "Custom logo uploaded.";
    }
    else
    {
        $errors->Add("The custom logo is not a supported image file.");
    }
}

if ($_POST['app_settings_submit'])
{
    $db->Execute("UPDATE as_applications SET installed = 0");
    foreach ($_POST['applications'] as $id => $value)
    {
        $db->Execute("UPDATE as_applications SET installed = ? where as_applications_id = ?", array($value, $id));
    }
    $response = "Expose application settings updated";
}


// ** RETRIEVE DATA

$application_list = $db->GetAll("SELECT * FROM as_applications ORDER BY name ASC");
$custom_logo = ConfigUtils::get_global_config(GlobalConfigNames::CUSTOM_LOGO_FILE);


// ** RENDER PAGE **

$page = new Layout();
$title = "Customize Administrator";
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', '<a href="system_mgmt.php">System Management</a>', $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('application_list', $application_list);
$page->mTemplate->assign('custom_logo', $custom_logo);
$page->Display("customize.tpl");

?>