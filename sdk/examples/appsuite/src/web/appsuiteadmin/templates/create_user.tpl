<script type="text/javascript" language="javascript">
<!--
{literal}
function toggle_ac_fields(toggle) 
{
    document.create_user.account_code.disabled = toggle;
    document.create_user.pin.disabled = toggle;
    document.create_user.pin_verify.disabled = toggle;
    
    if (toggle)
    {
        document.create_user.account_code.style.backgroundColor = '#ccc';
        document.create_user.pin.style.backgroundColor = '#ccc';
        document.create_user.pin_verify.style.backgroundColor = '#ccc';
    }
    else
    {
        document.create_user.account_code.style.backgroundColor = '#fff';
        document.create_user.pin.style.backgroundColor = '#fff';
        document.create_user.pin_verify.style.backgroundColor = '#fff';    
    }
}
{/literal}
//-->
</script>

<form action="{$SCRIPT_NAME}" method="post" name="create_user">
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
		<td><input type="text" name="username" value="{$username|escape:"html"}" /></td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Password</th>
		<td><input type="password" name="password" /></td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Verify Password</th>
		<td><input type="password" name="password_verify" /></td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Account Code</th>
		<td>
            <input type="text" name="account_code" value="{$account_code}" />
            <input type="checkbox" name="generate_account_code" value="1" onclick="javascript:toggle_ac_fields(this.checked);" /> Generate Account Code &amp; PIN
        </td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>PIN</th>
		<td><input type="password" name="pin" value="{$pin}" /></td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Verify PIN</th>
		<td><input type="password" name="pin_verify" value="" /></td>
	</tr>
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
        <th>Time Zone</th>
        <td>
            {html_options values=$timezone_list output=$timezone_list name="time_zone" selected=$time_zone}
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
		<th>PIN Change</th>
		<td>
			<input type="radio" name="pin_change_required" value="1" {if $pin_change_required == 1}checked="checked"{/if}/> Required
			<input type="radio" name="pin_change_required" value="0" {if $pin_change_required == 0}checked="checked"{/if}/> Not Required
		</td>
	</tr>
    <!--
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>External Authentication</th>
		<td>
			<input type="radio" name="external_auth_enabled" value="1" {if $external_auth_enabled == 1}checked="checked"{/if}/> Enabled
			<input type="radio" name="external_auth_enabled" value="0" {if $external_auth_enabled == 0}checked="checked"{/if}/> Not Enabled
		</td>
	</tr>
    -->
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
<input type="submit" name="submit" value="Create User" />
<input type="submit" name="cancel" value="Cancel" />
</form>