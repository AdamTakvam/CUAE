<?php

/**
*	User class for appsuiteadmin
*
* 	Developer		Date			Reason
* 	Jan Capps		13-JAN-06		Original
* 	Jan Capps		21-MAR-06		Fixed bugs APPS-418, APPS-419, APPS-420 in regards to setRecord, 
*							setRecordVisible, setPinChange.   Passing a False or 0 from python to PHP is not
*							considered a boolean value by PHP.  I had a check for !empty that threw an 
*							an exception.  I had to specifically check for boolean or 0 or 1. 
*/
require_once("init.php");
require_once("common.php");
require_once("class.MysqlDAO.php");
require_once("lib.TimeZoneUtils.php");
require_once("lib.UserUtils.php");

class User
{
	var $mDbObj;
	var $mUserId;
	var $mUsername;
	var $mAccountCode;
	var $mActiveSessions;
	var $mArTransferNumber;
	var $mCreated;	
	var $mExternalAuth;			
	var $mExternalAuthDn;
	var $mFailedLogins;			
	var $mLastLockout;			
	var $mLastUsed;				
	var $mLdapServerId;			
	var $mLdapSynched;			
	var $mPin;
	var $mPassword;
	var $mFirstName;
	var $mLastName;
	var $mEmail;
	var $mLockoutThreshold;
	var $mLockoutDuration;
	var $mMaxSessions;
	var $mPinChange;
	var $mRecord;
	var $mRecordVisible;
	var $mStatus;
	var $mTimeZone;
	var $mPlacedCalls;
	var $mSuccessfulCalls;
	var $mTotalCallTime;
	var $mResult;
	
    public function __construct()
    {
		$this->mDbObj = new MysqlDAO('localhost','application_suite','root','metreos');
		$this->mActiveSessions = 0;
		$this->mCreated;
		$this->mExternalAuth = 0;			
		$this->mExternalAuthDn = '';	
		$this->mFailedLogins = 0;			
		$this->mLastLockout;
		$this->mLastUsed;
		$this->mLdapServerId = NULL;
		$this->mLdapSynched = 0;				
		$this->mUserId = 0;
		$this->mUsername = NULL;
		$this->mAccountCode = NULL;
		$this->mPin = NULL;
		$this->mPassword = NULL;
		$this->mFirstName = NULL;
		$this->mLastName = NULL;
		$this->mEmail = '';
		$this->mLockoutThreshold = 0;
		$this->mLockoutDuration = 0;
		$this->mMaxSessions = 0;
		$this->mPinChange = 0;
		$this->mRecord = 0;
		$this->mRecordVisible = 0;
		$this->mArTransferNumber = '';
		$this->mStatus = 1;
		$this->mTimeZone = 'US/Central';
		$this->mPlacedCalls = 0;
		$this->mSuccessfulCalls = 0;
		$this->mTotalCallTime = 0;	
		$this->mResult = array(
            'rc'       	=> True,
    	    'result'   	=> NULL,
            'error'		=> NULL);
	}
	
	/**
	*	Function name: getActiveSessions
	*
	*	Purpose: This function gets the number of active sessions for the specified user
	*  
	*	Input:
	*	$username - username
	*
	*	Returns:$rc which contains:
	*		'rc': True for success, False for failure
	*		'result': the number of active sessions for the specified user on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/	
	public function getActiveSessions($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
			$rc = $this->mDbObj->doQuery("select current_active_sessions as myColumn from as_users where username = %s AND status != 8", 
			$username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getActiveSessions: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getCreated
	*
	*	Purpose: This function gets the creation timestamp of the specified user
	*  
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': the creation timestamp for the specified user on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/	
	public function getCreated($username)
	{
		$rc = $this->userExists($username);

		if($rc['result'])
		{		
		   $rc = $this->mDbObj->doQuery("select created as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getCreated: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		
		return $this->mResult;
	}
		
	/**
	*	Function name: getExternalAuth
	*
	*	Purpose: This function gets whether external authorization is enabled for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': 0 for success, 1 for failure
	*		'result': True or False on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*  
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/
	public function getExternalAuth($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select external_auth_enabled as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getExternalAuth: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getExternalAuthDn
	*
	*	Purpose: This function gets the external authorization DN for the specified user
	*
	*	Input:
	*	$username - username
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': the external auth DN onsuccess, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getExternalAuthDn($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select external_auth_dn as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}			
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getExternalAuthDn: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getFailedLogins
	*
	*	Purpose: This function gets the number of failed login attempts for the specified user
	*
	*	Input:
	*	$username - username
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': the number of failed login attemts on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getFailedLogins($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select failed_logins as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}			
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getFailedLogins: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getLastLockout
	*
	*	Purpose: This function gets the datetime of the last lockout for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': the datetime of the last lockout on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getLastLockout($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select last_lockout as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}			
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getLastLockout: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getLastUsed
	*
	*	Purpose: This function gets the datetime that the specified user last used the account
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': the datetime that the account was last used on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getLastUsed($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select last_used as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}			
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getLastUsed: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getLdapServerId
	*
	*	Purpose: This function gets the LDAP server ID for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': the LDAP server ID on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getLdapServerId($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select as_ldap_servers_id as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getLdapServerId: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}

	/**
	*	Function name: getLdapSynched
	*
	*	Purpose: This function gets whether the LDAP server in in sync with appsuiteadmin for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getLdapSynched($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select ldap_synched as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getLdapSynched: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getUserId
	*
	*	Purpose: This function gets the userId for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': userId on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getUserId($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['rc'])
		{
			if($rc['result'])
			{			
			   $rc = $this->mDbObj->doQuery("select as_users_id as myColumn from as_users where username = %s AND status != 8", 
			   $username);
				
				//Get the actual column value and return that as the result
				$colValue = array();
				foreach($rc as $row)
				{
					array_push($colValue, $row['myColumn']);
				}	
				$this->mResult['result'] = $colValue;
			}
			else
			{
				$error = sprintf("ERROR: User.getUserId: The user(%s) does not exist.", $username);
				return $this->_logError($error);
			}
		}
		else
		{
			$error = sprintf("ERROR: User.getUserId: userExists failed.", $username);
			return $this->_logError($error);
		}
		
		return $this->mResult;
	}
    
	/**
	*	Function name: getUserName
	*
	*	Purpose: This function gets the username for the specified user by userId
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': username on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getUserName($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select username as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getUserName: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getStatus
	*
	*	Purpose: This function gets the status of the specified user
	*  
	*	Input:
	*	$username - username
	*
	*	Returns:$rc which contains:
	*		'rc': True for success, False for failure
	*		'result': the status of the active user [Active | Deleted | Locked | Disabled]
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 21-FEB-2006	Reason: Original
	*/	
	public function getStatus($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
			$rc = $this->mDbObj->doQuery("select status as myColumn from as_users where username = %s", 
			$username);
			
			$colValue = '';
			
			switch ($rc[0]['myColumn']) {
				case 1:
					$colValue = 'Active';
					break;
				case 2:
					$colValue = 'Disabled';
					break;
				case 4:
					$colValue = 'Locked';
					break;
				case 8:
					$colValue = 'Deleted';
					break;
				default:
					return $this->_logError('ERROR: User.getStatus: Invalid value for status.');		
			}	
			
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getStatus: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setStatus
	*
	*	Purpose: This function sets the status of the specified user.  If the status is set to Deleted, the user
	*	will be gracefully deleted before setting the status to Deleted.
	*  
	*	Input:
	*	$username - username
	*	$status - status [Active | Deleted |Locked |Disabled]
	*
	*	Returns:$rc which contains:
	*		'rc': True for success, False for failure
	*		'result': True for success, False for failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 22-FEB-2006	Reason: Original
	*/	
	public function setStatus($username, $status)
	{
		// Make sure the user exists before changing the status
		$rc = $this->userExists($username);		

		if($rc['result'])
		{
			if(!empty($status))
			{
				// Make sure the status is valid. 
				switch ($status) {
				case 'Active':
					$this->mStatus = 1;
					break;
				case 'Disabled':
					$this->mStatus = 2;
					break;
				case 'Locked':
					$this->mStatus = 4;
					break;
				case 'Deleted':
					// If the status is set to deleted, delete the user gracefully
					$this->mResult['result'] = $this->deleteUser($username);
					if(!$this->mResult['rc'])
					{
						return $this->_logError('ERROR: User.setStatus: deleteUser failed.');
					}	
					else
					{
						return $this->mResult;
					}			
					break;
				default:
					$error = sprintf("ERROR: User.setStatus: Invalid value for status(%s).", $status);
					return $this->_logError($error);		
				}

				// Set the status for the 'easy' status-types, (not Delete)
				$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set status = %s where username = %s && status != 8", 
					$this->mStatus, $username);
				
				if(!$this->mResult['result'])
				{
					return $this->_logError('ERROR: User.setStatus: The database query failed.');
				}
			}
			else
			{
				return $this->_logError('ERROR: User.setStatus: No value was passed in for status.');
			}
		}
		else
		{
			$error = sprintf("ERROR: User.setStatus: The user(%s) does not exist.", $username);
			return $this->_logError($error);
		}

		return $this->mResult;
	}
	
