<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Cisco Unified Application Environment :: {$_title}</title>
    <link rel="stylesheet" href="/mceadmin/style.css" type="text/css" />
    {$_head_extra}
</head>

<body>

    <div id="box">
    
        <h1>
            <img src="/mceadmin/images/cisco_logo.gif" alt="Cisco Logo" />
            <br />
            Unified Application Environment Management Console
        </h1>
        
        <div id="mainContent">

<div>

<h3>Uncaught Exception</h3>

<table>
	<col style="inputLabels" />
	<col />
	<tr>
		<th>Error Code</th>
		<td>{$exception->getCode()|escape:"html"}</td>
	</tr>
	<tr>
		<th>Message</th>
		<td>{$exception->getMessage()|escape:"html"}</td>
	</tr>	
	<tr>
		<th>Stacktrace</th>
		<td>{$exception->getTraceAsString()|escape:"html"|nl2br}</td>
	</tr>	
	<tr>
		<th>File</th>
		<td>{$exception->getFile()|escape:"html"}</td>
	</tr>
</table>

</div>
        </div>
        
    </div>
    
    {if $_dev_mode}
    <p style="text-align: center;">
        <strong>DEVELOPMENT MODE</strong>
        {if !$_app_server_on}
        <br />
        <a href="main.php?reconnect=1" class="button">Reconnect To Application Server</a>
        {/if}
    </p>
    {/if}
    
    <p id="copyright">Copyright &copy; Cisco Systems, Inc.  All rights reserved.</p>

</body>
</html>