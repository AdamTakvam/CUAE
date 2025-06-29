<?php

require_once("class.AccessControl.php");

class RemoteAgentAccessControl extends AccessControl
{
	
	public function __construct()
	{
        $this->mDb = new MceDb();
        session_name("MCERemoteAgent");
        session_start();		
	}
	
	public function CheckLogin()
	{
		if (parent::CheckLogin())
		{
	    	$id = $this->GetUserId();
	    	$verify = $this->mDb->GetOne("SELECT as_remote_agents_id FROM as_remote_agents WHERE as_users_id = ?", array($id));
			if ($verify > 0)
				return TRUE;
			else
				return FALSE;
		}
		else
			return FALSE;
	}
	
}

?>