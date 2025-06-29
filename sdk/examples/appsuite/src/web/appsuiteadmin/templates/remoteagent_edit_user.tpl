<h3>Remote Agent</h3>
<form action="{$SCRIPT_NAME}" method="post">
<input type="hidden" name="id" value="{$id}" />

	<table>
		<col class="inputLabels" />
		<col class="inputFields" />	
		<col class="inputDescription" />
		<tr class="{cycle values='rowOne,rowTwo'}">
			<th>Remote Agent Device</th>
			<td>
			<select name="ra_as_phone_devices_id">
				<option value="">None Assigned</option>
				{foreach from=$ra_devices item=device}
				<option value="{$device.as_phone_devices_id}" {if $ra_as_phone_devices_id == $device.as_phone_devices_id}selected="selected"{/if}>{$device.name}</option>
				{/foreach}
			</select>
			</td>
			<td>Device whose primary directory number will be used for Remote Agent</td>
		</tr>
		<tr class="{cycle values='rowOne,rowTwo'}">
			<th>Remote Agent External Number</th>
			<td>
			<select name="ra_as_external_numbers_id">
				<option value="">None Assigned</option>
				{foreach from=$ra_numbers item=number}
				<option value="{$number.as_external_numbers_id}" {if $ra_as_external_numbers_id == $number.as_external_numbers_id}selected="selected"{/if}>{$number.name}</option>
				{/foreach}
			</select>
			</td>
			<td>External number will be used for Remote Agent</td>
		</tr>		
	</table>
	<input type="submit" name="update_remote_agent" value="Apply" />
</form>


<h3>Devices</h3>
<span style="font-size: .8em"><img src="/appsuiteadmin/images/reddot.gif" width="11" height="11" alt="Red dot" /> = Primary Device</span>
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
        	{if $device.is_primary_device}<img src="/appsuiteadmin/images/reddot.gif" width="11" height="11" alt="Red dot indicating primary device" />{/if}
    	</td>
        <td>{$device.name}</td>
        <td>{$device.mac_address}</td>
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
</form>



<h3>Find Me Numbers</h3>
<form action="{$SCRIPT_NAME}" method="post">
<table>
    <tr>
        <th>Name</th>
        <th>Phone Number</th>
        <th>Active Relay</th>
        <th>&nbsp;</th>
    </tr>
    {foreach from=$numbers item=number}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>{$number.name}</td>
        <td>{$number.phone_number}</td>
        <td>{if $number.ar_enabled}Enabled{else}Disabled{/if}</td>
        <td>
            <input type="submit" name="edit_number[{$number.as_external_numbers_id}]" value="Edit" />
            <input type="submit" name="delete_number[{$number.as_external_numbers_id}]" value="Delete" />
        </td>
    </tr>
    {/foreach}
</table>
<input type="hidden" name="id" value="{$id}" />
<input type="submit" name="add_number" value="Add Number" />
</form>
