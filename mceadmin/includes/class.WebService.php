<?php
// MCE Web Service interface class
require_once("SOAP/Server.php");
require_once("SOAP/Disco.php");
require_once("common.php");
require_once("class.MysqlDAO.php");

class WebService {
    var $dbobj;
    var $mUserData;
    var $__dispatch_map = array();
    var $__typedef      = array();
    
    function WebService() {
        $this->dbobj = new MysqlDAO('localhost','','root','metreos');

        // Define all of the supported operations
        $this->__dispatch_map['login'] = array(
            'in'  => array('username' => 'string', 'password' => 'string'),
            'out' => array('token' => 'string')
            );
        $this->__dispatch_map['getUserList'] = array(
            'in' => array('token' => 'string'),
            'out' => array('userlist' => '{urn:MetreosMCEAdminService}ArrayOfString')
            );

        $this->__typedef['ArrayOfString'] = array(array('item' => 'string'));

    }

    function _processToken($pickledToken, $accessLevel=Null) {
        try {
            // process token
            // raise exception on expired/invalid token
            // set the 'mUserData' data-structure representing the user/ACLs of the account
            // if 'accessLevel' is set, test mUserData for sufficient level of access
            if (!$pickledToken) {
                throw new Exception("Invalid authentication token");
            }
            // Extract session-id from token, and reload session
            $sid = unserialize(base64_decode(urldecode($pickledToken)));
            session_id($sid);
            session_start();
            
            // If token received from different client, raise Exception
            if ($_SERVER['REMOTE_ADDR'] != $_SESSION['token']['ip']) {
                throw new Exception("Invalid token");
            }

            // If token has expired, raise Exception
            if (time() > ($_SESSION['token']['last_accessed'] + MceConfig::SESSION_TIMEOUT)) {
                throw new Exception("Token Expired");
            } else {
                // Reset timeout
                $_SESSION['token']['last_accessed'] = time();
            }
            
            // Authenticate User and check access level
            if ($token['user'] && $_SESSION['token']['password']) {
                $r = $this->dbobj->doQuery("select * from mce.mce_users where username = %s and password = %s", $_SESSION['token']['user'], $_SESSION['token']['password']);
                
                // If invalid username/password - raise Exception
                if (!$r) {
                    throw new Exception("Invalid username/password");
                }
                $this->mUserData = $r[0];

                // If accessLevel is set, check AccessLevel, if insufficient, raise Exception
                if (($accessLevel) && ($accessLevel > $this->mUserData['access_level'])) {
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
    
    function login($username, $password) {
        // return a token

        // Start a new session
        session_start();
        
        // Construct a session variable 'token'
        $_SESSION['token'] = array(
            'user'          => $username,
            'password'      => Utils::encrypt_password($password),
            'ip'            => $_SERVER['REMOTE_ADDR'],
            'last_accessed' => time());

        // pickle the SID
        $pickledToken = urlencode(base64_encode(serialize(session_id())));

        // Process the Session 'token' variable, raise a SOAP_Fault on any errors
        if ($exception = $this->_processToken($pickledToken)) { return new SOAP_Fault($exception); }

        // Return the pickled SID
        return $pickledToken;
    }

    function getUserList($token) {
        // Token check, raise SOAP_Fault on an invalid/expired token
        if ($exception = $this->_processToken($token,1)) { return new SOAP_Fault($exception); }

        // Get User list
        $r = $this->dbobj->doQuery("select username from mce.mce_users");
        $userList = array();
        foreach ($r as $row) {
            array_push($userList, $row['username']);
        }
        return $userList;
    }
}

?>
