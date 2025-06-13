{literal}
<script type="text/javascript">
<!--
function SetAllCheckBoxes(FormName, FieldName, CheckValue)
{
	if(!document.forms[FormName])
		return;
	var objCheckBoxes = document.forms[FormName].elements[FieldName];
	if(!objCheckBoxes)
		return;
	var countCheckBoxes = objCheckBoxes.length;
	if(!countCheckBoxes)
		objCheckBoxes.checked = CheckValue;
	else
		// set the check value for all check boxes
		for(var i = 0; i < countCheckBoxes; i++)
            if(!objCheckBoxes[i].disabled)
                objCheckBoxes[i].checked = CheckValue;}
// -->
</script>
{/literal}

<h3>Alarm Managers</h3>
<table style="width: auto">
{foreach from=$alarms item=alarm}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><a href="edit_alarm.php?id={$alarm.mce_components_id}&amp;type={$alarm.type}">{$alarm.display_type|escape:"html"}</a></td>
    </tr>
{foreachelse}
	<tr><td>There are no alarm managers configured.</td></tr>
{/foreach}
</table>


{if $alarm_types}
<p>
<form method="post" action="{$SCRIPT_NAME}">
	Configure an alarm 
	<select name="type">
	{foreach from=$alarm_types item=description key=type}
		<option value="{$type}">{$description}</option>
	{/foreach}
	</select>
	<input type="submit" name="add_alarm" value="Create" />
</form>
</p>
{/if}


{if $admin_access}
<h3>Active Alarms</h3>
<form method="post" action="{$SCRIPT_NAME}" name="activeAlarms">
<table>
    <tr>
        <th>&nbsp;</th>
        <th>Time Occurred</th>
        <th>Message ID</th>
        <th>Message</th>
        <th>Details</th>
        <th>Severity</th>
        <th>Status</th>
    </tr>
    {if $active_alarms}
    <tr style="background-color: #ccc;">
        <td colspan="7">
        <input type="button" value="Select All" onclick="javascript:SetAllCheckBoxes('activeAlarms','selected[]',true);" />
        <input type="button" value="Select None" onclick="javascript:SetAllCheckBoxes('activeAlarms','selected[]',false);" />
        </td>
    </tr>
    {/if}
    {foreach from=$active_alarms item=alarm}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><input type="checkbox" name="selected[]" value="{$alarm.mce_event_log_id}" /></td>
        <td>{$alarm.created_timestamp|date_format:"%D %I:%M:%S %p"}</td>
        <td>{$alarm.message_id}</td>
        <td>{$alarm.message}</td>
        <td style="font-size:x-small;">{$alarm.data}</td>
        <td><strong>{$alarm.display_severity|upper}</strong></td>
        <td class="{if $alarm.is_open}openAlarm{elseif $alarm.is_acknowledged}ackAlarm{/if}">{$alarm.display_status}</td>
    </tr> 
    {foreachelse}
    <tr>
        <td colspan="7">There are currently no active alarms.</td>
    </tr>
    {/foreach}
    {if $active_alarms}
    <tr style="background-color: #ccc;"><td colspan="7">
        <input type="submit" name="ack" value="Set Acknowledged" />
        <input type="submit" name="resolve" value="Set Resolved" />
    </td></tr>
    {/if}
</table>
</form>

<p>
	<a href="alarm_ignore.php" class="button">Set Ignored Alarms</a>
	<a href="{$SCRIPT_NAME}?download_mib=1" class="button">Download MIB File</a>
</p>
{/if}