<?php

require_once("init.php");
require_once("class.ReplicationManagement.php");

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();

$rm = new ReplicationManagement();
$data = $rm->GetSettings();
$role = $_POST['role'] ? $_POST['role'] : $data['role'];
$data = $role == ReplicationRole::SLAVE ? $rm->GetGlobalSlaveSettings()	: $data;
    
switch ($role)
{
	case ReplicationRole::SLAVE:
		$title = "Replication Subscriber Setup";
		break;
	case ReplicationRole::MASTER:
		$title = "Replication Publisher Setup";
		break;
	default:
		$title = "Replication Setup";	
}


// -- HANDLE ACTION REQUESTS --

if ($_POST['commit'])
{
	if (floatval($_POST['server_id']) <= 0)
		$errors->Add("Server ID is not a positive integer");
	if ($_POST['role'] == ReplicationRole::SLAVE && empty($_POST['host']))
		$errors->Add("A publisher hostname is required");
	if (empty($_POST['user']))
		$errors->Add("A replication username is required");
    if (strtolower($_POST['user']) == 'root')
        $errors->Add("Username is not valid");
	if (empty($data['password']) && empty($_POST['password']))
		$errors->Add("A password for the replication user is required");
	if ($_POST['password'] != $_POST['password_verify'])
		$errors->Add("New passwords did not match.  Please try again.");
	
	if ($errors->IsEmpty())
	{
		$host = NULL;
		if (!empty($_POST['host']))
			$host = $_POST['host'];
        else if (!empty($data['host']))
            $host = $data['host'];
        
		if (empty($_POST['password']))
			$_POST['password'] = $data['password'];
		try
		{
			$rm->EnableReplication($_POST['role'], $_POST['server_id'], $_POST['user'], $_POST['password'], $host);
		}
		catch (Exception $e)
		{
			$errors->Add("Enabling replication failed.  Reason: " . $e->GetMessage());
		}
	}	
}

if ($_POST['disable'])
{
	$rm->DisableReplication();
	$rm->RestartMySQL();
	$response = "Replication has been disabled";
}

if ($errors->IsEmpty())
	$info = $rm->GetSettings();
else
	$info = $_POST;

    
if ($_POST['select'])
{
    if ($_POST['role'] == ReplicationRole::SLAVE)
        $info = $rm->GetGlobalSlaveSettings();
    $info['server_id'] = $rm->GetServerId();
    $info['role'] = $_POST['role'];
	$info['selected'] = 1;
}


// -- RENDER PAGE --

$page = new Layout();
$page->SetPageTitle($title);
if ($role <> ReplicationRole::SLAVE)
    $page->SetBreadcrumbs(array('<a href="main.php">Home</a>', $title));
else
    $page->SetBreadcrumbs(array($title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->Assign($info);
$page->Display('replication.tpl');

?>