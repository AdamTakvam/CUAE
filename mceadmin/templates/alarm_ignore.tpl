<p>Below, you can select which alarms you want to ignore and not be notified when they are triggered.</p>

<form action="{$SCRIPT_NAME}" method="post">

	<p>
	<input type="submit" name="submit" value="Update" />
	<input type="submit" name="done" value="Done" />
	</p>
	
	<table>
	<caption>General Alarms</caption>
	<col style="width: 4em;" />
	<col style="width: 4em;" />
	<col />
	<tr><th>Ignore</th><th>OID</th><th>Description</th></tr>
	{foreach from=$alarms_gen item=alarm}
		<tr class="{cycle values='evenRow,oddRow'}">
			<td><input type="checkbox" name="ignore[]" value="{$alarm.oid}" {if $alarm.ignore}checked="checked"{/if} /></td>
			<td>{$alarm.oid}</td>
			<td>{$alarm.description}</td>
		</tr>
	{/foreach}
	</table>
	
	
	<table>
	<caption>Media Engine Alarms</caption>
	<col style="width: 4em;" />
	<col style="width: 4em;" />
	<col />
	<tr><th>Ignore</th><th>OID</th><th>Description</th></tr>
	{foreach from=$alarms_media item=alarm}
		<tr class="{cycle values='evenRow,oddRow'}">
			<td><input type="checkbox" name="ignore[]" value="{$alarm.oid}" {if $alarm.ignore}checked="checked"{/if} /></td>
			<td>{$alarm.oid}</td>
			<td>{$alarm.description}</td>
		</tr>
	{/foreach}
	</table>
	
	
	<table>
	<caption>Application Server Alarms</caption>
	<col style="width: 4em;" />
	<col style="width: 4em;" />
	<col />
	<tr><th>Ignore</th><th>OID</th><th>Description</th></tr>
	{foreach from=$alarms_as item=alarm}
		<tr class="{cycle values='evenRow,oddRow'}">
			<td><input type="checkbox" name="ignore[]" value="{$alarm.oid}" {if $alarm.ignore}checked="checked"{/if} /></td>
			<td>{$alarm.oid}</td>
			<td>{$alarm.description}</td>
		</tr>
	{/foreach}
	</table>
	
	
	<table>
	<caption>Licensing Alarms</caption>
	<col style="width: 4em;" />
	<col style="width: 4em;" />
	<col />
	<tr><th>Ignore</th><th>OID</th><th>Description</th></tr>
	{foreach from=$alarms_lic item=alarm}
		<tr class="{cycle values='evenRow,oddRow'}">
			<td><input type="checkbox" name="ignore[]" value="{$alarm.oid}" {if $alarm.ignore}checked="checked"{/if} /></td>
			<td>{$alarm.oid}</td>
			<td>{$alarm.description}</td>
		</tr>
	{/foreach}
	</table>
	
	<p>
	<input type="submit" name="submit" value="Update" />
	<input type="submit" name="done" value="Done" />
	</p>
	
</form>