	/**
	*	Function name: setUserName
	*
	*	Purpose: This function sets the username for the specified userId
	*  
	*	Input:
	*	$userId - userId
	*	$username - username
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setUsername($userId, $username)
	{
		// Make sure that the the username passed in is not administrator because that's reserved for the
		// mceadmin appsuiteadmin administrator
		$tempUser = strtolower($username);
		
		if($tempUser == 'administrator')
		{
			$error = sprintf("ERROR: User.setUsername: The username cannot be %s.", $username);
			return $this->_logError($error);		
		}

		if(!empty($username))
		{			
			// Make sure the userId exists
			if($this->userIdExists($userId))
			{
				// Update the username
			    $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set username = %s where as_users_id = %s", 
				$username, $userId);
			}
			else
			{
				$error = sprintf("ERROR: User.setUsername: The userId(%s) does not exist.", $userId);
				return $this->_logError($error);		
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setUsername: The username cannot be NULL.');		
		}
		return $this->mResult;
	}		
	
	/**
	*	Function name: changeUsername
	*
	*	Purpose: This function changes the username for the specified user
	*  
	*	Input:
	*	$oldUsername - the current username
	*	$newUsername - the new username
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function changeUsername($oldUsername, $newUsername)
	{
		// Make sure the new username is passed in
		if(!empty($newUsername))
		{
			// Make sure that the the username passed in is not administrator because that's reserved for the
			// mceadmin appsuiteadmin administrator
			$tempUser = strtolower($newUsername);	
		
			if($tempUser == 'administrator')
			{		
				$error = sprintf("ERROR: User.changeUsername: The username cannot be %s.", $newUsername);		
				return $this->_logError($error);
			}
			
			// Make sure the user exists
			$rc = $this->userExists($oldUsername);
			
			if($rc['result'])
			{
				// Change the username
			    $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set username = %s where username = %s", 
				$newUsername, $oldUsername);
			}
			else
			{
				$error = sprintf("ERROR: User.changeUsername: The user(%s) does not exist.", $username);
				return $this->_logError($error);		
			}
		}
		else
		{
			return $this->_logError('ERROR: User.changeUsername: New user name cannot be NULL.');		
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: getAccountCode
	*
	*	Purpose: This function gets the account code for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Account code on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getAccountCode($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select account_code as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getAccountCode: The user(%s) does not exist.", $username);
			return $this->_logError($error);		
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setAccountCode
	*
	*	Purpose: This function sets the account code for the specified user
	*  
	*	Input:
	*	$username - username
	*	$accountCode - new account code for the specified user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setAccountCode($username, $accountCode)
	{
		// Make sure a value was passed in for account code
		if(!empty($accountCode))
		{
			// Make sure the account is valid
			if(is_integer($accountCode))
			{
				// Make sure the user exists
				$rc = $this->userExists($username);
				if($rc['result'])
				{
					// Update the account code
				    $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set account_code = %s where username = %s", 
					$accountCode, $username);
				}
				else
				{
					$error = sprintf("ERROR: User.setAccountCode: The user(%s) does not exist.", $username);
					return $this->_logError($error);		
				}
			}
			else
			{
				return $this->_logError('ERROR: User.setAccountCode: Invalid integer value for account code.');
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setAccountCode: Account code cannot be NULL.');
		}
		return $this->mResult;
	}		
	
	/**
	*	Function name: getPin
	*
	*	Purpose: This function gets the pin for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Pin on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getPin($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select pin as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getPin: The user(%s) does not exist.", $username);
			return $this->_logError($error);		
		}
		return $this->mResult;
	}

	/**
	*	Function name: setPin
	*
	*	Purpose: This function sets the pin for the specified user
	*  
	*	Input:
	*	$username - username
	*	$pin - pin
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setPin($username, $pin)
	{
		// Make sure a pin was passed in
		if(!empty($pin))
		{
			if(is_integer($pin))
			{
				// Make sure the user exists
				$rc = $this->userExists($username);
				if($rc['result'])
				{		
					// Update the pin
				    $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set pin = %s where username = %s", 
					$pin, $username);
				}
				else
				{
					$error = sprintf("ERROR: User.setPin: The user(%s) does not exist.", $username);
					return $this->_logError($error);		
				}
			}
			else
			{
				return $this->_logError('ERROR: User.setPin: Invalid integer value for pin.');
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setPin: Pin cannot be NULL.');
		}
		return $this->mResult;
	}		
	
	/**
	*	Function name: getPassword
	*
	*	Purpose: This function gets the password for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Password on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getPassword($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select password as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getPassword: The user(%s) does not exist.", $username);
			return $this->_logError($error);		
		}
		return $this->mResult;
	}

	/**
	*	Function name: setPassword
	*
	*	Purpose: This function sets the password for the specified user
	*  
	*	Input:
	*	$username - username
	*	$password - password
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setPassword($username, $password)
	{
		// Make sure the password was passed in
		if(!empty($password))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Update the password
				$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set password = %s where username = %s", 
				$password, $username);
			}
			else
			{
				$error = sprintf("ERROR: User.setPassword: The user(%s) does not exist.", $username);
				return $this->_logError($error);		
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setPassword: Password cannot be NULL.');
		}
		return $this->mResult;
	}		

	/**
	*	Function name: getFirstName
	*
	*	Purpose: This function gets the first name for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': First name on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getFirstName($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select first_name as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getFirstName: The user(%s) does not exist.", $username);
			return $this->_logError($error);		
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setFirstName
	*
	*	Purpose: This function sets the password for the specified user
	*  
	*	Input:
	*	$username - username
	*	$firstName - first name of user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setFirstName($username, $firstName)
	{
		if(!empty($firstName))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Update the first name
			    $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set first_name = %s where username = %s", 
				$firstName, $username);
			}
			else
			{
				$error = sprintf("ERROR: User.setFirstName: The user(%s) does not exist.", $username);
				return $this->_logError($error);		
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setFirstName: First name cannot be NULL.');
		}
		return $this->mResult;
	}		
	
	/**
	*	Function name: getLastName
	*
	*	Purpose: This function gets the last name for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Last name on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getLastName($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select last_name as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getLastName: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setLastName
	*
	*	Purpose: This function sets the last name for the specified user
	*  
	*	Input:
	*	$username - username
	*	$lastName - new last name of user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setLastName($username, $lastName)
	{
		if(!empty($lastName))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Update the last name
			    $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set last_name = %s where username = %s", 
				$lastName, $username);
			}
			else
			{
				$error = sprintf("ERROR: User.setLastName: The user(%s) does not exist.", $username);
				return $this->_logError($error);	
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setLastName: Last name cannot be NULL.');
		}
		return $this->mResult;
	}	
	
	/**
	*	Function name: getEmail
	*
	*	Purpose: This function gets the email address for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Email address on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getEmail($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select email as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getEmail: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setEmail
	*
	*	Purpose: This function sets the email address for the specified user
	*  
	*	Input:
	*	$username - username
	*	$email- new email address of user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setEmail($username, $email)
	{
		// Make sure the user exists
		$rc = $this->userExists($username);
		if($rc['result'])
		{
			// Update the last name
			$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set email = %s where username = %s", 
			$email, $username);
		}
		else
		{
			$error = sprintf("ERROR: User.setEmail: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}	
	
	/**
	*	Function name: getLockoutThreshold
	*
	*	Purpose: This function gets the lockout threshold for the specified user
	*  
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Lockout threshold on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getLockoutThreshold($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select lockout_threshold as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getLockoutThreshold: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setLockoutThreshold
	*
	*	Purpose: This function sets the lockout threshold  for the specified user
	*  
	*	Input:
	*	$username - username
	*	$lockoutThreshold -lockout threshold for user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setLockoutThreshold($username, $lockoutThreshold)
	{
		if(!empty($lockoutThreshold))
		{
			// Make sure that lockout threshold is an integer value
			if(is_integer($lockoutThreshold))
			{
				// Make sure the user exists
				$rc = $this->userExists($username);
				if($rc['result'])
				{
					// Update the last name
					$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set lockout_threshold = %s where username = %s", 
					$lockoutThreshold, $username);
				}
				else
				{
					$error = sprintf("ERROR: User.setLockoutThreshold: The user(%s) does not exist.", $username);
					return $this->_logError($error);	
				}
			}
			else
			{
				return $this->_logError('ERROR: User.setLockoutThreshold: Invalid integer value for lockout threshold.');		
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setLockoutThreshold: No value was passed in for lockoutThreshold.');		
		}
		return $this->mResult;
	}	
	
	/**
	*	Function name: getLockoutDuration
	*
	*	Purpose: This function gets the lockout duration for the specified user
	*  
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Lockout duration on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getLockoutDuration($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select lockout_duration as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getLockoutDuration: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setLockoutDuration
	*
	*	Purpose: This function sets the lockout duration for the specified user
	*  
	*	Input:
	*	$username - username
	*	$lockoutDuration- lockout duration for user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setLockoutDuration($username, $lockoutDuration)
	{
		// Make sure that a value was passed in for lockout duration
		if(!empty($lockoutDuration))
		{
			// Make sure that lockout threshold is an integer value
			if(is_integer($lockoutDuration))
			{
				// Convert minutes to minutes and hours
				if($lockoutDuration >= 60)
				{
					$hours = $lockoutDuration / 60;
					$minutes = $lockoutDuration % 60;
					$time = sprintf("%02u:%02u:00", intval($hours), $minutes);
				}
				else
				{
					$time = sprintf("00:%02u:00", $minutes);
				}
				
				// Make sure the user exists
				$rc = $this->userExists($username);
				if($rc['result'])
				{
					// Update the last name
					$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set lockout_duration = %s where username = %s", 
					$time, $username);
				}
				else
				{
					$error = sprintf("ERROR: User.setLockoutDuration: The user(%s) does not exist.", $username);
					return $this->_logError($error);	
				}
			}
			else
			{
				return $this->_logError('ERROR: User.setLockoutDuration: Invalid integer value for lockout duration.');		
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setLockoutDuration: No value was passed in for lockout duration.');		
		}
		return $this->mResult;
	}		

	/**
	*	Function name: getMaxSessions
	*
	*	Purpose: This function gets the maximum number of concurrent sessions for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Max number of concurrent sessions on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getMaxSessions($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select max_concurrent_sessions as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getMaxSessions: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}

	/**
	*	Function name: setMaxSessions
	*
	*	Purpose: This function sets the max concurrent sessions for the specified user
	*  
	*	Input:
	*	$username - username
	*	$maxSessions - max concurrent sessions for user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 08-FEB-2006	Reason: Original
	*/			
	public function setMaxSessions($username, $maxSessions)
	{
		// Make sure tha ta value was passed in for max sessions
		if(!empty($maxSessions))
		{
			// Make sure that max sessions is an integer value
			if(is_integer($maxSessions))
			{
				// Make sure the user exists
				$rc = $this->userExists($username);
				if($rc['result'])
				{
					// Update max sessions
					$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set max_concurrent_sessions = %s where username = %s", 
					$maxSessions, $username);
				}
				else
				{
					$error = sprintf("ERROR: User.setMaxSessions: The user(%s) does not exist.", $username);
					return $this->_logError($error);	
				}
			}
			else
			{
				return $this->_logError('ERROR: User.setMaxSessions: Invalid integer value for max sessions.');		
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setMaxSessions: No value was passed in for max sessions.');		
		}
		return $this->mResult;
	}		
	
