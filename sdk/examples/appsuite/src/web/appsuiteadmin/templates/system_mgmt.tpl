{include file="system_mgmt_nav.tpl"}

<form action="{$SCRIPT_NAME}" method="post">

    <h3>Change Administrator Password</h3>
    <table>
        <col class="inputLabels" />
        <col class="inputFields" style="width: 200px" />
        <col class="inputDescription" />
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Current Password</th>
            <td><input type="password" name="password" /></td>
            <td>Enter current password</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>New Password</th>
            <td><input type="password" name="new_password" /></td>
            <td>Enter new password to change it, or leave blank for no change</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Verify New Password</th>
            <td><input type="password" name="new_password_verify" /></td>
            <td>Enter new password again</td>
        </tr>
    </table>

    
    <h3>System</h3>
    <table>
        <col class="inputLabels" />
        <col class="inputFields" style="width: 200px" />
        <col class="inputDescription" />
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Default Time Zone</th>
            <td>{html_options values=$timezone_list output=$timezone_list name="default_timezone_offset" selected=$default_timezone_offset}</td>
            <td>Default time zone for users and administrator accounts</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Hide Devices From Users</th>
            <td><input type="checkbox" name="hide_devices_from_users" value="1" {if $hide_devices_from_users}checked="checked"{/if}/></td>
            <td>Hide device management functions from users</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Default Lockout Threshold</th>
            <td><input type="text" name="default_lockout_threshold" value="{$default_lockout_threshold}" size="5" /></td>
            <td>Default set lockout threshold for users</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Default Lockout Duration</th>
            <td><input type="text" name="default_lockout_duration" value="{$default_lockout_duration}" size="5" /></td>
            <td>Default lockout duration for users (in minutes)</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Default Max Concurrent Sessions</th>
            <td><input type="text" name="default_max_concurrent_sessions" value="{$default_max_concurrent_sessions}" size="5" /></td>
            <td>Default max concurrent sessions for users</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Media Ports</th>
            <td><input type="text" name="media_ports" value="{$media_ports}" size="5" /></td>
            <td>Number of available media ports</td>
        </tr>
    </table>

    
    <h3>SMTP Server</h3>
    <table>
        <col class="inputLabels" />
        <col class="inputFields" style="width: 200px" />
        <col class="inputDescription" />

        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Server</th>
            <td><input type="text" name="smtp_server" value="{$smtp_server|escape:"html"}" /></td>
            <td>Mail server to use for sending notifications</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Port</th>
            <td><input type="text" name="smtp_port" value="{$smtp_port}" size="5" /></td>
            <td>Mail server port</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>User</th>
            <td><input type="text" name="smtp_user" value="{$smtp_user|escape:"html"}" /></td>
            <td>User login to access the mail server</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Password</th>
            <td><input type="password" name="smtp_password" value="{$smtp_password|escape:"html"}" /></td>
            <td>User password to access the mail server</td>
        </tr>
    </table>

    
    <h3>RapidRecord</h3>
    <table>
        <col class="inputLabels" />
        <col class="inputFields" style="width: 200px" />
        <col class="inputDescription" />
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Recordings Expiration</th>
            <td><input type="text" name="recordings_expiration" value="{$recordings_expiration}" size="5" /></td>
            <td>Number of days to keep a recording (0 for indefinitely)</td>
        </tr>  
    </table>

    
    <h3>ScheduledConference</h3>
    <table>
        <col class="inputLabels" />
        <col class="inputFields" style="width: 200px" />
        <col class="inputDescription" />
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Scheduled Conference DN</th>
            <td><input type="text" name="scheduled_conference_dn" value="{$scheduled_conference_dn}" /></td>
            <td>Dialing number for all scheduled conferences</td>
        </tr>     
    </table>    
    

    <p>
    <input type="submit" name="submit" value="Apply" />
    <input type="submit" name="done" value="Done" />
    </p>
    
</form>