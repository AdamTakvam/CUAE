<?php

require_once("init.php");
require_once("lib.DateUtils.php");
require_once("lib.TimeZoneUtils.php");
require_once("class.MceTemplate.php");
require_once("Mail.php");
require_once("Mail/RFC822.php");


// Useful PIN generating and checking function

function generate_pin()
{
    $db = new MceDb();
    $found = FALSE;
    while (!$found)
    {
        $pin = mt_rand(MceConfig::SC_PIN_START_RANGE, MceConfig::SC_PIN_END_RANGE);
        $count = $db->GetOne("SELECT COUNT(*) FROM as_scheduled_conferences " .
                             "WHERE host_conf_id = ? OR participant_conf_id = ?",
                             array($pin,$pin));
        if (0 == $count) $found = TRUE;
    }
    return $pin;
}


// -- SETUP --

if ($_POST['cancel'])
    Utils::redirect("scheduled_conference.php");

// Check if the conference is good to edit

$db = new MceDb();
$access = new AccessControl();
$group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
$admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
$admin_id = $access->GetUserId();
$id = $_REQUEST['id'];
if (!empty($id))
{
    $conf = $db->GetRow("SELECT *, ((scheduled_timestamp + INTERVAL duration_minutes MINUTE) < NOW()) AS expired FROM as_scheduled_conferences WHERE as_scheduled_conferences_id = ?", array($id));
    if ($conf['status'] == ConferenceStatus::DISABLED || $conf['expired'])
        Utils::redirect('scheduled_conference.php');
    else
        $access->CheckPageAccess(AccessControl::NORMAL, $conf['as_users_id']);
}
else
{
    $access->CheckPageAccess(AccessControl::NORMAL);
}
if (!$admin_access)
    $user_id = $access->GetUserId();


Utils::trim_array($_POST);
$errors = new ErrorHandler();
$time_zone = TimeZoneUtils::get_user_timezone($access);


// -- HANDLE CONFERENCE DELETE --

if ($_POST['delete'])
{
    $cnt = $db->GetOne("SELECT COUNT(*) FROM as_call_records WHERE as_scheduled_conferences_id = ?", array($id));
    if ($cnt > 0)
    {
        $db->Execute("UPDATE as_scheduled_conferences SET status = ? WHERE as_scheduled_conferences_id = ?",
                     array(ConferenceStatus::DISABLED, $id));
    }
    else
    {
        $db->Execute("DELETE FROM as_scheduled_conferences WHERE as_scheduled_conferences_id = ?", array($id));
    }
    Utils::redirect("scheduled_conference.php");
}


// -- HANDLE CONFERENCE CREATION OR UPDATE --