	/**
	*	Function name: getPinChange
	*
	*	Purpose: This function gets whether a pin change is required for the specified user on next login
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getPinChange($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select pin_change_required as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getPinChange: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}
		
	/**
	*	Function name: setPinChange
	*
	*	Purpose: This function sets whether a pin change is required on the next login for the specified user
	*  
	*	Input:
	*	$username - username
	*	$pinChange -[0 | 1 | TRUE | FALSE]
	*
	*  	NOTE: If the pin is null or blank, that will be treated as false.
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: 	Last Modified:	Reason:
	*	Jan Capps	08-FEB-2006	Original
	*   	Jan Capps	21-MAR-2006	Fix bug APPS-420. 
	*
	*/			
	public function setPinChange($username, $pinChange)
	{		
		// Verify $pinChange and make sure it's in the correct format
		if((is_bool($pinChange)) || ($pinChange == 1) || ($pinChange == 0))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Update pin change required
				$rc = $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set pin_change_required = %s where username = %s", 
				$pinChange, $username);
				
				if($rc)
				{
					$this->mResult['result'] = $rc;
				}
				else
				{
					return $this->_logError('ERROR: User.setPinChange: Execution of update statement failed.');
				}
			}
			else
			{
				$error = sprintf("ERROR: User.setPinChange: The user(%s) does not exist.", $username);
				return $this->_logError($error);	
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setPinChange: Invalid value for pin change.');
		}
		return $this->mResult;
	}	
	
	/**
	*	Function name: getRecord
	*
	*	Purpose: This function gets whether recording is enabled for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getRecord($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select record as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getRecord: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}

	/**
	*	Function name: setRecord
	*
	*	Purpose: This function sets whether recording is enabled for the specified user
	*
	*	Input:
	*	$username - username
	*	$record - [0 | 1 | TRUE | FALSE]
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: 	Last Modified:	Reason:
	*	Jan Capps	08-FEB-2006	Original
	*   	Jan Capps	21-MAR-2006	Fix bug APPS-418. 
	*
	*/			
	public function setRecord($username, $record)
	{
		// Verify value passed in for record
		if((is_bool($record)) || ($record == 1) || ($record == 0))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
	
			if($rc['result'])
			{
				//Update record								
				$rc = $this->mDbObj->doSQL("update as_users set record = %s where username = %s", 
				$record, $username);
				
				if($rc)
				{
					$this->mResult['result'] = $rc;
				}
				else
				{
					return $this->_logError('ERROR: User.setRecord: Execution of update statement failed.');
				}
			}
			else
			{
				$error = sprintf("ERROR: User.setRecord: The user(%s) does not exist.", $username);
				return $this->_logError($error);	
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setRecord: Invalid value for record.');
		}
		return $this->mResult;
	}		
	
	/**
	*	Function name: getRecordVisible
	*
	*	Purpose: This function gets whether recording is visible for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getRecordVisible($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select recording_visible as myColumn from as_users where username = %s AND status != 8",
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getRecordVisible: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setRecordVisible
	*
	*	Purpose: This function sets whether recording is visible tfor the specified user
	*
	*	Input:
	*	$username - username
	*	$recordVisible - [0 | 1 | TRUE | FALSE]
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: 	Last Modified:	Reason:
	*	Jan Capps	08-FEB-2006	Original
	*   	Jan Capps	21-MAR-2006	Fix bug APPS-419. 
	*
	*/			
	public function setRecordVisible($username, $recordVisible)
	{
		// Make sure that recordVisible is a valid value
		if((is_bool($recordVisible)) || ($recordVisible == 1) || ($recordVisible == 0))			
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Update record visible
				$rc = $this->mResult['result'] = $this->mDbObj->doSQL("update as_users set recording_visible = %s where username = %s", 
				$recordVisible, $username);
				
				if($rc)
				{
					$this->mResult['result'] = $rc;
				}
				else
				{
					return $this->_logError('ERROR: User.setRecordVisible: Execution of update statement failed.');
				}
			}
			else
			{
				$error = sprintf("ERROR: User.setRecordVisible: The user(%s) does not exist.", $username);
				return $this->_logError($error);	
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setRecordVisible: Invalid value for record visible.');		
		}
		return $this->mResult;
	}		
	
	/**
	*	Function name: getArTransferNumber
	*
	*	Purpose: This function gets the Active Relay transfer number for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': AR transfer number on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getArTransferNumber($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select ar_transfer_number as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getArTransferNumber: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: setArTransferNumber
	*
	*	Purpose: This function sets the email address for the specified user
	*  
	*	Input:
	*	$username - username
	*	$email- new email address of user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/			
	public function setArTransferNumber($username, $arTransferNumber)
	{
		// Make sure the user exists
		$rc = $this->userExists($username);
		if($rc['result'])
		{
			// Update the AR transfer number
			$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set ar_transfer_number = %s where username = %s", 
			$arTransferNumber, $username);
		}
		else
		{
			$error = sprintf("ERROR: User.setArTransferNumber: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}	
	
	/**
	*
	*	Function name: getTimeZone
	*	Purpose: This function gets the timezone for the specified user
	*
	*	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': Timezone on success, NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 06-FEB-2006	Reason: Original
	*/		
	public function getTimeZone($username)
	{
		$rc = $this->userExists($username);
		
		if($rc['result'])
		{			
		   $rc = $this->mDbObj->doQuery("select time_zone as myColumn from as_users where username = %s AND status != 8", 
		   $username);
			
			//Get the actual column value and return that as the result
			$colValue = array();
			foreach($rc as $row)
			{
				array_push($colValue, $row['myColumn']);
			}	
			$this->mResult['result'] = $colValue;
		}
		else
		{
			$error = sprintf("ERROR: User.getTimeZone: The user(%s) does not exist.", $username);
			return $this->_logError($error);	
		}
		return $this->mResult;
	}

	/**
	*	Function name: setTimezone
	*
	*	Purpose: This function sets the timezone for the specified user
	*  
	*	Input:
	*	$username - username
	*	$timezone - timezone for the specified user
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 07-FEB-2006	Reason: Original
	*/		
	public function setTimezone($username, $timezone)
	{	
		// Make sure a value was passed in for timezone
		if(!empty($timezone))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Get supported timezone list
				$timezoneList = TimeZoneUtils::get_timezone_list();
			
				// If the timezone passed in is valid, set it
				if(in_array($timezone, $timezoneList))
				{
					// Update the timezone
					$this->mResult['result'] = $this->mDbObj->doSQL("update as_users set time_zone = %s where username = %s and status != 8", 					 $timezone, $username);
				}
				else
				{
					return $this->_logError('ERROR: User.setTimezone failed. Invalid timezone.');
				}
			}
			else
			{
				$error = sprintf("ERROR: User.setTimeZone: The user(%s) does not exist.", $username);
				return $this->_logError($error);	
			}
		}
		else
		{
			return $this->_logError('ERROR: User.setTimezone failed. Timezone cannot be NULL.');
		}
		return $this->mResult;
	}	
	
	/**
	*	Function name: getUserList
	*
	*	Purpose: This function gets a list of current appsuiteadmin users.  
	*	Deleted users are not included in this list.
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': list of users on success,  NULL on failure
	*		'error': NULL on success, an error message on failure*
	*
	*	Developer: Jan Capps	Last Modified: 03-FEB-2006	Reason: Original
	*/
	function getUserList()
	{
        $result = $this->mDbObj->doQuery("select username from as_users where status != 8");
		
        $userList = array();
	
        foreach ($result as $row) 
		{
            array_push($userList, $row['username']);
        }
		
		$this->mResult['result'] = $userList;
        return $this->mResult;
	}
	
	/**
	*	Function name: getAccountCodeList
	*
	*	Purpose: This function gets a list of account codes for all current users in appsuiteadmin.  
	*	Deleted users are not included in this list.	
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': list of account codes on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 03-FEB-2006	Reason: Original
	*/
	function getAccountCodeList() 
	{
        $result = $this->mDbObj->doQuery("select account_code from as_users where status != 8");
        $accountCodeList = array();
        foreach ($result as $row) 
		{
            array_push($accountCodeList, $row['account_code']);
        }
		
		$this->mResult['result'] = $accountCodeList;
        return $this->mResult;
	}
	
	/**
	*	Function name: userExists
	*
	*	Purpose: This function determines whether an appsuiteadmin user exists.  Deleted users are ignored.
	*	Usernames must be unique in appsuiteadmin
	*
	* 	Input:
	*	$username - username
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 03-FEB-2006	Reason: Original
	*/
	function userExists($username)
	{
		// If a username was passed in
		if(!empty($username))
		{
			// Query the DB to see if user exists
			$result = $this->mDbObj->doQuery("select username from as_users where username = %s AND status != 8", 
			$username);
					
			if(sizeof($result) > 0)
			{
				$this->mResult['result'] = True;
			}
			else 
			{
				$this->mResult['result'] = False;
			}
		}
		// No user was passed in
		else
		{
			return $this->_logError('ERROR: User.userExists: Username cannot be NULL.');
		}
		
		return $this->mResult;	
	}	
				
	/**
	*	Function name: userIdExists
	*
	*	Purpose: This function determines whether an appsuiteadmin user ID exists.
	*	User IDs must be unique in appsuiteadmin.
	*
	*	Input:
	*	$userId - userid
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False
	*		'error': NULL
	*
	*	Developer: Jan Capps	Last Modified: 01-MAR-2006	Reason: Original
	*/
	function userIdExists($userId)
	{
		if(!is_integer($userId))
		{
			if(is_array($userId))
			{
				if(empty($userId[0]))
				{
					$error = 'ERROR: User.userIdExists: userId cannot be NULL.';
					$this->mResult['error'] = $error;
					error_log($error);
					$this->mResult['rc'] = 0;
					return $this->mResult;
				}
				else
				{
					if(!is_integer($userId[0]))
					{
						$error = sprintf("ERROR: User.userIdExists: Invalid integer value for userId(%s).", $userId);
						$this->mResult['error'] = $error;
						$this->mResult['rc'] = 0;
						error_log(print_r($this->mResult,true));
						return $this->mResult;
					}
				}
			}
			else
			{
				if(empty($userId))
				{
					$error = 'ERROR: User.userIdExists: userId cannot be NULL.';
					$this->mResult['error'] = $error;
					error_log($error);
					$this->mResult['rc'] = 0;
					return $this->mResult;
				}
			}
		}
		// Query the DB for max user id.  They go from 0 to max.
		$result = $this->mDbObj->doQuery("select max(as_users_id) as ID from as_users");
		
		$maxUserId = array();
		foreach ($result as $row) 
		{
			array_push($maxUserId, $row['ID']);
		}
		
		if(($userId >= 0) && ($userId <= $maxUserId[0])) 
		{
			$this->mResult['result'] = True;
		}
		else 
		{
			$this->mResult['result'] = False;
		}
		return $this->mResult;
	}	

	/**
	*	Function name: accountCodeExists
	*
	*	Purpose: This function determines whether an account code exists in appsuiteadmin. 
	*	Account codes for deleted users are ignored.  Account codes must be unique in appsuiteadmin
	*
	*	Input:
	*	$accountCode - account code
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 03-FEB-2006	Reason: Original
	*/
	function accountCodeExists($accountCode)
	{
		// If an account code was passed in
		if(!empty($accountCode))
		{
			// If the account code is valid
			if(is_integer($accountCode))
			{
				// Query the DB to see if the account code exists
		        $result = $this->mDbObj->doQuery("select account_code from as_users where account_code = %s AND status != 8", 
				$accountCode);
							
				if(sizeof($result) > 0) 
				{
					$this->mResult['result'] = True;
				}
				else 
				{
					$this->mResult['result'] = False;
				}
			}
			// The account code is not valid
			else
			{
				return $this->_logError('ERROR: User.accountCodeExists: Account code must be an integer.');
			}
		}
		// No account code was passed in
		else
		{
			return $this->_logError('ERROR: User.accountCodeExists: Account code cannot be NULL.');
		}

		return $this->mResult;
	}	
	
	/**
	*	Function name: addUser
	*
	*	Purpose: This function creates a user in appsuiteadmin.
	*  
	*     	Input:$userRecord
	*			$userRecord required fields:
	*				firstName - User's first name
	*				lastName - User's last name
	*	     			username - User's username.  The username must be unique.
	*				password - User's password
	*				pin - User's pin
	*				accountCode - User's account code.  The account code must be unique.
	*				
	*			$userRecord optional fields:
	*				email - email address
	*				status - user status [Active | Locked | Disabled | Deleted]
	*				lockoutThreshold - number of times the user can enter an invalid password before the account is locked
	*				lockoutDuration - time in minutes the account will be locked if lockout threshold is reached
	*				maxSessions - max number of concurrent sessions per user
	*				pinChange - is a pin change required for the specified  user
	*				record - is recoding enabled for the specified  user
	*				recordVisible - is recording visible to the specified  user
	*				timezone - timezone for the specified user		
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	NOTES: Call $this->getAddUserRequiredCols() for a list of required columns for this function
	*	Call $this->getAddUserCols() for a list of all supported columns for this function
	*
	*	Developer:	Last Modified: 	Reason:
	*	Jan Capps	03-FEB-2006	Original
	*	Jan Capps	13-MAR-2006	Change validation for lockout duration to match what is in appsuiteadmin.
	*/
	function addUser($userRecord)
	{	
    	// Set required user values
		$this->mFirstName = $userRecord->firstName;
		$this->mLastName = $userRecord->lastName;
		$this->mUsername = $userRecord->username;
		$this->mPassword = $userRecord->password;
		$this->mPin = $userRecord->pin;
		$this->mAccountCode = $userRecord->accountCode;		
		
		// Make sure that first name is not NULL
		if(empty($this->mFirstName))
		{		
			return $this->_logError('ERROR: User.addUser: First name cannot be NULL.');
		}
		
		// Make sure that last name is not NULL
		if(empty($this->mLastName))
		{
			return $this->_logError('ERROR: User.addUser: Last name cannot be NULL.');
		}
	
		// Make sure that last name is not NULL
		if(empty($this->mPassword))
		{
			return $this->_logError('ERROR: User.addUser: Password cannot be NULL.');
		}
		
		// Make sure that the the username passed in is not administrator because that's reserved for the
		// mceadmin appsuiteadmin administrator
		$tempUser = strtolower($this->mUsername);	
		
		if($tempUser == 'administrator')
		{		
			$error = sprintf("ERROR: User.addUser: The username cannot be %s.", $this->mUsername);		
			return $this->_logError($error);
		}
		
		// Make sure that pin is not NULL
		if(!empty($this->mPin))
		{
			// Make sure the pin is an integer
			if(!is_integer($this->mPin))
			{
 				return $this->_logError('ERROR: User.addUser: Invalid integer value for pin.');
			}
		}
		else
		{	
			return $this->_logError('ERROR: User.addUser: Pin cannot be NULL.');
		}
		
		// Set email if a value was passed in
		if(!empty($userRecord->email))
		{
			$this->mEmail = $userRecord->email;
		}

		// Verify status if a value was passed in
		if(!empty($userRecord->status))
		{
			// Make sure the status is valid. 
			switch ($userRecord->status) {
			case 'Active':
				$this->mStatus = 1;
				break;
			case 'Disabled':
				$this->mStatus = 2;
				break;			
			case 'Locked':
				$this->mStatus = 4;
				break;			
			case 'Deleted':
				$this->mStatus = 8;
				break;			
			default:
				return $this->_logError('ERROR: User.addUser: Invalid value for status.');
			}	
		}
		
		$ac = $this->accountCodeExists($this->mAccountCode);
	
		// If the accountCodeExists failed, return an error
		if(!$ac['rc'])
		{
			$error = $ac['error'];	
			return $this->_logError($ac['error']);
		}

		// If the account code already exists, return an error
		if($ac['result'])
		{		
			$error = sprintf("ERROR: User.addUser: Account code(%s) must be unique.", $this->mAccountCode);
			return $this->_logError($error);
		}

		$user = $this->userExists($this->mUsername);
		
		// Verify that userExists did not fail due to an invalid username
		if(!$user['rc'])
		{
			$error = $user['error'];
			return $this->_logError($user['error']);	
		}	
		
		// If the user exists, return a failure
		if($user['result'])
		{	
			$error = sprintf("ERROR: User.addUser: Username(%s) must be unique.", $this->mUsername);
			return $this->_logError($error);		
		}
		
		// Get the next available user id
		$result = $this->mDbObj->doQuery("select max(as_users_id) as ID from as_users");
		
		$maxUserId = array();
        foreach ($result as $row) 
		{
            array_push($maxUserId, $row['ID']);
        }
		
		// If this is the first user in the system
		if(is_null($maxUserId[0]))
		{
			$this->mUserId = 1;
		}
		else
		{
			$this->mUserId = $maxUserId[0] + 1;
		}
		
		// If a value was passed in, verify lockout threshold
		if(!empty($userRecord->lockoutThreshold))
		{
			if(!is_integer($userRecord->lockoutThreshold))
			{
				return $this->_logError('ERROR: User.addUser: Invalid integer value for lockoutThreshold.');
			}
			
			$this->mLockoutThreshold = $userRecord->lockoutThreshold;
		}
		
		// If a value was passed in, verify lockout duration
		if(!empty($userRecord->lockoutDuration))
		{
			if(!is_integer($userRecord->lockoutDuration))
			{
				return $this->_logError('ERROR: User.addUser: Invalid integer value for lockoutDuration.');
			}
			
			if($userRecord->lockoutDuration >= 60)
			{
				$hours = $userRecord->lockoutDuration / 60;
				$minutes = $userRecord->lockoutDuration % 60;
				$time = sprintf("%02u:%02u:00", intval($hours), $minutes);
			}
			else
			{
				$time = sprintf("00:%02u:00", $minutes);
			}
			$this->mLockoutDuration = $time;
		}
		
		// If a value was passed in, verify max sessions
		if(!empty($userRecord->maxSessions))
		{
			if(!is_integer($userRecord->maxSessions))
			{
				return $this->_logError('ERROR: User.addUser: Invalid integer value for maxSessions.');
			}
			
			$this->mMaxSessions = $userRecord->maxSessions;
		}
		
		// If a value was passed in, verify pinChange
		if(!empty($userRecord->pinChange))
		{	
			if(!is_bool($userRecord->pinChange))
			{
				return $this->_logError('ERROR: User.addUser: Invalid boolean value for pinChange.');
			}
			
			$this->mPinChange = $userRecord->pinChange;
		}
				
		// If a value was passed in, verify record
		if(!empty($userRecord->record))
		{
			if(!is_bool($userRecord->record))
			{
				return $this->_logError('ERROR: User.addUser: Invalid boolean value for record.');
			}
			$this->mRecord = $userRecord->record;
		}

		// If a value was passed in, verify recordVisible
		if(!empty($userRecord->recordVisible))
		{	
			if(!is_bool($userRecord->recordVisible))
			{
				return $this->_logError('ERROR: User.addUser: Invalid boolean value for recordVisible.');
			}
			
			$this->mRecordVisible = $userRecord->recordVisible;
		}
		
		// Verify timezone
		if(!empty($userRecord->timezone))
		{
			// Get supported timezone list
			$timezoneList = TimeZoneUtils::get_timezone_list();
		
			// If the timezone passed in is valid, set it
			if(in_array($userRecord->timezone, $timezoneList))
			{
				$this->mTimeZone = $userRecord->timezone;
			}
			else
			{
				return $this->_logError('ERROR: User.addUser failed. Invalid timezone.');
			}
		}
		
		// Verify  arTransferNumber
		if(!empty($userRecord->arTransferNumber))
		{
			$this->mArTransferNumber = $userRecord->arTransferNumber;
		}	
				
		// Get column names and datatypes from as_users. 
		$result = $this->mDbObj->doQuery("describe as_users");
        $columns = array();
		
        foreach ($result as $row) 
		{
            array_push($columns, $row['Field']);
        }
		
		//Make sure that we are in sync with the current version of the as_users table.
		if (sizeof($columns) != 29)
		{
			return $this->_logError('ERROR: User.AddUser: There are more columns in the as_user table than expected.');
		}
		
		// Load user members in an array for use in building the insert statement
		$userValues = array();
		$userValues[0] = $this->mUserId;
		$userValues[1] = $this->mLdapServerId;
		$userValues[2] = $this->mUsername;
		$userValues[3] = $this->mAccountCode;
		$userValues[4] = $this->mPin;
		$userValues[5] = $this->mPassword;
		$userValues[6] = $this->mFirstName;
		$userValues[7] = $this->mLastName;
		$userValues[8] = $this->mEmail;
		$userValues[9] = $this->mStatus;
		$userValues[10] = $this->mCreated;
		$userValues[11] = $this->mLastUsed;
		$userValues[12] = $this->mLockoutThreshold;
		$userValues[13] = $this->mLockoutDuration;
		$userValues[14] = $this->mLastLockout;
		$userValues[15] = $this->mFailedLogins;
		$userValues[16] = $this->mActiveSessions;
		$userValues[17] = $this->mMaxSessions;
		$userValues[18] = $this->mPinChange;
		$userValues[19] = $this->mExternalAuth;
		$userValues[20] = $this->mRecord;
		$userValues[21] = $this->mRecordVisible;
		$userValues[22] = $this->mExternalAuthDn;
		$userValues[23] = $this->mLdapSynched;
		$userValues[24] = $this->mTimeZone;
		$userValues[25] = $this->mArTransferNumber;
		$userValues[26] = $this->mPlacedCalls;
		$userValues[27] = $this->mSuccessfulCalls;
		$userValues[28] = $this->mTotalCallTime;	
	
		// Build insert statement
		$insertStmt = "INSERT INTO as_users (" . implode(',',$columns) . ") VALUES (";

		$numColumns = sizeof($userValues);
		$colsProcessed = 0;
		
		for($i = 0; $i < $numColumns; $i++)
		{
			// If this is any column but the last column
			if($colsProcessed < ($numColumns-1))
			{
				// If the value is NULL, explicitly put that string into the db with no quotes
				if(is_null($userValues[$i]))
				{
					$insertStmt .= "NULL,";
				}
				else
				{
					$insertStmt .= "'" . $userValues[$i] . "',";
				}
			}
			else
			{
				if(is_null($userValues[$i]))
				{
					$insertStmt .= "NULL)";
				}
				else
				{
					$insertStmt .= "'" . $userValues[$i] . "')";
				}
			}
			
			$colsProcessed++;
		}
		$this->mResult['result'] = $this->mDbObj->doSQL($insertStmt);
		return $this->mResult;
	}
	
	/**
	*	Function name:deleteUser
	*
	*	Purpose: This function sets a user's status to deleted in appsuiteadmin and cleans up all stale user data.
	*  
	*     	Input: 
	*	$username - User's username.
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 03-FEB-2006	Reason: Original
	*/
	function deleteUser($username)
	{		
		if(!empty($username))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Get the userId
				$rc = $this->getUserId($username);
				if($rc['rc'])
				{
					// Everything associated with the user needs to be cleaned up so I'm using Hung's method to do this properly.  The user is not deleted.
					// The status of the user is set to UserStatus::DELETED but the user still exists because of the potential for call records.  A possible
					// problem is that now that a user can set their own status, they could set the status to deleted but not do the cleanup.  I need to modify
					// setStatus to prevent this.
					$this->mResult['result'] = UserUtils::delete_user($rc['result'][0]);
				}
				else
				{
					return $this->_logError($rc['error']);
				}
			}
			else
			{
				$error = sprintf("ERROR: User.deleteUser: The user(%s) does not exist.", $username);
				return $this->_logError($error);	
			}
		}
		else
		{
			return $this->_logError('ERROR: User.deleteUser: The user cannont be NULL.');
		}
		return $this->mResult;
	}
	
	/**
	*	Function name: deleteAllUsers
	*
	*	Purpose: This function deletes all users from appsuiteadmin
	*  
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True or False on success,  NULL on failure
	*		'error': NULL
	*
	*	NOTE: This is very destructive.  Don't use it unless you want to create all users and everything having
	*	to do with them from scratch.
	*
	*	Developer: Jan Capps	Last Modified: 03-FEB-2006	Reason: Original
	*	Added more tables with dependencies to as_users.
	*/
	function deleteAllUsers()
	{
		// All user tables.  These tables need to have their values dropped in a specific order due to 
		// Foreign key references.  
		$userTables = array();
		$userTables[0] = 'as_remote_agents';
		$userTables[1] = 'as_intercom_group_members';
		$userTables[2] = 'as_scheduled_conferences';
		$userTables[3] = 'as_intercom_group_members';
		$userTables[4] = 'as_user_group_members';
		$userTables[5] = 'as_activerelay_filter_numbers';
		$userTables[6] = 'as_single_reach_numbers';
		$userTables[7] = 'as_voicemail_settings';
		$userTables[8] = 'as_voicemails';
		$userTables[9] = 'as_recordings';
		$userTables[10] = 'as_call_records';
		$userTables[11] = 'as_auth_records';
		$userTables[12] = 'as_directory_numbers';
		$userTables[13] = 'as_phone_devices';
		$userTables[14] = 'as_external_numbers';		
		$userTables[15] = 'as_users';

		// Delete all values from all user-related tables.
		foreach ($userTables as $table)
		{
			$sqlStmt = sprintf("delete from %s", $table);
			$this->mResult['result'] = $this->mDbObj->doSQL($sqlStmt);
			
			// If execution of a sql statement fails, return a failure.
			if($this->mResult['result'] != 1)
			{
				$error = sprintf("ERROR: User.deleteAllUsers: delete from table(%s) failed.", $table);
				return $this->_logError($error);	
			}
		}
		
		return $this->mResult;
	}
		
	/**
	*	Function name: getAddUserRequiredCols
	*
	*	Purpose: This function returns a list of required columns for addUser
	*  
	*	Returns:  $requiredCols: List of requred columns for the addUser(userInfoArray) on success
	*
	*	Developer: Jan Capps	Last Modified: 01-FEB-2006	Reason: Original
	*/
	function getAddUserRequiredCols()
	{	
		$requiredCols = array();
		$requiredCols[0] = 'account_code';
		$requiredCols[1] = 'username';
		$requiredCols[2] = 'first_name';
		$requiredCols[3] = 'last_name';
		$requiredCols[4] = 'password';
		$requiredCols[5] = 'pin';		
		return $requiredCols;
	}
	
	/**
	*	Function name: getAddUserCols
	*
	*	Purpose: This function returns a list of supported columns for the addUser(userInfoArray) 
	*	function.
	*  
	*	Returns:  $userCols: List of supported columns for addUser(userInfoArray) on success
	*
	*	Developer: Jan Capps	Last Modified: 01-FEB-2006	Reason: Original
	*/
	function getAddUserCols()
	{	
		$userCols = array();
		$userCols[0] = 'account_code';
		$userCols[1] = 'username';
		$userCols[2] = 'first_name';
		$userCols[3] = 'last_name';
		$userCols[4] = 'password';
		$userCols[5] = 'pin';	
		$userCols[7] = 'email';
		$userCols[8] = 'status';
		$userCols[10] = 'lockout_threshold';
		$userCols[11] = 'lockout_duration';	
		$userCols[12] = 'max_concurrent_sessions';
		$userCols[13] = 'pin_change_required';
		$userCols[15] = 'record';
		$userCols[16] = 'recording_visible';
		$userCols[18] = 'time_zone';
		$userCols[19] = 'ar_transfer_number';
		return $userCols;
	}
		
	/**
	*	Function name: setFindMeNumbers
	*
	*	Purpose: This function sets all find me numbers for the specified user.  If $respectVoiceMail
	*	is true, active relay is not set for the voicemail number.  If $type = enable, ar is enabled. 
	*	If $type = disable, ar is disabled.
	*
	*	Input:
	*	$username - username
	*	$respectVoicemail - [TRUE|FALSE]
	*	$type - [enable | disable]
	*
	*	Returns:$this->mResult which contains:
	*		'rc': True for success, False for failure
	*		'result': True on success, False on failure
	*		'error': NULL on success, an error message on failure
	*
	*	Developer: Jan Capps	Last Modified: 01-MAR-2006	Reason: Original
	*/
	function setFindMeNumbers($username, $respectVoiceMail, $type)
	{
		if(!is_bool($respectVoiceMail))
		{
			$error = sprintf("ERROR: User.setFindMeNumbers: Invalid value for respectVoiceMail(%s).", $respectVoiceMail);
			return $this->_logError($error);	
		}
				
		if(!empty($username))
		{
			// Make sure the user exists
			$rc = $this->userExists($username);
			if($rc['result'])
			{
				// Get the userId
				$rc = $this->getUserId($username);
				if($rc['rc'])
				{
					$userId = $rc['result'][0];
					
					$query = sprintf("select as_external_numbers_id as ID from as_external_numbers where as_users_id = %s", $userId);
				    $result = $this->mDbObj->doQuery($query);
						
					// Find me id list		
					$idList = array();
						
					foreach ($result as $row) 
					{
						array_push($idList, $row['ID']);
					}
						
					$idListSize = sizeof($idList);
					
					// If the user has no find me numbers, return success
					if($idListSize == 0)
					{
						$this->mResult['rc'] = 1;
						$this->mResult['result'] = NULL;
						$this->mResult['error'] = NULL;
						return $this->mResult;
					}
					
					if($idListSize > 0)
					{
						foreach ($idList as $id)
						{
							if($type == 'enable')
							{
								if($respectVoiceMail)
								{
									
									$query = sprintf("update as_external_numbers set ar_enabled = 1 where as_external_numbers_id = %s and is_corporate = 1", $id);
									
									// Set ar to true for find me number
									$rc = $this->mDbObj->doSQL($query);
								}
								else
								{
									$query = sprintf("update as_external_numbers set ar_enabled = 1 where as_external_numbers_id = %s", $id);
									// Set ar to true for find me number
									$rc = $this->mDbObj->doSQL($query);
								}
							}
							else // $type = 'disable'
							{
								if($respectVoiceMail)
								{
									$query = sprintf("update as_external_numbers set ar_enabled = 0 where as_external_numbers_id = %s and is_corporate = 0", $id);
										
									// Set ar to true for find me number
									$rc = $this->mDbObj->doSQL($query);
								}
								else
								{
									$query = sprintf("update as_external_numbers set ar_enabled = 0 where as_external_numbers_id = %s", $id);
									// Set ar to true for find me number
									$rc = $this->mDbObj->doSQL($query);
								}
							}
						}
					}
				}
			}
			else
			{
				$error = sprintf("ERROR: User.setFindMeNumbers: The user(%s) does not exist.", $username);
				return $this->_logError($error);	
			}
		}
		else
		{
			$error = sprintf("ERROR: User.setFindMeNumbers: Username cannot be NULL.");
			return $this->_logError($error);	
		}
		
		return $this->mResult;
	}
	
	/**
	*	Function name: _logError
	*
	*	Purpose: This function logs an error to php.log and sets mResult to fail
	*  
	*	Input: string $error - error message
	*
	*	Returns: $this->mResult
	*
	*	Developer: Jan Capps	Last Modified: 22-FEB-2006	Reason: Original.
	*/
	private function _logError($error)
	{	
		error_log($error, 0);
		$this->mResult['error'] = $error;
		$this->mResult['rc'] = 0;
		return $this->mResult;
	}
}

?>