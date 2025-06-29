<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="rowOne">
        <th>Descriptive Name (Optional)</th>
        <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
        <td>Example: "My Work Phone"</td>
    </tr>
    <tr class="rowTwo">
        <th>IP Phone</th>
        <td><input type="checkbox" name="is_ip_phone" value="1" {if $is_ip_phone}checked="checked"{/if} onclick="javascript:this.form.mac_address.disabled=!this.checked;" /></td>
        <td>Check the box if this device is an IP phone</td>
    </tr>
    <tr class="rowOne">
        <th>Device Name</th>
        <td><input type="text" name="mac_address" value="{$mac_address}" {if not $is_ip_phone}disabled="disabled"{/if} /></td>
        <td>MAC Address or Device Name - Example: "1234ABCD1234" or "SEP123412341234"</td>
    </tr>
</table>
<input type="hidden" name="user_id" value="{$user_id}" />
{if $id > 0}
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="submit" value="Apply Device Settings" />
    <input type="submit" name="delete_device" value="Delete Device" />
{else}
    <input type="submit" name="submit" value="Create Device" />
{/if}
	<input type="submit" name="cancel" value="Go Back" />
</form>

{if $id > 0}
<h3>Line Numbers</h3>
<span style="font-size: .8em"><img src="/appsuiteadmin/images/reddot.gif" width="11" height="11" alt="Red dot" /> = Primary Number</span>
<form action="{$SCRIPT_NAME}" method="post">
<input type="hidden" name="user_id" value="{$user_id}" />
<input type="hidden" name="id" value="{$id}" />
<table>
	<col style="width:16px" />
	<col />
	<col style="width:200px" />	
	<tr>
		<th>&nbsp;</th>
		<th>Number</th>
		<th>&nbsp;</th>
	</tr>
	{foreach from=$numbers item=number}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td>{if $number.is_primary_number}<img src="/appsuiteadmin/images/reddot.gif" width="11" height="11" alt="Red dot indicating primary number" />{/if}</td>
		<td>{$number.directory_number|escape:"html"}</td>
		<td>
            <input type="submit" name="set_primary_number[{$number.as_directory_numbers_id}]" value="Set Primary" />
            <input type="submit" name="delete_number[{$number.as_directory_numbers_id}]" value="Delete" />		
		</td>
	</tr>
	{/foreach}
</table>
</form>
<form action="{$SCRIPT_NAME}" method="post">
	<input type="hidden" name="user_id" value="{$user_id}" />
    <input type="hidden" name="id" value="{$id}" />
	<input type="text" name="number" value="" /> <input type="submit" name="add_number" value="Add Number" />
</form>
{/if}