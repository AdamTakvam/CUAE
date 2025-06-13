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

$ipAddress = $_SERVER['REMOTE_ADDR'];
$url = $_SERVER['REQUEST_URI'] . $_SERVER['QUERY_STRING'];
$userId = $_SESSION["users_id"];

if(!$userId)
{
	// Session is not valid. Redirect to login
	header("Location: login.php");
	exit();
}

if(isset($_POST["Submit"]))
{
	// Insert new address data
	mysql_connect($dbHost, $dbUser, $dbPassword);
	@mysql_select_db($database) or die( "Unable to connect to UPC database!  Contact scall@metreos.com");
	$query="UPDATE upc_addresses set address1='" . $_POST["address1"] . "', address2='" . $_POST["address2"] . "', address3='" . $_POST["address3"] . "', address4='" . $_POST["address4"] . "', address5='" . $_POST["address5"] . "' WHERE users_id = " . $userId;
	mysql_query($query) or die( "Unable to update UPC addresses! Error as follows:\n" . mysql_error());
	mysql_close();
}

mysql_connect($dbHost,$dbUser,$dbPassword);
@mysql_select_db($database) or die( "Unable to connect to UPC database!  Contact scall@metreos.com");
$query="SELECT * FROM upc_addresses WHERE users_id = " . $userId;
$result = mysql_query($query) or die( "Unable to query UPC addresses! Error as follows:\n" . mysql_error());
$onlyRow = mysql_fetch_array($result, MYSQL_NUM);
$address1 = $onlyRow[1];
$address2 = $onlyRow[2];
$address3 = $onlyRow[3];
$address4 = $onlyRow[4];
$address5 = $onlyRow[5];

mysql_close();

RecordStats($dbHost, $dbUser, $dbPassword, $database, $userId, $ipAddress, $url, $login);

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<title>UPC Lookup by Metreos</title>
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

			<h1>UPC Lookup Demo&nbsp;&nbsp;&nbsp;&nbsp;512-687-2047</h1>
			<form action="<? print($PHP_SELF) ?>" method="post" id="form1">
				<div>
					 <label>SMS/Email 1</label><input type="text" name="address1" value="<? print($address1) ?>" id="address1" /></input><br />
					 <label>SMS/Email 2</label><input type="text" name="address2" value="<? print($address2) ?>" id="address2" /></input><br />
					 <label>SMS/Email 3</label><input type="text" name="address3" value="<? print($address3) ?>" id="address3" /></input><br />
					 <label>SMS/Email 4</label><input type="text" name="address4" value="<? print($address4) ?>" id="address4" /></input><br />
					 <label>SMS/Email 5</label><input type="text" name="address5" value="<? print($address5) ?>" id="address5" /></input><br />
					 
					<!-- Submit button -->
					<input style="width:auto;margin:auto" type="submit" value="Update Subscribers" id="Submit" name="Submit">
				</div>
			</form>
		</div>
		
		<div id="reminder">
			<table>
				<tr>
					<th style="text-align:left">Carrier</th>
					<th>Send SMS to CellNumber + ...</th>
				</tr>
				<tr>
					<td>Alltel</td>
					<td>@message.alltel.com</td>
				</tr>
				<tr>
					<td>AT&amp;T</td>
					<td>@mobile.att.net</td>
				</tr>
				<tr>

					<td>Cingular</td>
					<td>@mobile.mycingular.com </td>
				</tr>
				<tr>
					<td>Nextel</td>
					<td>@messaging.nextel.com</td>
				</tr>

				<tr>
					<td>Sprint</td>
					<td>@messaging.sprintpcs.com</td>
				</tr>
				<tr>
					<td>SunCom</td>
					<td>@tms.suncom.com</td>

				</tr>
				<tr>
					<td>T-mobile</td>
					<td>@tmomail.net</td>
				</tr>
				<tr>
					<td>VoiceStream</td>
					<td>@voicestream.net</td>
				</tr>
				<tr>
					<td>Verizon</td>
					<td>@vtext.com</td>
				</tr>
			</table>

		</div>
	</body>
</html>
