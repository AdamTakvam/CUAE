<?php

/* The AccessControl class is the class/object that manages the user sessions
 * on the management console.  It looks up a user's information from the database
 * and regulates permissiosn based on that user.
 */

require_once("common.php");

class AccessControl
{

/* ACCESS LEVEL ENUMERATIONS */

    const UNKNOWN =         0;
    const ADMINISTRATOR =   1;
    const RESTRICTED =      2;
    const NORMAL =          3;

    
/* PRIVATE MEMBERS */
    
    private $mUserData;
    

/* PUBLIC METHODS */
    
    public function __construct()
    // Initiate the request session
    {
        $this->mUserData = array();
        if (!is_dir(SESSIONS_PATH))
			mkdir(SESSIONS_PATH, 0600, TRUE);
		session_save_path(SESSIONS_PATH);
        session_name("CuaeManagement");
        session_start();
    }

    public function Login($username, $password)
    // Attempt to create the user's session with the login information $username and $password
    {
        $db = new MceDb();
        $rs = $db->Execute("SELECT * FROM mce_users WHERE username = ? AND password = ?",
                           array($username, Utils::encrypt_password($password)));
        if ($rs->RecordCount() > 0)
        {
            $this->mUserData = $rs->FetchRow();
            $_SESSION['id'] = $this->mUserData['mce_users_id'];
            $_SESSION['user_agent'] = $_SERVER['HTTP_USER_AGENT'];
            $_SESSION['last_accessed'] = time();
            $this->GenerateSessionKey();
            return TRUE;
        }
        else
        {
            $this->Destroy();
            return FALSE;
        }
    }
    
    public function CheckSession()
    // Do various checks to make sure the session is still valid or (hopefully) not stolen
    {
        if (empty($_SESSION))
            return FALSE;
        if (time() > ($_SESSION['last_accessed'] + MceConfig::SESSION_TIMEOUT))
            return FALSE;
        if ($_SERVER['HTTP_USER_AGENT'] <> $_SESSION['user_agent'])
            return FALSE;
        if ($_SESSION['key'] <> $_COOKIE['sesskey'])
            return FALSE;
        
        // Session seems okay, retrieve user data
        $db = new MceDb();
        $rs = $db->Execute("SELECT * FROM mce_users WHERE mce_users_id = ?", array($_SESSION['id']));
        if ($rs->RecordCount() == 0)
            return FALSE;
        $this->mUserData = $rs->FetchRow();
        
        $this->Touch();
        return TRUE;
    }
        
    public function CheckAccess($accessLevel)
    // Check to see if the user's access level is at least $accessLevel.
    {              
        if ($this->CheckSession())
        {
            if ($accessLevel >= $this->mUserData['access_level'])
                return TRUE;
        }
        else
        {
            $this->Destroy();
            Utils::redirect('index.php?expired=1');
        }
        return FALSE;
    }

    public function CheckPageAccess($accessLevel)
    // Check to see if this user's access level is at least $accessLevel.
    // If not, then notify the user.
    {
        if (!$this->CheckAccess($accessLevel))
            Utils::redirect("/mceadmin/page.php?no_access");
        return TRUE;
    }

    public function CheckWizardAccess()
    // Check to see if the user can access an install wizard page
    {
        if (INSTALL_PERFORMED)
            $this->CheckPageAccess(AccessControl::ADMINISTRATOR);
    }

    public function Touch()
    {
    // Unconditinally updates the user's access time
        $_SESSION['last_accessed'] = time();
    }

    public function GetData($data)
    // Get the data named $data that is associated with the user
    {
        if ($this->mUserData == array())
            $this->CheckSession();
        return $this->mUserData[$data];
    }
    
    public function Destroy()
    // Ends the user's session with the maanagement console
    {
        setcookie('sesskey', FALSE, 0);        
        session_unset();
        session_destroy();
    }
    
/* PRIVATE METHODS */
    
    private function GenerateSessionKey()
    {
    // We generate our own key and and force its validation for the session
        $key = "";
        for ($x = 0; $x < 20; ++$x)
        {
            $key .= chr(mt_rand(33,126));
        }
        $_SESSION['key'] = $key;
        setcookie('sesskey', $key, 0, NULL, NULL, FALSE, TRUE);
    }
    
}

?>
