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

$valid          = false;

$ipAddress = $_SERVER['REMOTE_ADDR'];
$url = $_SERVER['REQUEST_URI'] . $_SERVER['QUERY_STRING'];
$userId = $_SESSION["users_id"];
if($userId)
{
	// Redirect them to demo listing
	header("Location: demolisting.php");
	exit();
}
if(isset($_POST["Submit"]))
{
	// Retrieve login fields
	$email			= $_POST["email"];
	$password		= $_POST["password"];

	// MD5 that password!
	$md5password = md5($password);
	
	// Prepare MYSQL connection
	mysql_connect($dbHost,$dbUser,$dbPassword);
	@mysql_select_db($database) or die( "Unable to connect to Demo database!  Contact scall@metreos.com");
	
	// Insert user into users table
	$query = "SELECT id FROM users WHERE email = '" . $email . "' AND password = '" . $md5password . "'";
	
	$result = mysql_query($query) or die("Unable to add user to demoe database!");
	
	$userSettings = mysql_fetch_array($result, MYSQL_NUM);
	
	$userId = $userSettings[0];
	
	mysql_close();
	
	// valid user
	if(0 != mysql_num_rows())
	{
		$valid = true;
	}
	
	// Create session for user
	$_SESSION["users_id"] = $userId;
	
	RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ipAddress, $url, 1);
	
	// Redirect them to demo listing
	header("Location: demolisting.php");
	exit();
}
else
{
    RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ipAddress, $url, $login);
}

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<title>Login</title>
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
			<!--<img src="" style="float:left;" alt="" />-->
			<!--<img src="metreos_logo.gif" style="float:right;" alt="Metreos Logo" />-->
			<img src="metreos_logo.gif" style="margin:auto;display:block" alt="Metreos Logo" />

			<h1>Login</h1>
			<form action="<? print($PHP_SELF) ?>" method="post" id="form1">
				<div>
					 <label>Email</label><input type="text" name="email" value="<? print($email) ?>" id="email" /></input><br />
					 <label>Password</label><input type="password" name="password" value="<? print($password) ?>" id="password" /></input><br />
				     <a style="font-size:75%" href="createaccount.php">Create Account</a>
					<!-- Submit button -->
					<input style="width:auto;margin:auto" type="submit" value="Login" id="Submit" name="Submit">
				</div>
			</form>
		</div>	
	</body>
</html>
