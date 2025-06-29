<?php

require_once("init.php");
require_once("3rdparty/jpspan/JPSpan.php");
require_once(JPSPAN . "Server/PostOffice.php");
require_once("lib.GroupUtils.php");


define('JPSPAN_INCLUDE_COMPRESS', FALSE);
if (MceConfig::DEV_MODE)
    define ('JPSPAN_ERROR_DEBUG',TRUE);    

    
class UserAutoComplete
{
    
    function getUser($entry, $group_limit = FALSE)
    {
        $access = new AccessControl();
        $group_admin_access = $access->CheckAccess(AccessControl::GROUP_ADMIN);
        $admin_access = $access->CheckAccess(AccessControl::ADMINISTRATOR);
        $user_id = $access->GetUserId();

        $db = new MceDb();
        
        if (!$group_admin_access)
            return '';
        if (!$admin_access && $group_limit)
        {
            $group_ids = GroupUtils::get_administrators_group_ids($user_id);
            $sq = "SELECT as_users_id FROM as_user_group_members WHERE as_user_groups_id IN (" . implode(',',$group_ids) . ")";
            $clause[] = "as_users_id IN ($sq)";
        }
        $clause[] = "username LIKE '" . addslashes($entry) . "%'";
        $clause[] = "status <> " . UserStatus::DELETED;
        
        $usernames = $db->GetCol("SELECT username FROM as_users WHERE " . implode(' AND ', $clause) . " ORDER BY username ASC");
        
        return array_map('htmlspecialchars',$usernames);
    }
    
}


$server = & new JPSpan_Server_PostOffice();
$server->addHandler(new UserAutoComplete());

if (isset($_SERVER['QUERY_STRING']) && strcasecmp($_SERVER['QUERY_STRING'], 'client')==0) {
    $server->displayClient();
} else {
    // Include error handler - PHP errors, warnings and notices serialized to JS
    require_once JPSPAN . 'ErrorHandler.php';
    $server->serve();
}

?>