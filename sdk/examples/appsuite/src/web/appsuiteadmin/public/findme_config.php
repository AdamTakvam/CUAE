<?php

require_once("init.php");
require_once("lib.ConfigUtils.php");
require_once("lib.SimpleMatch.php");


function validate_blacklist_number($number)
{
    return ereg('^[\*\+#0-9]+$',$number);
}

function process_blacklist($blacklist)
{
    global $errors;
    if ($errors->IsEmpty())
    {
        foreach ($blacklist as $set)
        {
            $regexs[] = SimpleMatch::form_regex($set['type'],$set['number']);
        }

        $db = new MceDb();
        $db->StartTrans();
        $db->Execute("UPDATE as_external_numbers SET is_blacklisted = 0");
        if (!empty($regexs))
        {
            $db->Execute("UPDATE as_external_numbers SET is_blacklisted = 1 WHERE phone_number REGEXP ?", array( implode('|',$regexs) ));
        }
        ConfigUtils::set_global_config(GlobalConfigNames::FIND_ME_BLACKLIST_REGEXS, serialize($regexs));
        $db->CompleteTrans();        
    }
    return TRUE;
}


// ** SET UP **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

Utils::trim_array($_POST);

$db = new MceDb();
$errors = new ErrorHandler();
$response = Utils::safe_unserialize($_REQUEST['s_response']);


// ** RETRIEVE DATA **

$raw_cfgs = $db->GetAll("SELECT * FROM as_configs WHERE groupname = 'Find Me Numbers' ORDER BY name ASC");
foreach ($raw_cfgs as $cfg)
    $configs[$cfg['name']] = $cfg;

$stored_val = ConfigUtils::get_global_config(GlobalConfigNames::FIND_ME_VALIDATE_REGEXS);
if (!empty($stored_val))
    $valid_regexs = unserialize($stored_val);
else
    $valid_regexs = array();

$raw_blacklist = SimpleMatch::translate_from_regex( SimpleMatch::complex_list_format( unserialize(ConfigUtils::get_global_config(GlobalConfigNames::FIND_ME_BLACKLIST_REGEXS)) ) );


// ** HANDLE ACTIONS **

if ($_POST['update'])
{
    // Validate each field
    foreach ($configs as $name => $data)
    {
        if (0 != $data['required'] && "" == $_POST[$name])
        {
            $errors->Add(str_replace('_',' ',$name) . " is a required field");
        }
    }
    
    // Do the updates
    if ($errors->IsEmpty())
    {
        $u_query = "UPDATE as_configs SET value = ? WHERE name = ?";
        foreach ($configs as $name => $data)
        {
            $db->Execute($u_query, array($_POST[$name], $name));
        }
        $response = "Find Me Numbers configuration updated";
    }

    // Put submitted values in config data to display
    foreach ($configs as $name => $data)
    {
        $configs[$name]['value'] = $_POST[$name];
    }
}

// -- Find Me Validation Regex management actions

if ($_POST['add'])
{
    if (!empty($_POST['add_regex']))
    {
        $valid_regexs[] = $_POST['add_regex'];
        ConfigUtils::set_global_config(GlobalConfigNames::FIND_ME_VALIDATE_REGEXS, serialize($valid_regexs));
        $response = "The regular expression has been added for validation.";
    }
    else
    {
        $errors->Add('THe regular expression cannot be blank');
    }
}
    
if ($_POST['delete'])
{
    unset($valid_regexs[$_POST['del_regex']]);
    ConfigUtils::set_global_config(GlobalConfigNames::FIND_ME_VALIDATE_REGEXS, serialize($valid_regexs));
    $response = "The regular expression has been removed.";
}

if ($_POST['test'])
{
    if (!empty($_POST['add_regex']))
    {
        $tested['add_regex'] = $_POST['add_regex'];
        $tested['add_regex_test'] = $_POST['add_regex_test'];
        if (eregi($_POST['add_regex'], $_POST['add_regex_test']))
            $tested['result'] = "Pass";
        else
            $tested['result'] = "Fail";
    }
    else
    {
        $errors->Add('THe regular expression cannot be blank');
    }
}

// -- Blacklist management actions

if ($_POST['add_blacklist'] && !empty($_POST['add_blacklist_number']))
{
    if (validate_blacklist_number($_POST['add_blacklist_number']))
    {
        $raw_blacklist[] = array('type' => $_POST['add_blacklist_type'], 'number' => $_POST['add_blacklist_number']);
        if (process_blacklist($raw_blacklist))
            $response = "Number added to the block list";
    }
    else
        $errors->Add($_POST['add_blacklist_number'] . " is not a valid number");
}

if ($_POST['delete_blacklist'])
{
    $del_index = Utils::get_first_array_key($_POST['delete_blacklist']);
    unset($raw_blacklist[$del_index]);
    if (process_blacklist($raw_blacklist))
        $response = "Number removed from the block list";
}

if ($_POST['update_blacklist'] && !empty($_POST['blacklist']))
{
    foreach($_POST['blacklist'] as $index => $set)
    {
        if (!validate_blacklist_number($set['number']))
            $errors->Add($set['number'] . " is not a valid number");
        else
            $raw_blacklist[$index] = $set;
    }
    if (process_blacklist($raw_blacklist))
        $response = "Block list updated";
}


// ** RENDER PAGE **
    
$title = "Find Me Numbers";
$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', '<a href="system_mgmt.php">System Management</a>', $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('configs', $configs);
$page->mTemplate->assign('valid_regexs', $valid_regexs);
$page->mTemplate->assign('tested', $tested);
$page->mTemplate->assign('match_types', SimpleMatchType::$names);
$page->mTemplate->assign('blacklist', $raw_blacklist);
$page->Display("findme_config.tpl");


?>