
<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>First Name</th>
        <td>{$first_name|escape:"html"}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Last Name</th>
        <td>{$last_name|escape:"html"}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>E-mail Address</th>
        <td>{$email|escape:"html"}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Username</th>
        <td>{$username|escape:"html"}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Account Code</th>
        <td>{$account_code|escape:"html"}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Status</th>
        <td>Deleted</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Lockout Threshold</th>
        <td>{$lockout_threshold}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Lockout Duration</th>
        <td>
            {$lockout_duration.hours} Hours and {$lockout_duration.minutes} Minutes
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Max Concurrent Sessions</th>
        <td>{$max_concurrent_sessions}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Failed Logins</th>
        <td>{$failed_logins}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Current Active Sessions</th>
        <td>{$current_active_sessions}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>PIN Change</th>
        <td>
            {if $pin_change_required == 1}Required{else}Not Required{/if}
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>External Authentication</th>
        <td>
            {if $external_auth_enabled == 1}Enabled{else}Disabled{/if}
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Record Calls</th>
        <td>
            {if $record}Yes{else}No{/if}
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Recordings Visible</th>
        <td>
            {if $recording_visible}Yes{else}No{/if}
        </td>
    </tr>
</table>
<input type="submit" name="cancel" value="Done" />
</form>