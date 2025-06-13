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
$upcdemo        = false;
$remoteagent    = false;

$ipAddress = $_SERVER['REMOTE_ADDR'];
$url = $_SERVER['REQUEST_URI'] . $_SERVER['QUERY_STRING'];
$userId = $_SESSION["users_id"];

if($userId)
{
	// Prepare MYSQL connection
	mysql_connect($dbHost, $dbUser, $dbPassword);
	@mysql_select_db($database) or die("Unable to connect to Demo database!  Contact scall@metreos.com");

	// Insert user into users table
	$query="SELECT upcdemo, remoteagent FROM users WHERE id = " . $userId; 
	
	$result = mysql_query($query) or die("Unable to retrieve demos for user");

	$userSettings = mysql_fetch_array($result, MYSQL_NUM);

	if($userSettings)
	{
		$upcdemo = $userSettings[0];
		$remoteagent = $userSettings[1];
	}
	
	mysql_close();
	
	RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ipAddress, $url, $login);
}
else
{

    RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ipAddress, $url, $login);
	
	// Session is not valid. Redirect to login
	header("Location: login.php");
}

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<title>Demo Listing</title>
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

a
{
	display:block;
    margin:auto;
    text-align:center;
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

			<h1>Your Configured Demonstrations</h1>
				<div id="form1">
					<br />
					<br />
					<?
					if($upcdemo == 0 && $remoteagent == 0)
					{
						echo("<p>There are no demonstrations configured for you at this time.<br /> Contact Metreos to add access to IP Telephony demonstrations.</p>");
					}
					if($upcdemo)
					{
						echo("<a href=\"UpcLookup.php\">UPC Lookup</a>");
					}
					if($remoteagent)
					{
						echo("<a href=\"RemoteAgentConcept.html\">Remote Agent</a>");
					}
					?>
				</div>
			</form>
		</div>
	</body>
</html>