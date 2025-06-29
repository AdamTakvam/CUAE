<form action="{$SCRIPT_NAME}" method="post" autocomplete="off">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>First Name</th>
        <td><input type="text" name="first_name" value="{$first_name|escape:"html"}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Last Name</th>
        <td><input type="text" name="last_name" value="{$last_name|escape:"html"}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>E-mail Address</th>
        <td><input type="text" name="email" value="{$email}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Username</th>
        <td>
        {if $group_admin_access || $admin_access}
            <input type="text" name="username" value="{$username|escape:"html"}" {if $external_auth_enabled}disabled="disabled"{/if} />
        {else}
            {$username|escape:"html"}
        {/if}
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Password</th>
        <td><input type="button" onclick="window.open('user_password.php?id={$id}','password','width=600,height=400,resizable=yes,scrollbars=yes');" value="Change Password"  {if $external_auth_enabled}disabled="disabled"{/if} /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Account Code</th>
        <td>
        {if $group_admin_access || $admin_access}
            <input type="text" name="account_code" value="{$account_code}" />
        {else}
            {$account_code}
        {/if}
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>PIN</th>
        <td><input type="button" onclick="window.open('user_pin.php?id={$id}','pin','width=600,height=400,resizable=yes,scrollbars=yes');" value="Change PIN"  {if $external_auth_enabled}disabled="disabled"{/if} /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Time Zone</th>
        <td>
            {html_options values=$timezone_list output=$timezone_list name="time_zone" selected=$time_zone}
        </td>
    </tr>
{if $group_admin_access || $admin_access}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Status</th>
        <td>
            <select name="status">
            {foreach from=$s_options item=s_option key=s_option_value}
                <option value="{$s_option_value}" {if $s_option_value == $status}selected="selected"{/if}>{$s_option}</option>
            {/foreach}
            </select>
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Group</th>
        <td>
            <select name="as_user_groups_id">
            {if $admin_access}<option value=""></option>{/if}
            {foreach from=$user_groups item=group }
                <option value="{$group.as_user_groups_id}" {if $as_user_groups_id == $group.as_user_groups_id}selected="selected"{/if}>{$group.name|escape:"html"}</option>
            {/foreach}
            </select>
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Lockout Threshold</th>
        <td><input type="text" name="lockout_threshold" value="{$lockout_threshold}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Lockout Duration</th>
        <td>
            <input type="text" name="lockout_duration[hours]" value="{$lockout_duration.hours}" size="2" /> Hours
            <input type="text" name="lockout_duration[minutes]" value="{$lockout_duration.minutes}" size="2" maxlength="2" /> Minutes
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Max Concurrent Sessions</th>
        <td><input type="text" name="max_concurrent_sessions" value="{$max_concurrent_sessions}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Failed Logins</th>
        <td>{$failed_logins} <input type="submit" name="reset_failed_logins" value="Reset" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Current Active Sessions</th>
        <td>{$current_active_sessions} <input type="submit" name="reset_concurrent_logins" value="Reset" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>PIN Change</th>
        <td>
            <input type="radio" name="pin_change_required" value="1" {if $pin_change_required == 1}checked="checked"{/if}/> Required
            <input type="radio" name="pin_change_required" value="0" {if $pin_change_required == 0}checked="checked"{/if}/> Not Required
        </td>
    </tr>
    {if $ldap_synched && !$own_profile}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>External Authentication</th>
        <td>
            <input type="radio" name="external_auth_enabled" value="1" {if $external_auth_enabled == 1}checked="checked"{/if} {if !$external_auth_dn}disabled="disabled"{/if} /> Enabled
            <input type="radio" name="external_auth_enabled" value="0" {if $external_auth_enabled == 0}checked="checked"{/if} /> Not Enabled
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>External Authentication DN</th>
        <td>
            <input type="text" name="external_auth_dn" style="width:40em;" value="{$external_auth_dn|escape:"html"}" onchange="javascript:this.form.external_auth_enabled[0].disabled=false;" disabled="disabled" />
            <input type="button" name="enable_edit" onclick="javascript:this.form.external_auth_dn.disabled=false;" value="Edit This Field" />
        </td>
    </tr>
    {/if}
{/if}
{if $app_rr_exposed}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Record Calls</th>
        <td>
            <input type="checkbox" name="record" {if $record}checked="checked"{/if} />
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Recordings Visible</th>
        <td>
            <input type="checkbox" name="recording_visible" {if $recording_visible}checked="checked"{/if} />
        </td>
    </tr>
{/if}
{if $app_ar_exposed}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>ActiveRelay Transfer Number</th>
        <td>
            <input type="text" name="ar_transfer_number" value="{$ar_transfer_number|escape:"html"}" />
        </td>
    </tr>    
{/if}
</table>
<input type="hidden" name="id" value="{$id}" />
<input type="submit" name="submit" value="Apply User Settings" /> {if $group_admin_access}<input type="submit" name="delete" value="Delete User" />{/if} <input type="submit" name="cancel" value="Done" />
</form>


