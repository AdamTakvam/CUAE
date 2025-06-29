<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.PageLogic.php");
require_once("class.Template.php");
require_once("classlib.Filter.php");

define('WARN_USER_CALL_RECORD_LIMIT', 10000);


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

$status_enum_filter = new FilterEnumeration();
$status_enum_filter->SetEnumeration(UserStatus::$names);

$filters['Account'] = new SearchFilter('name',new FilterString());
$filters['Account']->SetSqlColDef("CONCAT(u.first_name, ' ', u.last_name)");
$filters['Status'] = new ExactFilter('status',$status_enum_filter);
$filters['Total Call Duration'] = new CompareFilter('total_call_time', new FilterMinutes());
$filters['Total Call Duration']->ToggleEq(FALSE);
$filters['Placed Calls'] = new CompareFilter('placed_calls', new FilterNumber());
$filters['Successful Calls'] = new CompareFilter('successfull_calls', new FilterNumber());
$filters['Average Call Length'] = new CompareFilter('avg_call_seconds', new FilterMinutes());
$filters['Average Call Length']->SetSqlColDef('(u.total_call_time DIV u.successfull_calls)');
$filters['Average Call Length']->ToggleEq(FALSE);
$filters['Failed Logins'] = new CompareFilter('failed_logins', new FilterNumber());
$filters['Last Login'] = new CompareFilter('last_used', new FilterTimestamp($time_zone));
$filters['Last Login']->ToggleEq(FALSE);

// Setup sort criteria

$sort['First Name'] = 'u.first_name';
$sort['Last Name'] = 'u.last_name';
$sort['Total Call Duration'] = 'total_call_time';
$sort['Placed Calls'] = 'placed_calls';
$sort['Successful Calls'] = 'successfull_calls';
$sort['Average Call Length'] = '(u.total_call_time DIV u.successfull_calls)';
$sort['Last Login'] = 'last_used';


// ** HANDLE USER ACTIONS **

if ($_POST['confirm_show'])
    $show = $show_okay = 1;
if ($_POST['reject_show'])
    $show = $show_okay = 0;
    
if ($_POST['view_detail'])
{
    $a_id = Utils::get_first_array_key($_POST['view_detail']);
    Utils::redirect("account_detail.php?id=$a_id");
}

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
else
{
    $filters['Status']->SetValue(UserStatus::ACTIVE);
}

foreach ($filters as $filter)
{
    $cond = $filter->GetSqlCondition();
    if (!is_null($cond))
        $filter_clause[] = $cond;
}
if (is_array($filter_clause))
    $filter_query = implode(' AND ', $filter_clause);
$sort_clause = isset($sort_by) ? "$sort_by $sort_order" : "u.last_name ASC";
    

// ** RETRIEVE RECORD DATA **

if (!$admin_access)
{
    $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
    $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
    $group_user_clause = "u.as_users_id IN ($sq)";
}
else
    $group_user_clause = "1 = 1";
    


$end_reason = EndReason::OK;
$select = <<<EOD
SELECT
    u.as_users_id                           AS user_id,
    CONCAT(u.first_name, ' ', u.last_name)  AS name,
    u.status                                AS status,
    u.total_call_time                       AS total_call_time,
    u.failed_logins                         AS failed_logins,
    u.placed_calls                          AS placed_calls,
    u.successfull_calls                     AS successfull_calls,
    u.total_call_time DIV u.successfull_calls     AS avg_call_seconds,
    u.last_used                             AS last_used
    
EOD;

$clauses = <<<EOD
FROM
    as_users AS u
WHERE
    $group_user_clause AND
    $filter_query
ORDER BY 
    $sort_clause
EOD;

$as_report_query = $select . $clauses;

if ($_POST['download'])
{

    // ** CREATE A DOWNLOADABLE REPORT **
    
    $db->SetSessionTimezone($time_zone);
    $accounts = $db->GetAll($as_report_query);
    foreach ($accounts as $x => $account)
    {
        $accounts[$x]['status_display'] = UserStatus::$names[ $account['status'] ];
        $accounts[$x]['total_call_duration'] = DateUtils::sec_to_min_sec_string($account['total_call_time']);
        $accounts[$x]['avg_call_length'] = DateUtils::sec_to_min_sec_string($account['avg_call_seconds']);
        if ($account['last_used'] != '0000-00-00 00:00:00') 
            $accounts[$x]['last_used'] = strtotime($account['last_used']);
    }
    $db->ResetSessionTimezone();
    
    $template = new Template();
    $template->Assign('accounts',$accounts);
    $report_csv = $template->Fetch('reports_download_account_summary.tpl');
    $filename = date("Y-m-d") . "_account_summary_report.csv";
    header("Content-type: text/plain;\r\n");
    header("Content-Length: ".strlen($report_csv).";\r\n");
    header("Content-Disposition: attachment; filename=\"$filename\";\r\n");
    echo $report_csv;
    exit();
    
}
else
{

    // ** RENDER PAGE **

    // Do paging    
    if ($show)
    {
        $db->SetSessionTimezone($time_zone);
        $show_okay = 1;
        if ($show_okay)
        {
            
            $record_count = $db->GetOne("SELECT COUNT(*) " . $clauses);
            $page_logic->SetCurrentPageNumber($_REQUEST['p']);
            $page_logic->SetItemCount($record_count);
            $page_logic->AddQueryVar('show', $show);
            $page_logic->AddQueryVar('show_okay', $show_okay);
            $page_logic->AddQueryVar('sort_by', $sort_by);
            $page_logic->AddQueryVar('sort_order', $sort_order);
            if (is_array($filter_values))
                $page_logic->AddQueryVar('s_fvalues', serialize($filter_values));
            $page_logic->Calculate();
            
            $accounts = $db->GetAll($as_report_query . " " . $page_logic->GetSqlLimit());
            foreach ($accounts as $x => $account)
            {
                $accounts[$x]['status_display'] = UserStatus::$names[ $account['status'] ];
                $accounts[$x]['total_call_duration'] = DateUtils::sec_to_min_sec_string($account['total_call_time']);
                $accounts[$x]['avg_call_length'] = DateUtils::sec_to_min_sec_string($account['avg_call_seconds']);
                if ($account['last_used'] != '0000-00-00 00:00:00') 
                    $accounts[$x]['last_used'] = strtotime($account['last_used']);
            }
        }
        $db->ResetSessionTimezone();
    }
    
    $page = new Layout();
    $page->SetPageTitle("Account Summary");
    $page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 'Account Summary'));
    $page->SetErrorMessage($errors->Dump());
    $page->mTemplate->assign('accounts', $accounts);
    $page->mTemplate->assign('sort', $sort);
    $page->mTemplate->assign('sort_by', $sort_by);
    $page->mTemplate->assign('sort_order', $sort_order);    
    $page->mTemplate->assign('page_logic', $page_logic);
    $page->mTemplate->assign('show', $show);
    $page->mTemplate->assign('show_okay', $show_okay);
    $page->mTemplate->assign('count', $count);
    $page->mTemplate->assign('filters', $filters);
    if (is_array($filter_values))
        $page->mTemplate->assign('s_fvalues', serialize($filter_values));
    $page->Display("reports_account_summary.tpl");
    
}

?>