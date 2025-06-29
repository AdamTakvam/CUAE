<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.GroupUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.PageLogic.php");
require_once("class.Template.php");
require_once("classlib.Filter.php");


// ** SET UP **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);

$db = new MceDb();
$errors = new ErrorHandler();
$page_logic = new PageLogic();
$show = $_REQUEST['show'];
$show_okay = $_REQUEST['show_okay'];
$time_zone = TimeZoneUtils::get_user_timezone($access);


// Setup filters

$apps_enum_filter = new FilterEnumeration();
$temp = $db->GetCol("SELECT DISTINCT application_name FROM as_auth_records ORDER BY application_name ASC");
foreach ($temp as $val)
    $all_apps[$val] = $val;
$apps_enum_filter->SetEnumeration($all_apps);

$parts_enum_filter = new FilterEnumeration();
$temp = $db->GetCol("SELECT DISTINCT partition_name FROM as_auth_records ORDER BY partition_name ASC");
foreach ($temp as $val)
    $all_parts[$val] = $val;
$parts_enum_filter->SetEnumeration($all_parts);

$status_filter = new FilterEnumeration();
$status_filter->SetEnumeration(AuthenticationResult::$names);

$filters['Account'] = new SearchFilter('name', new FilterString());
$filters['Account']->SetSqlColDef("CONCAT(u.first_name, ' ', u.last_name)");
$filters['Login Type'] = new LoginTypeFilter('login_type', new FilterString());
$filters['Originating Number'] = new SearchFilter('originating_number', new FilterNumber());
$filters['Source IP'] = new SearchFilter('source_ip_address', new FilterIP());
$filters['Username'] = new SearchFilter('username', new FilterString());
$filters['Username']->SetSqlColDef("u.username");
$filters['Login Time'] = new CompareFilter('auth_timestamp', new FilterTimestamp($time_zone));
$filters['Login Time']->ToggleEq(FALSE);
$filters['Application'] = new ExactFilter('application_name',$apps_enum_filter);
$filters['Partition'] = new ExactFilter('partition_name',$parts_enum_filter);
$filters['Status'] = new ExactFilter('status',$status_filter);

$sort['First Name'] = 'u.first_name';
$sort['Last Name'] = 'u.last_name';
$sort['Login Time'] = 'auth_timestamp';


// ** HANDLE USER REQUEST **

if ($_POST['confirm_show'])
    $show = $show_okay = 1;
if ($_POST['reject_show'])
    $show = $show_okay = 0;

if ($_POST['filter'])
    $filter_values = $_POST['__filter__'];
else if ($_REQUEST['s_fvalues'])
    $filter_values = unserialize($_REQUEST['s_fvalues']);

if ($_REQUEST['sort_by'])
{
    $sort_by = $_REQUEST['sort_by'];
    $sort_order = $_REQUEST['sort_order'];
}       
    
// Prepare report filters
if (is_array($filter_values))
{
    foreach ($filters as $display_name => $filter)
    {
        if (!$filter->SetValue($filter_values[$filter->GetName()]))
            $errors->Add("Value for the $display_name filter is not valid.");
    }
}

foreach ($filters as $filter)
{
    $cond = $filter->GetSqlCondition();
    if (!is_null($cond))
        $where_clause[] = $cond;
}
    
    
// ** RETRIEVE RECORD DATA **
    
// Prepare general SQL
$where_clause[] = "ar.as_users_id = u.as_users_id ";
if (!$admin_access)
{
    $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
    $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
    $where_clause[] = "u.as_users_id IN ($sq)";
}

$query[] = "FROM as_auth_records AS ar, as_users as u ";
if (!empty($where_clause))
    $query[] = "WHERE " . implode(' AND ', $where_clause);
$count_query = "SELECT COUNT(*) " . implode(' ',$query);

$query[] = isset($sort_by) ? "ORDER BY $sort_by $sort_order" : "ORDER BY auth_timestamp DESC";
$full_report_query = "SELECT ar.*, CONCAT(u.first_name, ' ', u.last_name) AS name " . implode(' ',$query);


if ($_POST['download'])
{

    // ** CREATE A DOWNLOADABLE REPORT **
    
    $db->SetSessionTimezone($time_zone);
    $report = $db->GetAll($full_report_query);
    foreach ($report as $x => $auth_record)
    {
        $report[$x]['status_display'] = AuthenticationResult::$names[ $auth_record['status'] ];
        $report[$x]['invalid_pin'] = ($auth_record['status'] == AuthenticationResult::INVALID_ACCOUNT_CODE_OR_PIN);
    }
    $db->ResetSessionTimezone();
    
    $template = new Template();
    $template->Assign('auths',$report);
    $report_csv = $template->Fetch('reports_download_security.tpl');
    $filename = date("Y-m-d") . "_security_report.csv";
    header("Content-type: text/plain;\r\n");
    header("Content-Length: ".strlen($report_csv).";\r\n");
    header("Content-Disposition: attachment; filename=\"$filename\";\r\n");
    echo $report_csv;
    exit();
    
}
else
{

    // ** RENDER PAGE **

    if ($show)
    {
        $db->SetSessionTimezone($time_zone);
        // Do page logic
        $count = $db->GetOne($count_query);
        if ($count <= MceConfig::MAX_RECORDS_BEFORE_WARN_USER)
            $show_okay = 1;
        if ($show_okay)
        {
            $page_logic->SetCurrentPageNumber($_REQUEST['p']);
            $page_logic->SetItemCount($count);
            $page_logic->AddQueryVar('show', $show);
            $page_logic->AddQueryVar('show_okay', $show_okay);
            $page_logic->AddQueryVar('sort_by', $sort_by);
            $page_logic->AddQueryVar('sort_order', $sort_order);
            if (is_array($filter_values))
                $page_logic->AddQueryVar('s_fvalues', serialize($filter_values));
            $page_logic->Calculate();
            
            // Retrieve authorization records
            $auths = $db->GetAll("$full_report_query " . $page_logic->GetSqlLimit());
            
            foreach ($auths as $x => $auth_record)
            {
                $auths[$x]['auth_timestamp'] = DateUtils::timezone_offset(strtotime($auth_record['auth_timestamp']), $tz_offset);
                $auths[$x]['status_display'] = AuthenticationResult::$names[ $auth_record['status'] ];
                $auths[$x]['invalid_pin'] = ($auth_record['status'] == AuthenticationResult::INVALID_ACCOUNT_CODE_OR_PIN);
            }
        }
        $db->ResetSessionTimezone();
    }
        
    $title = "Security & Access Control";
    
    $page = new Layout();
    $page->SetPageTitle($title);
    $page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                                $title));
    $page->SetErrorMessage($errors->Dump());
    $page->mTemplate->assign('page_logic', $page_logic);
    $page->mTemplate->assign('filters', $filters);
    $page->mTemplate->assign('auths', $auths);
    $page->mTemplate->assign('show', $show);
    $page->mTemplate->assign('show_okay', $show_okay);
    $page->mTemplate->assign('sort', $sort);
    $page->mTemplate->assign('sort_by', $sort_by);
    $page->mTemplate->assign('sort_order', $sort_order);    
    $page->mTemplate->assign('count', $count);
    if (is_array($filter_values))
        $page->mTemplate->assign('s_fvalues', serialize($filter_values));
    $page->Display("reports_security.tpl");
    
}
?>