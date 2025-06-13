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

<h3>Unhandled Exception</h3>

<p>
We apologize.  An error has occurred from which the system could not recover.  This error has been recorded in the Management Log, and you may contact customer support about this error.
</p>

<form>
	<input type="button" name="go back" onclick="history.go(-1);" value="Go Back" />
</form>

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