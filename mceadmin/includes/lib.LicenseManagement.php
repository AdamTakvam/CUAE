<?php

require_once('constants.stats.php');
require_once('lib.MceUtils.php');
require_once('class.StatsServerInterface.php');
require_once('class.ServiceControl.php');


define('__LM_LMUTIL_CMD',        '"' . LICENSE_MANAGER_ROOT . '\lmutil.exe"');
define('__LM_GETINFO_COMMAND',   '"' . FRAMEWORK_ROOT . '\GetLicenseInfo.exe"');
define('__LM_REREAD_COMMAND',    __LM_LMUTIL_CMD . ' lmreread -c "' . LICENSES_PATH . '" -all');
define('__LM_VALIDATE_CMD',      '"' . FRAMEWORK_ROOT . '\LicenseValidator.exe"');


abstract class LicenseManagement
{
    
	static function verify_license_file($uploaded_file)
	{
	    try
	    {
	        Utils::execute_with_cmd(__LM_VALIDATE_CMD . ' "' . $uploaded_file['tmp_name'] . '"');
	        return true;
	    }
	    catch (Exception $ex)
	    {
	        return false;
	    }
	}
	
	static function place_license_file($uploaded_file)
	{
		$source = $uploaded_file['tmp_name'];
		$dest = LICENSES_PATH . '/' . $uploaded_file['name'];
		return copy($source, $dest);
	}
	
	static function refresh_license_manager()
	{
	    global $errors;

	    try
	    {
	        Utils::execute_with_cmd(__LM_REREAD_COMMAND);
	    }
	    catch (Exception $e)
	    {
	        if (MceConfig::DEV_MODE)
	        {
	            $errors->Add("Exception : " . $e->getMessage());
	            return false;
	        }
	    }
	    // I can't help but feel like there ought to be a delay of some sort here.
	    $asi = new AppServerInterface();
	    if ($asi->Connected())
	    {
	        $params['ComponentType'] = ComponentType::display(ComponentType::CORE);
	        $params['ComponentName'] = 'LicenseManager';
	        $asi->Send( MceUtils::generate_xml_command('RefreshConfiguration', $params) );
	        if ($asi->Error())
	        {
	            $errors->Add($asi->Error());
	            return false;
	        }
	    }
	    
	    try
	    {
	        $sc = new ServiceControl();
	        $sc->GetFromName(MEDIA_SERVER_SERVICE_NAME);
	        if ($sc->IsEnabled())
	            $sc->Restart();
	    }
	    catch (Exception $e)
	    {
	        $errors->Add("A problem occurred while restarting the media engine.  Please go to Service Control to restart it manually.");
	    }
	    return true;
	}
	
	static function get_intervals()
	{
	    return array(STATS_INTERVAL_HOUR, STATS_INTERVAL_6HOURS, STATS_INTERVAL_12HOURS,
	                 STATS_INTERVAL_DAY, STATS_INTERVAL_WEEK, STATS_INTERVAL_MONTH, STATS_INTERVAL_YEAR);
	}
	
	static function get_license_active($oid)
	{
	    $stats_if = new StatsServerInterface();
	    $active = 0;
	
	    if ($stats_if->Connected())
	    {
	        $command = MceUtils::generate_xml_command("GetStatistic", array("OID" => $oid));
	        $stats_if->Send($command);
	        if (!$stats_if->Error())
	            $active = $stats_if->GetResponse();
	    }
	    return $active;
	}
	
	static function get_license_total($oid)
	{
        $params[STATS_OID_APPSESSIONS] = "appserver";
        $params[STATS_OID_VOICE] = "voiceports";
        $params[STATS_OID_RTP] = "rtpports";
        $params[STATS_OID_ERTP] = "enhancedrtpports";
        $params[STATS_OID_CONFERENCE] = "conferenceports";
        $params[STATS_OID_SPEECHINTEG] = "speechports";
        $params[STATS_OID_TTS] = "tts";
        
        $get_oid = $oid <> STATS_OID_CONFERENCE_USE ? $oid : STATS_OID_VOICE;
        
        try
        {
			if (isset($params[$get_oid]))
			{
				$output = Utils::execute_with_cmd(__LM_GETINFO_COMMAND . " -" . $params[$get_oid]);
				if ($oid == STATS_OID_CONFERENCE_USE)
				{
					return floor(intval($output) / 2);
				}
				else
					return $output;
			}
			else
				return 0;
        }
        catch (Exception $ex)
        {
            return 0;
        }

	}
	
	static function get_license_max($oid)
	{
        $params[STATS_OID_APPSESSIONS] = "appservermax";
        $params[STATS_OID_VOICE] = "maxvoice";
        $params[STATS_OID_RTP] = "maxrtp";
        $params[STATS_OID_ERTP] = "maxenhanced";
        $params[STATS_OID_CONFERENCE] = "maxconf";
        $params[STATS_OID_SPEECHINTEG] = "maxspeech";
        $params[STATS_OID_TTS] = "maxtts";
	    
        try
        {
			$output = Utils::execute_with_cmd(__LM_GETINFO_COMMAND . " -" . $params[$oid]);
			return $output;
        }
        catch (Exception $ex)
        {
            return 0;
        }
	    
	}
	
	static function get_cuae_license_mode()
	{
        try
        {
			$output = Utils::execute_with_cmd(__LM_GETINFO_COMMAND . " -modeCUAE");
			return substr($output,7);
        }
        catch (Exception $ex)
        {
            return "Unknown";
        }
	}

	static function get_cume_license_mode()
	{
        try
        {
			$output = Utils::execute_with_cmd(__LM_GETINFO_COMMAND . " -modeCUME");
			return substr($output,7);
        }
        catch (Exception $ex)
        {
            return "Unknown";
        }
	}
		
	static function generate_oid_graphs($oid)
	{
	    $stats_if = new StatsServerInterface();
	    $graph_files = array();
	    if ($stats_if->Connected())
	    {
		    $intervals = LicenseManagement::get_intervals();
	        $total = LicenseManagement::get_license_total($oid);
	        $params = array("OID" => $oid, "HLine" => $total);
		    foreach($intervals as $int)
		    {
		        $params["Interval"] = $int;
		        $com = MceUtils::generate_xml_command("GenerateGraph", $params);
		        $stats_if->Send($com);
		        if (!$stats_if->Error())
		        {
		            $graph_files[$int] = $stats_if->GetResponse();
		        }
		    }
		    return $graph_files;
	    }
	    return FALSE;
	}
	
	static function generate_interval_graphs($interval, $oids)
	{
	    $stats_if = new StatsServerInterface();
	    $graph_files = array();
	    if ($stats_if->Connected())
	    {
	        $params = array("Interval" => $interval);
		    foreach($oids as $oid => $oid_data)
		    {
		        $params["OID"] = $oid;
		        $params["HLine"] = LicenseManagement::get_license_total($oid);
		        $com = MceUtils::generate_xml_command("GenerateGraph", $params);
		        $stats_if->Send($com);
		        if (!$stats_if->Error())
		        {
		            $graph_files[$oid] = $stats_if->GetResponse();
		        }
		    }
		    return $graph_files;
	    }
	    return FALSE;
	}
	    
}

?>