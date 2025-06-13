<?

function RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ip, $url, $login)
{
       // Prepare MYSQL connection
		mysql_connect($dbHost,$dbUser,$dbPassword);
		@mysql_select_db($database);
		
		if(!$userId)
		{
			$userId = "NULL";
		}
		if(!$ip)
		{
			$ip = "NULL";
		}
		else
		{
			$ip = "'" . $ip . "'";
		}
		if(!$url)
		{
			$url = "NULL";
		}
		else
		{
			$url = "'" . $url . "'";
		}
		if(!$login)
		{
			$login = 0;
		}
		else
		{
			$login = 1;
		}
		
		// Check for email usage and account code usage
		$query = "INSERT INTO usage_stats (ip, users_id, url, login) VALUES (" . $ip . ", " . $userId . ", " . $url . ", " . $login . ")";

		$result = mysql_query($query);
	
		mysql_close();	
} 


header("Cache-control: private");
session_start();

$dbHost		    = "localhost";
$dbUser			= "root";
$dbPassword		= "metreos";
$database		= "demosite";

$valid			= false;
$postback       = false;
$email			= "";
$password		= "";
$password2		= "";
$account_code   = "";
$pin			= "";
$pin2			= "";
$first_name		= "";
$last_name		= "";
$company		= "";
$dayphone		= "";

$emailValid			= true;
$passwordValid		= true;
$account_codeValid  = true;
$pinValid			= true;
$first_nameValid	= true;
$last_nameValid		= true;
$companyValid		= true;

$emailInUse         = false;
$accoun_codeInUse   = false;

$ipAddress = $_SERVER['REMOTE_ADDR'];
$url = $_SERVER['REQUEST_URI'] . $_SERVER['QUERY_STRING'];
		
$userId = "";

if(isset($_POST["Submit"]))
{
	$postback = true;
	// Retrieve login fields
	$email				= $_POST["email"];
	$pin				= $_POST["pin"];
	$pin2				= $_POST["pin2"];
	$account_code		= $_POST["account_code"];
	$password			= $_POST["password"];
	$password2			= $_POST["password2"];
	$first_name			= $_POST["first_name"];
	$last_name			= $_POST["last_name"];
	$company			= $_POST["company"];
	$dayphone			= $_POST["dayphone"];

    $atom = '[-a-z0-9!#$%&\'*+/=?^_`{|}~]';    // allowed characters for part before "at" character
    $domain = '([a-z]([-a-z0-9]*[a-z0-9]+)?)'; // allowed characters for part after "at" character
	$regex = '^' . $atom . '+' .        // One or more atom characters.
	'(\.' . $atom . '+)*'.              // Followed by zero or more dot separated sets of one or more atom characters.
	'@'.                                // Followed by an "at" character.
	'(' . $domain . '{1,63}\.)+'.        // Followed by one or max 63 domain characters (dot separated).
	$domain . '{2,63}'.                  // Must be followed by one set consisting a period of two
	'$';                                // or max 63 domain characters.
	
	// Validate login
	if(!$email || $email == "") 
	{
		$emailValid = false;
	}	
	else if(!eregi($regex, $email))
	{
		$emailValid = false;
	}
	if((!$password || !$password2 || $password1 == "" || $password2 == "") && ($password != $password2))
	{
		$passwordValid = false;
	}
	if(!$account_code || $account_code == "")
	{
		$account_codeValid = false;
	}
	if((!$pin || !$pin2 || $pin1 == "" || $pin2 == "") && ($pin != $pin2))
	{
		$pinValid = false;
	}
	if(!$first_name || $first_name == "")
	{
		$first_nameValid = false;
	}
	if(!$last_name || $last_name == "")
	{
		$last_nameValid = false;
	}
	if(!$company || $company == "")
	{
		$companyValid = false;
	}
	
	if($emailValid && $passwordValid && $account_codeValid && $pinValid && $first_nameValid && $last_nameValid && $companyValid)
	{
		$valid = true;
	}
	
	if($valid)
	{
		// MD5 that password!
		$md5password = md5($password);
		
		// Prepare MYSQL connection
		mysql_connect($dbHost,$dbUser,$dbPassword);
		@mysql_select_db($database) or die( "Unable to connect to Demo database!  Contact scall@metreos.com");
		
		// Check for email usage and account code usage
		$query = "SELECT email FROM users WHERE email = '" . $email . "'";

		$result = mysql_query($query) or die("Unable to test email usage");
		
		if(0 != mysql_num_rows($result))
		{
			$emailInUse = true;
		}
		else
		{
			$emailInUse = false;
		}
		
		// Check for email usage and account code usage
		$query = "SELECT account_code FROM users WHERE account_code = '" . $account_code . "'";
		$result = mysql_query($query) or die("Unable to test account_code usage");
		
		if(0 != mysql_num_rows($result))
		{
			$account_codeInUse = true;
		}
		else
		{
			$account_codeInUse = false;
		}
		
		 
		if($emailInUse == false && $account_codeInUse == false)
		{
			// Insert user into users table
			$query= "INSERT INTO users (email, password, account_code, pin, first_name, last_name, company, dayphone) 
			VALUES ('" . $email . "', '" . $md5password . "', '" . $account_code . "', '" . $pin . "', '" . $first_name . "', '" . $last_name . "', '" . $company . "', '" . $dayphone . "')";
				
			mysql_query($query) or die("Unable to add user to demo database!");
			
			$userId = mysql_insert_id();
			
			// Create new blank upc settings
			$query = "INSERT INTO upc_addresses (users_id)
			VALUES ('" . $userId . "')";
			
			mysql_query($query) or die("Unable to upc demo settings!");
			
			mysql_close();
			
			// Create session for user
			$_SESSION["users_id"] = $userId;
			
			// Redirect to demo listing page
			header("Location: demolisting.php");
		}
		else
		{
			mysql_close();
			$valid = false;
		}
	}
}

RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ipAddress, $url, $login);

if(!$valid) // not valid also implies that this is not a post.
{
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<title>Create Demo Account</title>
	<style type="text/css">
<!--
html, body
{
	background-color: #eee;
	font-size: medium;
	font-family: Tahoma, Verdana, Arial, sans-serif;
}

select
{
	display:block;
}

#form
{
	background-color: #fff;
	padding: 1em;
	border: 1px solid #666;
	margin: 5em auto;
	width: 500px;
}

#reminder
{
	position:relative;
	top:-25px;
	background-color: #fff;
	padding: 1em;
	border: 1px solid #666;
	margin: 5em auto;
	width:30%;
}

h1
{
	text-align: center;
	clear: both;
	font-size: 1.2em;
	margin: .5em;
}

label
{
	display: block;
	
	clear: left;
	width: 12em;
	font-size: small;
	font-weight: bold;
	margin-left:25%;
}

input
{
	display:block;
	width: 50%;
	margin-left:25%;
}

#logout
{
	display:block;
	position:absolute;
	right:10px;
	top:5px;
	border:#666 1px solid;
	padding:2px 5px;
	background-color:#fff;
	font-size:75%;
}
-->
</style>
	</head>
	<body>
		<a id="logout" href="logout_.php">Logout</a>
		<div id="form">
			<img src="metreos_logo.gif" style="margin:auto;display:block" alt="Metreos Logo" />

			<h1>Create Account</h1>
			<form action="<? print($PHP_SELF) ?>" method="post" id="form1">
				<div>
					 <label<? if(!$postback || $emailValid) { echo(""); } else { echo(" style=color:red "); } ?>>Email * <? if($emailInUse) { echo("<span style=\"color:red\">In Use</span>"); } else { echo(""); } ?></label><input type="text" name="email" value="<? print($email) ?>" id="email" /></input><br />
					 <label<? if(!$postback || $passwordValid) { echo(""); } else { echo(" style=color:red "); } ?>>Password *</label><input type="password" name="password" value="<? print($password) ?>" id="password" /></input><br />
					 <label<? if(!$postback || $passwordValid) { echo(""); } else { echo(" style=color:red "); } ?>>Password Again *</label><input type="password" name="password2" value="<? print($password2) ?>" id="password2" /></input><br />
					 <label<? if(!$postback || $account_codeValid) { echo(""); } else { echo(" style=color:red "); } ?>>Account Code * (0-9) <? if($account_codeInUse) { echo("<span style=\"color:red\">In Use</span>"); } else { echo(""); } ?></label><input type="text" name="account_code" value="<? print($account_code) ?>" id="account_code" /></input><br />
					 <label<? if(!$postback || $pinValid) { echo(""); } else { echo(" style=color:red "); } ?>>Pin * (0-9)</label><input type="password" name="pin" value="<? print($pin) ?>" id="pin" /></input><br />
					 <label<? if(!$postback || $pinValid) { echo(""); } else { echo(" style=color:red "); } ?>>Pin Again * (0-9)</label><input type="password" name="pin2" value="<? print($pin2) ?>" id="pin2" /></input><br /> 
					 <label<? if(!$postback || $first_nameValid) { echo(""); } else { echo(" style=color:red "); } ?>>First Name *</label><input type="text" name="first_name" value="<? print($first_name) ?>" id="first_name" /></input><br />
					 <label<? if(!$postback || $last_nameValid) { echo(""); } else { echo(" style=color:red "); } ?>>Last Name *</label><input type="text" name="last_name" value="<? print($last_name) ?>" id="last_name" /></input><br />
					 <label<? if(!$postback || $companyValid) { echo(""); } else { echo(" style=color:red "); } ?>>Company *</label><input type="text" name="company" value="<? print($company) ?>" id="company" /></input><br />
					 <label>Day Phone</label><input type="text" name="dayphone" value="<? print($dayphone) ?>" id="dayphone" /></input><br />
					 
					<!-- Submit button -->
					<input style="width:auto;margin:auto" type="submit" value="Create Account" id="Submit" name="Submit">
				</div>
			</form>
		</div>
	</body>
</html><?
}
else
{
?>
<html><body><p>Redirecting to demo listing.</p></body></html><?
}
?>