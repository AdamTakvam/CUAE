<?php

require_once("init.php");
require_once("lib.UserUtils.php");
require_once("lib.GroupUtils.php");
require_once("class.PageLogic.php");


// ** Set up page **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
if (!$admin_access)
{
    $admin_id = $access->GetUserId();
}

$db = new MceDb();
$errors = new ErrorHandler();
$pl = new PageLogic();

$search = $_POST['search'] ? $_POST['search'] : unserialize(urldecode($_GET['s_search']));


// ** Handle user requests **

if ($_POST['create'])
    Utils::redirect("user.php");

if ($_POST['edit_user'])
{
    $user_id = Utils::get_first_array_key($_POST['edit_user']);
    Utils::redirect("user.php?id=$user_id");
}

if ($_POST['delete'])
{
    if (!empty($_POST['user_ids']))
    {
        foreach ($_POST['user_ids'] as $user_id)
        {
            UserUtils::delete_user($user_id);
        }
        $response = "Users have been deleted";
    }
    else
    {
        $errors->Add("No users were selected");
    }
}

if ($_POST['import'])
    Utils::redirect("user_import1.php");


// Create user filters from search

if ($search['mode'])
{
    $clause[] = $search['field'] . " LIKE '%" . addslashes($search['text']) . "%'";
    if ($search['group'] > 0)
        $clause[] = "as_user_group_members.as_user_groups_id = " . $search['group'];
    if ($search['user_type'] > 0)
        $clause[] = "as_user_group_members.user_level = " . $search['user_type'];
    if ($search['status'] > 0)
        $clause[] = "as_users.status = " . $search['status'];
    $sort_clause = "ORDER BY " . $search['sort'] . " " . $search['sort_order'] ;
}
    
// Handle paging logic and user filters

$clause[] = "1 = 1";
$clause[] = "status <> " . UserStatus::DELETED;
if (!$admin_access)
{
    $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
    $groups = $db->GetAll("SELECT * FROM as_user_groups WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")");
    $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
    $clause[] = "as_users.as_users_id IN ($sq)";
}
    
$user_count = $db->GetOne("SELECT COUNT(*) FROM as_users LEFT JOIN as_user_group_members USING (as_users_id) " .
                          "WHERE " . implode(' AND ', $clause));
if (empty($groups))
    $groups = $db->GetAll("SELECT * FROM as_user_groups");

$pl->SetCurrentPageNumber($_REQUEST['p']);
$pl->SetItemCount($user_count);
$pl->Calculate();


// ** Retrieve users **

if (empty($sort_clause))
    $sort_clause = "ORDER BY last_name ASC, first_name ASC";
UserUtils::unlock_expired_lockouts();
$users = $db->GetAll("SELECT *, as_users.as_users_id FROM as_users LEFT JOIN as_user_group_members USING (as_users_id) " .
                     "LEFT JOIN as_user_groups USING (as_user_groups_id) WHERE " .
                     implode(' AND ', $clause) . ' ' . $sort_clause . ' ' .
                     $pl->GetSqlLimit());
for ($x = 0; $x < sizeof($users); $x++)
{
    $users[$x]['display_status'] = UserStatus::$names[$users[$x]['status']];
    $users[$x]['group_admin'] = $users[$x]['user_level'] == GroupUserLevel::ADMINISTRATOR;
}


// ** Render page **

$statuses[""] = "(Any Status)";
$statuses += UserStatus::$names;
unset($statuses[UserStatus::DELETED]);

$sort_types = array("last_name"     => "Last Name",
                    "first_name"    => "First Name",
                    "username"      => "Username",
                    "email"         => "E-mail",
                    "account_code"  => "Account Code",
                    "name"          => "Group",
                    "status"        => "Status");

$pl->AddQueryVar('s_search', serialize($search));
$tpl_vars = array(  'users'         => $users,
                    'pages'         => range(0,$last_page),
                    'this_letter'   => $_REQUEST['a'],
                    'this_page'     => $this_page,
                    'last_page'     => $last_page,
                    'admin_access'  => $admin_access,
                    'admin_id'      => $admin_id,
                    'groups'        => $groups,
                    'search'        => $search,
                    'sort_types'    => $sort_types,
                    'statuses'      => $statuses,
                    'page_logic'    => $pl);

$page = new Layout();
$page->SetPageTitle("Account Management");
$page->SetBreadcrumbs(array('<a href="main.php">Home</a>', 'Account Management'));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign($tpl_vars);
$page->Display("account_mgmt.tpl");

?>