if ($_POST['submit'])
{
    
    // VALIDATE VALUES
    
    if ($group_admin_access && empty($id))
    {
        $group_ids = NULL;
        if (!$admin_access)
            $group_ids = GroupUtils::get_administrators_group_ids($admin_id);
        $user_id = UserUtils::find_by_username($_POST['username'], $group_ids);
        if (!$user_id)
            $errors->Add("No user was found by that username.");
    }

    if (intval($_POST['num_participants']) < 2) 
        $errors->Add("Expected participants must be greater than 1.");
    if (intval($_POST['duration']['minutes']) > 59) 
        $errors->Add("Duration minutes field cannot be 60 or larger.");
    if (!checkdate($_POST['datetime']['Month'], $_POST['datetime']['Day'], $_POST['datetime']['Year']))
        $errors->Add("Scheduled date is not valid");
    $duration_minutes = (intval($_POST['duration']['hours']) * 60) + intval($_POST['duration']['minutes']);
    if ($duration_minutes <= 0)
        $errors->Add("Duration time cannot be 0.");
    
    $time = $_POST['datetime'];
    if ($time['Meridian'] == 'pm')
        $time['Hour'] += 12;
    $timestring = "$time[Year]-$time[Month]-$time[Day] $time[Hour]:$time[Minute]";
    $_POST['datetime'] = $datetime = strtotime($timestring);
    $scheduled_timestamp = $db->GetOne("SELECT UNIX_TIMESTAMP(CONVERT_TZ('$timestring','$time_zone','SYSTEM'))");
    if (empty($scheduled_timestamp))
        $scheduled_timestamp = $timestring;
    
    if ($db->GetOne("SELECT FROM_UNIXTIME($scheduled_timestamp) <= NOW()"))
        $errors->Add("The scheduled date has already occurred.");
    
    // Compare with other conferences to make sure there is no resource overload.
    if ($errors->IsEmpty())
    {
    	$temp_id = intval($id) ? $id : 0;
    	
        // Calculate the start and end timestamps
        $start_ts = $scheduled_timestamp;
        $end_ts = $start_ts + ($duration_minutes * 60);
        
        // Create a resource counter array with an element for every 5 minutes (the current increment of 
        // selectable times) in between
        $ts_index = $start_ts;
        while ($ts_index <= $end_ts)
        {
        	$rt_counts[$ts_index] = $_POST['num_participants'];
        	$ts_index += 300;
        }
        
        $preceding = $db->GetAll("SELECT * FROM
								 (SELECT *, DATE_ADD(scheduled_timestamp, INTERVAL duration_minutes MINUTE) AS end_time
								 FROM as_scheduled_conferences
								 WHERE status = ? AND scheduled_timestamp > NOW() AND scheduled_timestamp < FROM_UNIXTIME(?) " .
								 "AND as_scheduled_conferences_id <> ?) AS conferences
								 WHERE end_time >= FROM_UNIXTIME(?)",
        						 array(ConferenceStatus::ENABLED, $start_ts, $temp_id, $start_ts));

        $following = $db->GetAll("SELECT *, DATE_ADD(scheduled_timestamp, INTERVAL duration_minutes MINUTE) as end_time " .
        						 "FROM as_scheduled_conferences WHERE " .
        						 "status = ?  AND as_scheduled_conferences_id <> ? " .
        						 "AND scheduled_timestamp >= FROM_UNIXTIME(?) AND scheduled_timestamp <= FROM_UNIXTIME(?)",
        						 array(ConferenceStatus::ENABLED, $temp_id, $start_ts, $end_ts));

		foreach ($preceding as $conf)
		{
			$temp_ts_index = $start_ts;
			$temp_end_ts = strtotime($conf['end_time']);
			while ($temp_ts_index <= $temp_end_ts)
			{
				$rt_counts[$temp_ts_index] += $conf['num_participants']; 
				$temp_ts_index += 300;
			}
		}
		
		foreach ($following as $conf)
		{
			$temp_ts_index = strtotime($conf['scheduled_timestamp']);
			$temp_end_ts = min($end_ts, strtotime($conf['end_time']));
			while ($temp_ts_index <= $temp_end_ts)
			{
				$rt_counts[$temp_ts_index] += $conf['num_participants']; 
				$temp_ts_index += 300;
			}
		}

		$max_ports = ConfigUtils::get_global_config(GlobalConfigNames::MEDIA_PORTS);
		
		foreach ($rt_counts as $port_count)
		{
			if ($port_count > $max_ports)
			{
				$errors->Add("This conference will require more media ports than will be available for the scheduled time.  Try changing the time or reduce the number of participants.");
				break;
			}
		}
    }
    
    
    // UPDATE/CREATE CONFERENCE
    
    if ($errors->IsEmpty())
    {
    
        // Update conference
        if ($id > 0)
        {
            $db->Execute("UPDATE as_scheduled_conferences SET " .
                         "scheduled_timestamp = FROM_UNIXTIME(?), duration_minutes = ?, num_participants = ?, " .
                         "status = ? " .
                         "WHERE as_scheduled_conferences_id = ?",
                         array($scheduled_timestamp, $duration_minutes, $_POST['num_participants'], ConferenceStatus::ENABLED, $id));
            $response = "Conference updated.";
        }
        else
        // Create conference
        {
            $host_conf_id = generate_pin();
            $participant_conf_id = generate_pin();
            $db->Execute("INSERT INTO as_scheduled_conferences " .
                         "(as_users_id, host_conf_id, participant_conf_id, scheduled_timestamp, duration_minutes, num_participants, status) " .
                         "VALUES (?,?,?,FROM_UNIXTIME(?),?,?, ?)",
                         array($user_id, $host_conf_id, $participant_conf_id, $scheduled_timestamp, $duration_minutes, $_POST['num_participants'], ConferenceStatus::ENABLED));
            $id = $db->Insert_ID();       
            Utils::redirect("scheduled_conference_detail.php?id=$id");
        }
    }
}


// -- HANDLE NOTIFICATION ACTION --

if ($_POST['submit_notify'])
{
	$emails = explode("\n",str_replace(",","\n",$_POST['emails']));
	$emails = array_map('trim',$emails);
	if (empty($emails))
		$errors->Add('No email addresses were specified');
	foreach ($emails as $email)
	{
		if (!Mail_RFC822::isValidInetAddress($email))
			$errors->Add($email . " is not a valid e-mail address.");
	}
	
	if ($errors->IsEmpty())
	{
		// Retrieve information for the mail
        $db->SetSessionTimezone($time_zone);
        $conference = $db->GetRow("SELECT * FROM as_scheduled_conferences WHERE as_scheduled_conferences_id = ? ",
                                  array($id));
        $conference['duration']['hours'] = floor(floatval($conference['duration_minutes']) / 60) ;
        $conference['duration']['minutes'] = floatval($conference['duration_minutes']) % 60;
        $host_email = $db->GetOne(	"SELECT email FROM as_scheduled_conferences LEFT JOIN as_users USING (as_users_id) " .
                                	"WHERE as_scheduled_conferences_id = ?",
                                	array($id));
		$host_email = trim($host_email);
		$db->ResetSessionTimezone();
        
		// Retrieve mail server information
		$mail_params['host'] = ConfigUtils::get_global_config(GlobalConfigNames::SMTP_SERVER);
		$mail_params['port'] = ConfigUtils::get_global_config(GlobalConfigNames::SMTP_PORT);
		$mail_params['username'] = ConfigUtils::get_global_config(GlobalConfigNames::SMTP_USER);
		$mail_params['password'] = ConfigUtils::get_global_config(GlobalConfigNames::SMTP_PASSWORD);
		if (!empty($mail_params['username']) && !empty($mail_params['password']))
			$mail_params['auth'] = TRUE;
		$mail_params['debug'] = MceConfig::DEV_MODE;
        $mail_params['timeout'] = MceConfig::SMTP_MAIL_TIMEOUT;
		$mail =& Mail::factory('smtp',$mail_params);
		
		// Compile mail template
		$mail_body = new MceTemplate();
		$mail_body->assign($conference);
		$mail_body->assign('timezone', $time_zone);
		$mail_body->assign('details', $_POST['details']);
		$mail_body->assign('conference_dn', ConfigUtils::get_global_config(GlobalConfigNames::SCHEDCONFERNCE_DN));
		
		// Send mail
		set_time_limit(0);
		$mail_headers['From'] = 'conference@' . $_SERVER['HTTP_HOST'];
		$mail_headers['Subject'] = "NOTIFICATION: You Are Hosting A Call Conference";
		$mail_return = $mail->Send($host_email,$mail_headers,$mail_body->Fetch('apps_scheduled_conference_host_email.tpl'));
		if (PEAR::isError($mail_return))
		{
			$errors->Add("Error " . $mail_return->GetCode() . ": " . $mail_return->GetMessage());
		}
		
		$mail_headers['Subject'] = "NOTIFICATION: A Call Conference Has Been Scheduled";
		$mail_return = $mail->Send($emails,$mail_headers,$mail_body->Fetch('apps_scheduled_conference_part_email.tpl'));
		if (PEAR::isError($mail_return))
		{
			$errors->Add("Error " . $mail_return->GetCode() . ": " . $mail_return->GetMessage());
		}
		
		if ($errors->IsEmpty())
		{
			$response = "Notifications about this conference have been sent.";
		}
	}
}


// -- RETRIEVE DATA --

if ($id > 0)
{
    $db->SetSessionTimezone($time_zone);
    if ($errors->IsEmpty())
    {
        $conference = $db->GetRow("SELECT * FROM as_scheduled_conferences WHERE as_scheduled_conferences_id = ? ",
                                  array($id));
        $datetime = $conference['scheduled_timestamp'];
        $conference['duration']['hours'] = floor(floatval($conference['duration_minutes']) / 60) ;
        $conference['duration']['minutes'] = floatval($conference['duration_minutes']) % 60;
    }
    if ($group_admin_access || $admin_access)
    {
        $username = $db->GetOne("SELECT username FROM as_scheduled_conferences LEFT JOIN as_users USING (as_users_id) " .
                                "WHERE as_scheduled_conferences_id = ?",
                                array($id));
    }
    $db->ResetSessionTimezone();
    $title = "Edit Conference";
}
else
{
    if ($errors->IsEmpty())
    {
        $datetime = $db->GetOne("SELECT UNIX_TIMESTAMP(CONVERT_TZ(NOW(),'SYSTEM','$time_zone'))");
    }
    
    $title = "Schedule New Conference";
}
$smtp_server = ConfigUtils::get_global_config(GlobalConfigNames::SMTP_SERVER);


// -- RENDER PAGE --

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs(array('<a href="/appsuiteadmin/main.php">Home</a>', 
                            '<a href="/appsuiteadmin/apps/scheduled_conference.php?id=' . $user_id . '">ScheduledConference</a>',
                            $title));
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($response);
$page->mTemplate->assign('username', $username);
$page->mTemplate->assign('group_admin_access', $group_admin_access);
$page->mTemplate->assign('admin_access', $admin_access);
$page->mTemplate->assign('datetime', $datetime);
$page->mTemplate->assign('conf_dn', ConfigUtils::get_global_config(GlobalConfigNames::SCHEDCONFERNCE_DN));
$page->mTemplate->assign('smtp_set', !empty($smtp_server));
if ($errors->IsEmpty() || $_POST['submit_notify'])
{
    $page->mTemplate->assign('id', $id);
    $page->mTemplate->assign($conference);
}
if (!$errors->IsEmpty() || $_POST['submit_notify'])
{
    $page->mTemplate->assign($_POST);
}
$page->Display("apps_scheduled_conference_detail.tpl");

?>