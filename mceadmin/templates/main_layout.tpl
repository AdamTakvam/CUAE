<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Cisco Unified Application Environment :: {$_title}</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="stylesheet" href="{$smarty.const.WEB_PATH}/style.css" type="text/css" />
    {$_head_extra}
    {if !empty($_focus_field)}
    <script type="text/javascript" src="{$smarty.const.WEB_PATH}/js/focus_field.js"> </script>
    <script type="text/javascript">
    	{literal}
    	function initFocusForm()
		{
		{/literal}
			focusField(document.getElementById("{$_focus_field}"));
			return true;
		{literal}
		}        
		{/literal}	
    	window.onload = initFocusForm;
    </script>
    {/if}
</head>

<body>

	<h1>
	    <img src="/mceadmin/images/cisco_logo.gif" alt="Cisco Logo" height="52" width="92" id="mainLogo" />
	    <span id="titleText">Unified Application Environment Management Console</span>
	</h1>
	
    <div id="systemName">{$_system_name|escape:"html"}</div>     
	
	<div id="consoleMode">
		{if $_dev_mode}
			DEVELOPMENT MODE
		{elseif $_sdk_mode}
			SDK MODE: Licensed for development and test use only
		{/if}  
	</div>

{if $_navigation}
   	<div id="actionLinks"> 	
		{if ($_app_server_enabled || $_dev_mode) && !$_app_server_on}
		<a href="main.php?reconnect=1">Connect to Application Server</a> |
		{/if}
		<a href="logout.php">Logout</a>
	</div>
    <div id="consoleStatus">         
        {if $_app_server_enabled}
            {if $_app_server_on}
                The application server service is <strong>running</strong>.
            {else}
                The application server service is enabled but <span class="warning">communication with it has failed</span>.
                <br />
                Please try to reestablish a connection to the application server or <a href="services.php">check the application server status</a>.
            {/if}
        {else}
            The application server service is <strong>disabled</strong>.
        {/if}
        {if $_apache_needs_restart}
            <br />
            <span class="warning">A recent change to the web server's configuration requires that the web server be restarted.</span>
            <br />
            Please <a href="services.php">go to Service Control</a> and restart the &quot;Management Console&quot; service as soon as possible.
        {/if}
    </div>
{/if}

{if $_title}
    <h2 id="pagetitle">{$_title|escape:"html"}</h2>
{/if}
{if $_navigation}
    <div id="navigation">
        <div id="breadcrumbs">
            {$_breadcrumbs}
        </div>
    </div>
{/if}

{if $_response_message}
    <div class="response">
    {$_response_message|escape:"html"|nl2br}
    </div>
{/if}

{if $_error_message}
    <div class="error">
        The following error(s) occurred:
        <br />
        {$_error_message|nl2br}
    </div>
{/if}

    <div id="mainContent">
    {include file="$_content_template"}
    </div>
    
    <div id="footer">
	    <div id="versions">
	    Firmware v{$_f_version} / Software v{$_s_version} {$_r_type}
	    {if $_serial_number}<br /> S/N {$_serial_number}{/if}
	    </div>
	
	    <div id="timestamp">
	    Current Time: {$smarty.now|date_format:"%D %I:%M:%S %p %Z"}
	    </div>
    </div>
    
	<p id="copyright">Copyright &copy; Cisco Systems, Inc.  All rights reserved.</p>

</body>
</html>