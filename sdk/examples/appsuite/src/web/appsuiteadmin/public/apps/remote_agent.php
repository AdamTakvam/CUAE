<?php

require_once("init.php");
require_once("lib.GroupUtils.php");
require_once("class.PageLogic.php");


// ** SET UP **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$db = new MceDb();
$plogic = new PageLogic();

$title = "Remote Agent";


// ** GATHER DATA **

if (!$admin_access)
{
    $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
    $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
    $group_user_clause = "as_users_id IN ($sq)";
}
else
    $group_user_clause = "1 = 1";

if ($_REQUEST['sort_by'])
{
    $sort_by = $_REQUEST['sort_by'];
    $sort_order = $_REQUEST['sort_order'];
}
    
$plogic->SetCurrentPageNumber($_REQUEST['p']);

$ra_user_count = $db->GetOne("SELECT COUNT(as_remote_agents_id) FROM as_remote_agents " .
                             "WHERE as_phone_devices_id IS NOT NULL " .
                             "AND as_external_numbers_id IS NOT NULL " .
                             "AND $group_user_clause");

$plogic->SetItemCount($ra_user_count);
$plogic->Calculate();
$plogic->AddQueryVar('sort_by', $sort_by);
$plogic->AddQueryVar('sort_order', $sort_order);

$sort_clause = isset($sort_by) ? "$sort_by $sort_order" : "username ASC";

if (!$admin_access)
    $group_user_clause = "u.as_users_id IN ($sq)";
$query = <<<EOD
SELECT
    CONCAT(u.first_name, ' ', u.last_name) AS name,
    u.as_users_id           AS user_id,
    pd.mac_address          AS device,
    dn.directory_number     AS directory_number,
    en.phone_number         AS external_number
FROM
    as_users                AS u,
    as_phone_devices        AS pd,
    as_directory_numbers    AS dn,
    as_external_numbers     AS en,
    as_remote_agents        AS ra
WHERE
    ra.as_phone_devices_id IS NOT NULL
    AND ra.as_external_numbers_id IS NOT NULL
    AND ra.as_users_id = u.as_users_id
    AND ra.as_phone_devices_id = pd.as_phone_devices_id
    AND ra.as_phone_devices_id = dn.as_phone_devices_id
    AND dn.is_primary_number = 1
    AND ra.as_external_numbers_id = en.as_external_numbers_id
    AND $group_user_clause
ORDER BY
    $sort_clause
EOD;

$query .= $plogic->GetSqlLimit();

$ra_users = $db->GetAll($query);


// ** RENDER PAGE **

$sort['First Name'] = 'u.first_name';
$sort['Last Name'] = 'u.last_name';
$sort['Device'] = 'device';
$sort['Directory Number'] = 'directory_number';
$sort['Find Me Number'] = 'external_number';

$page = new Layout();
$page->SetErrorMessage($errors->Dump());
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>',$title));
$page->mTemplate->assign('ra_users', $ra_users);
$page->mTemplate->assign_by_ref('page_logic', $plogic);
$page->mTemplate->assign('sort', $sort);
$page->mTemplate->assign('sort_by', $sort_by);
$page->mTemplate->assign('sort_order', $sort_order);    
$page->Display('apps_remote_agent.tpl');

?>