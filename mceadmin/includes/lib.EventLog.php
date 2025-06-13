<?php

require_once("common.php");
require_once("eventlog_enums.php");

abstract class EventLog
{

    public static function log($type, $message, $message_id = 0, $data = NULL, $severity = Severity::GREEN)
    {
        try
        {
            $db = new MceDb();
            if (!empty($_SESSION['user']))
                $user_info = "User: " . $_SESSION['user'] . "\n";
            $db->Execute("INSERT INTO mce_event_log (type, message_id, message, data, severity, created_timestamp, recovered_timestamp) " .
                         "VALUES (?,?,?,?,?,NOW(),NOW())",
                         array($type, $message_id, $message, $user_info . print_r($data, TRUE), $severity));
        }
        catch (Exception $e)
        {
            throw new Exception("There was an error in logging the event.\nDatabase error: " . $e->getMessage());
            return FALSE;
        }
    }

    public static function get_log_size($type)
    {
        $db = new MceDb();
        $type_clause = EventLog::_create_log_type_clause($type);
        return $db->GetOne("SELECT COUNT(mce_event_log_id) FROM mce_event_log $type_clause");
    }

    public static function get_log($type, $limit = 0, $start = 0)
    {
        $limit = intval($limit);
        $start = intval($start);
        $db = new MceDb();
        $type_clause = EventLog::_create_log_type_clause($type);
        $query = "SELECT * FROM mce_event_log $type_clause ORDER BY mce_event_log_id DESC";
        if ($limit > 0)
            $query .= " LIMIT $start, $limit";
        return $db->GetAll($query);
    }

    
	/* PRIVATE FUNCTIONS */    
    
    private static function _create_log_type_clause($type = LogMessageType::UNSPECIFIED)
    {
        $type_clause = "";
        if ($type != LogMessageType::UNSPECIFIED && !is_array($type))
            $type = array($type);
        if (is_array($type))
        {
            $types = array_map('intval', $type);
            $type_clause = "WHERE type IN (" . implode(',', $types) . ")";
        }
        return $type_clause;
    }


}

?>