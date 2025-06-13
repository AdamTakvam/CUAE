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


$dbHost		    = "localhost";
$dbUser			= "root";
$dbPassword		= "metreos";
$database		= "demosite";
$upcdemo        = false;
$remoteagent    = false;

$ipAddress = $_SERVER['REMOTE_ADDR'];
$url = $_SERVER['REQUEST_URI'] . $_SERVER['QUERY_STRING'];

session_start();
$userId = $_SESSION["users_id"];
$_SESSION = array();
session_destroy();

RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ipAddress, $url, $login);

header("Cache-control: private");
header("Location: login.php");
?>