{if not $hide_devices}
    <h3>Devices</h3>
    <span style="font-size: .8em"><img src="./images/reddot.gif" width="11" height="11" alt="Red dot" /> = Primary Device</span>
    <form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col style="width: 16px" />
        <col />
        <col />
        <col style="width: 250px" />
        <tr>
            <th>&nbsp;</th>
            <th>Descriptive Name</th>
            <th>Device Name</th>
            <th>&nbsp;</th>
        </tr>
        {foreach from=$devices item=device}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>
                {if $device.is_primary_device}<img src="./images/reddot.gif" width="11" height="11" alt="Red dot indicating primary device" />{/if}
            </td>
            <td>{$device.name|escape:"html"}</td>
            <td>{$device.mac_address|escape:"html"}</td>
            <td>
                <input type="submit" name="set_primary_device[{$device.as_phone_devices_id}]" value="Set Primary" />
                <input type="submit" name="edit_device[{$device.as_phone_devices_id}]" value="Edit" />
                <input type="submit" name="delete_device[{$device.as_phone_devices_id}]" value="Delete" />
            </td>
        </tr>
        {/foreach}
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="add_device" value="Add Device" />
    {if $voicemail_settings_exposed}
    <br /><br />
    <input type="submit" name="edit_voicemail" value="Edit VoiceMail Settings" />
    {/if}
    </form>
{/if}


{if $app_ar_exposed || $app_ra_exposed}
    <h3>Find Me Numbers</h3>
    <form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col />
        <col />
        {if $app_ar_exposed}<col />{/if}
        <col style="width: 13em;" />
        <tr>
            <th>Name</th>
            <th>Phone Number</th>
            {if $app_ar_exposed}<th>ActiveRelay</th>{/if}
            <th>&nbsp;</th>
        </tr>
        {foreach from=$numbers item=number}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$number.name|escape:"html"}</td>
            <td>{$number.phone_number|escape:"html"}</td>
            {if $app_ar_exposed}<td>{if $number.ar_enabled}Enabled{else}Disabled{/if}</td>{/if}
            <td>
                {if not $number.is_blacklisted}
                    <input type="submit" name="edit_number[{$number.as_external_numbers_id}]" value="Edit" />
                {else}
                    <input type="button" name="blacklist" value="Blacklisted" disabled="disabled" />
                {/if}
                <input type="submit" name="delete_number[{$number.as_external_numbers_id}]" value="Delete" />
            </td>
        </tr>
        {/foreach}
    </table>
    <input type="hidden" name="id" value="{$id}" />
    {if not $reached_numbers_limit}
        <input type="submit" name="add_number" value="Add Number" />
    {/if}
    </form>
{/if}

