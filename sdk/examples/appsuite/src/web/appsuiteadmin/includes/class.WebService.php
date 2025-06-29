<?php
// Appsuiteadmin Web Service interface class
// Developer: 	Date:			Reason:
// Jan Capps	18-MAR-2006	Restrict non-admin user from accessing web service.
require_once("SOAP/Server.php");
require_once("SOAP/Disco.php");
require_once("common.php");
require_once("class.AccessControl.php");
require_once("class.MysqlDAO.php");
require_once("class.User.php");
require_once("lib.TimeZoneUtils.php");

class WebService {
   	var $__dispatch_map = array();
    var $__typedef      = array();	
    var $mUserObj;
    var $mUserData;

    function WebService() {
 		$this->mUserObj = new User();
		$this->mDbObj = new MysqlDAO('localhost','application_suite','root','metreos');
        
        // Define all of the supported operations
        $this->__dispatch_map['login'] = array(
            'in'  => array('username' => 'string', 'password' => 'string'),
			'out' => array('token' => 'string')		
            );
			
        $this->__dispatch_map['getUserList'] = array(
            'in'  => array('token' => 'string'),
            'out' => array('result' => '{urn:MetreosAppSuiteAdmin}ArrayOfString')
			);
			
        $this->__dispatch_map['getAccountCodeList'] = array(	
            'in'  => array('token' => 'string'),
			'out' => array('result' => '{urn:MetreosAppSuiteAdmin}ArrayOfString')
            );
			
		$this->__dispatch_map['userExists'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'boolean')
            );

		$this->__dispatch_map['userIdExists'] = array(	
            'in'  => array('token' => 'string', 'userId' => 'integer'),
			'out' => array('result' => 'boolean')
            );
			
		$this->__dispatch_map['accountCodeExists'] = array(	
            'in'  => array('token' => 'string', 'accountCode' => 'integer'),
			'out' => array('result' => 'boolean')
            );
			
		$this->__dispatch_map['addUser'] = array(	
            'in'  => array('token' => 'string', 'userRecord' => '{urn:MetreosAppSuiteAdmin}UserRecord'),
			'out' => array('result' => 'boolean')
            );
			
