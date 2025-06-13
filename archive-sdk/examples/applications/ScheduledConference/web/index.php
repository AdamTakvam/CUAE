<html>
<head>
<title>Schedule Conference</title>
</head>
<body>

<?php
if (isset($_POST['submit'])) { // if form has been submitted


        $dbHost = "localhost";          // MySql host name
        $dbName = "metreos";            // Database name
        $dbUser = "metreos";            // User name
        $dbPasswd = "metreos";          // Password
        $link = @mysql_connect( $dbHost, $dbUser, $dbPasswd ) or mysql_error();
        mysql_select_db( $dbName ) or mysql_error();

        do 
        {
            $confNumber = rand(10000, 99999);
            $check = mysql_query("SELECT * FROM scheduledconferences WHERE confNumber=$confNumber");
            $rows = mysql_num_rows($check);
        }	while ($rows != 0);

		
		$hour = $_POST['hour'];
		
		if ($_POST['pmam'] == 'am')
		{
			if ($hour == '12')
				$hour = '00';
		}
		else 
		{
			if ($hour != '12')
				$hour += 12;
		}
		
		$date = $_POST['year']."-".$_POST['month']."-".$_POST['day']." ".$hour.":".$_POST['minute'].":00";
		$insertionString = $confNumber.",0,'".$date."',".$_POST['numhours'].",0";
        mysql_query("insert into scheduledconferences values ($insertionString)");

        mysql_close($link);
?>

Your conference has been scheduled for <?php echo $date . " for " . $_POST['numhours'] . " hour(s).";
echo "Your conference pin number is: $confNumber. Please write it down.";
}
else
{
?>


<h1>Schedule a conference:</h1>
<form action="<?php echo $_SERVER['PHP_SELF']?>" method="post">
<table align="center" border="1" cellspacing="3" cellpadding="3">
<tr><td>Date to shedule conference for</td><td>
<select name="month">
<?
$months = array (
    1 => "Jan",
    2 => "Feb",
    3 => "Mar",
    4 => "Apr",
    5 => "May",
    6 => "Jun",
    7 => "Jul",
    8 => "Aug",
    9 => "Sep",
    10 => "Oct",
    11 => "Nov",
    12 => "Dec",
);
for ($x = 1; $x <= 12; ++$x)
{
    echo "<option value=\"$x\">" . $months[$x] . "</option>";
}
?>
</select>
<select name="day">
<?
for ($x = 1; $x <= 31; ++$x)
{
    echo "<option value=\"$x\">$x</option>";
}
?>
</select>
<select name="year">
<?
for ($x = 0; $x < 5; ++$x)
{
    $year = intval(date("Y")) + $x;
    echo "<option value=\"$year\">$year</option>";
}
?>
</select>
</td></tr>
<tr><td>Time</td>
<td>
<select name="hour">
<?
for ($x = 1; $x <= 12; ++$x)
{
    echo "<option value=\"$x\">$x</option>";
}
?>
</select>
:
<select name="minute">
    <option value="00">00</option>
    <option value="15">15</option>
    <option value="30">30</option>
    <option value="45">45</option>
</select> 
<select name="pmam">
    <option value="pm">PM</option>
    <option value="am">AM</option>
</select>
</td>
</tr>

<tr><td>Number of hours</td><td>
<select name="numhours">
<?
for ($x = 1; $x <= 8; ++$x)
{
    echo "<option value=\"$x\">$x</option>";
}
?>
</select>
</td></tr>
<tr><td colspan="2" align="right">
<input type="submit" name="submit" value="Submit">
</td></tr>
</table>
</form>

<?php
}
?>

</body>
</html>