{if $app_ar_exposed}
    <form action="{$SCRIPT_NAME}" method="post">
    <strong>Voice Mail Box</strong>:
    <select name="corporate_number_id">
        <option value="0">None</option>
    {foreach from=$numbers item=number}
        <option value="{$number.as_external_numbers_id}" {if $number.as_external_numbers_id == $corporate_number_id}selected="selected"{/if}>{if $number.name}{$number.name|escape:"html"}{else}{$number.phone_number|escape:"html"}{/if}</option>
    {/foreach}
    </select>
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="set_corporate" value="Set" />
    </form>
{/if}


{if $app_ar_exposed}
    <h3>Single Reach Numbers</h3>
    <form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <table style="width:25em;">
        <col />
        <col />
        <tr>
            <th colspan="2">Number</th>
        </tr>
        {foreach from=$sr_numbers item=number}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$number.number|escape:"html"}</td>
            <td><input type="submit" name="delete_sr[{$number.as_single_reach_numbers_id}]" value="Delete" /></td>
        </tr>
        {/foreach}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td><input type="text" name="new_sr" value="" /></td>
            <td><input type="submit" name="add_sr" value="Add" /></td>
        </tr>
    </table>    
    </form>
{/if}


{if $app_ar_exposed}
    <h3>ActiveRelay Filters</h3>
    <form action="{$SCRIPT_NAME}" method="post">
        <input type="hidden" name="id" value="{$id}" />
        
        <input type="submit" name="whitelist" value="Manage Whitelist" style="width:15em; margin-right:1em;" />
        A list of Caller IDs which will cause ActiveRelay to always activate.
        <br />
        <input type="submit" name="blacklist" value="Manage Blacklist" style="width:15em; margin-right:1em;" />
        A list of Caller IDs which will cause ActiveRelay to not activate.
    </form>
{/if}

{if $group_admin_access || $admin_access || $remote_agent_enabled && $app_ra_exposed}
    <h3>Remote Agent</h3>
    <form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    {if $remote_agent_enabled}
        <table>
            <col class="inputLabels" />
            <col class="inputFields" />
            <col class="inputDescription" />
            {if $group_admin_access || $admin_access}
            <tr class="{cycle values='rowOne,rowTwo'}">
                <th>User Level</th>
                <td>
                <select name="ra_user_level">
                    {foreach from=$ral_options key=ral_key item=ral_name}
                    <option value="{$ral_key}" {if $ra_user_level == $ral_key}selected="selected"{/if}>{$ral_name|escape:"html"}</option>
                    {/foreach}
                </select>
                </td>
                <td>User's access level to Remote Agent</td>
            </tr>
            {else}
            <input type="hidden" name="ra_user_level" value="{$ra_user_level}" />
            {/if}
            <tr class="{cycle values='rowOne,rowTwo'}">
                <th>Remote Agent Device</th>
                <td>
                <select name="ra_as_phone_devices_id">
                    <option value="">None Assigned</option>
                    {foreach from=$ra_devices item=device}
                    <option value="{$device.as_phone_devices_id}" {if $ra_as_phone_devices_id == $device.as_phone_devices_id}selected="selected"{/if}>{$device.name|escape:"html"}</option>
                    {/foreach}
                </select>
                </td>
                <td>Device whose primary directory number will be used for Remote Agent</td>
            </tr>
            <tr class="{cycle values='rowOne,rowTwo'}">
                <th>Remote Agent Find Me Number</th>
                <td>
                <select name="ra_as_external_numbers_id">
                    <option value="">None Assigned</option>
                    {foreach from=$ra_numbers item=number}
                    <option value="{$number.as_external_numbers_id}" {if $ra_as_external_numbers_id == $number.as_external_numbers_id}selected="selected"{/if}>{$number.name|escape:"html"}</option>
                    {/foreach}
                </select>
                </td>
                <td>Find Me number will be used for Remote Agent</td>
            </tr>
        </table>
        <input type="submit" name="update_remote_agent" value="Apply" />
        {if $group_admin_access || $admin_access}
        <input type="submit" name="disable_remote_agent" value="Disable Remote Agent for this User" />
        {/if}
    {else}
        <input type="submit" name="enable_remote_agent" value="Enable Remote Agent for this User" />
    {/if}
    </form>
{/if}