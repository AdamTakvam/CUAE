<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("lib.GroupUtils.php");
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
$time_zone = TimeZoneUtils::get_user_timezone($access);
$show = $_REQUEST['show'];
$show_okay = $_REQUEST['show_okay'];


// Setup filters

$apps_enum_filter = new FilterEnumeration();
$temp = $db->GetCol("SELECT DISTINCT application_name FROM as_call_records ORDER BY application_name ASC");
foreach ($temp as $val)
    $all_apps[$val] = $val;
$apps_enum_filter->SetEnumeration($all_apps);

$parts_enum_filter = new FilterEnumeration();
$temp = $db->GetCol("SELECT DISTINCT partition_name FROM as_call_records ORDER BY partition_name ASC");
foreach ($temp as $val)
    $all_parts[$val] = $val;
$parts_enum_filter->SetEnumeration($all_parts);

$scripts_enum_filter = new FilterEnumeration();
$temp = $db->GetCol("SELECT DISTINCT script_name FROM as_call_records ORDER BY script_name ASC");
foreach ($temp as $val)
    $all_scripts[$val] = $val;
$scripts_enum_filter->SetEnumeration($all_scripts);

$end_reason_filter = new FilterEnumeration();
$end_reason_filter->SetEnumeration(EndReason::$names);

$filters['Account'] = new SearchFilter('name',new FilterString());
$filters['Account']->SetSqlColDef("CONCAT(first_name, ' ', last_name)");
$filters['Origin Number'] = new SearchFilter('origin_number',new FilterNumber());
$filters['Destination Number'] = new SearchFilter('destination_number',new FilterNumber());
$filters['Application'] = new ExactFilter('application_name',$apps_enum_filter);
$filters['Partition'] = new ExactFilter('partition_name',$parts_enum_filter);
$filters['Script'] = new ExactFilter('script_name',$scripts_enum_filter);
$filters['Start Time'] = new CompareFilter('start',new FilterTimestamp($time_zone));
$filters['Start Time']->ToggleEq(FALSE);
$filters['Duration'] = new CompareFilter('duration_seconds',new FilterMinutes());
$filters['Duration']->SetSqlColDef("(end - start)");
$filters['Duration']->ToggleEq(FALSE);
$filters['End Reason'] = new ExactFilter('end_reason',$end_reason_filter);

$sort['First Name'] = 'first_name';
$sort['Last Name'] = 'last_name';
$sort['Start Time'] = 'start';
$sort['Duration'] = 'duration_seconds';


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
if (!$admin_access)
{
    $group_ids = GroupUtils::get_administrators_group_ids($access->GetUserId());
    $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
    $where_clause[] = "as_users.as_users_id IN ($sq)";
}

$query[] = "FROM as_call_records AS cr LEFT JOIN as_users USING (as_users_id)";
if (!empty($where_clause))
    $query[] = "WHERE " . implode(' AND ', $where_clause);
$count_query = "SELECT COUNT(*) " . implode(' ',$query);

$query[] = isset($sort_by) ? "ORDER BY $sort_by $sort_order" : "ORDER BY start DESC";
$full_report_query = "SELECT *, IF(end-start >= 0,end-start,NULL) AS duration_seconds, " .
                     "CONCAT(first_name, ' ', last_name) AS name, " .
                     "(SELECT COUNT(*) FROM as_findme_call_records WHERE as_call_records_id = cr.as_call_records_id) AS findme_count " .
                     implode(' ',$query);

if ($_POST['download'])
{

    // ** CREATE A DOWNLOADABLE REPORT **
    
    $db->SetSessionTimezone($time_zone);
    $calls = $db->GetAll($full_report_query);
    $db->ResetSessionTimezone();
    
    foreach ($calls as $x => $call)
    {
        $calls[$x]['duration'] = DateUtils::sec_to_min_sec_string($call['duration_seconds']);
        $calls[$x]['end_reason_display'] = EndReason::$names[ $call['end_reason'] ];
    }
    
    $template = new Template();
    $template->Assign('calls',$calls);
    $report_csv = $template->Fetch('reports_download_call_stats.tpl');
    $filename = date("Y-m-d") . "_call_stats_report.csv";
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
        // Do page logic
        $db->SetSessionTimezone($time_zone);
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
            
            // Retrieve call records
            $calls = $db->GetAll($full_report_query . ' ' . $page_logic->GetSqlLimit());
            
            foreach ($calls as $x => $call)
            {
                $calls[$x]['duration'] = DateUtils::sec_to_min_sec_string($call['duration_seconds']);
                $calls[$x]['end_reason_display'] = EndReason::$names[ $call['end_reason'] ];
            }
        }
        $db->ResetSessionTimezone();
    }
    
    $page = new Layout();
    $page->SetPageTitle("Call Statistics");
    $page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                                'Call Statistics'));
    $page->SetErrorMessage($errors->Dump());
    $page->mTemplate->assign('calls', $calls);
    $page->mTemplate->assign('page_logic', $page_logic);
    $page->mTemplate->assign('filters', $filters);
    $page->mTemplate->assign('show', $show);
    $page->mTemplate->assign('show_okay', $show_okay);
    $page->mTemplate->assign('sort', $sort);
    $page->mTemplate->assign('sort_by', $sort_by);
    $page->mTemplate->assign('sort_order', $sort_order);    
    $page->mTemplate->assign('count', $count);
    if (is_array($filter_values))
        $page->mTemplate->assign('s_fvalues', serialize($filter_values));
    $page->Display("reports_call_stats.tpl");
    
}

?>