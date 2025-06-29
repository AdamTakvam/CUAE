<p>
The following lists all of the groups used for Metreos Intercom.
</p>

<p>{include file="page_nav.tpl"}</p>

<form action="{$SCRIPT_NAME}" method="post">
<table>
	<tr>
		<th>Name</th>
		<th>Status</th>
		<th>Talkback Enabled</th>
		<th>Type</th>
		<th>&nbsp;</th>
	</tr>
	{foreach from=$groups item=group}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td>{$group.name|escape:"html"}</td>
		<td>
			{if $group.is_enabled}
				Enabled
			{else}
				Disabled
			{/if}
		</td>
		<td>
			{if $group.is_talkback_enabled}
			Yes
			{else}
			No
			{/if}
		</td>
		<td>
			{if $group.is_private}
				Private
			{else}
				Public
			{/if}
		</td>
		<td><input type="submit" name="edit[{$group.as_intercom_groups_id}]" value="Edit" /></td>
	</tr>
	{/foreach}
</table>
<input type="submit" name="create" value="Create Group" />
</form>