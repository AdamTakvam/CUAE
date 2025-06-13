{if ($_app_server_enabled && !$_app_server_on) || $alarms_active || ($_sdk_mode && !$_dev_mode)}
<div class="overview">
    <h3>Notices</h3>

		{if $_sdk_mode && !$_dev_mode}
		<p>
		This server is qualified only for SDK licensing.  SDK licensing allows for development and test use only.
		</p>
		{/if}

        {if $_app_server_enabled && !$_app_server_on}
        <p class="warning">
        The application server is not running or the console is having trouble communicating with the server.
        Many of the functions of the management console have been disabled.
        Please try restarting the application server.
        </p>
        {/if}
        
        {if $alarms_active}
        <p class="warning">
        There are active alarms.  
        {if $alarm_count_red > 0}You have {$alarm_count_red} red-level alarm(s).{/if}
        {if $alarm_count_yellow > 0}You have {$alarm_count_yellow} yellow-level alarm(s).{/if}
        You can <a href="alarm_list.php">manage these alarms</a>.
        </p>
        {/if}
        
</div>
{/if}

<div class="mainBlock">
    <h3>Local Environment</h3>
    <ul>
        {if $admin_access}
        <li><a href="users.php">User Management</a></li>
        {/if}
        <li><a href="component_list.php?type={$core}">Core Component Configuration</a></li>
        {if $admin_access}
        <li><a href="media_server_config.php">Media Engine Configuration</a></li>
        <li><a href="logserver.php">Log Server Configuration</a></li>
        <li><a href="voice_recognition.php">Speech Recognition Configuration</a></li>
        <li><a href="alarm_list.php">Alarm Management</a></li>
        {/if}
    </ul>
</div>

<div class="mainBlock">
    <h3>Components</h3>
    <ul>
        <li><a href="component_list.php?type={$app}">Applications</a></li>
        <li><a href="media_server_list.php">Media Engines</a></li>
        <li><a href="component_list.php?type={$provider}">Providers</a></li>
        <li><a href="telephony.php">Telephony Servers</a></li>
    </ul>
</div>

<div class="clear"></div>

{if $admin_access}
<div class="mainBlock">
    <h3>System</h3>
    <ul>
        <li><a href="services.php">Service Control</a></li>
        <li><a href="license_mgmt.php">License Management</a></li>
        <li><a href="stats.php">Statistics</a></li>
        <li><a href="ssl_management.php">SSL Management</a></li>
        <li><a href="redundancy.php">Redundancy Setup</a></li>
        <!--
        <li><a href="backup.php">System Backup</a></li>
        <li><a href="restore.php">System Restore</a></li>
        -->
    </ul>
</div>


<div class="mainBlock">
    <h3>Logs</h3>
    <ul>
        <li><a href="logs.php">Server Logs</a></li>
        <li><a href="audit_log.php">Event Log</a></li>
        <li><a href="audit_log.php?t=2">Audit Log</a></li>
    </ul>
</div>
{/if}

<div class="clear"></div>
