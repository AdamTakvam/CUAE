<?php

require_once("init.php");
require_once("lib.LdapUtils.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$title = "Import Users";

$ac_attrib = ConfigUtils::get_global_config(GlobalConfigNames::LDAP_AC_ATTRIB);
$uid_attrib = ConfigUtils::get_global_config(GlobalConfigNames::LDAP_UID_ATTRIB);
$get_attributes = array('sn','givenName','mail','ciscoatUserProfile', $ac_attrib, $uid_attrib);

$step = $_REQUEST['step'] ? $_REQUEST['step'] : 1;
$as_ldap_servers_id = $_REQUEST['as_ldap_servers_id'];


function ldap_full_connect($server_info, ErrorHandler $errors)
{
    return LdapUtils::make_connection($server_info['hostname'], $server_info['port'], $server_info['secure_connect'], $server_info['user_dn'], $server_info['password'], $errors);
}



// ** Handle user actions **

if ($_POST['cancel'])
    Utils::redirect('account_mgmt.php');

if ($_POST['submit_step_1'])
{
    $server = $db->GetRow("SELECT * FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($as_ldap_servers_id));
    $ldap_conn = ldap_full_connect($server, $errors);
    @ldap_close($ldap_conn);

    if ($errors->IsEmpty())
        $step = 2;
}

if ($_POST['submit_step_2'])
{
    $validate_string = "^[[:alnum:][:space:]\*]*$";
    $freeform_string = "^.*$";
    if (!eregi($validate_string, $_POST['uid_search']))
        $errors->Add("The search string for Username is not valid.");
    if (!eregi($validate_string, $_POST['sn_search']))
        $errors->Add("The search string for Surname is not valid.");
    if (!eregi($validate_string, $_POST['gn_search']))
        $errors->Add("The search string for Given Name is not valid.");
    if (!eregi($freeform_string, $_POST['freeform']))
        $errors->Add("The search string for the free-formed field is not valid.");

    if ($errors->IsEmpty())
    {
        $server = $db->GetRow("SELECT * FROM as_ldap_servers WHERE as_ldap_servers_id = ?", array($as_ldap_servers_id));
        $ldap_conn = ldap_full_connect($server, $errors);

        if ($_POST['call_manager_search'] > 0)
            $query .= "(objectClass=ciscoocuser)";
        if (!empty($_POST['uid_search']))
            $query .= "($uid_attrib=$_POST[uid_search])";
        if (!empty($_POST['sn_search']))
            $query .= "(sn=$_POST[sn_search])";
        if (!empty($_POST['gn_search']))
            $query .= "(givenName=$_POST[gn_search])";
        if (!empty($_POST['freeform']))
            $query .= "$_POST[freeform]";
        $results = @ldap_search($ldap_conn, $server['base_dn'], "(&$query)", $get_attributes);
        if (!$results)
        {
            $message  = "There was an error performing the search.\n";
            $message .= "Error " . ldap_errno($ldap_conn) . ": " . ldap_error($ldap_conn);
            $errors->Add($message);
            @ldap_close($ldap_conn);
        }
        else if (ldap_count_entries($ldap_conn, $results) == 0)
        {
            $errors->Add("No users where found with that search criteria.");
            @ldap_close($ldap_conn);
        }
        else
        {
            $info = ldap_get_entries($ldap_conn, $results);
            @ldap_close($ldap_conn);

            for ($x = 0; $x < $info['count']; ++$x)
            {
                $info[$x]['account_code'] = $info[$x][strtolower($ac_attrib)];
                $info[$x]['uid'] = $info[$x][strtolower($uid_attrib)];
            }
            unset($info['count']);

            $_SESSION['s_data'] = serialize($info);
            $vars = array( 'uid_search' => $_POST['uid_search'],
                           'sn_search' => $_POST['sn_search'],
                           'gn_search' => $_POST['gn_search'],
                           'freeform' =>  $_POST['freeform'],
                           'as_ldap_servers_id' => $as_ldap_servers_id);
            Utils::redirect("user_import2.php" . Utils::make_query($vars));
        }

    }
}


// ** Create page based on which step of the user import it is on **

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>','<a href="account_mgmt.php">Account Management</a>',$title));
$page->SetErrorMessage($errors->Dump());
$page->mTemplate->assign('step', $step);
$page->mTemplate->assign('as_ldap_servers_id', $as_ldap_servers_id);
if ($step == 1)
    $page->mTemplate->assign('servers', $db->GetAll("SELECT * FROM as_ldap_servers"));
$page->Display('user_import1.tpl');

?>