		$this->__dispatch_map['deleteUser'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'boolean')
            );			
			
		$this->__dispatch_map['deleteAllUsers'] = array(	
            'in'  => array('token' => 'string'),
			'out' => array('result' => 'boolean')
            );	

		$this->__dispatch_map['setPassword'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'password', 'string'),
			'out' => array('result' => 'boolean')
            );	

		$this->__dispatch_map['setStatus'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'status' => 'string'),
			'out' => array('result' => 'boolean')
            );	

		$this->__dispatch_map['getActiveSessions'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );	

		$this->__dispatch_map['getCreateDate'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'timestamp')
            );	

		$this->__dispatch_map['getExternalAuthEnabled'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'boolean')
            );	

		$this->__dispatch_map['getExternalAuthDn'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
            );		

		$this->__dispatch_map['getNumFailedLogins'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );	

		$this->__dispatch_map['getLastLockout'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'timestamp')
            );	

		$this->__dispatch_map['getLastUsed'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'timestamp')
            );	

		$this->__dispatch_map['getLdapServerId'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );				
			
		$this->__dispatch_map['getLdapSynched'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'boolean')
            );		

		$this->__dispatch_map['getUserId'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );	

		$this->__dispatch_map['getUsername'] = array(	
            'in'  => array('token' => 'string', 'userId' => 'integer'),
			'out' => array('result' => 'string')
            );	

		$this->__dispatch_map['getAccountCode'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );	

		$this->__dispatch_map['getPassword'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
            );				
		
		$this->__dispatch_map['getFirstName'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
            );	

		$this->__dispatch_map['getLastName'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
            );	

		$this->__dispatch_map['getEmail'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
            );				
			
		$this->__dispatch_map['getLockoutThreshold'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );		

		$this->__dispatch_map['getLockoutDuration'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'time')
            );	

		$this->__dispatch_map['getMaxSessions'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );	

		$this->__dispatch_map['getPinChange'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'boolean')
            );	

		$this->__dispatch_map['getPin'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'integer')
            );	
			
		$this->__dispatch_map['getRecord'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'boolean')
            );				

		$this->__dispatch_map['getRecordVisible'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'boolean')
            );	

		$this->__dispatch_map['getArTransferNumber'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
            );	

		$this->__dispatch_map['getTimezone'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
            );		

		$this->__dispatch_map['setUsername'] = array(	
            'in'  => array('token' => 'string', 'userId' => 'integer', 'username' => 'string'),
			'out' => array('result' => 'boolean')
			);

		$this->__dispatch_map['changeUsername'] = array(	
            'in'  => array('token' => 'string', 'oldUsername' => 'string', 'newUsername' => 'string'),
			'out' => array('result' => 'boolean')
			);
		
		$this->__dispatch_map['setPin'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'pin' => 'integer'),
			'out' => array('result' => 'boolean')
			);

		$this->__dispatch_map['setAccountCode'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'accountCode' => 'integer'),
			'out' => array('result' => 'boolean')
			);
			
		$this->__dispatch_map['setFirstName'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'firstName' => 'string'),
			'out' => array('result' => 'boolean')
			);
			
		$this->__dispatch_map['setLastName'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'lastName' => 'string'),
			'out' => array('result' => 'boolean')
			);

		$this->__dispatch_map['setEmail'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'email' => 'string'),
			'out' => array('result' => 'boolean')
			);

		$this->__dispatch_map['setLockoutThreshold'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'lockoutThreshold' => 'integer'),
			'out' => array('result' => 'boolean')
			);
			
		$this->__dispatch_map['setLockoutDuration'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'lockoutDuration' => 'integer'),
			'out' => array('result' => 'boolean')
			);
			
		$this->__dispatch_map['setMaxSessions'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'maxSessions' => 'integer'),
			'out' => array('result' => 'boolean')
			);

		$this->__dispatch_map['setPinChange'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'pinChange' => 'boolean'),
			'out' => array('result' => 'boolean')
			);

		$this->__dispatch_map['setRecord'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'record' => 'boolean'),
			'out' => array('result' => 'boolean')
			);
			
		$this->__dispatch_map['setRecordVisible'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'recordVisible' => 'boolean'),
			'out' => array('result' => 'boolean')
			);			
			
		$this->__dispatch_map['setArTransferNumber'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'arTransferNumber' => 'string'),
			'out' => array('result' => 'boolean')
			);	

		$this->__dispatch_map['setTimezone'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'timezone' => 'string'),
			'out' => array('result' => 'boolean')
			);	
			
		$this->__dispatch_map['getAddUserRequiredCols'] = array(	
            'in'  => array('token' => 'string'),
			'out' => array('result' => '{urn:MetreosAppSuiteAdmin}ArrayOfString')
			);	
			
		$this->__dispatch_map['getAddUserCols'] = array(	
            'in'  => array('token' => 'string'),
			'out' => array('result' => '{urn:MetreosAppSuiteAdmin}ArrayOfString')
			);		
			
		$this->__dispatch_map['getStatus'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string'),
			'out' => array('result' => 'string')
			);					
			
		$this->__dispatch_map['enableFindMeNumbers'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'respectVoicemail' => 'boolean'),
			'out' => array('result' => 'boolean')
			);
			
		$this->__dispatch_map['disableFindMeNumbers'] = array(	
            'in'  => array('token' => 'string', 'username' => 'string', 'respectVoicemail' => 'boolean'),
			'out' => array('result' => 'boolean')
			);
			
        $this->__typedef['UserRecord'] = array(	
			'firstName' => 'string', 
			'lastName' => 'string',
			'userName' => 'string',
			'password' => 'string',
			'pin' => 'integer',
			'accountCode' => 'integer',
			'email' => 'string',
			'status' => 'string',
			'lockoutThreshold' => 'integer',
			'lockoutDuration' => 'integer',
			'maxSessions' => 'integer',
			'pinChange' => 'boolean',
			'record' => 'boolean',
			'timezone' => 'string',
			'arTransferNumber' => 'string'
			);
													
		$this->__typedef['ArrayOfString'] = array(array('item' => 'string'));		
    }
   
    function _processToken($pickledToken, $accessLevel=Null) {
		try 
		{
            // Process token
            // Raise exception on expired/invalid token
            // Set the 'mUserData' data-structure representing the user/ACLs of the account
            // If 'accessLevel' is set, test mUserData for sufficient level of access
            if (!$pickledToken)
			{
                throw new Exception("Invalid authentication token");
            }
							
            // Extract session-id from token, and reload session
            $sid = unserialize(base64_decode(urldecode($pickledToken)));
            session_id($sid);
            session_start();
            	
            // If token has expired, raise Exception
            if (time() > ($_SESSION['token']['last_accessed'] + MceConfig::SESSION_TIMEOUT)) 
			{
                throw new Exception("Token Expired");
            } 
			else 
			{
                // Reset timeout
                $_SESSION['token']['last_accessed'] = time();
            }
            
            // Authenticate User and check access level
            
			if ($_SESSION['token']['user'] && $_SESSION['token']['password']) 
			{		
				$isAdmin = strtolower($_SESSION['token']['user']) == 'administrator';
				if ($isAdmin)
				{
					$r = $this->mDbObj->doQuery("select * from application_suite.as_configs where name = %s AND value = %s", 'admin_password', Utils::encrypt_password($_SESSION['token']['password']));
                    $user_access_level = AccessControl::ADMINISTRATOR;
                }
				else
				{
					throw new Exception("Invalid username/password");
				}
				
				// If invalid username/password - raise Exception
                if (!$r) 
				{
                    throw new Exception("Invalid username/password");
                }
				
				$this->mUserData = $r[0];

                // If accessLevel is set, check AccessLevel, if insufficient, raise Exception
                if (($accessLevel) && ($accessLevel > $user_access_level)) 
				{
                    throw new Exception("Access Denied");
                }
                
    	    }
        } catch (Exception $e) {
            // Catch and return the exception message
            return $e->getMessage();
        }
        // Return 'False' on success
        return False;
    }	
      
    function login($username, $password) 
	{
		if(empty($username))
		{
			$error = 'ERROR: WebService.login: Username cannot be NULL.';
			error_log($error);
			return new SOAP_Fault($error); 
		}
		
		if(empty($password))
		{
			$error = 'ERROR: WebService.login: Password cannot be NULL.';
			error_log($error);
			return new SOAP_Fault($error); 
		}
		
		// Make sure that the the username passed in is not administrator because that's reserved for the
		// mceadmin appsuiteadmin administrator	
		$tempUser = strtolower($username);
		
		if($tempUser != 'administrator')
		{
			$error = 'ERROR: WebService.login: Only the Metreos Application Suite Administrator may access the Web Service.';
			error_log($error);
			return new SOAP_Fault($error); 
		}

		// Start a new session
        session_start();
        
        $_SESSION['token'] = array(
            'user'          => $username,
    	    'password'      => $password,
            'last_accessed' => time());
			
		// Pickle the SID
        $pickledToken = urlencode(base64_encode(serialize(session_id())));
		
		// Process the Session 'token' variable, raise a SOAP_Fault on any errors
        if ($exception = $this->_processToken($pickledToken)) 
		{ 
			return new SOAP_Fault($exception); 
		}

		// Return the pickled SID
        return $pickledToken;
    }

	/**
	*	Get the list of current users in appsuiteadmin
	*/
	public function getUserList($token) 
	{	
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$userList = $this->mUserObj->getUserList();

		if($userList['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{
			return $userList['result'];
		}
	}
	
	/**
	*	Get the list of  account codes for all current users in appsuiteadmin
	*/
	public function getAccountCodeList($token) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$accountCodeList = $this->mUserObj->getAccountCodeList();
		
		if($accountCodeList['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{
			return $accountCodeList['result'];
		}
	}
	
	/**
	*	Check to see if the specified username exists
	*/
	public function userExists($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->userExists($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{
			return $rc['result'];
		}
	}	

	/**
	*	Check to see if the specified userId exists
	*/
	public function userIdExists($token, $userId) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->userIdExists($userId);	
	
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{
			return $rc['result'];
		}
	}	
	
	/**
	*	Check to see if the specified account code exists
	*/
	public function accountCodeExists($token, $account_code) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->accountCodeExists($account_code);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);		}
		else
		{
			return $rc['result'];
		}		
	}	
	
	/**
	*	Add a user to appsuiteadmin
	*/
	public function addUser($token, $userRecord) 
	{	
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}

		$rc = $this->mUserObj->addUser($userRecord);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);	
		}
		else
		{
			return $rc['result'];
		}
	}
	
	/**
	*	Delete a user from appsuiteadmin
	*/
	public function deleteUser($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->deleteUser($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Delete all users from appsuiteadmin
	*/
	public function deleteAllUsers($token) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->deleteAllUsers();
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the number of active sessions for a user
	*/
	public function getActiveSessions($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getActiveSessions($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the date a user was created
	*/
	public function getCreateDate($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getCreated($username);
	
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get whether external authentication is enabled for a user
	*/
	public function getExternalAuthEnabled($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getExternalAuth($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the external authentication DN for a user
	*/
	public function getExternalAuthDn($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getExternalAuthDn($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the number of failed logins for a user
	*/
	public function getNumFailedLogins($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getFailedLogins($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the datetime of the last lockout for the specified user
	*/
	public function getLastLockout($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getLastLockout($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the datetime of the last time the specified user logged in
	*/
	public function getLastUsed($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getLastUsed($username);
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	

	/**
	*	Get the LDAP server ID for the specified user
	*/
	public function getLdapServerId($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getLdapServerId($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get whether the LDAP server is in sync for the specified user
	*/
	public function getLdapSynched($token, $username) 
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getLdapSynched($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the user ID for the specified user.
	*/
	public function getUserId($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getUserId($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the username for the specified user by userId
	*/
	public function getUsername($token, $userId)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getUsername($userId);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the account code for the specified user
	*/
	public function getAccountCode($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getAccountCode($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	

	/**
	*	Get the password for the specified user
	*/
	public function getPassword($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getPassword($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
		
	/**
	*	Get the first name for the specified user
	*/
	public function getFirstName($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getFirstName($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the last name for the specified user
	*/
	public function getLastName($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getLastName($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the email address for the specified user
	*/
	public function getEmail($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getEmail($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the lockout threshold for the specified user
	*/
	public function getLockoutThreshold($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getLockoutThreshold($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	

	/**
	*	Get the lockout duration for the specified user
	*/
	public function getLockoutDuration($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getLockoutDuration($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the max concurrent sessions for the specified user
	*/
	public function getMaxSessions($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getMaxSessions($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the whether a pin change is required for the specified user on first login
	*/
	public function getPinChange($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getPinChange($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the whether a recording is enabled for the specified user
	*/
	public function getRecord($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getRecord($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the whether a it is visible to the specified user when a call is being recorded
	*/
	public function getRecordVisible($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getRecordVisible($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the ActiveRelay transfer number for the specified user
	*/
	public function getArTransferNumber($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getArTransferNumber($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the timezone for the specified user
	*/
	public function getTimezone($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getTimezone($username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Get the status for the current user
	*/
	public function getStatus($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getStatus($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Set the username for the specified userId
	*/
	public function setUsername($token, $userId, $username)
	{
	    $this->_processToken($token);
		$rc = $this->mUserObj->setUsername($userId, $username);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Function name: changeUsername
	*	Purpose: Change the username for the specified user
	*/
	public function changeUsername($token, $oldUsername, $newUsername)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->changeUsername($oldUsername, $newUsername);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Get the pin for the specified user
	*/
	public function getPin($token, $username)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->getPin($username);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}	
	
	/**
	*	Set the pin number for the specified user
	*/
	public function setPin($token, $username, $pin)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setPin($username, $pin);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Set the account code for the specified user
	*/
	public function setAccountCode($token, $username, $accountCode)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setAccountCode($username, $accountCode);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}	
	
	/**
	*	Set the password for the specified user
	*/
	public function setPassword($token, $username, $password)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setPassword($username, $password);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Set the first name for the specified user
	*/
	public function setFirstName($token, $username, $firstName)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setFirstName($username, $firstName);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}
	
	/**
	*	Set the last name for the specified user
	*/
	public function setLastName($token, $username, $lastName)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setLastName($username, $lastName);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}
	
	/**
	*	Set the email address for the specified user
	*/
	public function setEmail($token, $username, $email)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setEmail($username, $email);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Set the lockout threshold for the specified user
	*/
	public function setLockoutThreshold($token, $username, $lockoutThreshold)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setLockoutThreshold($username, $lockoutThreshold);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}

	/**
	*	Set the lockout duration for the specified user
	*/
	public function setLockoutDuration($token, $username, $lockoutDuration)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setLockoutDuration($username, $lockoutDuration);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Set the max concurrent sessions for the specified user
	*/
	public function setMaxSessions($token, $username, $maxSessions)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setMaxSessions($username, $maxSessions);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}		
	}
	
	/**
	*	Set pin change required for the specified user
	*/
	public function setPinChange($token, $username, $pinChange)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setPinChange($username, $pinChange);

		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);		}
		else
		{		
			return $rc['result'];
		}		
	}
	
	/**
	*	Set whether recording is enabled for the specified user
	*/
	public function setRecord($token, $username, $record)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setRecord($username, $record);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Set whether the specified user can see whether a call is being recorded
	*/
	public function setRecordVisible($token, $username, $recordVisible)
	{
	    $this->_processToken($token);
		$rc = $this->mUserObj->setRecordVisible($username, $recordVisible);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Set the ActiveRelay transfer number for the specified user
	*/
	public function setArTransferNumber($token, $username, $arTransferNumber)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setArTransferNumber($username, $arTransferNumber);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Set the timezone for the specified user
	*/
	public function setTimezone($token, $username, $timezone)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setTimezone($username, $timezone);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/** 
	*	Get a list of all required columns for addUser
	*/
	function getAddUserRequiredCols($token)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$requiredUserCols = $this->mUserObj->getAddUserRequiredCols();
		return $requiredUserCols;
	}
	
	/** 
	*	Get a list of all supported columns for addUser
	*/	
	function getAddUserCols($token)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$supportedUserCols = $this->mUserObj->getAddUserCols();
		return $supportedUserCols;
	}
	
	/**
	*	Set the status for the specified user. [Active | Disabled | Locked | Deleted]
	*/
	public function setStatus($token, $username, $status)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
		
		$rc = $this->mUserObj->setStatus($username, $status);
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['result'];
		}
	}
	
	/**
	*	Enable all FindMe numbers for a user
	*/
	public function enableFindMeNumbers($token, $username, $respectVoicemail)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
			
		$rc = $this->mUserObj->setFindMeNumbers($username, $respectVoicemail, 'enable');
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['rc'];
		}
	}
	
	/**
	*	Disable all FindMe numbers for a user
	*/
	public function disableFindMeNumbers($token, $username, $respectVoicemail)
	{
	    // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) 
		{ 
			return new SOAP_Fault($exception); 
		}
					
		$rc = $this->mUserObj->setFindMeNumbers($username, $respectVoicemail, 'disable');
		
		if($rc['rc'] == False)
		{
			return new SOAP_Fault($rc['error']);
		}
		else
		{		
			return $rc['rc'];
		}
	}
}

?>
